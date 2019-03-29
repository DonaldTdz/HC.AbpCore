using Microsoft.AspNetCore.Antiforgery;
using HC.AbpCore.Controllers;

namespace HC.AbpCore.Web.Host.Controllers
{
    public class AntiForgeryController : AbpCoreControllerBase
    {
        private readonly IAntiforgery _antiforgery;

        public AntiForgeryController(IAntiforgery antiforgery)
        {
            _antiforgery = antiforgery;
        }

        public void GetToken()
        {
            _antiforgery.SetCookieTokenAndHeader(HttpContext);
        }
    }
}
