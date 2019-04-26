// ========================================
// Project Name : WodiLib
// File Name    : EnumerableExtension.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

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
        public static bool HasNullItem<T>(this IEnumerable<T> src)
        {
            return src.Any(x => x == null);
        }
    }
}