

using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using HC.AbpCore.Invoices;
using Abp.AutoMapper;

namespace HC.AbpCore.Invoices.Dtos
{
    [AutoMapFrom(typeof(Invoice))]
    public class InvoiceListDto : FullAuditedEntityDto<Guid> 
    {



		/// <summary>
		/// Type
		/// </summary>
		[Required(ErrorMessage="Type不能为空")]
		public InvoiceTypeEnum Type { get; set; }



		/// <summary>
		/// Code
		/// </summary>
		[Required(ErrorMessage="Code不能为空")]
		public string Code { get; set; }



		/// <summary>
		/// Amount
		/// </summary>
		public decimal? Amount { get; set; }



		/// <summary>
		/// SubmitDate
		/// </summary>
		public DateTime? SubmitDate { get; set; }



		/// <summary>
		/// RefId
		/// </summary>
		public Guid? RefId { get; set; }


        public string typeName
        {
            get { return Type.ToString(); }
        }


        /// <summary>
        /// RefName
        /// </summary>
        public string RefName { get; set; }

    }
}