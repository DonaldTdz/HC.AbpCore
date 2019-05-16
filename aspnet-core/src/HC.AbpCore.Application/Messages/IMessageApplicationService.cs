
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


using HC.AbpCore.Messages.Dtos;
using HC.AbpCore.Messages;

namespace HC.AbpCore.Messages
{
    /// <summary>
    /// Message应用层服务的接口方法
    ///</summary>
    public interface IMessageAppService : IApplicationService
    {
        /// <summary>
		/// 获取Message的分页列表信息
		///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<MessageListDto>> GetPaged(GetMessagesInput input);


		/// <summary>
		/// 通过指定id获取MessageListDto信息
		/// </summary>
		Task<MessageListDto> GetById(EntityDto<Guid> input);


        /// <summary>
        /// 返回实体的EditDto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetMessageForEditOutput> GetForEdit(NullableIdDto<Guid> input);


        /// <summary>
        /// 添加或者修改Message的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdate(CreateOrUpdateMessageInput input);


        /// <summary>
        /// 删除Message信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task Delete(EntityDto<Guid> input);


        /// <summary>
        /// 批量删除Message
        /// </summary>
        Task BatchDelete(List<Guid> input);


		/// <summary>
        /// 导出Message为excel表
        /// </summary>
        /// <returns></returns>
		//Task<FileDto> GetToExcel();

    }
}
