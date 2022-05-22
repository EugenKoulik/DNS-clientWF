
using System.Text.Json;
using System.Linq;

namespace DNS_clientWF
{
    public partial class Form1 : Form
    {
        DnsClient client;
        IDnsClientCache dnsClientMemoryCache;
        IDnsClientCache dnsClientFileCache;

        public Form1()
        {
            InitializeComponent();           
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dnsClientFileCache = new DnsClientFileCache();
            var domainNamePairsFromFile = dnsClientFileCache.GetDomainNameIpPairs();
            dnsClientMemoryCache = new DnsClientDictionaryCache();
            dnsClientMemoryCache.AddDomainNameIpPairs(domainNamePairsFromFile);
            int dnsServerIp = 134744072;
            int dnsServerPort = 53;
            var dnsSuffixSearchList = new DnsSuffixSearchList();
            client = new DnsClient(dnsClientMemoryCache, dnsServerIp, dnsServerPort, dnsSuffixSearchList);
            UpdateListBox();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string domainName = textBox1.Text;
            bool isLastSymbolPoint = domainName.Last() == '.';
            bool addSuffix = checkBox1.Checked && !isLastSymbolPoint;
            if (isLastSymbolPoint)
            {
                domainName = new string(domainName.Take(domainName.Length - 1).ToArray());
            }
            try
            {
                string address = client.GetIpAddress(domainName, addSuffix);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            UpdateListBox();
        }
    
        private void UpdateListBox()
        {
            Dictionary<string, string> domainNameIpPairs = dnsClientMemoryCache.GetDomainNameIpPairs();
            listBox1.Items.Clear();
            foreach(var domainNameIpPair in domainNameIpPairs)
            {
                listBox1.Items.Add($"{domainNameIpPair.Key}: {domainNameIpPair.Value}");
            }           
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Dictionary<string, string> domainNameIpPairs = dnsClientMemoryCache.GetDomainNameIpPairs();
            dnsClientFileCache.Clear();
            dnsClientFileCache.AddDomainNameIpPairs(domainNameIpPairs);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dnsClientMemoryCache.Clear();
            dnsClientFileCache.Clear();
            UpdateListBox();
        }
    }
}