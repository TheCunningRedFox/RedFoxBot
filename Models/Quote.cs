using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedCunningFoxBot.Models
{
    public partial class Quote
    {
        [JsonProperty("quoteText", NullValueHandling = NullValueHandling.Ignore)]
        public string QuoteText { get; set; }

        [JsonProperty("quoteAuthor", NullValueHandling = NullValueHandling.Ignore)]
        public string QuoteAuthor { get; set; }

        [JsonProperty("senderName", NullValueHandling = NullValueHandling.Ignore)]
        public string SenderName { get; set; }

        [JsonProperty("senderLink", NullValueHandling = NullValueHandling.Ignore)]
        public string SenderLink { get; set; }
    }

    public partial class Quote
    {
        public static Quote FromJson(string json) => JsonConvert.DeserializeObject<Quote>(json, Converter.Settings);
    }
}
