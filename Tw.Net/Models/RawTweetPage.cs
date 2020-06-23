using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tw.Net.Models
{
    public class RawTweetPage
    {
        public string MinPosition { get; set; }

        public bool HasMoreItems { get; set; }
        public string ItemsHtml { get; set; }
        public long NewLatentCount { get; set; }

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
