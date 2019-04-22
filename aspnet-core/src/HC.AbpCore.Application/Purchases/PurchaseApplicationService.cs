
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


using HC.AbpCore.Purchases;
using HC.AbpCore.Purchases.Dtos;
using HC.AbpCore.Purchases.DomainService;
using HC.AbpCore.DingTalk.Employees;
using HC.AbpCore.Projects;
using HC.AbpCore.Dtos;

namespace HC.AbpCore.Purchases
{
    /// <summary>
    /// Purchase应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize]
    public class PurchaseAppService : AbpCoreAppServiceBase, IPurchaseAppService
    {
        private readonly IRepository<Purchase, Guid> _entityRepository;
        private readonly IRepository<Employee, string> _employeeRepository;
        private readonly IRepository<Project, Guid> _projectRepository;
        private readonly IPurchaseManager _entityManager;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public PurchaseAppService(
        IRepository<Purchase, Guid> entityRepository,
         IRepository<Employee, string> employeeRepository,
          IRepository<Project, Guid> projectRepository
        , IPurchaseManager entityManager
        )
        {
            _employeeRepository = employeeRepository;
            _projectRepository = projectRepository;
            _entityRepository = entityRepository;
            _entityManager = entityManager;
        }


        /// <summary>
        /// 获取Purchase的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<PurchaseListDto>> GetPagedAsync(GetPurchasesInput input)
        {

            var query = _entityRepository.GetAll().WhereIf(input.ProjectId.HasValue, AA => AA.ProjectId == input.ProjectId.Value)
                .WhereIf(!String.IsNullOrEmpty(input.Code), aa => aa.Code.Contains(input.Code));
            // TODO:根据传入的参数添加过滤条件
            var employeeList = await _employeeRepository.GetAll().AsNoTracking().ToListAsync();
            var projectList = await _projectRepository.GetAll().AsNoTracking().ToListAsync();

            var count = await query.CountAsync();

            var entityList = await query
                    .OrderBy(input.Sorting).AsNoTracking()
                    .OrderByDescending(aa => aa.CreationTime)
                    .Select(aa => new PurchaseListDto()
                    {
                        Id = aa.Id,
                        Code = aa.Code,
                        ProjectId = aa.ProjectId,
                        EmployeeId = aa.EmployeeId,
                        PurchaseDate = aa.PurchaseDate,
                        Desc = aa.Desc,
                        ProjectName = aa.ProjectId.HasValue ? projectList.Where(bb => bb.Id == aa.ProjectId).FirstOrDefault().Name : null,
                        EmployeeName = !String.IsNullOrEmpty(aa.EmployeeId) ? employeeList.Where(bb => bb.Id == aa.EmployeeId).FirstOrDefault().Name : null,
                        CreationTime = aa.CreationTime
                    })
                    .PageBy(input)
                    .ToListAsync();

            // var entityListDtos = ObjectMapper.Map<List<PurchaseListDto>>(entityList);
            var entityListDtos = entityList.MapTo<List<PurchaseListDto>>();

            return new PagedResultDto<PurchaseListDto>(count, entityListDtos);
        }


        /// <summary>
        /// 通过指定id获取PurchaseListDto信息
        /// </summary>
        public async Task<PurchaseListDto> GetByIdAsync(EntityDto<Guid> input)
        {
            var entity = await _entityRepository.GetAsync(input.Id);

            var item = entity.MapTo<PurchaseListDto>();
            item.EmployeeName = !String.IsNullOrEmpty(item.EmployeeId) ? (await _employeeRepository.GetAsync(item.EmployeeId)).Name : null;
            item.ProjectName = item.ProjectId.HasValue ? (await _projectRepository.GetAsync(item.ProjectId.Value)).Name : null;
            return item;
        }

        /// <summary>
        /// 获取编辑 Purchase
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<GetPurchaseForEditOutput> GetForEditAsync(NullableIdDto<Guid> input)
        {
            var output = new GetPurchaseForEditOutput();
            PurchaseEditDto editDto;

            if (input.Id.HasValue)
            {
                var entity = await _entityRepository.GetAsync(input.Id.Value);

                editDto = entity.MapTo<PurchaseEditDto>();

                //purchaseEditDto = ObjectMapper.Map<List<purchaseEditDto>>(entity);
            }
            else
            {
                editDto = new PurchaseEditDto();
            }

            output.Purchase = editDto;
            return output;
        }


        /// <summary>
        /// 添加或者修改Purchase的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<APIResultDto> CreateOrUpdateAsync(CreateOrUpdatePurchaseInput input)
        {

            if (input.Purchase.Id.HasValue)
            {
                return await UpdateAsync(input.Purchase);
            }
            else
            {
                return await CreateAsync(input.Purchase);
            }
        }


        /// <summary>
        /// 新增Purchase
        /// </summary>
        protected virtual async Task<APIResultDto> CreateAsync(PurchaseEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增
            var purchaseCount = await _entityRepository.GetAll().Where(aa => aa.Code == input.Code).CountAsync();
            if (purchaseCount > 0)
                return new APIResultDto() { Code = 0, Msg = "保存失败,采购编号已存在" };

            // var entity = ObjectMapper.Map <Purchase>(input);
            var entity = input.MapTo<Purchase>();


            entity = await _entityRepository.InsertAsync(entity);
            if (entity != null)
                return new APIResultDto() { Code = 1, Msg = "保存成功" };
            else
                return new APIResultDto() { Code = 0, Msg = "保存失败" };
        }

        /// <summary>
        /// 编辑Purchase
        /// </summary>
        protected virtual async Task<APIResultDto> UpdateAsync(PurchaseEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新

            var entity = await _entityRepository.GetAsync(input.Id.Value);
            if (entity.Code != input.Code)
            {
                var purchaseCount = await _entityRepository.GetAll().Where(aa => aa.Code == input.Code).CountAsync();
                if (purchaseCount > 0)
                    return new APIResultDto() { Code = 0, Msg = "保存失败,采购编号已存在" };
            }

            input.MapTo(entity);

            // ObjectMapper.Map(input, entity);
            entity = await _entityRepository.UpdateAsync(entity);

            if (entity != null)
                return new APIResultDto() { Code = 1, Msg = "保存成功" };
            else
                return new APIResultDto() { Code = 0, Msg = "保存失败" };
        }



        /// <summary>
        /// 删除Purchase信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task DeleteAsync(EntityDto<Guid> input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _entityRepository.DeleteAsync(input.Id);
        }



        /// <summary>
        /// 批量删除Purchase的方法
        /// </summary>
        public async Task BatchDeleteAsync(List<Guid> input)
        {
            // TODO:批量删除前的逻辑判断，是否允许删除
            await _entityRepository.DeleteAsync(s => input.Contains(s.Id));
        }

        /// <summary>
        /// 获取采购下拉列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<DropDownDto>> GetDropDownsAsync()
        {
            var query = _entityRepository.GetAll();
            var entityList = await query
                    .OrderBy(a => a.CreationTime).AsNoTracking()
                    .Select(c => new DropDownDto() { Text = c.Code, Value = c.Id.ToString() })
                    .ToListAsync();
            return entityList;
        }

        /// <summary>
        /// 导出Purchase为excel表,等待开发。
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


