using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Service.DTOs;
using Service.Interfaces;
using System.Security.Claims;

namespace StudentManagementSystem.Controllers
{
    [AllowAnonymous]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO login)
        {
            return Ok(await _userService.Login(login));
        }

        [HttpPut("ChangePassword")]

        public async Task<IActionResult> ChangePassword([FromBody] string newPassword)
        {
            string email = User.FindFirst(ClaimTypes.Email)?.Value;
            return Ok(await _userService.ChangePassword(newPassword, email));
        }

        [Authorize]
        [HttpGet("GetUserDetails")]
        public async Task<IActionResult> GetUserDetail()
        {
            var userId = int.Parse(User.FindFirst("UserId")?.Value);
            return Ok(await _userService.GetUserDetail(userId));
        }

    }
}
