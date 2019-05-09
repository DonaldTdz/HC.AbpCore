
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


using HC.AbpCore.PaymentPlans;
using HC.AbpCore.PaymentPlans.Dtos;
using HC.AbpCore.PaymentPlans.DomainService;



namespace HC.AbpCore.PaymentPlans
{
    /// <summary>
    /// PaymentPlan应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize]
    public class PaymentPlanAppService : AbpCoreAppServiceBase, IPaymentPlanAppService
    {
        private readonly IRepository<PaymentPlan, Guid> _entityRepository;

        private readonly IPaymentPlanManager _entityManager;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public PaymentPlanAppService(
        IRepository<PaymentPlan, Guid> entityRepository
        ,IPaymentPlanManager entityManager
        )
        {
            _entityRepository = entityRepository; 
             _entityManager=entityManager;
        }


        /// <summary>
        /// 获取PaymentPlan的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
		 
        public async Task<PagedResultDto<PaymentPlanListDto>> GetPaged(GetPaymentPlansInput input)
		{

		    var query = _entityRepository.GetAll()
                .WhereIf(input.ProjectId.HasValue,aa=>aa.ProjectId==input.ProjectId.Value);
			// TODO:根据传入的参数添加过滤条件
            

			var count = await query.CountAsync();

			var entityList = await query
					.OrderBy(input.Sorting).AsNoTracking()
					.PageBy(input)
					.ToListAsync();

			// var entityListDtos = ObjectMapper.Map<List<PaymentPlanListDto>>(entityList);
			var entityListDtos =entityList.MapTo<List<PaymentPlanListDto>>();

			return new PagedResultDto<PaymentPlanListDto>(count,entityListDtos);
		}


		/// <summary>
		/// 通过指定id获取PaymentPlanListDto信息
		/// </summary>
		 
		public async Task<PaymentPlanListDto> GetById(EntityDto<Guid> input)
		{
			var entity = await _entityRepository.GetAsync(input.Id);

		    return entity.MapTo<PaymentPlanListDto>();
		}

		/// <summary>
		/// 获取编辑 PaymentPlan
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		
		public async Task<GetPaymentPlanForEditOutput> GetForEdit(NullableIdDto<Guid> input)
		{
			var output = new GetPaymentPlanForEditOutput();
PaymentPlanEditDto editDto;

			if (input.Id.HasValue)
			{
				var entity = await _entityRepository.GetAsync(input.Id.Value);

				editDto = entity.MapTo<PaymentPlanEditDto>();

				//paymentPlanEditDto = ObjectMapper.Map<List<paymentPlanEditDto>>(entity);
			}
			else
			{
				editDto = new PaymentPlanEditDto();
			}

			output.PaymentPlan = editDto;
			return output;
		}


		/// <summary>
		/// 添加或者修改PaymentPlan的公共方法
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		
		public async Task CreateOrUpdate(CreateOrUpdatePaymentPlanInput input)
		{

			if (input.PaymentPlan.Id.HasValue)
			{
				await Update(input.PaymentPlan);
			}
			else
			{
				await Create(input.PaymentPlan);
			}
		}


		/// <summary>
		/// 新增PaymentPlan
		/// </summary>
		
		protected virtual async Task<PaymentPlanEditDto> Create(PaymentPlanEditDto input)
		{
			//TODO:新增前的逻辑判断，是否允许新增

            // var entity = ObjectMapper.Map <PaymentPlan>(input);
            var entity=input.MapTo<PaymentPlan>();
			

			entity = await _entityRepository.InsertAsync(entity);
			return entity.MapTo<PaymentPlanEditDto>();
		}

		/// <summary>
		/// 编辑PaymentPlan
		/// </summary>
		
		protected virtual async Task Update(PaymentPlanEditDto input)
		{
			//TODO:更新前的逻辑判断，是否允许更新

			var entity = await _entityRepository.GetAsync(input.Id.Value);
			input.MapTo(entity);

			// ObjectMapper.Map(input, entity);
		    await _entityRepository.UpdateAsync(entity);
		}



		/// <summary>
		/// 删除PaymentPlan信息的方法
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		
		public async Task Delete(EntityDto<Guid> input)
		{
			//TODO:删除前的逻辑判断，是否允许删除
			await _entityRepository.DeleteAsync(input.Id);
		}



		/// <summary>
		/// 批量删除PaymentPlan的方法
		/// </summary>
		
		public async Task BatchDelete(List<Guid> input)
		{
			// TODO:批量删除前的逻辑判断，是否允许删除
			await _entityRepository.DeleteAsync(s => input.Contains(s.Id));
		}


		/// <summary>
		/// 导出PaymentPlan为excel表,等待开发。
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


