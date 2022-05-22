using System.Net;
using System.Net.Sockets;

namespace DNS_clientWF
{
    internal class DnsClient
    {
        readonly IPEndPoint dnsServerEndPoint;

        readonly IDnsClientCache dnsClientCache;

        readonly IDnsSuffixSearchList dnsSuffixSearchList;

        public DnsClient(IDnsClientCache dnsClientCache, int dnsServerIp, int dnsServerPort, IDnsSuffixSearchList dnsSuffixSearchList)
        {
            this.dnsClientCache = dnsClientCache;
            this.dnsSuffixSearchList = dnsSuffixSearchList;
            dnsServerEndPoint = new IPEndPoint(dnsServerIp, dnsServerPort);
        }

        public string GetIpAddress(string domainName, bool addSuffix)
        {
            string address;
            if(dnsClientCache.TryGetIp(domainName, out address))
            {
                return address;
            }

            if (addSuffix)
            {
                try
                {
                    address = GetIpAddress(domainName);

                }
                catch (DomainNameNotFoundException)
                {
                    address = GetIpAddressUsingSuffixSearchList(domainName);
                }
            }
            else
            {
                address = GetIpAddress(domainName);
            }

            dnsClientCache.AddDomainNameIpPair(domainName, address);

            return address;
        }

        string GetIpAddressUsingSuffixSearchList(string domainName)
        {
            int attemptCount = 1;
            string suffix;
            string address = null;
            while (dnsSuffixSearchList.TryGetSuffix(attemptCount, out suffix))
            {
                try
                {
                    address = GetIpAddress(domainName + suffix);
                    break;
                }
                catch (DomainNameNotFoundException)
                {
                    attemptCount++;
                }
            }
            if(address == null)
            {
                throw new DomainNameNotFoundException("Данное доменное имя не существует.");
            }
            return address;
        }

        string GetIpAddress(string domainName)
        {
            string address;
            if (dnsClientCache.TryGetIp(domainName, out address))
            {
                return address;
            }
            var dnsDiagram = DnsDiagramParser.ConvertToDnsDiagram(domainName);
            var udpClient = new UdpClient();
            int countSuccessBytes;

            do
            {
                countSuccessBytes = udpClient.Send(dnsDiagram, dnsDiagram.Length, dnsServerEndPoint);
            } while (countSuccessBytes != dnsDiagram.Length);

            IPEndPoint iPEndPoint = null;
            var data = udpClient.Receive(ref iPEndPoint);

            address = DnsDiagramParser.ConvertToResponceText(data, dnsDiagram.Length);
            return address;
        }
    }
}
