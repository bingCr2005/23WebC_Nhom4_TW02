using Microsoft.AspNetCore.Mvc;
using static _23WebC_Nhom4_TW02.PostDAO;

namespace _23WebC_Nhom4_TW02.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostDao _postDao;

        public PostController(IPostDao postDao)
        {
            _postDao = postDao;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
