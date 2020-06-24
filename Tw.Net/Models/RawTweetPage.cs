using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tw.Net.Models
{
    public class RawTweetPage
    {
        [JsonProperty("min_position")]
        public string MinPosition { get; set; }

        [JsonProperty("has_more_items")]
        public bool HasMoreItems { get; set; }
        
        [JsonProperty("items_html")]
        public string ItemsHtml { get; set; }

        [JsonProperty("new_latent_count")]
        public long NewLatentCount { get; set; }

        [JsonProperty("focused_refresh_interval")]
        public long FocusedRefreshInterval { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static RawTweetPage Parse(string json)
        {
            return JsonConvert.DeserializeObject<RawTweetPage>(json);
        }

        public static bool TryParse(string json, out RawTweetPage rawPage)
        {
            rawPage = null;
            try
            {
                rawPage = JsonConvert.DeserializeObject<RawTweetPage>(json);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
