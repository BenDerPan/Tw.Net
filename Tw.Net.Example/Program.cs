using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

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
                Console.WriteLine($"===================={acc}=====================");
                var user = await tw.GetUserProfileAsync(acc);

                Console.WriteLine(user);
            }



        }
    }
}
