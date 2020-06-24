using AngleSharp.Dom;
using System;
using System.IO;
using System.Text;
namespace Tw.Net.Core
{

    public class Utils
    {
        public static string FixInvalidFilePath(string originPath,string replace="_")
        {
            StringBuilder builder = new StringBuilder(originPath);
            foreach (char invalidChar in Path.GetInvalidFileNameChars())
            {
                builder.Replace(invalidChar.ToString(), replace);
            }
            //windows path can't contains ":", but linux can do that.
            builder.Replace(":", replace);
            return builder.ToString();
        }

        #region DateTime & TimeStamp
        static readonly DateTime StartDateTime = TimeZoneInfo.ConvertTime(new DateTime(1970,1,1,0,0,0),TimeZoneInfo.Local);

        /// <summary>
        /// 时间戳转为C#格式时间
        /// </summary>
        /// <param name="timeStamp">13位整数型timestamp</param>
        /// <returns></returns>
        public static DateTime GetDateTimeFromUnixTimestamp(long timeStamp)
        {
            try
            {
                TimeSpan toNow = TimeSpan.FromMilliseconds(timeStamp);
                return StartDateTime.Add(toNow);
            }
            catch
            {
                return DateTime.Now;
            }
        }

        /// <summary>
        /// 时间戳转为C#格式时间
        /// </summary>
        /// <param name="timeStamp">13位整数型timestamp</param>
        /// <returns></returns>
        public static DateTime GetDateTimeFromUnixTimestampString(string timeStamp)
        {
            if (string.IsNullOrEmpty(timeStamp)) { return DateTime.Now; }
            try
            {
                long lTime = long.Parse(timeStamp);
                TimeSpan toNow =TimeSpan.FromMilliseconds(lTime);
                return StartDateTime.Add(toNow);
            }
            catch
            {
                return DateTime.Now;
            }
        }


        /// <summary>
        /// DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name=”time”></param>
        /// <returns></returns>
        public static long ConvertDateTimeToUnixTimestamp(DateTime time)
        {
            System.DateTime startTime = StartDateTime;
            return (long)(time - startTime).TotalMilliseconds;
        }
        #endregion


        public static bool TryGetDomAttributeAsLong(IElement element,string attributeName,out long value)
        {
            value =0;
            var item = element.GetAttribute(attributeName);
            if (item!=null)
            {
                if (long.TryParse(item,out value))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool TryGetDomAttributeAsString(IElement element, string attributeName, out string value)
        {
            value = null;
            if (element==null)
            {
                return false;
            }
            var item = element.GetAttribute(attributeName);
            if (item != null)
            {
                value = item;
                return true;
            }

            return false;
        }

        public static bool TryGetDomAttributeAsBool(IElement element, string attributeName, out bool value)
        {
            value = false;
            if (element == null)
            {
                return false;
            }
            var item = element.GetAttribute(attributeName);
            if (item != null)
            {
                if (bool.TryParse(item, out value))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool TryGetDomAttributeAsInt(IElement element, string attributeName, out int value)
        {
            value = 0;
            if (element == null)
            {
                return false;
            }
            var item = element.GetAttribute(attributeName);
            if (item != null)
            {
                if (int.TryParse(item, out value))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool TryGetDomTextContent(IElement element,string selector,out string textContent,bool shouldTrim = true)
        {
            textContent = null;
            var elemItem = element.QuerySelector(selector);
            if (elemItem!=null)
            {
                textContent = elemItem.TextContent;
                if (shouldTrim)
                {
                    textContent = textContent.Trim();
                }
                return true;
            }
            return false;
            
        }

    }


}
