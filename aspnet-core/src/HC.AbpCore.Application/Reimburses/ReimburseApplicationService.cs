
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

namespace HC.AbpCore.Reimburses
{
    /// <summary>
    /// Reimburse应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize]
    public class ReimburseAppService : AbpCoreAppServiceBase, IReimburseAppService
    {
        private readonly IRepository<Reimburse, Guid> _entityRepository;
        private readonly IRepository<Project, Guid> _projectRepository;
        private readonly IRepository<Employee, string> _employeeRepository;
        private readonly IReimburseManager _entityManager;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public ReimburseAppService(
        IRepository<Reimburse, Guid> entityRepository
        , IRepository<Project, Guid> projectRepository
        , IRepository<Employee, string> employeeRepository
        , IReimburseManager entityManager
        )
        {
            _entityRepository = entityRepository; 
             _entityManager=entityManager;
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

		    var query = _entityRepository.GetAll().WhereIf(input.ProjectId.HasValue,aa=>aa.ProjectId==input.ProjectId.Value)
                .WhereIf(input.Status.HasValue,aa=>aa.Status==input.Status.Value)
                .WhereIf(!String.IsNullOrEmpty(input.EmployeeId), aa => aa.EmployeeId == input.EmployeeId);
            // TODO:根据传入的参数添加过滤条件
            var projects = _projectRepository.GetAll();
            var employees = _employeeRepository.GetAll();

			//var count = await query.CountAsync();

            var entityList = from item in query
                             join project in projects on item.ProjectId equals project.Id
                             join employee in employees on item.EmployeeId equals employee.Id into employeeName
                             join approver in employees on item.ApproverId equals approver.Id into approverName
                             from bb in employeeName.DefaultIfEmpty()
                             from cc in approverName.DefaultIfEmpty()
                             select new ReimburseListDto()
                             {
                                 Id = item.Id,
                                 ProjectId = item.ProjectId,
                                 ProjectName = project.Name+"("+project.ProjectCode+")",
                                 EmployeeId = item.EmployeeId,
                                 EmployeeName = bb.Name,
                                 Amount = item.Amount,
                                 Status = item.Status,
                                 SubmitDate = item.SubmitDate,
                                 ApproverId = item.ApproverId,
                                 ApprovalTime = item.ApprovalTime,
                                 ApproverName=cc.Name,
                                 CancelTime = item.CancelTime,
                                 CreationTime = item.CreationTime
                             };
            var count = await entityList.CountAsync();

            var items = await entityList
                .OrderByDescending(aa => aa.SubmitDate)
                //.OrderBy(aa => aa.Status)
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
                item.ApproverName = (await _employeeRepository.FirstOrDefaultAsync(aa => aa.Id == item.ApproverId)).Name;
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
        public async Task CreateOrUpdateAsync(CreateOrUpdateReimburseInput input)
		{

			if (input.Reimburse.Id.HasValue)
			{
				await UpdateAsync(input.Reimburse);
			}
			else
			{
				await CreateAsync(input.Reimburse);
			}
		}


		/// <summary>
		/// 新增Reimburse
		/// </summary>
		
		protected virtual async Task<ReimburseEditDto> CreateAsync(ReimburseEditDto input)
		{
			//TODO:新增前的逻辑判断，是否允许新增

            // var entity = ObjectMapper.Map <Reimburse>(input);
            var entity=input.MapTo<Reimburse>();
            entity.Status = ReimburseStatusEnum.草稿;

			entity = await _entityRepository.InsertAsync(entity);
			return entity.MapTo<ReimburseEditDto>();
		}

		/// <summary>
		/// 编辑Reimburse
		/// </summary>
		
		protected virtual async Task UpdateAsync(ReimburseEditDto input)
		{
			//TODO:更新前的逻辑判断，是否允许更新

			var entity = await _entityRepository.GetAsync(input.Id.Value);
			input.MapTo(entity);

			// ObjectMapper.Map(input, entity);
		    await _entityRepository.UpdateAsync(entity);
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


