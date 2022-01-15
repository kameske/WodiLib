// ========================================
// Project Name : WodiLib
// File Name    : ListValidationHelper.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     リストバリデーションヘルパークラス
    /// </summary>
    internal static class ListValidationHelper
    {
        /// <summary>
        ///     リスト操作時のIndex値を検証する。
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <param name="listCount">リスト要素数</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="index"/> が 0 未満 または <paramref name="listCount"/> 以上の場合。
        /// </exception>
        public static void SelectIndex(NamedValue<int> index, NamedValue<int> listCount)
        {
            var max = Math.Max(listCount.Value - 1, 0);
            const int min = 0;
            ThrowHelper.ValidateArgumentValueRange(
                index.Value < min || max < index.Value,
                index.Name,
                index.Value,
                min,
                max
            );
        }

        /// <summary>
        ///     リスト操作時のIndex値を検証する。
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <param name="listCount">リスト要素数</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="index"/> が 0 未満 または <paramref name="listCount"/> を超える場合。
        /// </exception>
        public static void InsertIndex(NamedValue<int> index, NamedValue<int> listCount)
        {
            var max = listCount.Value;
            const int min = 0;
            ThrowHelper.ValidateArgumentValueRange(
                index.Value < min || max < index.Value,
                index.Name,
                index.Value,
                min,
                max
            );
        }

        /// <summary>
        ///     リスト操作時のIndex値を検証する。
        /// </summary>
        /// <param name="count">要素数</param>
        /// <param name="listCount">リスト要素数</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="count"/> が 0 未満 または <paramref name="listCount"/> を超える場合
        /// </exception>
        public static void Count(NamedValue<int> count, NamedValue<int> listCount)
        {
            var max = listCount.Value;
            const int min = 0;
            ThrowHelper.ValidateArgumentValueRange(
                count.Value < min || max < count.Value,
                count.Name,
                count.Value,
                min,
                max
            );
        }

        /// <summary>
        ///     範囲操作時のパラメータを検証する。
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <param name="count">範囲数</param>
        /// <param name="listCount">リスト要素数</param>
        /// <exception cref="ArgumentException">
        ///     <paramref name="index"/> と <paramref name="count"/> の整合性が取れない場合。
        /// </exception>
        public static void Range(NamedValue<int> index, NamedValue<int> count, NamedValue<int> listCount)
        {
            ThrowHelper.ValidateListRange(
                listCount.Value - index.Value < count.Value,
                index.Name,
                count.Name
            );
        }

        /// <summary>
        ///     要素列挙に <see langword="null"/> 要素が含まれていないことを検証する。
        /// </summary>
        /// <param name="items">要素列挙</param>
        /// <typeparam name="T">検証型</typeparam>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="items"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        public static void ItemsHasNotNull<T>(NamedValue<IEnumerable<T>> items)
        {
            ThrowHelper.ValidateArgumentItemsHasNotNull(items.Value.HasNullItem(), items.Name);
        }

        /// <summary>
        ///     リストの要素数が0でないことを検証する。
        /// </summary>
        /// <param name="listCount">要素数</param>
        /// <exception cref="InvalidOperationException">
        ///     <paramref name="listCount"/> が 0 の場合
        /// </exception>
        public static void ItemCountNotZero(NamedValue<int> listCount)
        {
            ThrowHelper.ValidateListItemCountNotZero(listCount.Value == 0, listCount.Name);
        }
    }
}
