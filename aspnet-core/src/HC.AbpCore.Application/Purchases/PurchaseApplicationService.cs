
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


using HC.AbpCore.Purchases;
using HC.AbpCore.Purchases.Dtos;
using HC.AbpCore.Purchases.DomainService;
using HC.AbpCore.Purchases.PurchaseDetails;
using HC.AbpCore.AdvancePayments;
using HC.AbpCore.DingTalk.Employees;

namespace HC.AbpCore.Purchases
{
    /// <summary>
    /// Purchase应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize]
    public class PurchaseAppService : AbpCoreAppServiceBase, IPurchaseAppService
    {
        private readonly IRepository<Purchase, Guid> _entityRepository;
        private readonly IRepository<Employee, string> _employeeRepository;

        private readonly IPurchaseManager _entityManager;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public PurchaseAppService(
        IRepository<Purchase, Guid> entityRepository
        , IRepository<Employee, string> employeeRepository
        , IPurchaseManager entityManager
        )
        {
            _employeeRepository = employeeRepository;
            _entityRepository = entityRepository;
            _entityManager = entityManager;
        }


        /// <summary>
        /// 获取Purchase的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task<PagedResultDto<PurchaseListDto>> GetPagedAsync(GetPurchasesInput input)
        {

            var query = _entityRepository.GetAll().WhereIf(!String.IsNullOrEmpty(input.Code),aa=>aa.Code.Contains(input.Code))
                .WhereIf(!String.IsNullOrEmpty(input.EmployeeId),aa=>aa.EmployeeId==input.EmployeeId);
            // TODO:根据传入的参数添加过滤条件
            var employees = _employeeRepository.GetAll().AsNoTracking();

            var items= from item in query
                       join employee in employees on item.EmployeeId equals employee.Id into temp
                       from tem in temp.DefaultIfEmpty()
                       select new PurchaseListDto()
                       {
                           Code = item.Code,
                           EmployeeId = item.EmployeeId,
                           PurchaseDate = item.PurchaseDate,
                           Desc = item.Desc,
                           ArrivalDate=item.ArrivalDate,
                           InvoiceIssuance=item.InvoiceIssuance,
                           Attachments=item.Attachments,
                           EmployeeName=tem.Name
                       };

            var count = await items.CountAsync();

            var entityList = await items
                    .OrderBy(input.Sorting).AsNoTracking()
                    .PageBy(input)
                    .ToListAsync();

            // var entityListDtos = ObjectMapper.Map<List<PurchaseListDto>>(entityList);
            //var entityListDtos = entityList.MapTo<List<PurchaseListDto>>();

            return new PagedResultDto<PurchaseListDto>(count, entityList);
        }


        /// <summary>
        /// 通过指定id获取PurchaseListDto信息
        /// </summary>

        public async Task<PurchaseListDto> GetByIdAsync(EntityDto<Guid> input)
        {
            var entity = await _entityRepository.GetAsync(input.Id);

            return entity.MapTo<PurchaseListDto>();
        }

        /// <summary>
        /// 获取编辑 Purchase
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task<GetPurchaseForEditOutput> GetForEditAsync(NullableIdDto<Guid> input)
        {
            var output = new GetPurchaseForEditOutput();
            PurchaseEditDto editDto;

            if (input.Id.HasValue)
            {
                var entity = await _entityRepository.GetAsync(input.Id.Value);

                editDto = entity.MapTo<PurchaseEditDto>();

                //purchaseEditDto = ObjectMapper.Map<List<purchaseEditDto>>(entity);
            }
            else
            {
                editDto = new PurchaseEditDto();
            }

            output.Purchase = editDto;
            return output;
        }


        /// <summary>
        /// 添加或者修改Purchase的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task CreateOrUpdateAsync(CreateOrUpdatePurchaseInput input)
        {

            if (input.Purchase.Id.HasValue)
            {
                await Update(input.Purchase);
            }
            else
            {
                await Create(input.Purchase);
            }
        }


        /// <summary>
        /// 新增Purchase
        /// </summary>

        protected virtual async Task<PurchaseEditDto> Create(PurchaseEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增

            // var entity = ObjectMapper.Map <Purchase>(input);
            var entity = input.MapTo<Purchase>();


            entity = await _entityRepository.InsertAsync(entity);
            return entity.MapTo<PurchaseEditDto>();
        }

        /// <summary>
        /// 编辑Purchase
        /// </summary>

        protected virtual async Task Update(PurchaseEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新

            var entity = await _entityRepository.GetAsync(input.Id.Value);
            input.MapTo(entity);

            // ObjectMapper.Map(input, entity);
            await _entityRepository.UpdateAsync(entity);
        }



        /// <summary>
        /// 删除Purchase信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task DeleteAsync(EntityDto<Guid> input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _entityRepository.DeleteAsync(input.Id);
        }



        /// <summary>
        /// 批量删除Purchase的方法
        /// </summary>

        public async Task BatchDeleteAsync(List<Guid> input)
        {
            // TODO:批量删除前的逻辑判断，是否允许删除
            await _entityRepository.DeleteAsync(s => input.Contains(s.Id));
        }


        /// <summary>
        /// Web一键新增采购,采购明细,产品,预付款计划
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task OnekeyCreateAsync(OnekeyCreatePurchaseInput input)
        {
            var purchase = input.Purchase.MapTo<Purchase>();
            var purchaseDetailNews = input.PurchaseDetailNews.MapTo<List<PurchaseDetailNew>>();
            var advancePayments = input.AdvancePayments.MapTo<List<AdvancePayment>>();
            await _entityManager.OnekeyCreateAsync(purchase, purchaseDetailNews, advancePayments);
        }


        /// <summary>
        /// 自动生成采购编号
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<string> GetGeneratePurchaseCodeAsync()
        {
            string purchaseCode = "";
            DateTime dt = new DateTime();
            DateTime startDate = new DateTime(dt.Year, dt.Month, 1);
            DateTime endDate = new DateTime(dt.Year, dt.Month + 1, 1);
            purchaseCode = await _entityRepository.GetAll().Where(aa=>aa.CreationTime>=startDate&&aa.CreationTime<endDate).MaxAsync(aa => aa.Code);
            if (!String.IsNullOrEmpty(purchaseCode))
            {
                var arr = purchaseCode.Split("-");
                purchaseCode = arr[0].ToString() + "-" + arr[1].ToString() + "-" + (long.Parse(arr[2]) + 1).ToString();
            }
            else
            {
                purchaseCode = "HC-C-" + DateTime.Now.ToString("yyMM") + "001";
            }
            return purchaseCode;

        }


        /// <summary>
        /// 导出Purchase为excel表,等待开发。
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


