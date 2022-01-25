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
    /// <typeparam name="T">リスト要素型</typeparam>
    internal class FixedLengthListValidator<T> : FixedLengthListValidator<T, T>
    {
        public FixedLengthListValidator(IFixedLengthList<T, T> target, int capacity) : base(target, capacity)
        {
        }

        public FixedLengthListValidator(IFixedLengthList<T, T> target, Func<int> capacityGetter) : base(
            target,
            capacityGetter
        )
        {
        }
    }

    /// <summary>
    ///     IFixedLengthList の基本検証処理実施クラス
    /// </summary>
    /// <typeparam name="TIn">リスト要素入力型</typeparam>
    /// <typeparam name="TOut">リスト要素出力型</typeparam>
    internal class FixedLengthListValidator<TIn, TOut> : WodiLibListValidatorTemplate<TIn, TOut>
        where TOut : TIn
    {
        protected override IWodiLibListValidator<TIn>? BaseValidator { get; }

        private new IFixedLengthList<TIn, TOut> Target { get; }

        private ICapacityGetter Capacity { get; }

        public FixedLengthListValidator(IFixedLengthList<TIn, TOut> target, int capacity) : base(target)
        {
            Target = target;
            Capacity = new ICapacityGetter.ForStaticValue(capacity);
            BaseValidator = new CommonListValidator<TIn, TOut>(target);
        }

        public FixedLengthListValidator(IFixedLengthList<TIn, TOut> target, Func<int> capacityGetter) : base(target)
        {
            Target = target;
            Capacity = new ICapacityGetter.ForGetter(capacityGetter);
            BaseValidator = new CommonListValidator<TIn, TOut>(target);
        }

        public override void Constructor(NamedValue<IEnumerable<TIn>> initItems)
        {
            BaseValidator?.Constructor(initItems);
            FixedLengthListValidationHelper.ItemCount(initItems.Value.Count(), Capacity.Get());
        }

        public override void Reset(NamedValue<IEnumerable<TIn>> items)
        {
            BaseValidator?.Reset(items);
            FixedLengthListValidationHelper.ItemCount(items.Value.Count(), Capacity.Get());
        }

        public override void Insert(NamedValue<int> index, NamedValue<TIn> item)
            => throw new NotSupportedException();

        public override void Insert(NamedValue<int> index, NamedValue<IEnumerable<TIn>> items)
            => throw new NotSupportedException();

        public override void Overwrite(NamedValue<int> index, NamedValue<IEnumerable<TIn>> items)
            => throw new NotSupportedException();

        public override void Remove(NamedValue<TIn?> item)
            => throw new NotSupportedException();

        public override void Remove(NamedValue<int> index, NamedValue<int> count)
            => throw new NotSupportedException();

        public override void AdjustLength(NamedValue<int> length)
            => throw new NotSupportedException();

        public override void AdjustLengthIfShort(NamedValue<int> length)
            => throw new NotSupportedException();

        public override void AdjustLengthIfLong(NamedValue<int> length)
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
