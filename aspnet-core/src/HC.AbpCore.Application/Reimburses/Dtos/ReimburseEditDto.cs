
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using HC.AbpCore.Reimburses;
using HC.AbpCore.Reimburses.ReimburseDetails.Dtos;

namespace HC.AbpCore.Reimburses.Dtos
{
    public class ReimburseEditDto : EntityDto<Guid?>, IHasCreationTime
    {



        /// <summary>
        /// ProjectId
        /// </summary>
        public Guid? ProjectId { get; set; }



        /// <summary>
        /// EmployeeId
        /// </summary>
        public string EmployeeId { get; set; }



        /// <summary>
        /// Amount
        /// </summary>
        public decimal? Amount { get; set; }



        /// <summary>
        /// Status
        /// </summary>
        public ReimburseStatusEnum? Status { get; set; }



        /// <summary>
        /// Type
        /// </summary>
        public ReimburseTypeEnum Type { get; set; }



        /// <summary>
        /// GrantStatus
        /// </summary>
        public bool GrantStatus { get; set; }



        /// <summary>
        /// SubmitDate
        /// </summary>
        public DateTime? SubmitDate { get; set; }



        /// <summary>
        /// ApproverId
        /// </summary>
        public string ApproverId { get; set; }



        /// <summary>
        /// ApprovalTime
        /// </summary>
        public DateTime? ApprovalTime { get; set; }



        /// <summary>
        /// 审批实例Id
        /// </summary>
        public string ProcessInstanceId { get; set; }



        /// <summary>
        /// CancelTime
        /// </summary>
        public DateTime? CancelTime { get; set; }



        /// <summary>
        /// CreationTime
        /// </summary>
        [Required(ErrorMessage = "CreationTime不能为空")]
        public DateTime CreationTime { get; set; }



    }
}