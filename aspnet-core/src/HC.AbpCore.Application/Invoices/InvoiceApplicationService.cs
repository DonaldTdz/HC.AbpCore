
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


using HC.AbpCore.Invoices;
using HC.AbpCore.Invoices.Dtos;
using HC.AbpCore.Invoices.DomainService;
using HC.AbpCore.Projects;
using HC.AbpCore.Purchases;
using HC.AbpCore.Customers;

namespace HC.AbpCore.Invoices
{
    /// <summary>
    /// Invoice应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize]
    public class InvoiceAppService : AbpCoreAppServiceBase, IInvoiceAppService
    {
        private readonly IRepository<Invoice, Guid> _entityRepository;
        private readonly IRepository<Project, Guid> _projectRepository;
        private readonly IRepository<Purchase, Guid> _purchaseRepository;
        private readonly IRepository<Customer, int> _customerRepository;
        private readonly IInvoiceManager _entityManager;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public InvoiceAppService(
        IRepository<Invoice, Guid> entityRepository
        , IRepository<Project, Guid> projectRepository
        , IRepository<Purchase, Guid> purchaseRepository
        , IRepository<Customer, int> customerRepository
        , IInvoiceManager entityManager
        )
        {
            _customerRepository = customerRepository;
            _projectRepository = projectRepository;
            _purchaseRepository = purchaseRepository;
            _entityRepository = entityRepository;
            _entityManager = entityManager;
        }


        /// <summary>
        /// 获取Invoice的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task<PagedResultDto<InvoiceListDto>> GetPagedAsync(GetInvoicesInput input)
        {

            var query = _entityRepository.GetAll().WhereIf(!String.IsNullOrEmpty(input.Title), aa => aa.Title.Contains(input.Title))
                .WhereIf(!String.IsNullOrEmpty(input.Code), aa => aa.Code.Contains(input.Code))
                .WhereIf(input.Type.HasValue, aa => aa.Type == input.Type)
                .WhereIf(input.RefId.HasValue, aa => aa.RefId == input.RefId.Value);
            // TODO:根据传入的参数添加过滤条件
            var projects = await _projectRepository.GetAll().Select(aa => new { Id = aa.Id, Name = aa.Name,Code=aa.ProjectCode }).AsNoTracking().ToListAsync();
            var purchases = await _purchaseRepository.GetAll().Select(aa => new { aa.Id, aa.ProjectId,Code=aa.Code }).AsNoTracking().ToListAsync();

            var count = await query.CountAsync();

            var entityList = await query
                    .OrderBy(input.Sorting).AsNoTracking()
                    .OrderByDescending(aa => aa.SubmitDate)
                    .PageBy(input)
                    .ToListAsync();

            // var entityListDtos = ObjectMapper.Map<List<InvoiceListDto>>(entityList);
            List<InvoiceListDto> InvoiceListDtos = new List<InvoiceListDto>();
            foreach (var item in entityList)
            {
                var InvoiceListDto = item.MapTo<InvoiceListDto>();
                if (InvoiceListDto.RefId.HasValue)
                {
                    if (InvoiceListDto.Type == InvoiceTypeEnum.销项)
                    {
                        var project = projects.Where(aa => aa.Id == InvoiceListDto.RefId).FirstOrDefault();
                        InvoiceListDto.RefName = project.Name + "(" + project.Code + ")";
                    }
                    else
                    {
                        var purchase = purchases.Where(aa => aa.Id == InvoiceListDto.RefId).FirstOrDefault();
                        if (purchase.ProjectId.HasValue)
                        {
                            var project = projects.Where(aa => aa.Id == purchase.ProjectId.Value).FirstOrDefault();
                            InvoiceListDto.RefName = project.Name + "(" + purchase.Code + ")";
                        }
                        else
                        {
                            InvoiceListDto.RefName = null;
                        }
                    }

                    InvoiceListDtos.Add(InvoiceListDto);
                }
            }

            return new PagedResultDto<InvoiceListDto>(count, InvoiceListDtos);
        }


        /// <summary>
        /// 通过指定id获取InvoiceListDto信息
        /// </summary>

