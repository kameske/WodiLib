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
            if (count < min || max < count)
                throw new InvalidOperationException(
                    ErrorMessage.OutOfRange($"{itemName}の要素数", min, max, count));
        }

        /// <summary>
        /// 要素数が最大数を超えないことを検証する。
        /// </summary>
        /// <param name="count">要素数</param>
        /// <param name="max">要素最大数</param>
        /// <exception cref="InvalidOperationException">listの要素数が不適切な場合</exception>
        public static void ItemMaxCount(int count, int max)
        {
            if (count > max)
                throw new InvalidOperationException(
                    ErrorMessage.OverListLength(max));
        }

        /// <summary>
        /// 要素数が最小数未満にならないことを検証する。
        /// </summary>
        /// <param name="count">要素数</param>
        /// <param name="min">最小要素数</param>
        /// <exception cref="InvalidOperationException"></exception>
        public static void ItemMinCount(int count, int min)
        {
            if (count < min)
                throw new InvalidOperationException(
                    ErrorMessage.UnderListLength(min));
        }

        /// <summary>
        /// Overwrite時に要素数が最大数を超えないことを検証する。
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <param name="nowLength">現在要素数</param>
        /// <param name="itemLength">対象要素数</param>
        /// <param name="max">最大要素数</param>
        public static void OverwriteLength(int index, int nowLength, int itemLength, int max)
        {
            var updateCnt = itemLength;
            if (updateCnt + index > nowLength) updateCnt = nowLength - index;
            var insertCnt = itemLength - updateCnt;

            ItemMaxCount(nowLength + insertCnt, max);
        }
    }
}
