using AutoMapper;
using ePizza.Application.DTOs.Response;
using ePizza.Domain.Models;

namespace ePizza.Application.Mappers
{
    public class ItemServiceMappingExtension : Profile
    {
        public ItemServiceMappingExtension()
        {
            CreateMap<ItemResponseDto, ItemDomain>().ReverseMap();
        }
    }
}