        public async Task<InvoiceListDto> GetByIdAsync(EntityDto<Guid> input)
        {
            var entity = await _entityRepository.GetAsync(input.Id);
            var item = entity.MapTo<InvoiceListDto>();
            if (item.Type == InvoiceTypeEnum.销项 && item.RefId.HasValue)
                item.RefName = (await _projectRepository.GetAsync(item.RefId.Value)).Name;
            if (item.Type == InvoiceTypeEnum.进项 && item.RefId.HasValue)
            {
                var projectId = (await _purchaseRepository.GetAsync(item.RefId.Value)).ProjectId;
                if (projectId.HasValue)
                    item.RefName = (await _projectRepository.GetAsync(projectId.Value)).Name;
            }
            return item;
        }

        /// <summary>
        /// 获取编辑 Invoice
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task<GetInvoiceForEditOutput> GetForEditAsync(NullableIdDto<Guid> input)
        {
            var output = new GetInvoiceForEditOutput();
            InvoiceEditDto editDto;

            if (input.Id.HasValue)
            {
                var entity = await _entityRepository.GetAsync(input.Id.Value);

                editDto = entity.MapTo<InvoiceEditDto>();

                //invoiceEditDto = ObjectMapper.Map<List<invoiceEditDto>>(entity);
            }
            else
            {
                editDto = new InvoiceEditDto();
            }

            output.Invoice = editDto;
            return output;
        }


        /// <summary>
        /// 添加或者修改Invoice的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task<InvoiceEditDto> CreateOrUpdateAsync(CreateOrUpdateInvoiceInput input)
        {

            if (input.Invoice.Id.HasValue)
            {
                return await UpdateAsync(input.Invoice);
            }
            else
            {
               return await CreateAsync(input.Invoice);
            }
        }


        /// <summary>
        /// 新增Invoice
        /// </summary>

        protected virtual async Task<InvoiceEditDto> CreateAsync(InvoiceEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增

            // var entity = ObjectMapper.Map <Invoice>(input);
            var entity = input.MapTo<Invoice>();


            entity = await _entityRepository.InsertAsync(entity);
            return entity.MapTo<InvoiceEditDto>();
        }

        /// <summary>
        /// 编辑Invoice
        /// </summary>

        protected virtual async Task<InvoiceEditDto> UpdateAsync(InvoiceEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新

            var entity = await _entityRepository.GetAsync(input.Id.Value);
            input.MapTo(entity);

            // ObjectMapper.Map(input, entity);
            var item=await _entityRepository.UpdateAsync(entity);
            return item.MapTo<InvoiceEditDto>();
        }



        /// <summary>
        /// 删除Invoice信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task DeleteAsync(EntityDto<Guid> input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _entityRepository.DeleteAsync(input.Id);
        }



        /// <summary>
        /// 批量删除Invoice的方法
        /// </summary>

        public async Task BatchDeleteAsync(List<Guid> input)
        {
            // TODO:批量删除前的逻辑判断，是否允许删除
            await _entityRepository.DeleteAsync(s => input.Contains(s.Id));
        }

        /// <summary>
        /// 获取发票抬头
        /// </summary>
        /// <param name="type"></param>
        /// <param name="refId"></param>
        /// <returns></returns>
        public async Task<string> GetTitleByTypeAndRefIdAsync(InvoiceTypeEnum type, Guid refId)
        {
            string title = null;
            int? customerId;
            if (type == InvoiceTypeEnum.销项)
            {
                customerId = (await _projectRepository.GetAsync(refId)).CustomerId;
            }
            else
            {
                Guid? projectId = (await _purchaseRepository.GetAsync(refId)).ProjectId;
                if (projectId.HasValue)
                    customerId = (await _projectRepository.GetAsync(projectId.Value)).CustomerId;
                else
                    return title;
            }
            if (customerId.HasValue)
                return title = (await _customerRepository.GetAsync(customerId.Value)).Name;
            else
                return title;
        }

        /// <summary>
        /// 导出Invoice为excel表,等待开发。
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


