using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;

namespace Tw.Net.Core
{
    public class HtmlLoader
    {
        public static string Agent => FakeAgent.Net.FakeAgent.RandomAgent;

        public static async Task<bool> TryLoadAgentSource(bool useLocal = false)
        {
            var isOk = await FakeAgent.Net.FakeAgent.TryLoadSource(useLocal);
            Console.WriteLine($"Load dynammic agents from onlnie: {isOk}");
            return isOk;
        }

        public static async Task<string> TryLoadPageAsync(string url, RequestProxy proxy = null,bool isXmlHttpRequest=true,bool useLatestAgent=true, int timeoutMilliseconds = 15000)
        {
            HttpClient httpClient;
            if (proxy != null)
            {
                var httpHandler = new HttpClientHandler()
                {
                    Proxy = proxy.GetProxy()
                };
                httpClient = new HttpClient(httpHandler);
            }
            else
            {
                httpClient = new HttpClient();
            }


            httpClient.Timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);
            //OK
            //var agent="Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.106 Safari/537.36";

            //Not OK
            //var agent = "Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/537.14 (KHTML, like Gecko) Chrome/24.0.1292.0 Safari/537.14";

            //OK
            // var agent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.77 Safari/537.36";

            // Error 400
            //var agent = "Mozilla/5.0 (Linux; Android 10) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.106 Mobile Safari/537.36";

            //
            //var agent="Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_5) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.106 Safari/537.36";

            //var agent="Mozilla/5.0 (Windows NT 6.1; Trident/7.0; rv:11.0) like Gecko";

            string agent = string.Empty;
            while (true)
            {
                try
                {
                    if (useLatestAgent)
                    {
                        agent = ValidAgent.RandomAgent;
                    }
                    else
                    {
                        agent = Agent;
                    }
                    //sometimes there is invalid agent type will throw exception
                    httpClient.DefaultRequestHeaders.Add("User-Agent", agent);
                    Console.WriteLine($"Crawler Target Url={url}, User-Agent:{agent}");
                    break;
                }
                catch (Exception)
                {
                    continue;
                }
            }
            if (isXmlHttpRequest)
            {
                httpClient.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
            }
            
            try
            {
                var htmlSource = await httpClient.GetStringAsync(url);
                if (DebugSettings.IsDebug)
                {
                    DebugSettings.SaveUrlPage(url, htmlSource);
                }
                return htmlSource;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static async Task<IHtmlDocument> TryLoadAndParsePageAsync(string url, RequestProxy proxy = null, bool isXmlHttpRequest = true, bool useLatestAgent = true, int timeoutMilliseconds = 15000)
        {
            var htmlSource = await TryLoadPageAsync(url, proxy, isXmlHttpRequest,useLatestAgent,timeoutMilliseconds);
            try
            {
                var parser = new HtmlParser();
                var doc = await parser.ParseDocumentAsync(htmlSource);
                return doc;
            }
            catch (Exception e)
            {
                return null;
            }

        }
    }
}
