using System;
using System.Collections.Generic;
using System.Text;

namespace HC.AbpCore.Common
{
    public class SubmitApprovalEntity
    {
        public double? agent_id { get; set; }

        public string process_code { get; set; }

        public string originator_user_id { get; set; }

        public double? dept_id { get; set; }

        public string approvers { get; set; }

        public List<Approval> form_component_values { get; set; }

    }


    public class Approval
    {
        public string Name { get; set; }

        public string Value { get; set; }
    }



    public class ApprovalReturn
    {
        public int errcode { get; set; }

        public string errmsg { get; set; }

        public string process_instance_id { get; set; }
    }
}
