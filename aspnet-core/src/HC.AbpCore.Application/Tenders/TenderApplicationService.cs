
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
using HC.AbpCore.DingTalk;
using HC.AbpCore.Tasks;
using HC.AbpCore.CompletedTasks;
using static HC.AbpCore.Projects.ProjectBase;

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
        private readonly IRepository<CompletedTask, Guid> _taskRepository;
        private readonly IDingTalkManager _dingTalkManager;

        private readonly ITenderManager _entityManager;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public TenderAppService(
        IRepository<Tender, Guid> entityRepository
            , IDingTalkManager dingTalkManager
            , IRepository<Employee, string> employeeRepository
            , IRepository<Project, Guid> projectRepository
            , IRepository<CompletedTask, Guid> taskRepository
        , ITenderManager entityManager
        )
        {
            _entityRepository = entityRepository;
            _employeeRepository = employeeRepository;
            _projectRepository = projectRepository;
            _entityManager = entityManager;
            _dingTalkManager = dingTalkManager;
            _taskRepository = taskRepository;
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

            var projects = _projectRepository.GetAll();
            var employeeList = await _employeeRepository.GetAll().AsNoTracking().ToListAsync();

            //var count = await query.CountAsync();

            var items = from item in query
                        join project in projects on item.ProjectId equals project.Id
                        select new TenderListDto()
                        {
                            Id = item.Id,
                            ProjectName = project.Name + "(" + project.ProjectCode + ")",
                            ProjectId = item.ProjectId,
                            TenderTime = item.TenderTime,
                            Bond = item.Bond,
                            BondTime = item.BondTime,
                            ReadyTime = item.ReadyTime,
                            IsPayBond = item.IsPayBond,
                            IsReady = item.IsReady,
                            EmployeeId = item.EmployeeId,
                            EmployeeName = employeeList.Where(bb => bb.Id == item.EmployeeId).FirstOrDefault().Name,
                            ReadyEmployeeIds = item.ReadyEmployeeIds,
                            ReadyEmployeeNames = !String.IsNullOrEmpty(item.ReadyEmployeeIds) ? ReadyEmployeeNames(employeeList, item.ReadyEmployeeIds).ToString() : item.ReadyEmployeeIds,
                            IsWinbid = item.IsWinbid,
                            Attachments = item.Attachments
                        };
            var count = await items.CountAsync();
            var entityList = await items
                    .OrderBy(input.Sorting).AsNoTracking()
                    .OrderByDescending(a => a.TenderTime)
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
        public string ReadyEmployeeNames(List<Employee> employeeList, string readyEmployeeIds)
        {
            string readyEmployeeNames = null;
            string[] sArray = readyEmployeeIds.Split(',');
            foreach (string id in sArray)
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
        /// 通过指定id或者projectId获取TenderListDto信息
        /// </summary>

        public async Task<TenderListDto> GetByIdAsync(EntityDto<Guid?> input, Guid? projectId)
        {
            Tender entity = new Tender();
            if (projectId.HasValue)
                entity = await _entityRepository.FirstOrDefaultAsync(aa => aa.ProjectId == projectId.Value);
            else
                entity = await _entityRepository.GetAsync(input.Id.Value);
            var item = entity.MapTo<TenderListDto>();
            if (item == null)
                return item;
            var employeeList = await _employeeRepository.GetAll().AsNoTracking().ToListAsync();
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
            if (!entity.IsPayBond.HasValue)
                entity.IsPayBond = false;
            if (!entity.IsReady.HasValue)
                entity.IsReady = false;


            entity = await _entityRepository.InsertAsync(entity);
            //添加到任务列表
            await _taskRepository.InsertAsync(new CompletedTask() { Status = TaskStatusEnum.招标, IsCompleted = entity.IsWinbid, RefId = entity.Id, EmployeeId = entity.EmployeeId, ProjectId = entity.ProjectId, ClosingDate = entity.TenderTime.Value });
            await _taskRepository.InsertAsync(new CompletedTask() { Status = TaskStatusEnum.招标保证金缴纳, IsCompleted = entity.IsPayBond, RefId = entity.Id, EmployeeId = entity.EmployeeId, ProjectId = entity.ProjectId, ClosingDate = entity.BondTime.Value });
            //await _taskRepository.InsertAsync(new CompletedTask() { Status = TaskStatusEnum.招标准备, IsCompleted = entity.IsReady, RefId = entity.Id, EmployeeId = entity.EmployeeId, ProjectId = entity.ProjectId,ClosingDate = entity.ReadyTime.Value });
            var employeeIds = entity.ReadyEmployeeIds.Split(",");
            foreach (var employeeId in employeeIds)
            {
                await _taskRepository.InsertAsync(new CompletedTask() { Status = TaskStatusEnum.招标准备, IsCompleted = entity.IsReady, RefId = entity.Id, EmployeeId = employeeId, ProjectId = entity.ProjectId, ClosingDate = entity.ReadyTime.Value });
            }

            return entity.MapTo<TenderEditDto>();
        }

        /// <summary>
        /// 编辑Tender
        /// </summary>

        protected virtual async Task UpdateAsync(TenderEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新
            if (input.IsWinbid.HasValue)
            {
                var project = await _projectRepository.GetAsync(input.ProjectId);
                project.Status = input.IsWinbid.Value == true ? ProjectStatus.合同 : ProjectStatus.丢单;
                await _projectRepository.UpdateAsync(project);
            }

            var entity = await _entityRepository.GetAsync(input.Id.Value);
            //TODO:更新任务列表

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
            //同时删除任务列表
            await _taskRepository.DeleteAsync(s => s.RefId == input.Id);

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
        /// 发送钉钉工作通知
        /// </summary>
        /// <returns></returns>
        public async Task<List<GetTenderRemindListDto>> GetTenderRemindData()
        {
            List<GetTenderRemindListDto> getTenderRemindListDtos = new List<GetTenderRemindListDto>();
            var datas = await _entityRepository.GetAll().Where(aa => aa.BondTime <= DateTime.Now.AddDays(2) && aa.BondTime >= DateTime.Now).AsNoTracking().ToListAsync();
            var readyTimeRemind = await _entityRepository.GetAll().Where(aa => aa.ReadyTime <= DateTime.Now.AddDays(4) && aa.ReadyTime >= DateTime.Now).AsNoTracking().ToListAsync();
            string accessToken = await _dingTalkManager.GetAccessTokenByAppAsync(DingDingAppEnum.智能办公);
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


