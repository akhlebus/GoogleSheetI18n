using System.Security.Claims;
using GoogleSheetI18n.Api.Filters;
using Microsoft.AspNetCore.Mvc;

namespace GoogleSheetI18n.Api.Attributes
{
    public class ClaimRequirementAttribute : TypeFilterAttribute
    {
        public ClaimRequirementAttribute(string claimType, string claimValue) : base(typeof(ClaimRequirementFilter))
        {
            Arguments = new object[] { new Claim(claimType, claimValue) };
        }
    }
}