using System;
using System.Collections.Generic;
using System.Text;

namespace HC.AbpCore.Contracts
{
    public class ContractEnum
    {
        /// <summary>
        /// 合同分类 枚举
        /// </summary>
        public enum ContractTypeEnum
        {
            销项 = 1,
            进项 = 2
        }

        /// <summary>
        /// 合同编号分类
        /// </summary>
        public enum CodeTypeEnum
        {
            硬件=1,软件=2
        }
    }
}
