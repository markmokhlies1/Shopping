using API.Dtos.Products;
using API.Helpers;
using API.Models;

namespace API.Services
{
    public interface IProductRepo
    {
        Task<List<ProductDto>> GetProducts();
        Task DeleteProduct(int id);
        Task<List<ProductDto>> GetSupplierProducts(int id);
        Task<List<ProductDto>> GetCatogoreyProducts(int CatgoreyId);
        Task EditProductOption(OptionDto optionEdited);
        Task<List<ProductDto>> GetTopProducts(int top);
        Task<List<ProductDto>> GetTopProductsByCatogorey(int CatgoreyId, int topnum);
        Task UpdateSoldQuantity(int id, int quantity);
        Task<List<ProductDto>> GetSupplierProductsEx(int id);
    }
}
