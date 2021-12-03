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
        where TOutRow : IEnumerable<TOutItem>
        where TInRow : IEnumerable<TInItem>, TOutRow
        where TInItem : TOutItem
    {
        protected abstract ITwoDimensionalListValidator<TInRow, TInItem>? BaseValidator { get; }

        protected ITwoDimensionalList<TInRow, TOutRow, TInItem, TOutItem> Target { get; }

        protected string RowName { get; }

        protected string ColumnName { get; }

        protected WodiLibTwoDimensionalListValidator(ITwoDimensionalList<TInRow, TOutRow, TInItem, TOutItem> target,
            string rowName, string columnName)
        {
            Target = target;
            RowName = rowName;
            ColumnName = columnName;
        }

        public virtual void Constructor(TInRow[] initItems)
            => BaseValidator?.Constructor(initItems);

        public virtual void GetRow(int rowIndex, int rowCount)
            => BaseValidator?.GetRow(rowIndex, rowCount);

        public virtual void GetColumn(int columnIndex, int columnCount)
            => BaseValidator?.GetColumn(columnIndex, columnCount);

        public virtual void GetItem(int rowIndex, int rowCount, int columnIndex, int columnCount)
            => BaseValidator?.GetItem(columnIndex, columnCount, rowIndex, rowCount);

        public virtual void GetItem(int rowIndex, int columnIndex)
            => BaseValidator?.GetItem(rowIndex, columnIndex);

        public virtual void SetRow(int rowIndex, params TInRow[] rows)
            => BaseValidator?.SetRow(rowIndex, rows);

        public virtual void SetColumn(int columnIndex, params IEnumerable<TInItem>[] items)
            => BaseValidator?.SetColumn(columnIndex, items);

        public virtual void SetItem(int rowIndex, int columnIndex, TInItem item)
            => BaseValidator?.SetItem(rowIndex, columnIndex, item);

        public virtual void InsertRow(int rowIndex, params TInRow[] items)
            => BaseValidator?.InsertRow(rowIndex, items);

        public virtual void InsertColumn(int columnIndex, params IEnumerable<TInItem>[] items)
            => BaseValidator?.InsertColumn(columnIndex, items);

        public virtual void OverwriteRow(int rowIndex, params TInRow[] items)
            => BaseValidator?.OverwriteRow(rowIndex, items);

        public virtual void OverwriteColumn(int columnIndex, params IEnumerable<TInItem>[] items)
            => BaseValidator?.OverwriteColumn(columnIndex, items);

        public virtual void MoveRow(int oldRowIndex, int newRowIndex, int count)
            => BaseValidator?.MoveRow(oldRowIndex, newRowIndex, count);

        public virtual void MoveColumn(int oldColumnIndex, int newColumnIndex, int count)
            => BaseValidator?.MoveColumn(oldColumnIndex, newColumnIndex, count);

        public virtual void RemoveRow(int rowIndex, int count)
            => BaseValidator?.RemoveRow(rowIndex, count);

        public virtual void RemoveColumn(int columnIndex, int count)
            => BaseValidator?.RemoveColumn(columnIndex, count);

        public virtual void AdjustLength(int rowLength, int columnLength)
            => BaseValidator?.AdjustLength(rowLength, columnLength);

        public virtual void Reset(IEnumerable<TInRow> initItems)
            => BaseValidator?.Reset(initItems);
    }
}
