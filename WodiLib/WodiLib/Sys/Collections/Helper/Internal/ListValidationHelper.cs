// ========================================
// Project Name : WodiLib
// File Name    : ListValidationHelper.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;

namespace WodiLib.Sys
{
    /// <summary>
    /// リストバリデーションヘルパークラス
    /// </summary>
    internal static class ListValidationHelper
    {
        /// <summary>
        /// リスト操作時のIndex値を検証する。
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <param name="listCount">リスト要素数</param>
        /// <param name="itemName">エラーメッセージ中の項目名</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///    <paramref name="index"/> が 0 未満 または <paramref name="listCount"/> - 1 以上の場合。
        /// </exception>
        public static void SelectIndex(int index, int listCount, string itemName = "index")
        {
            var max = listCount - 1;
            const int min = 0;
            ThrowHelper.ValidateArgumentValueRange(index < min || max < index,
                itemName, index, min, max);
        }

        /// <summary>
        /// リスト操作時のIndex値を検証する。
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <param name="listCount">リスト要素数</param>
        /// <param name="itemName">エラーメッセージ中の項目名</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///    <paramref name="index"/> が 0 未満 または <paramref name="listCount"/> を超える場合。
        /// </exception>
        public static void InsertIndex(int index, int listCount, string itemName = "index")
        {
            var max = listCount;
            const int min = 0;
            ThrowHelper.ValidateArgumentValueRange(index < min || max < index,
                itemName, index, min, max);
        }

        /// <summary>
        /// リスト操作時のIndex値を検証する。
        /// </summary>
        /// <param name="count">要素数</param>
        /// <param name="listCount">リスト要素数</param>
        /// <param name="itemName">エラーメッセージ中の項目名</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///    count が 0 未満 または listCount を超える場合
        /// </exception>
        public static void Count(int count, int listCount,
            string itemName = "count")
        {
            const int min = 0;
            ThrowHelper.ValidateArgumentValueRange(count < min || listCount < count,
                itemName, count, min, listCount);
        }

        /// <summary>
        /// 範囲操作時のパラメータを検証する。
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <param name="count">範囲数</param>
        /// <param name="listCount">リスト要素数</param>
        /// <param name="indexArgName">エラーメッセージ中のインデックス引数名</param>
        /// <param name="countItemName">エラーメッセージ中の取得数引数名</param>
        /// <exception cref="ArgumentException">
        ///     <paramref name="index"/> と <paramref name="count"/> の整合性が取れない場合。
        /// </exception>
        public static void Range(int index, int count, int listCount,
            string indexArgName = "index", string countItemName = "count")
        {
            ThrowHelper.ValidateListRange(listCount - index < count,
                indexArgName, countItemName);
        }

        /// <summary>
        /// 要素列挙に <see langword="null"/> 要素が含まれていないことを検証する。
        /// </summary>
        /// <param name="items">要素列挙</param>
        /// <param name="itemName">エラーメッセージ中の項目名</param>
        /// <typeparam name="T">検証型</typeparam>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="items"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        public static void ItemsHasNotNull<T>(IEnumerable<T> items, string itemName = "items")
        {
            ThrowHelper.ValidateArgumentItemsHasNotNull(items.HasNullItem(), itemName);
        }

        /// <summary>
        /// リストの要素数が0でないことを検証する。
        /// </summary>
        /// <param name="listCount">要素数</param>
        /// <exception cref="InvalidOperationException">
        ///     要素数が0の場合
        /// </exception>
        public static void ItemCountNotZero(int listCount)
        {
            ThrowHelper.ValidateListItemCountNotZero(listCount == 0, "リスト");
        }
    }
}
