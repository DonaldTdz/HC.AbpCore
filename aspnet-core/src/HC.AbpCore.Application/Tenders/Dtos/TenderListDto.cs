

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
        [Required(ErrorMessage = "ProjectId不能为空")]
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
        /// IsReady
        /// </summary>
        public bool? IsReady { get; set; }



        /// <summary>
        /// PurchaseStartDate
        /// </summary>
        public DateTime? PurchaseStartDate { get; set; }



        /// <summary>
        /// ReadyEmployeeIds
        /// </summary>
        public DateTime? PurchaseEndDate { get; set; }



        /// <summary>
        /// BidPurchaser
        /// </summary>
        public string BidPurchaser { get; set; }



        /// <summary>
        /// PurchaseInformation
        /// </summary>
        public string PurchaseInformation { get; set; }



        /// <summary>
        /// BuyBidingPerson
        /// </summary>
        public string BuyBidingPerson { get; set; }



        /// <summary>
        /// PreparationPerson
        /// </summary>
        public string PreparationPerson { get; set; }



        /// <summary>
        /// Qualification
        /// </summary>
        public string Qualification { get; set; }



        /// <summary>
        /// Organizer
        /// </summary>
        public string Organizer { get; set; }



        /// <summary>
        /// Inspector
        /// </summary>
        public string Inspector { get; set; }



        /// <summary>
        /// Qualification
        /// </summary>
        public string Binder { get; set; }



        /// <summary>
        /// IsWinbid
        /// </summary>
        public bool? IsWinbid { get; set; }



        /// <summary>
        /// Attachments
        /// </summary>
        public string Attachments { get; set; }



        /// <summary>
        /// Voucher
        /// </summary>
        public string Voucher { get; set; }



        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }
    }
}