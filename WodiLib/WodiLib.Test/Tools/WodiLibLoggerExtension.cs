using System;
using WodiLib.Sys.Cmn;

namespace WodiLib.Test.Tools
{
    public static class WodiLibLoggerExtension
    {
        /// <summary>
        /// エラー情報を出力する。
        /// </summary>
        /// <param name="logger">ロガー</param>
        /// <param name="ex">[NotNull] 例外</param>
        public static void Exception(this WodiLibLogger logger, Exception ex)
        {
            if (ex == null) throw new ArgumentNullException(nameof(ex));

            logger.Error(ex.Message + Environment.NewLine + ex.StackTrace);
        }
    }
}