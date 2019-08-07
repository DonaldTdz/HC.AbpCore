
using System;
using System.Data;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

using Abp.UI;
using Abp.AutoMapper;
using Abp.Extensions;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Application.Services.Dto;
using Abp.Linq.Extensions;


using HC.AbpCore.TimeSheets;
using HC.AbpCore.TimeSheets.Dtos;
using HC.AbpCore.TimeSheets.DomainService;
using HC.AbpCore.Projects;
using HC.AbpCore.DingTalk.Employees;
using HC.AbpCore.Dtos;
using Abp.Auditing;
using Abp.Runtime.Session;
using HC.AbpCore.Messages.DomainService;
using HC.AbpCore.Authorization.Users;

namespace HC.AbpCore.TimeSheets
{
    /// <summary>
    /// TimeSheet应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize]
    public class TimeSheetAppService : AbpCoreAppServiceBase, ITimeSheetAppService
    {
        private readonly UserManager _userManager;
        private readonly IRepository<TimeSheet, Guid> _entityRepository;
        private readonly IRepository<Project, Guid> _projectRepository;
        private readonly IRepository<Employee, string> _employeeRepository;
        private readonly IMessageManager _messageManager;
        private readonly ITimeSheetManager _entityManager;


        /// <summary>
        /// 构造函数 
        ///</summary>
        public TimeSheetAppService(
        IRepository<TimeSheet, Guid> entityRepository
        , IRepository<Project, Guid> projectRepository
        , IRepository<Employee, string> employeeRepository
        , IMessageManager messageManager
        , ITimeSheetManager entityManager
        , UserManager userManager
        )
        {
            _userManager = userManager;
            _messageManager = messageManager;
            _entityRepository = entityRepository;
            _entityManager = entityManager;
            _projectRepository = projectRepository;
            _employeeRepository = employeeRepository;
        }


        /// <summary>
        /// 获取TimeSheet的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        [Audited]
        public async Task<PagedResultDto<TimeSheetListDto>> GetPagedAsync(GetTimeSheetsInput input)
        {

            var query = _entityRepository.GetAll().WhereIf(input.ProjectId.HasValue, aa => aa.ProjectId == input.ProjectId.Value)
               .WhereIf(input.Status.HasValue, aa => aa.Status == input.Status.Value);
            //  .WhereIf(!String.IsNullOrEmpty(input.EmployeeId), aa => aa.EmployeeId == input.EmployeeId)
            // TODO:根据传入的参数添加过滤条件
            if (String.IsNullOrEmpty(input.EmployeeId))
            {
                User user = new User() { Id = AbpSession.UserId.Value };
                var roles = await _userManager.GetRolesAsync(user);
                if (!roles.Contains("Admin") && !roles.Contains("Finance") && !roles.Contains("GeneralManager"))
                {
                    user = await _userManager.GetUserByIdAsync(AbpSession.UserId.Value);
                    query = query.Where(aa => aa.EmployeeId == user.EmployeeId);
                }
            }
            else
            {
                query = query.Where(aa => aa.EmployeeId == input.EmployeeId);
            }
            //var counts = await query.CountAsync();

            var projects = _projectRepository.GetAll();
            var employees = _employeeRepository.GetAll();

            var entityList = from item in query
                             join project in projects on item.ProjectId equals project.Id
                             join employee in employees on item.EmployeeId equals employee.Id into employeeName
                             from bb in employeeName.DefaultIfEmpty()
                             select new TimeSheetListDto()
                             {
                                 Id = item.Id,
                                 ProjectId = item.ProjectId,
                                 ProjectName = project.Name + "(" + project.ProjectCode + ")",
                                 EmployeeId = item.EmployeeId,
                                 EmployeeName = bb.Name,
                                 WorkeDate = item.WorkeDate,
                                 Status = item.Status,
                                 Hour = item.Hour,
                                 ApproverId = item.ApproverId,
                                 ApprovalTime = item.ApprovalTime,
                                 Content = item.Content,
                                 CreationTime = item.CreationTime
                             };


            var count = await entityList.CountAsync();
            var items = await entityList
                .OrderByDescending(aa => aa.WorkeDate)
                .OrderBy(aa => aa.Status)
                .PageBy(input)
                .AsNoTracking()
                .ToListAsync();

            return new PagedResultDto<TimeSheetListDto>(count, items);
        }


