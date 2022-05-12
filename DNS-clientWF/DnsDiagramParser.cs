using System;
using System.Linq;
using System.Text;

namespace DNS_clientWF
{
    static class DnsDiagramParser
    {
        public static byte[] ConvertToDnsDiagram(string domainName)
        {
            if (domainName == null || domainName == string.Empty)
            {
                throw new Exception("Введено пустое имя.");
            }

            byte[] header = CreateHeader();
            byte[] question = CreateQuestion(domainName);

            return header.Concat(question).ToArray();
        }

        public static string ConvertToResponceText(byte[] responce, int udpRequestDiagramLength)
        {
            CheckResponceStatus(responce);

            var ip = new StringBuilder();

            for(int offset = udpRequestDiagramLength; offset < responce.Length; offset += 16)
            {
                for(int ipAddressOffset = offset + 12; ipAddressOffset < offset + 16; ipAddressOffset++)
                {
                    ip.Append($"{responce[ipAddressOffset]}.");
                }
                ip.Remove(ip.Length - 1, 1);
                ip.Append(" ");
            }

            return ip.ToString();
        }

        static void CheckResponceStatus(byte[] responce)
        {
            RCodes rcode = (RCodes)(responce[3] & 15);
            switch (rcode)
            {
                case RCodes.FormatError:
                    throw new Exception("Format error - The name server was unable to interpret the query.");
                case RCodes.ServerFailure:
                    throw new Exception("Server failure - The name server was unable to process this query due to a problem with the name server.");
                case RCodes.NameError:
                    throw new Exception("Данное доменное имя не существует.");
                case RCodes.NotImplemented:
                    throw new Exception("Not Implemented - The name server does not support the requested kind of query.");
                case RCodes.Refused:
                    throw new Exception("Refused - The name server refuses to perform the specified operation for policy reasons.  For example, a name server may not wish to provide the information to the particular requester, or a name server may not wish to perform a particular operation(e.g., zone transfer) for particular data.");
            }
        }

        static byte[] CreateHeader()
        {
            var id = new byte[2] { 170, 170 };
            var messageParams = new byte[2] { 1, 0 };
            var countQuestions = new byte[2] { 0, 1 };
            var countResponces = new byte[2] { 0, 0 };
            var otherData = new byte[4] { 0, 0, 0, 0 };
            return id
                .Concat(messageParams)
                .Concat(countQuestions)
                .Concat(countResponces)
                .Concat(otherData)
                .ToArray();
        }

        static byte[] CreateQuestion(string domainName)
        {
            string[] labels = domainName.Split('.', StringSplitOptions.RemoveEmptyEntries);
            byte[] labelsBytes = Array.Empty<byte>();

            foreach (var label in labels)
            {
                var labelBytes = CreateLabel(label);
                labelsBytes = labelsBytes.Concat(labelBytes).ToArray();
            }

            var labelsEnd = new byte[1] { 0 };
            var qType = new byte[2] { 0, 1 };
            var qClass = new byte[2] { 0, 1 };

            return labelsBytes
                .Concat(labelsEnd)
                .Concat(qType)
                .Concat(qClass)
                .ToArray();
        }

        static byte[] CreateLabel(string label)
        {
            var labelLength = new byte[1] { (byte)label.Length };
            var labelBytes = label.Select(x => (byte)x).ToArray();
            return labelLength.Concat(labelBytes).ToArray();
        }
    }
}
