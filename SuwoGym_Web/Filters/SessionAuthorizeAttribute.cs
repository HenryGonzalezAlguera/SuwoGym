using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SuwoGym_Web.Filters
{
    public class SessionAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(
            ActionExecutingContext context)
        {
            var autenticado =
                context.HttpContext.Session
                    .GetString("Autenticado");

            if (string.IsNullOrEmpty(autenticado))
            {
                context.Result =
                    new RedirectToActionResult(
                        "Index",
                        "Home",
                        null);
            }

            base.OnActionExecuting(context);
        }
    }
}