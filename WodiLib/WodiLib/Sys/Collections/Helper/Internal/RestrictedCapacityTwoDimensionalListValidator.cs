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
        RestrictedCapacityTwoDimensionalListValidator<TInRow, TOutRow, TInItem, TOutItem> :
            WodiLibTwoDimensionalListValidator<TInRow, TOutRow,
                TInItem, TOutItem>
        where TInRow : IEnumerable<TInItem>
        where TOutRow : IEnumerable<TOutItem>, TInRow
        where TOutItem : TInItem
    {
        protected override ITwoDimensionalListValidator<TInRow, TInItem> BaseValidator { get; }

        public RestrictedCapacityTwoDimensionalListValidator(
            ITwoDimensionalList<TInRow, TOutRow, TInItem, TOutItem> target,
            string rowName,
            string columnName
        ) : base(target, rowName, columnName)
        {
            BaseValidator = new CommonTwoDimensionalListValidator<TInRow, TOutRow, TInItem, TOutItem>(target);
        }

        public override void Constructor(NamedValue<TInRow[]> initItems)
        {
#if DEBUG
            RestrictedCapacityTwoDimensionalListValidationHelper.CapacityConfig(
                ($"{Target.GetType().FullName}.{nameof(Target.GetMinRowCapacity)}", Target.GetMinRowCapacity()),
                ($"{Target.GetType().FullName}.{nameof(Target.GetMaxRowCapacity)}", Target.GetMaxRowCapacity()),
                ($"{Target.GetType().FullName}.{nameof(Target.GetMinColumnCapacity)}", Target.GetMinColumnCapacity()),
                ($"{Target.GetType().FullName}.{nameof(Target.GetMaxColumnCapacity)}", Target.GetMaxColumnCapacity())
            );
#endif
            BaseValidator.Constructor(initItems);
            RestrictedCapacityTwoDimensionalListValidationHelper.RowAndColCount(
                initItems.Value.Cast<IEnumerable<TInItem>>().ToTwoDimensionalArray(),
                Target.GetMinRowCapacity(),
                Target.GetMaxRowCapacity(),
                Target.GetMinColumnCapacity(),
                Target.GetMaxColumnCapacity(),
                RowName,
                ColumnName
            );
        }

        public override void InsertRow(NamedValue<int> rowIndex, string targetParamName, params TInRow[] items)
        {
            BaseValidator.InsertRow(rowIndex, targetParamName, items);

            if (items.Length <= 0) return;

            RestrictedCapacityTwoDimensionalListValidationHelper.ItemMaxCount(
                Target.RowCount + items.Length,
                Target.GetMaxRowCapacity(),
                RowName
            );
        }

        public override void InsertColumn(
            NamedValue<int> columnIndex,
            string targetParamName,
            params IEnumerable<TInItem>[] items
        )
        {
            BaseValidator.InsertColumn(columnIndex, targetParamName, items);

            if (items.Length <= 0) return;

            RestrictedCapacityTwoDimensionalListValidationHelper.ItemMaxCount(
                Target.ColumnCount + items.Length,
                Target.GetMaxColumnCapacity(),
                ColumnName
            );
        }

        public override void OverwriteRow(NamedValue<int> rowIndex, string targetParamName, params TInRow[] items)
        {
            BaseValidator.OverwriteRow(rowIndex, targetParamName, items);

            var itemArrays = items.Cast<IEnumerable<TInItem>>().ToTwoDimensionalArray();

            if (Target.ColumnCount == 0 && items.Length > 0)
            {
                RestrictedCapacityTwoDimensionalListValidationHelper.ItemMaxCount(
                    itemArrays[0].Length,
                    Target.GetMaxColumnCapacity(),
                    ColumnName
                );
            }

            if (rowIndex.Value + items.Length > Target.RowCount)
            {
                RestrictedCapacityTwoDimensionalListValidationHelper.ItemMaxCount(
                    rowIndex.Value + items.Length,
                    Target.GetMaxRowCapacity(),
                    RowName
                );
            }
        }

        public override void OverwriteColumn(
            NamedValue<int> columnIndex,
            string targetParamName,
            params IEnumerable<TInItem>[] items
        )
        {
            BaseValidator.OverwriteColumn(columnIndex, targetParamName, items);

            if (columnIndex.Value + items.Length > Target.ColumnCount)
            {
                RestrictedCapacityTwoDimensionalListValidationHelper.ItemMaxCount(
                    columnIndex.Value + items.Length,
                    Target.GetMaxColumnCapacity(),
                    ColumnName
                );
            }
        }

        public override void RemoveRow(NamedValue<int> rowIndex, NamedValue<int> count)
        {
            BaseValidator.RemoveRow(rowIndex, count);

            RestrictedCapacityTwoDimensionalListValidationHelper.ItemMinCount(
                Target.RowCount - count.Value,
                Target.GetMinRowCapacity(),
                RowName
            );
        }

        public override void RemoveColumn(NamedValue<int> columnIndex, NamedValue<int> count)
        {
            BaseValidator.RemoveColumn(columnIndex, count);

            RestrictedCapacityTwoDimensionalListValidationHelper.ItemMinCount(
                Target.ColumnCount - count.Value,
                Target.GetMinColumnCapacity(),
                ColumnName
            );
        }

        public override void AdjustLength(NamedValue<int> rowLength, NamedValue<int> columnLength)
        {
            BaseValidator.AdjustLength(rowLength, columnLength);
            RestrictedCapacityTwoDimensionalListValidationHelper.ItemCount(
                rowLength.Value,
                Target.GetMinRowCapacity(),
                Target.GetMaxRowCapacity(),
                RowName
            );
            RestrictedCapacityTwoDimensionalListValidationHelper.ItemCount(
                columnLength.Value,
                Target.GetMinColumnCapacity(),
                Target.GetMaxColumnCapacity(),
                ColumnName
            );
        }

        public override void Reset(NamedValue<IEnumerable<TInRow>> initItems)
        {
            BaseValidator.Reset(initItems);

            var itemTwoDimArray = initItems.Value.Cast<IEnumerable<TInItem>>().ToTwoDimensionalArray();

            RestrictedCapacityTwoDimensionalListValidationHelper.RowAndColCount(
                itemTwoDimArray,
                Target.GetMinRowCapacity(),
                Target.GetMaxRowCapacity(),
                Target.GetMinColumnCapacity(),
                Target.GetMaxColumnCapacity(),
                RowName,
                ColumnName
            );
        }
    }

    internal class RestrictedCapacityTwoDimensionalListValidator<TRow, TItem> :
        RestrictedCapacityTwoDimensionalListValidator<TRow, TRow, TItem, TItem>
        where TRow : IEnumerable<TItem>
    {
        public RestrictedCapacityTwoDimensionalListValidator(
            ITwoDimensionalList<TRow, TRow, TItem, TItem> target,
            string rowName,
            string columnName
        ) : base(target, rowName, columnName)
        {
        }
    }
}
