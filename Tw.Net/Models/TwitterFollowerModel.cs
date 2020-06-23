using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tw.Net.Models
{
    public class TwitterFollowerModel
    {
        /// <summary>
        /// The relation belong to account username
        /// </summary>
        public string BelongUserName { get; set; } = "";
        /// <summary>
        /// follower's screen name
        /// </summary>
        public string Name { get; set; } = "";
        /// <summary>
        /// follower's username
        /// </summary>
        public string UserName { get; set; } = "";
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

        public static TwitterFollowerModel Parse(string json)
        {
            return JsonConvert.DeserializeObject<TwitterFollowerModel>(json);
        }

        public static bool TryParse(string json, out TwitterFollowerModel follower)
        {
            follower = null;
            try
            {
                follower = JsonConvert.DeserializeObject<TwitterFollowerModel>(json);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
