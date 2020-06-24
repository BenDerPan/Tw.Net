using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Tw.Net.Core;

namespace Tw.Net.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            TestAsync().Wait();
        }

        static async Task TestAsync()
        {
            Twitter tw = new Twitter();
            tw.Proxies.Add(new Core.RequestProxy("127.0.0.1", 1080));
            //Notice: Online user agent are not work now!!!
            //await tw.InitAsync();

            var testUsers = new List<string>(){
                 "realDonaldTrump","BreitbartNews","taylorswift13"
            };
            foreach (var acc in testUsers)
            {
                //DebugSettings.LogInfo("Get User Profile", $"===================={acc}=====================");
                //var user = await tw.GetUserProfileAsync(acc);

                //DebugSettings.LogInfo("User Profile", $"{Environment.NewLine}{user}");

                DebugSettings.LogInfo("Get User Tweets", $"===================={acc}=====================");
                var pageModel = await tw.GetUserTweetsAsync(new TwitterOption() { UserName=acc,Since=new DateTime(2020,5,1,0,0,0)},-1);

                DebugSettings.LogInfo("User Tweet", $"{Environment.NewLine}{pageModel}");
                var nextPage = pageModel;
                while (true)
                {
                    if (nextPage==null)
                    {
                        break;
                    }
                    if (!nextPage.HasNext)
                    {
                        break;
                    }
                    nextPage = await tw.GetUserTweetsAsync(nextPage.Options, nextPage.NextPageParams);
                    DebugSettings.LogInfo("User Tweet", $"{Environment.NewLine}{nextPage}");
                    await Task.Delay(3);
                }
            }



        }
    }
}
