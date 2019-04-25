
using Abp.Runtime.Validation;
using HC.AbpCore.Dtos;
using HC.AbpCore.Contracts;
using static HC.AbpCore.Contracts.ContractEnum;
using System;

namespace HC.AbpCore.Contracts.Dtos
{
    public class GetContractsInput : PagedSortedAndFilteredInputDto, IShouldNormalize
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
        /// 查询条件-合同分类
        /// </summary>
        public ContractTypeEnum? Type { get; set; }

        /// <summary>
        /// 查询条件-合同编号
        /// </summary>
        public string ContractCode { get; set; }

        /// <summary>
        /// 查询条件-项目ID
        /// </summary>
        public Guid? ProjectId { get; set; }

        /// <summary>
        /// 查询条件-采购ID
        /// </summary>
        public Guid? PurchaseId { get; set; }

    }
}
