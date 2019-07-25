

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Entities;
using Abp.Domain.Services;
using HC.AbpCore.Common;
using HC.AbpCore.DingTalk;
using HC.AbpCore.Reimburses;
using HC.AbpCore.Reimburses.ReimburseDetails;

namespace HC.AbpCore.Reimburses.DomainService
{
    public interface IReimburseManager : IDomainService
    {

        /// <summary>
        /// 初始化方法
        ///</summary>
        void InitReimburse();


        /// <summary>
        /// 提交审批
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ResultCode> SubmitApprovalAsync(Reimburse reimburse, List<ReimburseDetail> reimburseDetail);


        /// <summary>
        /// 报销审批提醒
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="dingDingAppConfig"></param>
        /// <returns></returns>
        Task ReimburseApprovalRemind(string accessToken, DingDingAppConfig dingDingAppConfig);


        /// <summary>
        /// 根据审批实例Id修改报销状态
        /// </summary>
        /// <param name="processInstanceId">审批实例ID</param>
        /// <param name="result">审批结束状态</param>
        /// <returns></returns>
        Task UpdateReimburseByPIIdAsync(string processInstanceId, string result);


    }
}
