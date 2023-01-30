// ========================================
// Project Name : WodiLib
// File Name    : CommonListValidator.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     リスト編集メソッドの引数汎用検証処理実施クラス
    /// </summary>
    /// <typeparam name="T">リスト要素入力型</typeparam>
    internal class CommonListValidator<T> : WodiLibListValidatorTemplate<T>
    {
        protected override IWodiLibListValidator<T>? BaseValidator => null;

        public CommonListValidator(IRestrictedCapacityList<T> target) : base(new TargetAdapter(target))
        {
        }

        public CommonListValidator(IFixedLengthList<T> target, int capacity) : base(
            new TargetAdapter(target, capacity)
        )
        {
        }

        public CommonListValidator(IFixedLengthList<T> target, Func<int> capacityGetter) : base(
            new TargetAdapter(target, capacityGetter)
        )
        {
        }

        public CommonListValidator(IReadOnlyExtendedList<T> target) : base(new TargetAdapter(target))
        {
        }

        public CommonListValidator(IEnumerable target, int minCapacity, int maxCapacity) : base(
            new TargetAdapter(target, minCapacity, maxCapacity)
        )
        {
        }

        public override void Constructor(NamedValue<IEnumerable<T>> initItems)
        {
            ThrowHelper.ValidateArgumentNotNull(initItems.Value is null, initItems.Name);
            ListValidationHelper.ItemsHasNotNull(initItems);
        }

        public override void Get(NamedValue<int> index, NamedValue<int> count)
        {
            var namedCount = new NamedValue<int>(
                $"{nameof(Target)}.{nameof(IExtendedList<object>.Count)}",
                Target.GetCount()
            );
            ListValidationHelper.SelectIndex(index, namedCount);
            ListValidationHelper.Count(count, namedCount);
            ListValidationHelper.Range(index, count, namedCount);
        }

        public override void Set(NamedValue<int> index, NamedValue<IEnumerable<T>> items)
        {
            var namedCount = new NamedValue<int>(
                $"{nameof(Target)}.{nameof(IExtendedList<object>.Count)}",
                Target.GetCount()
            );
            ListValidationHelper.SelectIndex(index, namedCount);
            ThrowHelper.ValidateArgumentNotNull(items.Value is null, items.Name);
            ListValidationHelper.ItemsHasNotNull(items);
            ListValidationHelper.Range(
                index,
                ($"{nameof(items)}.{nameof(items.Value)}の要素数", items.Value.Count()),
                namedCount
            );
        }

        public override void Insert(NamedValue<int> index, NamedValue<IEnumerable<T>> items)
        {
            ThrowHelper.ValidateArgumentNotNull(items.Value is null, items.Name);
            ListValidationHelper.ItemsHasNotNull(items);
            ListValidationHelper.InsertIndex(
                index,
                ($"{nameof(Target)}.{nameof(IExtendedList<object>.Count)}", Target.GetCount())
            );
        }

        public override void Overwrite(NamedValue<int> index, NamedValue<IEnumerable<T>> items)
        {
            ThrowHelper.ValidateArgumentNotNull(items.Value is null, items.Name);
            ListValidationHelper.ItemsHasNotNull(items);
            ListValidationHelper.InsertIndex(
                index,
                ($"{nameof(Target)}.{nameof(IExtendedList<object>.Count)}", Target.GetCount())
            );
        }

        public override void Move(NamedValue<int> oldIndex, NamedValue<int> newIndex, NamedValue<int> count)
        {
            var namedCount = new NamedValue<int>(
                $"{nameof(Target)}.{nameof(IExtendedList<object>.Count)}",
                Target.GetCount()
            );
            ListValidationHelper.ItemCountNotZero(namedCount);
            ListValidationHelper.SelectIndex(oldIndex, namedCount);
            ListValidationHelper.SelectIndex(newIndex, namedCount);
            ListValidationHelper.Count(count, namedCount);
            ListValidationHelper.Range(oldIndex, count, namedCount);
            ListValidationHelper.Range(count, newIndex, namedCount);
        }

        public override void Remove(NamedValue<int> index, NamedValue<int> count)
        {
            var namedCount = new NamedValue<int>(
                $"{nameof(Target)}.{nameof(IExtendedList<object>.Count)}",
                Target.GetCount()
            );
            ListValidationHelper.SelectIndex(index, namedCount);
            ListValidationHelper.Count(count, namedCount);
            ListValidationHelper.Range(index, count, namedCount);
        }

        public override void AdjustLength(NamedValue<int> length)
        {
            ThrowHelper.ValidateArgumentValueGreaterOrEqual(
                length.Value < 0,
                nameof(length),
                0,
                length.Value
            );
        }

        public override void Reset(NamedValue<IEnumerable<T>> items)
        {
            ThrowHelper.ValidateArgumentNotNull(items.Value is null, items.Name);
            ListValidationHelper.ItemsHasNotNull(items);
        }
    }
}
