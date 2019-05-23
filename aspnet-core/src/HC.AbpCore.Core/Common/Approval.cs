using System;
using System.Collections.Generic;
using System.Text;

namespace HC.AbpCore.Common
{
    public class SubmitApprovalEntity
    {
        public long? agent_id { get; set; }

        public string process_code { get; set; }

        public string originator_user_id { get; set; }

        public long? dept_id { get; set; }

        public string approvers { get; set; }

        public List<Approval> form_component_values { get; set; }

    }


    public class Approval
    {
        public string name { get; set; }

        public string value { get; set; }

        public string ext_value { get; set; }
    }



    public class ApprovalReturn
    {
        public long errcode { get; set; }

        public string errmsg { get; set; }

        public string process_instance_id { get; set; }
    }

    /// <summary>
    /// 审批回调参数  
    /// </summary>
    public class ApprovalCallbackModel
    {
        /// <summary>
        /// 事件类型
        /// </summary>
        public string EventType { get; set; }

        /// <summary>
        /// 审批实例id
        /// </summary>
        public string processInstanceId { get; set; }

        /// <summary>
        /// 审批实例对应的企业
        /// </summary>
        public string corpId { get; set; }

        /// <summary>
        /// 实例创建时间
        /// </summary>
        public long? createTime { get; set; }

        /// <summary>
        /// 审批结束时间
        /// </summary>
        public long? finishTime { get; set; }

        /// <summary>
        /// 实例标题
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 审批正常结束（同意或拒绝）的type为finish，审批终止的type为terminate
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// 发起审批实例的员工
        /// </summary>
        public string staffId { get; set; }

        /// <summary>
        /// 审批实例url，可在钉钉内跳转到审批页面
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// 正常结束时result为agree，拒绝时result为refuse，审批终止时没这个值
        /// </summary>
        public string result { get; set; }

        /// <summary>
        /// 审批模板的唯一码
        /// </summary>
        public string processCode { get; set; }
    }
}
