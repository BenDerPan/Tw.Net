using System.IO;
using System.Text;
namespace Tw.Net.Core
{

    public class Utils
    {
        public static string FixInvalidFilePath(string originPath)
        {
            StringBuilder builder = new StringBuilder(originPath);
            foreach (char invalidChar in Path.GetInvalidFileNameChars())
            {
                builder.Replace(invalidChar.ToString(), string.Empty);
            }
            return builder.ToString();
        }
    }


}
