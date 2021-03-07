// ========================================
// Project Name : WodiLib
// File Name    : RestrictedCapacityTwoDimensionalListValidator.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================


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
            RestrictedCapacityTwoDimensionalListValidationHelper.CapacityConfig(Target.GetMinRowCapacity(),
                Target.GetMaxRowCapacity(), Target.GetMinColumnCapacity(), Target.GetMaxColumnCapacity());
#endif
            PreConditionValidator.Constructor(initItems);
            RestrictedCapacityTwoDimensionalListValidationHelper.RowAndColCount(initItems,
                Target.GetMinRowCapacity(), Target.GetMaxRowCapacity(), Target.GetMinColumnCapacity(),
                Target.GetMaxColumnCapacity());
        }

        public void Get(int row, int rowCount, int column, int columnCount)
        {
            PreConditionValidator.Get(row, rowCount, column, columnCount);
        }

        public void GetRow(int row, int count)
        {
            PreConditionValidator.GetRow(row, count);
        }

        public void GetColumn(int column, int count)
        {
            PreConditionValidator.GetColumn(column, count);
        }

        public void Set(int row, int column, T[][] items)
        {
            PreConditionValidator.Set(row, column, items);
        }

        public void InsertRow(int row, T[][] items)
        {
            PreConditionValidator.InsertRow(row, items);
            if (Target.ColumnCount == 0 && items.Length > 0)
            {
                RestrictedCapacityTwoDimensionalListValidationHelper.ItemMaxCount(
                    items[0].Length,
                    Target.GetMaxColumnCapacity(), Direction.Column);
            }

            RestrictedCapacityTwoDimensionalListValidationHelper.ItemMaxCount(
                Target.RowCount + items.Length,
                Target.GetMaxRowCapacity(), Direction.Row);
        }

        public void InsertColumn(int column, T[][] items)
        {
            PreConditionValidator.InsertColumn(column, items);
            if (Target.RowCount == 0 && items.Length > 0)
            {
                RestrictedCapacityTwoDimensionalListValidationHelper.ItemMaxCount(
                    items[0].Length,
                    Target.GetMaxRowCapacity(), Direction.Row);
            }

            RestrictedCapacityTwoDimensionalListValidationHelper.ItemMaxCount(
                Target.ColumnCount + items.Length,
                Target.GetMaxColumnCapacity(), Direction.Column);
        }

        public void OverwriteRow(int row, T[][] items)
        {
            PreConditionValidator.OverwriteRow(row, items);
            if (Target.ColumnCount == 0 && items.Length > 0)
            {
                RestrictedCapacityTwoDimensionalListValidationHelper.ItemMaxCount(
                    items[0].Length,
                    Target.GetMaxColumnCapacity(), Direction.Column);
            }

            if (row + items.Length > Target.RowCount)
            {
                RestrictedCapacityTwoDimensionalListValidationHelper.ItemMaxCount(
                    row + items.Length,
                    Target.GetMaxRowCapacity(), Direction.Row);
            }
        }

        public void OverwriteColumn(int column, T[][] items)
        {
            PreConditionValidator.OverwriteColumn(column, items);
            if (Target.RowCount == 0 && items.Length > 0)
            {
                RestrictedCapacityTwoDimensionalListValidationHelper.ItemMaxCount(
                    items[0].Length,
                    Target.GetMaxRowCapacity(), Direction.Row);
            }

            if (column + items.Length > Target.ColumnCount)
            {
                RestrictedCapacityTwoDimensionalListValidationHelper.ItemMaxCount(
                    column + items.Length,
                    Target.GetMaxColumnCapacity(), Direction.Column);
            }
        }

        public void MoveRow(int oldRow, int newRow, int count)
        {
            PreConditionValidator.MoveRow(oldRow, newRow, count);
        }

        public void MoveColumn(int oldColumn, int newColumn, int count)
        {
            PreConditionValidator.MoveColumn(oldColumn, newColumn, count);
        }

        public void RemoveRow(int row, int count)
        {
            PreConditionValidator.RemoveRow(row, count);
            RestrictedCapacityTwoDimensionalListValidationHelper.ItemMinCount(
                Target.RowCount - count, Target.GetMinRowCapacity(), Direction.Row);
        }

        public void RemoveColumn(int column, int count)
        {
            PreConditionValidator.RemoveColumn(column, count);
            RestrictedCapacityTwoDimensionalListValidationHelper.ItemMinCount(
                Target.ColumnCount - count, Target.GetMinColumnCapacity(), Direction.Column);
        }

        public void AdjustLength(int rowLength, int columnLength)
        {
            PreConditionValidator.AdjustLength(rowLength, columnLength);
            RestrictedCapacityTwoDimensionalListValidationHelper.ItemCount(rowLength,
                Target.GetMinRowCapacity(), Target.GetMaxRowCapacity(), Direction.Row);
            RestrictedCapacityTwoDimensionalListValidationHelper.ItemCount(columnLength,
                Target.GetMinColumnCapacity(), Target.GetMaxColumnCapacity(), Direction.Column);
        }

        public void AdjustLengthIfShort(int rowLength, int columnLength)
        {
            PreConditionValidator.AdjustLengthIfShort(rowLength, columnLength);
            RestrictedCapacityTwoDimensionalListValidationHelper.ItemCount(rowLength,
                Target.GetMinRowCapacity(), Target.GetMaxRowCapacity(), Direction.Row);
            RestrictedCapacityTwoDimensionalListValidationHelper.ItemCount(columnLength,
                Target.GetMinColumnCapacity(), Target.GetMaxColumnCapacity(), Direction.Column);
        }

        public void AdjustLengthIfLong(int rowLength, int columnLength)
        {
            PreConditionValidator.AdjustLengthIfLong(rowLength, columnLength);
            RestrictedCapacityTwoDimensionalListValidationHelper.ItemCount(rowLength,
                Target.GetMinRowCapacity(), Target.GetMaxRowCapacity(), Direction.Row);
            RestrictedCapacityTwoDimensionalListValidationHelper.ItemCount(columnLength,
                Target.GetMinColumnCapacity(), Target.GetMaxColumnCapacity(), Direction.Column);
        }

        public void AdjustRowLength(int length)
        {
            PreConditionValidator.AdjustRowLength(length);
            RestrictedCapacityTwoDimensionalListValidationHelper.ItemCount(length, Target.GetMinRowCapacity(),
                Target.GetMaxRowCapacity(), Direction.Row);
        }

        public void AdjustColumnLength(int length)
        {
            PreConditionValidator.AdjustColumnLength(length);
            RestrictedCapacityTwoDimensionalListValidationHelper.ItemCount(length, Target.GetMinColumnCapacity(),
                Target.GetMaxColumnCapacity(), Direction.Column);
        }

        public void AdjustRowLengthIfShort(int length)
        {
            PreConditionValidator.AdjustRowLengthIfShort(length);
            RestrictedCapacityTwoDimensionalListValidationHelper.ItemCount(length, Target.GetMinRowCapacity(),
                Target.GetMaxRowCapacity(), Direction.Row);
        }

        public void AdjustColumnLengthIfShort(int length)
        {
            PreConditionValidator.AdjustColumnLengthIfShort(length);
            RestrictedCapacityTwoDimensionalListValidationHelper.ItemCount(length, Target.GetMinColumnCapacity(),
                Target.GetMaxColumnCapacity(), Direction.Column);
        }

        public void AdjustRowLengthIfLong(int length)
        {
            PreConditionValidator.AdjustRowLengthIfLong(length);
            RestrictedCapacityTwoDimensionalListValidationHelper.ItemCount(length, Target.GetMinRowCapacity(),
                Target.GetMaxRowCapacity(), Direction.Row);
        }

        public void AdjustColumnLengthIfLong(int length)
        {
            PreConditionValidator.AdjustColumnLengthIfLong(length);
            RestrictedCapacityTwoDimensionalListValidationHelper.ItemCount(length, Target.GetMinColumnCapacity(),
                Target.GetMaxColumnCapacity(), Direction.Column);
        }

        public void Reset(T[][] items)
        {
            PreConditionValidator.Reset(items);
            RestrictedCapacityTwoDimensionalListValidationHelper.RowAndColCount(items, Target.GetMinRowCapacity(),
                Target.GetMaxRowCapacity(), Target.GetMinColumnCapacity(), Target.GetMaxColumnCapacity(),
                nameof(items));
        }
    }
}
