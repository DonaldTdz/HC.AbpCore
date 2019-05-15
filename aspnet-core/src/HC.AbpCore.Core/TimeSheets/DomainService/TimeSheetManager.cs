

using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Abp.Linq;
using Abp.Linq.Extensions;
using Abp.Extensions;
using Abp.UI;
using Abp.Domain.Repositories;
using Abp.Domain.Services;

using HC.AbpCore;
using HC.AbpCore.TimeSheets;
using HC.AbpCore.Common;
using HC.AbpCore.DingTalk.Employees;
using HC.AbpCore.Projects;
using HC.AbpCore.DingTalk;
using DingTalk.Api;
using DingTalk.Api.Request;
using DingTalk.Api.Response;

namespace HC.AbpCore.TimeSheets.DomainService
{
    /// <summary>
    /// TimeSheet领域层的业务管理
    ///</summary>
    public class TimeSheetManager :AbpCoreDomainServiceBase, ITimeSheetManager
    {
        private readonly IRepository<Project, Guid> _projectRepository;
        private readonly IRepository<Employee, string> _employeeRepository;
        private readonly IRepository<TimeSheet,Guid> _repository;
        private readonly IDingTalkManager _dingTalkManager;

        /// <summary>
        /// TimeSheet的构造方法
        ///</summary>
        public TimeSheetManager(
			IRepository<TimeSheet, Guid> repository,
            IRepository<Project, Guid> projectRepository,
            IRepository<Employee, string> employeeRepository,
            IDingTalkManager dingTalkManager

        )
		{
			_repository =  repository;
            _employeeRepository = employeeRepository;
            _projectRepository = projectRepository;
            _dingTalkManager = dingTalkManager;
        }


		/// <summary>
		/// 初始化
		///</summary>
		public void InitTimeSheet()
		{
			throw new NotImplementedException();
		}

        // TODO:编写领域业务代码


        /// <summary>
        /// 提交审批(return 1)  
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ResultCode> SubmitApproval(TimeSheet item)
        {
            var timeSheetId = await _repository.InsertAndGetIdAsync(item);
            ResultCode resultCode = new ResultCode();
            var config = await _dingTalkManager.GetDingDingConfigByAppAsync(DingDingAppEnum.智能办公);
            string accessToken = await _dingTalkManager.GetAccessTokenByAppAsync(DingDingAppEnum.智能办公);
            //var timeSheet = await _repository.GetAsync(Id);
            var project = await _projectRepository.GetAsync(item.ProjectId);
            if (project == null)
            {
                resultCode.Code = 1;
                resultCode.Msg = "所属项目不存在";
                return resultCode;
            }
            var employee = await _employeeRepository.GetAsync(item.EmployeeId);
            if (employee == null)
            {
                resultCode.Code = 2;
                resultCode.Msg = "员工不存在";
                return resultCode;
            }
            var deptId = employee.Department.Replace("[", "").Replace("]", "");
            //entity.dept_id = deptIds[1];
            //entity.form_component_values = approvals;

            DefaultDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/topapi/processinstance/create");
            OapiProcessinstanceCreateRequest request = new OapiProcessinstanceCreateRequest();
            request.ProcessCode = "PROC-0AA374CC-7381-46EA-BB8F-3BF4B2BFDEA2";
            request.OriginatorUserId = item.EmployeeId;
            request.AgentId = config.AgentID;
            request.DeptId = Convert.ToInt32(deptId);
            List<OapiProcessinstanceCreateRequest.FormComponentValueVoDomain> formComponentValues = new List<OapiProcessinstanceCreateRequest.FormComponentValueVoDomain>();

            OapiProcessinstanceCreateRequest.FormComponentValueVoDomain vo = new OapiProcessinstanceCreateRequest.FormComponentValueVoDomain();
            formComponentValues.Add(new OapiProcessinstanceCreateRequest.FormComponentValueVoDomain() { Name = "所属项目", Value = project.Name + "(" + project.ProjectCode + ")" });
            formComponentValues.Add(new OapiProcessinstanceCreateRequest.FormComponentValueVoDomain() { Name = "工作日期", Value = item.WorkeDate.ToString("yyyy-MM-dd") });
            formComponentValues.Add(new OapiProcessinstanceCreateRequest.FormComponentValueVoDomain() { Name = "员工", Value = employee.Name });
            formComponentValues.Add(new OapiProcessinstanceCreateRequest.FormComponentValueVoDomain() { Name = "工时", Value = item.Hour.HasValue? item.Hour.Value.ToString():null });
            formComponentValues.Add(new OapiProcessinstanceCreateRequest.FormComponentValueVoDomain() { Name = "工作内容", Value = item.Content});
            request.FormComponentValues_ = formComponentValues;
            OapiProcessinstanceCreateResponse response = client.Execute(request, accessToken);
            if (response.ErrCode == "0")
                return new ResultCode() { Code = 0,Msg="提交成功" };
            else
                return new ResultCode() { Code = 4, Msg = "提交失败" };
        }

        
    }
}
