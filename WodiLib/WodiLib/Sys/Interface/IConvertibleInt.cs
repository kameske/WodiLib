namespace WodiLib.Sys
{
    /// <summary>
    /// int に変換できることを示すインタフェース
    /// </summary>
    public interface IConvertibleInt
    {
        /// <summary>
        /// int に変換する。
        /// </summary>
        /// <returns>int値</returns>
        int ToInt();
    }
}