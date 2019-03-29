using Abp.Application.Services;
using HC.AbpCore.Dtos;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HC.AbpCore.Wechat.Menus
{
    public interface IWechatMenuAppService : IApplicationService
    {
        Task<APIResultDto> CreateMenu(GetMenuResultFull fullJson);

        GetMenuResult GetMenu();
    }
}
