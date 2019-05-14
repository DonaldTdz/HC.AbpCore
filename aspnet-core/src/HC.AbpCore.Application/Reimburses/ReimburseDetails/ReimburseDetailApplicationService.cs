
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


using HC.AbpCore.Reimburses.ReimburseDetails;
using HC.AbpCore.Reimburses.ReimburseDetails.Dtos;
using HC.AbpCore.Reimburses.ReimburseDetails.DomainService;
using Abp.Auditing;

namespace HC.AbpCore.Reimburses.ReimburseDetails
{
    /// <summary>
    /// ReimburseDetail应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize]
    public class ReimburseDetailAppService : AbpCoreAppServiceBase, IReimburseDetailAppService
    {
        private readonly IRepository<ReimburseDetail, Guid> _entityRepository;
        private readonly IRepository<Reimburse, Guid> _reimburseRepository;
        private readonly IReimburseDetailManager _entityManager;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public ReimburseDetailAppService(
        IRepository<ReimburseDetail, Guid> entityRepository
            , IRepository<Reimburse, Guid> reimburseRepository
        , IReimburseDetailManager entityManager
        )
        {
            _reimburseRepository = reimburseRepository;
            _entityRepository = entityRepository;
            _entityManager = entityManager;
        }


        /// <summary>
        /// 获取ReimburseDetail的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        [Audited]
        public async Task<PagedResultDto<ReimburseDetailListDto>> GetPagedAsync(GetReimburseDetailsInput input)
        {

            var query = _entityRepository.GetAll().WhereIf(input.ReimburseId.HasValue, aa => aa.ReimburseId == input.ReimburseId.Value);
            // TODO:根据传入的参数添加过滤条件


            var count = await query.CountAsync();

            var entityList = await query
                    .OrderBy(input.Sorting).AsNoTracking()
                    .OrderByDescending(aa => aa.CreationTime)
                    .PageBy(input)
                    .ToListAsync();

            // var entityListDtos = ObjectMapper.Map<List<ReimburseDetailListDto>>(entityList);
            var entityListDtos = entityList.MapTo<List<ReimburseDetailListDto>>();

            return new PagedResultDto<ReimburseDetailListDto>(count, entityListDtos);
        }


        /// <summary>
        /// 通过指定id获取ReimburseDetailListDto信息
        /// </summary>
        [AbpAllowAnonymous]
        [Audited]
        public async Task<ReimburseDetailListDto> GetByIdAsync(EntityDto<Guid> input)
        {
            var entity = await _entityRepository.GetAsync(input.Id);

            return entity.MapTo<ReimburseDetailListDto>();
        }

        /// <summary>
        /// 获取编辑 ReimburseDetail
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task<GetReimburseDetailForEditOutput> GetForEditAsync(NullableIdDto<Guid> input)
        {
            var output = new GetReimburseDetailForEditOutput();
            ReimburseDetailEditDto editDto;

            if (input.Id.HasValue)
            {
                var entity = await _entityRepository.GetAsync(input.Id.Value);

                editDto = entity.MapTo<ReimburseDetailEditDto>();

                //reimburseDetailEditDto = ObjectMapper.Map<List<reimburseDetailEditDto>>(entity);
            }
            else
            {
                editDto = new ReimburseDetailEditDto();
            }

            output.ReimburseDetail = editDto;
            return output;
        }


        /// <summary>
        /// 添加或者修改ReimburseDetail的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        [Audited]
        public async Task CreateOrUpdateAsync(CreateOrUpdateReimburseDetailInput input)
        {

            if (input.ReimburseDetail.Id.HasValue)
            {
                await UpdateAsync(input.ReimburseDetail);
            }
            else
            {
                await CreateAsync(input.ReimburseDetail);
            }
        }


        /// <summary>
        /// 新增ReimburseDetail
        /// </summary>

        protected virtual async Task<ReimburseDetailEditDto> CreateAsync(ReimburseDetailEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增
            var reimburse = await _reimburseRepository.GetAsync(input.ReimburseId);
            reimburse.Amount += input.Amount;
            await _reimburseRepository.UpdateAsync(reimburse);

            // var entity = ObjectMapper.Map <ReimburseDetail>(input);
            var entity = input.MapTo<ReimburseDetail>();


            entity = await _entityRepository.InsertAsync(entity);
            return entity.MapTo<ReimburseDetailEditDto>();
        }

        /// <summary>
        /// 编辑ReimburseDetail
        /// </summary>

        protected virtual async Task UpdateAsync(ReimburseDetailEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新

            var entity = await _entityRepository.GetAsync(input.Id.Value);
            if (input.Amount != entity.Amount)
            {
                var reimburse = await _reimburseRepository.GetAsync(input.ReimburseId);
                reimburse.Amount = reimburse.Amount - entity.Amount + input.Amount;
                await _reimburseRepository.UpdateAsync(reimburse);
            }

            input.MapTo(entity);



            // ObjectMapper.Map(input, entity);
            await _entityRepository.UpdateAsync(entity);
        }



        /// <summary>
        /// 删除ReimburseDetail信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        [Audited]
        public async Task DeleteAsync(EntityDto<Guid> input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            var entity = await _entityRepository.GetAsync(input.Id);
            var reimburse = await _reimburseRepository.GetAsync(entity.ReimburseId);
            reimburse.Amount -= entity.Amount;
            await _reimburseRepository.UpdateAsync(reimburse);

            await _entityRepository.DeleteAsync(input.Id);
        }



        /// <summary>
        /// 批量删除ReimburseDetail的方法
        /// </summary>

        public async Task BatchDeleteAsync(List<Guid> input)
        {
            // TODO:批量删除前的逻辑判断，是否允许删除
            await _entityRepository.DeleteAsync(s => input.Contains(s.Id));
        }


        /// <summary>
        /// 导出ReimburseDetail为excel表,等待开发。
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


