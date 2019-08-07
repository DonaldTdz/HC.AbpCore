
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


using HC.AbpCore.Reports.ProfitStatistics.Dtos;
using HC.AbpCore.Reports.ProfitStatistics;

namespace HC.AbpCore.Reports.ProfitStatistics
{
    /// <summary>
    /// ProfitStatistic应用层服务的接口方法
    ///</summary>
    public interface IProfitStatisticAppService : IApplicationService
    {
        /// <summary>
		/// 获取ProfitStatistic的利润统计
		///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<List<ProfitStatisticListDto>> GetProfitStatisticAsync(GetProfitStatisticsInput input);


		/// <summary>
		/// 通过指定id获取ProfitStatisticListDto信息
		/// </summary>
		Task<ProfitStatisticListDto> GetByIdAsync(EntityDto<Guid> input);


		/// <summary>
        /// 导出ProfitStatistic为excel表
        /// </summary>
        /// <returns></returns>
		//Task<FileDto> GetToExcel();

    }
}
