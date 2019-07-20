namespace WodiLib.Sys
{
    internal static class BoolExtension
    {
        /// <summary>
        /// int値に変換した結果を文字列にして返す。
        /// </summary>
        /// <param name="src">対象</param>
        /// <returns>trueの場合"1"、falseの場合"0"</returns>
        public static string ToIntString(this bool src)
        {
            return (src ? 1 : 0).ToString();
        }
    }
}