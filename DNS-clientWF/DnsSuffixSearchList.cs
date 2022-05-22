using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNS_clientWF
{
    internal class DnsSuffixSearchList : IDnsSuffixSearchList
    {
        string[] suffixes;

        const string pathToFile = "dnsSuffixesList.txt";

        public DnsSuffixSearchList()
        {
            if (File.Exists(pathToFile))
            {
                suffixes = GetSuffixesFromFile();
            }
        }
        public bool TryGetSuffix(int attemptNumber, out string suffix)
        {
            suffix = string.Empty;
            if(suffixes == null || attemptNumber >= suffixes.Length + 1 || attemptNumber < 1)
            {
                return false;
            }
            suffix = suffixes[attemptNumber - 1];
            return true;
        }

        string[] GetSuffixesFromFile()
        {
            string[] suffixes;
            using (var sr = new StreamReader(pathToFile))
            {
                string text = sr.ReadToEnd();
                suffixes = text.Split(new string[] { " ", Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            }
            return suffixes;
        }
    }
}
