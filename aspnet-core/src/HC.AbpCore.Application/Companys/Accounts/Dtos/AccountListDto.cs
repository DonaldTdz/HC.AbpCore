

using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using HC.AbpCore.Companys.Accounts;

namespace HC.AbpCore.Companys.Accounts.Dtos
{
    public class AccountListDto : EntityDto<long>, IHasCreationTime
    {

        
		/// <summary>
		/// CompanyId
		/// </summary>
		[Required(ErrorMessage="CompanyId不能为空")]
		public int CompanyId { get; set; }



		/// <summary>
		/// Type
		/// </summary>
		[Required(ErrorMessage="Type不能为空")]
		public AccountType Type { get; set; }



		/// <summary>
		/// Initial
		/// </summary>
		public decimal? Initial { get; set; }



		/// <summary>
		/// Amount
		/// </summary>
		public decimal? Amount { get; set; }



		/// <summary>
		/// Ending
		/// </summary>
		public decimal? Ending { get; set; }



		/// <summary>
		/// Desc
		/// </summary>
		public string Desc { get; set; }



		/// <summary>
		/// RefId
		/// </summary>
		public string RefId { get; set; }



		/// <summary>
		/// CreationTime
		/// </summary>
		[Required(ErrorMessage="CreationTime不能为空")]
		public DateTime CreationTime { get; set; }

        public string TypeName
        {
            get
            {
                return Type.ToString();
            }
        }


    }
}