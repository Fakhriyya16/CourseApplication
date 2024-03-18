using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Helpers.Exceptions
{
    public class NoGroupsAvailableException : Exception
    {
        public NoGroupsAvailableException(string msg) : base(msg) { }
    }
}
