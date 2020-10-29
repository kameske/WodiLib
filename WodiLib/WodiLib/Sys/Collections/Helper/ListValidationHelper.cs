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
        /// 最大・最小容量設定を検証する。
        /// </summary>
        /// <param name="min">最小容量</param>
        /// <param name="max">最大容量</param>
        public static void CapacityConfig(int min, int max)
        {
            if (min < 0)
                throw new InvalidOperationException(
                    ErrorMessage.GreaterOrEqual("最小容量", 0, max));

            if (min > max)
                throw new InvalidOperationException(
                    ErrorMessage.GreaterOrEqual("最大容量", $"最小容量（{min}）", max));
        }

        /// <summary>
        /// リスト操作時のIndex値を検証する。
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <param name="listCount">リスト要素数</param>
        /// <param name="itemName">エラーメッセージ中の項目名</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///    index が 0 未満 または Count - 1 以上の場合
        /// </exception>
        public static void SelectIndex(int index, int listCount, string itemName = "index")
        {
            var max = listCount - 1;
            const int min = 0;
            if (index < min || max < index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(itemName, min, max, index));
        }

        /// <summary>
        /// リスト操作時のIndex値を検証する。
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <param name="listCount">リスト要素数</param>
        /// <param name="itemName">エラーメッセージ中の項目名</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///    index が 0 未満 または listCount を超える場合
        /// </exception>
        public static void InsertIndex(int index, int listCount, string itemName = "index")
        {
            var max = listCount;
            const int min = 0;
            if (index < min || max < index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(itemName, min, max, index));
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
            if (count < 0 || listCount < count)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(itemName, min, listCount, count));
        }

        /// <summary>
        /// 範囲操作時のパラメータを検証する。
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <param name="count">範囲数</param>
        /// <param name="listCount">リスト要素数</param>
        /// <param name="indexItemName">エラーメッセージ中のindex項目名</param>
        /// <param name="countItemName">エラーメッセージ中のcount項目名</param>
        /// <exception cref="ArgumentException"></exception>
        public static void Range(int index, int count, int listCount,
            string indexItemName = "index", string countItemName = "count")
        {
            if (listCount - index < count)
                throw new ArgumentException(
                    $"{indexItemName}および{countItemName}が有効な範囲を示していません。");
        }

        /// <summary>
        /// 要素列挙にnull要素が含まれていないことを検証する。
        /// </summary>
        /// <param name="items">要素列挙</param>
        /// <param name="itemName">エラーメッセージ中の項目名</param>
        /// <typeparam name="T">検証型</typeparam>
        /// <exception cref="ArgumentNullException">itemがnullの場合</exception>
        public static void ItemsHasNotNull<T>(IEnumerable<T> items, string itemName = "items")
        {
            if (items.HasNullItem())
                throw new ArgumentNullException(
                    ErrorMessage.NotNullInList(itemName));
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
            if (listCount == 0)
                throw new InvalidOperationException(
                    ErrorMessage.NotExecute("リストの要素が0個のため"));
        }
    }
}
