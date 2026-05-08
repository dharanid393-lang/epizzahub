using ePizza.Application.DTOs.Request;
using ePizza.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizza.Application.Contracts
{
    public interface IUserService 
    {
        Task<UserDomain> GetUserDetailsAsync(string emailAddress);

        Task<bool> RegisterUserAsync(RegisterUserDto request);
        
    }
}
