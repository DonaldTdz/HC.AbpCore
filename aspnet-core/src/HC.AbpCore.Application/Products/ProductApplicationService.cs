
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


using HC.AbpCore.Products;
using HC.AbpCore.Products.Dtos;
using HC.AbpCore.Products.DomainService;
using HC.AbpCore.Dtos;
using Abp.Runtime.Session;
using HC.AbpCore.Authorization.Users;

namespace HC.AbpCore.Products
{
    /// <summary>
    /// Product应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize]
    public class ProductAppService : AbpCoreAppServiceBase, IProductAppService
    {
        private readonly IRepository<Product, int> _entityRepository;
        //private readonly IAbpSession _abpSession;
        //private readonly UserManager _userManager;
        private readonly IProductManager _entityManager;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public ProductAppService(
        IRepository<Product, int> entityRepository
        //, IAbpSession abpSession
        //, UserManager userManager
        , IProductManager entityManager
        )
        {
            //_userManager = userManager;
            //_abpSession = abpSession;
            _entityRepository = entityRepository;
            _entityManager = entityManager;
        }


        /// <summary>
        /// 获取Product的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task<PagedResultDto<ProductListDto>> GetPagedAsync(GetProductsInput input)
        {

            var query = _entityRepository.GetAll().WhereIf(!string.IsNullOrEmpty(input.Name), u => u.Name.Contains(input.Name))
                .WhereIf(input.Type.HasValue, a => a.Type == input.Type).WhereIf(input.IsEnabled.HasValue, a => a.IsEnabled == input.IsEnabled.Value);
            // TODO:根据传入的参数添加过滤条件
            //var user = await _userManager.GetUserByIdAsync(_abpSession.UserId.Value);
            //if (user.EmployeeId != "0205151055692871" && user.EmployeeId != "192656451022556048")
            //    query = query.Where(aa => aa.cr == _abpSession.UserId);


            var count = await query.CountAsync();

            var entityList = await query
                    .OrderBy(input.Sorting).AsNoTracking()
                     .OrderByDescending(a => a.CreationTime)
                    .PageBy(input)
                    .ToListAsync();

            // var entityListDtos = ObjectMapper.Map<List<ProductListDto>>(entityList);
            var entityListDtos = entityList.MapTo<List<ProductListDto>>();

            return new PagedResultDto<ProductListDto>(count, entityListDtos);
        }


        /// <summary>
        /// 通过指定id获取ProductListDto信息
        /// </summary>

        public async Task<ProductListDto> GetByIdAsync(EntityDto<int> input)
        {
            var entity = await _entityRepository.GetAsync(input.Id);

            return entity.MapTo<ProductListDto>();
        }

        /// <summary>
        /// 获取编辑 Product
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task<GetProductForEditOutput> GetForEditAsync(NullableIdDto<int> input)
        {
            var output = new GetProductForEditOutput();
            ProductEditDto editDto;

            if (input.Id.HasValue)
            {
                var entity = await _entityRepository.GetAsync(input.Id.Value);

                editDto = entity.MapTo<ProductEditDto>();

                //productEditDto = ObjectMapper.Map<List<productEditDto>>(entity);
            }
            else
            {
                editDto = new ProductEditDto();
            }

            output.Product = editDto;
            return output;
        }


        /// <summary>
        /// 添加或者修改Product的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task<APIResultDto> CreateOrUpdateAsync(CreateOrUpdateProductInput input)
        {
            input.Product.Name = input.Product.Name.Trim();
            input.Product.Specification = input.Product.Specification.Trim();
            if (input.Product.Id.HasValue)
            {
                return await UpdateAsync(input.Product);
            }
            else
            {
                return await CreateAsync(input.Product);
            }
        }


        /// <summary>
        /// 新增Product
        /// </summary>

        protected virtual async Task<APIResultDto> CreateAsync(ProductEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增
            var product = await _entityRepository.FirstOrDefaultAsync(aa => aa.Name == input.Name && aa.Specification == input.Specification
             && aa.Price == input.Price && aa.TaxRate == input.TaxRate);
            //有则修改,无则更新
            if (product != null)
            {
                if (product.Num.HasValue)
                    product.Num += input.Num;
                else
                    product.Num = input.Num;
                product = await _entityRepository.UpdateAsync(product);
                if (product != null)
                    return new APIResultDto() { Code = 1, Msg = "保存成功" };
                else
                    return new APIResultDto() { Code = 0, Msg = "保存失败" };
            }
            else
            {
                var entity = input.MapTo<Product>();
                entity.IsEnabled = true;
                entity = await _entityRepository.InsertAsync(entity);
                if (entity != null)
                    return new APIResultDto() { Code = 1, Msg = "保存成功" };
                else
                    return new APIResultDto() { Code = 0, Msg = "保存失败" };
            }
        }

        /// <summary>
        /// 编辑Product
        /// </summary>

        protected virtual async Task<APIResultDto> UpdateAsync(ProductEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新
            var entity = await _entityRepository.GetAsync(input.Id.Value);
            if (entity.Name == input.Name && entity.Specification == input.Specification&& entity.Price == input.Price && entity.TaxRate == input.TaxRate)
            {
                entity.Num = input.Num;
                entity = await _entityRepository.UpdateAsync(entity);
                if (entity != null)
                    return new APIResultDto() { Code = 1, Msg = "保存成功" };
                else
                    return new APIResultDto() { Code = 0, Msg = "保存失败" };
            }
            else
            {
                var productCount = await _entityRepository.GetAll().Where(aa => aa.Name == input.Name && aa.Specification == input.Specification
      && aa.Price == input.Price && aa.TaxRate == input.TaxRate).CountAsync();
                if (productCount > 0)
                {
                    return new APIResultDto() { Code = 1, Msg = "已经存在完全相同的产品" };
                }
                else
                {
                    input.MapTo(entity);
                    entity = await _entityRepository.UpdateAsync(entity);
                    if (entity != null)
                        return new APIResultDto() { Code = 1, Msg = "保存成功" };
                    else
                        return new APIResultDto() { Code = 0, Msg = "保存失败" };
                }
            }

            //    input.MapTo(entity);

            //// ObjectMapper.Map(input, entity);
            //entity = await _entityRepository.UpdateAsync(entity);

            //if (entity != null)
            //    return new APIResultDto() { Code = 1, Msg = "保存成功" };
            //else
            //    return new APIResultDto() { Code = 0, Msg = "保存失败" };
        }



        /// <summary>
        /// 删除Product信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task DeleteAsync(EntityDto<int> input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _entityRepository.DeleteAsync(input.Id);
        }



        /// <summary>
        /// 批量删除Product的方法
        /// </summary>

        public async Task BatchDeleteAsync(List<int> input)
        {
            // TODO:批量删除前的逻辑判断，是否允许删除
            await _entityRepository.DeleteAsync(s => input.Contains(s.Id));
        }


        /// <summary>
        /// 导出Product为excel表,等待开发。
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


