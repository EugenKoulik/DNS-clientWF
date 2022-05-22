using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNS_clientWF
{
    internal interface IDnsSuffixSearchList
    {
        bool TryGetSuffix(int attemptNumber, out string suffix);
    }
}
