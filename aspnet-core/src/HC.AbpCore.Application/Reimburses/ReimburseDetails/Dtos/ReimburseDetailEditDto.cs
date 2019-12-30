
using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using HC.AbpCore.Reimburses.ReimburseDetails;

namespace  HC.AbpCore.Reimburses.ReimburseDetails.Dtos
{
    public class ReimburseDetailEditDto : EntityDto<Guid?>, IHasCreationTime
    {     


        
		/// <summary>
		/// ReimburseId
		/// </summary>
		[Required(ErrorMessage="ReimburseId不能为空")]
		public Guid ReimburseId { get; set; }



		/// <summary>
		/// HappenDate
		/// </summary>
		[Required(ErrorMessage="HappenDate不能为空")]
		public DateTime HappenDate { get; set; }



		/// <summary>
		/// Type
		/// </summary>
		[Required(ErrorMessage="Type不能为空")]
		public string Type { get; set; }



		/// <summary>
		/// Place
		/// </summary>
		[Required(ErrorMessage="Place不能为空")]
		public string Place { get; set; }



		/// <summary>
		/// Customer
		/// </summary>
		public string Customer { get; set; }



		/// <summary>
		/// Desc
		/// </summary>
		public string Desc { get; set; }



		/// <summary>
		/// Amount
		/// </summary>
		[Required(ErrorMessage="Amount不能为空")]
		public decimal Amount { get; set; }



		/// <summary>
		/// InvoiceNum
		/// </summary>
		public int? InvoiceNum { get; set; }



		/// <summary>
		/// Attachments
		/// </summary>
		public string Attachments { get; set; }



		/// <summary>
		/// CreationTime
		/// </summary>
		[Required(ErrorMessage="CreationTime不能为空")]
		public DateTime CreationTime { get; set; }




    }
}