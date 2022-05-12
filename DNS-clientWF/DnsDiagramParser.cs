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
                return Array.Empty<byte>();
            }

            byte[] header = CreateHeader();
            byte[] question = CreateQuestion(domainName);

            return header.Concat(question).ToArray();
        }

        public static string ConvertToResponceText(byte[] responce, int udpRequestDiagramLength)
        {
            int offset = udpRequestDiagramLength + 12;
            var ip = new StringBuilder();
            byte currentByte = 0;

            for (; offset < responce.Length; offset++)
            {
                currentByte++;
                ip.Append($"{responce[offset]}.");
                if (currentByte == 4)
                {
                    ip = ip.Remove(ip.Length - 1, 1);
                    ip.Append(" ");
                    currentByte = 0;
                }
            }

            return ip.ToString();
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
