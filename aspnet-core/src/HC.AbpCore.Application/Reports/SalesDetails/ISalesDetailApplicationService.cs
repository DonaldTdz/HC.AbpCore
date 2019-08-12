
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
using Abp.Authorization;
using Abp.Linq.Extensions;
using Abp.Domain.Repositories;
using Abp.Application.Services;
using Abp.Application.Services.Dto;


using HC.AbpCore.Reports.SalesDetails.Dtos;
using HC.AbpCore.Reports.SalesDetails;

namespace HC.AbpCore.Reports.SalesDetails
{
    /// <summary>
    /// SalesDetail应用层服务的接口方法
    ///</summary>
    public interface ISalesDetailAppService : IApplicationService
    {
        /// <summary>
		/// 获取SalesDetail的分页列表信息
		///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<SalesDetailListDto>> GetSalesDetailPagedAsync(GetSalesDetailsInput input);


		/// <summary>
        /// 导出SalesDetail为excel表
        /// </summary>
        /// <returns></returns>
		//Task<FileDto> GetToExcel();

    }
}
