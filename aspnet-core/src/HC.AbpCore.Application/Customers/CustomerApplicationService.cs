
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


using HC.AbpCore.Customers;
using HC.AbpCore.Customers.Dtos;
using HC.AbpCore.Customers.DomainService;
using HC.AbpCore.Dtos;

namespace HC.AbpCore.Customers
{
    /// <summary>
    /// Customer应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize]
    public class CustomerAppService : AbpCoreAppServiceBase, ICustomerAppService
    {
        private readonly IRepository<Customer> _entityRepository;

        private readonly ICustomerManager _entityManager;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public CustomerAppService(
        IRepository<Customer> entityRepository
        , ICustomerManager entityManager
        )
        {
            _entityRepository = entityRepository;
            _entityManager = entityManager;
        }


        /// <summary>
        /// 获取Customer的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<CustomerListDto>> GetPagedAsync(GetCustomersInput input)
        {
            /*var query = _entityRepository.GetAll()
                  .WhereIf(!string.IsNullOrEmpty(input.FilterText), u => u.UserName.Contains(input.FilterText)
                  || u.Phone.Contains(input.FilterText))
                  .WhereIf(input.Status.HasValue, v => v.UserType == input.Status.Value);
                  */

            var query = _entityRepository.GetAll()
                .WhereIf(!string.IsNullOrEmpty(input.name), u => u.Name.Contains(input.name))
                .WhereIf(input.type.HasValue, v => v.Type == input.type.Value);
            // TODO:根据传入的参数添加过滤条件


            var count = await query.CountAsync();

            var entityList = await query

               .OrderBy(input.Sorting).AsNoTracking()
               .PageBy(input)
               .ToListAsync();
            var entityListDtos = entityList.MapTo<List<CustomerListDto>>();

            return new PagedResultDto<CustomerListDto>(count, entityListDtos);
        }

        /// <summary>
        /// 通过指定id获取CustomerListDto信息
        /// </summary>
        public async Task<CustomerListDto> GetByIdAsync(EntityDto<int> input)
        {
            var entity = await _entityRepository.GetAsync(input.Id);

            return entity.MapTo<CustomerListDto>();
        }

        /// <summary>
        /// 获取编辑 Customer
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<GetCustomerForEditOutput> GetForEditAsync(NullableIdDto<int> input)
        {
            var output = new GetCustomerForEditOutput();
            CustomerEditDto editDto;

            if (input.Id.HasValue)
            {
                var entity = await _entityRepository.GetAsync(input.Id.Value);

                editDto = entity.MapTo<CustomerEditDto>();

                //customerEditDto = ObjectMapper.Map<List<customerEditDto>>(entity);
            }
            else
            {
                editDto = new CustomerEditDto();
            }

            output.Customer = editDto;
            return output;
        }


        /// <summary>
        /// 添加或者修改Customer的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<APIResultDto> CreateOrUpdateAsync(CustomerEditDto input)
        {
            input.Name = input.Name.Trim();
            if (input.Id.HasValue)
            {
                return await UpdateAsync(input);
            }
            else
            {
                return await CreateAsync(input);
            }
        }


        /// <summary>
        /// 新增Customer
        /// </summary>
        protected virtual async Task<APIResultDto> CreateAsync(CustomerEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增
            int customerCount = await _entityRepository.GetAll().Where(aa => aa.Name == input.Name).CountAsync();
            if (customerCount > 0)
                return new APIResultDto() { Code = 0, Msg = "该客户名称已存在" };


            // var entity = ObjectMapper.Map <Customer>(input);
            var entity = input.MapTo<Customer>();


            entity = await _entityRepository.InsertAsync(entity);
            if (entity != null)
                return new APIResultDto() { Code = 1, Msg = "保存成功" };
            else
                return new APIResultDto() { Code = 0, Msg = "保存失败" };
        }

        /// <summary>
        /// 编辑Customer
        /// </summary>
        protected virtual async Task<APIResultDto> UpdateAsync(CustomerEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新

            var entity = await _entityRepository.GetAsync(input.Id.Value);
            if (entity.Name != input.Name)
            {
                int customerCount = await _entityRepository.GetAll().Where(aa => aa.Name == input.Name).CountAsync();
                if (customerCount > 0)
                    return new APIResultDto() { Code = 0, Msg = "该客户名称已存在" };
            }

            input.MapTo(entity);

            // ObjectMapper.Map(input, entity);
            entity = await _entityRepository.UpdateAsync(entity);
            if (entity != null)
                return new APIResultDto() { Code = 1, Msg = "保存成功" };
            else
                return new APIResultDto() { Code = 0, Msg = "保存失败" };
        }



        /// <summary>
        /// 删除Customer信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task DeleteAsync(EntityDto<int> input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _entityRepository.DeleteAsync(input.Id);

        }



        /// <summary>
        /// 批量删除Customer的方法
        /// </summary>
        public async Task BatchDelete(List<int> input)
        {
            // TODO:批量删除前的逻辑判断，是否允许删除
            await _entityRepository.DeleteAsync(s => input.Contains(s.Id));
        }

        /// <summary>
        /// 获取第一条图文消息
        /// </summary>
        public async Task<CustomerListDto> GetCustomerSubscribeInfo()
        {
            var entity = await _entityRepository.GetAll().FirstOrDefaultAsync();
            return entity.MapTo<CustomerListDto>();
        }

        public async Task<List<DropDownDto>> GetDropDownDtosAsync()
        {
            var DropDownDtoList = await _entityRepository.GetAll().OrderBy(a => a.CreationTime).AsNoTracking()
                .Select(c => new DropDownDto()
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }).ToListAsync();
            return DropDownDtoList;
        }

        /// <summary>
        /// 导出Customer为excel表,等待开发。
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


