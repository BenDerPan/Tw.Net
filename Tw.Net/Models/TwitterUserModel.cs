using System;
using Newtonsoft.Json;

namespace Tw.Net.Models
{
    public class TwitterUserModel
    {
        /// <summary>
        /// 账号id(唯一)
        /// </summary>
        public long ID { get; set; } = 0;
        /// <summary>
        /// 账号名字/昵称
        /// </summary>
        public string Name { get; set; } = "";
        /// <summary>
        /// 账号用户名（唯一）
        /// </summary>
        public string UserName { get; set; } = "";
        /// <summary>
        /// 账号简介
        /// </summary>
        public string Bio { get; set; } = "";
        /// <summary>
        /// 地址
        /// </summary>
        public string Location { get; set; } = "";
        /// <summary>
        /// 账号主页URL
        /// </summary>
        public string Url { get; set; } = "";

        /// <summary>
        /// 加入Twitter日期，创建账号日期
        /// </summary>
        public string JoinDate { get; set; } = "";

        /// <summary>
        /// 加入Twitter时间，创建账号时间
        /// </summary>
        public string JoinTime { get; set; } = "";

        /// <summary>
        /// 推文数量
        /// </summary>
        public long Tweets { get; set; } = 0;

        /// <summary>
        /// 关注的账号数
        /// </summary>
        public long Following { get; set; } = 0;

        /// <summary>
        /// 被关注的账号人数
        /// </summary>
        public long Followers { get; set; } = 0;

        /// <summary>
        /// 被点赞数
        /// </summary>
        public long Likes { get; set; } = 0;

        /// <summary>
        /// 发布过的媒体数量，包括图片，外部链接，视频等
        /// </summary>
        public long Media { get; set; } = 0;

        /// <summary>
        /// 是否受保护
        /// </summary>
        public int Private { get; set; } = 0;

        /// <summary>
        /// 是否认证过
        /// </summary>
        public int Verified { get; set; } = 0;

        /// <summary>
        /// 头像，md5格式
        /// </summary>
        public string Avatar { get; set; } = "";

        /// <summary>
        /// 背景图片，md5格式
        /// </summary>
        public string BackgroundImage { get; set; } = "";

        /// <summary>
        /// 爬取时间
        /// </summary>
        public string SpiderTime { get; set; }= "";

        public bool IsValid()
        {
            return ID > 0;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static TwitterUserModel Parse(string json)
        {
            return JsonConvert.DeserializeObject<TwitterUserModel>(json);
        }

        public static bool TryParse(string json, out TwitterUserModel user)
        {
            user = null;
            try
            {
                user= JsonConvert.DeserializeObject<TwitterUserModel>(json);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }

}
