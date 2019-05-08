

using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Services;
using HC.AbpCore.Reimburses.ReimburseDetails;


namespace HC.AbpCore.Reimburses.ReimburseDetails.DomainService
{
    public interface IReimburseDetailManager : IDomainService
    {

        /// <summary>
        /// 初始化方法
        ///</summary>
        void InitReimburseDetail();



		 
      
         

    }
}
