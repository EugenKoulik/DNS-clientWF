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

        public DnsSuffixSearchList()
        {
            suffixes = new string[] { ".ru", ".com", ".org" };
        }
        public bool TryGetSuffix(int attemptNumber, out string suffix)
        {
            suffix = string.Empty;
            if(attemptNumber >= suffixes.Length + 1 || attemptNumber < 1)
            {
                return false;
            }
            suffix = suffixes[attemptNumber - 1];
            return true;
        }
    }
}
