
using System.Text.Json;
using System.Linq;

namespace DNS_clientWF
{
    public partial class Form1 : Form
    {
        private DnsClient client;
        private IDnsClientCache domainNameIpPairsCache;

        public Form1()
        {
            InitializeComponent();           
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            client = new DnsClient(new DnsClientFileCache());
            domainNameIpPairsCache = new DnsClientDictionaryCache();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string domainName = textBox1.Text;
            string address = string.Empty;
            try
            {
                address = client.GetIpAddress(domainName);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            domainNameIpPairsCache.AddDomainNameIpPair(domainName, address);

            UpdateListBox();
        }
    
        private void UpdateListBox()
        {
            Dictionary<string, string> domainNameIpPairs = domainNameIpPairsCache.GetDomainNameIpPair();
            listBox1.Items.Clear();
            foreach(var domainNameIpPair in domainNameIpPairs)
            {
                listBox1.Items.Add($"{domainNameIpPair.Key}: {domainNameIpPair.Value}");
            }           
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }
    }
}