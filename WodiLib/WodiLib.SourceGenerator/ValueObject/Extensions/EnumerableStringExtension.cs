// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : EnumerableStringExtension.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using System.Linq;

namespace WodiLib.SourceGenerator.ValueObject.Extensions
{
    /// <summary>
    ///     IEnumerable{string} 拡張クラス
    /// </summary>
    internal static class EnumerableStringExtension
    {
        /// <summary>
        ///     空文字、<see langword="null"/> を除外して結合する。
        /// </summary>
        /// <param name="src">結合対象</param>
        /// <param name="separator">結合文字列</param>
        /// <returns>処理結果</returns>
        public static string JoinWithoutEmpty(this IEnumerable<string?> src, string separator)
            => string.Join(separator, src.Where(s => s is not null && !s.Equals("")));
    }
}
