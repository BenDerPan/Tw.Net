using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tw.Net.Models
{
    public class TwitterFollowingModel
    {
        /// <summary>
        /// The relation belong to account username
        /// </summary>
        public string BelongUserName { get; set; } = "";
        /// <summary>
        /// following's screen name
        /// </summary>
        public string Name { get; set; } = "";
        /// <summary>
        /// following's username
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
        public static TwitterFollowingModel Parse(string json)
        {
            return JsonConvert.DeserializeObject<TwitterFollowingModel>(json);
        }

        public static bool TryParse(string json, out TwitterFollowingModel following)
        {
            following = null;
            try
            {
                following = JsonConvert.DeserializeObject<TwitterFollowingModel>(json);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
