
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


using HC.AbpCore.Tenders;
using HC.AbpCore.Tenders.Dtos;
using HC.AbpCore.Tenders.DomainService;
using HC.AbpCore.DingTalk.Employees;
using HC.AbpCore.Projects;

namespace HC.AbpCore.Tenders
{
    /// <summary>
    /// Tender应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize]
    public class TenderAppService : AbpCoreAppServiceBase, ITenderAppService
    {
        private readonly IRepository<Tender, Guid> _entityRepository;
        private readonly IRepository<Employee, string> _employeeRepository;
        private readonly IRepository<Project, Guid> _projectRepository;

        private readonly ITenderManager _entityManager;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public TenderAppService(
        IRepository<Tender, Guid> entityRepository
            , IRepository<Employee, string> employeeRepository
            , IRepository<Project, Guid> projectRepository
        , ITenderManager entityManager
        )
        {
            _entityRepository = entityRepository;
            _employeeRepository = employeeRepository;
            _projectRepository = projectRepository;
            _entityManager = entityManager;
        }


        /// <summary>
        /// 获取Tender的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task<PagedResultDto<TenderListDto>> GetPagedAsync(GetTendersInput input)
        {

            var query = _entityRepository.GetAll().WhereIf(input.ProjectId.HasValue, aa => aa.ProjectId == input.ProjectId.Value)
                .WhereIf(!String.IsNullOrEmpty(input.EmployeeId), aa => aa.EmployeeId == input.EmployeeId);
            // TODO:根据传入的参数添加过滤条件

            var projectList = await _projectRepository.GetAll().AsNoTracking().ToListAsync();
            var employeeList = await _employeeRepository.GetAll().AsNoTracking().ToListAsync();

            var count = await query.CountAsync();

            var entityList = await query
                    .OrderBy(input.Sorting).AsNoTracking()
                    .OrderByDescending(a => a.CreationTime)
                    .Select(aa => new TenderListDto() {
                        Id = aa.Id,
                        ProjectName = projectList.Where(bb => bb.Id == aa.ProjectId).FirstOrDefault().Name,
                        ProjectId = aa.ProjectId,
                        TenderTime = aa.TenderTime,
                        BondTime = aa.BondTime,
                        ReadyTime = aa.ReadyTime,
                        EmployeeId = aa.EmployeeId,
                        EmployeeName = employeeList.Where(bb => bb.Id == aa.EmployeeId).FirstOrDefault().Name,
                        ReadyEmployeeIds = aa.ReadyEmployeeIds,
                        ReadyEmployeeNames = !String.IsNullOrEmpty(aa.ReadyEmployeeIds) ? ReadyEmployeeNames(employeeList,aa.ReadyEmployeeIds).ToString() : aa.ReadyEmployeeIds,
                        IsWinbid=aa.IsWinbid,
                        Attachments=aa.Attachments
                    })
                    .PageBy(input)
                    .ToListAsync();

            // var entityListDtos = ObjectMapper.Map<List<TenderListDto>>(entityList);
            //var entityListDtos = entityList.MapTo<List<TenderListDto>>();

            return new PagedResultDto<TenderListDto>(count, entityList);
        }

        /// <summary>
        /// 处理多个标书准备人名称
        /// </summary>
        /// <param name="readyEmployeeIds"></param>
        /// <returns></returns>
        public string ReadyEmployeeNames(List<Employee> employeeList,string readyEmployeeIds)
        {
            string readyEmployeeNames=null;
            string[] sArray = readyEmployeeIds.Split(',');
            foreach(string id in sArray)
            {
                string name = employeeList.Where(aa => aa.Id == id).FirstOrDefault().Name;
                if (!String.IsNullOrEmpty(name))
                {
                    if (!String.IsNullOrEmpty(readyEmployeeNames))
                        readyEmployeeNames += "," + name;
                    else
                        readyEmployeeNames = name;
                }
            }
            return readyEmployeeNames;

        }


        /// <summary>
        /// 通过指定id获取TenderListDto信息
        /// </summary>

        public async Task<TenderListDto> GetByIdAsync(EntityDto<Guid> input)
        {
            var entity = await _entityRepository.GetAsync(input.Id);
            var employeeList = await _employeeRepository.GetAll().AsNoTracking().ToListAsync();
            var item= entity.MapTo<TenderListDto>();
            item.ProjectName = (await _projectRepository.GetAsync(item.ProjectId)).Name;
            if (!String.IsNullOrEmpty(item.EmployeeId))
                item.EmployeeName = (await _employeeRepository.GetAsync(item.EmployeeId)).Name;
            if (!String.IsNullOrEmpty(item.ReadyEmployeeIds))
                item.ReadyEmployeeNames = ReadyEmployeeNames(employeeList, item.ReadyEmployeeIds).ToString();
            return item;
        }

