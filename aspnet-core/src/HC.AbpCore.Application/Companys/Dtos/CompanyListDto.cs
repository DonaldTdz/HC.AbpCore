

using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using HC.AbpCore.Companys;

namespace HC.AbpCore.Companys.Dtos
{
    public class CompanyListDto : EntityDto<int> 
    {

        
		/// <summary>
		/// Name
		/// </summary>
		public string Name { get; set; }



		/// <summary>
		/// Bank
		/// </summary>
		public string Bank { get; set; }



		/// <summary>
		/// BankAccount
		/// </summary>
		public string BankAccount { get; set; }



		/// <summary>
		/// DutyNo
		/// </summary>
		public string DutyNo { get; set; }



		/// <summary>
		/// Address
		/// </summary>
		public string Address { get; set; }



		/// <summary>
		/// Tel
		/// </summary>
		public string Tel { get; set; }



		/// <summary>
		/// Balance
		/// </summary>
		public decimal? Balance { get; set; }



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