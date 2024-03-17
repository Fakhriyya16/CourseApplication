using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Helpers.Exceptions
{
    public class RegistrationFailedException : Exception
    {
        public RegistrationFailedException(string message) : base(message) { }
    }
}
