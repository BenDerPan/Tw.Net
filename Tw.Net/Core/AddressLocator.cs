using System.Collections.Generic;
using System.Text;

namespace Tw.Net.Core
{
    public class AddressLocator
    {
        public const string Mobile = "https://mobile.twitter.com";
        public const string Web = "https://twitter.com/i";
        public const string TwitterBase = "https://twitter.com";

        public static string SanitizeQuery(string baseUrl, Dictionary<string, string> urlParams)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("?");
            foreach (var key in urlParams.Keys)
            {
                builder.Append($"{key}={urlParams[key]}&");
            }
            builder.Remove(builder.Length - 1, 1);
            builder.Replace(":", "%3A");
            builder.Replace(" ", "%20");
            builder.Insert(0, baseUrl);
            return builder.ToString();
        }

        public static string Following(string username, string cursor = "-1")
        {
            var url = $"{Mobile}/{username}/following?lang=en";
            if (!string.IsNullOrEmpty(cursor) && cursor != "-1")
            {
                url += $"&cursor={cursor}";
            }
            return url;
        }

        public static string Follower(string username, string cursor = "-1")
        {
            var url = $"{Mobile}/{username}/followers?lang=en";
            if (!string.IsNullOrEmpty(cursor) && cursor != "-1")
            {
                url += $"&cursor={cursor}";
            }
            return url;
        }
    }
}