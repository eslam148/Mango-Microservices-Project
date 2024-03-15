using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utitlity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace Mango.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
        _authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDto loginRequestDto = new();
            return View(loginRequestDto);
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto _loginRequestDto)
        {
            if (ModelState.IsValid)
            {
                ResponseDto responseDto = await _authService.LoginAsync(_loginRequestDto);
                 if (responseDto != null&& responseDto.IsSuccess)
                {
                    LoginRequestDto loginRequestDto = JsonConvert.DeserializeObject<LoginRequestDto>(Convert.ToString(responseDto.Result));
                    TempData["success"]="Registeration successfully";
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

        [HttpPost]
        public IActionResult Logout()
        {
             return View();
        }
    }
}
