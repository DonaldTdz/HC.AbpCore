

using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using HC.AbpCore.Tasks;
using HC.AbpCore.CompletedTasks;
using Abp.AutoMapper;

namespace HC.AbpCore.Tasks.Dtos
{
    [AutoMapFrom(typeof(CompletedTask))]
    public class CompletedTaskListDto : EntityDto<Guid>,IHasCreationTime 
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
        /// 截止日期
        /// </summary>
        public DateTime ClosingDate { get; set; }



        /// <summary>
        /// CreationTime
        /// </summary>
        public DateTime CreationTime { get; set; }


       
        /// <summary>
        /// 格式化创建日期
        /// </summary>
        public string CreationTimeFormat
        {
            get
            {
                return CreationTime.ToString("yyyy-MM-dd");
            }
        }



        /// <summary>
        /// 格式化截止日期
        /// </summary>
        public string ClosingDateFormat
        {
            get
            {
                return ClosingDate.ToString("yyyy-MM-dd");
            }
        }



        /// <summary>
        /// 任务类型名称
        /// </summary>
        public string StatusName
        {
            get { return Status.ToString(); }
        }



        /// <summary>
        /// 是否完成
        /// </summary>
        public string IsCompletedName
        {
            get { return IsCompleted == true ? "已完成" : "未完成"; }
        }



        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }
    }
}