using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Tw.Net.Core;

namespace Tw.Net.Example
{
    class Program
    {
        static readonly string CacheProfileDir = "Cache/Profile";
        static readonly string CacheTweetDir = "Cache/Tweet";
        static readonly string CacheFollowingDir = "Cache/Following";
        static readonly string CacheFollowerDir = "Cache/Follower";

        static void MakeDir(string dir)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(Path.GetFullPath(dir));
            }
        }
        static void Main(string[] args)
        {
            MakeDir(CacheProfileDir);
            MakeDir(CacheTweetDir);
            MakeDir(CacheFollowingDir);
            MakeDir(CacheFollowerDir);

            TestAsync().Wait();
        }

        static async Task TestAsync()
        {
            Twitter tw = new Twitter();
            tw.Proxies.Add(new Core.RequestProxy("127.0.0.1", 1080));
            //Notice: Online user agent are not work now!!!
            await tw.InitAsync();

            var testUsers = new List<string>(){
                 "realDonaldTrump","BreitbartNews","taylorswift13"
            };
            foreach (var acc in testUsers)
            {
                //-------------------------------Profile-------------------------------------------------------
                DebugSettings.LogInfo("Get User Profile", $"===================={acc}=====================");
                var user = await tw.GetUserProfileAsync(acc);
                File.AppendAllLines(Path.Combine(CacheProfileDir, $"{acc}-profile.json"), new[] { user.ToString() });

                DebugSettings.LogInfo("User Profile", $"{Environment.NewLine}{user}");

                //-------------------------------Tweets------------------------------------------------------ -

                DebugSettings.LogInfo("Get User Tweets", $"===================={acc}=====================");
                var pageModel = await tw.GetUserTweetsAsync(new TwitterOption() { UserName = acc, Since = new DateTime(2020, 5, 1, 0, 0, 0) }, -1);
                File.AppendAllLines(Path.Combine(CacheTweetDir, $"{acc}-tweet.json"), new[] { pageModel.ToString() });
                DebugSettings.LogInfo("User Tweet", $"{Environment.NewLine}{pageModel}");
                var nextPage = pageModel;
                while (true)
                {
                    if (nextPage == null)
                    {
                        break;
                    }
                    if (!nextPage.HasNext)
                    {
                        break;
                    }
                    nextPage = await tw.GetUserTweetsAsync(nextPage.Options, nextPage.NextPageParams);
                    File.AppendAllLines(Path.Combine(CacheTweetDir, $"{acc}-tweet.json"), new[] { nextPage.ToString() });
                    DebugSettings.LogInfo("User Tweet", $"{Environment.NewLine}{nextPage}");
                    await Task.Delay(3);
                }


                //-------------------------------Following-------------------------------------------------------
                DebugSettings.LogInfo("Get Following", $"===================={acc}=====================");
                var followingPage = await tw.GetFollowingAsync(acc);
                DebugSettings.LogInfo("User Following", $"{Environment.NewLine}{followingPage}");
                File.AppendAllLines(Path.Combine(CacheFollowingDir, $"{acc}-following.json"), new[] { followingPage.ToString() });
                var nextFollowingPage = followingPage;
                while (true)
                {
                    if (nextFollowingPage == null)
                    {
                        break;
                    }
                    if (!nextFollowingPage.HasNext)
                    {
                        break;
                    }
                    nextFollowingPage = await tw.GetFollowingAsync(acc, nextFollowingPage.NextCursor);
                    File.AppendAllLines(Path.Combine(CacheFollowingDir, $"{acc}-following.json"), new[] { nextFollowingPage.ToString() });
                    DebugSettings.LogInfo("User Following", $"{Environment.NewLine}{nextFollowingPage}");
                    await Task.Delay(3);
                }

                //-------------------------------Follower-------------------------------------------------------
                DebugSettings.LogInfo("Get Follower", $"===================={acc}=====================");
                var followerPage = await tw.GetFollowerAsync(acc);
                File.AppendAllLines(Path.Combine(CacheFollowingDir, $"{acc}-follower.json"), new[] { followerPage.ToString() });
                DebugSettings.LogInfo("User Follower", $"{Environment.NewLine}{followerPage}");
                var nextFollowerPage = followerPage;
                while (true)
                {
                    if (nextFollowerPage == null)
                    {
                        break;
                    }
                    if (!nextFollowerPage.HasNext)
                    {
                        break;
                    }
                    nextFollowerPage = await tw.GetFollowerAsync(acc, nextFollowerPage.NextCursor);
                    File.AppendAllLines(Path.Combine(CacheFollowingDir, $"{acc}-follower.json"), new[] { nextFollowerPage.ToString() });
                    DebugSettings.LogInfo("User Follower", $"{Environment.NewLine}{nextFollowerPage}");
                    await Task.Delay(3);
                }
            }



        }
    }
}
