﻿using Abp.AspNetCore.Mvc.Controllers;
using DingTalk.Api;
using DingTalk.Api.Request;
using DingTalk.Api.Response;
using HC.AbpCore.Common;
using HC.AbpCore.Controllers;
using HC.AbpCore.DingTalk;
using HC.AbpCore.DingTalk.ApprovalCommon;
using HC.AbpCore.Reimburses.DomainService;
using HC.AbpCore.TimeSheets.DomainService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Senparc.CO2NET.Helpers;
using Senparc.CO2NET.HttpUtility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Top.Api.Util;

namespace HC.AbpCore.Web.Host.Controllers
{
    public class DingTalkController : AbpCoreControllerBase
    {
        private readonly IDingTalkManager _dingTalkManager;
        private readonly IReimburseManager _reimburseManager;
        private readonly ITimeSheetManager _timeSheetManager;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public DingTalkController(
        IDingTalkManager dingTalkManager,
        IReimburseManager reimburseManager,
        ITimeSheetManager timeSheetManager
        )
        {
            _timeSheetManager = timeSheetManager;
            _reimburseManager = reimburseManager;
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
            request.Url = "http://hcpm.vaiwan.com/DingTalk/ApprovalCallbackAsync";
            request.AesKey = "45skhqweass5232345IUJKWEDL5251054DSFdsuhfW2";
            request.Token = "123";
            List<string> items = new List<string>();
            items.Add("bpms_instance_change");
            request.CallBackTag = items;
            OapiCallBackRegisterCallBackResponse response = client.Execute(request, accessToken);
            return response;
        }


        /// <summary>
        /// 审批事件参数回调
        /// </summary>
        /// <param name="approvalCallbackTestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task ApprovalCallbackAsync(string signature, string timestamp, string nonce)
        {

            string token = "123";
            string suitekey = "ding6f6f3ad4521c207335c2f4657eb6378f";
            string aes_key = "45skhqweass5232345IUJKWEDL5251054DSFdsuhfW2";
            //post数据包数据中的加密数据
            var encryptStr = GetPostParam(Request.GetRequestMemoryStream());

            DingTalkCrypt dingTalk = new DingTalkCrypt(token, aes_key, suitekey);
            string sEchoStr = "";
            int ret = dingTalk.VerifyURL(signature, timestamp, nonce, encryptStr, ref sEchoStr);

            //解密接受信息，进行事件处理
            string plainText = "";
            ret = dingTalk.DecryptMsg(signature, timestamp, nonce, encryptStr, ref plainText);

            Hashtable tb = (Hashtable)JsonConvert.DeserializeObject(plainText, typeof(Hashtable));
            string eventType = tb["EventType"].ToString();
            string res = "success";
            switch (eventType)
            {
                case "bpms_instance_change"://审批实例开始与结束，执行代码
                    #region 审批实例开始与结束，执行代码
                    string type = tb["type"].ToString();
                    if (type == "start")//审批实例开始
                    {
                        return;
                    }
                    else if (type == "finish")//审批实例结束
                    {
                        string title = tb["title"].ToString();
                        string result = tb["result"].ToString();
                        string processInstanceId = tb["processInstanceId"].ToString();
                        if (title.Contains("报销"))
                        {
                            await _reimburseManager.UpdateReimburseByPIIdAsync(processInstanceId, result);
                            return;
                        }
                        else //工时
                        {
                            await _timeSheetManager.UpdateTimeSheetByPIIdAsync(processInstanceId, result);
                            return;
                        }
                    }
                    else//审批实例终止
                    {
                        string title = tb["title"].ToString();
                        //string result = tb["result"].ToString();
                        string processInstanceId = tb["processInstanceId"].ToString();
                        if (title.Contains("报销"))
                        {
                            await _reimburseManager.UpdateReimburseByPIIdAsync(processInstanceId, "");
                            return;
                        }
                        else //工时
                        {
                            await _timeSheetManager.UpdateTimeSheetByPIIdAsync(processInstanceId, "");
                            return;
                        }
                    }

                #endregion
                default:
                    break;
            }

            timestamp = GetTimeStamp().ToString();
            string encrypt = "";
            string signature2 = "";
            dingTalk = new DingTalkCrypt(token, aes_key, suitekey);
            ret = dingTalk.EncryptMsg(res, timestamp, nonce, ref encrypt, ref signature2);
            Hashtable jsonMap = new Hashtable
                {
                    {"msg_signature", signature2},
                    {"encrypt", encrypt},
                    {"timeStamp", timestamp},
                    {"nonce", nonce}
                };
            var bbs = SerializerHelper.GetJsonString(jsonMap);
            await HttpContext.Response.WriteAsync(bbs);

        }

