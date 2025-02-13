using AutoMapper;
using DK.EventBus.Messages.Events;
using DK.Ordering.Application.Commands;
using DK.Ordering.Application.Responses;
using DK.Ordering.Core.Entities;

namespace DK.Ordering.Application.Mappers
{
    public class OrderMappingProfile : Profile
    {
        public OrderMappingProfile()
        {
            CreateMap<Order, OrderResponse>().ReverseMap();
            CreateMap<Order, CheckoutOrderCommand>().ReverseMap();
            CreateMap<Order, UpdateOrderCommand>().ReverseMap();
            CreateMap<CheckoutOrderCommand, BasketCheckoutEvent>().ReverseMap();
            CreateMap<CheckoutOrderCommand, BasketCheckoutEventV2>().ReverseMap();
        }
    }
}
