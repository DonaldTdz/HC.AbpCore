
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


using HC.AbpCore.Purchases.PurchaseDetails;
using HC.AbpCore.Purchases.PurchaseDetails.Dtos;
using HC.AbpCore.Purchases.PurchaseDetails.DomainService;
using HC.AbpCore.Products;
using HC.AbpCore.Suppliers;

namespace HC.AbpCore.Purchases.PurchaseDetails
{
    /// <summary>
    /// PurchaseDetail应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize]
    public class PurchaseDetailAppService : AbpCoreAppServiceBase, IPurchaseDetailAppService
    {
        private readonly IRepository<PurchaseDetail, Guid> _entityRepository;
        private readonly IRepository<Supplier, int> _supplierRepository;
        private readonly IRepository<Product, int> _productIdRepository;
        private readonly IPurchaseDetailManager _entityManager;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public PurchaseDetailAppService(
        IRepository<PurchaseDetail, Guid> entityRepository
        , IRepository<Supplier, int> supplierRepository
        , IRepository<Product, int> productIdRepository
        , IPurchaseDetailManager entityManager
        )
        {
            _productIdRepository = productIdRepository;
            _supplierRepository = supplierRepository;
            _entityRepository = entityRepository;
            _entityManager = entityManager;
        }


        /// <summary>
        /// 获取PurchaseDetail的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task<PagedResultDto<PurchaseDetailListDtoNew>> GetPagedAsync(GetPurchaseDetailsInput input)
        {

            var query = _entityRepository.GetAll().WhereIf(input.PurchaseId.HasValue, aa => aa.PurchaseId == input.PurchaseId.Value);
            // TODO:根据传入的参数添加过滤条件
            var suppliers = _supplierRepository.GetAll().AsNoTracking();
            var products = _productIdRepository.GetAll().AsNoTracking();

            var items = from item in query
                        join supplier in suppliers on item.SupplierId equals supplier.Id into temp
                        join product in products on item.ProductId equals product.Id into bbs
                        from tem in temp.DefaultIfEmpty()
                        from bb in bbs.DefaultIfEmpty()
                        select new PurchaseDetailListDtoNew()
                        {
                            Id = item.Id,
                            PurchaseId = item.PurchaseId,
                            SupplierId = item.SupplierId,
                            ProductId = item.ProductId,
                            Num = item.Num,
                            Name = bb.Name,
                            Specification = bb.Specification,
                            TaxRate = bb.TaxRate,
                            Price = bb.Price,
                            SupplierName = tem.Name
                        };

            var count = await items.CountAsync();

            var entityList = await items
                    .OrderBy(input.Sorting).AsNoTracking()
                    //.PageBy(input)
                    .ToListAsync();

            // var entityListDtos = ObjectMapper.Map<List<PurchaseDetailListDto>>(entityList);
            //var entityListDtos = entityList.MapTo<List<PurchaseDetailListDto>>();

            return new PagedResultDto<PurchaseDetailListDtoNew>(count, entityList);
        }


        /// <summary>
        /// 通过指定id获取PurchaseDetailListDto信息
        /// </summary>

        public async Task<PurchaseDetailListDto> GetByIdAsync(EntityDto<Guid> input)
        {
            var entity = await _entityRepository.GetAsync(input.Id);

            return entity.MapTo<PurchaseDetailListDto>();
        }

        /// <summary>
        /// 获取编辑 PurchaseDetail
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task<GetPurchaseDetailForEditOutput> GetForEditAsync(NullableIdDto<Guid> input)
        {
            var output = new GetPurchaseDetailForEditOutput();
            PurchaseDetailEditDto editDto;

            if (input.Id.HasValue)
            {
                var entity = await _entityRepository.GetAsync(input.Id.Value);

                editDto = entity.MapTo<PurchaseDetailEditDto>();

                //purchaseDetailEditDto = ObjectMapper.Map<List<purchaseDetailEditDto>>(entity);
            }
            else
            {
                editDto = new PurchaseDetailEditDto();
            }

            output.PurchaseDetail = editDto;
            return output;
        }


        /// <summary>
        /// 添加或者修改PurchaseDetail的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task CreateOrUpdateAsync(CreateOrUpdatePurchaseDetailInput input)
        {

            if (input.PurchaseDetail.Id.HasValue)
            {
                await Update(input.PurchaseDetail);
            }
            else
            {
                await Create(input.PurchaseDetail);
            }
        }


        /// <summary>
        /// 新增PurchaseDetail
        /// </summary>

        protected virtual async Task<PurchaseDetailEditDto> Create(PurchaseDetailEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增

            // var entity = ObjectMapper.Map <PurchaseDetail>(input);
            var entity = input.MapTo<PurchaseDetail>();


            entity = await _entityRepository.InsertAsync(entity);
            return entity.MapTo<PurchaseDetailEditDto>();
        }

        /// <summary>
        /// 编辑PurchaseDetail
        /// </summary>

        protected virtual async Task Update(PurchaseDetailEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新

            var entity = await _entityRepository.GetAsync(input.Id.Value);
            input.MapTo(entity);

            // ObjectMapper.Map(input, entity);
            await _entityRepository.UpdateAsync(entity);
        }



        /// <summary>
        /// 删除PurchaseDetail信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task DeleteAsync(EntityDto<Guid> input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _entityRepository.DeleteAsync(input.Id);
        }



        /// <summary>
        /// 批量删除PurchaseDetail的方法
        /// </summary>

        public async Task BatchDeleteAsync(List<Guid> input)
        {
            // TODO:批量删除前的逻辑判断，是否允许删除
            await _entityRepository.DeleteAsync(s => input.Contains(s.Id));
        }

        /// <summary>
        /// 添加PurchaseDetail并且更新Products的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CreateDetailAndUpdateproductAsync(CreatePurchaseDetailAndUpdateproductInput input)
        {
            var entity = input.PurchaseDetail.MapTo<PurchaseDetailNew>();
            await _entityManager.CreateAsync(entity);
        }

        /// <summary>
        /// 删除PurchaseDetail并更新Products的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task DeleteAndUpdatePurchaseAsync(EntityDto<Guid> input)
        {
            await _entityManager.DeleteAsync(input.Id);
        }

        /// <summary>
        /// 更新PurchaseDetail并且更新Products的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task UpdateDetailAndUpdateproductAsync(CreateOrUpdatePurchaseDetailInput input)
        {
            var entity = input.PurchaseDetail.MapTo<PurchaseDetail>();
            await _entityManager.UpdateAsync(entity);
        }

        /// <summary>
        /// 导出PurchaseDetail为excel表,等待开发。
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


