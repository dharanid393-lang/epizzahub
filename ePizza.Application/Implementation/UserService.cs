using AutoMapper;
using ePizza.Application.Contracts;
using ePizza.Application.DTOs.Request;
using ePizza.Application.Enums;
using ePizza.Domain.Interfaces;
using ePizza.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizza.Application.Implementation
{
    public class UserService : IUserService
    {
        //mapper is user for mapping dto to domain model
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository,IMapper mapper)
        {
            _userRepository = userRepository;
            this._mapper = mapper;
        }

        
        public async Task<UserDomain> GetUserDetailsAsync(string emailAddress)
        {
            return await _userRepository.GetUserByEmailAsync(emailAddress);
        }

        public async Task<bool> RegisterUserAsync(RegisterUserDto request)
        {
            var userDomain = _mapper.Map<UserDomain>(request);

            userDomain.Password = BCrypt.Net.BCrypt.HashPassword(userDomain.Password);

            // ensuring that the new user is assigned to user role and not admin
            userDomain.UserRoles
                = new List<string>()
                {
                    RoleEnum.User.ToString()
                };
            
            var rowsInserted =  await _userRepository.AddUserAsync(userDomain);

            return rowsInserted > 0;
        }
    }
}
