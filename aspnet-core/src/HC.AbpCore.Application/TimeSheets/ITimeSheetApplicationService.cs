
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


using HC.AbpCore.TimeSheets.Dtos;
using HC.AbpCore.TimeSheets;
using HC.AbpCore.Dtos;

namespace HC.AbpCore.TimeSheets
{
    /// <summary>
    /// TimeSheet应用层服务的接口方法
    ///</summary>
    public interface ITimeSheetAppService : IApplicationService
    {
        /// <summary>
		/// 获取TimeSheet的分页列表信息
		///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<TimeSheetListDto>> GetPagedAsync(GetTimeSheetsInput input);


		/// <summary>
		/// 通过指定id获取TimeSheetListDto信息
		/// </summary>
		Task<TimeSheetListDto> GetByIdAsync(EntityDto<Guid> input);


        /// <summary>
        /// 返回实体的EditDto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetTimeSheetForEditOutput> GetForEditAsync(NullableIdDto<Guid> input);


        /// <summary>
        /// 添加或者修改TimeSheet的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdateAsync(CreateOrUpdateTimeSheetInput input);


        /// <summary>
        /// 删除TimeSheet信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteAsync(EntityDto<Guid> input);


        /// <summary>
        /// 批量删除TimeSheet
        /// </summary>
        Task BatchDeleteAsync(List<Guid> input);


        /// <summary>
        /// 提交审批
        /// </summary>
        /// <param name="input"></param>
        /// <param name="messageId">对应消息中心Id</param>
        /// <returns></returns>
        Task<APIResultDto> SubmitApproval(CreateOrUpdateTimeSheetInput input);
        /// <summary>
        /// 导出TimeSheet为excel表
        /// </summary>
        /// <returns></returns>
        //Task<FileDto> GetToExcel();

    }
}
