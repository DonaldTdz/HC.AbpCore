
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


using HC.AbpCore.AdvancePayments.AdvancePaymentDetails;
using HC.AbpCore.AdvancePayments.AdvancePaymentDetails.Dtos;
using HC.AbpCore.AdvancePayments.AdvancePaymentDetails.DomainService;
using HC.AbpCore.Purchases;
using HC.AbpCore.Purchases.PurchaseDetails;
using HC.AbpCore.Products;

namespace HC.AbpCore.AdvancePayments.AdvancePaymentDetails
{
    /// <summary>
    /// AdvancePaymentDetail应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize]
    public class AdvancePaymentDetailAppService : AbpCoreAppServiceBase, IAdvancePaymentDetailAppService
    {
        private readonly IRepository<AdvancePaymentDetail, Guid> _entityRepository;
        private readonly IRepository<PurchaseDetail, Guid> _purchaseDetailRepository;
        private readonly IRepository<Product, int> _productRepository;
        private readonly IAdvancePaymentDetailManager _entityManager;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public AdvancePaymentDetailAppService(
        IRepository<AdvancePaymentDetail, Guid> entityRepository
        , IRepository<PurchaseDetail, Guid> purchaseDetailRepository
        , IRepository<Product, int> productRepository
        , IAdvancePaymentDetailManager entityManager
        )
        {
            _productRepository = productRepository;
            _purchaseDetailRepository = purchaseDetailRepository;
            _entityRepository = entityRepository;
            _entityManager = entityManager;
        }


        /// <summary>
        /// 获取AdvancePaymentDetail的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task<PagedResultDto<AdvancePaymentDetailListDto>> GetPagedAsync(GetAdvancePaymentDetailsInput input)
        {

            var query = _entityRepository.GetAll()
                .WhereIf(input.AdvancePaymentId.HasValue, aa => aa.AdvancePaymentId == input.AdvancePaymentId.Value);
            // TODO:根据传入的参数添加过滤条件
            var purchaseDetails = _purchaseDetailRepository.GetAll();
            var products = _productRepository.GetAll();
            var items = from item in query
                        join purchaseDetail in purchaseDetails on item.PurchaseDetailId equals purchaseDetail.Id
                        join product in products on purchaseDetail.ProductId equals product.Id into aa
                        from bb in aa.DefaultIfEmpty()
                        select new AdvancePaymentDetailListDto()
                        {
                            Id=item.Id,
                            AdvancePaymentId=item.AdvancePaymentId,
                            PurchaseDetailId=item.PurchaseDetailId,
                            Ratio=item.Ratio,
                            Amount=item.Amount,
                            CreationTime=item.CreationTime,
                            PurchaseDetailName=bb.Name+"("+bb.Specification+")"
                        };

            var count = await items.CountAsync();

            var entityList = await items
                    .OrderBy(input.Sorting).AsNoTracking()
                    .PageBy(input)
                    .ToListAsync();

            // var entityListDtos = ObjectMapper.Map<List<AdvancePaymentDetailListDto>>(entityList);
            //var entityListDtos = entityList.MapTo<List<AdvancePaymentDetailListDto>>();

            return new PagedResultDto<AdvancePaymentDetailListDto>(count, entityList);
        }


        /// <summary>
        /// 通过指定id获取AdvancePaymentDetailListDto信息
        /// </summary>

        public async Task<AdvancePaymentDetailListDto> GetByIdAsync(EntityDto<Guid> input)
        {
            var entity = await _entityRepository.GetAsync(input.Id);

            return entity.MapTo<AdvancePaymentDetailListDto>();
        }

        /// <summary>
        /// 获取编辑 AdvancePaymentDetail
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task<GetAdvancePaymentDetailForEditOutput> GetForEditAsync(NullableIdDto<Guid> input)
        {
            var output = new GetAdvancePaymentDetailForEditOutput();
            AdvancePaymentDetailEditDto editDto;

            if (input.Id.HasValue)
            {
                var entity = await _entityRepository.GetAsync(input.Id.Value);

                editDto = entity.MapTo<AdvancePaymentDetailEditDto>();

                //advancePaymentDetailEditDto = ObjectMapper.Map<List<advancePaymentDetailEditDto>>(entity);
            }
            else
            {
                editDto = new AdvancePaymentDetailEditDto();
            }

            output.AdvancePaymentDetail = editDto;
            return output;
        }


        /// <summary>
        /// 添加或者修改AdvancePaymentDetail的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task CreateOrUpdateAsync(CreateOrUpdateAdvancePaymentDetailInput input)
        {

            if (input.AdvancePaymentDetail.Id.HasValue)
            {
                await Update(input.AdvancePaymentDetail);
            }
            else
            {
                await Create(input.AdvancePaymentDetail);
            }
        }


        /// <summary>
        /// 新增AdvancePaymentDetail
        /// </summary>

        protected virtual async Task<AdvancePaymentDetailEditDto> Create(AdvancePaymentDetailEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增

            // var entity = ObjectMapper.Map <AdvancePaymentDetail>(input);
            var entity = input.MapTo<AdvancePaymentDetail>();


            entity = await _entityRepository.InsertAsync(entity);
            return entity.MapTo<AdvancePaymentDetailEditDto>();
        }

        /// <summary>
        /// 编辑AdvancePaymentDetail
        /// </summary>

        protected virtual async Task Update(AdvancePaymentDetailEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新

            var entity = await _entityRepository.GetAsync(input.Id.Value);
            input.MapTo(entity);

            // ObjectMapper.Map(input, entity);
            await _entityRepository.UpdateAsync(entity);
        }



        /// <summary>
        /// 删除AdvancePaymentDetail信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task DeleteAsync(EntityDto<Guid> input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _entityRepository.DeleteAsync(input.Id);
        }



        /// <summary>
        /// 批量删除AdvancePaymentDetail的方法
        /// </summary>

        public async Task BatchDeleteAsync(List<Guid> input)
        {
            // TODO:批量删除前的逻辑判断，是否允许删除
            await _entityRepository.DeleteAsync(s => input.Contains(s.Id));
        }


        /// <summary>
        /// 导出AdvancePaymentDetail为excel表,等待开发。
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


