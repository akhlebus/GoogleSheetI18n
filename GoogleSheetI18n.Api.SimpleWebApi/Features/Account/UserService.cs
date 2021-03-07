﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoogleSheetI18n.Api.SimpleWebApi.Features.Account
{
    public class UserService : IUserService
    {
        private readonly IDictionary<string, (string Password, User User)> _users = 
            new Dictionary<string, (string Password, User User)>();

        public UserService(IDictionary<string, string> credentials)
        {
            foreach (var cred in credentials)
            {
                // _users.Add(cred.Key.ToLower(), (BCrypt.Net.BCrypt.Hashpassword(cred.Value), new User(cred.Key)));
                _users.Add(cred.Key.ToLower(),(cred.Value, new User(cred.Key)));
            }
        }
        public Task<bool> ValidateCredentials(string userName, string password, out User user)
        {
            user = null;
            var key = userName.ToLower();

            if (!_users.ContainsKey(key))
            {
                return Task.FromResult(false);
            }

            // if(BCrypt.Net.BCrypt.Verify(password, _users[key].password))
            if (password != _users[key].Password)
            {
                return Task.FromResult(false);
            }

            user = _users[key].User;
            return Task.FromResult(true);
        }
    }
}
