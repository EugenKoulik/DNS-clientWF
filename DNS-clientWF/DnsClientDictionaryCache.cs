using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNS_clientWF
{
    internal class DnsClientDictionaryCache : IDnsClientCache
    {
        Dictionary<string, string> domainNameIpPairs = new Dictionary<string, string>();
        public void AddDomainNameIpPair(string domainName, string ip)
        {
            domainNameIpPairs[domainName] = ip;
        }

        public bool TryGetIp(string domainName, out string ip)
        {
            return domainNameIpPairs.TryGetValue(domainName, out ip);
        }

        public Dictionary<string, string> GetDomainNameIpPair()
        {
            return domainNameIpPairs;
        }
    }
}
