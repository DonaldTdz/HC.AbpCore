
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


using HC.AbpCore.Reports.SalesDetails;
using HC.AbpCore.Reports.SalesDetails.Dtos;
using HC.AbpCore.Reports.SalesDetails.DomainService;



namespace HC.AbpCore.Reports.SalesDetails
{
    /// <summary>
    /// SalesDetail应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize]
    public class SalesDetailAppService : AbpCoreAppServiceBase, ISalesDetailAppService
    {
        private readonly IRepository<SalesDetail, Guid> _entityRepository;

        private readonly ISalesDetailManager _entityManager;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public SalesDetailAppService(
        IRepository<SalesDetail, Guid> entityRepository
        , ISalesDetailManager entityManager
        )
        {
            _entityRepository = entityRepository;
            _entityManager = entityManager;
        }


        /// <summary>
        /// 获取SalesDetail的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task<PagedResultDto<SalesDetailListDto>> GetSalesDetailPagedAsync(GetSalesDetailsInput input)
        {

            var query = _entityRepository.GetAll().Where(aa => aa.CreationTime >= input.StartCreateDate && aa.CreationTime < input.EndCreateDate)
                .WhereIf(input.ProjectId.HasValue, aa => aa.Id == input.ProjectId.Value);
            // TODO:根据传入的参数添加过滤条件


            var count = await query.CountAsync();

            var entityList = await query
                    .OrderBy(input.Sorting).AsNoTracking()
                    //.PageBy(input)
                    .ToListAsync();

            // var entityListDtos = ObjectMapper.Map<List<SalesDetailListDto>>(entityList);
            var entityListDtos = entityList.MapTo<List<SalesDetailListDto>>();
            if (count > 0)
            {
                SalesDetailListDto salesDetailListDto = new SalesDetailListDto();
                salesDetailListDto.projectName = "合计";
                salesDetailListDto.contractAmount = entityListDtos.Sum(aa => aa.contractAmount);
                salesDetailListDto.acceptedAmount = entityListDtos.Sum(aa => aa.acceptedAmount);
                salesDetailListDto.uncollectedAmount = entityListDtos.Sum(aa => aa.uncollectedAmount);
                salesDetailListDto.Profit = entityListDtos.Sum(aa => aa.Profit);
                entityListDtos.Add(salesDetailListDto);
            }

            return new PagedResultDto<SalesDetailListDto>(count, entityListDtos);
        }


        /// <summary>
        /// 导出SalesDetail为excel表,等待开发。
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


