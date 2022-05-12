using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNS_clientWF
{
    internal enum RCodes
    {
        Success,
        FormatError,
        ServerFailure,
        NameError,
        NotImplemented,
        Refused
    }
}
