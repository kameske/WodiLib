// ========================================
// Project Name : WodiLib
// File Name    : WodiLibTwoDimensionalListValidator.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
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
    /// <typeparam name="TIn">リスト要素入力型</typeparam>
    /// <typeparam name="TOut">リスト要素出力型</typeparam>
    internal abstract class WodiLibTwoDimensionalListValidator<TIn, TOut> : ITwoDimensionalListValidator<TIn, TOut>
        where TOut : TIn
    {
        protected abstract ITwoDimensionalListValidator<TIn, TOut>? BaseValidator { get; }

        protected ITwoDimensionalList<TIn, TOut> Target { get; }

        protected string RowName { get; }

        protected string ColumnName { get; }

        protected WodiLibTwoDimensionalListValidator(ITwoDimensionalList<TIn, TOut> target,
            string rowName, string columnName)
        {
            Target = target;
            RowName = rowName;
            ColumnName = columnName;
        }

        public virtual void Constructor(TIn[][] initItems)
            => BaseValidator?.Constructor(initItems);

        public virtual void GetRow(int rowIndex, int rowCount)
            => BaseValidator?.GetRow(rowIndex, rowCount);

        public virtual void GetColumn(int columnIndex, int columnCount)
            => BaseValidator?.GetColumn(columnIndex, columnCount);

        public virtual void GetItem(int rowIndex, int rowCount, int columnIndex, int columnCount)
            => BaseValidator?.GetItem(columnIndex, columnCount, rowIndex, rowCount);

        public virtual void GetItem(int rowIndex, int columnIndex)
            => BaseValidator?.GetItem(rowIndex, columnIndex);

        public virtual void SetRow(int rowIndex, params IEnumerable<TIn>[] rows)
            => BaseValidator?.SetRow(rowIndex, rows);

        public virtual void SetColumn(int columnIndex, params IEnumerable<TIn>[] items)
            => BaseValidator?.SetColumn(columnIndex, items);

        public virtual void SetItem(int rowIndex, int columnIndex, TIn item)
            => BaseValidator?.SetItem(rowIndex, columnIndex, item);

        public virtual void InsertRow(int rowIndex, params IEnumerable<TIn>[] items)
            => BaseValidator?.InsertRow(rowIndex, items);

        public virtual void InsertColumn(int columnIndex, params IEnumerable<TIn>[] items)
            => BaseValidator?.InsertColumn(columnIndex, items);

        public virtual void OverwriteRow(int rowIndex, params IEnumerable<TIn>[] items)
            => BaseValidator?.OverwriteRow(rowIndex, items);

        public virtual void OverwriteColumn(int columnIndex, params IEnumerable<TIn>[] items)
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

        public virtual void Reset(IEnumerable<IEnumerable<TIn>> initItems)
            => BaseValidator?.Reset(initItems);

        public abstract ITwoDimensionalListValidator<TIn, TOut> CreateAnotherFor(ITwoDimensionalList<TIn, TOut> target);
    }

    /// <summary>
    ///     二次元リスト編集メソッドの引数検証クラステンプレート
    /// </summary>
    /// <remarks>
    ///     継承先で定義した <see cref="BaseValidator"/> を起動するだけの実装。<br/>
    ///     <see cref="BaseValidator"/> の検証処理を拡張したいときに対象のメソッドをオーバーライドして
    ///     処理拡張を行う。
    /// </remarks>
    /// <typeparam name="T">リスト内包型</typeparam>
    [Obsolete]
    internal abstract class WodiLibTwoDimensionalListValidator<T> : ITwoDimensionalListValidator<T>
    {
        protected abstract ITwoDimensionalListValidator<T>? BaseValidator { get; }

        protected ITwoDimensionalList<T> Target { get; }

        protected string RowName { get; }

        protected string ColumnName { get; }

        protected WodiLibTwoDimensionalListValidator(ITwoDimensionalList<T> target,
            string rowName, string columnName)
        {
            Target = target;
            RowName = rowName;
            ColumnName = columnName;
        }

        public virtual void Constructor(T[][] initItems)
            => BaseValidator?.Constructor(initItems);

        public virtual void GetRow(int rowIndex, int rowCount)
            => BaseValidator?.GetRow(rowIndex, rowCount);

        public virtual void GetColumn(int columnIndex, int columnCount)
            => BaseValidator?.GetColumn(columnIndex, columnCount);

        public virtual void GetItem(int rowIndex, int rowCount, int columnIndex, int columnCount)
            => BaseValidator?.GetItem(columnIndex, columnCount, rowIndex, rowCount);

        public virtual void GetItem(int rowIndex, int columnIndex)
            => BaseValidator?.GetItem(rowIndex, columnIndex);

        public virtual void SetRow(int rowIndex, params IEnumerable<T>[] rows)
            => BaseValidator?.SetRow(rowIndex, rows);

        public virtual void SetColumn(int columnIndex, params IEnumerable<T>[] items)
            => BaseValidator?.SetColumn(columnIndex, items);

        public virtual void SetItem(int rowIndex, int columnIndex, T item)
            => BaseValidator?.SetItem(rowIndex, columnIndex, item);

        public virtual void InsertRow(int rowIndex, params IEnumerable<T>[] items)
            => BaseValidator?.InsertRow(rowIndex, items);

        public virtual void InsertColumn(int columnIndex, params IEnumerable<T>[] items)
            => BaseValidator?.InsertColumn(columnIndex, items);

        public virtual void OverwriteRow(int rowIndex, params IEnumerable<T>[] items)
            => BaseValidator?.OverwriteRow(rowIndex, items);

        public virtual void OverwriteColumn(int columnIndex, params IEnumerable<T>[] items)
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

        public virtual void Reset(IEnumerable<IEnumerable<T>> initItems)
            => BaseValidator?.Reset(initItems);

        public abstract ITwoDimensionalListValidator<T> CreateAnotherFor(ITwoDimensionalList<T> target);
    }
}
