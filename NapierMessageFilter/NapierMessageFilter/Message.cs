using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NapierMessageFilter
{
    public class SIReport
    {
        public string Sortcode { get; set; }
        public string Incident { get; set; }
    }

    public class Message
    {
        public bool IsSIR { get; set; }
        public string Header { get; set; }
        public string Sender { get; set; }
        public string Subject { get; set; }
        public string Type { get; set; }
        public List<string> Words { get; set; }
        public List<string> Hashtags { get; set; }
        public List<string> Mentions { get; set; }
        public List<string> URLs { get; set; }
        public SIReport SIReport { get; set; } 

        public Message()
        {
            Words = new List<string>();
            Hashtags = new List<string>();
            Mentions = new List<string>();
            URLs = new List<string>();
            SIReport = new SIReport();
        }
    }
}
