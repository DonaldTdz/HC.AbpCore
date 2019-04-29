

using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using HC.AbpCore.Purchases;
using Abp.AutoMapper;

namespace HC.AbpCore.Purchases.Dtos
{
    [AutoMapFrom(typeof(Purchase))]
    public class PurchaseListDto : FullAuditedEntityDto<Guid> 
    {

        
		/// <summary>
		/// Code
		/// </summary>
		public string Code { get; set; }



		/// <summary>
		/// ProjectId
		/// </summary>
		public Guid? ProjectId { get; set; }

        /// <summary>
        /// Type
        /// </summary>
        public PurchaseTypeEnum Type { get; set; }

        /// <summary>
        /// EmployeeId
        /// </summary>
        public string EmployeeId { get; set; }



		/// <summary>
		/// PurchaseDate
		/// </summary>
		public DateTime? PurchaseDate { get; set; }



		/// <summary>
		/// Desc
		/// </summary>
		public string Desc { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 采购负责人名称
        /// </summary>
        public string EmployeeName { get; set; }


        public string TypeName
        {
            get { return Type.ToString(); }
        }
        
    }
}