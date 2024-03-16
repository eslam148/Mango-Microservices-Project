﻿using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utitlity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
  
namespace Mango.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ITokenProvider _tokenProvider;
 
        public AuthController(IAuthService authService, ITokenProvider tokenProvider)
        {
            _authService = authService;
            _tokenProvider=tokenProvider;
        }

        [HttpGet]
        public IActionResult Login()
        {
             return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto _loginRequestDto)
        {
            if (ModelState.IsValid)
            {
                ResponseDto responseDto = await _authService.LoginAsync(_loginRequestDto);
                 if (responseDto != null&& responseDto.IsSuccess)
                {
                    LoginResponseDto loginResponseDto = JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(responseDto.Result));
                    await SignInUser(loginResponseDto);
                    _tokenProvider.setToken(loginResponseDto.Token);

                    TempData["success"]="Log in successfully";
                    return RedirectToAction("Index","Home");

                }
                else
                {
                    ModelState.AddModelError("CustomError", responseDto.Message);
                    TempData["error"]=responseDto?.Message;
                    return View();
                }

            }
            return View();

        }

        public IActionResult Register()
        {
            var roleList = new List<SelectListItem>()
            {
                new SelectListItem{Text = SD.RoleCustomer,Value=SD.RoleAdmin },
                new SelectListItem{Text = SD.RoleAdmin,Value=SD.RoleAdmin},

            };
            ViewBag.RoleList = roleList;
             return View( );
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterationRequestDto obj)
        {
            if (ModelState.IsValid)
            {
                ResponseDto result = await _authService.RegisterAsync(obj);
                ResponseDto assingRole;
                if (result != null&& result.IsSuccess)
                {
                    if(string.IsNullOrEmpty(obj.roleName))
                    {
                        obj.roleName = SD.RoleCustomer;
                    }

                    assingRole = await _authService.AssignRoleAsync(obj);
                    if (assingRole != null&& assingRole.IsSuccess)
                    {
                        TempData["success"]="Registeration successfully";
                        return RedirectToAction(nameof(Login));
                    }
                        

                }
                else
                {
                    TempData["error"]=result?.Message;
                }
            }

            var roleList = new List<SelectListItem>()
            {
                new SelectListItem{Text = SD.RoleCustomer,Value=SD.RoleAdmin },
                new SelectListItem{Text = SD.RoleAdmin,Value=SD.RoleAdmin},

            };
            ViewBag.RoleList = roleList;
            return View(obj);
        }

         public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            _tokenProvider.ClearToken();
            return RedirectToAction("Index", "Home");
        }

        private async Task SignInUser(LoginResponseDto loginResponse)
        {

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(loginResponse.Token);
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);


            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email,jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub,jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name,jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name).Value));


            identity.AddClaim(new Claim(ClaimTypes.Name,jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(ClaimTypes.Role, jwt.Claims.FirstOrDefault(u => u.Type == "role").Value));

            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
    }
}
