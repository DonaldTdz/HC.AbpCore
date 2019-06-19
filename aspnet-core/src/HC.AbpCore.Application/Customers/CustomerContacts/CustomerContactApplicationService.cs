
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


using HC.AbpCore.Customers.CustomerContacts;
using HC.AbpCore.Customers.CustomerContacts.Dtos;
using HC.AbpCore.Customers.CustomerContacts.DomainService;
using HC.AbpCore.Dtos;

namespace HC.AbpCore.Customers.CustomerContacts
{
    /// <summary>
    /// CustomerContact应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize]
    public class CustomerContactAppService : AbpCoreAppServiceBase, ICustomerContactAppService
    {
        private readonly IRepository<CustomerContact, int> _entityRepository;

        private readonly ICustomerContactManager _entityManager;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public CustomerContactAppService(
        IRepository<CustomerContact, int> entityRepository
        ,ICustomerContactManager entityManager
        )
        {
            _entityRepository = entityRepository; 
             _entityManager=entityManager;
        }


        /// <summary>
        /// 获取CustomerContact的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
		 
        public async Task<PagedResultDto<CustomerContactListDto>> GetPagedAsync(GetCustomerContactsInput input)
		{

		    var query = _entityRepository.GetAll().WhereIf(input.CustomerId.HasValue,aa=>aa.CustomerId==input.CustomerId.Value);
			// TODO:根据传入的参数添加过滤条件
            

			var count = await query.CountAsync();

			var entityList = await query
					.OrderBy(input.Sorting).AsNoTracking()
					.PageBy(input)
					.ToListAsync();

			// var entityListDtos = ObjectMapper.Map<List<CustomerContactListDto>>(entityList);
			var entityListDtos =entityList.MapTo<List<CustomerContactListDto>>();

			return new PagedResultDto<CustomerContactListDto>(count,entityListDtos);
		}


		/// <summary>
		/// 通过指定id获取CustomerContactListDto信息
		/// </summary>
		 
		public async Task<CustomerContactListDto> GetByIdAsync(EntityDto<int> input)
		{
			var entity = await _entityRepository.GetAsync(input.Id);

		    return entity.MapTo<CustomerContactListDto>();
		}

		/// <summary>
		/// 获取编辑 CustomerContact
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		
		public async Task<GetCustomerContactForEditOutput> GetForEditAsync(NullableIdDto<int> input)
		{
			var output = new GetCustomerContactForEditOutput();
CustomerContactEditDto editDto;

			if (input.Id.HasValue)
			{
				var entity = await _entityRepository.GetAsync(input.Id.Value);

				editDto = entity.MapTo<CustomerContactEditDto>();

				//customerContactEditDto = ObjectMapper.Map<List<customerContactEditDto>>(entity);
			}
			else
			{
				editDto = new CustomerContactEditDto();
			}

			output.CustomerContact = editDto;
			return output;
		}


		/// <summary>
		/// 添加或者修改CustomerContact的公共方法
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		
		public async Task CreateOrUpdateAsync(CreateOrUpdateCustomerContactInput input)
		{

			if (input.CustomerContact.Id.HasValue)
			{
				await UpdateAsync(input.CustomerContact);
			}
			else
			{
				await CreateAsync(input.CustomerContact);
			}
		}


		/// <summary>
		/// 新增CustomerContact
		/// </summary>
		
		protected virtual async Task<CustomerContactEditDto> CreateAsync(CustomerContactEditDto input)
		{
			//TODO:新增前的逻辑判断，是否允许新增

            // var entity = ObjectMapper.Map <CustomerContact>(input);
            var entity=input.MapTo<CustomerContact>();
			

			entity = await _entityRepository.InsertAsync(entity);
			return entity.MapTo<CustomerContactEditDto>();
		}

		/// <summary>
		/// 编辑CustomerContact
		/// </summary>
		
		protected virtual async Task UpdateAsync(CustomerContactEditDto input)
		{
			//TODO:更新前的逻辑判断，是否允许更新

			var entity = await _entityRepository.GetAsync(input.Id.Value);
			input.MapTo(entity);

			// ObjectMapper.Map(input, entity);
		    await _entityRepository.UpdateAsync(entity);
		}



		/// <summary>
		/// 删除CustomerContact信息的方法
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		
		public async Task DeleteAsync(EntityDto<int> input)
		{
			//TODO:删除前的逻辑判断，是否允许删除
			await _entityRepository.DeleteAsync(input.Id);
		}



		/// <summary>
		/// 批量删除CustomerContact的方法
		/// </summary>
		
		public async Task BatchDeleteAsync(List<int> input)
		{
			// TODO:批量删除前的逻辑判断，是否允许删除
			await _entityRepository.DeleteAsync(s => input.Contains(s.Id));
		}

        public async Task<List<DropDownDto>> GetContactByCustomerIdAsync(int customerId)
        {
            var DropDownDtoList = await _entityRepository.GetAll()
                .Where(aa=>aa.CustomerId==customerId)
               .OrderByDescending(a => a.CreationTime).Distinct()
               .Select(c => new DropDownDto()
               {
                   Text = c.Name+"("+c.DeptName+")",
                   Value = c.Id.ToString()
               }).AsNoTracking().ToListAsync();
            return DropDownDtoList;
        }

        /// <summary>
        /// 导出CustomerContact为excel表,等待开发。
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


