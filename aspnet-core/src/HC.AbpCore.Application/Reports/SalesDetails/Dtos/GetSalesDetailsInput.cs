
using Abp.Runtime.Validation;
using HC.AbpCore.Dtos;
using HC.AbpCore.Reports.SalesDetails;
using System;

namespace HC.AbpCore.Reports.SalesDetails.Dtos
{
    public class GetSalesDetailsInput : PagedSortedAndFilteredInputDto, IShouldNormalize
    {

        /// <summary>
        /// 正常化排序使用
        /// </summary>
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Id";
            }
        }
        
        /// <summary>
        /// 查询条件-项目Id
        /// </summary>
        public Guid? ProjectId { get; set; }

        /// <summary>
        /// 查询条件-创建日期
        /// </summary>
        public DateTime CreateDate { get; set; }

        public DateTime StartCreateDate
        {
            get { return new DateTime(CreateDate.Year, 1, 1); }
        }

        public DateTime EndCreateDate
        {
            get
            {
                return new DateTime(CreateDate.Year + 1, 1, 1);
            }
        }

    }
}
