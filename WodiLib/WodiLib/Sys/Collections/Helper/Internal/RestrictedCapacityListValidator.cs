// ========================================
// Project Name : WodiLib
// File Name    : CommonListValidator.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Diagnostics.CodeAnalysis;

namespace WodiLib.Sys
{
    /// <summary>
    /// リスト編集メソッドの引数汎用検証処理実施クラス
    /// </summary>
    internal class RestrictedCapacityListValidator<T> : IExtendedListValidator<T>
    {
        private IReadOnlyRestrictedCapacityList<T> Target { get; }

        private CommonListValidator<T> PreConditionValidator { get; }

        public RestrictedCapacityListValidator(IReadOnlyRestrictedCapacityList<T> target)
        {
            Target = target;
            PreConditionValidator = new CommonListValidator<T>(target);
        }

        public void Constructor(params T[] initItems)
        {
#if DEBUG
            try
            {
                RestrictedListValidationHelper.CapacityConfig(
                    Target.GetMinCapacity(), Target.GetMaxCapacity());
            }
            catch (Exception ex)
            {
                throw new TypeInitializationException(GetType().Name, ex);
            }
#endif

            PreConditionValidator.Constructor(initItems);
            RestrictedListValidationHelper.ItemCount(
                initItems.Length, Target.GetMinCapacity(), Target.GetMaxCapacity());
        }

        public void Get(int index, int count)
        {
            PreConditionValidator.Get(index, count);
        }

        public void Set(int index, params T[] items)
        {
            PreConditionValidator.Set(index, items);
        }

        public void Insert(int index, params T[] items)
        {
            PreConditionValidator.Insert(index, items);
            RestrictedListValidationHelper.ItemMaxCount(
                Target.Count + items.Length, Target.GetMaxCapacity());
        }

        public void Overwrite(int index, params T[] items)
        {
            PreConditionValidator.Overwrite(index, items);
            if (index + items.Length > Target.Count)
            {
                RestrictedListValidationHelper.ItemMaxCount(
                    index + items.Length, Target.GetMaxCapacity());
            }
        }

        public void Move(int oldIndex, int newIndex, int count)
        {
            PreConditionValidator.Move(oldIndex, newIndex, count);
        }

        public void Remove([AllowNull] T item)
        {
            PreConditionValidator.Remove(item);
        }

        public void Remove(int index, int count)
        {
            PreConditionValidator.Remove(index, count);
            RestrictedListValidationHelper.ItemMinCount(
                Target.Count - count, Target.GetMinCapacity());
        }

        public void AdjustLength(int length)
        {
            PreConditionValidator.AdjustLength(length);
            RestrictedListValidationHelper.ItemCount(
                length, Target.GetMinCapacity(), Target.GetMaxCapacity(),
                nameof(length));
        }

        public void AdjustLengthIfShort(int length)
        {
            PreConditionValidator.AdjustLengthIfShort(length);
            RestrictedListValidationHelper.ItemCount(
                length, Target.GetMinCapacity(), Target.GetMaxCapacity(),
                nameof(length));
        }

        public void AdjustLengthIfLong(int length)
        {
            PreConditionValidator.AdjustLengthIfLong(length);
            RestrictedListValidationHelper.ItemCount(
                length, Target.GetMinCapacity(), Target.GetMaxCapacity(),
                nameof(length));
        }

        public void Reset(params T[] items)
        {
            PreConditionValidator.Reset(items);
            RestrictedListValidationHelper.ItemCount(
                items.Length, Target.GetMinCapacity(), Target.GetMaxCapacity(),
                nameof(items));
        }
    }
}
