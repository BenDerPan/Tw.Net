using AngleSharp.Html.Dom;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Tw.Net.Models;

namespace Tw.Net.Core
{
    public partial class HtmlExtracter
    {
        public static bool TryParseFollowing(IHtmlDocument doc, out TwitterFollowPageModel followingPage)
        {
            return TryParseFollow(doc, out followingPage);
        }

        public static bool TryParseFollower(IHtmlDocument doc, out TwitterFollowPageModel followerPage)
        {
            return TryParseFollow(doc, out followerPage);
        }

        public static bool TryParseFollow(IHtmlDocument doc, out TwitterFollowPageModel followPage)
        {
            followPage = new TwitterFollowPageModel();

            var followElems = doc.QuerySelectorAll("td.info.fifty.screenname");
            if (followElems != null)
            {
                foreach (var follow in followElems)
                {
                    var followingModel = new TwitterFollowModel();
                    if (Utils.TryGetDomAttributeAsString(follow.QuerySelector("a"), "name", out var userName))
                    {
                        followingModel.UserName = userName;
                    }
                    if (Utils.TryGetDomTextContent(follow, "a strong", out var name))
                    {
                        followingModel.Name = name;
                    }
                    if (!string.IsNullOrEmpty(followingModel.UserName))
                    {
                        followPage.Follows.Add(followingModel);
                    }
                }

                //find the next cursor
                if (Utils.TryGetDomAttributeAsString(doc.QuerySelector("div.w-button-more a"), "href", out var nextCursorUrl))
                {
                    if (nextCursorUrl.Contains("?cursor="))
                    {
                        try
                        {
                            var cursor = nextCursorUrl.Split(new[] { "?cursor=" }, StringSplitOptions.RemoveEmptyEntries)[1].Trim();
                            followPage.NextCursor = cursor;
                        }
                        catch (Exception)
                        {

                        }

                    }
                }

                return true;

            }


            return false;
        }
    }
}
