// ========================================
// Project Name : WodiLib
// File Name    : WodiLibTwoDimensionalListValidator.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.Sys
{
    /// <summary>
    ///     二次元リスト編集メソッドの引数検証クラステンプレート
    /// </summary>
    /// <remarks>
    ///     継承先で定義した <see cref="BaseValidator"/> を起動するだけの実装。<br/>
    ///     <see cref="BaseValidator"/> の検証処理を拡張したいときに対象のメソッドをオーバーライドして
    ///     処理拡張を行う。
    /// </remarks>
    /// <typeparam name="T">リスト内包型</typeparam>
    internal abstract class WodiLibTwoDimensionalListValidator<T> : ITwoDimensionalListValidator<T>
    {
        protected abstract ITwoDimensionalListValidator<T>? BaseValidator { get; }

        protected IReadOnlyTwoDimensionalList<T> Target { get; }

        protected WodiLibTwoDimensionalListValidator(IReadOnlyTwoDimensionalList<T> target)
        {
            Target = target;
        }

        public virtual void Constructor(T[][] initItems)
        {
            BaseValidator?.Constructor(initItems);
        }

        public virtual void Get(int row, int rowCount, int column, int columnCount)
        {
            BaseValidator?.Get(row, rowCount, column, columnCount);
        }

        public virtual void GetRow(int row, int count)
        {
            BaseValidator?.GetRow(row, count);
        }

        public virtual void GetColumn(int column, int count)
        {
            BaseValidator?.GetColumn(column, count);
        }

        public virtual void Set(int row, int column, T[][] items)
        {
            BaseValidator?.Set(row, column, items);
        }

        public virtual void InsertRow(int row, T[][] items)
        {
            BaseValidator?.InsertRow(row, items);
        }

        public virtual void InsertColumn(int column, T[][] items)
        {
            BaseValidator?.InsertColumn(column, items);
        }

        public virtual void OverwriteRow(int row, T[][] items)
        {
            BaseValidator?.OverwriteRow(row, items);
        }

        public virtual void OverwriteColumn(int column, T[][] items)
        {
            BaseValidator?.OverwriteColumn(column, items);
        }

        public virtual void MoveRow(int oldRow, int newRow, int count)
        {
            BaseValidator?.MoveRow(oldRow, newRow, count);
        }

        public virtual void MoveColumn(int oldColumn, int newColumn, int count)
        {
            BaseValidator?.MoveColumn(oldColumn, newColumn, count);
        }

        public virtual void RemoveRow(int row, int count)
        {
            BaseValidator?.RemoveRow(row, count);
        }

        public virtual void RemoveColumn(int column, int count)
        {
            BaseValidator?.RemoveColumn(column, count);
        }

        public virtual void AdjustLength(int rowLength, int columnLength)
        {
            BaseValidator?.AdjustLength(rowLength, columnLength);
        }

        public virtual void AdjustLengthIfShort(int rowLength, int columnLength)
        {
            BaseValidator?.AdjustLengthIfShort(rowLength, columnLength);
        }

        public virtual void AdjustLengthIfLong(int rowLength, int columnLength)
        {
            BaseValidator?.AdjustLengthIfLong(rowLength, columnLength);
        }

        public virtual void AdjustRowLength(int length)
        {
            BaseValidator?.AdjustRowLength(length);
        }

        public virtual void AdjustColumnLength(int length)
        {
            BaseValidator?.AdjustColumnLength(length);
        }

        public virtual void AdjustRowLengthIfShort(int length)
        {
            BaseValidator?.AdjustRowLengthIfShort(length);
        }

        public virtual void AdjustColumnLengthIfShort(int length)
        {
            BaseValidator?.AdjustColumnLengthIfShort(length);
        }

        public virtual void AdjustRowLengthIfLong(int length)
        {
            BaseValidator?.AdjustRowLengthIfLong(length);
        }

        public virtual void AdjustColumnLengthIfLong(int length)
        {
            BaseValidator?.AdjustColumnLengthIfLong(length);
        }

        public virtual void Reset(T[][] items)
        {
            BaseValidator?.Reset(items);
        }
    }
}
