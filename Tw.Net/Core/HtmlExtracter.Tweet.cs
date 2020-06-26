using AngleSharp.Html.Dom;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Tw.Net.Models;

namespace Tw.Net.Core
{
    public partial class HtmlExtracter
    {
        public static bool TryParseTweet(TwitterOption option, IHtmlDocument doc, out TwitterTweetPageModel pageModel)
        {
            pageModel = null;
            pageModel = new TwitterTweetPageModel() { HasNext = false, Options = option };

            var tweetDivs = doc.QuerySelectorAll("div.tweet");
            if (tweetDivs == null)
            {
                return false;
            }
            for (int i = 0; i < tweetDivs.Length; i++)
            {
                var tDiv = tweetDivs[i];

                TwitterTweetModel tweet = new TwitterTweetModel();
                if (Utils.TryGetDomAttributeAsLong(tDiv, "data-item-id", out var twID))
                {
                    tweet.ID = twID;
                }
                else
                {
                    //if can't get the tweet id, then next.
                    continue;
                }

                if (Utils.TryGetDomAttributeAsString(tDiv, "data-conversation-id", out var convID))
                {
                    tweet.ConversationId = convID;
                }

                if (Utils.TryGetDomAttributeAsLong(tDiv.QuerySelector("span._timestamp"), "data-time-ms", out var createTimestamp))
                {
                    tweet.CreatedAt = createTimestamp;
                    var dt = Utils.GetDateTimeFromUnixTimestamp(createTimestamp);
                    tweet.Date = dt.ToString("yyyy-MM-dd");
                    tweet.Time = dt.ToString("HH:mm:ss");
                }

                if (Utils.TryGetDomAttributeAsLong(tDiv, "data-user-id", out var userID))
                {
                    tweet.UserId = userID;
                }
                if (Utils.TryGetDomAttributeAsString(tDiv, "data-screen-name", out var userName))
                {
                    tweet.UserName = userName;
                }
                if (Utils.TryGetDomAttributeAsString(tDiv, "data-name", out var name))
                {
                    tweet.Name = name;
                }
                if (Utils.TryGetDomTextContent(tDiv, "a.js-geo-pivot-link", out var place, true))
                {
                    tweet.Place = place;
                }

                tweet.Timezone = TimeZoneInfo.Local.StandardName;

                if (Utils.TryGetDomAttributeAsString(tDiv, "data-mentions", out var mentionStr))
                {
                    var mentions = mentionStr.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                    if (mentions.Length > 0)
                    {
                        tweet.Mentions.AddRange(mentions);
                    }
                }

                var links = tDiv.QuerySelectorAll("a.twitter-timeline-link");
                if (links != null)
                {
                    foreach (var lnk in links)
                    {
                        if (lnk.HasAttribute("data-expanded-url"))
                        {
                            tweet.Urls.Add(lnk.GetAttribute("data-expanded-url").Trim());
                        }
                    }
                }

                var photos = tDiv.QuerySelectorAll("div.AdaptiveMedia-photoContainer");
                if (photos != null)
                {
                    foreach (var pho in photos)
                    {
                        if (Utils.TryGetDomAttributeAsString(pho, "data-image-url", out var photoUrl))
                        {
                            tweet.Photos.Add(photoUrl.Trim());
                        }
                    }
                }

                if (Utils.TryGetDomTextContent(tDiv, "p.tweet-text", out var content))
                {
                    content = content.Replace("http", " http").Replace("pic.twitter", " pic.twitter");
                    tweet.Tweet = content;
                }

                var hashTags = tDiv.QuerySelectorAll("a.twitter-hashtag");
                if (hashTags != null)
                {
                    foreach (var hashTag in hashTags)
                    {
                        tweet.HashTags.Add(hashTag.TextContent.Trim());
                    }
                }

                var cashTags = tDiv.QuerySelectorAll("a.twitter-cashtag");
                if (cashTags != null)
                {
                    foreach (var cash in cashTags)
                    {
                        tweet.CashTags.Add(cash.TextContent.Trim());
                    }
                }

                if (Utils.TryGetDomAttributeAsLong(tDiv.QuerySelector("span.ProfileTweet-action--reply.u-hiddenVisually span"), "data-tweet-stat-count", out var replyCount))
                {
                    tweet.RepliesCount = replyCount;
                }

                if (Utils.TryGetDomAttributeAsLong(tDiv.QuerySelector("span.ProfileTweet-action--retweet.u-hiddenVisually span"), "data-tweet-stat-count", out var retweetCount))
                {
                    tweet.RetweetsCount = retweetCount;
                }

                if (Utils.TryGetDomAttributeAsLong(tDiv.QuerySelector("span.ProfileTweet-action--favorite.u-hiddenVisually span"), "data-tweet-stat-count", out var favoriteCount))
                {
                    tweet.LikesCount = favoriteCount;
                }

                tweet.Link = $"https://twitter.com/{tweet.UserName}/status/{tweet.ID}";

                if (Utils.TryGetDomAttributeAsString(tDiv.QuerySelector("span.js-retweet-text a"), "data-user-id", out var retUserID))
                {
                    tweet.UserRtId = retUserID;
                }

                if (Utils.TryGetDomAttributeAsString(tDiv.QuerySelector("span.js-retweet-text a"), "href", out var retUserName))
                {
                    tweet.UserRt = retUserName.Substring(1);
                }

                tweet.Retweet = !string.IsNullOrEmpty(tweet.UserRt);

                if (!string.IsNullOrEmpty(tweet.UserRt))
                {
                    if (Utils.TryGetDomAttributeAsString(tDiv, "data-retweet-id", out var reTweetID))
                    {
                        tweet.RetweetId = reTweetID;
                        if (int.TryParse(tweet.RetweetId, out var rID))
                        {
                            tweet.RetweetDate = Utils.GetDateTimeFromUnixTimestamp(((rID >> 22) + 1288834974657) / 1000).ToString("yyyy-MM-dd HH:mm:ss");
                        }

                    }
                }

                if (Utils.TryGetDomAttributeAsString(tDiv.QuerySelector("div.QuoteTweet-innerContainer"), "href", out var quote))
                {
                    tweet.QuoteUrl = $"{AddressLocator.TwitterBase}{quote}";
                }

                tweet.Near = option.Near;
                tweet.Geo = option.Geo;
                tweet.Source = option.Source;
                if (Utils.TryGetDomAttributeAsString(tDiv, "data-reply-to-users-json", out var replyToJson))
                {
                    try
                    {
                        var replyToList = JsonConvert.DeserializeObject<List<TwitterTweetModel.ReplyToStruct>>(replyToJson);
                        tweet.ReplyTo.AddRange(replyToList);

                    }
                    catch (Exception)
                    {

                    }
                }

                tweet.Translate = "";
                tweet.TransSrc = "";
                tweet.TransDest = "";

                if (option.Translate)
                {
                    //TODO: Add translate implement
                }

                pageModel.Tweets.Add(tweet);
            }

            return true;

        }
    }
}
