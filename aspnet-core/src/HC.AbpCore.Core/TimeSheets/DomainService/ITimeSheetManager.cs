

using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Services;
using HC.AbpCore.Common;
using HC.AbpCore.DingTalk;
using HC.AbpCore.TimeSheets;


namespace HC.AbpCore.TimeSheets.DomainService
{
    public interface ITimeSheetManager : IDomainService
    {

        /// <summary>
        /// 初始化方法
        ///</summary>
        void InitTimeSheet();



        /// <summary>
        /// 提交审批
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ResultCode> SubmitApproval(TimeSheet item);


        /// <summary>
        /// 工时审批提醒
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="dingDingAppConfig"></param>
        /// <returns></returns>
        Task TimeSheetApprovalRemind(string accessToken, DingDingAppConfig dingDingAppConfig);


        /// <summary>
        /// 根据审批实例Id修改工时状态
        /// </summary>
        /// <param name="processInstanceId"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        Task UpdateTimeSheetByPIIdAsync(string processInstanceId, string result);
    }
}
