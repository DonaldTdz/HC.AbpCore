using Abp.Domain.Repositories;
using HC.AbpCore.DingTalk.DingTalkConfigs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Senparc.CO2NET.HttpUtility;
using HC.AbpCore.DingTalk.Dtos;
using HC.AbpCore.PaymentPlans.DomainService;
using Abp.Auditing;
using HC.AbpCore.Reimburses.DomainService;
using HC.AbpCore.TimeSheets.DomainService;

namespace HC.AbpCore.DingTalk
{
    public class DingTalkManager : AbpCoreDomainServiceBase, IDingTalkManager
    {
        private readonly IRepository<DingTalkConfig, int> _dingTalkConfigRepository;
        private readonly IPaymentPlanManager _paymentPlanManager;
        private readonly IReimburseManager _reimburseManager;
        private readonly ITimeSheetManager _timeSheetManager;

        public DingTalkManager(IRepository<DingTalkConfig, int> dingTalkConfigRepository,
            IPaymentPlanManager paymentPlanManager,
            ITimeSheetManager timeSheetManager,
            IReimburseManager reimburseManager)
        {
            _timeSheetManager = timeSheetManager;
            _reimburseManager = reimburseManager;
            _paymentPlanManager = paymentPlanManager;
            _dingTalkConfigRepository = dingTalkConfigRepository;
        }

        /// <summary>
        /// 工作消息通知  每天早上9点提醒
        /// </summary>
        /// <returns></returns>
        public async Task AutoWorkNotificationMessageAsync()
        {
            var accessToken = await GetAccessTokenByAppAsync(DingDingAppEnum.智能办公);
            var ddConfig = await GetDingDingConfigByAppAsync(DingDingAppEnum.智能办公);
            //催款提醒
            await _paymentPlanManager.PaymentRemindAsync(accessToken, ddConfig);
            //报销审批提醒
            await _reimburseManager.ReimburseApprovalRemind(accessToken, ddConfig);
            //工时报销提醒
            await _timeSheetManager.TimeSheetApprovalRemind(accessToken, ddConfig);
        }

        public async Task<string> GetAccessTokenByAppAsync(DingDingAppEnum app)
        {
            var config = await GetDingDingConfigByAppAsync(app);
            DingAccessToken accessToken = Get.GetJson<DingAccessToken>(string.Format("https://oapi.dingtalk.com/gettoken?appkey={0}&appsecret={1}", config.Appkey, config.Appsecret));
            Logger.InfoFormat("AccessToken response errmsg:{0} body:{1}", accessToken.errmsg, accessToken.access_token);
            return accessToken.access_token;
        }

        public async Task<DingDingAppConfig> GetDingDingConfigByAppAsync(DingDingAppEnum app)
        {
            DingDingAppConfig config = new DingDingAppConfig();
            var configList = new List<DingTalkConfig>();
            switch (app)
            {
                case DingDingAppEnum.智能办公:
                    {
                        configList = await _dingTalkConfigRepository.GetAll()
                            .Where(d => d.Type == DingDingTypeEnum.公共配置 || d.Type == DingDingTypeEnum.智能办公)
                            .AsNoTracking()
                            .ToListAsync();
                    }
                    break;
                default:
                    break;
            }

            foreach (var item in configList)
            {
                if (item.Code.ToLower() == DingDingConfigCode.CorpId.ToLower())
                {
                    config.CorpId = item.Value;
                }
                else if (item.Code.ToLower() == DingDingConfigCode.Appkey.ToLower())
                {
                    config.Appkey = item.Value;
                }
                else if (item.Code.ToLower() == DingDingConfigCode.Appsecret.ToLower())
                {
                    config.Appsecret = item.Value;
                }
                else if (item.Code.ToLower() == DingDingConfigCode.AgentID.ToLower())
                {
                    int outAgenId = 0;
                    if (int.TryParse(item.Value, out outAgenId))
                    {
                        config.AgentID = outAgenId;
                    }
                }
            }

            return config;
        }

        public string GetUserId(string accessToken, string code)
        {
            DingUserInfoDto user = Get.GetJson<DingUserInfoDto>(string.Format("https://oapi.dingtalk.com/user/getuserinfo?access_token={0}&code={1}", accessToken, code));
            Logger.InfoFormat("Userid response errmsg:{0} body:{1}", user.errmsg, user.userid);
            return user.userid;
        }
    }
}
