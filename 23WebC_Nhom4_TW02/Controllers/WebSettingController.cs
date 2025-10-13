using Microsoft.AspNetCore.Mvc;

namespace _23WebC_Nhom4_TW02.Controllers
{
    public class WebSettingController : Controller
    {
        private readonly IWebSettingDao _webSettingDao;

        public WebSettingController(IWebSettingDao webSettingDao)
        {
            _webSettingDao = webSettingDao;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
