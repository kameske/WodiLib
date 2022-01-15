// ========================================
// Project Name : WodiLib
// File Name    : WodiLibTwoDimensionalListValidator.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     二次元リスト編集メソッドの引数検証クラステンプレート
    /// </summary>
    /// <remarks>
    ///     継承先で定義した <see cref="BaseValidator"/> を起動するだけの実装。<br/>
    ///     <see cref="BaseValidator"/> の検証処理を拡張したいときに対象のメソッドをオーバーライドして
    ///     処理拡張を行う。
    /// </remarks>
    /// <typeparam name="TInRow">リスト行データ入力型</typeparam>
    /// <typeparam name="TOutRow">リスト行データ出力型</typeparam>
    /// <typeparam name="TInItem">リスト要素入力型</typeparam>
    /// <typeparam name="TOutItem">リスト要素出力型</typeparam>
    internal abstract class
        WodiLibTwoDimensionalListValidator<TInRow, TOutRow, TInItem, TOutItem>
        : ITwoDimensionalListValidator<TInRow, TInItem>
        where TInRow : IEnumerable<TInItem>
        where TOutRow : IEnumerable<TOutItem>, TInRow
        where TOutItem : TInItem
    {
        protected abstract ITwoDimensionalListValidator<TInRow, TInItem>? BaseValidator { get; }

        protected ITwoDimensionalList<TInRow, TOutRow, TInItem, TOutItem> Target { get; }

        protected string RowName { get; }

        protected string ColumnName { get; }

        protected WodiLibTwoDimensionalListValidator(
            ITwoDimensionalList<TInRow, TOutRow, TInItem, TOutItem> target,
            string rowName,
            string columnName
        )
        {
            Target = target;
            RowName = rowName;
            ColumnName = columnName;
        }

        public virtual void Constructor(NamedValue<TInRow[]> initItems)
            => BaseValidator?.Constructor(initItems);

        public virtual void GetRow(NamedValue<int> rowIndex, NamedValue<int> rowCount)
            => BaseValidator?.GetRow(rowIndex, rowCount);

        public virtual void GetColumn(NamedValue<int> columnIndex, NamedValue<int> columnCount)
            => BaseValidator?.GetColumn(columnIndex, columnCount);

        public virtual void GetItem(
            NamedValue<int> rowIndex,
            NamedValue<int> rowCount,
            NamedValue<int> columnIndex,
            NamedValue<int> columnCount
        )
            => BaseValidator?.GetItem(columnIndex, columnCount, rowIndex, rowCount);

        public virtual void GetItem(NamedValue<int> rowIndex, NamedValue<int> columnIndex)
            => BaseValidator?.GetItem(rowIndex, columnIndex);

        public virtual void SetRow(NamedValue<int> rowIndex, string targetParamName, params TInRow[] rows)
            => BaseValidator?.SetRow(rowIndex, targetParamName, rows);

        public virtual void SetColumn(
            NamedValue<int> columnIndex,
            string targetParamName,
            params IEnumerable<TInItem>[] items
        )
            => BaseValidator?.SetColumn(columnIndex, targetParamName, items);

        public virtual void SetItem(NamedValue<int> rowIndex, NamedValue<int> columnIndex, NamedValue<TInItem> item)
            => BaseValidator?.SetItem(rowIndex, columnIndex, item);

        public virtual void InsertRow(NamedValue<int> rowIndex, string targetParamName, params TInRow[] items)
            => BaseValidator?.InsertRow(rowIndex, targetParamName, items);

        public virtual void InsertColumn(
            NamedValue<int> columnIndex,
            string targetParamName,
            params IEnumerable<TInItem>[] items
        )
            => BaseValidator?.InsertColumn(columnIndex, targetParamName, items);

        public virtual void OverwriteRow(NamedValue<int> rowIndex, string targetParamName, params TInRow[] items)
            => BaseValidator?.OverwriteRow(rowIndex, targetParamName, items);

        public virtual void OverwriteColumn(NamedValue<int> columnIndex, string targetParamName,
            params IEnumerable<TInItem>[] items)
            => BaseValidator?.OverwriteColumn(columnIndex, targetParamName, items);

        public virtual void MoveRow(NamedValue<int> oldRowIndex, NamedValue<int> newRowIndex, NamedValue<int> count)
            => BaseValidator?.MoveRow(oldRowIndex, newRowIndex, count);

        public virtual void MoveColumn(
            NamedValue<int> oldColumnIndex,
            NamedValue<int> newColumnIndex,
            NamedValue<int> count
        )
            => BaseValidator?.MoveColumn(oldColumnIndex, newColumnIndex, count);

        public virtual void RemoveRow(NamedValue<int> rowIndex, NamedValue<int> count)
            => BaseValidator?.RemoveRow(rowIndex, count);

        public virtual void RemoveColumn(NamedValue<int> columnIndex, NamedValue<int> count)
            => BaseValidator?.RemoveColumn(columnIndex, count);

        public virtual void AdjustLength(NamedValue<int> rowLength, NamedValue<int> columnLength)
            => BaseValidator?.AdjustLength(rowLength, columnLength);

        public virtual void Reset(NamedValue<IEnumerable<TInRow>> initItems)
            => BaseValidator?.Reset(initItems);
    }
}
