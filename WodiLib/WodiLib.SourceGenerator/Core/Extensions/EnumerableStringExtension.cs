// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : EnumerableStringExtension.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;

namespace WodiLib.SourceGenerator.Core.Extensions
{
    /// <summary>
    ///     IEnumerable{string} 拡張クラス
    /// </summary>
    internal static class EnumerableStringExtension
    {
        /// <summary>
        ///     指定した文字列で結合する。
        /// </summary>
        /// <param name="src">結合対象</param>
        /// <param name="separator">結合文字</param>
        /// <returns>処理結果</returns>
        public static string Join(this IEnumerable<string> src, string separator)
            => string.Join(separator, src);
    }
}
