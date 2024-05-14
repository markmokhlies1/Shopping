using System.Security.Claims;
using API.Dtos.Supplier.SupplierUploadProducts;
using API.Models;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ProductImageController : ControllerBase
    {
        private readonly ISupplierRepo _service;
        private readonly IMapper _mapper;
        private readonly ISouqlyRepo _isouqlyRepo;

        public ProductImageController(ISupplierRepo service, IMapper mapper, ISouqlyRepo IsouqlyRepo)
        {
            _service = service;
            _mapper = mapper;
            _isouqlyRepo = IsouqlyRepo;
        }

        [HttpPost("{productId}")]
        public async Task<IActionResult> AddImageForProduct( int productId, [FromForm] ImageForCreateDto imageForCreateDto)
        {
            return Ok(await _service.AddImageForProduct(productId, imageForCreateDto));
        }

        [HttpPost("addproduct/{supplierId}")]
        public async Task<IActionResult> AddProduct(int supplierId, ProductForUploadDto productForUploadDto)
        {
            if (supplierId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }
            Product product = _mapper.Map<Product>(productForUploadDto);
            await _isouqlyRepo.Add(product);
            await _isouqlyRepo.SaveAll();
            Option option = _mapper.Map<Option>(productForUploadDto);
            option.ProductId = product.Id;
            await _isouqlyRepo.Add(option);
            await _isouqlyRepo.SaveAll();
            return Ok(product.Id);
        }
    }
}

