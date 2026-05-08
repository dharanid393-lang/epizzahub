using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizza.Domain.Models
{
    public class UserDomain
    {
        //will handle the exception of null list in case of no roles assigned
        public UserDomain()
        {
            UserRoles = new();
        }
        public int Id { get; set; }

        public string Name { get; set; } = default!; 

        public string Email { get; set; } = default!;

        public string Password { get; set; } = default!;

        public string PhoneNumber { get; set; } = default!;

        public List<string> UserRoles { get; set; } 
    }
}
