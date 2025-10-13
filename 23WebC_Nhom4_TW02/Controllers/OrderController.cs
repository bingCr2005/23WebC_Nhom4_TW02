using Microsoft.AspNetCore.Mvc;

namespace _23WebC_Nhom4_TW02.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderDao _orderDao;

        public OrderController(IOrderDao orderDao)
        {
            _orderDao = orderDao;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
