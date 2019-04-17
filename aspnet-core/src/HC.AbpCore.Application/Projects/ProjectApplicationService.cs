
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


using HC.AbpCore.Projects;
using HC.AbpCore.Projects.Dtos;
using HC.AbpCore.Projects.DomainService;
using HC.AbpCore.Dtos;
using HC.AbpCore.Customers;
using HC.AbpCore.DingTalk.Employees;

namespace HC.AbpCore.Projects
{
    /// <summary>
    /// Project应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize]
    public class ProjectAppService : AbpCoreAppServiceBase, IProjectAppService
    {
        private readonly IRepository<Project, Guid> _entityRepository;
        private readonly IRepository<Customer, int> _customerRepository;
        private readonly IRepository<Employee, string> _employeeRepository;

        private readonly IProjectManager _entityManager;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public ProjectAppService(
        IRepository<Project, Guid> entityRepository,
        IRepository<Customer, int> customerRepository,
        IRepository<Employee, string> employeeRepository
        , IProjectManager entityManager
        )
        {
            _entityRepository = entityRepository;
            _entityManager = entityManager;
            _customerRepository = customerRepository;
            _employeeRepository = employeeRepository;
        }


        /// <summary>
        /// 获取Project的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task<PagedResultDto<ProjectListDto>> GetPagedAsync(GetProjectsInput input)
        {

            var query = _entityRepository.GetAll()
                .WhereIf(!String.IsNullOrEmpty(input.Name), a => a.Name.Contains(input.Name))
                .WhereIf(input.Status.HasValue, a => a.Status == input.Status.Value)
                .WhereIf(input.CustomerId.HasValue,a=>a.CustomerId==input.CustomerId.Value)
                .WhereIf(input.Id.HasValue,a=>a.Id==input.Id.Value);
            // TODO:根据传入的参数添加过滤条件

            var customerList = await _customerRepository.GetAll().AsNoTracking().ToListAsync();
            var employeeList = await _employeeRepository.GetAll().AsNoTracking().ToListAsync();

            var count = await query.CountAsync();

            var entityList = await query
                    .OrderBy(input.Sorting).AsNoTracking()
                    .PageBy(input)
                    .Select(aa => new ProjectListDto()
                    {
                        Id=aa.Id,
                        Mode = aa.Mode,
                        ProfitRatio = aa.ProfitRatio,
                        BillCost = aa.BillCost,
                        Type = aa.Type,
                        ProjectCode = aa.ProjectCode,
                        Name = aa.Name,
                        StartDate = aa.StartDate,
                        EndDate = aa.EndDate,
                        Year = aa.Year,
                        Budget = aa.Budget,
                        Status = aa.Status,
                        Desc = aa.Desc,
                        CustomerName = !aa.CustomerId.HasValue ? null : customerList.Where(bb => bb.Id == aa.CustomerId.Value).FirstOrDefault().Name,
                        EmployeeName = String.IsNullOrEmpty(aa.EmployeeId) ? null : employeeList.Where(bb => bb.Id == aa.EmployeeId).FirstOrDefault().Name,
                    })
                    .ToListAsync();

            // var entityListDtos = ObjectMapper.Map<List<ProjectListDto>>(entityList);
            //var entityListDtos =entityList.MapTo<List<ProjectListDto>>();

            return new PagedResultDto<ProjectListDto>(count, entityList);
        }


        /// <summary>
        /// 通过指定id获取ProjectListDto信息
        /// </summary>

        public async Task<ProjectListDto> GetByIdAsync(EntityDto<Guid> input)
        {
            var entity = await _entityRepository.GetAsync(input.Id);

            var projectListDto = entity.MapTo<ProjectListDto>();
            if (projectListDto.CustomerId.HasValue)
                projectListDto.CustomerName = (await _customerRepository.GetAsync(projectListDto.CustomerId.Value)).Name;
            if (!String.IsNullOrEmpty(projectListDto.EmployeeId))
                projectListDto.EmployeeName = (await _employeeRepository.GetAsync(projectListDto.EmployeeId)).Name;
            return projectListDto;
        }

        /// <summary>
        /// 获取编辑 Project
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task<GetProjectForEditOutput> GetForEditAsync(NullableIdDto<Guid> input)
        {
            var output = new GetProjectForEditOutput();
            ProjectEditDto editDto;

            if (input.Id.HasValue)
            {
                var entity = await _entityRepository.GetAsync(input.Id.Value);

                editDto = entity.MapTo<ProjectEditDto>();

                //projectEditDto = ObjectMapper.Map<List<projectEditDto>>(entity);
            }
            else
            {
                editDto = new ProjectEditDto();
            }

            output.Project = editDto;
            return output;
        }


        /// <summary>
        /// 添加或者修改Project的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task CreateOrUpdateAsync(CreateOrUpdateProjectInput input)
        {

            if (input.Project.Id.HasValue)
            {
                await UpdateAsync(input.Project);
            }
            else
            {
                await CreateAsync(input.Project);
            }
        }


        /// <summary>
        /// 新增Project
        /// </summary>

        protected virtual async Task<ProjectEditDto> CreateAsync(ProjectEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增

            // var entity = ObjectMapper.Map <Project>(input);
            var entity = input.MapTo<Project>();


            entity = await _entityRepository.InsertAsync(entity);
            return entity.MapTo<ProjectEditDto>();
        }

        /// <summary>
        /// 编辑Project
        /// </summary>

        protected virtual async Task UpdateAsync(ProjectEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新

            var entity = await _entityRepository.GetAsync(input.Id.Value);
            input.MapTo(entity);

            // ObjectMapper.Map(input, entity);
            await _entityRepository.UpdateAsync(entity);
        }



        /// <summary>
        /// 删除Project信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task DeleteAsync(EntityDto<Guid> input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _entityRepository.DeleteAsync(input.Id);
        }



        /// <summary>
        /// 批量删除Project的方法
        /// </summary>

        public async Task BatchDeleteAsync(List<Guid> input)
        {
            // TODO:批量删除前的逻辑判断，是否允许删除
            await _entityRepository.DeleteAsync(s => input.Contains(s.Id));
        }

        public async Task<List<DropDownDto>> GetDropDownsAsync()
        {
            var query = _entityRepository.GetAll();
            var entityList = await query
                    .OrderBy(a => a.CreationTime).AsNoTracking()
                    .Select(c => new DropDownDto() { Text = c.Name, Value = c.Id.ToString() })
                    .ToListAsync();
            return entityList;
        }

        /// <summary>
        /// 导出Project为excel表,等待开发。
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