        /// <summary>
        /// 查询已创建的回调事件
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<OapiCallBackGetCallBackResponse> GetCallBackAsync()
        {
            string accessToken = await _dingTalkManager.GetAccessTokenByAppAsync(DingDingAppEnum.智能办公);
            DefaultDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/call_back/get_call_back");
            //OapiCallBackGetCallBackResponse response = Get.GetJson<OapiCallBackGetCallBackResponse>(string.Format("https://oapi.dingtalk.com/call_back/get_call_back?accessToken={0}", accessToken));
            OapiCallBackGetCallBackRequest request = new OapiCallBackGetCallBackRequest();
            request.SetHttpMethod("GET");
            OapiCallBackGetCallBackResponse response = client.Execute(request, accessToken);
            return response;
        }

        /// <summary>
        /// 更新已创建的回调事件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<OapiCallBackUpdateCallBackResponse> UpdateCallBackAsync(string requestUrl)
        {
            string accessToken = await _dingTalkManager.GetAccessTokenByAppAsync(DingDingAppEnum.智能办公);
            //var url = string.Format("https://oapi.dingtalk.com/call_back/update_call_back?access_token={0}", accessToken);
            DefaultDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/call_back/update_call_back");
            OapiCallBackUpdateCallBackRequest request = new OapiCallBackUpdateCallBackRequest();
            request.Url = requestUrl;
            request.AesKey = "45skhqweass5232345IUJKWEDL5251054DSFdsuhfW2";
            request.Token = "123";
            List<string> items = new List<string>();
            items.Add("bpms_instance_change");
            request.CallBackTag = items;
            var rq = SerializerHelper.GetJsonString(request, null);
            OapiCallBackUpdateCallBackResponse response = client.Execute(request, accessToken);
            return response;
        }

        /// <summary>
        /// 删除已创建的回调事件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<OapiCallBackDeleteCallBackResponse> DeleteCallBackAsync()
        {
            string accessToken = await _dingTalkManager.GetAccessTokenByAppAsync(DingDingAppEnum.智能办公);
            DefaultDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/call_back/delete_call_back");
            OapiCallBackDeleteCallBackRequest request = new OapiCallBackDeleteCallBackRequest();
            request.SetHttpMethod("GET");
            OapiCallBackDeleteCallBackResponse response = client.Execute(request, accessToken);
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


        /// <summary>
        /// 上传媒体文件
        /// </summary>
        /// <param name="fileUrl"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> UploadMediaAsync(string path)
        {
            string accessToken = await _dingTalkManager.GetAccessTokenByAppAsync(DingDingAppEnum.智能办公);
            var url = string.Format("https://oapi.dingtalk.com/media/upload?access_token={0}&type=image", accessToken);
            string mediaId = PostMediaAsync(url, path);
            return mediaId;
        }

        public string PostMediaAsync(string url, string filePath)
        {
            var result = string.Empty;
            var request = (HttpWebRequest)WebRequest.Create(url);
            var boundary = "----------" + DateTime.Now.Ticks.ToString("x");
            request.ContentType = "multipart/form-data; boundary=" + boundary;
            request.Method = "POST";
            using (Stream requestStream = request.GetRequestStream())
            {
                byte[] boundarybytes = Encoding.UTF8.GetBytes("--" + boundary + "\r\n");
                byte[] trailer = Encoding.UTF8.GetBytes("\r\n--" + boundary + "–-\r\n");
                var filename = Path.GetFileName(filePath);
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    byte[] bArr = new byte[fs.Length];
                    fs.Read(bArr, 0, bArr.Length);
                    requestStream.Write(boundarybytes, 0, boundarybytes.Length);
                    var header = $"Content-Disposition:form-data;name=\"media\";filename=\"{filename}\"\r\nfilelength=\"{fs.Length}\"\r\nContent-Type:application/octet-stream\r\n\r\n";
                    byte[] postHeaderBytes = Encoding.UTF8.GetBytes(header.ToString());
                    requestStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);
                    fs.Close();
                    requestStream.Write(bArr, 0, bArr.Length);
                    requestStream.Write(trailer, 0, trailer.Length);
                }
            }
            var response = (HttpWebResponse)request.GetResponse();
            var responseStream = response.GetResponseStream();
            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }
            return result;
        }
    }


}
