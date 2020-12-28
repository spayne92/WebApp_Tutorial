using AutoMapper;
using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;

namespace DutchTreat.Data
{
    public class DutchMappingProfile : Profile
    {
        public DutchMappingProfile()
        {
            // Autmamtically tries to map based on property names and types.
            CreateMap<Order, OrderViewModel>()
                // For destination member vm.OrderId, map from source Order.Id
                .ForMember(o => o.OrderId, ex => ex.MapFrom(o => o.Id))
                // Also specifies to include reverse of custom member mappings.
                .ReverseMap();

            CreateMap<OrderItem, OrderItemViewModel>()
                .ReverseMap();
        }
    }
}
