using Microsoft.EntityFrameworkCore;
using API.Models;
using API.Dtos.User;

namespace API.Services
{
    public class SouqlyRepo : ISouqlyRepo
    {
        private readonly DataContext _context;
        public SouqlyRepo(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Shipping>> GetAllshipping()
        {
            var data = await _context.Shippings.ToListAsync();
            return data;
        }

        public async Task<User> GetUser(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            var data = await _context.Users.ToListAsync();
            return (data);
        }

        public async Task Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public async Task Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public void Update<T>(T entity) where T : class
        {
            _context.Update(entity);
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> IsMarketHaveCart(int id)
        {
            if (await _context.Carts.AnyAsync(i => i.MarketingId == id)) return true;
            return false;
        }

        public async Task<int> GetCartID(int id)
        {
            var cartID = await _context.Carts.FirstOrDefaultAsync(i => i.MarketingId == id);
            return cartID.Id;
        }

        public async Task<bool> IsOptionExist(int id)
        {
            if (await _context.ProductOptionCart.AnyAsync(i => i.OptionId == id)) return true;
            return false;
        }

        public async Task<ProductOptionCart> GetOption(int optionId, int cartId)
        {
            var ProductOptionInCart = await _context.ProductOptionCart.Include(i => i.Option).FirstOrDefaultAsync(i => i.OptionId == optionId && i.CartId == cartId);
            return ProductOptionInCart;
        }

        public async Task<int> GetStock(int optionId)
        {
            var Option = await _context.Option.FirstOrDefaultAsync(i => i.Id == optionId);

            return Option.StockIn;
        }

        public async Task<List<ProductOptionCart>> GetCart(int id)
        {
            var CartItems = await _context.ProductOptionCart.Include(i => i.Option).ThenInclude(i => i.Product).ThenInclude(id => id.Images).Where(i => i.CartId == id).ToListAsync();
            return CartItems;
        }

        public async Task<float> GetProductPrice(int cartID)
        {
            var TotalPrice = await _context.ProductOptionCart.Where(p => p.CartId == cartID)
                .Select(p => p.NewPrice)
                .ToListAsync();
            float Total = 0;
            foreach (var item in TotalPrice)
            {
                Total += item; 
            }
            return Total;
        }

        public async Task<float> GetShippingPrice(int shippingId)
        {
            var Shipping = await _context.Shippings.FirstOrDefaultAsync(i => i.Id == shippingId);
            return Shipping.price;
        }

        public async Task<List<int>> GetOptionsIds(int CartId)
        {
            var OptionIds = await _context.ProductOptionCart.Where(i => i.CartId == CartId)
            .Select(i => i.OptionId)
            .ToListAsync();
            return OptionIds;
        }

        public async Task<ProductOptionCart> GetProductOption(int optionId, int cartId)
        {
            var ProductOptionInCart = await _context.ProductOptionCart.Include(i => i.Option).ThenInclude(i => i.Product).FirstOrDefaultAsync(i => i.OptionId == optionId && i.CartId == cartId);
            return ProductOptionInCart;
        }

        public async Task<List<Category>> GetAllCategories()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<float> GetOptionPrice(int optionId)
        {
            var Option = await _context.Option.FirstOrDefaultAsync(i => i.Id == optionId);
            var price = Option.ItemPrice;
            return price;
        }

        public async Task<Shipping> GetShipping(int id)
        {
            var ship = await _context.Shippings.FirstOrDefaultAsync(i => i.Id == id);
            return ship;
        }

        public async Task<Order> GetOrderInfoById(int id, int marketingId)
        {
            var order = await _context.Orders.Include(i => i.Bill).Include(i => i.Shipping)
                .Include(i => i.OrderDetail).ThenInclude(i => i.Option).ThenInclude(i => i.Product)
                .ThenInclude(i => i.Images)
                .FirstOrDefaultAsync(i => i.Id == id && i.MarketingId == marketingId);
            return order;
        }

        public async Task<IEnumerable<Order>> GetAllOrders(int id)
        {
            var order = await _context.Orders.Include(i => i.Bill).Where(i => i.MarketingId == id).OrderBy(i => i.OrderDate).ToListAsync();
            return order;
        }

        public async Task<bool> DeleteAllSelected(ICollection<string> ids)
        {
            if (ids.Count < 1)
            {
                return false;
            }
            var i = 0;
            foreach (var id in ids)
            {
                try
                {
                    var ProOptionCartId = int.Parse(id);
                    var ProOptionCart = await _context.ProductOptionCart.FirstOrDefaultAsync(i => i.Id == ProOptionCartId);
                    if (ProOptionCart != null)
                    {
                        _context.ProductOptionCart.Remove(ProOptionCart);
                        i++;
                    }

                }
                catch (System.Exception)
                {
                    throw;
                }
            }
            if (i > 0)
            {
                await _context.SaveChangesAsync();
            }
            return true;
        }

        public async Task<UserProfitsDto> GetUserProfits(int user_id)
        {
            var result = await _context.Users.Include(u => u.UserBills).Include(u => u.WithdrawRequests).FirstOrDefaultAsync(u => u.Id == user_id);
            return new UserProfitsDto()
            {
                TotalProfits = (int)result.TotalProfits,
                AvailableProfits = (int)(result.TotalProfits - result.WithdrawnProfits),
                ExpectedProfits = (int)result.UserBills.Where(o => o.UserId == user_id && o.Active == false).Sum(o => o.UserProfit),
                HasPaymentMethod = (result.WalletNumber != 0),
                IsRequestAvailable = (result.WithdrawRequests.FirstOrDefault(o => o.Confirmed == false) == null),
                RecentTransferred = result.WithdrawRequests.Where(o => (DateTime.Now - o.DateOfRequest).Hours <= 48 && o.Confirmed == true).Sum(o => o.AmountOfMoney)
            };
        }

        public async Task<int> AddWithdrawnRequest(int user_id, int money)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == user_id);
            if (money <= (user.TotalProfits - user.WithdrawnProfits))
            {
                await _context.WithdrawRequests.AddAsync(
                    new WithdrawRequest() { UserId = user_id, AmountOfMoney = money }
                );
                await SaveAll();
                return 1;
            }
            return 0;
        }

