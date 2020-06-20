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

                user.Verified = doc.QuerySelector("span.ProfileHeaderCard-badges").TextContent.Contains("Verified account") ? 1 : 0;

                //Get display Name
                var avatarElem = doc.QuerySelector("img.ProfileAvatar-image");
                if (avatarElem != null)
                {
                    user.Avatar = avatarElem.GetAttribute("src");
                }

                user.Bio = doc.QuerySelector("p.ProfileHeaderCard-bio").TextContent.Replace("\n", " ");
                user.Location = doc.QuerySelector("span.ProfileHeaderCard-locationText").TextContent.Replace("\n", " ");
                user.Url = doc.QuerySelector("span.ProfileHeaderCard-urlText a").GetAttribute("title");
                var joinDateTime = doc.QuerySelector("span.ProfileHeaderCard-joinDateText").GetAttribute("title").Split(new string[] { " - " }, 2, StringSplitOptions.RemoveEmptyEntries);
                user.JoinTime = joinDateTime[0];
                user.JoinDate = joinDateTime[1];
                int.TryParse(doc.QuerySelector("li.ProfileNav-item.ProfileNav-item--tweets span.ProfileNav-value").GetAttribute("data-count"), out var tweetsCount);
                user.Tweets = tweetsCount;

                int.TryParse(doc.QuerySelector("li.ProfileNav-item.ProfileNav-item--following span.ProfileNav-value").GetAttribute("data-count"), out var followingCount);
                user.Following = followingCount;

                int.TryParse(doc.QuerySelector("li.ProfileNav-item.ProfileNav-item--followers span.ProfileNav-value").GetAttribute("data-count"), out var followersCount);
                user.Followers = followersCount;

                int.TryParse(doc.QuerySelector("li.ProfileNav-item.ProfileNav-item--favorites span.ProfileNav-value").GetAttribute("data-count"), out var favoritesCount);
                user.Likes = favoritesCount;
                var mediaCountStr = doc.QuerySelector("a.PhotoRail-headingWithCount").TextContent.Split(new[] { ' ' })[0].Replace(",", "").ToLower();
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
                user.BackgroundImage = doc.QuerySelector("span.ProfileCanopy-headerBg img").GetAttribute("src");
                return true;
            }
            catch (Exception ex)
            {
                //按照新的界面格式解析
                

            }

            return false;


        }
    }
}