

using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Services;
using HC.AbpCore.Reports.SalesDetails;


namespace HC.AbpCore.Reports.SalesDetails.DomainService
{
    public interface ISalesDetailManager : IDomainService
    {

        /// <summary>
        /// 初始化方法
        ///</summary>
        void InitSalesDetail();



		 
      
         

    }
}