        public async Task<IEnumerable<OrderDetails>> GetMarketeerOrders(int id) 
        {
            var orders = await _context.OrderDetails.Include(i => i.Option).ThenInclude(i => i.Product).Include(i => i.Order).ThenInclude(i => i.Bill).Where(i => i.Order.MarketingId == id).OrderBy(i => i.Order.OrderDate).ToListAsync();
            return orders;
        }

        public async Task<IEnumerable<Category>> GetallCategories()
        {
            var allcategories = await _context.Categories.ToListAsync();
            return allcategories;
        }

        public async Task<Category> UpdateCategory(int id, string name)
        {
            var cat = await _context.Categories.FirstOrDefaultAsync(i => i.Id == id);
            cat.CategoryName = name;
            await _context.SaveChangesAsync();
            return cat;
        }

        public Category GetCatById(int id)
        {
            var categorywithId = _context.Categories.FirstOrDefault(i => i.Id == id);
            return categorywithId;
        }

        public async Task<Count> GetCounts()
        {
            Count count = new Count();
            count.NumOfSupplier = _context.UserRoles.Where(t => t.RoleId == 3).Count();//supplier
            count.NumOfMarkiting = _context.UserRoles.Where(t => t.RoleId == 2).Count();//Marketting
            count.NumOfShipping = _context.UserRoles.Where(t => t.RoleId == 4).Count();//Shipping
            count.NumOfProduct = _context.Products.Count();
            count.NumOfCategory = _context.Categories.Count();
            return count;
        }

        public async Task<IEnumerable<Order>> GetOrdersForShipping()
        {
            var orders = await _context.Orders.Where(i => i.Status == "InShipping").OrderBy(i => i.OrderDate).ToListAsync();
            return orders;
        }

        public async Task<Order> getOrder(int OrderId)
        {
            var Order = _context.Orders.FirstOrDefault(i => i.Id == OrderId);
            return Order;
        }

        public async Task<IEnumerable<UserBill>> getBillActive(int OrderId)
        {
            Order order = _context.Orders.Include(i => i.Bill).FirstOrDefault(i => i.Id == OrderId);
            int billId = order.BillId;
            var UserBill = await _context.UserBills.Where(i => i.BillId == billId).ToListAsync();
            return UserBill;
        }

        public async Task<User> getUserprofits(int UserId)
        {
            var User = _context.Users.FirstOrDefault(i => i.Id == UserId);
            return User;
        }

        public async Task<IEnumerable<OrderDetails>> GetOrderDetailsOption(int orderId)
        {
            var OrderDetailsOption = await _context.OrderDetails.Include(i => i.Option).Where(i => i.OrderId == orderId).ToListAsync();
            return OrderDetailsOption;
        }

        public async Task<Option> GetOptionToUPdateQauntaty(int OptionID)
        {
            var option = await _context.Option.FirstOrDefaultAsync(i => i.Id == OptionID);
            return option;
        }
    }
}