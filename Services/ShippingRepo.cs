using System;
using API.Dtos.Shipping;
using API.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
	public class ShippingRepo : IShippingRepo
	{
        private readonly DataContext _dbContext;
        private readonly IMapper _mapper;

        public ShippingRepo(DataContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public Task<List<Order>> getAllBindingOrders()
        {
            var BindingOrders = _dbContext.Orders.Where(order => order.Status == "Binding").ToListAsync();
            return BindingOrders;
        }

        public Task<List<ShippingCompanyDto>> getAllShippingCompanies()
        {
            var companies = _dbContext.shippingCompanies.Where(a => a.Id != 1).Select(a => new ShippingCompanyDto
            {
                Id = a.Id,
                companyName = a.companyName
            }).ToListAsync();
            return companies;
        }

        public async Task<List<ShippingOrderDetailDto>> getAllShippingOrderDetails(int id)
        {
            List<ShippingOrderDetailDto> ShippingOderDetails =
                                       await _dbContext.OrderDetails
                                       .Include(a => a.Option).ThenInclude(m => m.Product).ThenInclude(k => k.Supplier)
                                       .Where(a => a.OrderId == id)
                                       .Select(a => new ShippingOrderDetailDto()
                                       {
                                           SuplierName = a.Option.Product.Supplier.FirstName,
                                           ProductName = a.Option.Product.ProductName,
                                           AvailableOptions = a.Option.AvailableOptions,
                                           Quantity = a.Quantity
                                       }).ToListAsync();

            return ShippingOderDetails;
        }

        public async Task OrderInShipping(int id, string policy, int company)
        {

            var order = await _dbContext.Orders.FirstOrDefaultAsync(a => a.Id == id);
            if (order != null)
            {
                order.Status = "InShipping";
                order.shippingPolicy = policy;
                order.shippingCompaniesId = company;
                order.ShippedDate = System.DateTime.Now;
                _dbContext.SaveChanges();
            }
        }
    }
}

