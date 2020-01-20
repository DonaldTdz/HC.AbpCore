

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

        /// <summary>
        /// 新增报销明细
        /// </summary>
        /// <param name="reimburseDetail"></param>
        /// <returns></returns>
        Task<decimal> Create(ReimburseDetail reimburseDetail);

        /// <summary>
        /// 编辑报销明细
        /// </summary>
        /// <param name="reimburseDetail"></param>
        /// <returns></returns>
        Task<decimal> Update(ReimburseDetail reimburseDetail);

        /// <summary>
        /// 删除报销明细
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<decimal> Delete(Guid id);
    }
}
