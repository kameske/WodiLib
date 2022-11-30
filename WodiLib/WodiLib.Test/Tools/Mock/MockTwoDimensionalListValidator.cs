// ========================================
// Project Name : WodiLib.Test
// File Name    : MockTwoDimensionalListValidator.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using WodiLib.Sys;
using WodiLib.Sys.Collections;

namespace WodiLib.Test.Tools
{
    /// <summary>
    ///     <see cref="ITwoDimensionalListValidator{TRow,TItem}"/> モック用
    /// </summary>
    internal class MockTwoDimensionalListValidator<TRow, TItem> : MockBase<ITwoDimensionalListValidator<TRow, TItem>>,
        ITwoDimensionalListValidator<TRow, TItem>
    {
        public void Constructor(NamedValue<IEnumerable<TRow>> initItems)
        {
            AddCalledHistory(nameof(Constructor));
        }

        public void GetRow(NamedValue<int> rowIndex, NamedValue<int> rowCount)
        {
            AddCalledHistory(nameof(GetRow));
        }

        public void GetColumn(NamedValue<int> columnIndex, NamedValue<int> columnCount)
        {
            AddCalledHistory(nameof(GetColumn));
        }

        public void GetItem(
            NamedValue<int> rowIndex,
            NamedValue<int> rowCount,
            NamedValue<int> columnIndex,
            NamedValue<int> columnCount
        )
        {
            AddCalledHistory(nameof(GetItem));
        }

        public void GetItem(NamedValue<int> rowIndex, NamedValue<int> columnIndex)
        {
            AddCalledHistory(nameof(GetItem));
        }

        public void SetRow(NamedValue<int> rowIndex, NamedValue<IEnumerable<TRow>> rows)
        {
            AddCalledHistory(nameof(SetRow));
        }

        public void SetColumn(NamedValue<int> columnIndex, NamedValue<IEnumerable<IEnumerable<TItem>>> items)
        {
            AddCalledHistory(nameof(SetColumn));
        }

        public void SetItem(NamedValue<int> rowIndex, NamedValue<int> columnIndex, NamedValue<TItem> item)
        {
            AddCalledHistory(nameof(SetItem));
        }

        public void InsertRow(NamedValue<int> rowIndex, NamedValue<IEnumerable<TRow>> items)
        {
            AddCalledHistory(nameof(InsertRow));
        }

        public void InsertColumn(NamedValue<int> columnIndex, NamedValue<IEnumerable<IEnumerable<TItem>>> items)
        {
            AddCalledHistory(nameof(InsertColumn));
        }

        public void OverwriteRow(NamedValue<int> rowIndex, NamedValue<IEnumerable<TRow>> items)
        {
            AddCalledHistory(nameof(OverwriteRow));
        }

        public void OverwriteColumn(NamedValue<int> columnIndex, NamedValue<IEnumerable<IEnumerable<TItem>>> items)
        {
            AddCalledHistory(nameof(OverwriteColumn));
        }

        public void MoveRow(NamedValue<int> oldRowIndex, NamedValue<int> newRowIndex, NamedValue<int> count)
        {
            AddCalledHistory(nameof(MoveRow));
        }

        public void MoveColumn(NamedValue<int> oldColumnIndex, NamedValue<int> newColumnIndex, NamedValue<int> count)
        {
            AddCalledHistory(nameof(MoveColumn));
        }

        public void RemoveRow(NamedValue<int> rowIndex, NamedValue<int> count)
        {
            AddCalledHistory(nameof(RemoveRow));
        }

        public void RemoveColumn(NamedValue<int> columnIndex, NamedValue<int> count)
        {
            AddCalledHistory(nameof(RemoveColumn));
        }

        public void AdjustLength(NamedValue<int> rowLength, NamedValue<int> columnLength)
        {
            AddCalledHistory(nameof(AdjustLength));
        }

        public void Reset(NamedValue<IEnumerable<TRow>> initItems)
        {
            AddCalledHistory(nameof(Reset));
        }

        public void Clear()
        {
            AddCalledHistory(nameof(Clear));
        }
    }
}
