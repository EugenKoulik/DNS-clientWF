using DnsClient;
using System.Text.Json;

namespace DNS_clientWF
{
    public partial class Form1 : Form
    {
        private DnsClient client;
        private Dictionary<string, string> domainNameIpPairs;

        public Form1()
        {
            InitializeComponent();           
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            client = new DnsClient(new DnsClientFileCache());
            domainNameIpPairs = new Dictionary<string, string>();

            UpdateListBox();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            client.GetIpAddress(textBox1.Text);

            UpdateListBox();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ClearJson();

            UpdateListBox();
        }
    
        private void UpdateListBox()
        {
            if (File.Exists(DnsClientFileCache.pathToFileCache))
            {
                using (var sr = new StreamReader(DnsClientFileCache.pathToFileCache))
                {
                    string json = sr.ReadToEnd();

                    domainNameIpPairs = JsonSerializer.Deserialize<Dictionary<string, string>>(json);

                    listBox1.Items.Clear();

                    foreach (var pair in domainNameIpPairs)
                    {
                        listBox1.Items.Add($"{pair.Key}:{pair.Value}");
                    }
                }
            }           
        }

        private void ClearJson()
        {
            if (File.Exists(DnsClientFileCache.pathToFileCache))
            {
                using (var sw = new StreamWriter(DnsClientFileCache.pathToFileCache, false))
                {
                    string json = "{}";

                    sw.Write(json);
                }
            }

        }
    }
}