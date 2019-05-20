

using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Entities;
using Abp.Domain.Services;
using HC.AbpCore.Common;
using HC.AbpCore.DingTalk;
using HC.AbpCore.Reimburses;


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
        Task<ResultCode> SubmitApproval(Guid Id);


        /// <summary>
        /// 报销审批提醒
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="dingDingAppConfig"></param>
        /// <returns></returns>
        Task ReimburseApprovalRemind(string accessToken, DingDingAppConfig dingDingAppConfig);


    }
}
