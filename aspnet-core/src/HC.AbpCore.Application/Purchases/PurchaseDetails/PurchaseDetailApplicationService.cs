
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
using HC.AbpCore.Suppliers;
using HC.AbpCore.Projects.ProjectDetails;
using HC.AbpCore.Dtos;
using HC.AbpCore.Projects;

namespace HC.AbpCore.Purchases.PurchaseDetails
{
    /// <summary>
    /// PurchaseDetail应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize]
    public class PurchaseDetailAppService : AbpCoreAppServiceBase, IPurchaseDetailAppService
    {
        private readonly IRepository<PurchaseDetail, Guid> _entityRepository;
        private readonly IRepository<Purchase, Guid> _purchaseRepository;
        private readonly IRepository<Supplier, int> _supplierRepository;
        private readonly IRepository<ProjectDetail, Guid> _projectDetailRepository;
        private readonly IPurchaseDetailManager _entityManager;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public PurchaseDetailAppService(
        IRepository<PurchaseDetail, Guid> entityRepository,
                IRepository<Supplier, int> supplierRepository
            , IRepository<ProjectDetail, Guid> projectDetailRepository
            , IRepository<Purchase, Guid> purchaseRepository
        , IPurchaseDetailManager entityManager
        )
        {
            _projectDetailRepository = projectDetailRepository;
            _purchaseRepository = purchaseRepository;
            _supplierRepository = supplierRepository;
            _entityRepository = entityRepository;
            _entityManager = entityManager;
        }


        /// <summary>
        /// 获取PurchaseDetail的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task<PagedResultDto<PurchaseDetailListDto>> GetPagedAsync(GetPurchaseDetailsInput input)
        {

            var query = _entityRepository.GetAll().WhereIf(input.PurchaseId.HasValue, aa => aa.PurchaseId == input.PurchaseId.Value)
                .WhereIf(input.SupplierId.HasValue, aa => aa.SupplierId == input.SupplierId.Value);
            // TODO:根据传入的参数添加过滤条件
            var suppliers = await _supplierRepository.GetAll().OrderByDescending(aa => aa.CreationTime).AsNoTracking().ToListAsync();
            var projectDetails = await _projectDetailRepository.GetAll().OrderByDescending(aa => aa.CreationTime).AsNoTracking().ToListAsync();


            var count = await query.CountAsync();

            var entityList = await query
                    .OrderBy(input.Sorting).AsNoTracking()
                    .OrderByDescending(aa => aa.CreationTime)
                    .Select(bb => new PurchaseDetailListDto()
                    {
                        Id = bb.Id,
                        PurchaseId = bb.PurchaseId,
                        SupplierId = bb.SupplierId,
                        Price = bb.Price,
                        ProjectDetailId = bb.ProjectDetailId,
                        SupplierName = bb.SupplierId.HasValue ? suppliers.Where(aa => aa.Id == bb.SupplierId.Value).FirstOrDefault().Name : null,
                        ProjectDetailName = bb.ProjectDetailId.HasValue ? projectDetails.Where(aa => aa.Id == bb.ProjectDetailId.Value).FirstOrDefault().Name : null,
                    })
                    .PageBy(input)
                    .ToListAsync();

            // var entityListDtos = ObjectMapper.Map<List<PurchaseDetailListDto>>(entityList);

            return new PagedResultDto<PurchaseDetailListDto>(count, entityList);
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
                await UpdateAsync(input.PurchaseDetail);
            }
            else
            {
                await CreateAsync(input.PurchaseDetail);
            }
        }


        /// <summary>
        /// 新增PurchaseDetail
        /// </summary>

        protected virtual async Task<PurchaseDetailEditDto> CreateAsync(PurchaseDetailEditDto input)
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

        protected virtual async Task UpdateAsync(PurchaseDetailEditDto input)
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
        /// 根据采购id获取采购明细下拉列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<DropDownDto>> GetDropDownsByPurchaseIdAsync(Guid purchaseId)
        {
            var projectId = _purchaseRepository.Get(purchaseId).ProjectId;
            var projectDetails = await _projectDetailRepository.GetAll().Where(aa => aa.ProjectId == projectId).AsNoTracking().ToListAsync();
            var query = _entityRepository.GetAll();
            var entityList = await query
                    .OrderBy(a => a.CreationTime).AsNoTracking()
                    .Where(aa => aa.PurchaseId == purchaseId)
                    .Select(c => new DropDownDto() { Text = projectDetails.Where(aa=>aa.Id==c.ProjectDetailId).FirstOrDefault().Name, Value = c.Id.ToString() })
                    .ToListAsync();
            return entityList;
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


