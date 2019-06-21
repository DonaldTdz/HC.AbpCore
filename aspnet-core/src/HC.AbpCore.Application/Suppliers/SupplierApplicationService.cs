
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


using HC.AbpCore.Suppliers;
using HC.AbpCore.Suppliers.Dtos;
using HC.AbpCore.Suppliers.DomainService;
using HC.AbpCore.Dtos;
using HC.AbpCore.Purchases;
using HC.AbpCore.Purchases.PurchaseDetails;
using HC.AbpCore.Projects;
using HC.AbpCore.Projects.ProjectDetails;

namespace HC.AbpCore.Suppliers
{
    /// <summary>
    /// Supplier应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize]
    public class SupplierAppService : AbpCoreAppServiceBase, ISupplierAppService
    {
        private readonly IRepository<Supplier, int> _entityRepository;
        private readonly IRepository<Purchase, Guid> _purchaseRepository;
        private readonly IRepository<PurchaseDetail, Guid> _purchaseDetailRepository;
        private readonly IRepository<ProjectDetail, Guid> _projectDetailRepository;
        private readonly ISupplierManager _entityManager;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public SupplierAppService(
        IRepository<Supplier, int> entityRepository,
                 IRepository<ProjectDetail, Guid> projectDetailRepository,
         IRepository<Purchase, Guid> purchaseRepository,
         IRepository<PurchaseDetail, Guid> purchaseDetailRepository
        , ISupplierManager entityManager
        )
        {
            _purchaseDetailRepository = purchaseDetailRepository;
            _projectDetailRepository = projectDetailRepository;
            _purchaseRepository = purchaseRepository;
            _entityRepository = entityRepository;
            _entityManager = entityManager;
        }


        /// <summary>
        /// 获取Supplier的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task<PagedResultDto<SupplierListDto>> GetPagedAsync(GetSuppliersInput input)
        {

            var query = _entityRepository.GetAll().WhereIf(!String.IsNullOrEmpty(input.Name), a => a.Name.Contains(input.Name));
            // TODO:根据传入的参数添加过滤条件


            var count = await query.CountAsync();

            var entityList = await query
                    .OrderBy(input.Sorting).AsNoTracking()
                 .OrderByDescending(a => a.CreationTime)
                    .PageBy(input)
                    .ToListAsync();

            // var entityListDtos = ObjectMapper.Map<List<SupplierListDto>>(entityList);
            var entityListDtos = entityList.MapTo<List<SupplierListDto>>();

            return new PagedResultDto<SupplierListDto>(count, entityListDtos);
        }

        /// <summary>
        /// 获取所属供应商采购产品记录查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<GetPurchaseProductListDto>> GetPurchaseProductListAsync(GetSuppliersInput input)
        {
            var projectDetails = _projectDetailRepository.GetAll().AsNoTracking();
            var purchases = _purchaseRepository.GetAll().AsNoTracking();
            var Ids = _purchaseDetailRepository.GetAll().Where(aa => aa.SupplierId == input.Id).Select(aa=>new { purchaseId =aa.PurchaseId, projectDetailId=aa.ProjectDetailId});
            var items = from item in Ids
                        join purchase in purchases on item.purchaseId equals purchase.Id
                        join projectDetail in projectDetails on item.projectDetailId equals projectDetail.Id into temp
                        from tem in temp.DefaultIfEmpty()
                        select new GetPurchaseProductListDto()
                        {
                            PurchaseId = item.purchaseId.Value,
                            PurchaseCode = purchase.Code,
                            PurchaseDate = purchase.PurchaseDate,
                            PurchaseName = tem.Name,
                            //Specification = tem.Specification
                        };
            var count = await items.CountAsync();
            var entity =await items.OrderByDescending(aa => aa.PurchaseDate)
                .PageBy(input)
                .ToListAsync();
            return new PagedResultDto<GetPurchaseProductListDto>(count, entity);
        }


        /// <summary>
        /// 通过指定id获取SupplierListDto信息
        /// </summary>

        public async Task<SupplierListDto> GetByIdAsync(EntityDto<int> input)
        {
            var entity = await _entityRepository.GetAsync(input.Id);

            return entity.MapTo<SupplierListDto>();
        }

        /// <summary>
        /// 获取编辑 Supplier
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task<GetSupplierForEditOutput> GetForEditAsync(NullableIdDto<int> input)
        {
            var output = new GetSupplierForEditOutput();
            SupplierEditDto editDto;

            if (input.Id.HasValue)
            {
                var entity = await _entityRepository.GetAsync(input.Id.Value);

                editDto = entity.MapTo<SupplierEditDto>();

                //supplierEditDto = ObjectMapper.Map<List<supplierEditDto>>(entity);
            }
            else
            {
                editDto = new SupplierEditDto();
            }

            output.Supplier = editDto;
            return output;
        }


        /// <summary>
        /// 添加或者修改Supplier的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task<APIResultDto> CreateOrUpdateAsync(CreateOrUpdateSupplierInput input)
        {
            input.Supplier.Name = input.Supplier.Name.Trim();
            if (input.Supplier.Id.HasValue)
            {
                return await UpdateAsync(input.Supplier);
            }
            else
            {
                return await CreateAsync(input.Supplier);
            }
        }


        /// <summary>
        /// 新增Supplier
        /// </summary>

        protected virtual async Task<APIResultDto> CreateAsync(SupplierEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增
            int supplierCount = await _entityRepository.GetAll().Where(aa => aa.Name == input.Name).CountAsync();
            if (supplierCount > 0)
                return new APIResultDto() { Code = 0, Msg = "该供应商名称已存在" };

            // var entity = ObjectMapper.Map <Supplier>(input);
            var entity = input.MapTo<Supplier>();


            entity = await _entityRepository.InsertAsync(entity);
            if (entity != null)
                return new APIResultDto() { Code = 1, Msg = "保存成功" };
            else
                return new APIResultDto() { Code = 0, Msg = "保存失败" };
        }

        /// <summary>
        /// 编辑Supplier
        /// </summary>

        protected virtual async Task<APIResultDto> UpdateAsync(SupplierEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新

            var entity = await _entityRepository.GetAsync(input.Id.Value);

            if (entity.Name != input.Name)
            {
                int supplierCount = await _entityRepository.GetAll().Where(aa => aa.Name == input.Name).CountAsync();
                if (supplierCount > 0)
                    return new APIResultDto() { Code = 0, Msg = "该供应商名称已存在" };
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
        /// 删除Supplier信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task DeleteAsync(EntityDto<int> input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _entityRepository.DeleteAsync(input.Id);
        }



        /// <summary>
        /// 批量删除Supplier的方法
        /// </summary>

        public async Task BatchDeleteAsync(List<int> input)
        {
            // TODO:批量删除前的逻辑判断，是否允许删除
            await _entityRepository.DeleteAsync(s => input.Contains(s.Id));
        }

        /// <summary>
        /// 获取供应商下拉列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<DropDownDto>> GetDropDownsAsync()
        {
            var items = await _entityRepository.GetAll()
                .OrderByDescending(aa => aa.CreationTime)
                .Select(aa => new DropDownDto()
                {
                    Text = aa.Name,
                    Value = aa.Id.ToString()
                }).AsNoTracking().ToListAsync();
            return items;
        }

        /// <summary>
        /// 导出Supplier为excel表,等待开发。
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


