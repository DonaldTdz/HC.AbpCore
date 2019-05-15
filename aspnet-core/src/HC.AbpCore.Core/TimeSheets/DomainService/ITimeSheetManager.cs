

using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Services;
using HC.AbpCore.Common;
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



    }
}
