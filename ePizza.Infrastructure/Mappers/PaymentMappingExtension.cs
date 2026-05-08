using AutoMapper;
using ePizza.Domain.Models;
using ePizza.Infrastructure.Entities;


namespace ePizza.Infrastructure.Mappers
{
    public class PaymentMappingExtension : Profile
    {
        public PaymentMappingExtension()
        {
            // why for member created this for date time and as id columns are different in entities and domain model
            CreateMap<PaymentDomain, PaymentDetail>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.PaymentId))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow));


            CreateMap<OrderDomain, Order>()
                  .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.OrderId))
                    .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<OrderItemDomain, OrderItem>();
        }


    }
}
