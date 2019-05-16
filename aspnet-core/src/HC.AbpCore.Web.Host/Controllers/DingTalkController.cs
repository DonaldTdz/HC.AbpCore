using Abp.AspNetCore.Mvc.Controllers;
using DingTalk.Api;
using DingTalk.Api.Request;
using DingTalk.Api.Response;
using HC.AbpCore.Controllers;
using HC.AbpCore.DingTalk;
using HC.AbpCore.DingTalk.ApprovalCommon;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Senparc.CO2NET.HttpUtility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC.AbpCore.Web.Host.Controllers
{
    public class DingTalkController : AbpCoreControllerBase
    {
        private readonly IDingTalkManager _dingTalkManager;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public DingTalkController(
        IDingTalkManager dingTalkManager
        )
        {
            _dingTalkManager = dingTalkManager;
        }

        /// <summary>
        /// 创建审批事件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async virtual Task CreateApprovalCallbackEventAsync()
        {
            string accessToken = await _dingTalkManager.GetAccessTokenByAppAsync(DingDingAppEnum.智能办公);
            DefaultDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/call_back/register_call_back");
            OapiCallBackRegisterCallBackRequest request = new OapiCallBackRegisterCallBackRequest();
            request.Url = "http://hcpm.vaiwan.com/DingTalk/ApprovalCallbackTestAsync";
            request.AesKey = "1234567890123456789012345678901234567890123";
            request.Token = "123456";
            List<string> items = new List<string>();
            items.Add("bpms_instance_change");
            request.CallBackTag = items;
            OapiCallBackRegisterCallBackResponse response = client.Execute(request, accessToken);
        }


        /// <summary>
        /// 创建审批事件参数回调
        /// </summary>
        /// <param name="approvalCallbackTestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual Task<string> ApprovalCallbackTestAsync(ApprovalCallbackModel approvalCallbackModel)
        {
            var bb = approvalCallbackModel;
            return null;
        }
        //public async virtual Task<string> ApprovalCallbackTestAsync(ApprovalCallbackTestModel approvalCallbackTestModel)
        //{
        //    try
        //    {
        //        string mToken = "123456";
        //        string mSuiteKey = "";
        //        string mEncodingAesKey = "1234567890123456789012345678901234567890123";

        //        #region 回调url里面的参数
        //        //url中的签名
        //        //string msgSignature = _httpContext.HttpContext.Request.Query["signature"];
        //        string msgSignature = approvalCallbackTestModel.Signature;
        //        //url中的时间戳
        //        //string timeStamp = _httpContext.HttpContext.Request.Query["timestamp"];
        //        string timeStamp = approvalCallbackTestModel.timestamp;
        //        //url中的随机字符串
        //        //string nonce = _httpContext.HttpContext.Request.Query["nonce"];
        //        string nonce = approvalCallbackTestModel.nonce;
        //        //post数据包数据中的加密数据
        //        string encryptStr = GetPostParam(Request.GetRequestMemoryStream());
        //        #endregion
        //        string sEchoStr = "";

        //        #region 验证回调的url
        //        SuiteAuth suiteAuth = new SuiteAuth();

        //        //var ret = suiteAuth.VerifyURL(mToken, mEncodingAesKey, msgSignature, timeStamp, nonce, encryptStr,
        //        //    ref mSuiteKey);

        //        //if (ret != 0)
        //        //{
        //            //Helper.WriteLog("ERR: VerifyURL fail, ret: " + ret);
        //            //return null;
        //        //}
        //        #endregion
        //        #region
        //        //构造DingTalkCrypt
        //        DingTalkCrypt dingTalk = new DingTalkCrypt(mToken, mEncodingAesKey, mSuiteKey);

        //        string plainText = "";
        //        dingTalk.DecryptMsg(msgSignature, timeStamp, nonce, encryptStr, ref plainText);
        //        Hashtable tb = (Hashtable)JsonConvert.DeserializeObject(plainText, typeof(Hashtable));
        //        string eventType = tb["EventType"].ToString();
        //        string res = "success";
        //        Logger.InfoFormat("plainText:" + plainText);
        //        Logger.InfoFormat("eventType:" + eventType);
        //        switch (eventType)
        //        {
        //            case "suite_ticket"://定时推送Ticket
        //                //ConfigurationManager.AppSettings["SuiteTicket"] = tb["SuiteTicket"].ToString();
        //                mSuiteKey = tb["SuiteKey"].ToString();
        //                suiteAuth.SaveSuiteTicket(tb);
        //                break;
        //            case "tmp_auth_code"://钉钉推送过来的临时授权码
        //                //ConfigurationManager.AppSettings["TmpAuthCode"] = tb["AuthCode"].ToString();
        //                suiteAuth.SaveTmpAuthCode(tb);
        //                break;
        //            case "change_auth":// do something;
        //                break;
        //            case "check_update_suite_url":
        //                res = tb["Random"].ToString();
        //                mSuiteKey = tb["TestSuiteKey"].ToString();
        //                break;
        //        }

        //        timeStamp = GetTimeStamp().ToString();
        //        string encrypt = "";
        //        string signature = "";
        //        dingTalk = new DingTalkCrypt(mToken, mEncodingAesKey, mSuiteKey);
        //        dingTalk.EncryptMsg(res, timeStamp, nonce, ref encrypt, ref signature);
        //        Hashtable jsonMap = new Hashtable
        //        {
        //            {"msg_signature", signature},
        //            {"encrypt", encrypt},
        //            {"timeStamp", timeStamp},
        //            {"nonce", nonce}
        //        };
        //        string result = JsonConvert.SerializeObject(jsonMap);
        //        //context.Response.Write(result);
        //        return result;
        //        #endregion
        //    }
        //    catch (Exception ex)
        //    {
        //        //Helper.WriteLog(DateTime.Now + ex.Message);
        //        Logger.InfoFormat(DateTime.Now + ex.Message);
        //        return null;
        //    }
        //}


        public async virtual Task GetCallBack()
        {
            string accessToken = await _dingTalkManager.GetAccessTokenByAppAsync(DingDingAppEnum.智能办公);
            DefaultDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/call_back/get_call_back");
            OapiCallBackGetCallBackRequest request = new OapiCallBackGetCallBackRequest();
            request.SetHttpMethod("GET");
            OapiCallBackGetCallBackResponse response = client.Execute(request, accessToken);
        }

        public static double GetTimeStamp()
        {
            DateTime dt1 = Convert.ToDateTime("1970-01-01 00:00:00");
            TimeSpan ts = DateTime.Now - dt1;
            return Math.Ceiling(ts.TotalSeconds);
        }

        private string GetPostParam(Stream stream)
        {
            Stream sm = stream;//获取post正文
            int len = (int)sm.Length;//post数据长度
            byte[] inputByts = new byte[len];//字节数据,用于存储post数据
            sm.Read(inputByts, 0, len);//将post数据写入byte数组中
            sm.Close();//关闭IO流

            //**********下面是把字节数组类型转换成字符串**********

            string data = Encoding.UTF8.GetString(inputByts);//转为String
            data = data.Replace("{\"encrypt\":\"", "").Replace("\"}", "");
            return data;
        }
    }


}
