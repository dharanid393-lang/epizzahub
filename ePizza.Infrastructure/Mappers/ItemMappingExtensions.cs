using AutoMapper;
using ePizza.Domain.Models;
using ePizza.Infrastructure.Entities;

namespace ePizza.Infrastructure.Mappers
{
    internal class ItemMappingExtensions : Profile
    {
        public ItemMappingExtensions()
        {
            CreateMap<ItemDomain, Item>().ReverseMap();
        }
    }
}
