using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Clinic.Controllers
{
    public class CustomAuthorizationFilter : Attribute, IAuthorizationFilter
    {
        private readonly string[] _roles;

        public CustomAuthorizationFilter(params string[] roles)
        {
            _roles = roles;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Проверяем, авторизован ли пользователь
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                // Если не авторизован, возвращаем страницу с сообщением "Авторизируйтесь"
                context.Result = new ViewResult { ViewName = "Unauthorized" };
                return;
            }

            // Проверяем, есть ли у пользователя одна из требуемых ролей
            foreach (var role in _roles)
            {
                if (context.HttpContext.User.IsInRole(role))
                {
                    // Если у пользователя есть одна из требуемых ролей, разрешаем доступ
                    return;
                }
            }

            // Если у пользователя нет ни одной из требуемых ролей, возвращаем страницу с сообщением "Недостаточно прав"
            context.Result = new ViewResult { ViewName = "InsufficientPermissions" };
        }
    }
    public class AnonymousOnlyAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                // Если пользователь авторизован, перенаправить его на главную страницу или другую страницу по умолчанию
                context.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }
        }
    }
}