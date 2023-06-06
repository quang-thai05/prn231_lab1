using AutoMapper;
using Lab1.Dtos;
using Lab1.Models;

namespace Lab1.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<OrderDetail, ProductDto>()
                .ForMember(dest => dest.Price, otp => otp.MapFrom(src => src.UnitPrice))
                .ReverseMap();
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.Products, otp => otp.MapFrom(src => src.OrderDetails))
                .ReverseMap();
        }
    }
}
