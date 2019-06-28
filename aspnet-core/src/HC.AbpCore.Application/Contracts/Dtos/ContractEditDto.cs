
using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using HC.AbpCore.Contracts;
using static HC.AbpCore.Contracts.ContractEnum;

namespace  HC.AbpCore.Contracts.Dtos
{
    public class ContractEditDto:FullAuditedEntityDto<Guid?>
    {      
        
		/// <summary>
		/// Type
		/// </summary>
		[Required(ErrorMessage="Type不能为空")]
		public ContractTypeEnum Type { get; set; }



		/// <summary>
		/// ContractCode
		/// </summary>
		public string ContractCode { get; set; }

        /// <summary>
        /// RefId
        /// </summary>
        public Guid? RefId { get; set; }



		/// <summary>
		/// SignatureTime
		/// </summary>
		public DateTime? SignatureTime { get; set; }



		/// <summary>
		/// Amount
		/// </summary>
		public decimal? Amount { get; set; }



        /// <summary>
        /// 合同拟写
        /// </summary>
        public int ContractDrafting { get; set; }



        /// <summary>
        /// 原件回收情况
        /// </summary>
        public int OriginalRecycling { get; set; }



        /// <summary>
        /// 原件附件 多个 逗号分隔
        /// </summary>
        public string OriginalAnnex { get; set; }



        /// <summary>
        /// Attachments
        /// </summary>
        public string Attachments { get; set; }




    }
}