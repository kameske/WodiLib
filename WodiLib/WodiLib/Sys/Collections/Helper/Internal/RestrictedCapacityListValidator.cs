// ========================================
// Project Name : WodiLib
// File Name    : CommonListValidator.cs
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
    ///     リスト編集メソッドの引数汎用検証処理実施クラス
    /// </summary>
    internal class RestrictedCapacityListValidator<T> : RestrictedCapacityListValidator<T, T>
    {
        public RestrictedCapacityListValidator(IRestrictedCapacityList<T, T> target) : base(target)
        {
        }
    }

    /// <summary>
    ///     リスト編集メソッドの引数汎用検証処理実施クラス
    /// </summary>
    internal class RestrictedCapacityListValidator<TIn, TOut> : WodiLibListValidatorTemplate<TIn, TOut>
        where TOut : TIn
    {
        private new IRestrictedCapacityList<TIn, TOut> Target { get; }

        protected override IWodiLibListValidator<TIn>? BaseValidator { get; }

        protected virtual string ListItemsName => "要素数";

        public RestrictedCapacityListValidator(IRestrictedCapacityList<TIn, TOut> target) : base(target)
        {
            Target = target;
            BaseValidator = new CommonListValidator<TIn, TOut>(target);
        }

        public override void Constructor(NamedValue<IEnumerable<TIn>> initItems)
        {
#if DEBUG
            try
            {
                RestrictedListValidationHelper.CapacityConfig(
                    ($"{Target.GetType().FullName}.{nameof(Target.GetMinCapacity)}", Target.GetMinCapacity()),
                    ($"{Target.GetType().FullName}.{nameof(Target.GetMaxCapacity)}", Target.GetMaxCapacity())
                );
            }
            catch (Exception ex)
            {
                throw new TypeInitializationException(GetType().Name, ex);
            }
#endif

            BaseValidator?.Constructor(initItems);
            RestrictedListValidationHelper.ArgumentItemsCount(
                initItems.Value.Count(),
                Target.GetMinCapacity(),
                Target.GetMaxCapacity()
            );
        }

        public override void Insert(NamedValue<int> index, NamedValue<TIn> item)
        {
            BaseValidator?.Insert(index, item);
            RestrictedListValidationHelper.ItemMaxCount(
                Target.Count + 1,
                Target.GetMaxCapacity()
            );
        }

        public override void Insert(NamedValue<int> index, NamedValue<IEnumerable<TIn>> items)
        {
            BaseValidator?.Insert(index, items);
            RestrictedListValidationHelper.ItemMaxCount(
                Target.Count + items.Value.Count(),
                Target.GetMaxCapacity()
            );
        }

        public override void Overwrite(NamedValue<int> index, NamedValue<IEnumerable<TIn>> items)
        {
            BaseValidator?.Overwrite(index, items);
            RestrictedListValidationHelper.OverwrittenCount(
                index.Value,
                items.Value.Count(),
                Target.Count,
                Target.GetMaxCapacity()
            );
        }

        public override void Remove(NamedValue<TIn?> item)
        {
            if (item.Value is null) return;
            var index = Target.FindIndex(tItem => ReferenceEquals(tItem, item.Value));
            if (index == -1) return;

            BaseValidator?.Remove(item);
            RestrictedListValidationHelper.ItemMinCount(
                (ListItemsName, Target.Count - 1),
                Target.GetMinCapacity()
            );
        }

        public override void Remove(NamedValue<int> index, NamedValue<int> count)
        {
            BaseValidator?.Remove(index, count);
            RestrictedListValidationHelper.ItemMinCount(
                (ListItemsName, Target.Count - count.Value),
                Target.GetMinCapacity()
            );
        }

        public override void AdjustLength(NamedValue<int> length)
        {
            BaseValidator?.AdjustLength(length);
            RestrictedListValidationHelper.ArgumentItemsCount(
                length.Value,
                Target.GetMinCapacity(),
                Target.GetMaxCapacity(),
                length.Name
            );
        }

        public override void AdjustLengthIfShort(NamedValue<int> length)
        {
            BaseValidator?.AdjustLengthIfShort(length);
            RestrictedListValidationHelper.ArgumentItemsCount(
                length.Value,
                Target.GetMinCapacity(),
                Target.GetMaxCapacity(),
                length.Name
            );
        }

        public override void AdjustLengthIfLong(NamedValue<int> length)
        {
            BaseValidator?.AdjustLengthIfLong(length);
            RestrictedListValidationHelper.ArgumentItemsCount(
                length.Value,
                Target.GetMinCapacity(),
                Target.GetMaxCapacity(),
                length.Name
            );
        }

        public override void Reset(NamedValue<IEnumerable<TIn>> items)
        {
            BaseValidator?.Reset(items);
            RestrictedListValidationHelper.ArgumentItemsCount(
                items.Value.Count(),
                Target.GetMinCapacity(),
                Target.GetMaxCapacity(),
                items.Name
            );
        }
    }
}
