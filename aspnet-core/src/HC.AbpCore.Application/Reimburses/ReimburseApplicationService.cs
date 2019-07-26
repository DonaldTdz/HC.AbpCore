
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


using HC.AbpCore.Reimburses;
using HC.AbpCore.Reimburses.Dtos;
using HC.AbpCore.Reimburses.DomainService;
using HC.AbpCore.Projects;
using HC.AbpCore.DingTalk.Employees;
using Abp.Auditing;
using HC.AbpCore.Dtos;
using HC.AbpCore.DingTalk;
using Microsoft.AspNetCore.Http;
using Abp.Runtime.Session;
using HC.AbpCore.Authorization.Users;
using HC.AbpCore.Reimburses.ReimburseDetails;

namespace HC.AbpCore.Reimburses
{
    /// <summary>
    /// Reimburse应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize]
    public class ReimburseAppService : AbpCoreAppServiceBase, IReimburseAppService
    {
        private readonly IRepository<Reimburse, Guid> _entityRepository;
        private readonly IRepository<ReimburseDetail, Guid> _reimburseDetailRepository;
        private readonly IRepository<Project, Guid> _projectRepository;
        private readonly IRepository<Employee, string> _employeeRepository;
        private readonly IReimburseManager _entityManager;
        private readonly UserManager _userManager;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public ReimburseAppService(
        IRepository<Reimburse, Guid> entityRepository
        , IRepository<Project, Guid> projectRepository
        , IRepository<Employee, string> employeeRepository
        , IRepository<ReimburseDetail, Guid> reimburseDetailRepository
        , IReimburseManager entityManager
        , UserManager userManager
        )
        {
            _reimburseDetailRepository = reimburseDetailRepository;
            _userManager = userManager;
            _entityRepository = entityRepository;
            _entityManager = entityManager;
            _projectRepository = projectRepository;
            _employeeRepository = employeeRepository;
        }


        /// <summary>
        /// 获取Reimburse的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        [Audited]
        public async Task<PagedResultDto<ReimburseListDto>> GetPagedAsync(GetReimbursesInput input)
        {
            var query = _entityRepository.GetAll().WhereIf(input.ProjectId.HasValue, aa => aa.ProjectId == input.ProjectId.Value)
                .WhereIf(input.Status.HasValue, aa => aa.Status == input.Status.Value)
                .WhereIf(input.type.HasValue, aa => aa.Type == input.type.Value);
            // TODO:根据传入的参数添加过滤条件
            //if (!String.IsNullOrEmpty(input.EmployeeId))
            //{
            //    query = query.Where(aa => aa.EmployeeId == input.EmployeeId);
            //}
            //else
            //{
            //    var user = await _userManager.GetUserByIdAsync(AbpSession.UserId.Value);
            //    if (user.EmployeeId != "0205151055692871" && user.EmployeeId != "1706561401635019335")  //如果是代姐或吴总则可以查看全部
            //        query = query.Where(aa => aa.EmployeeId == user.EmployeeId);
            //}
            var user = await _userManager.GetUserByIdAsync(AbpSession.UserId.Value);
            if (user != null &&( user.EmployeeId != "0205151055692871" && user.EmployeeId != "1706561401635019335"))//如果是代姐或吴总则可以查看全部
            {
                query = query.Where(aa => aa.EmployeeId == user.EmployeeId);
            }
            else
            {
                if (!String.IsNullOrEmpty(input.EmployeeId))
                {
                    query = query.Where(aa => aa.EmployeeId == input.EmployeeId);
                }
            }


            var projects = _projectRepository.GetAll();
            var employees = _employeeRepository.GetAll();
            //var count = await query.CountAsync();
            var entityList = from item in query
                             join project in projects on item.ProjectId equals project.Id into projectName
                             join employee in employees on item.EmployeeId equals employee.Id into employeeName
                             //join approver in employees on item.ApproverId equals approver.Id into approverName
                             from bb in employeeName.DefaultIfEmpty()
                             from cc in projectName.DefaultIfEmpty()
                                 //from cc in approverName.DefaultIfEmpty()
                             select new ReimburseListDto()
                             {
                                 Id = item.Id,
                                 ProjectId = item.ProjectId,
                                 ProjectName = cc.Name + "(" + cc.ProjectCode + ")",
                                 EmployeeId = item.EmployeeId,
                                 EmployeeName = bb.Name,
                                 Amount = item.Amount,
                                 Status = item.Status,
                                 SubmitDate = item.SubmitDate,
                                 Type = item.Type,
                                 ApproverId = item.ApproverId,
                                 ApprovalTime = item.ApprovalTime,
                                 //ApproverName = cc.Name,
                                 CancelTime = item.CancelTime,
                                 CreationTime = item.CreationTime
                             };

            var count = await entityList.CountAsync();

            var items = await entityList
                .OrderByDescending(aa => aa.SubmitDate)
                .OrderBy(aa => aa.Status)
                .PageBy(input)
                .AsNoTracking()
                .ToListAsync();
            return new PagedResultDto<ReimburseListDto>(count, items);
        }


