using API.Dtos.Admin;
using API.Dtos.Orders;
using API.Dtos.Shipping;
using API.Dtos.Supplier.SupplierUploadProducts;
using API.Dtos.User;
using API.Models;
using AutoMapper;

namespace API.Helpers
{
	public class AutoMapperProfiles : Profile
	{
		public AutoMapperProfiles()
		{
			CreateMap<UserForRegister, User>();
			CreateMap<User, UserForDetails>();
			CreateMap<ManageCategoriesDto, Category>();
			CreateMap<Category, ManageCategoriesDto>();
            CreateMap<User, UserForManage>();
            CreateMap<UserForManage, User>();
            CreateMap<UserVisa, User>();
			CreateMap<User, UserVisa>();
            CreateMap<AddOrderDto, Order>();
            CreateMap<AddOrderDto, Shipping>();
            CreateMap<AddOrderDto, Bill>();
            CreateMap<AddOrderDto, OrderDetails>();
            CreateMap<Image, ImageForReturnDto>();
            CreateMap<ImageForCreateDto, Image>();
            CreateMap<Product, ProductForUploadDto>();
            CreateMap<ProductForUploadDto, Product>();
            CreateMap<Option, ProductForUploadDto>();
            CreateMap<ProductForUploadDto, Option>();
            CreateMap<Order, OrderShippingDto>();
            CreateMap<OrderShippingDto, Order>();
            CreateMap<Order, ShippingOrdersStatusDto>();
            CreateMap<Option, ProductOptionDataDto>();
            CreateMap<ProductOptionDataDto, Option>();
            CreateMap<Product, ProductDataDto>();
            CreateMap<ProductDataDto, Product>();

            CreateMap<Order, OrderDetailsDto>()
             .ForMember(dest => dest.City, opt => { opt.MapFrom(src => src.Shipping.City); })
             .ForMember(dest => dest.price, opt => { opt.MapFrom(src => src.Shipping.price); })
             .ForMember(dest => dest.Duration, opt => { opt.MapFrom(src => src.Shipping.Duration); })
             .ForMember(dest => dest.DealPrice, opt => { opt.MapFrom(src => src.Bill.DealPrice); })
             .ForMember(dest => dest.SiteProfits, opt => { opt.MapFrom(src => src.Bill.SiteProfits); })
             .ForMember(dest => dest.ShippingProfits, opt => { opt.MapFrom(src => src.Bill.ShippingProfits); })
             .ForMember(dest => dest.MarktingProfits, opt => { opt.MapFrom(src => src.Bill.MarktingProfits); });

            CreateMap<Order, OrderListDto>()
             .ForMember(dest => dest.MarktingProfits, opt => { opt.MapFrom(src => src.Bill.MarktingProfits); });
        }
	}
}

