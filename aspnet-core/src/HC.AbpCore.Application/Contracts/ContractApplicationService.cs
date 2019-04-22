
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


using HC.AbpCore.Contracts;
using HC.AbpCore.Contracts.Dtos;
using HC.AbpCore.Contracts.DomainService;



namespace HC.AbpCore.Contracts
{
    /// <summary>
    /// Contract应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize]
    public class ContractAppService : AbpCoreAppServiceBase, IContractAppService
    {
        private readonly IRepository<Contract, Guid> _entityRepository;

        private readonly IContractManager _entityManager;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public ContractAppService(
        IRepository<Contract, Guid> entityRepository
        ,IContractManager entityManager
        )
        {
            _entityRepository = entityRepository; 
             _entityManager=entityManager;
        }


        /// <summary>
        /// 获取Contract的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
		 
        public async Task<PagedResultDto<ContractListDto>> GetPaged(GetContractsInput input)
		{

		    var query = _entityRepository.GetAll();
			// TODO:根据传入的参数添加过滤条件
            

			var count = await query.CountAsync();

			var entityList = await query
					.OrderBy(input.Sorting).AsNoTracking()
					.PageBy(input)
					.ToListAsync();

			// var entityListDtos = ObjectMapper.Map<List<ContractListDto>>(entityList);
			var entityListDtos =entityList.MapTo<List<ContractListDto>>();

			return new PagedResultDto<ContractListDto>(count,entityListDtos);
		}


		/// <summary>
		/// 通过指定id获取ContractListDto信息
		/// </summary>
		 
		public async Task<ContractListDto> GetById(EntityDto<Guid> input)
		{
			var entity = await _entityRepository.GetAsync(input.Id);

		    return entity.MapTo<ContractListDto>();
		}

		/// <summary>
		/// 获取编辑 Contract
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		
		public async Task<GetContractForEditOutput> GetForEdit(NullableIdDto<Guid> input)
		{
			var output = new GetContractForEditOutput();
ContractEditDto editDto;

			if (input.Id.HasValue)
			{
				var entity = await _entityRepository.GetAsync(input.Id.Value);

				editDto = entity.MapTo<ContractEditDto>();

				//contractEditDto = ObjectMapper.Map<List<contractEditDto>>(entity);
			}
			else
			{
				editDto = new ContractEditDto();
			}

			output.Contract = editDto;
			return output;
		}


		/// <summary>
		/// 添加或者修改Contract的公共方法
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		
		public async Task CreateOrUpdate(CreateOrUpdateContractInput input)
		{

			if (input.Contract.Id.HasValue)
			{
				await Update(input.Contract);
			}
			else
			{
				await Create(input.Contract);
			}
		}


		/// <summary>
		/// 新增Contract
		/// </summary>
		
		protected virtual async Task<ContractEditDto> Create(ContractEditDto input)
		{
			//TODO:新增前的逻辑判断，是否允许新增

            // var entity = ObjectMapper.Map <Contract>(input);
            var entity=input.MapTo<Contract>();
			

			entity = await _entityRepository.InsertAsync(entity);
			return entity.MapTo<ContractEditDto>();
		}

		/// <summary>
		/// 编辑Contract
		/// </summary>
		
		protected virtual async Task Update(ContractEditDto input)
		{
			//TODO:更新前的逻辑判断，是否允许更新

			var entity = await _entityRepository.GetAsync(input.Id.Value);
			input.MapTo(entity);

			// ObjectMapper.Map(input, entity);
		    await _entityRepository.UpdateAsync(entity);
		}



		/// <summary>
		/// 删除Contract信息的方法
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		
		public async Task Delete(EntityDto<Guid> input)
		{
			//TODO:删除前的逻辑判断，是否允许删除
			await _entityRepository.DeleteAsync(input.Id);
		}



		/// <summary>
		/// 批量删除Contract的方法
		/// </summary>
		
		public async Task BatchDelete(List<Guid> input)
		{
			// TODO:批量删除前的逻辑判断，是否允许删除
			await _entityRepository.DeleteAsync(s => input.Contains(s.Id));
		}


		/// <summary>
		/// 导出Contract为excel表,等待开发。
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


