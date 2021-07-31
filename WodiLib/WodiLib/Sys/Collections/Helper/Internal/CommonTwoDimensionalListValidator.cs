// ========================================
// Project Name : WodiLib
// File Name    : CommonTwoDimensionalListValidator.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     二次元リスト編集メソッドの引数汎用検証処理実施クラス
    /// </summary>
    /// <typeparam name="T">リスト内包型</typeparam>
    internal class CommonTwoDimensionalListValidator<T> : WodiLibTwoDimensionalListValidator<T>
    {
        protected override ITwoDimensionalListValidator<T>? BaseValidator => null;

        private string RowIndexName { get; }
        private string ColumnIndexName { get; }

        public CommonTwoDimensionalListValidator(ITwoDimensionalList<T> target,
            string rowName = "行", string rowIndexName = "row",
            string columnName = "列", string columnIndexName = "column") : base(target, rowName, columnName)
        {
            RowIndexName = rowIndexName;
            ColumnIndexName = columnIndexName;
        }

        public override void Constructor(T[][] initItems)
        {
            ThrowHelper.ValidateArgumentNotNull(initItems is null, nameof(initItems));

            TwoDimensionalListValidationHelper.ItemNotNull(initItems);
            TwoDimensionalListValidationHelper.InnerItemLength(initItems);
        }

        public override void GetRow(int rowIndex, int rowCount)
        {
            ListValidationHelper.SelectIndex(rowIndex, Target.RowCount, nameof(rowIndex));
            ListValidationHelper.Count(rowCount, Target.RowCount, nameof(rowCount));
            ListValidationHelper.Range(rowIndex, rowCount, Target.RowCount, nameof(rowIndex), nameof(rowCount));
        }

        public override void GetColumn(int columnIndex, int columnCount)
        {
            ListValidationHelper.SelectIndex(columnIndex, Target.ColumnCount, nameof(columnIndex));
            ListValidationHelper.Count(columnCount, Target.ColumnCount, nameof(columnCount));
            ListValidationHelper.Range(columnIndex, columnCount, Target.ColumnCount, nameof(columnIndex),
                nameof(columnCount));
        }

        public override void GetItem(int rowIndex, int rowCount, int columnIndex, int columnCount)
        {
            ListValidationHelper.SelectIndex(rowIndex, Target.RowCount, nameof(rowIndex));
            ListValidationHelper.Count(rowCount, Target.RowCount, nameof(rowCount));
            ListValidationHelper.Range(rowIndex, rowCount, Target.RowCount, nameof(rowIndex), nameof(rowCount));
            ListValidationHelper.SelectIndex(columnIndex, Target.ColumnCount, nameof(columnIndex));
            ListValidationHelper.Count(columnCount, Target.ColumnCount, nameof(columnCount));
            ListValidationHelper.Range(columnIndex, columnCount, Target.ColumnCount, nameof(columnIndex),
                nameof(columnCount));
        }

        public override void GetItem(int rowIndex, int columnIndex)
        {
            ListValidationHelper.SelectIndex(rowIndex, Target.RowCount, nameof(rowIndex));
            ListValidationHelper.SelectIndex(columnIndex, Target.ColumnCount, nameof(columnIndex));
        }

        public override void SetRow(int rowIndex, params IEnumerable<T>[] rows)
        {
            ListValidationHelper.SelectIndex(rowIndex, Target.RowCount, nameof(rowIndex));

            ThrowHelper.ValidateArgumentNotNull(rows is null, nameof(rows));

            var rowArrays = rows.ToTwoDimensionalArray();

            TwoDimensionalListValidationHelper.ItemNotNull(rowArrays, nameof(rows));
            TwoDimensionalListValidationHelper.InnerItemLength(rowArrays);

            ListValidationHelper.Range(rowIndex, rowArrays.Length, Target.RowCount);

            var columnLength = rowArrays.GetInnerArrayLength();
            TwoDimensionalListValidationHelper.SizeEqual(columnLength, Target.ColumnCount);
        }

        public override void SetColumn(int columnIndex, params IEnumerable<T>[] items)
        {
            ListValidationHelper.SelectIndex(columnIndex, Target.ColumnCount, nameof(columnIndex));

            ThrowHelper.ValidateArgumentNotNull(items is null, nameof(items));

            var itemArrays = items.ToTwoDimensionalArray();

            TwoDimensionalListValidationHelper.ItemNotNull(itemArrays, nameof(items));
            TwoDimensionalListValidationHelper.InnerItemLength(itemArrays);

            ListValidationHelper.Range(columnIndex, items.Length, Target.ColumnCount);

            var rowLength = itemArrays.GetInnerArrayLength();
            TwoDimensionalListValidationHelper.SizeEqual(rowLength, Target.RowCount);
        }

        public override void SetItem(int rowIndex, int columnIndex, T item)
        {
            ListValidationHelper.SelectIndex(rowIndex, Target.RowCount, nameof(rowIndex));
            ListValidationHelper.SelectIndex(columnIndex, Target.ColumnCount, nameof(columnIndex));
            ThrowHelper.ValidateArgumentNotNull(item is null, nameof(item));
        }

        public override void InsertRow(int rowIndex, params IEnumerable<T>[] items)
        {
            ListValidationHelper.InsertIndex(rowIndex, Target.RowCount, RowIndexName);

            ThrowHelper.ValidateArgumentNotNull(items is null, nameof(items));

            var itemArrays = items.ToTwoDimensionalArray();

            TwoDimensionalListValidationHelper.InnerItemLength(itemArrays);
            TwoDimensionalListValidationHelper.ItemNotNull(itemArrays, nameof(items));

            if (!Target.IsEmpty && items.Length > 0)
            {
                TwoDimensionalListValidationHelper.SizeEqual(itemArrays[0].Length,
                    Target.ColumnCount, ColumnIndexName);
            }
        }

        public override void InsertColumn(int columnIndex, params IEnumerable<T>[] items)
        {
            ListValidationHelper.InsertIndex(columnIndex, Target.ColumnCount, ColumnIndexName);

            ThrowHelper.ValidateArgumentNotNull(items is null, nameof(items));

            var itemArrays = items.ToTwoDimensionalArray();

            TwoDimensionalListValidationHelper.InnerItemLength(itemArrays);
            TwoDimensionalListValidationHelper.ItemNotNull(itemArrays, nameof(items));

            if (!Target.IsEmpty && items.Length > 0)
            {
                TwoDimensionalListValidationHelper.SizeEqual(itemArrays[0].Length,
                    Target.RowCount, RowIndexName);
            }
        }

        public override void OverwriteRow(int rowIndex, params IEnumerable<T>[] items)
        {
            ListValidationHelper.InsertIndex(rowIndex, Target.RowCount, RowIndexName);

            ThrowHelper.ValidateArgumentNotNull(items is null, nameof(items));

            var itemArrays = items.ToTwoDimensionalArray();

            TwoDimensionalListValidationHelper.InnerItemLength(itemArrays);
            TwoDimensionalListValidationHelper.ItemNotNull(itemArrays, nameof(items));

            if (!Target.IsEmpty && items.Length > 0)
            {
                TwoDimensionalListValidationHelper.SizeEqual(itemArrays[0].Length,
                    Target.ColumnCount, ColumnIndexName);
            }
        }

        public override void OverwriteColumn(int columnIndex, params IEnumerable<T>[] items)
        {
            ListValidationHelper.InsertIndex(columnIndex, Target.ColumnCount, ColumnIndexName);

            ThrowHelper.ValidateArgumentNotNull(items is null, nameof(items));

            var itemArrays = items.ToTwoDimensionalArray();

            TwoDimensionalListValidationHelper.InnerItemLength(itemArrays);
            TwoDimensionalListValidationHelper.ItemNotNull(itemArrays, nameof(items));

            if (!Target.IsEmpty && items.Length > 0)
            {
                TwoDimensionalListValidationHelper.SizeEqual(itemArrays[0].Length,
                    Target.RowCount, RowIndexName);
            }
        }

        public override void MoveRow(int oldRowIndex, int newRowIndex, int count)
        {
            TwoDimensionalListValidationHelper.LengthNotZero(Target.RowCount, RowIndexName);
            ListValidationHelper.SelectIndex(oldRowIndex, Target.RowCount, nameof(oldRowIndex));
            ListValidationHelper.InsertIndex(newRowIndex, Target.RowCount, nameof(newRowIndex));
            ListValidationHelper.Count(count, Target.RowCount);
            ListValidationHelper.Range(oldRowIndex, count, Target.RowCount, nameof(oldRowIndex));
            ListValidationHelper.Range(count, newRowIndex, Target.RowCount, nameof(newRowIndex));
        }

        public override void MoveColumn(int oldColumnIndex, int newColumnIndex, int count)
        {
            TwoDimensionalListValidationHelper.LengthNotZero(Target.ColumnCount, ColumnIndexName);
            ListValidationHelper.SelectIndex(oldColumnIndex, Target.ColumnCount, nameof(oldColumnIndex));
            ListValidationHelper.InsertIndex(newColumnIndex, Target.ColumnCount, nameof(newColumnIndex));
            ListValidationHelper.Count(count, Target.ColumnCount);
            ListValidationHelper.Range(oldColumnIndex, count, Target.ColumnCount, nameof(oldColumnIndex));
            ListValidationHelper.Range(count, newColumnIndex, Target.ColumnCount, nameof(newColumnIndex));
        }

        public override void RemoveRow(int rowIndex, int count)
        {
            ValidateTargetIsEmpty();
            ListValidationHelper.SelectIndex(rowIndex, Target.RowCount, RowIndexName);
            ListValidationHelper.Count(count, Target.RowCount);
            ListValidationHelper.Range(rowIndex, count, Target.RowCount, RowIndexName);
        }

        public override void RemoveColumn(int columnIndex, int count)
        {
            ValidateTargetIsEmpty();
            ListValidationHelper.SelectIndex(columnIndex, Target.ColumnCount, ColumnIndexName);
            ListValidationHelper.Count(count, Target.ColumnCount);
            ListValidationHelper.Range(columnIndex, count, Target.ColumnCount, ColumnIndexName);
        }

        public override void AdjustLength(int rowLength, int columnLength)
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
                $"{RowName}数および{ColumnName}数の指定", $"{RowName}数 == 0 かつ {ColumnName}数 != 0 の指定はできません");
        }

        public override void Reset(IEnumerable<IEnumerable<T>> items)
        {
            ThrowHelper.ValidateArgumentNotNull(items is null, nameof(items));
            var itemArray = items.ToTwoDimensionalArray();

            TwoDimensionalListValidationHelper.ItemNotNull(itemArray);
            TwoDimensionalListValidationHelper.InnerItemLength(itemArray);
        }

        public override ITwoDimensionalListValidator<T> CreateAnotherFor(
            ITwoDimensionalList<T> target)
            => new CommonTwoDimensionalListValidator<T>(target, RowName, RowIndexName, ColumnName, ColumnIndexName);

        private void ValidateTargetIsEmpty()
        {
            ThrowHelper.InvalidOperationIf(Target.IsEmpty,
                () => ErrorMessage.NotExecute("空リストのため"));
        }
    }
}
