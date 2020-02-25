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
    /// Enumerable の拡張クラス
    /// </summary>
    public static class EnumerableExtension
    {
        /// <summary>
        /// null項目があるかどうか判定する。
        /// </summary>
        /// <typeparam name="T">対象リスト内の型</typeparam>
        /// <param name="src">対象</param>
        /// <returns>null項目がある場合、true</returns>
        public static bool HasNullItem<T>(this IEnumerable<T> src)
        {
            return src.Any(x => ReferenceEquals(x, null));
        }

        /// <summary>
        /// 条件を満たす要素のインデックスを取得する。
        /// </summary>
        /// <typeparam name="T">対象リスト内の型</typeparam>
        /// <param name="src">対象</param>
        /// <param name="predicate">条件</param>
        /// <returns>条件に一致する要素が存在する場合、最初の要素のインデックス。存在しない場合-1</returns>
        public static int FindIndex<T>(this IEnumerable<T> src, Func<T, bool> predicate)
        {
            var searchResult = src.Select((x, idx) => (x, idx))
                .FirstOrDefault(x => predicate(x.x));
            return searchResult.Equals(default)
                ? -1
                : searchResult.idx;
        }
    }
}