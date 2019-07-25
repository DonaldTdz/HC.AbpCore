using System;
using System.Collections.Generic;
using System.Text;

namespace HC.AbpCore.Reimburses
{
    public class ReimburseBase
    {
    }

    public enum ReimburseStatusEnum
    {
        提交 = 1, 审批通过 = 2, 拒绝 = 3, 取消 = 4,
    }

    public enum ReimburseTypeEnum
    {
        项目型报销 = 1, 非项目报销 = 2
    }

}
