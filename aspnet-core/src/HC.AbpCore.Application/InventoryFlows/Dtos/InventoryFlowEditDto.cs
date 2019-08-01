
using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using HC.AbpCore.InventoryFlows;

namespace  HC.AbpCore.InventoryFlows.Dtos
{
    public class InventoryFlowEditDto : EntityDto<long?>, IHasCreationTime
    {

		/// <summary>
		/// ProductId
		/// </summary>
		[Required(ErrorMessage="ProductId不能为空")]
		public int ProductId { get; set; }



		/// <summary>
		/// Type
		/// </summary>
		[Required(ErrorMessage="Type不能为空")]
		public InventoryFlowType Type { get; set; }



		/// <summary>
		/// Initial
		/// </summary>
		public int? Initial { get; set; }



		/// <summary>
		/// StreamNumber
		/// </summary>
		public int? StreamNumber { get; set; }



		/// <summary>
		/// Ending
		/// </summary>
		public int? Ending { get; set; }



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




    }
}