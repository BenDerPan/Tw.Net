using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tw.Net.Models
{
    public class TwitterFollowPageModel
    {
        /// <summary>
        /// The relation belong to account username
        /// </summary>
        public string BelongUserName { get; set; } = "";
        public List<TwitterFollowModel> Follows { get; set; }

        public string NextCursor { get; set; }

        public bool IsValid => Follows.Count > 0;

        public bool HasNext => !string.IsNullOrEmpty(NextCursor);

        public TwitterFollowPageModel()
        {
            Follows = new List<TwitterFollowModel>();
            NextCursor = string.Empty;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static TwitterFollowPageModel Parse(string json)
        {
            return JsonConvert.DeserializeObject<TwitterFollowPageModel>(json);
        }

        public static bool TryParse(string json, out TwitterFollowPageModel followPage)
        {
            followPage = null;
            try
            {
                followPage = JsonConvert.DeserializeObject<TwitterFollowPageModel>(json);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
