using System;
using System.Net;

namespace Tw.Net.Core
{
    public class RequestProxy
    {
        public string Host { get; set; }

        public int Port { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public RequestProxy(string host = "127.0.0.1", int port = 1080, string username = "", string password = "")
        {
            Host = host;
            Port = port;
            UserName = username;
            Password = password;
        }

        public WebProxy GetProxy()
        {
            var proxy = new WebProxy($"http://{Host}:{Port}");
            if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password))
            {
                //Basic Auth
                proxy.Credentials = new NetworkCredential(UserName, Password);
            }
            return proxy;
        }
    }
}