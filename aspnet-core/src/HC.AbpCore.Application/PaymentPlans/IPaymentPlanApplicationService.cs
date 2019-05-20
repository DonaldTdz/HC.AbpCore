
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using HC.AbpCore.PaymentPlans.Dtos;

namespace HC.AbpCore.PaymentPlans
{
    /// <summary>
    /// PaymentPlan应用层服务的接口方法
    ///</summary>
    public interface IPaymentPlanAppService : IApplicationService
    {
        /// <summary>
		/// 获取PaymentPlan的分页列表信息
		///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<PaymentPlanListDto>> GetPagedAsync(GetPaymentPlansInput input);


		/// <summary>
		/// 通过指定id获取PaymentPlanListDto信息
		/// </summary>
		Task<PaymentPlanListDto> GetByIdAsync(EntityDto<Guid> input);


        /// <summary>
        /// 返回实体的EditDto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetPaymentPlanForEditOutput> GetForEditAsync(NullableIdDto<Guid> input);


        /// <summary>
        /// 添加或者修改PaymentPlan的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdateAsync(CreateOrUpdatePaymentPlanInput input);


        /// <summary>
        /// 删除PaymentPlan信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteAsync(EntityDto<Guid> input);


        /// <summary>
        /// 批量删除PaymentPlan
        /// </summary>
        Task BatchDeleteAsync(List<Guid> input);

        

        /// <summary>
        /// 导出PaymentPlan为excel表
        /// </summary>
        /// <returns></returns>
        //Task<FileDto> GetToExcel();

    }
}
