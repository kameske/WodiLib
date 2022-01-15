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
        /// <param name="min">最小要素数</param>
        /// <param name="max">最大要素数</param>
        /// <param name="itemName">エラーメッセージ中の項目名</param>
        /// <exception cref="ArgumentException">
        ///     <paramref name="count"/> &lt; <paramref name="min"/> の場合、
        ///     または <paramref name="count"/> &gt; <paramref name="max"/> の場合。
        /// </exception>
        public static void ArgumentItemsCount(int count, int min, int max, string itemName = "initItems")
        {
            ThrowHelper.ValidateArgumentNotExecute(
                count < min || max < count,
                () => ErrorMessage.OutOfRange($"{itemName}の要素数", min, max, count)
            );
        }

        /// <summary>
        ///     要素数が最大数を超えないことを検証する。
        /// </summary>
        /// <param name="count">要素数</param>
        /// <param name="max">最大要素数</param>
        /// <exception cref="ArgumentException">
        ///     <paramref name="count"/> &gt; <paramref name="max"/> の場合。
        /// </exception>
        public static void ItemMaxCount(int count, int max)
        {
            ThrowHelper.ValidateArgumentNotExecute(
                count > max,
                () => ErrorMessage.OverListLength(max)
            );
        }

        /// <summary>
        ///     要素数が最小数未満にならないことを検証する。
        /// </summary>
        /// <param name="count">要素数</param>
        /// <param name="min">最小要素数</param>
        /// <exception cref="ArgumentException">
        ///     <paramref name="count"/> &lt; <paramref name="min"/> の場合。
        /// </exception>
        public static void ItemMinCount(NamedValue<int> count, int min)
        {
            ThrowHelper.ValidateListMinItemCount(count.Value < min, count.Name, min);
        }

        /// <summary>
        /// 上書き後の要素数が最大数を超えないことを検証する。
        /// </summary>
        /// <param name="start">上書き開始インデックス</param>
        /// <param name="overwrite">上書き要素数</param>
        /// <param name="now">現在要素数</param>
        /// <param name="max">最大要素数</param>
        public static void OverwrittenCount(int start, int overwrite, int now, int max)
        {
            if (start + overwrite <= now)
            {
                return;
            }

            ItemMaxCount(start + overwrite, max);
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
        public static void CapacityConfig(NamedValue<int> min, NamedValue<int> max)
        {
            ThrowHelper.InvalidOperationIf(
                min.Value < 0,
                () => ErrorMessage.GreaterOrEqual(min.Name, 0, min.Value)
            );
            ThrowHelper.InvalidOperationIf(
                min.Value > max.Value,
                () => ErrorMessage.GreaterOrEqual(max.Name, $"MinValue({min})", max.Value)
            );
        }
    }
}
