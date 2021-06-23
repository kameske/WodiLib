// ========================================
// Project Name : WodiLib
// File Name    : RestrictedCapacityTwoDimensionalListValidator.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;

namespace WodiLib.Sys.Collections
{
    internal class RestrictedCapacityTwoDimensionalListValidator<T> : ITwoDimensionalListValidator<T>
    {
        private IReadOnlyRestrictedCapacityTwoDimensionalList<T> Target { get; }

        private CommonTwoDimensionalListValidator<T> PreConditionValidator { get; }

        public RestrictedCapacityTwoDimensionalListValidator(IReadOnlyRestrictedCapacityTwoDimensionalList<T> target)
        {
            Target = target;
            PreConditionValidator = new CommonTwoDimensionalListValidator<T>(target);
        }

        public void Constructor(T[][] initItems)
        {
#if DEBUG
            RestrictedCapacityTwoDimensionalListValidationHelper.CapacityConfig(Target.GetMinCapacity(),
                Target.GetMaxCapacity(), Target.GetMinItemCapacity(), Target.GetMaxItemCapacity());
#endif
            PreConditionValidator.Constructor(initItems);
            RestrictedCapacityTwoDimensionalListValidationHelper.RowAndColCount(initItems,
                Target.GetMinCapacity(), Target.GetMaxCapacity(), Target.GetMinItemCapacity(),
                Target.GetMaxItemCapacity());
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
                ? Target.GetMaxCapacity()
                : Target.GetMaxItemCapacity();
            var nowLength = direction != Direction.Column
                ? Target.Count
                : Target.ItemCount;
            RestrictedCapacityTwoDimensionalListValidationHelper.ItemMaxCount(
                nowLength + items.Length, max, direction);
        }

        public void Overwrite(int row, T[][] items, Direction direction)
        {
            PreConditionValidator.Overwrite(row, items, direction);
            if (Target.ItemCount == 0 && items.Length > 0)
            {
                RestrictedCapacityTwoDimensionalListValidationHelper.ItemMaxCount(
                    items[0].Length,
                    Target.GetMaxItemCapacity(), Direction.Column);
            }

            if (direction != Direction.Column)
            {
                if (row + items.Length > Target.Count)
                {
                    RestrictedCapacityTwoDimensionalListValidationHelper.ItemMaxCount(
                        row + items.Length,
                        Target.GetMaxCapacity(), Direction.Row);
                }
            }
            else
            {
                if (row + items.Length > Target.ItemCount)
                {
                    RestrictedCapacityTwoDimensionalListValidationHelper.ItemMaxCount(
                        row + items.Length,
                        Target.GetMaxItemCapacity(), Direction.Column);
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
                ? Target.Count
                : Target.ItemCount) - count;
            var min = direction != Direction.Column
                ? Target.GetMinCapacity()
                : Target.GetMinItemCapacity();
            RestrictedCapacityTwoDimensionalListValidationHelper.ItemMinCount(
                checkLength, min, direction);
        }

        public void AdjustLength(int rowLength, int columnLength)
        {
            PreConditionValidator.AdjustLength(rowLength, columnLength);
            RestrictedCapacityTwoDimensionalListValidationHelper.ItemCount(rowLength,
                Target.GetMinCapacity(), Target.GetMaxCapacity(), Direction.Row);
            RestrictedCapacityTwoDimensionalListValidationHelper.ItemCount(columnLength,
                Target.GetMinItemCapacity(), Target.GetMaxItemCapacity(), Direction.Column);
        }

        public void Reset(T[][] items)
        {
            PreConditionValidator.Reset(items);
            RestrictedCapacityTwoDimensionalListValidationHelper.RowAndColCount(items, Target.GetMinCapacity(),
                Target.GetMaxCapacity(), Target.GetMinItemCapacity(), Target.GetMaxItemCapacity(),
                nameof(items));
        }

        public ITwoDimensionalListValidator<T> CreateAnotherFor(IReadOnlyTwoDimensionalList<T> target)
        {
            if (target is IReadOnlyRestrictedCapacityTwoDimensionalList<T> casted)
            {
                return new RestrictedCapacityTwoDimensionalListValidator<T>(casted);
            }

            throw new ArgumentException(ErrorMessage.InvalidAnyCast(nameof(target),
                nameof(RestrictedCapacityTwoDimensionalListValidator<T>)));
        }
    }
}
