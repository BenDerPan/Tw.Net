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
            await tw.InitAsync();

            var testUsers = new List<string>(){
                "realDonaldTrump","jack","taylorswift"
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
