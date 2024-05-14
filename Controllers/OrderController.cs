using System.Security.Claims;
using System.Threading.Tasks;
using API.Dtos.Orders;
using API.Models;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("[Controller]/[Action]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ISouqlyRepo _repo;
        private readonly IMapper _mapper;
        private readonly IProductRepo _productRepo;

        public OrderController(ISouqlyRepo repo, IProductRepo productRepo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
            _productRepo = productRepo;
        }

        [HttpPost]
        public async Task<IActionResult> MakeOrder(AddOrderDto model)
        {
            var order = _mapper.Map<Order>(model);
            var bill = _mapper.Map<Bill>(model);
            var marketingId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            await _repo.Add(bill);
            await _repo.SaveAll();
            order.BillId = bill.Id;
            DateTime endDate = DateTime.Now;
            var ShippingInfo = await _repo.GetShipping(model.ShippingId);
            order.ShippedDate = endDate.AddDays(ShippingInfo.Duration);
            order.Status = "Binding";
            order.MarketingId = int.Parse(marketingId);
            await _repo.Add(order);
            await _repo.SaveAll();
            int cartID = await _repo.GetCartID(int.Parse(marketingId));
            List<int> OptionsIds = await _repo.GetOptionsIds(cartID);
            try
            {
                foreach (var OptionIdInCart in OptionsIds)
                {
                    var OptionInfo = await _repo.GetProductOption(OptionIdInCart, cartID);
                    OrderDetails orderDetail = new OrderDetails()
                    {
                        Quantity = OptionInfo.Quantity,
                        OrderId = order.Id,
                        OptionId = OptionInfo.OptionId,
                        TotalOptionPrice = OptionInfo.NewPrice
                    };
                    await _productRepo.UpdateSoldQuantity(OptionInfo.Option.Product.Id, OptionInfo.Quantity);
                    await _repo.Add(orderDetail);
                    var option = await _repo.GetOptionToUPdateQauntaty(OptionInfo.OptionId);
                    option.StockIn = option.StockIn - OptionInfo.Quantity;
                    await _repo.SaveAll();
                    await _repo.Delete(OptionInfo);
                    UserBill Suppliers = new UserBill()
                    {
                        UserId = orderDetail.Option.Product.SupplierId,
                        BillId = bill.Id,
                        UserProfit = orderDetail.TotalOptionPrice,
                        Active = false
                    };
                    await _repo.Add(Suppliers);
                    await _repo.SaveAll();
                }
            }
            catch (Exception e)
            {
                return BadRequest("BAAAAAAAAAAAD");
            }
            UserBill Marketing = new UserBill()
            {
                UserId = int.Parse(marketingId),
                BillId = bill.Id,
                UserProfit = model.MarketingProfits,
                Active = false
            };
            await _repo.Add(Marketing);
            await _repo.SaveAll();
            return Ok(order.Id);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> OrderDetails(int id)
        {
            var marketingId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var products = await _repo.GetOrderInfoById(id, int.Parse(marketingId));
            var productsReturn = _mapper.Map<OrderDetailsDto>(products);
            return Ok(productsReturn);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var marketingId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var products = await _repo.GetAllOrders(int.Parse(marketingId));
            var productsReturn = _mapper.Map<IEnumerable<OrderListDto>>(products);
            return Ok(productsReturn);
        }
    }

}

