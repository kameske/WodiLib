// ========================================
// Project Name : WodiLib
// File Name    : FixedLengthListValidator.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;

namespace WodiLib.Sys.Collections
{
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

        public FixedLengthListValidator(IFixedLengthList<TIn, TOut> target) : base(target)
        {
            Target = target;
            BaseValidator = new CommonListValidator<TIn, TOut>(target);
        }

        public override void Constructor(IReadOnlyList<TIn> initItems)
        {
            BaseValidator?.Constructor(initItems);
        }

        public override void Reset(IReadOnlyList<TIn> items)
        {
            BaseValidator?.Reset(items);
            FixedLengthListValidationHelper.ItemCount(items.Count, Target.Count);
        }

        public override void Insert(int index, TIn item)
            => throw new NotSupportedException();

        public override void Insert(int index, IReadOnlyList<TIn> items)
            => throw new NotSupportedException();

        public override void Overwrite(int index, IReadOnlyList<TIn> items)
            => throw new NotSupportedException();

        public override void Remove(TIn? item)
            => throw new NotSupportedException();

        public override void Remove(int index, int count)
            => throw new NotSupportedException();

        public override void AdjustLength(int length)
            => throw new NotSupportedException();

        public override void AdjustLengthIfShort(int length)
            => throw new NotSupportedException();

        public override void AdjustLengthIfLong(int length)
            => throw new NotSupportedException();
    }

    /// <summary>
    ///     IFixedLengthList の基本検証処理実施クラス
    /// </summary>
    [Obsolete]
    internal class FixedLengthListValidator<T> : WodiLibListValidatorTemplate<T>
    {
        protected override IWodiLibListValidator<T>? BaseValidator { get; }

        private new IFixedLengthList<T> Target { get; }

        public FixedLengthListValidator(IFixedLengthList<T> target) : base(target.AsReadableList())
        {
            Target = target;
            BaseValidator = new CommonListValidator<T>(target.AsReadableList());
        }

        public override void Constructor(IReadOnlyList<T> initItems)
        {
            BaseValidator?.Constructor(initItems);
        }

        public override void Reset(IReadOnlyList<T> items)
        {
            BaseValidator?.Reset(items);
            FixedLengthListValidationHelper.ItemCount(items.Count, Target.Count);
        }

        public override void Insert(int index, T item)
            => throw new NotSupportedException();

        public override void Insert(int index, IReadOnlyList<T> items)
            => throw new NotSupportedException();

        public override void Overwrite(int index, IReadOnlyList<T> items)
            => throw new NotSupportedException();

        public override void Remove(T? item)
            => throw new NotSupportedException();

        public override void Remove(int index, int count)
            => throw new NotSupportedException();

        public override void AdjustLength(int length)
            => throw new NotSupportedException();

        public override void AdjustLengthIfShort(int length)
            => throw new NotSupportedException();

        public override void AdjustLengthIfLong(int length)
            => throw new NotSupportedException();
    }
}
