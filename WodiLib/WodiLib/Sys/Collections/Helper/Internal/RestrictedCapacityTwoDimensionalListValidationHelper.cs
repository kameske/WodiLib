// ========================================
// Project Name : WodiLib
// File Name    : RestrictedCapacityTwoDimensionalListValidationHelper.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     二次元リスト検証 Helper クラス
    /// </summary>
    internal static class RestrictedCapacityTwoDimensionalListValidationHelper
    {
        /// <summary>
        ///     最大・最小行・列設定を検証する。
        /// </summary>
        /// <param name="rowMin">最小行数</param>
        /// <param name="rowMax">最大行数</param>
        /// <param name="columnMin">最小列数</param>
        /// <param name="columnMax">最大列数</param>
        public static void CapacityConfig(
            NamedValue<int> rowMin,
            NamedValue<int> rowMax,
            NamedValue<int> columnMin,
            NamedValue<int> columnMax
        )
        {
            ThrowHelper.ValidateArgumentValueGreaterOrEqual(rowMin.Value < 0, rowMin.Name, 0, rowMax.Value);
            ThrowHelper.ValidateArgumentValueGreaterOrEqual(
                rowMin.Value > rowMax.Value,
                rowMax.Name,
                $"{rowMin.Name}（{rowMin.Value}）",
                rowMax.Value
            );
            ThrowHelper.ValidateArgumentValueGreaterOrEqual(columnMin.Value < 0, columnMin.Name, 0, columnMax.Value);
            ThrowHelper.ValidateArgumentValueGreaterOrEqual(
                columnMin.Value > columnMax.Value,
                columnMax.Name,
                $"{columnMin.Name}（{columnMin.Value}）",
                columnMax.Value
            );
        }

        /// <summary>
        ///     行 or 列数が適切であることを検証する、
        /// </summary>
        /// <param name="count">行 or 列数</param>
        /// <param name="min">最小数</param>
        /// <param name="max">最大数</param>
        /// <param name="lineName">行名 or 列名</param>
        public static void ItemCount(int count, int min, int max, string lineName)
        {
            ThrowHelper.ValidateArgumentValueRange(count < min || max < count, lineName, count, min, max);
        }

        /// <summary>
        ///     行数および列数が適切であることを検証する。
        /// </summary>
        /// <remarks>
        ///     【事前条件】<br/>
        ///     - すべての行の要素数が一致すること
        /// </remarks>
        /// <param name="target">検証対象</param>
        /// <param name="rowMin">行数最小数</param>
        /// <param name="rowMax">行数最大数</param>
        /// <param name="colMin">列数最小数</param>
        /// <param name="colMax">列数最大数</param>
        /// <param name="rowName">行名</param>
        /// <param name="columnName">列名</param>
        /// <param name="itemName">エラーメッセージ中の項目名</param>
        /// <typeparam name="T">リスト内包型</typeparam>
        /// <exception cref="InvalidOperationException">targetの行数または列数が不適切な場合</exception>
        public static void RowAndColCount<T>(
            T[][] target,
            int rowMin,
            int rowMax,
            int colMin,
            int colMax,
            string rowName,
            string columnName,
            string itemName = "initItems"
        )
        {
            var rowCount = target.Length;
            ThrowHelper.ValidateArgumentValueRange(
                rowCount < rowMin || rowMax < rowCount,
                $"{itemName}の{rowName}数",
                rowCount,
                rowMin,
                rowMax
            );

            if (rowCount == 0) return;

            var colCount = target[0].Length;
            ThrowHelper.ValidateArgumentValueRange(
                colCount < colMin || colMax < colCount,
                $"{itemName}の{columnName}数",
                colCount,
                colMin,
                colMax
            );
        }

        /// <summary>
        ///     要素数が最大値を超えないことを検証する。
        /// </summary>
        /// <param name="count">要素数</param>
        /// <param name="max">最大値</param>
        /// <param name="lineName">行 or 列名</param>
        /// <exception cref="ArgumentException">
        ///     <paramref name="count"/> &gt; <paramref name="max"/> の場合。
        /// </exception>
        public static void ItemMaxCount(int count, int max, string lineName)
        {
            ThrowHelper.ValidateListMaxItemCount(count > max, $"{lineName}数", max);
        }

        /// <summary>
        ///     要素数が最小値を下回らないことを検証する。
        /// </summary>
        /// <param name="count">要素数</param>
        /// <param name="min">最小値</param>
        /// <param name="lineName">行 or 列名</param>
        /// <exception cref="ArgumentException">
        ///     <paramref name="count"/> &lt; <paramref name="min"/> の場合。
        /// </exception>
        public static void ItemMinCount(int count, int min, string lineName)
        {
            ThrowHelper.ValidateListMinItemCount(count < min, $"{lineName}数", min);
        }
    }
}
