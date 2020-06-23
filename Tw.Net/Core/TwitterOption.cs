using System;
using System.Collections.Generic;
using System.Text;

namespace Tw.Net.Core
{
    public class TwitterOption
    {
        public string UserName { get; set; }
        public string UserID { get; set; }
        public string Search { get; set; }
        public string Geo { get; set; }
        public bool Location { get; set; } = false;
        public string Near { get; set; }
        public string Lang { get; set; }
        public string Output { get; set; }
        public string Elasticsearch { get; set; }
        public int Year { get; set; }
        public DateTime? Since { get; set; }
        public DateTime? Until { get; set; }
        public bool Email { get; set; } = false;
        public bool Phone { get; set; } = false;
        public bool Verified { get; set; } = false;
        public bool StoreCsv { get; set; } = false;
        public bool StoreJson { get; set; } = false;
        public bool ShowHashTags { get; set; } = false;
        public bool ShowCashTags { get; set; } = false;
        public int Limit { get; set; }
        public int Count { get; set; }
        public bool Stats { get; set; } = false;
        public string Database { get; set; }
        public string To { get; set; }
        public string All { get; set; }
        public bool Debug { get; set; } = false;
        public string Format { get; set; }
        public string Essid { get; set; }
        public bool Profile { get; set; } = false;
        public bool Followers { get; set; } = false;
        public bool Following { get; set; } = false;
        public bool Favorites { get; set; } = false;
        public bool TwitterSearch { get; set; } = false;
        public bool UserFull { get; set; } = false;
        public bool ProfileFull { get; set; } = false;
        public bool StoreObject { get; set; } = false;
        public string StorObjectTweetsList { get; set; }
        public string StoreObjectUsersList { get; set; }
        public string StoreObjectFollowList { get; set; }
        public string PandasType { get; set; }
        public bool Pandas { get; set; } = false;
        public string IndexTweets { get; set; } = "twinttweets";
        public string IndexFollow { get; set; } = "twintgraph";
        public string IndexUsers { get; set; } = "twintuser";
        public long RetriesCount { get; set; } = 10;
        public string Resume { get; set; }
        public bool Images { get; set; } = false;
        public bool Videos { get; set; } = false;
        public bool Media { get; set; } = false;
        public bool Replies { get; set; } = false;
        public bool PandasClean { get; set; } = false;
        public bool LowerCase { get; set; } = false;
        public bool PandasAu { get; set; } = false;
        public string ProxyHost { get; set; } = "localhost";
        public int ProxyPort { get; set; } = 1080;
        public string ProxyType { get; set; } = "http";
        public int TorControlPort { get; set; } = 9051;
        public string TorControlPassword { get; set; }
        public bool Retweets { get; set; } = false;
        public string Query { get; set; }
        public bool HideOutput { get; set; } = false;
        public string CustomQuery { get; set; } = "";
        public bool PopularTweets { get; set; } = false;
        public bool SkipCerts { get; set; } = false;
        public bool NativeRetweets { get; set; } = false;
        public long MinLikes { get; set; } = 0;
        public long MinRetweets { get; set; } = 0;
        public long MinReplies { get; set; } = 0;
        public string Links { get; set; }
        public string Source { get; set; }
        public string MembersList { get; set; }
        public bool FilterRetweets { get; set; } = false;
        public bool Translate { get; set; } = false;
        public string TranslateSrc { get; set; } = "en";
        public string TranslateDest { get; set; } = "en";
        public double BackoffExponent { get; set; } = 3.0;
        public long MinWaitTime { get; set; } = 0;
    }
}
