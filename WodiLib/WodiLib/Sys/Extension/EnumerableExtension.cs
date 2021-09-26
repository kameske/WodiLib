// ========================================
// Project Name : WodiLib
// File Name    : EnumerableExtension.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Linq;

namespace WodiLib.Sys
{
    /// <summary>
    ///     Enumerable の拡張クラス
    /// </summary>
    internal static class EnumerableExtension
    {
        /// <summary>
        ///     <see langword="null"/>項目があるかどうか判定する。
        /// </summary>
        /// <typeparam name="T">対象リスト内の型</typeparam>
        /// <param name="src">対象</param>
        /// <returns><see langword="null"/> 項目がある場合、<see langword="true"/></returns>
        public static bool HasNullItem<T>(this IEnumerable<T> src)
        {
            return src.Any(x => x is null);
        }

        /// <summary>
        ///     条件を満たす要素のインデックスを取得する。
        /// </summary>
        /// <typeparam name="T">対象リスト内の型</typeparam>
        /// <param name="src">対象</param>
        /// <param name="predicate">条件</param>
        /// <returns>
        ///     条件に一致する要素が存在する場合、最初の要素のインデックス。
        ///     存在しない場合、-1。
        /// </returns>
        public static int FindIndex<T>(this IEnumerable<T> src, Func<T, bool> predicate)
        {
            var searchResult = src.Select((x, idx) => (x, idx))
                .FirstOrDefault(x => predicate(x.x));
            return searchResult.Equals(default)
                ? -1
                : searchResult.idx;
        }

        /// <summary>
        ///     指定した要素のインデックスを取得する。
        /// </summary>
        /// <typeparam name="T">対象リスト内の型</typeparam>
        /// <param name="src">対象</param>
        /// <param name="searchItem">探索要素</param>
        /// <param name="comparer">比較処理実装</param>
        /// <returns>
        ///     一致する要素が存在する場合、最初の要素のインデックス。
        ///     存在しない場合、-1。
        /// </returns>
        public static int FindIndex<T>(this IEnumerable<T> src, T searchItem, IEqualityComparer<T>? comparer)
            => src.FindIndex(elem => comparer?.Equals(elem, searchItem) ?? elem?.Equals(searchItem) ?? false);

        /// <summary>
        ///     インデックス番号をキーとした<see cref="IReadOnlyDictionary{TKey,TValue}"/>に変換する。
        /// </summary>
        /// <param name="src">変換対象</param>
        /// <typeparam name="T">対象リスト内の要素型</typeparam>
        /// <returns>変換結果</returns>
        public static IReadOnlyDictionary<int, T> ToDict<T>(this IEnumerable<T> src)
        {
            var result = new Dictionary<int, T>();
            src.ForEach((item, idx) => result[idx] = item);
            return result;
        }
    }
}
