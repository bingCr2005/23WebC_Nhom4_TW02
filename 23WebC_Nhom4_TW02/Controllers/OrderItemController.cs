using Microsoft.AspNetCore.Mvc;

namespace _23WebC_Nhom4_TW02.Controllers
{
    public class OrderItemController : Controller
    {
        private readonly IOrderItemDao _orderItemDao;

        public OrderItemController(IOrderItemDao orderItemDao)
        {
            _orderItemDao = orderItemDao;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
