using API.Models;
using System.Security.Claims;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using API.Dtos.Shipping;

namespace API.Controllers
{
    [Route("[Controller]/[Action]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ISouqlyRepo _repo;
        private readonly IMapper _mapper;

        public CartController(ISouqlyRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;

        }

        [HttpGet("{id}/{Qauntity}")]
        public async Task<IActionResult> AddToCart(int id, int Qauntity)
        {
            var marketingId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            int CartId;
            if (await _repo.IsMarketHaveCart(Int32.Parse(marketingId)))
            {
                CartId = await _repo.GetCartID(Int32.Parse(marketingId));
            }
            else
            {
                Cart cart = new Cart()
                {
                    MarketingId = Int32.Parse(marketingId)
                };
                await _repo.Add(cart);
                await _repo.SaveAll();
                CartId = await _repo.GetCartID(Int32.Parse(marketingId));
            }

            var ItemPrice = await _repo.GetOptionPrice(id);
            float NewPrice = ItemPrice * Qauntity;
            ProductOptionCart ProOptionCart;
            if (await _repo.IsOptionExist(id))
            {
                var DBOption = await _repo.GetOption(id, CartId);
                Qauntity = DBOption.Quantity + Qauntity;
                NewPrice = DBOption.NewPrice + NewPrice;
                DBOption.Quantity = Qauntity;
                DBOption.NewPrice = NewPrice;
                int Stock = await _repo.GetStock(DBOption.OptionId);
                if (DBOption.Quantity <= Stock)
                {
                    await _repo.SaveAll();
                    return Ok();
                }
                else
                {
                    return StatusCode(500, "عفوا الكمية المطلوبة غير متاحة");
                }
            }
            else
            {
                ProductOptionCart newitem = new ProductOptionCart();
                newitem.NewPrice = NewPrice;
                newitem.OptionId = id;
                newitem.Quantity = Qauntity;
                newitem.CartId = CartId;

                int Stock = await _repo.GetStock(id);
                if (Qauntity <= Stock)
                {
                    await _repo.Add(newitem);
                    await _repo.SaveAll();
                    return Ok();
                }
                else
                {
                    return StatusCode(500, "عفوا الكمية المطلوبة غير متاحة");
                }
            }
        }

        [HttpGet("{id}/{model}")]
        public async Task<IActionResult> UpdateQuantityCart(int id, int model)
        {
            var marketingId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var CartId = await _repo.GetCartID(Int32.Parse(marketingId));
            var OptionfromDB = await _repo.GetOption(id, CartId);
            float calc = OptionfromDB.Option.ItemPrice * model;
            OptionfromDB.NewPrice = calc;
            OptionfromDB.Quantity = model;
            int Stock = await _repo.GetStock(id);
            if (model == 0)
            {
                return StatusCode(500, "ادخل كميه واحده او اكثر");
            }
            else if (model <= Stock)
            {
                await _repo.SaveAll();
                return Ok();
            }
            else
            {
                return StatusCode(500, "عفوا الكمية المطلوبة غير متاحة");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetProductsFromCart()
        {
            var marketId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var CartId = await _repo.GetCartID(Int32.Parse(marketId));
            var CartItem = await _repo.GetCart(CartId);
            return Ok(CartItem);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProOptionFromCart(int id)
        {
            var marketingId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var CartId = await _repo.GetCartID(Int32.Parse(marketingId));
            var Option = _repo.GetOption(id, CartId);
            await _repo.Delete(Option);
            await _repo.SaveAll();
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAllProOptionFromCart(ICollection<String> ids)
        {
            if (ids.Count < 1)
            {
                return StatusCode(500, "يجب عليك تحديد المنتجات للحذف");
            }
            var result = await _repo.DeleteAllSelected(ids);
            if (result)
            {
                return Ok();
            }
            else
            {
                return StatusCode(500, "يوجد مشكله في الحذف");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllShipping()
        { 
            var Allshipping = await _repo.GetAllshipping();
            var data = _mapper.Map<IEnumerable<AllShippingDto>>(Allshipping);
            return Ok(data);
        }
    }
}

