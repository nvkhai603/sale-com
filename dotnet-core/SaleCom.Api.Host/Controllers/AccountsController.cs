﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SaleCom.Application.Contracts.Accounts;
using System.Threading.Tasks;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace SaleCom.Api.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;   
        }

        [HttpPost("register")]
        public async Task<IdentityResult> Register(RegisterAccount input) {
            return await _accountService.RegisterAccountAsync(input);
        }

        [HttpGet("active")]
        public async Task<IdentityResult> ActiveAccountByEmailToken(string email,string token)
        {
            return await _accountService.ActiveAccountByEmailTokenAsync(email,token);
        }

        [HttpPost("login")]
        public async Task<SignInResult> Login(Login input)
        {
            return await _accountService.SignInAsync(input);
        }

        [HttpGet("reset-password")]
        public async Task<IActionResult> ResetPassword(string email) {
            await _accountService.ResetPasswordAsync(email);
            return Ok();
        }

        [HttpGet("session")]
        [Authorize]
        public async Task<SessionData> Session()
        {
            return await _accountService.GetSessionDataAsync();
        }

        [HttpGet("logout")]
        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            await _accountService.LogOutAsync();
            return Ok();
        }
    }
}
