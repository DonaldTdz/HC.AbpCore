
using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using HC.AbpCore.Tenders;

namespace HC.AbpCore.Tenders.Dtos
{
    public class TenderEditDto : FullAuditedEntityDto<Guid?>
    {



        /// <summary>
        /// ProjectId
        /// </summary>
        [Required(ErrorMessage = "ProjectId不能为空")]
        public Guid ProjectId { get; set; }



        /// <summary>
        /// TenderTime
        /// </summary>
        public DateTime? TenderTime { get; set; }



        /// <summary>
        /// BondTime
        /// </summary>
        public DateTime? BondTime { get; set; }



        /// <summary>
        /// ReadyTime
        /// </summary>
        public DateTime? ReadyTime { get; set; }



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




    }
}