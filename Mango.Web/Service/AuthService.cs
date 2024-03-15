using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utitlity;

namespace Mango.Web.Service
{
    public class AuthService : IAuthService
    {
          private readonly IBaseService _baseService;
        public AuthService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public async Task<ResponseDto> AssignRoleAsync(RegisterationRequestDto registerationRequest)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = registerationRequest,
                Url = SD.AuthApiBase+"/api/auth/AssignRole"
            });
        }

        public async Task<ResponseDto> LoginAsync(LoginRequestDto loginRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = loginRequestDto,
                Url = SD.AuthApiBase+"/api/auth/login"
            });
        }

        public async Task<ResponseDto> RegisterAsync(RegisterationRequestDto registerationRequest)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = registerationRequest,
                Url = SD.AuthApiBase+"/api/auth/register"
            });
        }
    }
}
