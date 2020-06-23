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


    }


}
