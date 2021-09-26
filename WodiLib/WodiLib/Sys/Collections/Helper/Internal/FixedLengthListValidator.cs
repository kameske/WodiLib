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
