using System;
using System.Collections.Generic;
using System.Text;

namespace HC.AbpCore.DingTalk.ApprovalCommon
{
    public class ApprovalCallbackTestModel
    {
        public string Signature { get; set; }

        public string timestamp { get; set; }

        public string nonce { get; set; }
    }
}
