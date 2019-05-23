
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


using HC.AbpCore.Messages;
using HC.AbpCore.Messages.Dtos;
using HC.AbpCore.Messages.DomainService;
using Abp.Auditing;

namespace HC.AbpCore.Messages
{
    /// <summary>
    /// Message应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize]
    public class MessageAppService : AbpCoreAppServiceBase, IMessageAppService
    {
        private readonly IRepository<Message, Guid> _entityRepository;

        private readonly IMessageManager _entityManager;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public MessageAppService(
        IRepository<Message, Guid> entityRepository
        , IMessageManager entityManager
        )
        {
            _entityRepository = entityRepository;
            _entityManager = entityManager;
        }


        /// <summary>
        /// 获取Message的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        [Audited]
        public async Task<PagedResultDto<MessageListDto>> GetPagedAsync(GetMessagesInput input)
        {

            var query = _entityRepository.GetAll().WhereIf(!String.IsNullOrEmpty(input.EmployeeId), aa => aa.EmployeeId == input.EmployeeId);
            // TODO:根据传入的参数添加过滤条件

            var count = await query.CountAsync();

            var entityList = await query
                    .OrderByDescending(aa => aa.SendTime)
                    .OrderBy(aa => aa.IsRead)
                    .AsNoTracking()
                    .PageBy(input)
                    .ToListAsync();

            // var entityListDtos = ObjectMapper.Map<List<MessageListDto>>(entityList);
            var entityListDtos = entityList.MapTo<List<MessageListDto>>();

            return new PagedResultDto<MessageListDto>(count, entityListDtos);
        }


        /// <summary>
        /// 通过指定id获取MessageListDto信息
        /// </summary>
        [AbpAllowAnonymous]
        [Audited]
        public async Task<MessageListDto> GetByIdAsync(EntityDto<Guid> input)
        {
            var entity = await _entityRepository.GetAsync(input.Id);

            return entity.MapTo<MessageListDto>();
        }

        /// <summary>
        /// 获取编辑 Message
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task<GetMessageForEditOutput> GetForEditAsync(NullableIdDto<Guid> input)
        {
            var output = new GetMessageForEditOutput();
            MessageEditDto editDto;

            if (input.Id.HasValue)
            {
                var entity = await _entityRepository.GetAsync(input.Id.Value);

                editDto = entity.MapTo<MessageEditDto>();

                //messageEditDto = ObjectMapper.Map<List<messageEditDto>>(entity);
            }
            else
            {
                editDto = new MessageEditDto();
            }

            output.Message = editDto;
            return output;
        }


        /// <summary>
        /// 添加或者修改Message的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        [Audited]
        public async Task CreateOrUpdateAsync(CreateOrUpdateMessageInput input)
        {

            if (input.Message.Id.HasValue)
            {
                await UpdateAsync(input.Message);
            }
            else
            {
                await CreateAsync(input.Message);
            }
        }


        /// <summary>
        /// 新增Message
        /// </summary>

        protected virtual async Task<MessageEditDto> CreateAsync(MessageEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增

            // var entity = ObjectMapper.Map <Message>(input);
            var entity = input.MapTo<Message>();


            entity = await _entityRepository.InsertAsync(entity);
            return entity.MapTo<MessageEditDto>();
        }

        /// <summary>
        /// 编辑Message
        /// </summary>

        protected virtual async Task UpdateAsync(MessageEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新

            var entity = await _entityRepository.GetAsync(input.Id.Value);
            input.MapTo(entity);

            // ObjectMapper.Map(input, entity);
            await _entityRepository.UpdateAsync(entity);
        }



        /// <summary>
        /// 删除Message信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task DeleteAsync(EntityDto<Guid> input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _entityRepository.DeleteAsync(input.Id);
        }



        /// <summary>
        /// 批量删除Message的方法
        /// </summary>

        public async Task BatchDeleteAsync(List<Guid> input)
        {
            // TODO:批量删除前的逻辑判断，是否允许删除
            await _entityRepository.DeleteAsync(s => input.Contains(s.Id));
        }



        /// <summary>
        /// 通过指定id修改是否已读
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        [Audited]
        public async Task<MessageListDto> ModifyReadByIdAsync(EntityDto<Guid> input)
        {
            //TODO:更新前的逻辑判断，是否允许更新

            var entity = await _entityRepository.GetAsync(input.Id);
            entity.IsRead = true;
            entity.ReadTime = DateTime.Now;

            // ObjectMapper.Map(input, entity);
            entity = await _entityRepository.UpdateAsync(entity);
            var item = entity.MapTo<MessageListDto>();
            return item;
        }

        /// <summary>
        /// 导出Message为excel表,等待开发。
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


