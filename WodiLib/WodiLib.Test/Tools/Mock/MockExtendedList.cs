// ========================================
// Project Name : WodiLib.Test
// File Name    : MockExtendedList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections;
using System.Collections.Generic;
using WodiLib.Sys;
using WodiLib.Sys.Collections;

namespace WodiLib.Test.Tools
{
    /// <summary>
    ///     <see cref="IExtendedList{T}"/> モック用
    /// </summary>
    internal class MockExtendedList<T> : MockBase<IExtendedList<T>>,
        IExtendedList<T>
    {
        public IExtendedList<T> Impl { get; set; } = new ExtendedList<T>(
            _ => default!,
            new MockWodiLibListValidator<T>()
        );

        public MockWodiLibListValidator<T> ValidatorMock => (MockWodiLibListValidator<T>)validator;

        public IEnumerator<T> GetEnumerator()
        {
            AddCalledHistory(nameof(GetEnumerator));
            return Impl.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            AddCalledHistory(nameof(GetEnumerator));
            return Impl.GetEnumerator();
        }

        public T this[int index]
        {
            get
            {
                AddCalledHistory(ListConstant.IndexerName);
                return Impl[index];
            }
            set
            {
                AddCalledHistory(ListConstant.IndexerName);
                Impl[index] = value;
            }
        }

        public int Count
        {
            get
            {
                AddCalledHistory(nameof(Count));
                return Impl.Count;
            }
        }

        public Func<int, T> MakeDefaultItem
        {
            get
            {
                AddCalledHistory(nameof(MakeDefaultItem));
                return Impl.MakeDefaultItem;
            }
        }

        private IWodiLibListValidator<T> validator = new MockWodiLibListValidator<T>();

        public IWodiLibListValidator<T> Validator
        {
            get
            {
                AddCalledHistory(nameof(Validator));
                return validator;
            }
            set => validator = value;
        }

        public IEnumerable<T> GetRangeCore(int index, int count)
        {
            AddCalledHistory(nameof(GetRangeCore));
            return Impl.GetRangeCore(index, count);
        }

        public void SetRangeCore(int index, IEnumerable<T> items) => AddCalledHistory(nameof(SetRangeCore));

        public void InsertRangeCore(int index, IEnumerable<T> items) => AddCalledHistory(nameof(InsertRangeCore));

        public void OverwriteCore(int index, IEnumerable<T> items) => AddCalledHistory(nameof(OverwriteCore));

        public void MoveRangeCore(int oldIndex, int newIndex, int count) => AddCalledHistory(nameof(MoveRangeCore));

        public void RemoveRangeCore(int index, int count) => AddCalledHistory(nameof(RemoveRangeCore));

        public void AdjustLengthCore(int length) => AddCalledHistory(nameof(AdjustLengthCore));

        public void ResetCore(IEnumerable<T> items) => AddCalledHistory(nameof(ResetCore));

        public void ClearCore() => AddCalledHistory(nameof(ClearCore));

        public bool ItemEquals(IEnumerable<T>? other) => Impl.ItemEquals(other);
    }
}
