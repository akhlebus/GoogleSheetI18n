﻿using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using GoogleSheetI18n.Api.Integrations.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoogleSheetI18n.Api.SimpleWebApi.Features.Account
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (!await _userService.ValidateCredentials(model.Username, model.Password, out var user))
            {
                return BadRequest();
            }

            var claims = new List<Claim>
            {
                new (ClaimTypes.NameIdentifier, model.Username),
                new ("name", model.Username),
                new (I18nClaimTypes.Admin, "")
            };

            var authUser = TokenUtils.BuildUserAuthObject(user, claims);

            return Ok(authUser);
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            return Ok();
        }
    }
}
