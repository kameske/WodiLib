// ========================================
// Project Name : WodiLib
// File Name    : LinqExtension.cs
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
    ///     Linq拡張クラス
    /// </summary>
    internal static class LinqExtension
    {
        /// <summary>
        ///     <see cref="IEnumerable{T}"/> の各要素について何かしらの処理を実行する。
        /// </summary>
        /// <param name="source">処理を呼び出す対象となる値のシーケンス。</param>
        /// <param name="action">
        ///     各ソース要素に適用する処理。
        ///     この関数の 2 つ目のパラメーターは、ソース要素のインデックスを表す。
        /// </param>
        /// <typeparam name="TSource"><paramref name="source"/> の要素の型。</typeparam>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="source"/> または <paramref name="action"/> が <see langword="null"/> の場合。
        /// </exception>
        public static void ForEach<TSource>(
            this IEnumerable<TSource> source,
            Action<TSource, int> action)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (action == null) throw new ArgumentNullException(nameof(action));

            var index = -1;
            foreach (var element in source)
            {
                checked
                {
                    index++;
                }

                action(element, index);
            }
        }

        /// <summary>
        ///     <see cref="IEnumerable{T}"/>の各要素について何かしらの処理を実行する。
        /// </summary>
        /// <param name="source">処理を呼び出す対象となる値のシーケンス。</param>
        /// <param name="action">各ソース要素に適用する処理。</param>
        /// <typeparam name="TSource"><paramref name="source"/>の要素の型。</typeparam>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="source"/> または <paramref name="action"/> が <see langword="null"/> の場合。
        /// </exception>
        public static void ForEach<TSource>(
            this IEnumerable<TSource> source,
            Action<TSource> action)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (action == null) throw new ArgumentNullException(nameof(action));

            source.ForEach((elem, _) => action(elem));
        }

        /// <summary>
        ///     シーケンスの任意の要素が条件を満たすかどうかを決定する。
        /// </summary>
        /// <param name="source">術後を適用する要素</param>
        /// <param name="predicate">各要素が条件を満たしているかどうかをテストする関数</param>
        /// <typeparam name="TSource">要素の型</typeparam>
        /// <returns>
        ///     いずれかの要素のテスト結果が <see langword="true"/> の場合 <see langword="true"/>、
        ///     それ以外の場合 <see langword="false"/>
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="source"/>, <paramref name="predicate"/> が <see langword="null"/> の場合。
        /// </exception>
        public static bool Any<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, int, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            var idx = -1;
            foreach (var elem in source)
            {
                idx++;
                if (predicate(elem, idx)) return true;
            }

            return false;
        }

        /// <summary>
        ///     2つのシーケンスの要素を組み合わせた一つのValueTupleシーケンスを生成する。
        /// </summary>
        /// <param name="src">組み合わせ元1</param>
        /// <param name="other">組み合わせ元2</param>
        /// <typeparam name="TFirst"><paramref name="src"/> の要素型</typeparam>
        /// <typeparam name="TSecond"><paramref name="other"/> の要素型</typeparam>
        /// <returns></returns>
        public static IEnumerable<(TFirst, TSecond)> Zip<TFirst, TSecond>(this IEnumerable<TFirst> src,
            IEnumerable<TSecond> other)
        {
            return src.Zip<TFirst, TSecond, (TFirst, TSecond)>(other, (item1, item2) => (item1, item2));
        }

        /// <summary>
        ///     シーケンスの指定した範囲を切り出す。
        /// </summary>
        /// <param name="src">対象シーケンス</param>
        /// <param name="index">取得開始インデックス</param>
        /// <param name="count">取得数</param>
        /// <typeparam name="T">要素の型</typeparam>
        /// <returns>指定した範囲を列挙するシーケンス</returns>
        public static IEnumerable<T> Range<T>(this IEnumerable<T> src, int index, int count)
        {
            return src.Skip(index).Take(count);
        }
    }
}
