
using System;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities.Auditing;
using HC.AbpCore.Messages;

namespace  HC.AbpCore.Messages.Dtos
{
    public class MessageEditDto
    {

        /// <summary>
        /// Id
        /// </summary>
        public Guid? Id { get; set; }         


        
		/// <summary>
		/// EmployeeId
		/// </summary>
		[Required(ErrorMessage="EmployeeId不能为空")]
		public Guid EmployeeId { get; set; }



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
		public int? Type { get; set; }



		/// <summary>
		/// IsRead
		/// </summary>
		public bool? IsRead { get; set; }



		/// <summary>
		/// ReadTime
		/// </summary>
		public DateTime? ReadTime { get; set; }




    }
}