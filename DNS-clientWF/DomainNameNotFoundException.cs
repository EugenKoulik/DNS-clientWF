using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNS_clientWF
{
    internal class DomainNameNotFoundException : Exception
    {
        public DomainNameNotFoundException(string message) : base(message)
        {

        }
    }
}
