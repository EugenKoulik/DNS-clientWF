using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNS_clientWF
{
    internal interface IDnsClientCache
    {
        void AddDomainNameIpPair(string domainName, string ip);
        bool TryGetIp(string domainName, out string ip);
        Dictionary<string, string> GetDomainNameIpPair();
    }
}
