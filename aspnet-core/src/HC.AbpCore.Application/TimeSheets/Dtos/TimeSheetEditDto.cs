
using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using HC.AbpCore.TimeSheets;

namespace HC.AbpCore.TimeSheets.Dtos
{
    public class TimeSheetEditDto : EntityDto<Guid?>, IHasCreationTime
    {

        /// <summary>
        /// ProjectId
        /// </summary>
        [Required(ErrorMessage = "ProjectId不能为空")]
        public Guid ProjectId { get; set; }



        /// <summary>
        /// WorkeDate
        /// </summary>
        [Required(ErrorMessage = "WorkeDate不能为空")]
        public DateTime WorkeDate { get; set; }



        /// <summary>
        /// EmployeeId
        /// </summary>
        [Required(ErrorMessage = "EmployeeId不能为空")]
        public string EmployeeId { get; set; }



        /// <summary>
        /// Hour
        /// </summary>
        public int? Hour { get; set; }



        /// <summary>
        /// Content
        /// </summary>
        public string Content { get; set; }



        /// <summary>
        /// Status
        /// </summary>
        public TimeSheetStatusEnum? Status { get; set; }



        /// <summary>
        /// ApproverId
        /// </summary>
        public string ApproverId { get; set; }



        /// <summary>
        /// ApprovalTime
        /// </summary>
        public DateTime? ApprovalTime { get; set; }



        /// <summary>
        /// CreationTime
        /// </summary>
        [Required(ErrorMessage = "CreationTime不能为空")]
        public DateTime CreationTime { get; set; }




    }
}