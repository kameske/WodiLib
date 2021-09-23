// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : StringExtension.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.SourceGenerator.ValueObject.Extensions
{
    /// <summary>
    ///     <see cref="string"/> 拡張クラス
    /// </summary>
    internal static class StringExtension
    {
        /// <summary>
        ///     最初の文字をLowerCaseに変換する。
        /// </summary>
        /// <param name="src">処理対象</param>
        /// <returns>処理結果</returns>
        public static string ToLowerFirstChar(this string src)
        {
            if (src.Equals(""))
            {
                return "";
            }

            var firstLetter = src.Substring(0, 1).ToLower();

            return src.Length == 1
                ? firstLetter
                : $"{firstLetter}{src.Substring(1)}";
        }
    }
}
