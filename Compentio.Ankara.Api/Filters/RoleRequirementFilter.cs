namespace Compentio.Ankara.Api.Filters
{
    using Compentio.Ankara.Models.Users;
    using Compentio.Ankara.Session;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using System;
    using System.Linq;

    public class RoleAuthorizationFilter : IAuthorizationFilter
    {
        readonly Roles[] _roles;

        public RoleAuthorizationFilter(params Roles[] roles)
        {
            _roles = roles;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (!context.HttpContext.Session.IsSessionActive())
            {
                context.Result = new ForbidResult();
                return;
            }

            var user = context.HttpContext.Session.GetCurrentUser();
            var hasRole = user.Roles?.Any(role => _roles.Any(r => r == role.Role));

            if (!hasRole.HasValue || (hasRole.HasValue && !hasRole.Value))
            {
                context.Result = new ForbidResult();
            }
        }
    }

    public class RoleAuthorizationAttribute : TypeFilterAttribute
    {
        public RoleAuthorizationAttribute(params Roles[] roles) : base(typeof(RoleAuthorizationFilter))
        {
            Arguments = new object[] { roles };
        }
    }
}
