

using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Services;
using HC.AbpCore.DingTalk.Employees;


namespace HC.AbpCore.DingTalk.Employees.DomainService
{
    public interface IEmployeeManager : IDomainService
    {

        /// <summary>
        /// 初始化方法
        ///</summary>
        void InitEmployee();


        /// <summary>
        /// 员工周报提醒 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="dingDingAppConfig"></param>
        /// <returns></returns>
        Task EmployeeWeeklyRemind(string accessToken, DingDingAppConfig dingDingAppConfig);




    }
}
