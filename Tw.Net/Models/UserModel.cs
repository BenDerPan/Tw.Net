using System;
using Newtonsoft.Json;

namespace Tw.Net.Models
{
    public class UserModel
    {
        /// <summary>
        /// 账号id(唯一)
        /// </summary>
        public long ID { get; set; } = 0;
        /// <summary>
        /// 账号名字/昵称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 账号用户名（唯一）
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 账号简介
        /// </summary>
        public string Bio { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// 账号主页URL
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 加入Twitter日期，创建账号日期
        /// </summary>
        public string JoinDate { get; set; }

        /// <summary>
        /// 加入Twitter时间，创建账号时间
        /// </summary>
        public string JoinTime { get; set; }

        /// <summary>
        /// 推文数量
        /// </summary>
        public int Tweets { get; set; }

        /// <summary>
        /// 关注的账号数
        /// </summary>
        public int Following { get; set; }

        /// <summary>
        /// 被关注的账号人数
        /// </summary>
        public int Followers { get; set; }

        /// <summary>
        /// 被点赞数
        /// </summary>
        public int Likes { get; set; }

        /// <summary>
        /// 发布过的媒体数量，包括图片，外部链接，视频等
        /// </summary>
        public int Media { get; set; }

        /// <summary>
        /// 是否受保护
        /// </summary>
        public int Private { get; set; }

        /// <summary>
        /// 是否认证过
        /// </summary>
        public int Verified { get; set; }

        /// <summary>
        /// 头像，md5格式
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// 背景图片，md5格式
        /// </summary>
        public string BackgroundImage { get; set; }

        /// <summary>
        /// 爬取时间
        /// </summary>
        public string SpiderTime { get; set; }

        public bool IsValid()
        {
            return ID > 0;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

    }

}
