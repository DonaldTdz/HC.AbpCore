

using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using HC.AbpCore.Messages;

namespace HC.AbpCore.Messages.Dtos
{
    public class MessageListDto : EntityDto<Guid> 
    {

        
		/// <summary>
		/// EmployeeId
		/// </summary>
		[Required(ErrorMessage="EmployeeId不能为空")]
		public string EmployeeId { get; set; }



		/// <summary>
		/// SendTime
		/// </summary>
		public DateTime? SendTime { get; set; }



		/// <summary>
		/// Content
		/// </summary>
		public string Content { get; set; }



		/// <summary>
		/// Type
		/// </summary>
		public MessageTypeEnum? Type { get; set; }



		/// <summary>
		/// IsRead
		/// </summary>
		public bool? IsRead { get; set; }



		/// <summary>
		/// ReadTime
		/// </summary>
		public DateTime? ReadTime { get; set; }



        /// <summary>
        /// 格式化发送时间
        /// </summary>
        public string SendTimeFormat
        {
            get
            {
                if (SendTime.HasValue)
                {
                    return SendTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                }
                return string.Empty;
            }
        }



        /// <summary>
        /// 格式化已读时间
        /// </summary>
        public string ReadTimeFormat
        {
            get
            {
                if (ReadTime.HasValue)
                {
                    return ReadTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                }
                return string.Empty;
            }
        }
    }
}