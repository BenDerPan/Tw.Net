using System;
using System.IO;
using System.Linq;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using Tw.Net.Models;

namespace Tw.Net.Core
{
    public class HtmlExtracter
    {
        public static bool TryParseUser(IHtmlDocument doc, out UserModel user)
        {
            user = new UserModel();
            // var htmlSource = File.ReadAllText("https:twitter.comrealDonaldTrump.html");
            // var parser = new HtmlParser();
            // var doc = await parser.ParseDocumentAsync(htmlSource);
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
            catch (Exception ex)
            {
                return false;
            }
            try
            {
                user.Verified = doc.QuerySelector("span.ProfileHeaderCard-badges").TextContent.Contains("Verified account") ? 1 : 0;
            }
            catch (Exception ex)
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
            catch (Exception ex)
            {
                user.Avatar = string.Empty;
            }
            try
            {
                user.Bio = doc.QuerySelector("p.ProfileHeaderCard-bio").TextContent.Replace("\n", " ");
            }
            catch (Exception ex)
            {
                user.Bio = string.Empty;
            }
            try
            {
                user.Location = doc.QuerySelector("span.ProfileHeaderCard-locationText").TextContent.Replace("\n", " ").Trim();
            }
            catch (Exception ex)
            {
                user.Location = string.Empty;
            }
            try
            {
                user.Url = doc.QuerySelector("span.ProfileHeaderCard-urlText a").GetAttribute("title");
            }
            catch (Exception ex)
            {
                user.Url = string.Empty;
            }
            try
            {
                var joinDateTime = doc.QuerySelector("span.ProfileHeaderCard-joinDateText").GetAttribute("title").Split(new string[] { " - " }, 2, StringSplitOptions.RemoveEmptyEntries);
                user.JoinTime = joinDateTime[0];
                user.JoinDate = joinDateTime[1];
            }
            catch (Exception ex)
            {
                user.JoinDate = string.Empty;
                user.JoinTime = string.Empty;
            }
            try
            {
                int.TryParse(doc.QuerySelector("li.ProfileNav-item.ProfileNav-item--tweets a span.ProfileNav-value").GetAttribute("data-count"), out var tweetsCount);
                user.Tweets = tweetsCount;
            }
            catch (Exception ex)
            {
                user.Tweets = 0;
            }
            try
            {
                int.TryParse(doc.QuerySelector("li.ProfileNav-item.ProfileNav-item--following a span.ProfileNav-value").GetAttribute("data-count"), out var followingCount);
                user.Following = followingCount;
            }
            catch (Exception ex)
            {
                user.Following = 0;
            }

            try
            {
                int.TryParse(doc.QuerySelector("li.ProfileNav-item.ProfileNav-item--followers a span.ProfileNav-value").GetAttribute("data-count"), out var followersCount);
                user.Followers = followersCount;
            }
            catch (Exception ex)
            {
                user.Followers = 0;
            }

            try
            {
                int.TryParse(doc.QuerySelector("li.ProfileNav-item.ProfileNav-item--favorites a span.ProfileNav-value").GetAttribute("data-count"), out var favoritesCount);
                user.Likes = favoritesCount;
            }
            catch (Exception ex)
            {
                user.Likes = 0;
            }
            try
            {
                var mediaCountStr = doc.QuerySelector("a.PhotoRail-headingWithCount").TextContent.Trim().Split(new[] { ' ' })[0].Replace(",", "").ToLower().Trim();
                int mediaCount = 0;
                if (mediaCountStr.EndsWith("k"))
                {
                    float.TryParse(mediaCountStr.Replace("k", ""), out var count);
                    mediaCount = (int)(count * 1000);
                }
                else if (mediaCountStr.EndsWith("m"))
                {
                    float.TryParse(mediaCountStr.Replace("m", ""), out var count);
                    mediaCount = (int)(count * 1000000);
                }
                else if (mediaCountStr.EndsWith("b"))
                {
                    float.TryParse(mediaCountStr.Replace("b", ""), out var count);
                    mediaCount = (int)(count * 1000000000);
                }
                else
                {
                    int.TryParse(mediaCountStr, out mediaCount);
                }
                user.Media = mediaCount;
            }
            catch (Exception ex)
            {
                user.Media = 0;
            }
            try
            {
                user.BackgroundImage = doc.QuerySelector("div.ProfileCanopy-headerBg img").GetAttribute("src");
            }
            catch (Exception ex)
            {
                user.BackgroundImage = string.Empty;
            }

            return true;


        }
    }
}