using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Dtos.Marketeer;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class MarketeerOrdersController : ControllerBase
    {
        private IMapper _mapper;
        private ISouqlyRepo _repo;

        public MarketeerOrdersController(ISouqlyRepo repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        { 
            var marketingId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var orders = await _repo.GetMarketeerOrders(marketingId);
            var ordersToReturn = _mapper.Map<IEnumerable<MarketeerOrdersDto>>(orders);
            return Ok(ordersToReturn);
        }
    }
}

