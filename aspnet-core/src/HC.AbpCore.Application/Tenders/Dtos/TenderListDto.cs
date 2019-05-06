

using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using HC.AbpCore.Tenders;
using Abp.AutoMapper;

namespace HC.AbpCore.Tenders.Dtos
{
    [AutoMapFrom(typeof(Tender))]
    public class TenderListDto : FullAuditedEntityDto<Guid> 
    {

        
		/// <summary>
		/// ProjectId
		/// </summary>
		[Required(ErrorMessage="ProjectId不能为空")]
		public Guid ProjectId { get; set; }



		/// <summary>
		/// TenderTime
		/// </summary>
		public DateTime? TenderTime { get; set; }


        /// <summary>
        /// Bond
        /// </summary>
        public decimal? Bond { get; set; }



        /// <summary>
        /// BondTime
        /// </summary>
        public DateTime? BondTime { get; set; }



        /// <summary>
        /// IsPayBond
        /// </summary>
        public bool? IsPayBond { get; set; }



        /// <summary>
        /// ReadyTime
        /// </summary>
        public DateTime? ReadyTime { get; set; }



        /// <summary>
        /// IsReady
        /// </summary>
        public bool? IsReady { get; set; }



        /// <summary>
        /// EmployeeId
        /// </summary>
        public string EmployeeId { get; set; }



		/// <summary>
		/// ReadyEmployeeIds
		/// </summary>
		public string ReadyEmployeeIds { get; set; }



		/// <summary>
		/// IsWinbid
		/// </summary>
		public bool? IsWinbid { get; set; }



		/// <summary>
		/// Attachments
		/// </summary>
		public string Attachments { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 负责人名称
        /// </summary>
        public string EmployeeName { get; set; }

        /// <summary>
        /// 标书准备人名称
        /// </summary>
        public string ReadyEmployeeNames { get; set; }




    }
}