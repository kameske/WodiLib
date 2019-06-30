using System.IO;

namespace WodiLib.Test.Tools
{
    static class StringExtension
    {
        /// <summary>
        /// パス文字列中のディレクトリが存在しない場合、作成する
        /// </summary>
        /// <param name="dir">パス文字列</param>
        public static void CreateDirectoryIfNeed(this string dir)
        {
            if (dir != null && !Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }
    }
}