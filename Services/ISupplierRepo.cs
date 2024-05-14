using API.Dtos.Supplier;
using API.Dtos.Supplier.SupplierUploadProducts;
using API.Models;

namespace API.Services
{
	public interface ISupplierRepo
	{
        Task<int> AddProduct(ProductForUploadDto productForUploadDto);
        Task<int> AddProductMainData(Product ProductData);
        Task<ImageForReturnDto> AddImageForProduct(int productId, ImageForCreateDto imageForCreateDto);
        Task<List<SupplierOrderDto>> GetOrders(long supplierId);
        int GetCountOfOrders(long supplierId);
    }
}

