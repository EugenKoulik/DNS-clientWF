using System.Net;
using System.Net.Sockets;

namespace DNS_clientWF
{
    internal class DnsClient
    {
        const int dnsServerIp = 134744072;

        const int dnsServerPort = 53;

        IPEndPoint dnsServerEndPoint = new(dnsServerIp, dnsServerPort);

        IDnsClientCache dnsClientCache;

        public DnsClient(IDnsClientCache dnsClientCache)
        {
            this.dnsClientCache = dnsClientCache;
        }

        public string GetIpAddress(string domainName)
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

            dnsClientCache.AddDomainNameIpPair(domainName, address);

            return address;
        }
    }
}
