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
    internal sealed class RestrictedCapacityListValidator<T> : WodiLibListValidatorTemplate<T>
    {
        protected override IWodiLibListValidator<T>? BaseValidator { get; }

        private static string ListItemsName => "要素数";

        public RestrictedCapacityListValidator(IRestrictedCapacityList<T> target) : base(new TargetAdapter(target))
        {
            BaseValidator = new CommonListValidator<T>(target);
        }

        public RestrictedCapacityListValidator(IEnumerable<T> target, int minCapacity, int maxCapacity) : base(
            new TargetAdapter(target, minCapacity, maxCapacity)
        )
        {
            BaseValidator = new CommonListValidator<T>(target, minCapacity, maxCapacity);
        }

        public override void Constructor(NamedValue<IEnumerable<T>> initItems)
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

        public override void Insert(NamedValue<int> index, NamedValue<IEnumerable<T>> items)
        {
            BaseValidator?.Insert(index, items);
            RestrictedListValidationHelper.ItemMaxCount(
                Target.Count + items.Value.Count(),
                Target.GetMaxCapacity()
            );
        }

        public override void Overwrite(NamedValue<int> index, NamedValue<IEnumerable<T>> items)
        {
            BaseValidator?.Overwrite(index, items);
            RestrictedListValidationHelper.OverwrittenCount(
                index.Value,
                items.Value.Count(),
                Target.Count,
                Target.GetMaxCapacity()
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

        public override void Reset(NamedValue<IEnumerable<T>> items)
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
