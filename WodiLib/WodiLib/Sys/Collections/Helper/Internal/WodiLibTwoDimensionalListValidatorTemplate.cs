// ========================================
// Project Name : WodiLib
// File Name    : WodiLibTwoDimensionalListValidatorTemplate.cs
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
    /// <typeparam name="TRow">リスト行データ型</typeparam>
    /// <typeparam name="TItem">リスト要素型</typeparam>
    internal abstract class
        WodiLibTwoDimensionalListValidatorTemplate<TRow, TItem>
        : ITwoDimensionalListValidator<TRow, TItem>
        where TRow : IEnumerable<TItem>
    {
        protected abstract ITwoDimensionalListValidator<TRow, TItem>? BaseValidator { get; }

        protected ITwoDimensionalList<TRow, TItem> Target { get; }

        protected string RowName { get; }

        protected string ColumnName { get; }

        protected WodiLibTwoDimensionalListValidatorTemplate(
            ITwoDimensionalList<TRow, TItem> target,
            string rowName,
            string columnName
        )
        {
            Target = target;
            RowName = rowName;
            ColumnName = columnName;
        }

        public virtual void Constructor(NamedValue<IEnumerable<TRow>> initItems)
        {
            BaseValidator?.Constructor(initItems);
            Row(new NamedValue<int>("", 0), initItems);
        }

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
            => BaseValidator?.GetItem(rowIndex, rowCount, columnIndex, columnCount);

        public virtual void GetItem(NamedValue<int> rowIndex, NamedValue<int> columnIndex)
            => BaseValidator?.GetItem(rowIndex, columnIndex);

        public virtual void SetRow(NamedValue<int> rowIndex, NamedValue<IEnumerable<TRow>> rows)
        {
            BaseValidator?.SetRow(rowIndex, rows);
            Row(rowIndex, rows);
        }

        public virtual void SetColumn(NamedValue<int> columnIndex, NamedValue<IEnumerable<IEnumerable<TItem>>> items)
        {
            BaseValidator?.SetColumn(columnIndex, items);
            Column(columnIndex, items);
        }

        public virtual void SetItem(NamedValue<int> rowIndex, NamedValue<int> columnIndex, NamedValue<TItem> item)
            => BaseValidator?.SetItem(rowIndex, columnIndex, item);

        public virtual void InsertRow(NamedValue<int> rowIndex, NamedValue<IEnumerable<TRow>> items)
        {
            BaseValidator?.InsertRow(rowIndex, items);
            Row(rowIndex, items);
        }

        public virtual void InsertColumn(
            NamedValue<int> columnIndex,
            NamedValue<IEnumerable<IEnumerable<TItem>>> items
        )
        {
            BaseValidator?.InsertColumn(columnIndex, items);
            Column(columnIndex, items);
        }

        public virtual void OverwriteRow(NamedValue<int> rowIndex, NamedValue<IEnumerable<TRow>> items)
        {
            BaseValidator?.OverwriteRow(rowIndex, items);
            Row(rowIndex, items);
        }

        public virtual void OverwriteColumn(
            NamedValue<int> columnIndex,
            NamedValue<IEnumerable<IEnumerable<TItem>>> items
        )
        {
            BaseValidator?.OverwriteColumn(columnIndex, items);
            Column(columnIndex, items);
        }

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

        public virtual void Reset(NamedValue<IEnumerable<TRow>> initItems)
        {
            BaseValidator?.Reset(initItems);
            Row(("index", 0), initItems);
        }

        public void Clear()
            => BaseValidator?.Clear();

        /// <summary>
        ///     この二次元配列に対して 追加、挿入、上書き される行データに対する共通的な検証処理。
        /// </summary>
        /// <remarks>
        ///     デフォルトでは何もしない。
        /// </remarks>
        /// <param name="rowIndex">行インデックス</param>
        /// <param name="rows">追加/挿入/上書き行データ</param>
        protected virtual void Row(NamedValue<int> rowIndex, NamedValue<IEnumerable<TRow>> rows)
        {
        }

        /// <summary>
        ///     この二次元配列に対して 追加、挿入、上書き される列データに対する共通的な検証処理。
        /// </summary>
        /// <remarks>
        ///     デフォルトでは何もしない。
        /// </remarks>
        /// <param name="columnIndex">列インデックス</param>
        /// <param name="columns">追加/挿入/上書き列データ</param>
        protected virtual void Column(
            NamedValue<int> columnIndex,
            NamedValue<IEnumerable<IEnumerable<TItem>>> columns
        )
        {
        }
    }
}
