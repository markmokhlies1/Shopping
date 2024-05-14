using System.Security.Claims;
using API.Dtos.Supplier.SupplierUploadProducts;
using API.Models;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api")]
    [ApiController]
    public class UploadProductController : Controller
    {
        private readonly ISupplierRepo _service;
        private readonly IMapper _mapper;
        private readonly ISouqlyRepo _isouqlyRepo;

        public UploadProductController(ISupplierRepo service, IMapper mapper, ISouqlyRepo IsouqlyRepo)
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
        public async Task<IActionResult> AddProductMainData(int supplierId, ProductForUploadDto productForUploadDto)
        {
            if (supplierId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            Product product = _mapper.Map<Product>(productForUploadDto);
            await _isouqlyRepo.Add(product);
            await _isouqlyRepo.SaveAll();
            return Ok(product.Id);
        }

        [HttpPost("addproductopion")]
        public async Task<IActionResult> AddProductOptionData(ProductOptionDataDto productoption)
        {
            Option option = _mapper.Map<Option>(productoption);
            await _isouqlyRepo.Add(option);
            await _isouqlyRepo.SaveAll();
            return Ok();
        }

        [HttpPost("addproduct")]
        public async Task<IActionResult> AddNewProduct(ProductDataDto productdata)
        {
            Product product = _mapper.Map<Product>(productdata);
            await _isouqlyRepo.Add(product);
            await _isouqlyRepo.SaveAll();

            for (int i = 0; i < productdata.Codes.Length; i++)
            {
                Option op = new Option();
                op.ProductId = product.Id;
                op.Code = productdata.Codes[i];
                op.StockIn = productdata.StockIns[i];
                op.ItemPrice = productdata.ItemPrices[i];
                op.AvailableOptions = productdata.AvailableOptions[i];
                await _isouqlyRepo.Add(op);
                await _isouqlyRepo.SaveAll();

            }
            return Ok(product.Id);
        }
    }
}

