using Business.Abstract;
using Business.Constants;
using Core.Utilities.Results;
using Core.Utilities.Security.Jwt;
using Entities.Dtos;
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
            if(result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("gettoken")]
        public async Task<IDataResult<GetAccessTokenDto>> GetAccessToken()
        {
            var result = await _tokenService.GetAccessToken();
            if(result == null)
            {
                return new ErrorDataResult<GetAccessTokenDto>(Messages.AccessTokenError);
            }
            return new SuccessDataResult<GetAccessTokenDto>(result.Data, Messages.AccessTokenCreated);
        }
    }
}
