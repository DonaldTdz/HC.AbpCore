
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


using HC.AbpCore.Reports.AccountAnalysisReport;
using HC.AbpCore.Reports.AccountAnalysisReport.Dtos;
using HC.AbpCore.Reports.AccountAnalysisReport.DomainService;



namespace HC.AbpCore.Reports.AccountAnalysisReport
{
    /// <summary>
    /// AccountAnalysis应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize]
    public class AccountAnalysisAppService : AbpCoreAppServiceBase, IAccountAnalysisAppService
    {

        private readonly IAccountAnalysisManager _entityManager;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public AccountAnalysisAppService(
        IAccountAnalysisManager entityManager
        )
        {
             _entityManager=entityManager;
        }


        /// <summary>
        /// 获取AccountAnalysis的列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task<ResultAccountAnalysisDto> GetAccountAnalysesAsync(GetAccountAnalysissInput input)
		{

		    var query =await _entityManager.GetAccountAnalysesAsync(input.Type,input.RefId);
            // TODO:根据传入的参数添加过滤条件


            //var count = await query.CountAsync();

            //var entityList = await query
            //		.OrderBy(input.Sorting).AsNoTracking()
            //		.PageBy(input)
            //		.ToListAsync();

            // var entityListDtos = ObjectMapper.Map<List<AccountAnalysisListDto>>(entityList);
            var entityListDtos = query.MapTo<ResultAccountAnalysisDto>();

            //return new PagedResultDto<AccountAnalysisListDto>(count,entityListDtos);
            return entityListDtos;
        }

    }
}


