// ========================================
// Project Name : WodiLib
// File Name    : RestrictedCapacityTwoDimensionalListValidationHelper.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;

namespace WodiLib.Sys
{
    /// <summary>
    /// 二次元リスト検証 Helper クラス
    /// </summary>
    internal static class RestrictedCapacityTwoDimensionalListValidationHelper
    {
        /// <summary>
        /// 最大・最小行・列設定を検証する。
        /// </summary>
        /// <param name="rowMin">最小行数</param>
        /// <param name="rowMax">最大行数</param>
        /// <param name="columnMin">最小列数</param>
        /// <param name="columnMax">最大列数</param>
        public static void CapacityConfig(int rowMin, int rowMax, int columnMin, int columnMax)
        {
            if (rowMin < 0)
            {
                throw new InvalidOperationException(
                    ErrorMessage.GreaterOrEqual("最小行数", 0, rowMax));
            }

            if (rowMin > rowMax)
            {
                throw new InvalidOperationException(
                    ErrorMessage.GreaterOrEqual("最大行数", $"最小行数（{rowMin}）", rowMax));
            }

            if (columnMin < 0)
            {
                throw new InvalidOperationException(
                    ErrorMessage.GreaterOrEqual("最小列数", 0, columnMax));
            }

            if (columnMin > columnMax)
            {
                throw new InvalidOperationException(
                    ErrorMessage.GreaterOrEqual("最大列数", $"最小列数（{columnMin}）", columnMax));
            }
        }

        /// <summary>
        /// 行 or 列数が適切であることを検証する、
        /// </summary>
        /// <param name="count">行 or 列数</param>
        /// <param name="min">最小数</param>
        /// <param name="max">最大数</param>
        /// <param name="direction">行 or 列</param>
        /// <param name="itemName">エラーメッセージ中の項目名</param>
        public static void ItemCount(int count, int min, int max, Direction direction)
        {
            var elementName = direction == Direction.Row
                ? "行数"
                : "列数";
            if (count < min || max < count)
                throw new InvalidOperationException(
                    ErrorMessage.OutOfRange(elementName, min, max, count));
        }

        /// <summary>
        /// 行数および列数が適切であることを検証する。
        /// </summary>
        /// <remarks>
        /// 【事前条件】<br/>
        /// - すべての行の要素数が一致すること
        /// </remarks>
        /// <param name="target">検証対象</param>
        /// <param name="rowMin">行数最小数</param>
        /// <param name="rowMax">行数最大数</param>
        /// <param name="colMin">列数最小数</param>
        /// <param name="colMax">列数最大数</param>
        /// <param name="itemName">エラーメッセージ中の項目名</param>
        /// <typeparam name="T">リスト内包型</typeparam>
        /// <exception cref="InvalidOperationException">targetの行数または列数が不適切な場合</exception>
        public static void RowAndColCount<T>(T[][] target, int rowMin, int rowMax,
            int colMin, int colMax, string itemName = "initItems")
        {
            var rowCount = target.Length;
            if (rowCount < rowMin || rowMax < rowCount)
                throw new InvalidOperationException(
                    ErrorMessage.OutOfRange($"{itemName}の行数", rowMin, rowMax, rowCount));

            if (rowCount == 0) return;

            var colCount = target[0].Length;
            if (colCount < colMin || colMax < colCount)
                throw new InvalidOperationException(
                    ErrorMessage.OutOfRange($"{itemName}の列数", colMin, colMax, colCount));
        }

        /// <summary>
        /// 要素数が最大値を超えないことを検証する。
        /// </summary>
        /// <param name="count">要素数</param>
        /// <param name="max">最大値</param>
        /// <param name="direction">行 or 列</param>
        public static void ItemMaxCount(int count, int max, Direction direction)
        {
            if (count > max)
            {
                var targetName = direction == Direction.Row ? "行数" : "列数";
                throw new InvalidOperationException(
                    ErrorMessage.OverListLength(max, targetName));
            }
        }

        /// <summary>
        /// 要素数が最小値を下回らないことを検証する。
        /// </summary>
        /// <param name="count">要素数</param>
        /// <param name="min">最小値</param>
        /// <param name="direction">行 or 列</param>
        public static void ItemMinCount(int count, int min, Direction direction)
        {
            if (count < min)
            {
                var targetName = direction == Direction.Row ? "行数" : "列数";
                throw new InvalidOperationException(
                    ErrorMessage.UnderListLength(min, targetName));
            }
        }
    }
}
