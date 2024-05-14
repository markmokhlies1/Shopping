using API.Dtos.Products;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [AllowAnonymous]
    [Route("[controller]")]
    [ApiController]
    public class ProductController:ControllerBase
	{
        private readonly IProductRepo _repo;
        private readonly IMapper _mapper;

        public ProductController(IProductRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var prods = await _repo.GetProducts();
            return Ok(prods);
        }

        [HttpGet("{CatgoreyId}")]
        public async Task<IActionResult> GetCatgoreyProducts(int CatgoreyId)
        {
            var prods = await _repo.GetCatogoreyProducts(CatgoreyId);
            if (prods != null)
            {
                return Ok(prods);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("GetSupplierProducts/{id}")]
        public async Task<IActionResult> GetSupplierProducts(int id)
        {
            var prods = await _repo.GetSupplierProducts(id);
            return Ok(prods);
        }



        [HttpDelete("deleteProduct/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _repo.DeleteProduct(id);



            return Ok();
        }

        [HttpPut("editOption")]
        public async Task<IActionResult> EditProductOption(OptionDto optionEdited)
        {
            await _repo.EditProductOption(optionEdited);
            return Ok("تم تعديل الاختيارات");
        }

        [HttpGet("GetTopProducts/{top}")]
        public async Task<IActionResult> GetTopProducts(int top = 10)
        {
            var prods = await _repo.GetTopProducts(top);
            return Ok(prods);
        }

        [HttpGet("GetTopProducts/{CatgoreyId}/{top}")]
        public async Task<IActionResult> GetTopProductsByCatorey(int CatgoreyId, int top = 10)
        {
            var prods = await _repo.GetTopProductsByCatogorey(CatgoreyId, top);
            return Ok(prods);
        }

        [HttpGet("GetSupplierProductsEx/{id}")]
        public async Task<IActionResult> GetSupplierProductsEx(int id)
        {
            var prods = await _repo.GetSupplierProductsEx(id);
            return Ok(prods);
        }
    }
}

