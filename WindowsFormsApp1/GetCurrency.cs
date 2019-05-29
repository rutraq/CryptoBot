using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace WindowsFormsApp1
{
    class GetCurrency
    {
        public string GetCur()
        {
            StreamReader strr = new StreamReader(WebRequest.Create(@"https://cex.io/api/last_price/XRP/USD").GetResponse().GetResponseStream());
            return strr.ReadToEnd();
        }

        public Dictionary<string, string> ParseJSON()
        {
            Dictionary<string, string> dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(GetCur());
            return dict;
        }
    }
  
}
