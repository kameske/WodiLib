// ========================================
// Project Name : WodiLib
// File Name    : FixedLengthListValidator.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace WodiLib.Sys
{
    /// <summary>
    ///     IFixedLengthList の基本検証処理実施クラス
    /// </summary>
    internal class FixedLengthListValidator<T> : WodiLibListValidatorTemplate<T>
    {
        protected override IWodiLibListValidator<T>? BaseValidator { get; }

        private new IReadOnlyFixedLengthList<T> Target { get; }

        public FixedLengthListValidator(IReadOnlyFixedLengthList<T> target) : base(target)
        {
            Target = target;
            BaseValidator = new CommonListValidator<T>(target);
        }

        public override void Constructor(IReadOnlyList<T> initItems)
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
            BaseValidator?.Constructor(initItems);
            FixedLengthListValidationHelper.ItemCount(initItems.Count, Target.GetCapacity());
        }

        public override void Reset(IReadOnlyList<T> items)
        {
            BaseValidator?.Reset(items);
            FixedLengthListValidationHelper.ItemCount(items.Count, Target.GetCapacity());
        }

        public override void Insert(int index, T item)
            => throw new NotSupportedException();

        public override void Insert(int index, IReadOnlyList<T> items)
            => throw new NotSupportedException();

        public override void Overwrite(int index, IReadOnlyList<T> items)
            => throw new NotSupportedException();

        public override void Remove([AllowNull] T item)
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
