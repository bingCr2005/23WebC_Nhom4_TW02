using Microsoft.AspNetCore.Mvc;

namespace _23WebC_Nhom4_TW02.Controllers
{
    public class TagController : Controller
    {
        private readonly ITagDao _tagDao;

        public TagController(ITagDao tagDao)
        {
            _tagDao = tagDao;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
