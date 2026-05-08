using AutoMapper;
using ePizza.Application.DTOs.Request;
using ePizza.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizza.Application.Mappers
{
    public  class UserServiceMappingExtension : Profile
    {

        public UserServiceMappingExtension()
        {
            CreateMap<RegisterUserDto, UserDomain>();
        }
    }
}
