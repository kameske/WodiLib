// ========================================
// Project Name : WodiLib
// File Name    : FixedLengthListValidator.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Linq;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     IFixedLengthList の基本検証処理実施クラス
    /// </summary>
    /// <typeparam name="T">リスト要素入力型</typeparam>
    internal class FixedLengthListValidator<T> : WodiLibListValidatorTemplate<T>
    {
        protected override IWodiLibListValidator<T>? BaseValidator { get; }

        private ICapacityGetter Capacity { get; }

        public FixedLengthListValidator(IFixedLengthList<T> target, int capacity) : base(
            new TargetAdapter(target, capacity)
        )
        {
            Capacity = new ICapacityGetter.ForStaticValue(capacity);
            BaseValidator = new CommonListValidator<T>(target, capacity);
        }

        public FixedLengthListValidator(IFixedLengthList<T> target, Func<int> capacityGetter) : base(
            new TargetAdapter(target, capacityGetter)
        )
        {
            Capacity = new ICapacityGetter.ForGetter(capacityGetter);
            BaseValidator = new CommonListValidator<T>(target, capacityGetter);
        }

        public override void Constructor(NamedValue<IEnumerable<T>> initItems)
        {
            BaseValidator?.Constructor(initItems);
            FixedLengthListValidationHelper.ItemCount(initItems.Value.Count(), Capacity.Get());
        }

        public override void Reset(NamedValue<IEnumerable<T>> items)
        {
            BaseValidator?.Reset(items);
            FixedLengthListValidationHelper.ItemCount(items.Value.Count(), Capacity.Get());
        }

        public override void Insert(NamedValue<int> index, NamedValue<IEnumerable<T>> items)
            => throw new NotSupportedException();

        public override void Overwrite(NamedValue<int> index, NamedValue<IEnumerable<T>> items)
            => throw new NotSupportedException();

        public override void Remove(NamedValue<int> index, NamedValue<int> count)
            => throw new NotSupportedException();

        public override void AdjustLength(NamedValue<int> length)
            => throw new NotSupportedException();

        private interface ICapacityGetter
        {
            public int Get();

            public class ForStaticValue : ICapacityGetter
            {
                private readonly int value;

                public ForStaticValue(int capacity)
                {
                    value = capacity;
                }

                public int Get() => value;
            }

            public class ForGetter : ICapacityGetter
            {
                private readonly Func<int> getter;

                public ForGetter(Func<int> getter)
                {
                    this.getter = getter;
                }

                public int Get() => getter.Invoke();
            }
        }
    }
}
