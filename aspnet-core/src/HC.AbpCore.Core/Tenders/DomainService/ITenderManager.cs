

using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Services;
using HC.AbpCore.DingTalk;
using HC.AbpCore.Tenders;


namespace HC.AbpCore.Tenders.DomainService
{
    public interface ITenderManager : IDomainService
    {

        /// <summary>
        /// 初始化方法
        ///</summary>
        void InitTender();


        /// <summary>
        /// 新增招标并新增到任务列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<Tender> CreateAndCreateTaskAsync(Tender input);



        /// <summary>
        /// 编辑招标并编辑到任务列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<Tender> UpdateAndUpdateTaskAsync(Tender input);


        /// <summary>
        /// 招标提醒
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="dingDingAppConfig"></param>
        /// <returns></returns>
        Task TenderRemindAsync(string accessToken, DingDingAppConfig dingDingAppConfig);

    }
}
