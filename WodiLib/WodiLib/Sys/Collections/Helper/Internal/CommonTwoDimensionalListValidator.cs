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
        public CommonTwoDimensionalListValidator(
            ITwoDimensionalList<TRow, TRow, TItem, TItem> target,
            string rowName = "行",
            string rowIndexName = "row",
            string columnName = "列",
            string columnIndexName = "column"
        ) : base(
            target,
            rowName,
            rowIndexName,
            columnName,
            columnIndexName
        )
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
        where TInRow : IEnumerable<TInItem>
        where TOutRow : IEnumerable<TOutItem>, TInRow
        where TOutItem : TInItem
    {
        protected override ITwoDimensionalListValidator<TInRow, TInItem>? BaseValidator => null;

        private string RowIndexName { get; }
        private string ColumnIndexName { get; }

        public CommonTwoDimensionalListValidator(
            ITwoDimensionalList<TInRow, TOutRow, TInItem, TOutItem> target,
            string rowName = "行",
            string rowIndexName = "row",
            string columnName = "列",
            string columnIndexName = "column"
        ) : base(target, rowName, columnName)
        {
            RowIndexName = rowIndexName;
            ColumnIndexName = columnIndexName;
        }

        public override void Constructor(NamedValue<TInRow[]> initItems)
        {
            ThrowHelper.ValidateArgumentNotNull(initItems is null, initItems.Name);

            var casted = initItems.Value.Cast<IEnumerable<TInItem>>().ToArray();

            TwoDimensionalListValidationHelper.ItemNotNull<TInItem>((nameof(initItems), casted));
            TwoDimensionalListValidationHelper.InnerItemLength(casted);
        }

        public override void GetRow(NamedValue<int> rowIndex, NamedValue<int> rowCount)
        {
            var namedRowCount = MakeNamedRowCount();
            ListValidationHelper.SelectIndex(rowIndex, namedRowCount);
            ListValidationHelper.Count(rowCount, namedRowCount);
            ListValidationHelper.Range(rowIndex, rowCount, namedRowCount);
        }

        public override void GetColumn(NamedValue<int> columnIndex, NamedValue<int> columnCount)
        {
            var namedColumnCount = MakeNamedColumnCount();
            ListValidationHelper.SelectIndex(columnIndex, namedColumnCount);
            ListValidationHelper.Count(columnCount, namedColumnCount);
            ListValidationHelper.Range(columnIndex, columnCount, namedColumnCount);
        }

        public override void GetItem(
            NamedValue<int> rowIndex,
            NamedValue<int> rowCount,
            NamedValue<int> columnIndex,
            NamedValue<int> columnCount
        )
        {
            var namedRowCount = MakeNamedRowCount();
            var namedColumnCount = MakeNamedColumnCount();
            ListValidationHelper.SelectIndex(rowIndex, namedRowCount);
            ListValidationHelper.Count(rowCount, namedRowCount);
            ListValidationHelper.Range(rowIndex, rowCount, namedRowCount);
            ListValidationHelper.SelectIndex(columnIndex, namedColumnCount);
            ListValidationHelper.Count(columnCount, namedColumnCount);
            ListValidationHelper.Range(columnIndex, columnCount, namedColumnCount);
        }

        public override void GetItem(NamedValue<int> rowIndex, NamedValue<int> columnIndex)
        {
            ListValidationHelper.SelectIndex(
                rowIndex,
                MakeNamedRowCount()
            );
            ListValidationHelper.SelectIndex(
                columnIndex,
                MakeNamedColumnCount()
            );
        }

        public override void SetRow(NamedValue<int> rowIndex, string targetParamName, params TInRow[] rows)
        {
            var namedRowCount = MakeNamedRowCount();

            ListValidationHelper.SelectIndex(rowIndex, namedRowCount);

            ThrowHelper.ValidateArgumentNotNull(rows is null, targetParamName);

            var rowArrays = rows.Cast<IEnumerable<TInItem>>().ToTwoDimensionalArray();

            TwoDimensionalListValidationHelper.ItemNotNull<TInItem>((targetParamName, rowArrays));
            TwoDimensionalListValidationHelper.InnerItemLength(rowArrays);

            ListValidationHelper.Range(
                rowIndex,
                ($"{nameof(rows)}.{nameof(rows.Length)}", rowArrays.Length),
                namedRowCount
            );

            var columnLength = rowArrays.GetInnerArrayLength();
            TwoDimensionalListValidationHelper.SizeEqual(
                ($"{targetParamName}の要素数", columnLength),
                MakeNamedColumnCount()
            );
        }

        public override void SetColumn(
            NamedValue<int> columnIndex,
            string targetParamName,
            params IEnumerable<TInItem>[] items
        )
        {
            var namedColumnCount = MakeNamedColumnCount();

            ListValidationHelper.SelectIndex(columnIndex, namedColumnCount);

            ThrowHelper.ValidateArgumentNotNull(items is null, targetParamName);

            var itemArrays = items.ToTwoDimensionalArray();

            TwoDimensionalListValidationHelper.ItemNotNull<TInItem>((targetParamName, itemArrays));
            TwoDimensionalListValidationHelper.InnerItemLength(itemArrays);

            ListValidationHelper.Range(columnIndex, (nameof(items.Length), items.Length), namedColumnCount);

            var rowLength = itemArrays.GetInnerArrayLength();
            TwoDimensionalListValidationHelper.SizeEqual(($"{targetParamName}の要素数", rowLength), MakeNamedRowCount());
        }

        public override void SetItem(NamedValue<int> rowIndex, NamedValue<int> columnIndex, NamedValue<TInItem> item)
        {
            ListValidationHelper.SelectIndex(
                rowIndex,
                MakeNamedRowCount()
            );
            ListValidationHelper.SelectIndex(
                columnIndex,
                MakeNamedColumnCount()
            );
            ThrowHelper.ValidateArgumentNotNull(item is null, item.Name);
        }

        public override void InsertRow(NamedValue<int> rowIndex, string targetParamName, params TInRow[] items)
        {
            ListValidationHelper.InsertIndex(
                rowIndex,
                MakeNamedRowCount()
            );

            ThrowHelper.ValidateArgumentNotNull(items is null, targetParamName);

            var itemArrays = items.Cast<IEnumerable<TInItem>>().ToTwoDimensionalArray();

            TwoDimensionalListValidationHelper.InnerItemLength(itemArrays);
            TwoDimensionalListValidationHelper.ItemNotNull<TInItem>((targetParamName, itemArrays));

            if (!Target.IsEmpty && items.Length > 0)
            {
                TwoDimensionalListValidationHelper.SizeEqual(
                    (ColumnIndexName, itemArrays[0].Length),
                    MakeNamedColumnCount()
                );
            }
        }

        public override void InsertColumn(
            NamedValue<int> columnIndex,
            string targetParamName,
            params IEnumerable<TInItem>[] items
        )
        {
            ListValidationHelper.InsertIndex(
                columnIndex,
                MakeNamedColumnCount()
            );

            ThrowHelper.ValidateArgumentNotNull(items is null, targetParamName);

            var itemArrays = items.ToTwoDimensionalArray();

            TwoDimensionalListValidationHelper.InnerItemLength(itemArrays);
            TwoDimensionalListValidationHelper.ItemNotNull<TInItem>((targetParamName, itemArrays));

            if (!Target.IsEmpty && items.Length > 0)
            {
                TwoDimensionalListValidationHelper.SizeEqual(
                    (RowIndexName, itemArrays[0].Length),
                    MakeNamedRowCount()
                );
            }
        }

        public override void OverwriteRow(NamedValue<int> rowIndex, string targetParamName, params TInRow[] items)
        {
            ListValidationHelper.InsertIndex(
                rowIndex,
                MakeNamedRowCount()
            );

            ThrowHelper.ValidateArgumentNotNull(items is null, targetParamName);

            var itemArrays = items.Cast<IEnumerable<TInItem>>().ToTwoDimensionalArray();

            TwoDimensionalListValidationHelper.InnerItemLength(itemArrays);
            TwoDimensionalListValidationHelper.ItemNotNull<TInItem>((targetParamName, itemArrays));

            if (!Target.IsEmpty && items.Length > 0)
            {
                TwoDimensionalListValidationHelper.SizeEqual(
                    (ColumnIndexName, itemArrays[0].Length),
                    MakeNamedColumnCount()
                );
            }
        }

        public override void OverwriteColumn(
            NamedValue<int> columnIndex,
            string targetParamName,
            params IEnumerable<TInItem>[] items
        )
        {
            ListValidationHelper.InsertIndex(
                columnIndex,
                MakeNamedColumnCount()
            );

            ThrowHelper.ValidateArgumentNotNull(items is null, targetParamName);

            var itemArrays = items.ToTwoDimensionalArray();

            TwoDimensionalListValidationHelper.InnerItemLength(itemArrays);
            TwoDimensionalListValidationHelper.ItemNotNull<TInItem>((targetParamName, itemArrays));

            if (!Target.IsEmpty && items.Length > 0)
            {
                TwoDimensionalListValidationHelper.SizeEqual(
                    (RowIndexName, itemArrays[0].Length),
                    MakeNamedRowCount()
                );
            }
        }

        public override void MoveRow(NamedValue<int> oldRowIndex, NamedValue<int> newRowIndex, NamedValue<int> count)
        {
            var namedRowCount = MakeNamedRowCount();
            TwoDimensionalListValidationHelper.LengthNotZero((RowName, Target.RowCount));
            ListValidationHelper.SelectIndex(oldRowIndex, namedRowCount);
            ListValidationHelper.InsertIndex(newRowIndex, namedRowCount);
            ListValidationHelper.Count(count, namedRowCount);
            ListValidationHelper.Range(oldRowIndex, count, namedRowCount);
            ListValidationHelper.Range(count, newRowIndex, namedRowCount);
        }

        public override void MoveColumn(
            NamedValue<int> oldColumnIndex,
            NamedValue<int> newColumnIndex,
            NamedValue<int> count
        )
        {
            var namedColumnCount = MakeNamedColumnCount();
            TwoDimensionalListValidationHelper.LengthNotZero((ColumnIndexName, Target.ColumnCount));
            ListValidationHelper.SelectIndex(oldColumnIndex, namedColumnCount);
            ListValidationHelper.InsertIndex(newColumnIndex, namedColumnCount);
            ListValidationHelper.Count(count, namedColumnCount);
            ListValidationHelper.Range(oldColumnIndex, count, namedColumnCount);
            ListValidationHelper.Range(count, newColumnIndex, namedColumnCount);
        }

        public override void RemoveRow(NamedValue<int> rowIndex, NamedValue<int> count)
        {
            ValidateTargetIsEmpty();
            var namedRowCount = MakeNamedRowCount();
            ListValidationHelper.SelectIndex(
                rowIndex,
                namedRowCount
            );
            ListValidationHelper.Count(count, MakeNamedRowCount());
            ListValidationHelper.Range(
                rowIndex,
                count,
                namedRowCount
            );
        }

        public override void RemoveColumn(NamedValue<int> columnIndex, NamedValue<int> count)
        {
            ValidateTargetIsEmpty();
            var namedColumnCount = MakeNamedColumnCount();
            ListValidationHelper.SelectIndex(columnIndex, namedColumnCount);
            ListValidationHelper.Count(count, namedColumnCount);
            ListValidationHelper.Range(columnIndex, count, namedColumnCount);
        }

        public override void AdjustLength(NamedValue<int> rowLength, NamedValue<int> columnLength)
        {
            ThrowHelper.ValidateArgumentValueGreaterOrEqual(
                rowLength.Value < 0,
                rowLength.Name,
                0,
                rowLength.Value
            );
            ThrowHelper.ValidateArgumentValueGreaterOrEqual(
                columnLength.Value < 0,
                columnLength.Name,
                0,
                columnLength.Value
            );

            if (rowLength.Value > 0) return;
            // rowLength == 0

            if (columnLength.Value == 0) return;

            // rowLength == 0 && columnLength != 0 は不正な指定
            ThrowHelper.ValidateArgumentUnsuitable(
                true,
                $"{RowName}数および{ColumnName}数の指定",
                $"{RowName}数 == 0 かつ {ColumnName}数 != 0 の指定はできません"
            );
        }

        public override void Reset(NamedValue<IEnumerable<TInRow>> initItems)
        {
            ThrowHelper.ValidateArgumentNotNull(initItems.Value is null, initItems.Name);
            var itemArray = initItems.Value.Cast<IEnumerable<TInItem>>().ToTwoDimensionalArray();

            TwoDimensionalListValidationHelper.ItemNotNull<TInItem>((nameof(initItems), itemArray));
            TwoDimensionalListValidationHelper.InnerItemLength(itemArray);
        }

        private void ValidateTargetIsEmpty()
        {
            ThrowHelper.InvalidOperationIf(
                Target.IsEmpty,
                () => ErrorMessage.NotExecute("空リストのため")
            );
        }

        private NamedValue<int> MakeNamedRowCount()
            => new($"{RowName}数", Target.RowCount);

        private NamedValue<int> MakeNamedColumnCount()
            => new($"{ColumnName}数", Target.ColumnCount);
    }
}
