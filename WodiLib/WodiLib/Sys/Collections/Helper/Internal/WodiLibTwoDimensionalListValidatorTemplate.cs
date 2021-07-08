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
    /// <typeparam name="T">リスト内包型</typeparam>
    internal abstract class WodiLibTwoDimensionalListValidator<T> : ITwoDimensionalListValidator<T>
    {
        protected abstract ITwoDimensionalListValidator<T>? BaseValidator { get; }

        protected IReadOnlyTwoDimensionalList<T> Target { get; }

        protected string RowName { get; }

        protected string ColumnName { get; }

        protected WodiLibTwoDimensionalListValidator(IReadOnlyTwoDimensionalList<T> target,
            string rowName, string columnName)
        {
            Target = target;
            RowName = rowName;
            ColumnName = columnName;
        }

        public virtual void Constructor(T[][] initItems)
            => BaseValidator?.Constructor(initItems);

        public virtual void CopyTo(IReadOnlyList<T>[] array, int index)
            => BaseValidator?.CopyTo(array, index);

        public virtual void CopyTo(IEnumerable<T>[] array, int index)
            => BaseValidator?.CopyTo(array, index);

        public virtual void CopyTo(T[] array, int index, Direction direction)
            => BaseValidator?.CopyTo(array, index, direction);

        public virtual void CopyTo(T[,] array, int row, int column)
            => BaseValidator?.CopyTo(array, row, column);

        public virtual void CopyTo(T[][] array, int row, int column)
            => BaseValidator?.CopyTo(array, row, column);

        public virtual void Get(int row, int rowCount, int column, int columnCount, Direction direction)
            => BaseValidator?.Get(row, rowCount, column, columnCount, direction);

        public virtual void Set(int row, int column, T[][] items, Direction direction, bool needFitItemsInnerSize)
            => BaseValidator?.Set(row, column, items, direction, needFitItemsInnerSize);

        public virtual void Insert(int index, T[][] items, Direction direction)
            => BaseValidator?.Insert(index, items, direction);

        public virtual void Overwrite(int index, T[][] items, Direction direction)
            => BaseValidator?.Overwrite(index, items, direction);

        public virtual void Move(int oldIndex, int newIndex, int count, Direction direction)
            => BaseValidator?.Move(oldIndex, newIndex, count, direction);

        public virtual void Remove(int index, int count, Direction direction)
            => BaseValidator?.Remove(index, count, direction);

        public virtual void AdjustLength(int rowLength, int columnLength)
            => BaseValidator?.AdjustLength(rowLength, columnLength);

        public virtual void Reset(T[][] items)
            => BaseValidator?.Reset(items);

        public abstract ITwoDimensionalListValidator<T> CreateAnotherFor(
            ITwoDimensionalList<T> target);
    }
}
