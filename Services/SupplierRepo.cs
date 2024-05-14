using API.Dtos.Supplier;
using API.Dtos.Supplier.SupplierUploadProducts;
using API.Models;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using API.Helpers;

namespace API.Services
{
	public class SupplierRepo:ISupplierRepo
	{
        private readonly DataContext _dbcontext;
        private readonly IOptions<CloudSettings> _cloudinaryConfig;
        private readonly IMapper _mapper;
        private readonly Cloudinary _cloudinary;

        public SupplierRepo(DataContext dbcontext, IOptions<CloudSettings> cloudinaryConfig, IMapper Mapper)
        {
            _dbcontext = dbcontext;
            _cloudinaryConfig = cloudinaryConfig;
            Account Acount = new Account(
               _cloudinaryConfig.Value.CloudName,
               _cloudinaryConfig.Value.APIKey,
               _cloudinaryConfig.Value.APISecret
            );
            _cloudinary = new Cloudinary(Acount);
            _mapper = Mapper;
        }

        public async Task<ImageForReturnDto> GetImage(int id)
        {
            var imageFromDataBase = await _dbcontext.Images4.FirstOrDefaultAsync(m => m.Id == id);
            var image = _mapper.Map<ImageForReturnDto>(imageFromDataBase);
            return image;
        }

        public async Task<ImageForReturnDto> AddImageForProduct(int productId, ImageForCreateDto imageForCreateDto)
        {
            var file = imageForCreateDto.File;
            var uploadResult = new ImageUploadResult();
            if (file != null && file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation()
                                              .Width(500)
                                              .Height(500)
                                              .Crop("fill")
                    };
                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }
            imageForCreateDto.URL = uploadResult.Uri.ToString();
            imageForCreateDto.publicId = uploadResult.PublicId;
            var image = _mapper.Map<Image>(imageForCreateDto);
            _dbcontext.Images4.Add(new Image { Url = image.Url, publicId = image.publicId, ProductId = productId });
            _dbcontext.SaveChanges();
            var imageForReturn = _mapper.Map<ImageForReturnDto>(image);
            return imageForReturn;
        }

        public async Task<int> AddProductMainData(Product ProductData)
        {
            await _dbcontext.Products.AddAsync(ProductData);
            await _dbcontext.SaveChangesAsync();
            var productId = ProductData.Id;
            return productId;
        }

        public async Task<int> AddProduct(ProductForUploadDto productForUploadDto)
        {
            Product product = _mapper.Map<Product>(productForUploadDto);
            int productId = int.Parse(AddProductMainData(product).ToString());
            var productOption = _mapper.Map<Option>(productForUploadDto);
            productOption.ProductId = productId;
            await _dbcontext.Option.AddAsync(productOption);
            await _dbcontext.SaveChangesAsync();
            return productId;
        }

        public async Task<List<SupplierOrderDto>> GetOrders(long supplierId)
        {
            List<OrderDetails> unseenOrders = (from o in _dbcontext.OrderDetails.Include(o => o.Option).ThenInclude(o => o.Product)
                                               where o.Seen_Supplier == false && o.Option.Product.SupplierId == supplierId
                                               select o).ToList();
            foreach (var item in unseenOrders)
            {
                item.Seen_Supplier = true;
            }
            _dbcontext.SaveChanges();
            var orders = await _dbcontext.OrderDetails.Include(i => i.Order).
                                           Include(i => i.Option).ThenInclude(o => o.Product).Select(o => new SupplierOrderDto()
                                           {
                                               OrderId = o.OrderId,
                                               OrderDate = o.Order.OrderDate,
                                               ProductId = o.Option.ProductId,
                                               ProductName = o.Option.Product.ProductName,
                                               Quantity = o.Quantity,
                                               Status = o.Order.Status,
                                               TotalOptionPrice = o.TotalOptionPrice
                                           }).ToListAsync();
            return orders;
        }

        public int GetCountOfOrders(long supplierId)
        {
            var result = _dbcontext.OrderDetails.Include(o => o.Option).ThenInclude(o => o.Product).Where(
                    x => x.Option.Product.SupplierId == supplierId && x.Seen_Supplier == false
                ).Count();
            return result;
        }
    }
}

