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
            线索 = 1,
            立项 = 2,
            招标 = 3,
            执行 = 4,
            已完成 = 5,
            丢单=6
        }

        public enum AppMenu
        {
            工时统计=1
        }
    }
}
