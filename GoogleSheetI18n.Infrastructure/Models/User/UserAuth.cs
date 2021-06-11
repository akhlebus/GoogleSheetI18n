using System.Collections.Generic;

namespace GoogleSheetI18n.Core.Models.User
{
    public class UserAuth
    {
        public string Username { get; set; }
        public string Token { get; set; }
        public List<UserClaim> Claims { get; set; } = new();
    }
}
