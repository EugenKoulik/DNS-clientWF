using System.Text.Json;


namespace DNS_clientWF
{
    internal class DnsClientFileCache : IDnsClientCache
    {
        const string pathToFileCache = "dnsClientCache.json";
        public void AddDomainNameIpPair(string domainName, string ip)
        {
            Dictionary<string, string> domainNameIpPairs = GetDomainNameIpPair();

            domainNameIpPairs[domainName] = ip;

            using (var sw = new StreamWriter(pathToFileCache, false))
            {
                string json = JsonSerializer.Serialize(domainNameIpPairs);
                sw.Write(json);
            }
        }

        public Dictionary<string, string> GetDomainNameIpPair()
        {
            Dictionary<string, string> domainNameIpPairs;

            if (!File.Exists(pathToFileCache))
            {
                domainNameIpPairs = new Dictionary<string, string>();
            }
            else
            {
                using (var sr = new StreamReader(pathToFileCache))
                {
                    string json = sr.ReadToEnd();
                    domainNameIpPairs = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
                }
            }

            return domainNameIpPairs;
        }

        public bool TryGetIp(string domainName, out string ip)
        {
            if (!File.Exists(pathToFileCache))
            {
                ip = string.Empty;
                return false;
            }
            Dictionary<string, string> domainNameIpPairs;
            using (var sr = new StreamReader(pathToFileCache))
            {
                string json = sr.ReadToEnd();
                domainNameIpPairs = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
            }

            if (domainNameIpPairs == null || !domainNameIpPairs.ContainsKey(domainName))
            {
                ip = string.Empty;
                return false;
            }

            ip = domainNameIpPairs[domainName];

            return true;
        }
    }
}
