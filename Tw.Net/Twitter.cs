using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using Newtonsoft.Json;
using Tw.Net.Core;
using Tw.Net.Models;

namespace Tw.Net
{
    public class Twitter
    {
        private readonly List<RequestProxy> proxies = new List<RequestProxy>();
        public List<RequestProxy> Proxies => proxies;

        public Twitter()
        {
            DebugSettings.IsDebug = true;
        }

        RequestProxy GetRandomProxy()
        {
            if (proxies.Count < 1)
            {
                return null;
            }
            if (proxies.Count == 1)
            {
                return proxies[0];
            }
            Random random = new Random();
            var index = random.Next(0, proxies.Count);
            return proxies[index];
        }
        public async Task InitAsync()
        {
            var isOfflineOk = await HtmlLoader.TryLoadAgentSource(true);
            if (!isOfflineOk)
            {
                var isOnlineOk = await HtmlLoader.TryLoadAgentSource(false);
            }
        }

        public async Task<TwitterFollowPageModel> GetFollowingAsync(string username,string cursor="-1")
        {
            var url = AddressLocator.Following(username,cursor);
            var htmlDoc = await HtmlLoader.TryLoadAndParsePageAsync(url, GetRandomProxy(),false,false);
            if (htmlDoc!=null)
            {
                if (HtmlExtracter.TryParseFollowing(htmlDoc,out var followingPage))
                {
                    followingPage.BelongUserName = username;
                    return followingPage;
                }
            }
            return null; 
        }

        public async Task<TwitterFollowPageModel> GetFollowerAsync(string username, string cursor = "-1")
        {
            var url = AddressLocator.Follower(username,cursor);
            var htmlDoc = await HtmlLoader.TryLoadAndParsePageAsync(url, GetRandomProxy(), false, false);
            if (htmlDoc != null)
            {
                if (HtmlExtracter.TryParseFollower(htmlDoc, out var followerPage))
                {
                    followerPage.BelongUserName = username;
                    return followerPage;
                }
            }
            return null;
        }

        public async Task<TwitterUserModel> GetUserProfileAsync(string username)
        {

            var htmlDoc = await HtmlLoader.TryLoadAndParsePageAsync($"https://twitter.com/{username}?lang=en", GetRandomProxy());
            if (htmlDoc != null)
            {
                if (HtmlExtracter.TryParseUser(htmlDoc, out var user))
                {
                    return user;
                }
            }

            return null;
        }


        public async Task<TwitterTweetPageModel> GetUserTweetsAsync(TwitterOption option,long init=-1)
        {
            var paramDict = new Dictionary<string, string>() 
            {
                {"vertical","default" },
                {"src","unkn" },
                {"include_available_features","1" },
                {"include_entities","1" },
                {"max_position",$"{init}" },
                {"reset_error_state","false" },
            };

            StringBuilder query = new StringBuilder();

            if (!option.PopularTweets)
            {
                paramDict.Add("f", "tweets");
            }

            if (!string.IsNullOrEmpty(option.Lang))
            {
                paramDict.Add("l", option.Lang);
                paramDict.Add("lang", "en");
            }

            if (!string.IsNullOrEmpty(option.Query))
            {
                query.Append($" from:{option.Query}");
            }

            if (!string.IsNullOrEmpty(option.UserName))
            {
                query.Append($" from:{option.UserName}");
            }

            if (!string.IsNullOrEmpty(option.Geo))
            {
                option.Geo = option.Geo.Replace(" ", "");
                query.Append($" geocode:{option.Geo}");
            }

            if (!string.IsNullOrEmpty(option.Search))
            {
                query.Append($" {option.Search}");
            }
            if (option.Year>0)
            {
                query.Append($" until:{option.Year}-1-1");
            }
            if (option.Since!=null)
            {
                query.Append($" since:{option.Since.Value.ToString("yyyy-MM-dd")}");
            }
            if (option.Until!=null)
            {
                query.Append($" until:{option.Until.Value.ToString("yyyy-MM-dd")}");
            }
            if (option.Email)
            {
                query.Append(" \"mail\" OR \"email\" OR");
                query.Append(" \"gmail\" OR \"e-mail\"");
            }
            if (option.Phone)
            {
                query.Append($"  \"phone\" OR \"call me\" OR \"text me\"");
            }
            if (option.Verified)
            {
                query.Append($" filter:verified");
            }
            if (!string.IsNullOrEmpty(option.To))
            {
                query.Append($" to:{option.To}");
            }
            if (!string.IsNullOrEmpty(option.All))
            {
                query.Append($" to:{option.All} OR from:{option.All} OR @{option.All}");
            }
            if (!string.IsNullOrEmpty(option.Near))
            {
                query.Append($" near:\"{option.Near}\"");
            }
            if (option.Images)
            {
                query.Append($" filter:images");
            }
            if (option.Videos)
            {
                query.Append($" filter:videos");
            }
            if (option.Media)
            {
                query.Append($" filter:media");
            }
            if (option.Replies)
            {
                query.Append($" filter:replies");
            }
            if (option.NativeRetweets)
            {
                query.Append($" filter:nativeretweets");
            }
            if (option.MinLikes>0)
            {
                query.Append($" min_faves:{option.MinLikes}");
            }
            if (option.MinRetweets > 0)
            {
                query.Append($" min_retweets:{option.MinRetweets}");
            }
            if (option.MinReplies > 0)
            {
                query.Append($" min_replies:{option.MinReplies}");
            }

            if (option.Links == "include")
            {
                query.Append(" filter:links");
            }
            else if (option.Links=="exclude")
            {
                query.Append(" exclude:links");
            }


            if (!string.IsNullOrEmpty(option.Source))
            {
                query.Append($" source:\"{option.Source}\"");
            }

            if (!string.IsNullOrEmpty(option.MembersList))
            {
                query.Append($" list:{option.MembersList}");
            }
            if (option.FilterRetweets)
            {
                query.Append(" exclude:nativeretweets exclude:retweets");
            }
            if (!string.IsNullOrEmpty(option.CustomQuery))
            {
                query.Clear();
                query.Append(option.CustomQuery);
            }

            paramDict.Add("q", query.ToString());

            return await GetUserTweetsAsync(option, paramDict);
        }

        public async Task<TwitterTweetPageModel> GetUserTweetsAsync(TwitterOption option, Dictionary<string,string> paramDict)
        {
            var url = $"{AddressLocator.Web}/search/timeline";
            url = AddressLocator.SanitizeQuery(url, paramDict);
            var htmlSource = await HtmlLoader.TryLoadPageAsync(url, GetRandomProxy(),true,false);
            if (!string.IsNullOrEmpty(htmlSource))
            {
                if (RawTweetPage.TryParse(htmlSource,out var rawPage))
                {
                    var parser = new HtmlParser();
                    IHtmlDocument htmlDoc = null;
                    try
                    {
                        htmlDoc = await parser.ParseDocumentAsync(rawPage.ItemsHtml);
                    }
                    catch (Exception)
                    {
                        
                    }
                    
                    if (htmlDoc != null)
                    {
                        if (HtmlExtracter.TryParseTweet(option,htmlDoc, out var pageModel))
                        {
                            pageModel.NextPageParams = new Dictionary<string, string>(paramDict);
                            pageModel.HasNext = rawPage.HasMoreItems;
                            pageModel.SetMaxPosition(rawPage.MinPosition);
                            pageModel.Options = option;
                            return pageModel;
                        }
                    }
                }
            }
            

            return null;
        }
    }
}
