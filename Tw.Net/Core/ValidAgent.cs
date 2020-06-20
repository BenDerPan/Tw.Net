using System;
using System.Collections.Generic;
using FakeAgent.Net.Model;

namespace Tw.Net.Core
{
    public class ValidAgent
    {
        static readonly Random _random = new Random();
        public static readonly Agent Source = new Agent();
        static ValidAgent()
        {
            //Source: From https://www.whatismybrowser.com/guides/the-latest-user-agent/chrome
            Source.Browsers.Add("chrome", new List<string>(){
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.106 Safari/537.36",
                "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.106 Safari/537.36",
                "Mozilla/5.0 (Windows NT 10.0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.106 Safari/537.36",
                "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_5) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.106 Safari/537.36",
                "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.106 Safari/537.36",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
            });
            Source.Browsers.Add("firefox", new List<string>(){
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
            });
            Source.Browsers.Add("safari", new List<string>(){
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
            });
            Source.Browsers.Add("edge", new List<string>(){
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
            });
            Source.Browsers.Add("ie", new List<string>(){
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
            });
            Source.Browsers.Add("opera", new List<string>(){
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
            });

            foreach (var browser in Source.Browsers.Keys)
            {
                var startIndexKey = Source.Randomize.Count;

                for (int i = startIndexKey; i < startIndexKey + Source.Browsers[browser].Count; i++)
                {
                    Source.Randomize.Add($"{i}", browser);
                }
            }
        }


        /// <summary>
        /// get a random agent
        /// </summary>
        public static string RandomAgent
        {
            get
            {
                var browserKey = _random.Next(0, Source.Randomize.Count).ToString();
                var browser = Source.Randomize[browserKey];
                var agentIndex = _random.Next(0, Source.Browsers[browser].Count);
                return Source.Browsers[browser][agentIndex];
            }
        }

    }
}