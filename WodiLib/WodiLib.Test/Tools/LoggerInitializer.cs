using System;
using Commons;

namespace WodiLib.Test.Tools
{
    public static class LoggerInitializer
    {
        public static readonly string KeyNameForDebug = "forDebug";
        public static readonly string KeyNameForProjectTest = "forProjectTest";

        /// <summary>
        /// デバッグ用Loggerのセットを行う
        /// </summary>
        public static void SetupLoggerForDebug()
        {
            var logHandler = new LogHandler(
                Console.WriteLine,
                Console.WriteLine,
                Console.WriteLine,
                Console.WriteLine,
                ExceptionAction
            );

            Logger.ChangeTargetKey(KeyNameForDebug);
            Logger.SetLogHandler(logHandler);
        }

        /// <summary>
        /// Project テスト用のロガーインスタンスを生成する。
        /// </summary>
        /// <returns></returns>
        public static void SetupLoggerForProjectTest()
        {
            var logHandler = new LogHandler(
                Console.WriteLine,
                Console.WriteLine,
                exceptionAction: ExceptionAction
            );

            Logger.ChangeTargetKey(KeyNameForProjectTest);
            Logger.SetLogHandler(logHandler);
        }

        /// <summary>
        /// エラー情報を出力する。
        /// </summary>
        /// <param name="ex">例外</param>
        private static void ExceptionAction(Exception ex)
        {
            if (ex == null) throw new ArgumentNullException(nameof(ex));

            Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
        }
    }
}