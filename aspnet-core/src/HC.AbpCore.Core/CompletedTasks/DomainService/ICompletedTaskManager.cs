

using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Services;
using HC.AbpCore.DingTalk;
using HC.AbpCore.Tasks;


namespace HC.AbpCore.Tasks.DomainService
{
    public interface ICompletedTaskManager : IDomainService
    {

        /// <summary>
        /// 初始化方法
        ///</summary>
        void InitCompletedTask();


        /// <summary>
        /// 待完成任务提醒
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="dingDingAppConfig"></param>
        /// <returns></returns>
        Task TaskRemind(string accessToken, DingDingAppConfig dingDingAppConfig);





    }
}
