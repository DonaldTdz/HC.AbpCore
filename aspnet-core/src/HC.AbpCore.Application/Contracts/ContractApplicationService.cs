
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


using HC.AbpCore.Contracts;
using HC.AbpCore.Contracts.Dtos;
using HC.AbpCore.Contracts.DomainService;
using HC.AbpCore.Projects;
using HC.AbpCore.Purchases;
using static HC.AbpCore.Contracts.ContractEnum;
using HC.AbpCore.Dtos;

namespace HC.AbpCore.Contracts
{
    /// <summary>
    /// Contract应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize]
    public class ContractAppService : AbpCoreAppServiceBase, IContractAppService
    {
        private readonly IRepository<Contract, Guid> _entityRepository;
        private readonly IRepository<Project, Guid> _projectRepository;
        private readonly IRepository<Purchase, Guid> _purchaseRepository;

        private readonly IContractManager _entityManager;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public ContractAppService(
        IRepository<Contract, Guid> entityRepository
            , IRepository<Project, Guid> projectRepository
            , IRepository<Purchase, Guid> purchaseRepository
        , IContractManager entityManager
        )
        {
            _entityRepository = entityRepository;
            _projectRepository = projectRepository;
            _purchaseRepository = purchaseRepository;
            _entityManager = entityManager;
        }


        /// <summary>
        /// 获取Contract的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task<PagedResultDto<ContractListDto>> GetPagedAsync(GetContractsInput input)
        {

            var query = _entityRepository.GetAll().WhereIf(input.Type.HasValue, aa => aa.Type == input.Type.Value)
                .WhereIf(!String.IsNullOrEmpty(input.ContractCode), aa => aa.ContractCode.Contains(input.ContractCode))
                .WhereIf(input.ProjectId.HasValue,aa=>aa.RefId==input.ProjectId.Value);
            // TODO:根据传入的参数添加过滤条件
            var projectList = await _projectRepository.GetAll().AsNoTracking().ToListAsync();
            var purchaseList = await _purchaseRepository.GetAll().AsNoTracking().ToListAsync();

            var count = await query.CountAsync();

            var entityList = await query
                    .OrderBy(input.Sorting).AsNoTracking()
                    .OrderByDescending(aa => aa.CreationTime)
                    .Select(aa => new ContractListDto()
                    {
                        Id=aa.Id,
                        Type = aa.Type,
                        ContractCode = aa.ContractCode,
                        RefId = aa.RefId,
                        SignatureTime = aa.SignatureTime,
                        Amount = aa.Amount,
                        Desc = aa.Desc,
                        Attachments = aa.Attachments,
                        RefName = aa.Type == ContractTypeEnum.销项 ? projectList.Where(bb => bb.Id == aa.RefId).FirstOrDefault().Name : purchaseList.Where(bb => bb.Id == aa.RefId).FirstOrDefault().Code,
                    })
                    .PageBy(input)
                    .ToListAsync();

            // var entityListDtos = ObjectMapper.Map<List<ContractListDto>>(entityList);
            var entityListDtos = entityList.MapTo<List<ContractListDto>>();

            return new PagedResultDto<ContractListDto>(count, entityListDtos);
        }


        /// <summary>
        /// 通过指定id获取ContractListDto信息
        /// </summary>

        public async Task<ContractListDto> GetByIdAsync(EntityDto<Guid> input)
        {
            var entity = await _entityRepository.GetAsync(input.Id);

            var item= entity.MapTo<ContractListDto>();
            if (item.Type == ContractTypeEnum.销项 && item.RefId.HasValue)
                item.RefName = (await _projectRepository.GetAsync(item.RefId.Value)).Name;
            if (item.Type == ContractTypeEnum.进项 && item.RefId.HasValue)
                item.RefName = (await _purchaseRepository.GetAsync(item.RefId.Value)).Code;

            return item;
        }

        /// <summary>
        /// 获取编辑 Contract
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task<GetContractForEditOutput> GetForEditAsync(NullableIdDto<Guid> input)
        {
            var output = new GetContractForEditOutput();
            ContractEditDto editDto;

            if (input.Id.HasValue)
            {
                var entity = await _entityRepository.GetAsync(input.Id.Value);

                editDto = entity.MapTo<ContractEditDto>();

                //contractEditDto = ObjectMapper.Map<List<contractEditDto>>(entity);
            }
            else
            {
                editDto = new ContractEditDto();
            }

            output.Contract = editDto;
            return output;
        }


        /// <summary>
        /// 添加或者修改Contract的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task<APIResultDto> CreateOrUpdateAsync(CreateOrUpdateContractInput input)
        {

            if (input.Contract.Id.HasValue)
            {
               return await UpdateAsync(input.Contract);
            }
            else
            {
               return await CreateAsync(input.Contract);
            }
        }


        /// <summary>
        /// 新增Contract
        /// </summary>

        protected virtual async Task<APIResultDto> CreateAsync(ContractEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增
            input.Amount = 0;
            //判断合同编号是否重复
            var contractCount = await _entityRepository.GetAll().Where(aa => aa.ContractCode == input.ContractCode).CountAsync();
            if(contractCount>0)
                return new APIResultDto() { Code = 0, Msg = "保存失败,合同编号已存在" };

            // var entity = ObjectMapper.Map <Contract>(input);
            var entity = input.MapTo<Contract>();


            entity = await _entityRepository.InsertAsync(entity);
            if (entity != null)
                return new APIResultDto() { Code = 1, Msg = "保存成功" };
            else
                return new APIResultDto() { Code = 0, Msg = "保存失败" };
        }

        /// <summary>
        /// 编辑Contract
        /// </summary>

        protected virtual async Task<APIResultDto> UpdateAsync(ContractEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新

            var entity = await _entityRepository.GetAsync(input.Id.Value);
            //判断合同编号是否重复
            if (entity.ContractCode != input.ContractCode)
            {
                var contractCount = await _entityRepository.GetAll().Where(aa => aa.ContractCode == input.ContractCode).CountAsync();
                if (contractCount > 0)
                    return new APIResultDto() { Code = 0, Msg = "保存失败,合同编号已存在" };
            }

            input.MapTo(entity);

            // ObjectMapper.Map(input, entity);
            entity= await _entityRepository.UpdateAsync(entity);

            if (entity != null)
                return new APIResultDto() { Code = 1, Msg = "保存成功" };
            else
                return new APIResultDto() { Code = 0, Msg = "保存失败" };
        }



        /// <summary>
        /// 删除Contract信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task DeleteAsync(EntityDto<Guid> input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _entityRepository.DeleteAsync(input.Id);
        }



        /// <summary>
        /// 批量删除Contract的方法
        /// </summary>

        public async Task BatchDeleteAsync(List<Guid> input)
        {
            // TODO:批量删除前的逻辑判断，是否允许删除
            await _entityRepository.DeleteAsync(s => input.Contains(s.Id));
        }


        /// <summary>
        /// 导出Contract为excel表,等待开发。
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


