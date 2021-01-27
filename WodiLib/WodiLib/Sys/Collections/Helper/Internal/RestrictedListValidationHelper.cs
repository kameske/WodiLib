// ========================================
// Project Name : WodiLib
// File Name    : RestrictedListValidationHelper.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;

namespace WodiLib.Sys
{
    /// <summary>
    /// 長さ制限リストの検証Helperクラス
    /// </summary>
    internal static class RestrictedListValidationHelper
    {
        /// <summary>
        /// 要素数が適切であることを検証する。
        /// </summary>
        /// <param name="count">要素数</param>
        /// <param name="min">要素最小数</param>
        /// <param name="max">要素最大数</param>
        /// <param name="itemName">エラーメッセージ中の項目名</param>
        /// <exception cref="InvalidOperationException">listの要素数が不適切な場合</exception>
        public static void ItemCount(int count, int min, int max, string itemName = "initItems")
        {
            ThrowHelper.ValidateArgumentValueRange(count < min || max < count,
                $"{itemName}の要素数", count, min, max);
        }

        /// <summary>
        /// 要素数が最大数を超えないことを検証する。
        /// </summary>
        /// <param name="count">要素数</param>
        /// <param name="max">要素最大数</param>
        /// <exception cref="InvalidOperationException">listの要素数が不適切な場合</exception>
        public static void ItemMaxCount(int count, int max)
        {
            ThrowHelper.ValidateListMaxItemCount(count > max, "要素数", max);
        }

        /// <summary>
        /// 要素数が最小数未満にならないことを検証する。
        /// </summary>
        /// <param name="count">要素数</param>
        /// <param name="min">最小要素数</param>
        /// <exception cref="InvalidOperationException">listの要素数が不適切な場合</exception>
        public static void ItemMinCount(int count, int min)
        {
            ThrowHelper.ValidateListMinItemCount(count < min, "要素数", min);
        }

        /// <summary>
        /// 最大・最小容量設定を検証する。
        /// </summary>
        /// <param name="min">最小容量</param>
        /// <param name="max">最大容量</param>
        public static void CapacityConfig(int min, int max)
        {
            ThrowHelper.ValidateArgumentValueGreaterOrEqual(min < 0, "最小容量", 0, min);
            ThrowHelper.ValidateArgumentValueGreaterOrEqual(min > max, "最大容量", $"最小容量（{min}）", max);
        }
    }
}