        /// <summary>
        /// 获取编辑 Tender
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task<GetTenderForEditOutput> GetForEditAsync(NullableIdDto<Guid> input)
        {
            var output = new GetTenderForEditOutput();
            TenderEditDto editDto;

            if (input.Id.HasValue)
            {
                var entity = await _entityRepository.GetAsync(input.Id.Value);

                editDto = entity.MapTo<TenderEditDto>();

                //tenderEditDto = ObjectMapper.Map<List<tenderEditDto>>(entity);
            }
            else
            {
                editDto = new TenderEditDto();
            }

            output.Tender = editDto;
            return output;
        }


        /// <summary>
        /// 添加或者修改Tender的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task CreateOrUpdateAsync(CreateOrUpdateTenderInput input)
        {
            if (input.Tender.Id.HasValue)
            {
                await UpdateAsync(input.Tender);
            }
            else
            {
                input.Tender.IsWinbid = false;
                await CreateAsync(input.Tender);
            }
        }


        /// <summary>
        /// 新增Tender
        /// </summary>

        protected virtual async Task<TenderEditDto> CreateAsync(TenderEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增

            // var entity = ObjectMapper.Map <Tender>(input);
            var entity = input.MapTo<Tender>();


            entity = await _entityRepository.InsertAsync(entity);
            return entity.MapTo<TenderEditDto>();
        }

        /// <summary>
        /// 编辑Tender
        /// </summary>

        protected virtual async Task UpdateAsync(TenderEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新

            var entity = await _entityRepository.GetAsync(input.Id.Value);
            input.MapTo(entity);

            // ObjectMapper.Map(input, entity);
            await _entityRepository.UpdateAsync(entity);
        }



        /// <summary>
        /// 删除Tender信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task DeleteAsync(EntityDto<Guid> input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _entityRepository.DeleteAsync(input.Id);
        }



        /// <summary>
        /// 批量删除Tender的方法
        /// </summary>

        public async Task BatchDeleteAsync(List<Guid> input)
        {
            // TODO:批量删除前的逻辑判断，是否允许删除
            await _entityRepository.DeleteAsync(s => input.Contains(s.Id));
        }

        /// <summary>
        /// 获取招标提醒数据
        /// </summary>
        /// <returns></returns>
        public async Task<List<GetTenderRemindListDto>> GetTenderRemindData()
        {
            List<GetTenderRemindListDto> getTenderRemindListDtos = new List<GetTenderRemindListDto>();
            var datas = await _entityRepository.GetAll().Where(aa => aa.BondTime <= DateTime.Now.AddDays(2) && aa.BondTime>=DateTime.Now).AsNoTracking().ToListAsync();
            var readyTimeRemind= await _entityRepository.GetAll().Where(aa =>aa.ReadyTime <= DateTime.Now.AddDays(4) && aa.ReadyTime>=DateTime.Now).AsNoTracking().ToListAsync();
            if (datas?.Count > 0)
            {
                foreach (var item in datas)
                {
                    GetTenderRemindListDto getTenderRemindListDto = new GetTenderRemindListDto();
                    getTenderRemindListDto.Title = "招标保证金截止时间提醒";
                    var dateTime = item.BondTime.Value - DateTime.Now;
                    var project = await _projectRepository.GetAsync(item.ProjectId);
                    getTenderRemindListDto.Content = project.Name + "的招标保证金截止时间还剩" + dateTime.Days + "天" + dateTime.Hours + "小时" + dateTime.Minutes + "分" + dateTime.Seconds + "秒";
                    getTenderRemindListDtos.Add(getTenderRemindListDto);
                }
            }
            if (readyTimeRemind?.Count > 0)
            {
                foreach (var item in readyTimeRemind)
                {
                    GetTenderRemindListDto getTenderRemindListDto = new GetTenderRemindListDto();
                    getTenderRemindListDto.Title = "招标准备完成时间提醒";
                    var dateTime = item.ReadyTime.Value - DateTime.Now;
                    var project = await _projectRepository.GetAsync(item.ProjectId);
                    getTenderRemindListDto.Content = project.Name + "的招标准备完成时间还剩" + dateTime.Days + "天" + dateTime.Hours + "小时" + dateTime.Minutes + "分" + dateTime.Seconds + "秒";
                    getTenderRemindListDtos.Add(getTenderRemindListDto);
                }
            }
            
            return getTenderRemindListDtos;
        }

        /// <summary>
        /// 导出Tender为excel表,等待开发。
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


