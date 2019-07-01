
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


using HC.AbpCore.Invoices.InvoiceDetails;
using HC.AbpCore.Invoices.InvoiceDetails.Dtos;
using HC.AbpCore.Invoices.InvoiceDetails.DomainService;
using HC.AbpCore.Projects.ProjectDetails;
using HC.AbpCore.Purchases.PurchaseDetails;

namespace HC.AbpCore.Invoices.InvoiceDetails
{
    /// <summary>
    /// InvoiceDetail应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize]
    public class InvoiceDetailAppService : AbpCoreAppServiceBase, IInvoiceDetailAppService
    {
        private readonly IRepository<InvoiceDetail, Guid> _entityRepository;
        private readonly IRepository<ProjectDetail, Guid> _projectDetailRepository;
        private readonly IRepository<PurchaseDetail, Guid> _purchaseDetailRepository;
        private readonly IInvoiceDetailManager _entityManager;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public InvoiceDetailAppService(
        IRepository<InvoiceDetail, Guid> entityRepository
        , IRepository<ProjectDetail, Guid> projectDetailRepository
        , IRepository<PurchaseDetail, Guid> purchaseDetailRepository
        , IInvoiceDetailManager entityManager
        )
        {
            _projectDetailRepository = projectDetailRepository;
            _purchaseDetailRepository = purchaseDetailRepository;
            _entityRepository = entityRepository; 
             _entityManager=entityManager;
        }


        /// <summary>
        /// 获取InvoiceDetail的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
		 
        public async Task<PagedResultDto<InvoiceDetailListDto>> GetPagedAsync(GetInvoiceDetailsInput input)
		{

		    var query = _entityRepository.GetAll().WhereIf(input.InvoiceId.HasValue,aa=>aa.InvoiceId==input.InvoiceId);
            // TODO:根据传入的参数添加过滤条件
            //var projectDetails = await _projectDetailRepository.GetAll().AsNoTracking().ToListAsync();
            //var purchaseDetails = await _purchaseDetailRepository.GetAll().AsNoTracking().ToListAsync();

            var count = await query.CountAsync();

			var entityList = await query
					.OrderBy(input.Sorting).AsNoTracking()
                     .OrderByDescending(aa => aa.CreationTime)
                    //.PageBy(input)
                    .ToListAsync();

            // var entityListDtos = ObjectMapper.Map<List<InvoiceDetailListDto>>(entityList);
            List<InvoiceDetailListDto> InvoiceDetailListDtos = entityList.MapTo<List<InvoiceDetailListDto>>();
            //foreach (var item in entityList)
            //{
            //    var InvoiceDetailListDto = item.MapTo<InvoiceDetailListDto>();
            //    if (InvoiceDetailListDto.RefId.HasValue)
            //    {
            //        if (input.Type == InvoiceTypeEnum.销项)
            //        {
            //            InvoiceDetailListDto.RefName = projectDetails.Where(aa => aa.Id == InvoiceDetailListDto.RefId.Value).FirstOrDefault() != null ? projectDetails.Where(aa => aa.Id == InvoiceDetailListDto.RefId.Value).FirstOrDefault().Name : null;
            //        }
            //        else
            //        {
            //            var projectDetailId = purchaseDetails.Where(aa => aa.Id == InvoiceDetailListDto.RefId.Value).FirstOrDefault() != null ? purchaseDetails.Where(aa => aa.Id == InvoiceDetailListDto.RefId.Value).FirstOrDefault().ProjectDetailId : null;
            //            if (projectDetailId.HasValue)
            //                InvoiceDetailListDto.RefName = projectDetails.Where(aa => aa.Id == projectDetailId.Value).FirstOrDefault() != null ? projectDetails.Where(aa => aa.Id == projectDetailId.Value).FirstOrDefault().Name : null;
            //            else
            //                InvoiceDetailListDto.RefName = null;
            //        }
            //    }
            //    else
            //    {
            //        InvoiceDetailListDto.RefName = null;
            //    }
            //    InvoiceDetailListDtos.Add(InvoiceDetailListDto);
            //}

            return new PagedResultDto<InvoiceDetailListDto>(count, InvoiceDetailListDtos);
        }


		/// <summary>
		/// 通过指定id获取InvoiceDetailListDto信息
		/// </summary>
		 
		public async Task<InvoiceDetailListDto> GetByIdAsync(EntityDto<Guid> input)
		{
			var entity = await _entityRepository.GetAsync(input.Id);

		    return entity.MapTo<InvoiceDetailListDto>();
		}

		/// <summary>
		/// 获取编辑 InvoiceDetail
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		
		public async Task<GetInvoiceDetailForEditOutput> GetForEditAsync(NullableIdDto<Guid> input)
		{
			var output = new GetInvoiceDetailForEditOutput();
InvoiceDetailEditDto editDto;

			if (input.Id.HasValue)
			{
				var entity = await _entityRepository.GetAsync(input.Id.Value);

				editDto = entity.MapTo<InvoiceDetailEditDto>();

				//invoiceDetailEditDto = ObjectMapper.Map<List<invoiceDetailEditDto>>(entity);
			}
			else
			{
				editDto = new InvoiceDetailEditDto();
			}

			output.InvoiceDetail = editDto;
			return output;
		}


		/// <summary>
		/// 添加或者修改InvoiceDetail的公共方法
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		
		public async Task CreateOrUpdateAsync(CreateOrUpdateInvoiceDetailInput input)
		{

			if (input.InvoiceDetail.Id.HasValue)
			{
				await UpdateAsync(input.InvoiceDetail);
			}
			else
			{
				await CreateAsync(input.InvoiceDetail);
			}
		}


		/// <summary>
		/// 新增InvoiceDetail
		/// </summary>
		
		protected virtual async Task<InvoiceDetailEditDto> CreateAsync(InvoiceDetailEditDto input)
		{
			//TODO:新增前的逻辑判断，是否允许新增

            // var entity = ObjectMapper.Map <InvoiceDetail>(input);
            var entity=input.MapTo<InvoiceDetail>();


            entity = await _entityManager.CreateAsync(entity);
            var item = entity.MapTo<InvoiceDetailEditDto>();
            return item;
        }

		/// <summary>
		/// 编辑InvoiceDetail
		/// </summary>
		
		protected virtual async Task UpdateAsync(InvoiceDetailEditDto input)
		{
			//TODO:更新前的逻辑判断，是否允许更新

			var entity =input.MapTo<InvoiceDetail>();

			// ObjectMapper.Map(input, entity);
		    await _entityManager.UpdateAsync(entity);
		}



		/// <summary>
		/// 删除InvoiceDetail信息的方法
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		
		public async Task DeleteAsync(EntityDto<Guid> input)
		{
            //TODO:删除前的逻辑判断，是否允许删除
            await _entityManager.DeleteAsync(input.Id);
        }



		/// <summary>
		/// 批量删除InvoiceDetail的方法
		/// </summary>
		
		public async Task BatchDeleteAsync(List<Guid> input)
		{
			// TODO:批量删除前的逻辑判断，是否允许删除
			await _entityRepository.DeleteAsync(s => input.Contains(s.Id));
		}


		/// <summary>
		/// 导出InvoiceDetail为excel表,等待开发。
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


