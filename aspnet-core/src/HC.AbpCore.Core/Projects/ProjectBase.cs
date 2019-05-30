using System;
using System.Collections.Generic;
using System.Text;

namespace HC.AbpCore.Projects
{
    public class ProjectBase
    {
        /// <summary>
        /// 项目模式
        /// </summary>
        public enum ProjectMode
        {
            内部 = 1,
            合伙 = 2,
            外部 = 3
        }

        /// <summary>
        /// 项目状态
        /// </summary>
        public enum ProjectStatus
        {
            立项 = 1,
            招标 = 2,
            合同 = 3,
            收款 = 4,
            已完成 = 5,
            丢单 = 6
        }
    }
}
