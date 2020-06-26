using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tw.Net.Models
{
    public class TwitterFollowModel
    {
        /// <summary>
        /// follower/following's screen name
        /// </summary>
        public string Name { get; set; } = "";
        /// <summary>
        ///  follower/following's username
        /// </summary>
        public string UserName { get; set; } = "";

        /// <summary>
        /// 爬取时间
        /// </summary>
        public string SpiderTime { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
        public static TwitterFollowModel Parse(string json)
        {
            return JsonConvert.DeserializeObject<TwitterFollowModel>(json);
        }

        public static bool TryParse(string json, out TwitterFollowModel following)
        {
            following = null;
            try
            {
                following = JsonConvert.DeserializeObject<TwitterFollowModel>(json);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
