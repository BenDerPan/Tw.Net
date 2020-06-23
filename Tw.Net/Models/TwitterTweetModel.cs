using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tw.Net.Models
{
    public class TwitterTweetModel
    {
        public class ReplyToStruct
        {
            public string UserID { get; set; } = "";
            public string UserName { get; set; } = "";
        }
        public long ID { get; set; } = 0;
        public string ConversationId { get; set; } = "";
        public long CreatedAt { get; set; } = 0;
        public string Date { get; set; } = "";
        public string Time { get; set; } = "";
        public string Timezone { get; set; } = "";
        public long UserId { get; set; } = 0;
        public string UserName { get; set; } = "";
        public string Name { get; set; } = "";
        public string Place { get; set; } = "";
        public string Tweet { get; set; } = "";
        public List<string> Mentions { get; set; } = new List<string>();
        public List<string> Urls { get; set; } = new List<string>();
        public List<string> Photos { get; set; } = new List<string>();
        public long RepliesCount { get; set; } = 0;
        public long RetweetsCount { get; set; } = 0;
        public long LikesCount { get; set; } = 0;
        public List<string> HashTags { get; set; } = new List<string>();
        public List<string> CashTags { get; set; } = new List<string>();
        public string Link { get; set; } = "";

        public bool Retweet { get; set; } = false;
        public string QuoteUrl { get; set; } = "";
        public long Video { get; set; } = 0;
        public string Near { get; set; } = "";
        public string Geo { get; set; } = "";
        public string Source { get; set; } = "";
        public string UserRtId { get; set; } = "";
        public string UserRt { get; set; } = "";
        public string RetweetId { get; set; } = "";
        public List<ReplyToStruct> ReplyTo { get; set; } = new List<ReplyToStruct>();
        public string RetweetDate { get; set; } = "";
        public string Translate { get; set; } = "";
        public string TransSrc { get; set; } = "";
        public string TransDest { get; set; } = "";


        /// <summary>
        /// 爬取时间
        /// </summary>
        public string SpiderTime { get; set; } = "";

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
        public static TwitterTweetModel Parse(string json)
        {
            return JsonConvert.DeserializeObject<TwitterTweetModel>(json);
        }

        public static bool TryParse(string json, out TwitterTweetModel tweet)
        {
            tweet = null;
            try
            {
                tweet = JsonConvert.DeserializeObject<TwitterTweetModel>(json);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
