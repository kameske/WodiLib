// ========================================
// Project Name : WodiLib
// File Name    : CommonTwoDimensionalListValidator.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using System.Linq;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     二次元リスト編集メソッドの引数汎用検証処理実施クラス
    /// </summary>
    /// <typeparam name="TRow">リスト行データ型</typeparam>
    /// <typeparam name="TItem">リスト要素型</typeparam>
    internal class CommonTwoDimensionalListValidator<TRow, TItem>
        : CommonTwoDimensionalListValidator<TRow, TRow, TItem, TItem>
        where TRow : IEnumerable<TItem>
    {
        public CommonTwoDimensionalListValidator(ITwoDimensionalList<TRow, TRow, TItem, TItem> target,
            string rowName = "行",
            string rowIndexName = "row", string columnName = "列", string columnIndexName = "column") : base(target,
            rowName, rowIndexName, columnName, columnIndexName)
        {
        }
    }

    /// <summary>
    ///     二次元リスト編集メソッドの引数汎用検証処理実施クラス
    /// </summary>
    /// <typeparam name="TInRow">リスト行データ入力型</typeparam>
    /// <typeparam name="TOutRow">リスト行データ出力型</typeparam>
    /// <typeparam name="TInItem">リスト要素入力型</typeparam>
    /// <typeparam name="TOutItem">リスト要素出力型</typeparam>
    internal class CommonTwoDimensionalListValidator<TInRow, TOutRow, TInItem, TOutItem>
        : WodiLibTwoDimensionalListValidator<TInRow, TOutRow, TInItem, TOutItem>
        where TOutRow : IEnumerable<TOutItem>
        where TInRow : IEnumerable<TInItem>, TOutRow
        where TInItem : TOutItem
    {
        protected override ITwoDimensionalListValidator<TInRow, TInItem>? BaseValidator => null;

        private string RowIndexName { get; }
        private string ColumnIndexName { get; }

        public CommonTwoDimensionalListValidator(ITwoDimensionalList<TInRow, TOutRow, TInItem, TOutItem> target,
            string rowName = "行", string rowIndexName = "row",
            string columnName = "列", string columnIndexName = "column") : base(target, rowName, columnName)
        {
            RowIndexName = rowIndexName;
            ColumnIndexName = columnIndexName;
        }

        public override void Constructor(TInRow[] initItems)
        {
            ThrowHelper.ValidateArgumentNotNull(initItems is null, nameof(initItems));

            var casted = initItems.Cast<IEnumerable<TInItem>>().ToArray();

            TwoDimensionalListValidationHelper.ItemNotNull(casted);
            TwoDimensionalListValidationHelper.InnerItemLength(casted);
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

        public override void SetRow(int rowIndex, params TInRow[] rows)
        {
            ListValidationHelper.SelectIndex(rowIndex, Target.RowCount, nameof(rowIndex));

            ThrowHelper.ValidateArgumentNotNull(rows is null, nameof(rows));

            var rowArrays = rows.Cast<IEnumerable<TInItem>>().ToTwoDimensionalArray();

            TwoDimensionalListValidationHelper.ItemNotNull(rowArrays, nameof(rows));
            TwoDimensionalListValidationHelper.InnerItemLength(rowArrays);

            ListValidationHelper.Range(rowIndex, rowArrays.Length, Target.RowCount);

            var columnLength = rowArrays.GetInnerArrayLength();
            TwoDimensionalListValidationHelper.SizeEqual(columnLength, Target.ColumnCount);
        }

        public override void SetColumn(int columnIndex, params IEnumerable<TInItem>[] items)
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

        public override void SetItem(int rowIndex, int columnIndex, TInItem item)
        {
            ListValidationHelper.SelectIndex(rowIndex, Target.RowCount, nameof(rowIndex));
            ListValidationHelper.SelectIndex(columnIndex, Target.ColumnCount, nameof(columnIndex));
            ThrowHelper.ValidateArgumentNotNull(item is null, nameof(item));
        }

        public override void InsertRow(int rowIndex, params TInRow[] items)
        {
            ListValidationHelper.InsertIndex(rowIndex, Target.RowCount, RowIndexName);

            ThrowHelper.ValidateArgumentNotNull(items is null, nameof(items));

            var itemArrays = items.Cast<IEnumerable<TInItem>>().ToTwoDimensionalArray();

            TwoDimensionalListValidationHelper.InnerItemLength(itemArrays);
            TwoDimensionalListValidationHelper.ItemNotNull(itemArrays, nameof(items));

            if (!Target.IsEmpty && items.Length > 0)
            {
                TwoDimensionalListValidationHelper.SizeEqual(itemArrays[0].Length,
                    Target.ColumnCount, ColumnIndexName);
            }
        }

        public override void InsertColumn(int columnIndex, params IEnumerable<TInItem>[] items)
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

        public override void OverwriteRow(int rowIndex, params TInRow[] items)
        {
            ListValidationHelper.InsertIndex(rowIndex, Target.RowCount, RowIndexName);

            ThrowHelper.ValidateArgumentNotNull(items is null, nameof(items));

            var itemArrays = items.Cast<IEnumerable<TInItem>>().ToTwoDimensionalArray();

            TwoDimensionalListValidationHelper.InnerItemLength(itemArrays);
            TwoDimensionalListValidationHelper.ItemNotNull(itemArrays, nameof(items));

            if (!Target.IsEmpty && items.Length > 0)
            {
                TwoDimensionalListValidationHelper.SizeEqual(itemArrays[0].Length,
                    Target.ColumnCount, ColumnIndexName);
            }
        }

        public override void OverwriteColumn(int columnIndex, params IEnumerable<TInItem>[] items)
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

        public override void Reset(IEnumerable<TInRow> items)
        {
            ThrowHelper.ValidateArgumentNotNull(items is null, nameof(items));
            var itemArray = items.Cast<IEnumerable<TInItem>>().ToTwoDimensionalArray();

            TwoDimensionalListValidationHelper.ItemNotNull(itemArray);
            TwoDimensionalListValidationHelper.InnerItemLength(itemArray);
        }

        private void ValidateTargetIsEmpty()
        {
            ThrowHelper.InvalidOperationIf(Target.IsEmpty,
                () => ErrorMessage.NotExecute("空リストのため"));
        }
    }
}
