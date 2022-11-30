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
        RestrictedCapacityTwoDimensionalListValidator<TRow, TItem> :
            WodiLibTwoDimensionalListValidatorTemplate<TRow, TItem>
        where TRow : IEnumerable<TItem>
    {
        protected override ITwoDimensionalListValidator<TRow, TItem> BaseValidator { get; }

        public RestrictedCapacityTwoDimensionalListValidator(
            ITwoDimensionalList<TRow, TItem> target,
            string rowName,
            string columnName
        ) : base(target, rowName, columnName)
        {
            BaseValidator = new CommonTwoDimensionalListValidator<TRow, TItem>(target);
        }

        public override void Constructor(NamedValue<IEnumerable<TRow>> initItems)
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
                initItems.Value.Cast<IEnumerable<TItem>>().ToTwoDimensionalArray(),
                Target.GetMinRowCapacity(),
                Target.GetMaxRowCapacity(),
                Target.GetMinColumnCapacity(),
                Target.GetMaxColumnCapacity(),
                RowName,
                ColumnName
            );
        }

        public override void InsertRow(NamedValue<int> rowIndex, NamedValue<IEnumerable<TRow>> items)
        {
            BaseValidator.InsertRow(rowIndex, items);

            var rowArrays = items.Value.ToArray();

            if (rowArrays.Length <= 0) return;

            RestrictedCapacityTwoDimensionalListValidationHelper.ItemMaxCount(
                Target.RowCount + rowArrays.Length,
                Target.GetMaxRowCapacity(),
                RowName
            );
        }

        public override void InsertColumn(
            NamedValue<int> columnIndex,
            NamedValue<IEnumerable<IEnumerable<TItem>>> items
        )
        {
            BaseValidator.InsertColumn(columnIndex, items);

            var itemArrays = items.Value.ToArray();

            if (itemArrays.Length <= 0) return;

            RestrictedCapacityTwoDimensionalListValidationHelper.ItemMaxCount(
                Target.ColumnCount + itemArrays.Length,
                Target.GetMaxColumnCapacity(),
                ColumnName
            );
        }

        public override void OverwriteRow(NamedValue<int> rowIndex, NamedValue<IEnumerable<TRow>> items)
        {
            BaseValidator.OverwriteRow(rowIndex, items);

            var itemArrays = items.Cast<IEnumerable<IEnumerable<TItem>>>().Value.ToTwoDimensionalArray();

            if (Target.ColumnCount == 0 && itemArrays.Length > 0)
            {
                RestrictedCapacityTwoDimensionalListValidationHelper.ItemMinCount(
                    itemArrays[0].Length,
                    Target.GetMinColumnCapacity(),
                    ColumnName
                );
                RestrictedCapacityTwoDimensionalListValidationHelper.ItemMaxCount(
                    itemArrays[0].Length,
                    Target.GetMaxColumnCapacity(),
                    ColumnName
                );
            }

            if (rowIndex.Value + itemArrays.Length > Target.RowCount)
            {
                RestrictedCapacityTwoDimensionalListValidationHelper.ItemMaxCount(
                    rowIndex.Value + itemArrays.Length,
                    Target.GetMaxRowCapacity(),
                    RowName
                );
            }
        }

        public override void OverwriteColumn(
            NamedValue<int> columnIndex,
            NamedValue<IEnumerable<IEnumerable<TItem>>> items
        )
        {
            BaseValidator.OverwriteColumn(columnIndex, items);

            var itemLength = items.Value.Count();
            if (columnIndex.Value + itemLength > Target.RowCount)
            {
                RestrictedCapacityTwoDimensionalListValidationHelper.ItemMaxCount(
                    columnIndex.Value + itemLength,
                    Target.GetMaxColumnCapacity(),
                    RowName
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

        public override void Reset(NamedValue<IEnumerable<TRow>> initItems)
        {
            BaseValidator.Reset(initItems);

            var itemTwoDimArray = initItems.Value.Cast<IEnumerable<TItem>>().ToTwoDimensionalArray();

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
}
