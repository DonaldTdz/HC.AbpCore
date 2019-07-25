
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


using HC.AbpCore.AdvancePayments;
using HC.AbpCore.AdvancePayments.Dtos;
using HC.AbpCore.AdvancePayments.DomainService;



namespace HC.AbpCore.AdvancePayments
{
    /// <summary>
    /// AdvancePayment应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize]
    public class AdvancePaymentAppService : AbpCoreAppServiceBase, IAdvancePaymentAppService
    {
        private readonly IRepository<AdvancePayment, Guid> _entityRepository;

        private readonly IAdvancePaymentManager _entityManager;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public AdvancePaymentAppService(
        IRepository<AdvancePayment, Guid> entityRepository
        ,IAdvancePaymentManager entityManager
        )
        {
            _entityRepository = entityRepository; 
             _entityManager=entityManager;
        }


        /// <summary>
        /// 获取AdvancePayment的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
		 
        public async Task<PagedResultDto<AdvancePaymentListDto>> GetPagedAsync(GetAdvancePaymentsInput input)
		{

		    var query = _entityRepository.GetAll().WhereIf(input.PurchaseId.HasValue,aa=>aa.PurchaseId==input.PurchaseId.Value);
			// TODO:根据传入的参数添加过滤条件
            

			var count = await query.CountAsync();

			var entityList = await query
					.OrderBy(input.Sorting).AsNoTracking()
					.PageBy(input)
					.ToListAsync();

			// var entityListDtos = ObjectMapper.Map<List<AdvancePaymentListDto>>(entityList);
			var entityListDtos =entityList.MapTo<List<AdvancePaymentListDto>>();

			return new PagedResultDto<AdvancePaymentListDto>(count,entityListDtos);
		}


		/// <summary>
		/// 通过指定id获取AdvancePaymentListDto信息
		/// </summary>
		 
		public async Task<AdvancePaymentListDto> GetByIdAsync(EntityDto<Guid> input)
		{
			var entity = await _entityRepository.GetAsync(input.Id);

		    return entity.MapTo<AdvancePaymentListDto>();
		}

		/// <summary>
		/// 获取编辑 AdvancePayment
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		
		public async Task<GetAdvancePaymentForEditOutput> GetForEditAsync(NullableIdDto<Guid> input)
		{
			var output = new GetAdvancePaymentForEditOutput();
AdvancePaymentEditDto editDto;

			if (input.Id.HasValue)
			{
				var entity = await _entityRepository.GetAsync(input.Id.Value);

				editDto = entity.MapTo<AdvancePaymentEditDto>();

				//advancePaymentEditDto = ObjectMapper.Map<List<advancePaymentEditDto>>(entity);
			}
			else
			{
				editDto = new AdvancePaymentEditDto();
			}

			output.AdvancePayment = editDto;
			return output;
		}


		/// <summary>
		/// 添加或者修改AdvancePayment的公共方法
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		
		public async Task CreateOrUpdateAsync(CreateOrUpdateAdvancePaymentInput input)
		{

			if (input.AdvancePayment.Id.HasValue)
			{
				await Update(input.AdvancePayment);
			}
			else
			{
				await Create(input.AdvancePayment);
			}
		}


		/// <summary>
		/// 新增AdvancePayment
		/// </summary>
		
		protected virtual async Task<AdvancePaymentEditDto> Create(AdvancePaymentEditDto input)
		{
			//TODO:新增前的逻辑判断，是否允许新增

            // var entity = ObjectMapper.Map <AdvancePayment>(input);
            var entity=input.MapTo<AdvancePayment>();
			

			entity = await _entityRepository.InsertAsync(entity);
			return entity.MapTo<AdvancePaymentEditDto>();
		}

		/// <summary>
		/// 编辑AdvancePayment
		/// </summary>
		
		protected virtual async Task Update(AdvancePaymentEditDto input)
		{
			//TODO:更新前的逻辑判断，是否允许更新

			var entity = await _entityRepository.GetAsync(input.Id.Value);
			input.MapTo(entity);

			// ObjectMapper.Map(input, entity);
		    await _entityRepository.UpdateAsync(entity);
		}



		/// <summary>
		/// 删除AdvancePayment信息的方法
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		
		public async Task DeleteAsync(EntityDto<Guid> input)
		{
			//TODO:删除前的逻辑判断，是否允许删除
			await _entityRepository.DeleteAsync(input.Id);
		}



		/// <summary>
		/// 批量删除AdvancePayment的方法
		/// </summary>
		
		public async Task BatchDeleteAsync(List<Guid> input)
		{
			// TODO:批量删除前的逻辑判断，是否允许删除
			await _entityRepository.DeleteAsync(s => input.Contains(s.Id));
		}


		/// <summary>
		/// 导出AdvancePayment为excel表,等待开发。
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


