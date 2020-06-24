using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Tw.Net.Core;

namespace Tw.Net.Models
{
    public class TwitterTweetPageModel
    {
        public List<TwitterTweetModel> Tweets { get; set; }

        public Dictionary<string,string> NextPageParams { get; set; }

        public bool HasNext { get; set; }
        public TwitterOption Options { get; set; }

        public bool IsValid => Tweets.Count > 0;

        public TwitterTweetPageModel()
        {
            Tweets = new List<TwitterTweetModel>();
            NextPageParams = new Dictionary<string, string>();
            Options = new TwitterOption();
            HasNext = false;
        }

        public void SetMaxPosition(string maxPosition)
        {
            NextPageParams["max_position"] = maxPosition;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static TwitterTweetPageModel Parse(string json)
        {
            return JsonConvert.DeserializeObject<TwitterTweetPageModel>(json);
        }

        public static bool TryParse(string json, out TwitterTweetPageModel tweetPage)
        {
            tweetPage = null;
            try
            {
                tweetPage = JsonConvert.DeserializeObject<TwitterTweetPageModel>(json);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
