using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GoogleSheetI18n.Api.SimpleWebApi.Features.Account
{
    public class LoginModel
    {
        [Required]
        public string Username { get; set; }
        
        [Required]
        public string Password { get; set; }
    }

    public class UserAuth
    {
        public string Username { get; set; }
        public string Token { get; set; }
        public List<UserClaim> Claims { get; set; } = new ();
    }

    public class UserClaim
    {
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }
}
