
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


using HC.AbpCore.Tasks;
using HC.AbpCore.Tasks.Dtos;
using HC.AbpCore.Tasks.DomainService;
using HC.AbpCore.Projects;
using HC.AbpCore.DingTalk.Employees;
using Abp.Auditing;
using HC.AbpCore.Tenders;
using HC.AbpCore.CompletedTasks;

namespace HC.AbpCore.Tasks
{
    /// <summary>
    /// CompletedTask应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize]
    public class CompletedTaskAppService : AbpCoreAppServiceBase, ICompletedTaskAppService
    {
        private readonly IRepository<CompletedTask, Guid> _entityRepository;
        private readonly IRepository<Project, Guid> _projectRepository;
        private readonly IRepository<Tender, Guid> _tenderRepository;
        private readonly IRepository<Employee, string> _employeeRepository;
        private readonly ICompletedTaskManager _entityManager;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public CompletedTaskAppService(
        IRepository<CompletedTask, Guid> entityRepository
        , IRepository<Project, Guid> projectRepository
        , IRepository<Tender, Guid> tenderRepository
        , IRepository<Employee, string> employeeRepository
        , ICompletedTaskManager entityManager
        )
        {
            _entityRepository = entityRepository;
            _entityManager = entityManager;
            _tenderRepository = tenderRepository;
            _projectRepository = projectRepository;
            _employeeRepository = employeeRepository;
        }


        /// <summary>
        /// 获取CompletedTask的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        [Audited]
        public async Task<PagedResultDto<CompletedTaskListDto>> GetPagedAsync(GetCompletedTasksInput input)
        {

            var query = _entityRepository.GetAll().WhereIf(!String.IsNullOrEmpty(input.EmployeeId), aa => aa.EmployeeId == input.EmployeeId);
            // TODO:根据传入的参数添加过滤条件
            var projects = _projectRepository.GetAll();
            //var employees = _employeeRepository.GetAll();
            var completedTaskList = from item in query
                                    join project in projects on item.ProjectId equals project.Id
                                    //join employee in employees on item.EmployeeId equals employee.Id
                                    select new CompletedTaskListDto()
                                    {
                                        Id = item.Id,
                                        ProjectId = item.ProjectId,
                                        ProjectName = project.Name + "(" + project.ProjectCode + ")",
                                        Content = item.Content,
                                        IsCompleted = item.IsCompleted,
                                        Status = item.Status,
                                        RefId = item.RefId,
                                        EmployeeId = item.EmployeeId,
                                        ClosingDate = item.ClosingDate,
                                        CreationTime = item.CreationTime
                                    };


            var count = await query.CountAsync();

            var entityList = await completedTaskList
                    .OrderByDescending(aa => aa.CreationTime).AsNoTracking()
                    .OrderBy(aa => aa.IsCompleted)
                    .PageBy(input)
                    .ToListAsync();

            // var entityListDtos = ObjectMapper.Map<List<CompletedTaskListDto>>(entityList);
            //var entityListDtos =entityList.MapTo<List<CompletedTaskListDto>>();

            return new PagedResultDto<CompletedTaskListDto>(count, entityList);
        }


        /// <summary>
        /// 通过指定id获取CompletedTaskListDto信息
        /// </summary>
        [AbpAllowAnonymous]
        [Audited]
        public async Task<CompletedTaskListDto> GetByIdAsync(EntityDto<Guid> input)
        {

            var entity = await _entityRepository.GetAsync(input.Id);


            var project = await _projectRepository.GetAsync(entity.ProjectId);
            var item= entity.MapTo<CompletedTaskListDto>();
            item.ProjectName = project.Name + "(" + project.ProjectCode + ")";
            return item;
        }

        /// <summary>
        /// 获取编辑 CompletedTask
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task<GetCompletedTaskForEditOutput> GetForEditAsync(NullableIdDto<Guid> input)
        {
            var output = new GetCompletedTaskForEditOutput();
            CompletedTaskEditDto editDto;

            if (input.Id.HasValue)
            {
                var entity = await _entityRepository.GetAsync(input.Id.Value);

                editDto = entity.MapTo<CompletedTaskEditDto>();

                //completedTaskEditDto = ObjectMapper.Map<List<completedTaskEditDto>>(entity);
            }
            else
            {
                editDto = new CompletedTaskEditDto();
            }

            output.CompletedTask = editDto;
            return output;
        }


        /// <summary>
        /// 添加或者修改CompletedTask的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        [Audited]
        public async Task CreateOrUpdateAsync(CreateOrUpdateCompletedTaskInput input)
        {

            if (input.CompletedTask.Id.HasValue)
            {
                await UpdateAsync(input.CompletedTask);
            }
            else
            {
                await CreateAsync(input.CompletedTask);
            }
        }


        /// <summary>
        /// 新增CompletedTask
        /// </summary>

        protected virtual async Task<CompletedTaskEditDto> CreateAsync(CompletedTaskEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增

            // var entity = ObjectMapper.Map <CompletedTask>(input);
            var entity = input.MapTo<CompletedTask>();


            entity = await _entityRepository.InsertAsync(entity);
            return entity.MapTo<CompletedTaskEditDto>();
        }

        /// <summary>
        /// 编辑CompletedTask
        /// </summary>

        protected virtual async Task UpdateAsync(CompletedTaskEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新
            var item = await _tenderRepository.GetAsync(input.RefId.Value);
            
            if (input.Status == TaskStatusEnum.招标保证金缴纳)
                item.IsPayBond = input.IsCompleted;
            if (input.Status == TaskStatusEnum.招标准备)
            {
                int incompleteCount = await _entityRepository.CountAsync(aa => aa.RefId == item.Id && aa.Status == TaskStatusEnum.招标准备 && aa.Id != input.Id&&aa.IsCompleted==false);
                if (incompleteCount == 0)
                {
                    item.IsReady = input.IsCompleted;
                }
            }
            await _tenderRepository.UpdateAsync(item);
            var entity = await _entityRepository.GetAsync(input.Id.Value);
            input.MapTo(entity);

            // ObjectMapper.Map(input, entity);
            await _entityRepository.UpdateAsync(entity);
        }



        /// <summary>
        /// 删除CompletedTask信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task DeleteAsync(EntityDto<Guid> input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _entityRepository.DeleteAsync(input.Id);
        }



        /// <summary>
        /// 批量删除CompletedTask的方法
        /// </summary>

        public async Task BatchDeleteAsync(List<Guid> input)
        {
            // TODO:批量删除前的逻辑判断，是否允许删除
            await _entityRepository.DeleteAsync(s => input.Contains(s.Id));
        }


        /// <summary>
        /// 导出CompletedTask为excel表,等待开发。
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


