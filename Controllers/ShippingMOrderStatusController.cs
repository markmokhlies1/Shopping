using API.Dtos.Shipping;
using API.Models;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("[Controller]/[Action]")]
    [ApiController]
    [AllowAnonymous]
    public class ShippingMOrderStatusController : ControllerBase
    {
        private readonly ISouqlyRepo _repo;
        private readonly IMapper _mapper;
        public ShippingMOrderStatusController(ISouqlyRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrdersStatesForShipping()
        {
            var orders = await _repo.GetOrdersForShipping();
            var ordersToReturn = _mapper.Map<IEnumerable<ShippingOrdersStatusDto>>(orders);
            return Ok(ordersToReturn);
        }

        [HttpPost("{Status}/{OrderId}")]
        public async Task<IActionResult> UpdateOrder(string Status, int OrderId)
        {
            var order = await _repo.getOrder(OrderId);
            order.Status = Status;
            await _repo.SaveAll();
            if (Status == "Deliverd")
            {
                var userBill = await _repo.getBillActive(OrderId);
                foreach (UserBill B in userBill)
                {
                    B.Active = true;
                    var user = await _repo.getUserprofits(B.UserId);
                    user.TotalProfits = user.TotalProfits + (int)B.UserProfit;
                }
            }
            else if (Status == "Canceld")
            {
                var OrderDetailsOption = await _repo.GetOrderDetailsOption(OrderId);
                foreach (OrderDetails ODO in OrderDetailsOption)
                {
                    ODO.Option.StockIn = ODO.Option.StockIn + ODO.Quantity;
                }
            }
            await _repo.SaveAll();
            return Ok();
        }
    }
}

