// ========================================
// Project Name : WodiLib
// File Name    : FixedLengthListValidationHelper.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;

namespace WodiLib.Sys
{
    /// <summary>
    ///     長さ固定リストの検証Helperクラス
    /// </summary>
    internal static class FixedLengthListValidationHelper
    {
        /// <summary>
        ///     要素数が適切であることを検証する。
        /// </summary>
        /// <param name="count">要素数</param>
        /// <param name="capacity">要素数</param>
        /// <param name="itemName">エラーメッセージ中の項目名</param>
        /// <exception cref="ArgumentException">リストの要素数が不適切な場合</exception>
        public static void ItemCount(int count, int capacity, string itemName = "initItems")
        {
            ThrowHelper.ValidateArgumentNotExecute(count != capacity,
                () => ErrorMessage.NotEqual($"{itemName}の要素数", $"適切な要素数({capacity})"));
        }

        /// <summary>
        ///     容量設定を検証する。
        /// </summary>
        /// <param name="capacity">容量</param>
        /// <exception cref="InvalidOperationException">
        ///     <paramref name="capacity"/> が 0 未満の場合。
        /// </exception>
        public static void CapacityConfig(int capacity)
        {
            ThrowHelper.InvalidOperationIf(capacity < 0,
                () => ErrorMessage.GreaterOrEqual("Capacity", 0, capacity));
        }
    }
}
