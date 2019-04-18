
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.AbpCore.Tenders.Dtos
{
    public class GetTenderRemindListDto
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
    }
}
