

using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Services;
using HC.AbpCore.DingTalk;
using HC.AbpCore.Projects;


namespace HC.AbpCore.Projects.DomainService
{
    public interface IProjectManager : IDomainService
    {

        /// <summary>
        /// 初始化方法
        ///</summary>
        void InitProject();


        /// <summary>
        /// 项目进度提醒
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="dingDingAppConfig"></param>
        Task ProjectStatusRemind(string accessToken, DingDingAppConfig dingDingAppConfig);

            

    }
}
