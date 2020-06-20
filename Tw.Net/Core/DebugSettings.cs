
using System.IO;

namespace Tw.Net.Core
{
    public class DebugSettings
    {
        public static bool IsDebug = false;

        public static void SaveUrlPage(string url, string htmlSource)
        {
            try
            {
                var fileName = $"{Utils.FixInvalidFilePath(url)}.html";
                using (FileStream stream = new FileStream(fileName, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(htmlSource);
                    }
                }
            }
            catch
            {

            }
        }
    }
}