using System;
using WodiLib.Sys.Cmn;

namespace WodiLib.Test.Tools
{
    public static class LoggerInitializer
    {
        /// <summary>
        /// デバッグ用WodiLibLoggerのセットを行う
        /// </summary>
        public static void SetupWodiLibLoggerForDebug()
        {
            var logHandler = new WodiLibLogHandler(
                Console.WriteLine,
                Console.WriteLine,
                Console.WriteLine,
                Console.WriteLine
            );

            WodiLibLogger.ChangeTargetKey("forDebug");
            WodiLibLogger.SetLogHandler(logHandler);
        }

        /// <summary>
        /// Project テスト用のロガーインスタンスを生成する。
        /// </summary>
        /// <returns></returns>
        public static void SetupWodiLibLoggerForProjectTest()
        {
            var logHandler = new WodiLibLogHandler(
                Console.WriteLine,
                Console.WriteLine
            );

            WodiLibLogger.ChangeTargetKey("forProjectTest");
            WodiLibLogger.SetLogHandler(logHandler);
        }
    }
}