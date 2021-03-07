// ========================================
// Project Name : WodiLib
// File Name    : RestrictedListValidationHelper.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     長さ制限リストの検証Helperクラス
    /// </summary>
    internal static class RestrictedListValidationHelper
    {
        /// <summary>
        ///     要素数が適切であることを検証する。
        /// </summary>
        /// <param name="count">要素数</param>
        /// <param name="min">要素最小数</param>
        /// <param name="max">要素最大数</param>
        /// <param name="itemName">エラーメッセージ中の項目名</param>
        /// <exception cref="ArgumentException">
        ///     <paramref name="count"/> &lt; <paramref name="min"/> の場合、
        ///     または <paramref name="count"/> &gt; <paramref name="max"/> の場合。
        /// </exception>
        public static void ArgumentItemsCount(int count, int min, int max, string itemName = "initItems")
        {
            ThrowHelper.ValidateArgumentNotExecute(count < min || max < count,
                () => ErrorMessage.OutOfRange($"{itemName}の要素数", min, max, count));
        }

        /// <summary>
        ///     要素数が最大数を超えないことを検証する。
        /// </summary>
        /// <param name="count">要素数</param>
        /// <param name="max">要素最大数</param>
        /// <exception cref="ArgumentException">
        ///     <paramref name="count"/> &gt; <paramref name="max"/> の場合。
        /// </exception>
        public static void ItemMaxCount(int count, int max)
        {
            ThrowHelper.ValidateArgumentNotExecute(count > max,
                () => ErrorMessage.OverListLength(max));
        }

        /// <summary>
        ///     要素数が最小数未満にならないことを検証する。
        /// </summary>
        /// <param name="count">要素数</param>
        /// <param name="min">最小要素数</param>
        /// <exception cref="ArgumentException">
        ///     <paramref name="count"/> &lt; <paramref name="min"/> の場合。
        /// </exception>
        public static void ItemMinCount(int count, int min)
        {
            ThrowHelper.ValidateListMinItemCount(count < min, "要素数", min);
        }

        /// <summary>
        ///     最大・最小容量設定を検証する。
        /// </summary>
        /// <param name="min">最小容量</param>
        /// <param name="max">最大容量</param>
        /// <exception cref="InvalidOperationException">
        ///     <paramref name="min"/> が 0 未満の場合、
        ///     または <paramref name="max"/> が <paramref name="min"/> 未満の場合。
        /// </exception>
        public static void CapacityConfig(int min, int max)
        {
            ThrowHelper.InvalidOperationIf(min < 0,
                () => ErrorMessage.GreaterOrEqual("MinCapacity", 0, min));
            ThrowHelper.InvalidOperationIf(min > max,
                () => ErrorMessage.GreaterOrEqual("MaxCapacity", $"MinValue({min})", max));
        }
    }
}
