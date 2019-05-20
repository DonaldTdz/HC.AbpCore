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
        public async Task<OapiCallBackRegisterCallBackResponse> CreateApprovalCallbackEventAsync()
        {
            string accessToken = await _dingTalkManager.GetAccessTokenByAppAsync(DingDingAppEnum.智能办公);
            DefaultDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/call_back/register_call_back");
            OapiCallBackRegisterCallBackRequest request = new OapiCallBackRegisterCallBackRequest();
            request.Url = "http://hcpmabc.vaiwan.com/DingTalk/ApprovalCallbackTestHCAsync";
            request.AesKey = "45skhqweass5232345IUJKWEDL5251054DSFdsuhfW2";
            request.Token = "123";
            List<string> items = new List<string>();
            items.Add("bpms_instance_change");
            request.CallBackTag = items;
            OapiCallBackRegisterCallBackResponse response = client.Execute(request, accessToken);
            return response;
        }


        /// <summary>
        /// 创建审批事件参数回调
        /// </summary>
        /// <param name="approvalCallbackTestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ApprovalCallbackTestHCAsync(string msgSignature, string timestamp, string nonce)
        {

            string mToken = "123";
            string mSuiteKey = "ding6f6f3ad4521c207335c2f4657eb6378f";
            string mEncodingAesKey = "45skhqweass5232345IUJKWEDL5251054DSFdsuhfW2";

            //post数据包数据中的加密数据
            var encryptStr = GetPostParam(Request.GetRequestMemoryStream());

            string newSignature = "";
            DingTalkCrypt.GenerateSignature(mToken, timestamp, nonce, encryptStr, ref newSignature);
            if (msgSignature != newSignature)
            {
                Logger.Error("消息有可能被篡改！签名验证错误！ ");
            }

            var sReplyEchoStr = "";
            DingTalkCrypt suiteAuth = new DingTalkCrypt(mToken, mEncodingAesKey, mSuiteKey);
            var ret = suiteAuth.VerifyURL(msgSignature, timestamp, nonce, encryptStr, ref sReplyEchoStr);

            DingTalkCrypt dingTalk = new DingTalkCrypt(mToken, mEncodingAesKey, mSuiteKey);
            string plainText = "";
            dingTalk.DecryptMsg(msgSignature, timestamp, nonce, encryptStr, ref plainText);
            Hashtable tb = (Hashtable)JsonConvert.DeserializeObject(plainText, typeof(Hashtable));
            string eventType = tb["EventType"].ToString();
            string result = "";
            switch (eventType)
            {
                case "bpms_task_change"://审批通知
                    break;
                case "bpms_instance_change"://审批通知
                    break;
                case "check_url"://测试url
                    string encrypt = "";
                    string signature = "";
                    dingTalk = new DingTalkCrypt(mToken, mEncodingAesKey, mSuiteKey);
                    dingTalk.EncryptMsg("success", timestamp, nonce, ref encrypt, ref signature);
                    Hashtable json = new Hashtable
                        {
                            {"msg_signature", signature},
                            {"timeStamp", timestamp},
                            {"nonce", nonce},
                            {"encrypt", encrypt}
                        };
                    result = JsonConvert.SerializeObject(json);
                    return Json(json);
            }
            return null;
            //接收encrypt参数
            //string encryptStr = GetPostParam(Request.GetRequestMemoryStream());
            //注册时填写的token、aes_key、suitekey
            //string token = "123";
            //token = "123456";//钉钉测试文档中的token
            //string aes_key = "45skhqweass5232345IUJKWEDL5251054DSFdsuhfW2";
            //aes_key = "4g5j64qlyl3zvetqxz5jiocdr586fn2zvjpa8zls3ij";//钉钉测试文档中的aes_key
            //string suitekey = "";
            //string suitekey = "ding6f6f3ad4521c207335c2f4657eb6378f";

            //suitekey = "suite4xxxxxxxxxxxxxxx";//钉钉测试文档中的suitekey

            //#region 验证回调的url
            //DingTalkCrypt dingTalk = new DingTalkCrypt(token, aes_key, suitekey);
            //string sEchoStr = "";
            //int ret = dingTalk.VerifyURL(signature, timestamp, nonce, encryptStr, ref sEchoStr);
            //#endregion

            //#region 解密接受信息，进行事件处理
            //string plainText = "";
            //ret = dingTalk.DecryptMsg(signature, timestamp, nonce, encryptStr, ref plainText);

            //Hashtable tb = (Hashtable)JsonConvert.DeserializeObject(plainText, typeof(Hashtable));
            //string eventType = tb["EventType"].ToString();
            //string res = "success";
            //switch (eventType)
            //{
            //    case "user_modify_org"://用户信息修改，执行代码
            //        #region 用户信息修改，执行代码

            //        #endregion
            //        break;
            //    default:
            //        break;
            //}

            //timestamp = GetTimeStamp().ToString();
            //string encrypt = "";
            //string signature2 = "";
            //dingTalk = new DingTalkCrypt(token, aes_key, suitekey);
            //ret = dingTalk.EncryptMsg(res, timestamp, nonce, ref encrypt, ref signature2);
            //Hashtable jsonMap = new Hashtable
            //    {
            //        {"msg_signature", signature2},
            //        {"encrypt", encrypt},
            //        {"timeStamp", timestamp},
            //        {"nonce", nonce}
            //    };
            //string bb = "";
            //var aa = dingTalk.DecryptMsg(signature2, timestamp, nonce, encrypt, ref bb);
            //return Json(jsonMap);
            //#endregion

        }

        /// <summary>
        /// 查询已创建事件
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<OapiCallBackGetCallBackResponse> GetCallBackAsync()
        {
            string accessToken = await _dingTalkManager.GetAccessTokenByAppAsync(DingDingAppEnum.智能办公);
            DefaultDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/call_back/get_call_back");
            OapiCallBackGetCallBackRequest request = new OapiCallBackGetCallBackRequest();
            request.SetHttpMethod("GET");
            OapiCallBackGetCallBackResponse response = client.Execute(request, accessToken);
            return response;
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
