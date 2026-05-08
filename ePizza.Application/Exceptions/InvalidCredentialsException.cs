using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizza.Application.Exceptions
{
    public class InvalidCredentialsException(string message) : Exception(message)
    {
    }
}
