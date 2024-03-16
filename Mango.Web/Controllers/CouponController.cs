using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Reflection;

namespace Mango.Web.Controllers
{
     public class CouponController : Controller
    {
        private readonly ICouponService _couponService;
        public CouponController(ICouponService couponService)
        {
            _couponService = couponService;
        }

        public async Task<IActionResult> CouponIndex()
        {
            List<CouponDto>? list = new();
            ResponseDto? response = await _couponService.GetAllCouponAsync();

            if (response != null&& response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<CouponDto>>(Convert.ToString(response.Result));
                TempData["success"]="successfully";
            }
            else
            {
                TempData["error"]=response?.Message;
            }
            return View(list);
        }
        public async Task<IActionResult> CouponCreate()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CouponCreate(CouponDto model)
        {
 
            if (ModelState.IsValid)
            {
                ResponseDto? response = await _couponService.CreateCouponAsync(model);

                if (response != null&& response.IsSuccess)
                {
                    TempData["success"]="successfully";
                    return RedirectToAction(nameof(CouponIndex));

                }
                else
                {
                    TempData["error"]=response?.Message;
                }
            }

            
            return View(model);
        }
        public async Task<IActionResult> CouponDelete(int couponId)
        {
            ResponseDto? response = await _couponService.GetCouponByIdAsync(couponId);

            if (response != null&& response.IsSuccess)
            {
               CouponDto? model= JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(response.Result));
                TempData["success"]="successfully";

                return View(model);
            }
            else
            {
                TempData["error"]=response?.Message;
                return View( );

            }
         }

        [HttpPost]
        public async Task<IActionResult> CouponDelete(CouponDto couponDto)
        {
 
            if (ModelState.IsValid)
            {
                ResponseDto? response = await _couponService.DeleteCouponAsync(couponDto.CouponId);

                if (response != null&& response.IsSuccess)
                {
                    TempData["success"]="successfully";

                    return RedirectToAction(nameof(CouponIndex));
                }
                else
                {
                    TempData["error"]=response?.Message;
                }
            }
           

            return RedirectToAction(nameof(CouponIndex));
        }
    }
}
