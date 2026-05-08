using AutoMapper;
using ePizza.Domain.Interfaces;
using ePizza.Domain.Models;
using ePizza.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizza.Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<UserDomain, User>, IUserRepository
    {
        public UserRepository(ePizzaDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {

        }

        public async Task<int> AddUserAsync(UserDomain userDomain)
        {

            var roles
                  = await _dbContext.Roles.FirstAsync(x => x.Name == userDomain.UserRoles.First());

            if (roles is not null)
            {
                //below line will convert domain model to entity model
                var user = _mapper.Map<User>(userDomain);
                user.CreatedDate = DateTime.UtcNow;

                user.Roles.Add(roles);

                // this will add user to user table
                _dbContext.Users.Add(user);

                return await CommitAsync();

            }
            return await Task.FromResult(0); //to do
        }

        public async Task<UserDomain> GetUserByEmailAsync(string emailAddress)
        {
            var userDetails
                 = await _dbContext.Users
                 .FirstOrDefaultAsync(x => x.Email == emailAddress);


            return _mapper.Map<UserDomain>(userDetails);

        }

    }
}
