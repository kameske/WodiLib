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
    internal class CommonListValidator<T> : IExtendedListValidator<T>
    {
        private IReadOnlyExtendedList<T> Target { get; }

        public CommonListValidator(IReadOnlyExtendedList<T> target)
        {
            Target = target;
        }

        public void Constructor(params T[] initItems)
        {
            ListValidationHelper.ItemsHasNotNull(initItems, nameof(initItems));
        }

        public void Get(int index, int count)
        {
            ListValidationHelper.SelectIndex(index, Target.Count);
            ListValidationHelper.Count(count, Target.Count);
            ListValidationHelper.Range(index, count, Target.Count);
        }

        public void Set(int index, params T[] items)
        {
            ListValidationHelper.SelectIndex(index, Target.Count);
            ListValidationHelper.ItemsHasNotNull(items);
            ListValidationHelper.Range(index, items.Length, Target.Count);
        }

        public void Insert(int index, params T[] items)
        {
            ListValidationHelper.InsertIndex(index, Target.Count);
            ListValidationHelper.ItemsHasNotNull(items);
        }

        public void Overwrite(int index, params T[] items)
        {
            ListValidationHelper.InsertIndex(index, Target.Count);
            ListValidationHelper.ItemsHasNotNull(items);
        }

        public void Move(int oldIndex, int newIndex, int count)
        {
            ListValidationHelper.ItemCountNotZero(Target.Count);
            ListValidationHelper.SelectIndex(oldIndex, Target.Count, nameof(oldIndex));
            ListValidationHelper.InsertIndex(newIndex, Target.Count, nameof(newIndex));
            ListValidationHelper.Count(count, Target.Count);
            ListValidationHelper.Range(oldIndex, count, Target.Count, nameof(oldIndex));
            ListValidationHelper.Range(count, newIndex, Target.Count, nameof(count), nameof(newIndex));
        }

        public void Remove([AllowNull] T item)
        {
        }

        public void Remove(int index, int count)
        {
            ListValidationHelper.SelectIndex(index, Target.Count);
            ListValidationHelper.Count(count, Target.Count);
            ListValidationHelper.Range(index, count, Target.Count);
        }

        public void AdjustLength(int length)
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.GreaterOrEqual(nameof(length), 0, length));
            }
        }

        public void AdjustLengthIfShort(int length)
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.GreaterOrEqual(nameof(length), 0, length));
            }
        }

        public void AdjustLengthIfLong(int length)
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.GreaterOrEqual(nameof(length), 0, length));
            }
        }

        public void Reset(params T[] items)
        {
            ListValidationHelper.ItemsHasNotNull(items);
        }
    }
}
