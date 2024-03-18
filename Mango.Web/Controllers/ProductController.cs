using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mango.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
         }

        public async Task<IActionResult> ProductIndex()
        {
            List<ProductDto>? list = new();
            ResponseDto? response = await _productService.GetAllProductAsync();

            if (response != null&& response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result));
                TempData["success"]="successfully";
            }
            else
            {
                TempData["error"]=response?.Message;
            }
            return View(list);
        }
        public async Task<IActionResult> ProductCreate()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ProductCreate(ProductDto model)
        {

            if (ModelState.IsValid)
            {
                ResponseDto? response = await _productService.CreateProductAsync(model);

                if (response != null&& response.IsSuccess)
                {
                    TempData["success"]="successfully";
                    return RedirectToAction(nameof(ProductIndex));

                }
                else
                {
                    TempData["error"]=response?.Message;
                }
            }


            return View(model);
        }
        public async Task<IActionResult> ProductDelete(int ProductId)
        {
            ResponseDto? response = await _productService.GetProductByIdAsync(ProductId);

            if (response != null&& response.IsSuccess)
            {
                ProductDto? model = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
                TempData["success"]="successfully";

                return View(model);
            }
            else
            {
                TempData["error"]=response?.Message;
                return View();

            }
        }

        [HttpPost]
        public async Task<IActionResult> ProductDelete(ProductDto productDto)
        {

            if (ModelState.IsValid)
            {
                ResponseDto? response = await _productService.DeleteProductAsync(productDto.ProductId);

                if (response != null&& response.IsSuccess)
                {
                    TempData["success"]="successfully";

                    return RedirectToAction(nameof(ProductIndex));
                }
                else
                {
                    TempData["error"]=response?.Message;
                }
            }


            return RedirectToAction(nameof(ProductIndex));
        }


        public async Task<IActionResult> ProductEdit(int productId)
        {
            ResponseDto? response = await _productService.GetProductByIdAsync(productId);

            if (response != null && response.IsSuccess)
            {
                ProductDto? model = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
                return View(model);
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> ProductEdit(ProductDto productDto)
        {
            if (ModelState.IsValid)
            {
                ResponseDto? response = await _productService.UpdateProductAsync(productDto);

                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Product updated successfully";
                    return RedirectToAction(nameof(ProductIndex));
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }
            return View(productDto);
        }

    }
}
