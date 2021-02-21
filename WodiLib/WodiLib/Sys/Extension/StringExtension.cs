// ========================================
// Project Name : WodiLib
// File Name    : StringExtension.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace WodiLib.Sys
{
    /// <summary>
    ///     string拡張クラス
    /// </summary>
    internal static class StringExtension
    {
        /// <summary>
        ///     空文字かどうかを返す。
        /// </summary>
        /// <param name="src">対象</param>
        /// <returns>空文字の場合、<see langword="true"/></returns>
        public static bool IsEmpty(this string src)
        {
            return src.Equals(string.Empty);
        }

        /// <summary>
        ///     改行を含むかどうかを返す。
        /// </summary>
        /// <param name="src">対象</param>
        /// <returns>改行を含む場合、<see langword="true"/></returns>
        public static bool HasNewLine(this string src)
        {
            return src.Contains("\n") || src.Contains("\r\n");
        }

        /// <summary>
        ///     ファイル名に使用不可能な文字列を含むかどうかを返す。
        /// </summary>
        /// <param name="src">対象</param>
        /// <returns>使用不可能な文字列を含む場合、<see langword="true"/></returns>
        public static bool HasInvalidFileNameChars(this string src)
        {
            // "COM0", "LPT0" は作成可能

            var regex = new Regex(
                "[\\x00-\\x1f<>:\"/\\\\|?*]|^(CON|COM[1-9]|NUL|PRN|AUX|LPT[1-9])(\\.|$)|[\\. ]$",
                RegexOptions.IgnoreCase
            );

            return regex.IsMatch(src);
        }

        /// <summary>
        ///     文字列をintに変換する。
        /// </summary>
        /// <param name="src">対象</param>
        /// <returns>変換したint値。変換に失敗した場合、<see langword="null"/></returns>
        public static int? TryToInt(this string src)
        {
            var result = int.TryParse(src, out var numeric);

            if (!result) return null;

            return numeric;
        }

        /// <summary>
        ///     ウディタ文字列のbyte配列に変換する。
        /// </summary>
        /// <returns>ウディタ文字列のbyte配列</returns>
        public static IEnumerable<byte> ToWoditorStringBytes(this string src)
        {
            var woditorStr = new WoditorString(src);
            return woditorStr.StringByte;
        }
    }
}
