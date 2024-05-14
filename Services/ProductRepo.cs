using API.Dtos.Products;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class ProductRepo : IProductRepo
    {
        private readonly DataContext _context;

        public ProductRepo(DataContext context)
        {
            _context = context;
        }

        public async Task DeleteProduct(int id)
        {
            Product product = await _context.Products.FirstOrDefaultAsync(a => a.Id == id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ProductDto>> GetProducts()
        {

            List<ProductDto> result = await _context.Products.Include(p => p.Options).Include(p => p.Images).Select(p => new ProductDto()
            {
                id = p.Id,
                productName = p.ProductName,
                price = p.Options.Min(o => o.ItemPrice),
                stockIn = p.Options.Sum(o => o.StockIn),
                images = p.Images.Select(i => i.Url).ToList(),
                options = p.Options.Select(o => new OptionDto()
                {
                    Id = o.Id,
                    Name = o.AvailableOptions,
                    ItemPrice = o.ItemPrice,
                    StockIn = o.StockIn
                }).ToList()
            }).ToListAsync();
            return result;
        }

        public async Task<List<ProductDto>> GetSupplierProducts(int id)
        {
            List<ProductDto> result = await _context.Products.Include(p => p.Options).Include(p => p.Images).Where(a => a.SupplierId == id).Select(p => new ProductDto()
            {
                id = p.Id,
                productName = p.ProductName,
                price = p.Options.Min(o => o.ItemPrice),
                stockIn = p.Options.Sum(o => o.StockIn),
                images = p.Images.Select(i => i.Url).ToList(),
                options = p.Options.Select(o => new OptionDto()
                {
                    Id = o.Id,
                    Name = o.AvailableOptions,
                    ItemPrice = o.ItemPrice,
                    StockIn = o.StockIn
                }).ToList()
            }).ToListAsync();
            return result;
        }

        public async Task EditProductOption(OptionDto optionEdited)
        {
            var option = await _context.Option.FirstOrDefaultAsync(op => op.Id == optionEdited.Id);
            option.ItemPrice = optionEdited.ItemPrice;
            option.StockIn = optionEdited.StockIn;
            option.AvailableOptions = optionEdited.Name;
            await _context.SaveChangesAsync();
        }

        public async Task<List<ProductDto>> GetTopProducts(int top)
        {
            List<ProductDto> result = await _context.Products.OrderByDescending(p => p.QuantitySold).Include(p => p.Options).Include(p => p.Images).Take(top).Select(p => new ProductDto()
            {
                id = p.Id,
                productName = p.ProductName,
                price = p.Options.Min(o => o.ItemPrice),
                stockIn = p.Options.Sum(o => o.StockIn),
                images = p.Images.Select(i => i.Url).ToList(),
                options = p.Options.Select(o => new OptionDto()
                {
                    Id = o.Id,
                    Name = o.AvailableOptions,
                    ItemPrice = o.ItemPrice,
                    StockIn = o.StockIn
                }).ToList()
            }).ToListAsync();
            return result;
        }

        public async Task<List<ProductDto>> GetTopProductsByCatogorey(int CatgoreyId, int topnum)
        {
            List<ProductDto> result = await _context.Products.Where(p => p.CategoryId == CatgoreyId).OrderByDescending(p => p.QuantitySold).Include(p => p.Options).Include(p => p.Images).Take(topnum).Select(p => new ProductDto()
            {
                id = p.Id,
                productName = p.ProductName,
                price = p.Options.Min(o => o.ItemPrice),
                stockIn = p.Options.Sum(o => o.StockIn),
                images = p.Images.Select(i => i.Url).ToList(),
                options = p.Options.Select(o => new OptionDto()
                {
                    Id = o.Id,
                    Name = o.AvailableOptions,
                    ItemPrice = o.ItemPrice,
                    StockIn = o.StockIn
                }).ToList()
            }).ToListAsync();
            return result;
        }

        public async Task UpdateSoldQuantity(int id, int quantity)
        {
            Product product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product.QuantitySold != null)
            {
                product.QuantitySold += quantity;
            }
            else
            {
                product.QuantitySold = quantity;
            }
            await _context.SaveChangesAsync();
        }

        public async Task<List<ProductDto>> GetCatogoreyProducts(int CatgoreyId)
        {
            List<ProductDto> result = await _context.Products.Include(p => p.Options).Include(p => p.Images).Where(p => p.CategoryId == CatgoreyId).Select(p => new ProductDto()
            {
                id = p.Id,
                productName = p.ProductName,
                price = p.Options.Min(o => o.ItemPrice),
                stockIn = p.Options.Sum(o => o.StockIn),
                images = p.Images.Select(i => i.Url).ToList(),
                options = p.Options.Select(o => new OptionDto()
                {
                    Id = o.Id,
                    Name = o.AvailableOptions,
                    ItemPrice = o.ItemPrice,
                    StockIn = o.StockIn
                }).ToList()
            }).ToListAsync();
            return result;
        }

        public async Task<List<ProductDto>> GetSupplierProductsEx(int id)
        {
            List<ProductDto> result = await _context.Products.Include(p => p.Options).Include(p => p.Images).Where(a => a.SupplierId == id).Select(p => new ProductDto()
            {
                id = p.Id,
                productName = p.ProductName,
                price = p.Options.Min(o => o.ItemPrice),
                stockIn = p.Options.Sum(o => o.StockIn),
                images = p.Images.Select(i => i.Url).ToList(),
                options = p.Options.Where(am => am.StockIn <= 15).Select(o => new OptionDto()
                {
                    Id = o.Id,
                    Name = o.AvailableOptions,
                    ItemPrice = o.ItemPrice,
                    StockIn = o.StockIn
                }).ToList()
            }).ToListAsync();
            return result;
        }
    }
}

