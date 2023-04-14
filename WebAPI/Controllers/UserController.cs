using Business.Abstract;
using Business.Constants;
using Core.Utilities.Results;
using Core.Utilities.Security.Jwt;
using Entities.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        IUserService _userService;
        IGetTokenService _tokenService;

        public UserController(IUserService userService, IGetTokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterDto registerDto)
        {
            var result = _userService.Register(registerDto);
            return Ok(result);
        }

        [HttpPost("login/username")]
        public IActionResult LoginUserName(NameLoginDto nameLoginDto)
        {
            var userToLogin = _userService.LoginUserName(nameLoginDto.UserName, nameLoginDto.Password);
            if (!userToLogin.Success)
            {
                return BadRequest(userToLogin.Message);
            }

            var result = _userService.CreateAccessToken(userToLogin.Data);
            return Ok(result);

        }

        [HttpPost("login/mail")]
        public IActionResult LoginMail(EmailLoginDto emailLoginDto)
        {
            var userToLogin = _userService.LoginMail(emailLoginDto.Email, emailLoginDto.Password);
            if (!userToLogin.Success)
            {
                return BadRequest(userToLogin.Message);
            }

            var result = _userService.CreateAccessToken(userToLogin.Data);
            return Ok(result);
        }

        [HttpPost("update/password")]
        public IActionResult ChangePassword(ChangePasswordDto changePasswordDto)
        {
            if (changePasswordDto.NewPassword != changePasswordDto.ConfirmPassword)
            {
                return BadRequest(Messages.PasswordNotMatch);
            }
            var result = _userService.ChangePassword(changePasswordDto.NewPassword);
            return Ok(result);
        }

        [HttpGet("get/library")]
        public IActionResult GetUserLibrary(int userId)
        {
            var result = _userService.GetById(userId);
            return Ok(result);
        }

        //[HttpPost("follow/user")]
        //public IActionResult FollowUser(int userId) 
        //{
            
        //}


    }
}
