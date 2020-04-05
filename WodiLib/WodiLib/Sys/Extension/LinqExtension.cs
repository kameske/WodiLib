// ========================================
// Project Name : WodiLib
// File Name    : LinqExtension.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;

namespace WodiLib.Sys
{
    /// <summary>
    /// Linq拡張クラス
    /// </summary>
    internal static class LinqExtension
    {
        /// <summary>
        /// <see cref="IEnumerable{T}"/>の各要素について何かしらの処理を実行する。
        /// </summary>
        /// <param name="source">処理を呼び出す対象となる値のシーケンス。</param>
        /// <param name="action">各ソース要素に適用する処理。
        /// この関数の 2 つ目のパラメーターは、ソース要素のインデックスを表す。</param>
        /// <typeparam name="TSource"><paramref name="source"/>の要素の型。</typeparam>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/>または<paramref name="action"/>がnullの場合
        /// </exception>
        public static void ForEach<TSource>(
            this IEnumerable<TSource> source,
            Action<TSource> action)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (action == null) throw new ArgumentNullException(nameof(action));

            foreach (var element in source)
            {
                action(element);
            }
        }

        /// <summary>
        /// <see cref="IEnumerable{T}"/>の各要素について何かしらの処理を実行する。
        /// </summary>
        /// <param name="source">処理を呼び出す対象となる値のシーケンス。</param>
        /// <param name="action">各ソース要素に適用する処理。
        /// この関数の 2 つ目のパラメーターは、ソース要素のインデックスを表す。</param>
        /// <typeparam name="TSource"><paramref name="source"/>の要素の型。</typeparam>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/>または<paramref name="action"/>がnullの場合
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
        /// <see cref="IEnumerable{T}"/>の各要素について何かしらの処理を実行し、
        /// 元の値を後続の処理に流す。
        /// </summary>
        /// <param name="source">処理を呼び出す対象となる値のシーケンス。</param>
        /// <param name="action">各ソース要素に適用する処理。
        /// この関数の 2 つ目のパラメーターは、ソース要素のインデックスを表す。</param>
        /// <typeparam name="TSource"><paramref name="source"/>の要素の型。</typeparam>
        /// <returns><paramref name="source"/>の各要素</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/>または<paramref name="action"/>がnullの場合
        /// </exception>
        public static IEnumerable<TSource> Do<TSource>(
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

                yield return element;
            }
        }

        /// <summary>
        /// <see cref="IEnumerable{T}"/>の各要素について何かしらの処理を実行し、
        /// 元の値を後続の処理に流す。
        /// </summary>
        /// <param name="source">処理を呼び出す対象となる値のシーケンス。</param>
        /// <param name="action">各ソース要素に適用する処理。</param>
        /// <typeparam name="TSource"><paramref name="source"/>の要素の型。</typeparam>
        /// <returns><paramref name="source"/>の各要素</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/>または<paramref name="action"/>がnullの場合
        /// </exception>
        public static IEnumerable<TSource> Do<TSource>(
            this IEnumerable<TSource> source,
            Action<TSource> action)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (action == null) throw new ArgumentNullException(nameof(action));

            return source.Do((elem, _) => action(elem));
        }
    }
}