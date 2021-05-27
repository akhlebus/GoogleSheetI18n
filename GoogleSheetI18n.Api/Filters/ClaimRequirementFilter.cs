using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GoogleSheetI18n.Api.Filters
{
    public class ClaimRequirementFilter : IAuthorizationFilter
    {
        private readonly Claim _claim;

        public ClaimRequirementFilter(Claim claim)
        {
            _claim = claim;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var hasClaim = context.HttpContext.User.Claims.Any(c =>
                c.Type == _claim.Type &&
                (_claim.Value is not null and not "" || c.Value == _claim.Value)
            );
            if (!hasClaim)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
