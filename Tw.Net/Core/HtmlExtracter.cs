using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using Newtonsoft.Json;
using Tw.Net.Models;

namespace Tw.Net.Core
{
    public class HtmlExtracter
    {
        public static bool TryParseUser(IHtmlDocument doc, out TwitterUserModel user)
        {
            user = new TwitterUserModel();
            user.SpiderTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            try
            {
                //Get user account ID.
                var userIDElem = doc.All.Where(m => m.LocalName == "div" && m.ClassList.Contains("user-actions") &&
                 m.ClassList.Contains("btn-group") && m.ClassList.Contains("not-following") && m.HasAttribute("data-user-id")).FirstOrDefault();
                if (userIDElem != null)
                {
                    long.TryParse(userIDElem.GetAttribute("data-user-id"), out var userID);
                    user.ID = userID;
                    user.UserName = userIDElem.GetAttribute("data-screen-name");
                    user.Name = userIDElem.GetAttribute("data-name");
                    user.Private = userIDElem.GetAttribute("data-protected") == "true" ? 1 : 0;
                }
            }
            catch (Exception)
            {
                return false;
            }
            try
            {
                user.Verified = doc.QuerySelector("span.ProfileHeaderCard-badges").TextContent.Contains("Verified account") ? 1 : 0;
            }
            catch (Exception )
            {
                user.Verified = 0;
            }

            try
            {
                //Get display Name
                var avatarElem = doc.QuerySelector("img.ProfileAvatar-image");
                if (avatarElem != null)
                {
                    user.Avatar = avatarElem.GetAttribute("src");
                }
            }
            catch (Exception )
            {
                user.Avatar = string.Empty;
            }
            try
            {
                user.Bio = doc.QuerySelector("p.ProfileHeaderCard-bio").TextContent.Replace("\n", " ");
            }
            catch (Exception )
            {
                user.Bio = string.Empty;
            }
            try
            {
                user.Location = doc.QuerySelector("span.ProfileHeaderCard-locationText").TextContent.Replace("\n", " ").Trim();
            }
            catch (Exception )
            {
                user.Location = string.Empty;
            }
            try
            {
                user.Url = doc.QuerySelector("span.ProfileHeaderCard-urlText a").GetAttribute("title");
            }
            catch (Exception )
            {
                user.Url = string.Empty;
            }
            try
            {
                var joinDateTime = doc.QuerySelector("span.ProfileHeaderCard-joinDateText").GetAttribute("title").Split(new string[] { " - " }, 2, StringSplitOptions.RemoveEmptyEntries);
                user.JoinTime = joinDateTime[0];
                user.JoinDate = joinDateTime[1];
            }
            catch (Exception )
            {
                user.JoinDate = string.Empty;
                user.JoinTime = string.Empty;
            }
            try
            {
                long.TryParse(doc.QuerySelector("li.ProfileNav-item.ProfileNav-item--tweets a span.ProfileNav-value").GetAttribute("data-count"), out var tweetsCount);
                user.Tweets = tweetsCount;
            }
            catch (Exception )
            {
                user.Tweets = 0;
            }
            try
            {
                long.TryParse(doc.QuerySelector("li.ProfileNav-item.ProfileNav-item--following a span.ProfileNav-value").GetAttribute("data-count"), out var followingCount);
                user.Following = followingCount;
            }
            catch (Exception )
            {
                user.Following = 0;
            }

            try
            {
                long.TryParse(doc.QuerySelector("li.ProfileNav-item.ProfileNav-item--followers a span.ProfileNav-value").GetAttribute("data-count"), out var followersCount);
                user.Followers = followersCount;
            }
            catch (Exception )
            {
                user.Followers = 0;
            }

            try
            {
                long.TryParse(doc.QuerySelector("li.ProfileNav-item.ProfileNav-item--favorites a span.ProfileNav-value").GetAttribute("data-count"), out var favoritesCount);
                user.Likes = favoritesCount;
            }
            catch (Exception )
            {
                user.Likes = 0;
            }
            try
            {
                var mediaCountStr = doc.QuerySelector("a.PhotoRail-headingWithCount").TextContent.Trim().Split(new[] { ' ' })[0].Replace(",", "").ToLower().Trim();
                long mediaCount = 0;
                if (mediaCountStr.EndsWith("k"))
                {
                    float.TryParse(mediaCountStr.Replace("k", ""), out var count);
                    mediaCount = (long)(count * 1000);
                }
                else if (mediaCountStr.EndsWith("m"))
                {
                    float.TryParse(mediaCountStr.Replace("m", ""), out var count);
                    mediaCount = (long)(count * 1000000);
                }
                else if (mediaCountStr.EndsWith("b"))
                {
                    float.TryParse(mediaCountStr.Replace("b", ""), out var count);
                    mediaCount = (long)(count * 1000000000);
                }
                else
                {
                    long.TryParse(mediaCountStr, out mediaCount);
                }
                user.Media = mediaCount;
            }
            catch (Exception )
            {
                user.Media = 0;
            }
            try
            {
                user.BackgroundImage = doc.QuerySelector("div.ProfileCanopy-headerBg img").GetAttribute("src");
            }
            catch (Exception )
            {
                user.BackgroundImage = string.Empty;
            }

            return true;


        }

