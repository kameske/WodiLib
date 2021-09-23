// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : DictionaryExtension.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using System.Linq;

namespace WodiLib.SourceGenerator.Core.Extensions
{
    /// <summary>
    ///     ディクショナリ型（連想配列型）拡張クラス
    /// </summary>
    internal static class DictionaryExtension
    {
        /// <summary>
        ///     すべての Key-Value を文字列にして返す。
        /// </summary>
        /// <param name="src">処理対象</param>
        /// <typeparam name="TKey">キー型</typeparam>
        /// <typeparam name="TValue">値型</typeparam>
        /// <returns>処理結果</returns>
        public static string Description<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> src)
            => string.Join(", ", src.Select(pair => $"{pair.Key}: {pair.Value}"));
    }
}
