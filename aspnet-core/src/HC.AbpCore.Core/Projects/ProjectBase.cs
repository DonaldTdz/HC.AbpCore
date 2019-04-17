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
            内部=1,
            合伙=2,
            外部=3
        }

        /// <summary>
        /// 项目状态
        /// </summary>
        public enum ProjectStatus
        {
            线索=1,
            立项=2,
            进行中=3,
            已完成=4,
            已回款=5,
            取消
        }
    }
}
