using System.Text.Json;


namespace DNS_clientWF
{
    internal class DnsClientFileCache : IDnsClientCache
    {
        const string pathToFileCache = "dnsClientCache.json";
        public void AddDomainNameIpPair(string domainName, string ip)
        {
            Dictionary<string, string> domainNameIpPairs = GetDomainNameIpPairs();

            domainNameIpPairs[domainName] = ip;

            WriteCacheToFile(domainNameIpPairs);
        }

        public void AddDomainNameIpPairs(Dictionary<string, string> domainNameIpPairs)
        {
            Dictionary<string, string> domainNameIpPairsFromFile = GetDomainNameIpPairs();
            foreach (var domainNameIpPair in domainNameIpPairs)
            {
                domainNameIpPairsFromFile[domainNameIpPair.Key] = domainNameIpPair.Value;
            }
            WriteCacheToFile(domainNameIpPairsFromFile);
        }

        public Dictionary<string, string> GetDomainNameIpPairs()
        {
            Dictionary<string, string> domainNameIpPairs;

            if (!File.Exists(pathToFileCache))
            {
                domainNameIpPairs = new Dictionary<string, string>();
            }
            else
            {
                domainNameIpPairs = UnsafeGetDomainNameIpPair();
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
            Dictionary<string, string> domainNameIpPairs = UnsafeGetDomainNameIpPair();

            if (domainNameIpPairs == null || !domainNameIpPairs.ContainsKey(domainName))
            {
                ip = string.Empty;
                return false;
            }

            ip = domainNameIpPairs[domainName];

            return true;
        }
        public void Clear()
        {
            if (File.Exists(pathToFileCache))
            {
                File.Delete(pathToFileCache);
            }
        }

        Dictionary<string, string> UnsafeGetDomainNameIpPair()
        {
            Dictionary<string, string> domainNameIpPairs;
            using (var sr = new StreamReader(pathToFileCache))
            {
                string json = sr.ReadToEnd();
                domainNameIpPairs = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
            }
            return domainNameIpPairs;
        }

        void WriteCacheToFile(Dictionary<string, string> domainNameIpPairs)
        {
            using (var sw = new StreamWriter(pathToFileCache, false))
            {
                string json = JsonSerializer.Serialize(domainNameIpPairs);
                sw.Write(json);
            }
        }
    }
}
