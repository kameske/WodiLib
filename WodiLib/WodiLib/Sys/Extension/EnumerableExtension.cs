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
        ///     二重シーケンスを二次元配列に変換する。
        /// </summary>
        /// <param name="src">対象</param>
        /// <typeparam name="T">対象シーケンスの内包型</typeparam>
        /// <returns>二次元配列</returns>
        internal static T[][] ToTwoDimensionalArray<T>(this IEnumerable<IEnumerable<T>> src)
        {
            return src.Select(line => line.ToArray()).ToArray();
        }

        /// <summary>
        ///     行列を入れ替えた二次元配列を返す。<br/>
        ///     【事前条件】<br/>
        ///     すべての行について要素数が一致すること
        /// </summary>
        /// <param name="src">対象</param>
        /// <returns><paramref name="src"/> の転置行列</returns>
        public static T[][] ToTransposedArray<T>(this IEnumerable<IEnumerable<T>> src)
        {
            return src.ToTwoDimensionalArray()
                .ToTransposedArray();
        }
    }
}
