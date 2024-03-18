using AutoMapper;
using Azure;
using Mango.Services.ProductAPI.Data;
using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ProductAPI.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductAPIController : Controller
    {
        private readonly AppDbContext _db;
        private   IMapper _mapper;
        private   ResponseDto response;

        public ProductAPIController(AppDbContext db,IMapper mapper)
        {
            _db = db;
            response = new ResponseDto();
            _mapper = mapper;
        }
        [HttpGet]
        public ResponseDto Get()
        {
            try
            {
 
                IEnumerable<Product> objList =  _db.Products.ToList();
                response.Result=_mapper.Map<IEnumerable<ProductDto>>(objList);

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        [HttpGet]
        [Route("{id:int}")]
        public ResponseDto Get(int id)
        {
            try
            {

                Product obj = _db.Products.FirstOrDefault(p=> p.ProductId == id);
                response.Result= _mapper.Map<ProductDto>(obj);
               

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }
        [HttpGet]
        [Route("{name}")]
        public ResponseDto GetByName(string name)
        {
            try
            {

               IEnumerable<Product> objList = _db.Products.Where(p=> p.Name.ToLower().Contains(name.ToLower()));
                response.Result= _mapper.Map<IEnumerable<ProductDto>>(objList);


            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        [HttpPost]
         public  ResponseDto Post(ProductDto model)
        {
            try
            {
                
                Product obj = _mapper.Map<Product>(model);
                _db.Products.Add(obj);
                _db.SaveChanges();

                // if (model.Image != null)
                //{

                //    string fileName = obj.ProductId + Path.GetExtension(model.Image.FileName);
                //    string filePath = @"wwwroot\ProductImages\" + fileName;

                //    //I have added the if condition to remove the any image with same name if that exist in the folder by any change
                //    var directoryLocation = Path.Combine(Directory.GetCurrentDirectory(), filePath);
                //    FileInfo file = new FileInfo(directoryLocation);
                //    if (file.Exists)
                //    {
                //        file.Delete();
                //    }

                //    var filePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), filePath);
                //    using (var fileStream = new FileStream(filePathDirectory, FileMode.Create))
                //    {
                //        model.Image.CopyTo(fileStream);
                //    }
                //    var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
                //    obj.ImageUrl = baseUrl+ "/ProductImages/"+ fileName;
                //    obj.ImageLocalPath = filePath;
                //}
                //else
                //{
                //    obj.ImageUrl = "https://placehold.co/600x400";
                //}
                //_db.Products.Update(obj);
                //_db.SaveChanges();
                response.Result = _mapper.Map<ProductDto>(obj);

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        [HttpPut]
         public ResponseDto Put(ProductDto ProductDto)
        {
            try
            {
                Product product = _mapper.Map<Product>(ProductDto);

                //if (ProductDto.Image != null)
                //{
                //    if (!string.IsNullOrEmpty(product.ImageLocalPath))
                //    {
                //        var oldFilePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), product.ImageLocalPath);
                //        FileInfo file = new FileInfo(oldFilePathDirectory);
                //        if (file.Exists)
                //        {
                //            file.Delete();
                //        }
                //    }

                //    string fileName = product.ProductId + Path.GetExtension(ProductDto.Image.FileName);
                //    string filePath = @"wwwroot\ProductImages\" + fileName;
                //    var filePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), filePath);
                //    using (var fileStream = new FileStream(filePathDirectory, FileMode.Create))
                //    {
                //        ProductDto.Image.CopyTo(fileStream);
                //    }
                //    var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
                //    product.ImageUrl = baseUrl + "/ProductImages/" + fileName;
                //    product.ImageLocalPath = filePath;
                //}


                _db.Products.Update(product);
                _db.SaveChanges();

                response.Result = _mapper.Map<ProductDto>(product);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        [HttpDelete]
        [Route("{id:int}")]
     //   [Authorize(Roles = "ADMIN")]
        public ResponseDto Delete(int id)
        {
            try
            {
                Product obj = _db.Products.First(u => u.ProductId==id);
                if (!string.IsNullOrEmpty(obj.ImageLocalPath))
                {
                    var oldFilePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), obj.ImageLocalPath);
                    FileInfo file = new FileInfo(oldFilePathDirectory);
                    if (file.Exists)
                    {
                        file.Delete();
                    }
                }
                _db.Products.Remove(obj);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
