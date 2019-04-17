

using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Services;
using HC.AbpCore.Projects.ProjectDetails;


namespace HC.AbpCore.Projects.ProjectDetails.DomainService
{
    public interface IProjectDetailManager : IDomainService
    {

        /// <summary>
        /// 初始化方法
        ///</summary>
        void InitProjectDetail();



		 
      
         

    }
}