        /// <summary>
        /// 通过指定id获取TimeSheetListDto信息
        /// </summary>
        [AbpAllowAnonymous]
        [Audited]
        public async Task<TimeSheetListDto> GetByIdAsync(EntityDto<Guid> input)
        {
            var entity = await _entityRepository.GetAsync(input.Id);

            var item = entity.MapTo<TimeSheetListDto>();

            var project = await _projectRepository.GetAsync(item.ProjectId);
            item.ProjectName = project.Name + "(" + project.ProjectCode + ")";
            if (!String.IsNullOrEmpty(item.ApproverId))
                item.ApproverName = (await _employeeRepository.GetAsync(item.ApproverId)).Name;
            return item;
        }

        /// <summary>
        /// 获取编辑 TimeSheet
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task<GetTimeSheetForEditOutput> GetForEditAsync(NullableIdDto<Guid> input)
        {
            var output = new GetTimeSheetForEditOutput();
            TimeSheetEditDto editDto;

            if (input.Id.HasValue)
            {
                var entity = await _entityRepository.GetAsync(input.Id.Value);

                editDto = entity.MapTo<TimeSheetEditDto>();

                //timeSheetEditDto = ObjectMapper.Map<List<timeSheetEditDto>>(entity);
            }
            else
            {
                editDto = new TimeSheetEditDto();
            }

            output.TimeSheet = editDto;
            return output;
        }


        /// <summary>
        /// 添加或者修改TimeSheet的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        
        [AbpAllowAnonymous]
        [Audited]
        public async Task CreateOrUpdateAsync(CreateOrUpdateTimeSheetInput input)
        {

            if (input.TimeSheet.Id.HasValue)
            {
                await UpdateAsync(input.TimeSheet);
            }
            else
            {
                await CreateAsync(input.TimeSheet);
            }
        }


        /// <summary>
        /// 新增TimeSheet
        /// </summary>

        protected virtual async Task<TimeSheetEditDto> CreateAsync(TimeSheetEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增
            // var entity = ObjectMapper.Map <TimeSheet>(input);
            var entity = input.MapTo<TimeSheet>();


            entity = await _entityRepository.InsertAsync(entity);
            return entity.MapTo<TimeSheetEditDto>();
        }

        /// <summary>
        /// 编辑TimeSheet
        /// </summary>

        protected virtual async Task UpdateAsync(TimeSheetEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新

            var entity = await _entityRepository.GetAsync(input.Id.Value);
            input.MapTo(entity);

            // ObjectMapper.Map(input, entity);
            await _entityRepository.UpdateAsync(entity);
        }



        /// <summary>
        /// 删除TimeSheet信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task DeleteAsync(EntityDto<Guid> input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _entityRepository.DeleteAsync(input.Id);
        }



        /// <summary>
        /// 批量删除TimeSheet的方法
        /// </summary>

        public async Task BatchDeleteAsync(List<Guid> input)
        {
            // TODO:批量删除前的逻辑判断，是否允许删除
            await _entityRepository.DeleteAsync(s => input.Contains(s.Id));
        }



        /// <summary>
        /// 提交审批
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        [Audited]
        public async Task<APIResultDto> SubmitApproval(CreateOrUpdateTimeSheetInput input)
        {
            if (input.messageId.HasValue)
                await _messageManager.ModifyDoReadById(input.messageId.Value);
            var item = input.TimeSheet.MapTo<TimeSheet>();
            //var reimburse = await CreateAsync(input.TimeSheet);
            var apiResult = await _entityManager.SubmitApproval(item);
            return apiResult.MapTo<APIResultDto>();
        }


        /// <summary>
        /// 导出TimeSheet为excel表,等待开发。
        /// </summary>
        /// <returns></returns>
        //public async Task<FileDto> GetToExcel()
        //{
        //	var users = await UserManager.Users.ToListAsync();
        //	var userListDtos = ObjectMapper.Map<List<UserListDto>>(users);
        //	await FillRoleNames(userListDtos);
        //	return _userListExcelExporter.ExportToFile(userListDtos);
        //}

    }
}


