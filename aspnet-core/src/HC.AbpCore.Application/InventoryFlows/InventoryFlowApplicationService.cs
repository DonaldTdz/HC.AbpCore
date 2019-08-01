
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


using HC.AbpCore.InventoryFlows;
using HC.AbpCore.InventoryFlows.Dtos;
using HC.AbpCore.InventoryFlows.DomainService;



namespace HC.AbpCore.InventoryFlows
{
    /// <summary>
    /// InventoryFlow应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize]
    public class InventoryFlowAppService : AbpCoreAppServiceBase, IInventoryFlowAppService
    {
        private readonly IRepository<InventoryFlow, long> _entityRepository;

        private readonly IInventoryFlowManager _entityManager;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public InventoryFlowAppService(
        IRepository<InventoryFlow, long> entityRepository
        ,IInventoryFlowManager entityManager
        )
        {
            _entityRepository = entityRepository; 
             _entityManager=entityManager;
        }


        /// <summary>
        /// 获取InventoryFlow的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
		 
        public async Task<PagedResultDto<InventoryFlowListDto>> GetPaged(GetInventoryFlowsInput input)
		{

		    var query = _entityRepository.GetAll();
			// TODO:根据传入的参数添加过滤条件
            

			var count = await query.CountAsync();

			var entityList = await query
					.OrderBy(input.Sorting).AsNoTracking()
					.PageBy(input)
					.ToListAsync();

			// var entityListDtos = ObjectMapper.Map<List<InventoryFlowListDto>>(entityList);
			var entityListDtos =entityList.MapTo<List<InventoryFlowListDto>>();

			return new PagedResultDto<InventoryFlowListDto>(count,entityListDtos);
		}


		/// <summary>
		/// 通过指定id获取InventoryFlowListDto信息
		/// </summary>
		 
		public async Task<InventoryFlowListDto> GetById(EntityDto<long> input)
		{
			var entity = await _entityRepository.GetAsync(input.Id);

		    return entity.MapTo<InventoryFlowListDto>();
		}

		/// <summary>
		/// 获取编辑 InventoryFlow
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		
		public async Task<GetInventoryFlowForEditOutput> GetForEdit(NullableIdDto<long> input)
		{
			var output = new GetInventoryFlowForEditOutput();
InventoryFlowEditDto editDto;

			if (input.Id.HasValue)
			{
				var entity = await _entityRepository.GetAsync(input.Id.Value);

				editDto = entity.MapTo<InventoryFlowEditDto>();

				//inventoryFlowEditDto = ObjectMapper.Map<List<inventoryFlowEditDto>>(entity);
			}
			else
			{
				editDto = new InventoryFlowEditDto();
			}

			output.InventoryFlow = editDto;
			return output;
		}


		/// <summary>
		/// 添加或者修改InventoryFlow的公共方法
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		
		public async Task CreateOrUpdate(CreateOrUpdateInventoryFlowInput input)
		{

			if (input.InventoryFlow.Id.HasValue)
			{
				await Update(input.InventoryFlow);
			}
			else
			{
				await Create(input.InventoryFlow);
			}
		}


		/// <summary>
		/// 新增InventoryFlow
		/// </summary>
		
		protected virtual async Task<InventoryFlowEditDto> Create(InventoryFlowEditDto input)
		{
			//TODO:新增前的逻辑判断，是否允许新增

            // var entity = ObjectMapper.Map <InventoryFlow>(input);
            var entity=input.MapTo<InventoryFlow>();
			

			entity = await _entityRepository.InsertAsync(entity);
			return entity.MapTo<InventoryFlowEditDto>();
		}

		/// <summary>
		/// 编辑InventoryFlow
		/// </summary>
		
		protected virtual async Task Update(InventoryFlowEditDto input)
		{
			//TODO:更新前的逻辑判断，是否允许更新

			var entity = await _entityRepository.GetAsync(input.Id.Value);
			input.MapTo(entity);

			// ObjectMapper.Map(input, entity);
		    await _entityRepository.UpdateAsync(entity);
		}



		/// <summary>
		/// 删除InventoryFlow信息的方法
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		
		public async Task Delete(EntityDto<long> input)
		{
			//TODO:删除前的逻辑判断，是否允许删除
			await _entityRepository.DeleteAsync(input.Id);
		}



		/// <summary>
		/// 批量删除InventoryFlow的方法
		/// </summary>
		
		public async Task BatchDelete(List<long> input)
		{
			// TODO:批量删除前的逻辑判断，是否允许删除
			await _entityRepository.DeleteAsync(s => input.Contains(s.Id));
		}


		/// <summary>
		/// 导出InventoryFlow为excel表,等待开发。
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


