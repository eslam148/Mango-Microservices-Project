using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utitlity;

namespace Mango.Web.Service
{
    public class ProductService : IProductService
    {
         private readonly IBaseService _baseService;
        public ProductService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto?> CreateProductAsync(ProductDto productDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = productDto,
                Url = SD.ProductApiBase+"/api/product",
                ContentType = SD.ContentType.MultipartFormData
            });
        }

        public async Task<ResponseDto?> DeleteProductAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.DELETE,
                Url = SD.ProductApiBase+"/api/product/"+id
            });
        }

        public async Task<ResponseDto?> GetAllProductAsync()
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.ProductApiBase+"/api/product"
            });
        }

        public async Task<ResponseDto?> GetProductAsync(string Name)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Utitlity.SD.ApiType.GET,
                Url = SD.ProductApiBase+"/api/product/"+Name
            });
        }

        public async Task<ResponseDto?> GetProductByIdAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Utitlity.SD.ApiType.GET,
                Url = SD.ProductApiBase+"/api/product/"+id
            });
        }

        public async Task<ResponseDto?> UpdateProductAsync(ProductDto ProductDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.PUT,
                Data = ProductDto,
                Url = SD.ProductApiBase+"/api/Product/"
            });
        }
    }
}
