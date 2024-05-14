using API.Dtos.Supplier;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierRepo _repo;
        private readonly IMapper _mapper;
        private readonly IAdminRepo _admin_repo;

        public SupplierController(ISupplierRepo repo, IMapper mapper, IAdminRepo admin_repo)
        {
            _repo = repo;
            _mapper = mapper;
            _admin_repo = admin_repo;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrders(int id)
        {
            List<SupplierOrderDto> orderDetails = await _repo.GetOrders(id);
            return Ok(orderDetails);
        }

        [HttpGet("count/{id}")]
        public IActionResult GetOrdersCount(int id)
        { 
            return Ok(_repo.GetCountOfOrders(id));
        }
    }
}

