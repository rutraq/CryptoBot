using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace LibraryCex
{
    public class TradeHistory
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("amount")]
        public string Amount { get; set; }

        [JsonProperty("price")]
        public string Price { get; set; }

        [JsonProperty("tid")]
        public string Tid { get; set; }
    }
}
