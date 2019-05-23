
using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using HC.AbpCore.CompletedTasks;
using HC.AbpCore.Tasks;

namespace  HC.AbpCore.Tasks.Dtos
{
    public class CompletedTaskEditDto : EntityDto<Guid?>, IHasCreationTime
    {
        /// <summary>
        /// ProjectId
        /// </summary>
        public Guid ProjectId { get; set; }



        /// <summary>
        /// Content
        /// </summary>
        public string Content { get; set; }



		/// <summary>
		/// IsCompleted
		/// </summary>
		public bool? IsCompleted { get; set; }


        /// <summary>
        /// Status
        /// </summary>
        public TaskStatusEnum? Status { get; set; }



        /// <summary>
        /// RefId
        /// </summary>
        public Guid? RefId { get; set; }



		/// <summary>
		/// EmployeeId
		/// </summary>
		public string EmployeeId { get; set; }



        /// <summary>
        /// ½ØÖ¹ÈÕÆÚ
        /// </summary>
        public DateTime ClosingDate { get; set; }




        /// <summary>
        /// CreationTime
        /// </summary>
        public DateTime CreationTime { get; set; }




    }
}