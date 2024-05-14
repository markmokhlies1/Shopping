using API.Dtos.Shipping;
using API.Models;

namespace API.Services
{
	public interface IShippingRepo
	{
        Task<List<Order>> getAllBindingOrders();
        Task<List<ShippingCompanyDto>> getAllShippingCompanies();
        Task OrderInShipping(int id, string ploicy, int company);
        Task<List<ShippingOrderDetailDto>> getAllShippingOrderDetails(int id);
    }
}

