// ========================================
// Project Name : WodiLib
// File Name    : CommonTwoDimensionalListValidator.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     二次元リスト編集メソッドの引数汎用検証処理実施クラス
    /// </summary>
    /// <typeparam name="T">リスト内包型</typeparam>
    internal class CommonTwoDimensionalListValidator<T> : WodiLibTwoDimensionalListValidator<T>
    {
        protected override ITwoDimensionalListValidator<T>? BaseValidator => null;

        private new IReadOnlyTwoDimensionalList<T> Target { get; }

        public CommonTwoDimensionalListValidator(IReadOnlyTwoDimensionalList<T> target) : base(target)
        {
            Target = target;
        }

        public override void Constructor(T[][] initItems)
        {
            TwoDimensionalListValidationHelper.ItemNotNull(initItems);
            TwoDimensionalListValidationHelper.InnerItemLength(initItems);
        }

        public override void Get(int row, int rowCount, int column, int columnCount)
        {
            ListValidationHelper.SelectIndex(row, Target.RowCount, nameof(row));
            ListValidationHelper.Count(rowCount, Target.RowCount, nameof(rowCount));
            ListValidationHelper.Range(row, rowCount, Target.RowCount, nameof(row), nameof(rowCount));
            ListValidationHelper.SelectIndex(column, Target.ColumnCount, nameof(column));
            ListValidationHelper.Count(columnCount, Target.ColumnCount, nameof(columnCount));
            ListValidationHelper.Range(column, columnCount, Target.ColumnCount, nameof(column), nameof(columnCount));
        }

        public override void GetRow(int row, int count)
        {
            ListValidationHelper.SelectIndex(row, Target.RowCount, nameof(row));
            ListValidationHelper.Count(count, Target.RowCount);
            ListValidationHelper.Range(row, count, Target.RowCount, nameof(row));
        }

        public override void GetColumn(int column, int count)
        {
            ValidateTargetIsEmpty();
            ListValidationHelper.SelectIndex(column, Target.ColumnCount, nameof(column));
            ListValidationHelper.Count(count, Target.ColumnCount);
            ListValidationHelper.Range(column, count, Target.ColumnCount, nameof(column));
        }

        public override void Set(int row, int column, T[][] items)
        {
            ListValidationHelper.SelectIndex(row, Target.RowCount, nameof(row));
            ListValidationHelper.SelectIndex(column, Target.ColumnCount, nameof(column));
            TwoDimensionalListValidationHelper.ItemNotNull(items, nameof(items));
            TwoDimensionalListValidationHelper.InnerItemLength(items);
            ListValidationHelper.Range(row, items.Length, Target.RowCount);
            if (items.Length > 0)
            {
                ListValidationHelper.Range(column, items[0].Length, Target.ColumnCount);
            }
        }

        public override void InsertRow(int row, T[][] items)
        {
            ListValidationHelper.InsertIndex(row, Target.RowCount, nameof(row));
            TwoDimensionalListValidationHelper.InnerItemLength(items);
            TwoDimensionalListValidationHelper.ItemNotNull(items, nameof(items));
            if (!Target.IsEmpty && items.Length > 0)
            {
                TwoDimensionalListValidationHelper.SizeEqual(items[0].Length, Target.ColumnCount,
                    countName: nameof(Target.ColumnCount));
            }
        }

        public override void InsertColumn(int column, T[][] items)
        {
            ValidateTargetIsEmpty();
            ListValidationHelper.InsertIndex(column, Target.ColumnCount, nameof(column));
            TwoDimensionalListValidationHelper.InnerItemLength(items);
            TwoDimensionalListValidationHelper.ItemNotNull(items, nameof(items));
            if (items.Length > 0)
            {
                TwoDimensionalListValidationHelper.SizeEqual(items[0].Length, Target.RowCount,
                    countName: nameof(Target.RowCount));
            }
        }

        public override void OverwriteRow(int row, T[][] items)
        {
            ListValidationHelper.InsertIndex(row, Target.RowCount, nameof(row));
            TwoDimensionalListValidationHelper.InnerItemLength(items);
            TwoDimensionalListValidationHelper.ItemNotNull(items, nameof(items));
            if (!Target.IsEmpty && items.Length > 0)
            {
                TwoDimensionalListValidationHelper.SizeEqual(items[0].Length, Target.ColumnCount,
                    countName: nameof(Target.ColumnCount));
            }
        }

        public override void OverwriteColumn(int column, T[][] items)
        {
            ValidateTargetIsEmpty();
            ListValidationHelper.InsertIndex(column, Target.ColumnCount, nameof(column));
            TwoDimensionalListValidationHelper.InnerItemLength(items);
            TwoDimensionalListValidationHelper.ItemNotNull(items, nameof(items));
            if (!Target.IsEmpty && items.Length > 0)
            {
                TwoDimensionalListValidationHelper.SizeEqual(items[0].Length, Target.RowCount,
                    countName: nameof(Target.RowCount));
            }
        }

        public override void MoveRow(int oldRow, int newRow, int count)
        {
            ValidateTargetIsEmpty();
            TwoDimensionalListValidationHelper.LengthNotZero(Target.RowCount, Direction.Row);
            ListValidationHelper.SelectIndex(oldRow, Target.RowCount, nameof(oldRow));
            ListValidationHelper.InsertIndex(newRow, Target.RowCount, nameof(newRow));
            ListValidationHelper.Count(count, Target.RowCount);
            ListValidationHelper.Range(oldRow, count, Target.RowCount, nameof(oldRow));
            ListValidationHelper.Range(count, newRow, Target.RowCount, nameof(count), nameof(newRow));
        }

        public override void MoveColumn(int oldColumn, int newColumn, int count)
        {
            ValidateTargetIsEmpty();
            TwoDimensionalListValidationHelper.LengthNotZero(Target.ColumnCount, Direction.Column);
            ListValidationHelper.SelectIndex(oldColumn, Target.ColumnCount, nameof(oldColumn));
            ListValidationHelper.InsertIndex(newColumn, Target.ColumnCount, nameof(newColumn));
            ListValidationHelper.Count(count, Target.ColumnCount);
            ListValidationHelper.Range(oldColumn, count, Target.ColumnCount, nameof(oldColumn));
            ListValidationHelper.Range(count, newColumn, Target.ColumnCount, nameof(count), nameof(newColumn));
        }

        public override void RemoveRow(int row, int count)
        {
            ValidateTargetIsEmpty();
            ListValidationHelper.SelectIndex(row, Target.RowCount, nameof(row));
            ListValidationHelper.Count(count, Target.RowCount);
            ListValidationHelper.Range(row, count, Target.RowCount, nameof(row));
        }

        public override void RemoveColumn(int column, int count)
        {
            ValidateTargetIsEmpty();
            ListValidationHelper.SelectIndex(column, Target.ColumnCount, nameof(column));
            ListValidationHelper.Count(count, Target.ColumnCount);
            ListValidationHelper.Range(column, count, Target.ColumnCount, nameof(column));
        }

        public override void AdjustLength(int rowLength, int columnLength)
        {
            ValidateSquareSize(rowLength, columnLength);
        }

        public override void AdjustLengthIfShort(int rowLength, int columnLength)
        {
            ValidateSquareSize(rowLength, columnLength);
        }

        public override void AdjustLengthIfLong(int rowLength, int columnLength)
        {
            ValidateSquareSize(rowLength, columnLength);
        }

        public override void AdjustRowLength(int length)
        {
            ThrowHelper.ValidateArgumentValueGreaterOrEqual(length < 0,
                nameof(length), 0, length);
        }

        public override void AdjustColumnLength(int length)
        {
            ValidateTargetIsEmpty();
            ThrowHelper.ValidateArgumentValueGreaterOrEqual(length < 0,
                nameof(length), 0, length);
        }

        public override void AdjustRowLengthIfShort(int length)
        {
            ThrowHelper.ValidateArgumentValueGreaterOrEqual(length < 0,
                nameof(length), 0, length);
        }

        public override void AdjustColumnLengthIfShort(int length)
        {
            ValidateTargetIsEmpty();
            ThrowHelper.ValidateArgumentValueGreaterOrEqual(length < 0,
                nameof(length), 0, length);
        }

        public override void AdjustRowLengthIfLong(int length)
        {
            ThrowHelper.ValidateArgumentValueGreaterOrEqual(length < 0,
                nameof(length), 0, length);
        }

        public override void AdjustColumnLengthIfLong(int length)
        {
            ValidateTargetIsEmpty();
            ThrowHelper.ValidateArgumentValueGreaterOrEqual(length < 0,
                nameof(length), 0, length);
        }

        public override void Reset(T[][] items)
        {
            TwoDimensionalListValidationHelper.ItemNotNull(items);
            TwoDimensionalListValidationHelper.InnerItemLength(items);
        }

        private void ValidateTargetIsEmpty()
        {
            ThrowHelper.InvalidOperationIf(Target.IsEmpty,
                () => ErrorMessage.NotExecute("空リストのため"));
        }

        private static void ValidateSquareSize(int rowLength, int columnLength)
        {
            ThrowHelper.ValidateArgumentValueGreaterOrEqual(rowLength < 0,
                nameof(rowLength), 0, rowLength);
            ThrowHelper.ValidateArgumentValueGreaterOrEqual(columnLength < 0,
                nameof(columnLength), 0, columnLength);

            if (rowLength > 0) return;
            // rowLength == 0

            if (columnLength == 0) return;

            // rowLength == 0 && columnLength != 0 は不正な指定
            ThrowHelper.ValidateArgumentUnsuitable(true,
                "行数および列数の指定", "行数 == 0 かつ 列数 != 0 の指定はできません");
        }
    }
}
