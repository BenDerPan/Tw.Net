using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp.Html.Parser;
using Tw.Net.Core;
using Tw.Net.Models;

namespace Tw.Net
{
    public class Twitter
    {

        public Twitter()
        {
            DebugSettings.IsDebug = true;
        }

        public async Task InitAsync()
        {
            var isOnlineOk = await HtmlLoader.TryLoadAgentSource(true);
            if (!isOnlineOk)
            {
                isOnlineOk = await HtmlLoader.TryLoadAgentSource(false);
            }
        }

        public async Task<List<string>> GetFollowingAsync(string username)
        {
            throw new NotImplementedException();
        }

        public async Task<UserModel> GetUserProfileAsync(string username)
        {

            var htmlDoc = await HtmlLoader.TryLoadAndParsePageAsync($"https://twitter.com/{username}");
            if (htmlDoc != null)
            {
                if (HtmlExtracter.TryParseUser(htmlDoc, out var user))
                {
                    return user;
                }
            }

            return null;
        }
    }
}
