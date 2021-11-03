// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : LinqExtension.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Linq;

namespace WodiLib.SourceGenerator.Core.Extensions
{
    /// <summary>
    /// <see cref="IEnumerable{T}"/> 拡張クラス
    /// </summary>
    internal static class EnumerableExtension
    {
        /// <summary>
        /// 条件を満たす最初の要素のインデックスを取得する。
        /// </summary>
        /// <param name="src">検索対象コレクション</param>
        /// <param name="predicate">検索条件</param>
        /// <typeparam name="T">コレクションの要素型</typeparam>
        /// <returns>条件を最初に満たす要素のインデックス。存在しない場合 -1。</returns>
        public static int IndexOf<T>(this IEnumerable<T> src, Func<T, bool> predicate)
            => src.Select<T, int?>((item, index) => predicate(item) ? index : null)
                .FirstOrDefault(idx => idx is not null).GetValueOrDefault(-1);

        /// <summary>
        /// コレクションの末尾が省略された新たなコレクションを返す。
        /// </summary>
        /// <param name="src">対象</param>
        /// <param name="count">省略数</param>
        /// <typeparam name="T">コレクション要素型</typeparam>
        /// <returns>指定した数の要素を末尾から省略したコレクション</returns>
        public static IEnumerable<T> SkipLast<T>(this IEnumerable<T> src, int count)
            => src.Reverse().Skip(count).Reverse();
    }
}
