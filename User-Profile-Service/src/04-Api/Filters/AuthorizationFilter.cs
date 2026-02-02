using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace User_Profile_Service.src._04_Api.Filters
{
    public class AuthorizationFilter : ActionFilterAttribute
    {
        private readonly string _requiredRole;

        public AuthorizationFilter(string requiredRole = "User")
        {
            _requiredRole = requiredRole;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            if (!string.IsNullOrEmpty(_requiredRole))
            {
                var hasRole = context.HttpContext.User.IsInRole(_requiredRole);
                if (!hasRole)
                {
                    context.Result = new ForbidResult();
                    return;
                }
            }

            base.OnActionExecuting(context);
        }
    }
}
