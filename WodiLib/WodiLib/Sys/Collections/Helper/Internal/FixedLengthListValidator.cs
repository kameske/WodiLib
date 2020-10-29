// ========================================
// Project Name : WodiLib
// File Name    : FixedLengthListValidator.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Diagnostics.CodeAnalysis;

namespace WodiLib.Sys
{
    /// <summary>
    /// IFixedLengthList の基本検証処理実施クラス
    /// </summary>
    internal class FixedLengthListValidator<T> : IExtendedListValidator<T>
    {
        private IReadOnlyFixedLengthList<T> Target { get; }

        private CommonListValidator<T> PreConditionValidator { get; }

        public FixedLengthListValidator(IReadOnlyFixedLengthList<T> target)
        {
            Target = target;
            PreConditionValidator = new CommonListValidator<T>(target);
        }

        public void Constructor(params T[] initItems)
        {
#if DEBUG
            try
            {
                FixedLengthListValidationHelper.CapacityConfig(Target.GetCapacity());
            }
            catch (Exception ex)
            {
                throw new TypeInitializationException(Target.GetType().Name, ex);
            }
#endif
            PreConditionValidator.Constructor(initItems);
            FixedLengthListValidationHelper.ItemCount(initItems.Length, Target.GetCapacity());
        }

        public void Get(int index, int count)
        {
            PreConditionValidator.Get(index, count);
        }

        public void Set(int index, params T[] items)
        {
            PreConditionValidator.Set(index, items);
        }

        public void Move(int oldIndex, int newIndex, int count)
        {
            PreConditionValidator.Move(oldIndex, newIndex, count);
        }

        public void Reset(params T[] items)
        {
            PreConditionValidator.Reset(items);
            FixedLengthListValidationHelper.ItemCount(items.Length, Target.GetCapacity());
        }

        public void Insert(int index, params T[] items)
            => throw new NotSupportedException();

        public void Overwrite(int index, params T[] items)
            => throw new NotSupportedException();

        public void Remove([AllowNull] T item)
            => throw new NotSupportedException();

        public void Remove(int index, int count)
            => throw new NotSupportedException();

        public void AdjustLength(int length)
            => throw new NotSupportedException();

        public void AdjustLengthIfShort(int length)
            => throw new NotSupportedException();

        public void AdjustLengthIfLong(int length)
            => throw new NotSupportedException();
    }
}
