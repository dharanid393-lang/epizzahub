using AutoMapper;
using ePizza.Domain.Models;
using ePizza.Infrastructure.Entities;

namespace ePizza.Infrastructure.Mappers
{
    public class UserMappingExtension : Profile
    {
        public UserMappingExtension()
        {
            CreateMap<User, UserDomain>();

            CreateMap<UserDomain, User>();

        }
    }
}
