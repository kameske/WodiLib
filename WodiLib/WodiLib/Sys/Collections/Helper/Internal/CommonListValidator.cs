// ========================================
// Project Name : WodiLib
// File Name    : CommonListValidator.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;

namespace WodiLib.Sys
{
    /// <summary>
    /// リスト編集メソッドの引数汎用検証処理実施クラス
    /// </summary>
    internal class CommonListValidator<T> : WodiLibListValidatorTemplate<T>
    {
        protected override IWodiLibListValidator<T>? BaseValidator => null;

        public CommonListValidator(IReadOnlyExtendedList<T> target) : base(target)
        {
        }

        public override void Constructor(IReadOnlyList<T> initItems)
        {
            if (initItems is null) throw new ArgumentNullException(ErrorMessage.NotNull(nameof(initItems)));
            ListValidationHelper.ItemsHasNotNull(initItems, nameof(initItems));
        }

        public override void Get(int index, int count)
        {
            ListValidationHelper.SelectIndex(index, Target.Count);
            ListValidationHelper.Count(count, Target.Count);
            ListValidationHelper.Range(index, count, Target.Count);
        }

        public override void Set(int index, T item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(ErrorMessage.NotNull(nameof(item)));
            }

            Set(index, new[] {item}!);
        }

        public override void Set(int index, IReadOnlyList<T> items)
        {
            ListValidationHelper.SelectIndex(index, Target.Count);
            ListValidationHelper.ItemsHasNotNull(items);
            ListValidationHelper.Range(index, items.Count, Target.Count);
        }

        public override void Insert(int index, T item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(ErrorMessage.NotNull(nameof(item)));
            }

            Insert(index, new[] {item}!);
        }

        public override void Insert(int index, IReadOnlyList<T> items)
        {
            if (items is null) throw new ArgumentNullException(ErrorMessage.NotNull(nameof(items)));
            ListValidationHelper.InsertIndex(index, Target.Count);
            ListValidationHelper.ItemsHasNotNull(items);
        }

        public override void Overwrite(int index, IReadOnlyList<T> items)
        {
            if (items is null) throw new ArgumentNullException(ErrorMessage.NotNull(nameof(items)));
            ListValidationHelper.InsertIndex(index, Target.Count);
            ListValidationHelper.ItemsHasNotNull(items);
        }

        public override void Move(int oldIndex, int newIndex, int count)
        {
            ListValidationHelper.ItemCountNotZero(Target.Count);
            ListValidationHelper.SelectIndex(oldIndex, Target.Count, nameof(oldIndex));
            ListValidationHelper.InsertIndex(newIndex, Target.Count, nameof(newIndex));
            ListValidationHelper.Count(count, Target.Count);
            ListValidationHelper.Range(oldIndex, count, Target.Count, nameof(oldIndex));
            ListValidationHelper.Range(count, newIndex, Target.Count, nameof(count), nameof(newIndex));
        }

        public override void Remove(int index, int count)
        {
            ListValidationHelper.SelectIndex(index, Target.Count);
            ListValidationHelper.Count(count, Target.Count);
            ListValidationHelper.Range(index, count, Target.Count);
        }

        public override void AdjustLength(int length)
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.GreaterOrEqual(nameof(length), 0, length));
            }
        }

        public override void AdjustLengthIfShort(int length)
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.GreaterOrEqual(nameof(length), 0, length));
            }
        }

        public override void AdjustLengthIfLong(int length)
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.GreaterOrEqual(nameof(length), 0, length));
            }
        }

        public override void Reset(IReadOnlyList<T> items)
        {
            if (items is null) throw new ArgumentNullException(ErrorMessage.NotNull(nameof(items)));
            ListValidationHelper.ItemsHasNotNull(items);
        }
    }
}
