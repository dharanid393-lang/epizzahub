using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizza.Application.DTOs.Request
{
    public  class RegisterUserDto
    {

        public string Name { get; set; } = default!;

        public string Password { get; set; } = default!;

        public string Email { get; set; } = default!;

        public string PhoneNumber { get; set; } = default!;
    }
}
