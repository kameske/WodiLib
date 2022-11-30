// ========================================
// Project Name : WodiLib.Test
// File Name    : MockSimpleList.cs
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
    ///     <see cref="SimpleList{T}"/> モック用
    /// </summary>
    internal class MockSimpleList<T> : MockBase<ISimpleList<T>>,
        ISimpleList<T>
    {
        private readonly SimpleList<T> impl;

        public int Count
        {
            get
            {
                AddCalledHistory(nameof(Count));
                return impl.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                AddCalledHistory(nameof(IsReadOnly));
                return false;
            }
        }

        public DelegateMakeListDefaultItem<T> MakeDefaultItem => impl.MakeDefaultItem;

        public MockSimpleList(DelegateMakeListDefaultItem<T> makeDefaultItem, IEnumerable<T>? initValues = null)
        {
            impl = new SimpleList<T>(makeDefaultItem, initValues);
        }

        public IEnumerator<T> GetEnumerator()
        {
            AddCalledHistory(nameof(GetEnumerator));
            return impl.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            AddCalledHistory(nameof(GetEnumerator));
            return GetEnumerator();
        }

        public void Add(T item) => AddCalledHistory(nameof(Add));

        public void Clear() => AddCalledHistory(nameof(Clear));


        public bool Contains(T item)
        {
            AddCalledHistory(nameof(Contains));
            return false;
        }

        public void CopyTo(T[] array, int arrayIndex) => AddCalledHistory(nameof(CopyTo));


        public bool Remove(T item)
        {
            AddCalledHistory(nameof(Remove));
            return false;
        }

        public int IndexOf(T item)
        {
            AddCalledHistory(nameof(Index));
            return -1;
        }

        public void Insert(int index, T item) => AddCalledHistory(nameof(Insert));

        public void RemoveAt(int index) => AddCalledHistory(nameof(RemoveAt));

        public T this[int index]
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

        public IEnumerable<T> Get(int index, int count)
        {
            AddCalledHistory(nameof(Get));
            return impl.Get(index, count);
        }

        public void Set(int index, params T[] items) => AddCalledHistory(nameof(Set));


        public void Add(params T[] items) => AddCalledHistory(nameof(Add));


        public void Insert(int index, params T[] items) => AddCalledHistory(nameof(Insert));


        public void Overwrite(int index, params T[] items) => AddCalledHistory(nameof(Overwrite));


        public void Move(int oldIndex, int newIndex) => AddCalledHistory(nameof(Move));


        public void Move(int oldIndex, int newIndex, int count) => AddCalledHistory(nameof(Move));


        public void Remove(int index, int count) => AddCalledHistory(nameof(Remove));


        public void Adjust(int length) => AddCalledHistory(nameof(Adjust));


        public void AdjustIfLong(int length) => AddCalledHistory(nameof(AdjustIfLong));


        public void AdjustIfShort(int length) => AddCalledHistory(nameof(AdjustIfShort));


        public void Reset(params T[] items) => AddCalledHistory(nameof(Reset));
    }
}
