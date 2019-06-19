
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
using Abp.Auditing;
using HC.AbpCore.Projects.ProjectDetails;
using HC.AbpCore.Projects.ProjectDetails.DomainService;
using static HC.AbpCore.Projects.ProjectBase;
using HC.AbpCore.Customers.DomainService;

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
        private readonly IProjectDetailManager _projectDetailManager;
        private readonly IProjectManager _entityManager;
        private readonly ICustomerManager _customerManager;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public ProjectAppService(
        IRepository<Project, Guid> entityRepository,
        IRepository<Customer, int> customerRepository,
        IRepository<Employee, string> employeeRepository,
        IProjectDetailManager projectDetailManager,
        ICustomerManager customerManager
        , IProjectManager entityManager
        )
        {
            _customerManager = customerManager;
            _projectDetailManager = projectDetailManager;
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
                .WhereIf(input.CustomerId.HasValue, a => a.CustomerId == input.CustomerId.Value)
                .WhereIf(input.Id.HasValue, a => a.Id == input.Id.Value);
            // TODO:根据传入的参数添加过滤条件

            var customerList = await _customerRepository.GetAll().AsNoTracking().ToListAsync();
            var employeeList = await _employeeRepository.GetAll().AsNoTracking().ToListAsync();

            var count = await query.CountAsync();

            var entityList = await query
                    .OrderBy(input.Sorting).AsNoTracking()
                    .OrderByDescending(aa => aa.CreationTime)
                    .PageBy(input)
                    .Select(aa => new ProjectListDto()
                    {
                        Id = aa.Id,
                        Mode = aa.Mode,
                        ProfitRatio = aa.ProfitRatio,
                        BillCost = aa.BillCost,
                        Type = aa.Type,
                        ProjectCode = aa.ProjectCode,
                        Name = aa.Name,
                        StartDate = aa.StartDate,
                        EndDate = aa.EndDate,
                        Budget = aa.Budget,
                        Status = aa.Status,
                        ImplementMoney = aa.ImplementMoney,
                        CustomerName = !aa.CustomerId.HasValue ? null : customerList.Where(bb => bb.Id == aa.CustomerId.Value).FirstOrDefault().Name,
                        ProjectSalesName = String.IsNullOrEmpty(aa.ProjectSalesId) ? null : employeeList.Where(bb => bb.Id == aa.ProjectSalesId).FirstOrDefault().Name,
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
            if (!String.IsNullOrEmpty(projectListDto.ProjectSalesId))
                projectListDto.ProjectSalesName = (await _employeeRepository.GetAsync(projectListDto.ProjectSalesId)).Name;
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

        public async Task<APIResultDto> CreateOrUpdateAsync(CreateOrUpdateProjectInput input)
        {
            if (input.Project.Id.HasValue)
            {
                return await UpdateAsync(input.Project);
            }
            else
            {
                return await CreateAsync(input.Project);
            }
        }


        /// <summary>
        /// 添加project以及projectDetail的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<APIResultDto> CreateProjectAndDetailAsync(CreateProjectAndDetailInput input)
        {
            var projectCount = await _entityRepository.GetAll().Where(aa => aa.ProjectCode == input.Project.ProjectCode).CountAsync();
            if (projectCount > 0)
                return new APIResultDto() { Code = 0, Msg = "保存失败,项目编号已存在" };

            var entity = input.Project.MapTo<Project>();
            entity = await _entityRepository.InsertAsync(entity);

            foreach (var projectDetail in input.ProjectDetails)
            {
                projectDetail.ProjectId = entity.Id;
                var detail = projectDetail.MapTo<ProjectDetail>();
                await _projectDetailManager.CreateAsync(detail);
            }
            var item = entity.MapTo<ProjectEditDto>();
            if (entity != null)
                return new APIResultDto() { Code = 1, Msg = "保存成功", Data = item };
            else
                return new APIResultDto() { Code = 0, Msg = "保存失败" };
        }


        /// <summary>
        /// 新增Project
        /// </summary>

        protected virtual async Task<APIResultDto> CreateAsync(ProjectEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增

            //判断编号是否重复
            var projectCount = await _entityRepository.GetAll().Where(aa => aa.ProjectCode == input.ProjectCode).CountAsync();
            if (projectCount > 0)
                return new APIResultDto() { Code = 0, Msg = "保存失败,项目编号已存在" };

            // var entity = ObjectMapper.Map <Project>(input);
            var entity = input.MapTo<Project>();


            entity = await _entityRepository.InsertAsync(entity);
            var item = entity.MapTo<ProjectEditDto>();
            if (entity != null)
                return new APIResultDto() { Code = 1, Msg = "保存成功", Data = item };
            else
                return new APIResultDto() { Code = 0, Msg = "保存失败" };
            //return entity.MapTo<ProjectEditDto>();
        }

        /// <summary>
        /// 编辑Project
        /// </summary>

        protected virtual async Task<APIResultDto> UpdateAsync(ProjectEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新

            var entity = await _entityRepository.GetAsync(input.Id.Value);
            //判断合同编号是否重复
            if (entity.ProjectCode != input.ProjectCode)
            {
                var projectCount = await _entityRepository.GetAll().Where(aa => aa.ProjectCode == input.ProjectCode).CountAsync();
                if (projectCount > 0)
                    return new APIResultDto() { Code = 0, Msg = "保存失败,项目编号已存在" };
            }
            input.MapTo(entity);

            // ObjectMapper.Map(input, entity);
            entity = await _entityRepository.UpdateAsync(entity);
            var item = entity.MapTo<ProjectEditDto>();
            if (entity != null)
                return new APIResultDto() { Code = 1, Msg = "保存成功", Data = item };
            else
                return new APIResultDto() { Code = 0, Msg = "保存失败" };
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

        /// <summary>
        /// 获取项目下拉列表
        /// </summary>
        /// <returns></returns>
        [AbpAllowAnonymous]
        [Audited]
        public async Task<List<DropDownDto>> GetDropDownsAsync()
        {
            var query = _entityRepository.GetAll();
            var entityList = await query
                    .OrderBy(a => a.CreationTime).AsNoTracking()
                    .Select(c => new DropDownDto() { Text = c.Name + "(" + c.ProjectCode + ")", Value = c.Id.ToString() })
                    .ToListAsync();
            return entityList;
        }

        /// <summary>
        /// 修改项目状态
        /// </summary>
        /// <param name="input"></param>
        /// <param name="projectStatus"></param>
        /// <returns></returns>
        public async Task ModifyProjectStatusAsync(EntityDto<Guid> input, ProjectStatus projectStatus)
        {
            var entity = await _entityRepository.GetAsync(input.Id);
            entity.Status = projectStatus;
            await _entityRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 自动生成项目编号
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<string> GenerateProjectCodeAsync(string type)
        {
            string projectCode = "";
            if (!String.IsNullOrEmpty(type))
            {
                var project = await _entityRepository.GetAll().Where(aa => aa.Type == type).AsNoTracking().ToListAsync();
                if (project?.Count > 0)
                {
                    projectCode = project.Max(aa => aa.ProjectCode);
                }
                switch (type)
                {
                    case "软件开发":
                        if (!String.IsNullOrEmpty(projectCode))
                        {
                            var arr = projectCode.Split("-");
                            projectCode = arr[0].ToString() + "-" + arr[1].ToString() + "-" + (long.Parse(arr[2]) + 1).ToString();
                        }
                        else
                        {
                            projectCode = "HC-R-" + DateTime.Now.ToString("yyMM") + "001";
                        }
                        break;
                    case "系统集成":
                        if (!String.IsNullOrEmpty(projectCode))
                        {
                            var arr = projectCode.Split("-");
                            projectCode = arr[0].ToString() + "-" + arr[1].ToString() + "-" + (long.Parse(arr[2]) + 1).ToString();
                        }
                        else
                        {
                            projectCode = "HC-X-" + DateTime.Now.ToString("yyMM") + "001";
                        }
                        break;
                    case "维保服务":
                        if (!String.IsNullOrEmpty(projectCode))
                        {
                            var arr = projectCode.Split("-");
                            projectCode = arr[0].ToString() + "-" + arr[1].ToString() + "-" + (long.Parse(arr[2]) + 1).ToString();
                        }
                        else
                        {
                            projectCode = "HC-W-" + DateTime.Now.ToString("yyMM") + "001";
                        }
                        break;
                    case "内部需求":
                        if (!String.IsNullOrEmpty(projectCode))
                        {
                            var arr = projectCode.Split("-");
                            projectCode = arr[0].ToString() + "-" + arr[1].ToString() + "-" + (long.Parse(arr[2]) + 1).ToString();
                        }
                        else
                        {
                            projectCode = "HC-N-" + DateTime.Now.ToString("yyMM") + "001";
                        }
                        break;
                    default:
                        if (!String.IsNullOrEmpty(projectCode))
                        {
                            var arr = projectCode.Split("-");
                            projectCode = arr[0].ToString() + "-" + arr[1].ToString() + "-" + (long.Parse(arr[2]) + 1).ToString();
                        }
                        else
                        {
                            projectCode = "HC-Y-" + DateTime.Now.ToString("yyMM") + "001";
                        }
                        break;
                }

            }
            else
            {
                projectCode = "HC-R-" + DateTime.Now.ToString("yyMM") + "001";
            }
            return projectCode;

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


