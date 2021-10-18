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
    internal class
        RestrictedCapacityTwoDimensionalListValidator<TIn, TOut> : WodiLibTwoDimensionalListValidator<TIn, TOut>
        where TOut : TIn
    {
        protected override ITwoDimensionalListValidator<TIn, TOut> BaseValidator { get; }

        public RestrictedCapacityTwoDimensionalListValidator(ITwoDimensionalList<TIn, TOut> target,
            string rowName, string columnName) : base(target, rowName, columnName)
        {
            BaseValidator = new CommonTwoDimensionalListValidator<TIn, TOut>(target);
        }

        public override void Constructor(TIn[][] initItems)
        {
#if DEBUG
            RestrictedCapacityTwoDimensionalListValidationHelper.CapacityConfig(Target.GetMinRowCapacity(),
                Target.GetMaxRowCapacity(), Target.GetMinColumnCapacity(), Target.GetMaxColumnCapacity());
#endif
            BaseValidator.Constructor(initItems);
            RestrictedCapacityTwoDimensionalListValidationHelper.RowAndColCount(initItems,
                Target.GetMinRowCapacity(), Target.GetMaxRowCapacity(), Target.GetMinColumnCapacity(),
                Target.GetMaxColumnCapacity(), RowName, ColumnName);
        }

        public override void InsertRow(int rowIndex, params IEnumerable<TIn>[] items)
        {
            BaseValidator.InsertRow(rowIndex, items);

            if (items.Length <= 0) return;

            RestrictedCapacityTwoDimensionalListValidationHelper.ItemMaxCount(
                Target.RowCount + items.Length, Target.GetMaxRowCapacity(), RowName);
        }

        public override void InsertColumn(int columnIndex, params IEnumerable<TIn>[] items)
        {
            BaseValidator.InsertColumn(columnIndex, items);

            if (items.Length <= 0) return;

            RestrictedCapacityTwoDimensionalListValidationHelper.ItemMaxCount(
                Target.ColumnCount + items.Length, Target.GetMaxColumnCapacity(), ColumnName);
        }

        public override void OverwriteRow(int rowIndex, params IEnumerable<TIn>[] items)
        {
            BaseValidator.OverwriteRow(rowIndex, items);

            var itemArrays = items.ToTwoDimensionalArray();

            if (Target.ColumnCount == 0 && items.Length > 0)
            {
                RestrictedCapacityTwoDimensionalListValidationHelper.ItemMaxCount(
                    itemArrays[0].Length,
                    Target.GetMaxColumnCapacity(), ColumnName);
            }

            if (rowIndex + items.Length > Target.RowCount)
            {
                RestrictedCapacityTwoDimensionalListValidationHelper.ItemMaxCount(
                    rowIndex + items.Length, Target.GetMaxRowCapacity(), RowName);
            }
        }

        public override void OverwriteColumn(int columnIndex, params IEnumerable<TIn>[] items)
        {
            BaseValidator.OverwriteColumn(columnIndex, items);

            if (columnIndex + items.Length > Target.ColumnCount)
            {
                RestrictedCapacityTwoDimensionalListValidationHelper.ItemMaxCount(
                    columnIndex + items.Length, Target.GetMaxColumnCapacity(), ColumnName);
            }
        }

        public override void RemoveRow(int rowIndex, int count)
        {
            BaseValidator.RemoveRow(rowIndex, count);

            RestrictedCapacityTwoDimensionalListValidationHelper.ItemMinCount(
                Target.RowCount - count, Target.GetMinRowCapacity(), RowName);
        }

        public override void RemoveColumn(int columnIndex, int count)
        {
            BaseValidator.RemoveColumn(columnIndex, count);

            RestrictedCapacityTwoDimensionalListValidationHelper.ItemMinCount(
                Target.ColumnCount - count, Target.GetMinColumnCapacity(), ColumnName);
        }

        public override void AdjustLength(int rowLength, int columnLength)
        {
            BaseValidator.AdjustLength(rowLength, columnLength);
            RestrictedCapacityTwoDimensionalListValidationHelper.ItemCount(rowLength,
                Target.GetMinRowCapacity(), Target.GetMaxRowCapacity(), Direction.Row);
            RestrictedCapacityTwoDimensionalListValidationHelper.ItemCount(columnLength,
                Target.GetMinColumnCapacity(), Target.GetMaxColumnCapacity(), Direction.Column);
        }

        public override void Reset(IEnumerable<IEnumerable<TIn>> initItems)
        {
            var itemArrays = initItems.ToTwoDimensionalArray();

            BaseValidator.Reset(itemArrays);

            RestrictedCapacityTwoDimensionalListValidationHelper.RowAndColCount(itemArrays,
                Target.GetMinRowCapacity(), Target.GetMaxRowCapacity(), Target.GetMinColumnCapacity(),
                Target.GetMaxColumnCapacity(), RowName, ColumnName);
        }

        public override ITwoDimensionalListValidator<TIn, TOut> CreateAnotherFor(ITwoDimensionalList<TIn, TOut> target)
        {
            return new RestrictedCapacityTwoDimensionalListValidator<TIn, TOut>(target, RowName, ColumnName);
        }
    }

    [Obsolete]
    internal class RestrictedCapacityTwoDimensionalListValidator<T> : WodiLibTwoDimensionalListValidator<T>
    {
        protected override ITwoDimensionalListValidator<T> BaseValidator { get; }

        public RestrictedCapacityTwoDimensionalListValidator(ITwoDimensionalList<T> target,
            string rowName, string columnName) : base(target, rowName, columnName)
        {
            BaseValidator = new CommonTwoDimensionalListValidator<T>(target);
        }

        public override void Constructor(T[][] initItems)
        {
#if DEBUG
            RestrictedCapacityTwoDimensionalListValidationHelper.CapacityConfig(Target.GetMinRowCapacity(),
                Target.GetMaxRowCapacity(), Target.GetMinColumnCapacity(), Target.GetMaxColumnCapacity());
#endif
            BaseValidator.Constructor(initItems);
            RestrictedCapacityTwoDimensionalListValidationHelper.RowAndColCount(initItems,
                Target.GetMinRowCapacity(), Target.GetMaxRowCapacity(), Target.GetMinColumnCapacity(),
                Target.GetMaxColumnCapacity(), RowName, ColumnName);
        }

        public override void InsertRow(int rowIndex, params IEnumerable<T>[] items)
        {
            BaseValidator.InsertRow(rowIndex, items);

            if (items.Length <= 0) return;

            RestrictedCapacityTwoDimensionalListValidationHelper.ItemMaxCount(
                Target.RowCount + items.Length, Target.GetMaxRowCapacity(), RowName);
        }

        public override void InsertColumn(int columnIndex, params IEnumerable<T>[] items)
        {
            BaseValidator.InsertColumn(columnIndex, items);

            if (items.Length <= 0) return;

            RestrictedCapacityTwoDimensionalListValidationHelper.ItemMaxCount(
                Target.ColumnCount + items.Length, Target.GetMaxColumnCapacity(), ColumnName);
        }

        public override void OverwriteRow(int rowIndex, params IEnumerable<T>[] items)
        {
            BaseValidator.OverwriteRow(rowIndex, items);

            var itemArrays = items.ToTwoDimensionalArray();

            if (Target.ColumnCount == 0 && items.Length > 0)
            {
                RestrictedCapacityTwoDimensionalListValidationHelper.ItemMaxCount(
                    itemArrays[0].Length,
                    Target.GetMaxColumnCapacity(), ColumnName);
            }

            if (rowIndex + items.Length > Target.RowCount)
            {
                RestrictedCapacityTwoDimensionalListValidationHelper.ItemMaxCount(
                    rowIndex + items.Length, Target.GetMaxRowCapacity(), RowName);
            }
        }

        public override void OverwriteColumn(int columnIndex, params IEnumerable<T>[] items)
        {
            BaseValidator.OverwriteColumn(columnIndex, items);

            if (columnIndex + items.Length > Target.ColumnCount)
            {
                RestrictedCapacityTwoDimensionalListValidationHelper.ItemMaxCount(
                    columnIndex + items.Length, Target.GetMaxColumnCapacity(), ColumnName);
            }
        }

        public override void RemoveRow(int rowIndex, int count)
        {
            BaseValidator.RemoveRow(rowIndex, count);

            RestrictedCapacityTwoDimensionalListValidationHelper.ItemMinCount(
                Target.RowCount - count, Target.GetMinRowCapacity(), RowName);
        }

        public override void RemoveColumn(int columnIndex, int count)
        {
            BaseValidator.RemoveColumn(columnIndex, count);

            RestrictedCapacityTwoDimensionalListValidationHelper.ItemMinCount(
                Target.ColumnCount - count, Target.GetMinColumnCapacity(), ColumnName);
        }

        public override void AdjustLength(int rowLength, int columnLength)
        {
            BaseValidator.AdjustLength(rowLength, columnLength);
            RestrictedCapacityTwoDimensionalListValidationHelper.ItemCount(rowLength,
                Target.GetMinRowCapacity(), Target.GetMaxRowCapacity(), Direction.Row);
            RestrictedCapacityTwoDimensionalListValidationHelper.ItemCount(columnLength,
                Target.GetMinColumnCapacity(), Target.GetMaxColumnCapacity(), Direction.Column);
        }

        public override void Reset(IEnumerable<IEnumerable<T>> initItems)
        {
            var itemArrays = initItems.ToTwoDimensionalArray();

            BaseValidator.Reset(itemArrays);

            RestrictedCapacityTwoDimensionalListValidationHelper.RowAndColCount(itemArrays,
                Target.GetMinRowCapacity(), Target.GetMaxRowCapacity(), Target.GetMinColumnCapacity(),
                Target.GetMaxColumnCapacity(), RowName, ColumnName);
        }

        public override ITwoDimensionalListValidator<T> CreateAnotherFor(ITwoDimensionalList<T> target)
        {
            return new RestrictedCapacityTwoDimensionalListValidator<T>(target, RowName, ColumnName);
        }
    }
}
