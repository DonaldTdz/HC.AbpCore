

using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using HC.AbpCore.Reimburses;
using Abp.AutoMapper;

namespace HC.AbpCore.Reimburses.Dtos
{
    [AutoMapFrom(typeof(Reimburse))]
    public class ReimburseListDto : EntityDto<Guid>, IHasCreationTime
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
        /// CancelTime
        /// </summary>
        public DateTime? CancelTime { get; set; }



        /// <summary>
        /// CreationTime
        /// </summary>
        [Required(ErrorMessage = "CreationTime不能为空")]
        public DateTime CreationTime { get; set; }



        public string StatusName
        {
            get { return Status.ToString(); }
        }

        /// <summary>
        /// 所属项目名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 员工名称
        /// </summary>
        public string EmployeeName { get; set; }

        /// <summary>
        /// 审批人名称
        /// </summary>
        public string ApproverName { get; set; }
    }
}