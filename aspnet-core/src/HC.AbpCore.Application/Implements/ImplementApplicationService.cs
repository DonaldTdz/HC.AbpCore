
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


using HC.AbpCore.Implements;
using HC.AbpCore.Implements.Dtos;
using HC.AbpCore.Implements.DomainService;



namespace HC.AbpCore.Implements
{
    /// <summary>
    /// Implement应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize]
    public class ImplementAppService : AbpCoreAppServiceBase, IImplementAppService
    {
        private readonly IRepository<Implement, Guid> _entityRepository;

        private readonly IImplementManager _entityManager;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public ImplementAppService(
        IRepository<Implement, Guid> entityRepository
        ,IImplementManager entityManager
        )
        {
            _entityRepository = entityRepository; 
             _entityManager=entityManager;
        }


        /// <summary>
        /// 获取Implement的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
		 
        public async Task<PagedResultDto<ImplementListDto>> GetPagedAsync(GetImplementsInput input)
		{

		    var query = _entityRepository.GetAll();
			// TODO:根据传入的参数添加过滤条件
            

			var count = await query.CountAsync();

			var entityList = await query
					.OrderBy(input.Sorting).AsNoTracking()
					.PageBy(input)
					.ToListAsync();

			// var entityListDtos = ObjectMapper.Map<List<ImplementListDto>>(entityList);
			var entityListDtos =entityList.MapTo<List<ImplementListDto>>();

			return new PagedResultDto<ImplementListDto>(count,entityListDtos);
		}


		/// <summary>
		/// 通过指定id获取ImplementListDto信息
		/// </summary>
		 
		public async Task<ImplementListDto> GetByIdAsync(EntityDto<Guid> input)
		{
			var entity = await _entityRepository.GetAsync(input.Id);

		    return entity.MapTo<ImplementListDto>();
		}

		/// <summary>
		/// 获取编辑 Implement
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		
		public async Task<GetImplementForEditOutput> GetForEditAsync(NullableIdDto<Guid> input)
		{
			var output = new GetImplementForEditOutput();
ImplementEditDto editDto;

			if (input.Id.HasValue)
			{
				var entity = await _entityRepository.GetAsync(input.Id.Value);

				editDto = entity.MapTo<ImplementEditDto>();

				//implementEditDto = ObjectMapper.Map<List<implementEditDto>>(entity);
			}
			else
			{
				editDto = new ImplementEditDto();
			}

			output.Implement = editDto;
			return output;
		}


		/// <summary>
		/// 添加或者修改Implement的公共方法
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		
		public async Task CreateOrUpdateAsync(CreateOrUpdateImplementInput input)
		{

			if (input.Implement.Id.HasValue)
			{
				await UpdateAsync(input.Implement);
			}
			else
			{
				await CreateAsync(input.Implement);
			}
		}


		/// <summary>
		/// 新增Implement
		/// </summary>
		
		protected virtual async Task<ImplementEditDto> CreateAsync(ImplementEditDto input)
		{
			//TODO:新增前的逻辑判断，是否允许新增

            // var entity = ObjectMapper.Map <Implement>(input);
            var entity=input.MapTo<Implement>();
			

			entity = await _entityRepository.InsertAsync(entity);
			return entity.MapTo<ImplementEditDto>();
		}

		/// <summary>
		/// 编辑Implement
		/// </summary>
		
		protected virtual async Task UpdateAsync(ImplementEditDto input)
		{
			//TODO:更新前的逻辑判断，是否允许更新

			var entity = await _entityRepository.GetAsync(input.Id.Value);
			input.MapTo(entity);

			// ObjectMapper.Map(input, entity);
		    await _entityRepository.UpdateAsync(entity);
		}



		/// <summary>
		/// 删除Implement信息的方法
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		
		public async Task DeleteAsync(EntityDto<Guid> input)
		{
			//TODO:删除前的逻辑判断，是否允许删除
			await _entityRepository.DeleteAsync(input.Id);
		}



		/// <summary>
		/// 批量删除Implement的方法
		/// </summary>
		
		public async Task BatchDeleteAsync(List<Guid> input)
		{
			// TODO:批量删除前的逻辑判断，是否允许删除
			await _entityRepository.DeleteAsync(s => input.Contains(s.Id));
		}


		/// <summary>
		/// 导出Implement为excel表,等待开发。
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


