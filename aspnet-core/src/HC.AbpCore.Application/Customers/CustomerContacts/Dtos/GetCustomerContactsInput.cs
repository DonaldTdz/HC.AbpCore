
using Abp.Runtime.Validation;
using HC.AbpCore.Dtos;
using HC.AbpCore.Customers.CustomerContacts;

namespace HC.AbpCore.Customers.CustomerContacts.Dtos
{
    public class GetCustomerContactsInput : PagedSortedAndFilteredInputDto, IShouldNormalize
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
        /// 查询条件客户Id
        /// </summary>
        public int? CustomerId { get; set; }

    }
}