        /// <summary>
        /// 通过指定id获取ReimburseListDto信息
        /// </summary>
        [AbpAllowAnonymous]
        [Audited]
        public async Task<ReimburseListDto> GetByIdAsync(EntityDto<Guid> input)
        {
            var entity = await _entityRepository.GetAsync(input.Id);
            var item = entity.MapTo<ReimburseListDto>();
            if (item.ProjectId.HasValue)
            {
                var project = await _projectRepository.FirstOrDefaultAsync(aa => aa.Id == item.ProjectId.Value);
                item.ProjectName = project.Name + "(" + project.ProjectCode + ")";
            }
            if (!String.IsNullOrEmpty(item.EmployeeId))
                item.EmployeeName = (await _employeeRepository.FirstOrDefaultAsync(aa => aa.Id == item.EmployeeId)).Name;
            if (!String.IsNullOrEmpty(item.ApproverId))
            {
                var approverIds = item.ApproverId.Split(",");
                var employeesList = await _employeeRepository.GetAll().Where(aa => approverIds.Contains(aa.Id)).Select(aa => aa.Name).AsNoTracking().ToListAsync();
                item.ApproverName = String.Join(",", employeesList);
            }
            //item.ApproverName = (await _employeeRepository.FirstOrDefaultAsync(aa => aa.Id == item.ApproverId)).Name;
            return item;
        }

        /// <summary>
        /// 获取编辑 Reimburse
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task<GetReimburseForEditOutput> GetForEditAsync(NullableIdDto<Guid> input)
        {
            var output = new GetReimburseForEditOutput();
            ReimburseEditDto editDto;

            if (input.Id.HasValue)
            {
                var entity = await _entityRepository.GetAsync(input.Id.Value);

                editDto = entity.MapTo<ReimburseEditDto>();

                //reimburseEditDto = ObjectMapper.Map<List<reimburseEditDto>>(entity);
            }
            else
            {
                editDto = new ReimburseEditDto();
            }

            output.Reimburse = editDto;
            return output;
        }


        /// <summary>
        /// 添加或者修改Reimburse的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        [Audited]
        public async Task<ReimburseEditDto> CreateOrUpdateAsync(CreateOrUpdateReimburseInput input)
        {

            if (input.Reimburse.Id.HasValue)
            {
                return await UpdateAsync(input.Reimburse);
            }
            else
            {
                return await CreateAsync(input.Reimburse);
            }
        }


        /// <summary>
        /// 新增Reimburse
        /// </summary>

        protected virtual async Task<ReimburseEditDto> CreateAsync(ReimburseEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增
            var entity = input.MapTo<Reimburse>();

            entity.Status = ReimburseStatusEnum.提交; //新增报销默认为提交状态

            //如果报销人为空则为当前登录人
            //if (String.IsNullOrEmpty(entity.EmployeeId))
            //{
            //    var user = await _userManager.GetUserByIdAsync(AbpSession.UserId.Value);
            //    entity.EmployeeId = user.EmployeeId;
            //}
            //entity.SubmitDate = DateTime.Now;
            entity = await _entityRepository.InsertAsync(entity);
            return entity.MapTo<ReimburseEditDto>();
        }

        /// <summary>
        /// 编辑Reimburse
        /// </summary>

        protected virtual async Task<ReimburseEditDto> UpdateAsync(ReimburseEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新

            var entity = await _entityRepository.GetAsync(input.Id.Value);
            input.MapTo(entity);

            // ObjectMapper.Map(input, entity);
            entity = await _entityRepository.UpdateAsync(entity);
            return entity.MapTo<ReimburseEditDto>();
        }



        /// <summary>
        /// 删除Reimburse信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        [Audited]
        public async Task DeleteAsync(EntityDto<Guid> input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _entityRepository.DeleteAsync(input.Id);
        }



        /// <summary>
        /// 批量删除Reimburse的方法
        /// </summary>

        public async Task BatchDeleteAsync(List<Guid> input)
        {
            // TODO:批量删除前的逻辑判断，是否允许删除
            await _entityRepository.DeleteAsync(s => input.Contains(s.Id));
        }


        /// <summary>
        /// 提交审批(并保存报销与报销详情)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        [Audited]
        public async Task<APIResultDto> SubmitApprovalAsync(CreateReimburseAndDetailInput input)
        {
            //如果报销人为空则为当前登录人
            if (String.IsNullOrEmpty(input.Reimburse.EmployeeId))
            {
                var user = await _userManager.GetUserByIdAsync(AbpSession.UserId.Value);
                input.Reimburse.EmployeeId = user.EmployeeId;
            }
            input.Reimburse.SubmitDate = DateTime.Now;
            List<ReimburseDetail> reimburseDetails = input.ReimburseDetails.MapTo<List<ReimburseDetail>>();
            Reimburse reimburse = input.Reimburse.MapTo<Reimburse>();

            var apiResult = await _entityManager.SubmitApprovalAsync(reimburse, reimburseDetails);
            if (apiResult.Code == 0)
            {
                input.Reimburse.ProcessInstanceId = apiResult.Data.ToString();
                ReimburseEditDto reimburseModel = await CreateAsync(input.Reimburse);  //保存报销
                //List<ReimburseDetail> reimburseDetails = new List<ReimburseDetail>();

                foreach (var item in input.ReimburseDetails)
                {
                    var entity = item.MapTo<ReimburseDetail>();
                    entity.ReimburseId = reimburseModel.Id.Value;
                    entity = await _reimburseDetailRepository.InsertAsync(entity);
                    //reimburseDetails.Add(entity);
                }
                return apiResult.MapTo<APIResultDto>();
            }
            else
            {
                return apiResult.MapTo<APIResultDto>();
            }

        }


        /// <summary>
        /// 导出Reimburse为excel表,等待开发。
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


