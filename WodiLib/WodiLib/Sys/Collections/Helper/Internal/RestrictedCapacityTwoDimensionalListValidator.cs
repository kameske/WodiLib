// ========================================
// Project Name : WodiLib
// File Name    : RestrictedCapacityTwoDimensionalListValidator.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using System.Linq;

namespace WodiLib.Sys.Collections
{
    internal class
        RestrictedCapacityTwoDimensionalListValidator<TRow, TItem> : WodiLibTwoDimensionalListValidator<TRow, TRow,
            TItem, TItem>
        where TRow : IEnumerable<TItem>
    {
        protected override ITwoDimensionalListValidator<TRow, TItem> BaseValidator { get; }

        public RestrictedCapacityTwoDimensionalListValidator(ITwoDimensionalList<TRow, TRow, TItem, TItem> target,
            string rowName, string columnName) : base(target, rowName, columnName)
        {
            BaseValidator = new CommonTwoDimensionalListValidator<TRow, TItem>(target);
        }

        public override void Constructor(TRow[] initItems)
        {
#if DEBUG
            RestrictedCapacityTwoDimensionalListValidationHelper.CapacityConfig(Target.GetMinRowCapacity(),
                Target.GetMaxRowCapacity(), Target.GetMinColumnCapacity(), Target.GetMaxColumnCapacity());
#endif
            BaseValidator.Constructor(initItems);
            RestrictedCapacityTwoDimensionalListValidationHelper.RowAndColCount(
                initItems.Cast<IEnumerable<TItem>>().ToTwoDimensionalArray(),
                Target.GetMinRowCapacity(), Target.GetMaxRowCapacity(), Target.GetMinColumnCapacity(),
                Target.GetMaxColumnCapacity(), RowName, ColumnName);
        }

        public override void InsertRow(int rowIndex, params TRow[] items)
        {
            BaseValidator.InsertRow(rowIndex, items);

            if (items.Length <= 0) return;

            RestrictedCapacityTwoDimensionalListValidationHelper.ItemMaxCount(
                Target.RowCount + items.Length, Target.GetMaxRowCapacity(), RowName);
        }

        public override void InsertColumn(int columnIndex, params IEnumerable<TItem>[] items)
        {
            BaseValidator.InsertColumn(columnIndex, items);

            if (items.Length <= 0) return;

            RestrictedCapacityTwoDimensionalListValidationHelper.ItemMaxCount(
                Target.ColumnCount + items.Length, Target.GetMaxColumnCapacity(), ColumnName);
        }

        public override void OverwriteRow(int rowIndex, params TRow[] items)
        {
            BaseValidator.OverwriteRow(rowIndex, items);

            var itemArrays = items.Cast<IEnumerable<TItem>>().ToTwoDimensionalArray();

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

        public override void OverwriteColumn(int columnIndex, params IEnumerable<TItem>[] items)
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

        public override void Reset(IEnumerable<TRow> initItems)
        {
            var itemArrays = initItems.ToArray();

            BaseValidator.Reset(itemArrays);

            var itemTwoDimArray = itemArrays.Cast<IEnumerable<TItem>>().ToTwoDimensionalArray();

            RestrictedCapacityTwoDimensionalListValidationHelper.RowAndColCount(itemTwoDimArray,
                Target.GetMinRowCapacity(), Target.GetMaxRowCapacity(), Target.GetMinColumnCapacity(),
                Target.GetMaxColumnCapacity(), RowName, ColumnName);
        }
    }
}
