// ========================================
// Project Name : WodiLib.Test
// File Name    : MockTwoDimensionalList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using WodiLib.Sys;
using WodiLib.Sys.Collections;

namespace WodiLib.Test.Tools
{
    /// <summary>
    ///     <see cref="ITwoDimensionalListValidator{TRow,TItem}"/> モック用
    /// </summary>
    internal class MockTwoDimensionalList<T> : MockBase<ITwoDimensionalList<MockExtendedList<T>, T>>,
        ITwoDimensionalList<MockExtendedList<T>, T>
    {
        public IEnumerator<MockExtendedList<T>> GetEnumerator()
        {
            AddCalledHistory(nameof(ItemEquals));
            return default!;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public MockExtendedList<T> this[int rowIndex]
        {
            get
            {
                AddCalledHistory(ListConstant.IndexerName);
                return default!;
            }
            set
            {
                AddCalledHistory(ListConstant.IndexerName);
                // "value" 未使用による警告を抑制するため適当に利用する
                var _ = value;
            }
        }

        public T this[int rowIndex, int columnIndex]
        {
            get
            {
                AddCalledHistory(ListConstant.IndexerName);
                return default!;
            }
            set
            {
                AddCalledHistory(ListConstant.IndexerName);
                // "value" 未使用による警告を抑制するため適当に利用する
                var _ = value;
            }
        }

        public bool IsEmpty
        {
            get
            {
                AddCalledHistory(nameof(IsEmpty));
                return default!;
            }
        }

        public int RowCount
        {
            get
            {
                AddCalledHistory(nameof(RowCount));
                return default!;
            }
        }

        public int ColumnCount
        {
            get
            {
                AddCalledHistory(nameof(ColumnCount));
                return default!;
            }
        }

        public int AllCount
        {
            get
            {
                AddCalledHistory(nameof(AllCount));
                return default!;
            }
        }

        public ITwoDimensionalListValidator<MockExtendedList<T>, T> Validator
        {
            get
            {
                AddCalledHistory(nameof(Validator));
                return default!;
            }
        }

        public void AddRowPropertyChanged(PropertyChangedEventHandler handler)
        {
            AddCalledHistory(nameof(AddRowPropertyChanged));
        }

        public void RemoveRowPropertyChanged(PropertyChangedEventHandler handler)
        {
            AddCalledHistory(nameof(RemoveRowPropertyChanged));
        }

        public void AddRowCollectionChanged(NotifyCollectionChangedEventHandler handler)
        {
            AddCalledHistory(nameof(AddRowCollectionChanged));
        }

        public void RemoveRowCollectionChanged(NotifyCollectionChangedEventHandler handler)
        {
            AddCalledHistory(nameof(RemoveRowCollectionChanged));
        }

        public int GetMaxRowCapacity()
        {
            AddCalledHistory(nameof(GetMaxRowCapacity));
            return int.MaxValue;
        }

        public int GetMinRowCapacity()
        {
            AddCalledHistory(nameof(GetMinRowCapacity));
            return 0;
        }

        public int GetMaxColumnCapacity()
        {
            AddCalledHistory(nameof(GetMaxColumnCapacity));
            return int.MaxValue;
        }

        public int GetMinColumnCapacity()
        {
            AddCalledHistory(nameof(GetMinColumnCapacity));
            return 0;
        }

        public IEnumerable<MockExtendedList<T>> GetRowRangeCore(int rowIndex, int rowCount)
        {
            AddCalledHistory(nameof(GetRowRangeCore));
            return default!;
        }

        public IEnumerable<IEnumerable<T>> GetColumnRangeCore(int columnIndex, int columnCount)
        {
            AddCalledHistory(nameof(GetColumnRangeCore));
            return default!;
        }

        public IEnumerable<IEnumerable<T>> GetItemCore(int rowIndex, int rowCount, int columnIndex, int columnCount)
        {
            AddCalledHistory(nameof(GetItemCore));
            return default!;
        }

        public void SetRowRangeCore(int rowIndex, IEnumerable<MockExtendedList<T>> items)
        {
            AddCalledHistory(nameof(SetRowRangeCore));
        }

        public void SetColumnRangeCore(int columnIndex, IEnumerable<IEnumerable<T>> items)
        {
            AddCalledHistory(nameof(SetColumnRangeCore));
        }

        public void SetItemCore(int rowIndex, int columnIndex, T item)
        {
            AddCalledHistory(nameof(SetItemCore));
        }

        public void AddRowRangeCore(IEnumerable<MockExtendedList<T>> items)
        {
            AddCalledHistory(nameof(AddRowRangeCore));
        }

        public void AddColumnRangeCore(IEnumerable<IEnumerable<T>> items)
        {
            AddCalledHistory(nameof(AddColumnRangeCore));
        }

        public void InsertRowRangeCore(int rowIndex, IEnumerable<MockExtendedList<T>> items)
        {
            AddCalledHistory(nameof(InsertRowRangeCore));
        }

        public void InsertColumnRangeCore(int columnIndex, IEnumerable<IEnumerable<T>> items)
        {
            AddCalledHistory(nameof(InsertColumnRangeCore));
        }

        public void OverwriteRowCore(int rowIndex, IEnumerable<MockExtendedList<T>> items)
        {
            AddCalledHistory(nameof(OverwriteRowCore));
        }

        public void OverwriteColumnCore(int columnIndex, IEnumerable<IEnumerable<T>> items)
        {
            AddCalledHistory(nameof(OverwriteColumnCore));
        }

        public void MoveRowRangeCore(int oldRowIndex, int newRowIndex, int count)
        {
            AddCalledHistory(nameof(MoveRowRangeCore));
        }

        public void MoveColumnRangeCore(int oldColumnIndex, int newColumnIndex, int count)
        {
            AddCalledHistory(nameof(MoveColumnRangeCore));
        }

        public void RemoveRowRangeCore(int rowIndex, int count)
        {
            AddCalledHistory(nameof(RemoveRowRangeCore));
        }

        public void RemoveColumnRangeCore(int columnIndex, int count)
        {
            AddCalledHistory(nameof(RemoveColumnRangeCore));
        }

        public void AdjustLengthCore(int rowLength, int columnLength)
        {
            AddCalledHistory(nameof(AdjustLengthCore));
        }

        public void ResetCore(IEnumerable<MockExtendedList<T>> rows)
        {
            AddCalledHistory(nameof(ResetCore));
        }

        public void ClearCore()
        {
            AddCalledHistory(nameof(ClearCore));
        }

        public T[][] ToTwoDimensionalArray(bool isTranspose = false)
        {
            AddCalledHistory(nameof(ToTwoDimensionalArray));
            return default!;
        }
    }
}
