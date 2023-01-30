// ========================================
// Project Name : WodiLib.Test
// File Name    : MockRestrictedCapacityList.cs
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
    ///     <see cref="IRestrictedCapacityList{T}"/> 用Mock
    /// </summary>
    internal class MockRestrictedCapacityList<T> : MockBase<MockRestrictedCapacityList<T>>,
        IRestrictedCapacityList<T>
    {
        public IExtendedList<T> Impl { get; set; } = new ExtendedList<T>(
            _ => default!,
            new MockWodiLibListValidator<T>()
        );

        public int MaxCapacity { get; }
        public int MinCapacity { get; }

        public MockWodiLibListValidator<T> ValidatorMock => (MockWodiLibListValidator<T>)Impl.Validator!;

        public IEnumerator<T> GetEnumerator()
        {
            AddCalledHistory(nameof(GetEnumerator));
            return Impl.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int Count
        {
            get
            {
                AddCalledHistory(nameof(Count));
                return Impl.Count;
            }
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

        public MockRestrictedCapacityList(int minCapacity, int maxCapacity)
        {
            MinCapacity = minCapacity;
            MaxCapacity = maxCapacity;
        }

        public int GetMaxCapacity()
        {
            throw new NotImplementedException();
        }

        public int GetMinCapacity()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetRange(int index, int count)
        {
            throw new NotImplementedException();
        }

        public void SetRange(int index, IEnumerable<T> items)
        {
            throw new NotImplementedException();
        }

        public void Add(T item)
        {
            throw new NotImplementedException();
        }

        public void AddRange(IEnumerable<T> items)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, T item)
        {
            throw new NotImplementedException();
        }

        public void InsertRange(int index, IEnumerable<T> items)
        {
            throw new NotImplementedException();
        }

        public void Overwrite(int index, IEnumerable<T> items)
        {
            throw new NotImplementedException();
        }

        public void Move(int oldIndex, int newIndex)
        {
            throw new NotImplementedException();
        }

        public void MoveRange(int oldIndex, int newIndex, int count)
        {
            throw new NotImplementedException();
        }

        public void Remove(int index)
        {
            throw new NotImplementedException();
        }

        public void RemoveRange(int index, int count)
        {
            throw new NotImplementedException();
        }

        public void AdjustLength(int length)
        {
            throw new NotImplementedException();
        }

        public void AdjustLengthIfShort(int length)
        {
            throw new NotImplementedException();
        }

        public void AdjustLengthIfLong(int length)
        {
            throw new NotImplementedException();
        }

        public void Reset(IEnumerable<T> initItems)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public void ValidateGet(int index)
        {
            throw new NotImplementedException();
        }

        public void ValidateGetRange(int index, int count)
        {
            throw new NotImplementedException();
        }

        public void ValidateSet(int index, T item)
        {
            throw new NotImplementedException();
        }

        public void ValidateSetRange(int index, IEnumerable<T> items)
        {
            throw new NotImplementedException();
        }

        public void ValidateAdd(T item)
        {
            throw new NotImplementedException();
        }

        public void ValidateAddRange(IEnumerable<T> items)
        {
            throw new NotImplementedException();
        }

        public void ValidateInsert(int index, T item)
        {
            throw new NotImplementedException();
        }

        public void ValidateInsertRange(int index, IEnumerable<T> items)
        {
            throw new NotImplementedException();
        }

        public void ValidateOverwrite(int index, IEnumerable<T> items)
        {
            throw new NotImplementedException();
        }

        public void ValidateMove(int oldIndex, int newIndex)
        {
            throw new NotImplementedException();
        }

        public void ValidateMoveRange(int oldIndex, int newIndex, int count)
        {
            throw new NotImplementedException();
        }

        public void ValidateRemove(int index)
        {
            throw new NotImplementedException();
        }

        public void ValidateRemoveRange(int index, int count)
        {
            throw new NotImplementedException();
        }

        public void ValidateAdjustLength(int length)
        {
            throw new NotImplementedException();
        }

        public void ValidateAdjustLengthIfShort(int length)
        {
            throw new NotImplementedException();
        }

        public void ValidateAdjustLengthIfLong(int length)
        {
            throw new NotImplementedException();
        }

        public void ValidateReset(IEnumerable<T> items)
        {
            throw new NotImplementedException();
        }

        public void ValidateClear()
        {
            throw new NotImplementedException();
        }

        public T GetCore(int index)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetRangeCore(int index, int count)
        {
            throw new NotImplementedException();
        }

        public void SetCore(int index, T item)
        {
            throw new NotImplementedException();
        }

        public void SetRangeCore(int index, IEnumerable<T> items)
        {
            throw new NotImplementedException();
        }

        public void AddCore(T item)
        {
            throw new NotImplementedException();
        }

        public void AddRangeCore(IEnumerable<T> items)
        {
            throw new NotImplementedException();
        }

        public void InsertCore(int index, T item)
        {
            throw new NotImplementedException();
        }

        public void InsertRangeCore(int index, IEnumerable<T> items)
        {
            throw new NotImplementedException();
        }

        public void OverwriteCore(int index, IEnumerable<T> items)
        {
            throw new NotImplementedException();
        }

        public void MoveCore(int oldIndex, int newIndex)
        {
            throw new NotImplementedException();
        }

        public void MoveRangeCore(int oldIndex, int newIndex, int count)
        {
            throw new NotImplementedException();
        }

        public void RemoveCore(int index)
        {
            throw new NotImplementedException();
        }

        public void RemoveRangeCore(int index, int count)
        {
            throw new NotImplementedException();
        }

        public void AdjustLengthCore(int length)
        {
            throw new NotImplementedException();
        }

        public void AdjustLengthIfShortCore(int length)
        {
            throw new NotImplementedException();
        }

        public void AdjustLengthIfLongCore(int length)
        {
            throw new NotImplementedException();
        }

        public void ResetCore(IEnumerable<T> items)
        {
            throw new NotImplementedException();
        }

        public void ClearCore()
        {
            throw new NotImplementedException();
        }

        public bool ItemEquals(IRestrictedCapacityList<T>? other)
        {
            throw new NotImplementedException();
        }

        public IRestrictedCapacityList<T> DeepClone()
        {
            throw new NotImplementedException();
        }
    }
}
