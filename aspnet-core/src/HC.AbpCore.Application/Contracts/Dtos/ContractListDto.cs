

using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using HC.AbpCore.Contracts;
using static HC.AbpCore.Contracts.ContractEnum;
using Abp.AutoMapper;

namespace HC.AbpCore.Contracts.Dtos
{
    [AutoMapFrom(typeof(Contract))]
    public class ContractListDto : FullAuditedEntityDto<Guid> 
    {

        
		/// <summary>
		/// Type
		/// </summary>
		[Required(ErrorMessage="Type不能为空")]
		public ContractTypeEnum Type { get; set; }



		/// <summary>
		/// ContractCode
		/// </summary>
		[Required(ErrorMessage="ContractCode不能为空")]
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
		/// Desc
		/// </summary>
		public string Desc { get; set; }



		/// <summary>
		/// Attachments
		/// </summary>
		public string Attachments { get; set; }




        public string TypeName
        {
            get
            {
                return Type.ToString();
            }
        }


        public string RefName { get; set; }



    }
}