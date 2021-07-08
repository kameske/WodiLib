// ========================================
// Project Name : WodiLib
// File Name    : RestrictedCapacityTwoDimensionalListValidator.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;

namespace WodiLib.Sys.Collections
{
    internal class RestrictedCapacityTwoDimensionalListValidator<T> : ITwoDimensionalListValidator<T>
    {
        private ITwoDimensionalList<T> Target { get; }

        private CommonTwoDimensionalListValidator<T> PreConditionValidator { get; }

        public RestrictedCapacityTwoDimensionalListValidator(ITwoDimensionalList<T> target)
        {
            Target = target;
            PreConditionValidator = new CommonTwoDimensionalListValidator<T>(target);
        }

        public void Constructor(T[][] initItems)
        {
#if DEBUG
            RestrictedCapacityTwoDimensionalListValidationHelper.CapacityConfig(Target.GetMinRowCapacity(),
                Target.GetMaxRowCapacity(), Target.GetMinColumnCapacity(), Target.GetMaxColumnCapacity());
#endif
            PreConditionValidator.Constructor(initItems);
            RestrictedCapacityTwoDimensionalListValidationHelper.RowAndColCount(initItems,
                Target.GetMinRowCapacity(), Target.GetMaxRowCapacity(), Target.GetMinColumnCapacity(),
                Target.GetMaxColumnCapacity());
        }

        public void CopyTo(IReadOnlyList<T>[] array, int index)
        {
            PreConditionValidator.CopyTo(array, index);
        }

        public void CopyTo(IEnumerable<T>[] array, int index)
        {
            PreConditionValidator.CopyTo(array, index);
        }

        public void CopyTo(T[] array, int index, Direction direction)
        {
            PreConditionValidator.CopyTo(array, index, direction);
        }

        public void CopyTo(T[,] array, int row, int column)
        {
            PreConditionValidator.CopyTo(array, row, column);
        }

        public void CopyTo(T[][] array, int row, int column)
        {
            PreConditionValidator.CopyTo(array, row, column);
        }

        public void Get(int row, int rowCount, int column, int columnCount, Direction direction)
        {
            PreConditionValidator.Get(row, rowCount, column, columnCount, direction);
        }

        public void Set(int row, int column, T[][] items, Direction direction, bool needFitItemsInnerSize)
        {
            PreConditionValidator.Set(row, column, items, direction, needFitItemsInnerSize);
        }

        public void Insert(int row, T[][] items, Direction direction)
        {
            PreConditionValidator.Insert(row, items, direction);

            if (items.Length <= 0) return;

            var max = direction != Direction.Column
                ? Target.GetMaxRowCapacity()
                : Target.GetMaxColumnCapacity();
            var nowLength = direction != Direction.Column
                ? Target.RowCount
                : Target.ColumnCount;
            RestrictedCapacityTwoDimensionalListValidationHelper.ItemMaxCount(
                nowLength + items.Length, max, direction);
        }

        public void Overwrite(int row, T[][] items, Direction direction)
        {
            PreConditionValidator.Overwrite(row, items, direction);
            if (Target.ColumnCount == 0 && items.Length > 0)
            {
                RestrictedCapacityTwoDimensionalListValidationHelper.ItemMaxCount(
                    items[0].Length,
                    Target.GetMaxColumnCapacity(), Direction.Column);
            }

            if (direction != Direction.Column)
            {
                if (row + items.Length > Target.RowCount)
                {
                    RestrictedCapacityTwoDimensionalListValidationHelper.ItemMaxCount(
                        row + items.Length,
                        Target.GetMaxRowCapacity(), Direction.Row);
                }
            }
            else
            {
                if (row + items.Length > Target.ColumnCount)
                {
                    RestrictedCapacityTwoDimensionalListValidationHelper.ItemMaxCount(
                        row + items.Length,
                        Target.GetMaxColumnCapacity(), Direction.Column);
                }
            }
        }

        public void Move(int oldIndex, int newIndex, int count, Direction direction)
        {
            PreConditionValidator.Move(oldIndex, newIndex, count, direction);
        }

        public void Remove(int row, int count, Direction direction)
        {
            PreConditionValidator.Remove(row, count, direction);

            var checkLength = (direction != Direction.Column
                ? Target.RowCount
                : Target.ColumnCount) - count;
            var min = direction != Direction.Column
                ? Target.GetMinRowCapacity()
                : Target.GetMinColumnCapacity();
            RestrictedCapacityTwoDimensionalListValidationHelper.ItemMinCount(
                checkLength, min, direction);
        }

        public void AdjustLength(int rowLength, int columnLength)
        {
            PreConditionValidator.AdjustLength(rowLength, columnLength);
            RestrictedCapacityTwoDimensionalListValidationHelper.ItemCount(rowLength,
                Target.GetMinRowCapacity(), Target.GetMaxRowCapacity(), Direction.Row);
            RestrictedCapacityTwoDimensionalListValidationHelper.ItemCount(columnLength,
                Target.GetMinColumnCapacity(), Target.GetMaxColumnCapacity(), Direction.Column);
        }

        public void Reset(T[][] items)
        {
            PreConditionValidator.Reset(items);
            RestrictedCapacityTwoDimensionalListValidationHelper.RowAndColCount(items, Target.GetMinRowCapacity(),
                Target.GetMaxRowCapacity(), Target.GetMinColumnCapacity(), Target.GetMaxColumnCapacity(),
                nameof(items));
        }

        public ITwoDimensionalListValidator<T> CreateAnotherFor(ITwoDimensionalList<T> target)
        {
            return new RestrictedCapacityTwoDimensionalListValidator<T>(target);
        }
    }
}