        public static bool TryParseTweet(TwitterOption option,IHtmlDocument doc,out TwitterTweetPageModel pageModel)
        {
            pageModel = null;
            pageModel = new TwitterTweetPageModel() {  HasNext = false ,Options=option};

            var tweetDivs = doc.QuerySelectorAll("div.tweet");
            if (tweetDivs==null)
            {
                return false;
            }
            for (int i = 0; i < tweetDivs.Length; i++)
            {
                var tDiv = tweetDivs[i];

                TwitterTweetModel tweet = new TwitterTweetModel();
                if (Utils.TryGetDomAttributeAsLong(tDiv,"data-item-id",out var twID))
                {
                    tweet.ID = twID;
                }
                else
                {
                    //if can't get the tweet id, then next.
                    continue;
                }

                if (Utils.TryGetDomAttributeAsString(tDiv, "data-conversation-id",out var convID))
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

                if (Utils.TryGetDomAttributeAsLong(tDiv,"data-user-id",out var userID))
                {
                    tweet.UserId = userID;
                }
                if (Utils.TryGetDomAttributeAsString(tDiv, "data-screen-name",out var userName))
                {
                    tweet.UserName = userName;
                }
                if (Utils.TryGetDomAttributeAsString(tDiv, "data-name", out var name))
                {
                    tweet.Name = name;
                }
                if (Utils.TryGetDomTextContent(tDiv, "a.js-geo-pivot-link",out var place,true))
                {
                    tweet.Place = place;
                }

                tweet.Timezone = TimeZoneInfo.Local.StandardName;

                if (Utils.TryGetDomAttributeAsString(tDiv, "data-mentions", out var mentionStr))
                {
                    var mentions = mentionStr.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                    if (mentions.Length>0)
                    {
                        tweet.Mentions.AddRange(mentions);
                    }
                }

                var links = tDiv.QuerySelectorAll("a.twitter-timeline-link");
                if (links!=null)
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
                if (photos!=null)
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
                if (hashTags!=null)
                {
                    foreach (var hashTag in hashTags)
                    {
                        tweet.HashTags.Add(hashTag.TextContent.Trim());
                    }
                }

                var cashTags = tDiv.QuerySelectorAll("a.twitter-cashtag");
                if (cashTags!=null)
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
                    if (Utils.TryGetDomAttributeAsString(tDiv, "data-retweet-id",out var reTweetID))
                    {
                        tweet.RetweetId = reTweetID;
                        if (int.TryParse(tweet.RetweetId,out var rID))
                        {
                            tweet.RetweetDate = Utils.GetDateTimeFromUnixTimestamp(((rID >> 22) + 1288834974657) / 1000).ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        
                    }
                }

                if (Utils.TryGetDomAttributeAsString(tDiv.QuerySelector("div.QuoteTweet-innerContainer"),"href",out var quote))
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