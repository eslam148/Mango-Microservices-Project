using Mango.Services.AuthAPI.Models.Dto;
using Mango.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.AuthAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        protected ResponseDto _responseDto;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
            _responseDto = new();
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterationRequestDto model)
        {
            var errorMessage = await _authService.Register(model);
            if(!string.IsNullOrEmpty(errorMessage))
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = errorMessage;
                return BadRequest(_responseDto);
            }
            return Ok(_responseDto);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {
            var loginResponse = await _authService.Login(model);
            if(loginResponse.User == null)
            {
                _responseDto.IsSuccess=false;
                _responseDto.Message = "Username or Password is incorrect";
                return BadRequest(_responseDto);
            }
            _responseDto.Result = loginResponse;
            return Ok(_responseDto);
        }
        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRole([FromBody] RegisterationRequestDto model)
        {
            var assignRoleSuccessful = await _authService.AssignRole(model.Email,model.roleName.ToUpper());
            if (!assignRoleSuccessful)
            {
                _responseDto.IsSuccess=false;
                _responseDto.Message = "Error Encounterd";
                return BadRequest(_responseDto);
            }
             return Ok(_responseDto);
        }
    }
}
