using API.Dtos.Shipping;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ShippingController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ISouqlyRepo _souqlyService;
        private readonly IShippingRepo _shippingService;

        public ShippingController(IMapper mapper, ISouqlyRepo souqlyService, IShippingRepo shippingRepo)
        {
            _mapper = mapper;
            _souqlyService = souqlyService;
            _shippingService = shippingRepo;
        }

        [HttpGet("GetBindingOrders")]
        public async Task<IActionResult> GetBindingOrders()
        {
            var BindingOrders = await _shippingService.getAllBindingOrders();
            var BindingOrdersForShipping = _mapper.Map<IEnumerable<OrderShippingDto>>(BindingOrders);
            return Ok(BindingOrdersForShipping);
        }

        [HttpGet("GetAllShippingCompanies")]
        public async Task<IActionResult> GetAllShippingCompanies()
        {
            var companies = await _shippingService.getAllShippingCompanies();
            if (companies != null)
            {
                return Ok(companies);

            }
            return Ok("لا توجد شركات شحن");
        }

        [HttpPost("MakeOrderInShipping/{orderNumber}/{one}/{two}")]
        public async Task<IActionResult> MakeOrderInShipping(int orderNumber, string one, int two)
        {
            await _shippingService.OrderInShipping(orderNumber, one, two);
            return Ok();
        }

        [HttpGet("GetShippingOrderDetails/{id}")]
        public async Task<IActionResult> GetShippingOrderDetails(int id)
        {
            var Details = await _shippingService.getAllShippingOrderDetails(id);
            return Ok(Details);
        }
    }
}

