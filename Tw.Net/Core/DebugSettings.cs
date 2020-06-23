
using System;
using System.IO;

namespace Tw.Net.Core
{
    public class DebugSettings
    {
        public enum LoggingLevel
        {
            Debug,
            Info,
            Warning,
            Error,
        }
        public static readonly string DebugDataDir = Path.GetFullPath("./DebugOutput/");
        public static bool IsDebug = false;
        public static void CheckDebugDataDirExists()
        {
            if (!Directory.Exists(DebugDataDir))
            {
                Directory.CreateDirectory(DebugDataDir);
            }
        }
        public static void SaveUrlPage(string url, string htmlSource)
        {
            try
            {
                CheckDebugDataDirExists();
                var fileName = Path.Combine(DebugDataDir, $"{Utils.FixInvalidFilePath(url)}.html");
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

        public static void Log(LoggingLevel level, string title, string message)
        {
            var timeStr = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var msg = $"[{timeStr} [{level}] <{title}>  {message}";
            Console.WriteLine(msg);
        }

        public static void LogDebug(string title, string message)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Log(LoggingLevel.Debug, title, message);
        }
        public static void LogInfo(string title, string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Log(LoggingLevel.Info, title, message);
        }
        public static void LogWarning(string title, string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Log(LoggingLevel.Warning, title, message);
        }
        public static void LogError(string title, string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Log(LoggingLevel.Error, title, message);
        }
    }
}