using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace _23WebC_Nhom4_TW02.Filters
{
    public class AdminAuthFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var session = context.HttpContext.Session.GetString("AdminUser");
            if (string.IsNullOrEmpty(session))
            {
                context.Result = new RedirectToActionResult("Index", "AdminLogin", new { area = "Admin" });
            }
            base.OnActionExecuting(context);
        }
    }
}
