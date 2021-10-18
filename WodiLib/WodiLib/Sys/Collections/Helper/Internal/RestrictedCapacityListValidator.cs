// ========================================
// Project Name : WodiLib
// File Name    : CommonListValidator.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     リスト編集メソッドの引数汎用検証処理実施クラス
    /// </summary>
    internal class RestrictedCapacityListValidator<TIn, TOut> : WodiLibListValidatorTemplate<TIn, TOut>
        where TOut : TIn
    {
        private new IRestrictedCapacityList<TIn, TOut> Target { get; }

        protected override IWodiLibListValidator<TIn>? BaseValidator { get; }

        public RestrictedCapacityListValidator(IRestrictedCapacityList<TIn, TOut> target) : base(target)
        {
            Target = target;
            BaseValidator = new CommonListValidator<TIn, TOut>(target);
        }

        public override void Constructor(IReadOnlyList<TIn> initItems)
        {
#if DEBUG
            try
            {
                RestrictedListValidationHelper.CapacityConfig(
                    Target.GetMinCapacity(), Target.GetMaxCapacity());
            }
            catch (Exception ex)
            {
                throw new TypeInitializationException(GetType().Name, ex);
            }
#endif

            BaseValidator?.Constructor(initItems);
            RestrictedListValidationHelper.ArgumentItemsCount(
                initItems.Count, Target.GetMinCapacity(), Target.GetMaxCapacity());
        }

        public override void Insert(int index, TIn item) => Insert(index, new[] { item });

        public override void Insert(int index, IReadOnlyList<TIn> items)
        {
            BaseValidator?.Insert(index, items);
            RestrictedListValidationHelper.ItemMaxCount(
                Target.Count + items.Count, Target.GetMaxCapacity());
        }

        public override void Overwrite(int index, IReadOnlyList<TIn> items)
        {
            BaseValidator?.Overwrite(index, items);
            if (index + items.Count > Target.Count)
            {
                RestrictedListValidationHelper.ItemMaxCount(
                    index + items.Count, Target.GetMaxCapacity());
            }
        }

        public override void Remove(TIn? item)
        {
            if (item is null) return;
            var index = Target.FindIndex(tItem => ReferenceEquals(tItem, item));
            if (index == -1) return;

            BaseValidator?.Remove(item);
            RestrictedListValidationHelper.ItemMinCount(
                Target.Count - 1, Target.GetMinCapacity());
        }

        public override void Remove(int index, int count)
        {
            BaseValidator?.Remove(index, count);
            RestrictedListValidationHelper.ItemMinCount(
                Target.Count - count, Target.GetMinCapacity());
        }

        public override void AdjustLength(int length)
        {
            BaseValidator?.AdjustLength(length);
            RestrictedListValidationHelper.ArgumentItemsCount(
                length, Target.GetMinCapacity(), Target.GetMaxCapacity(),
                nameof(length));
        }

        public override void AdjustLengthIfShort(int length)
        {
            BaseValidator?.AdjustLengthIfShort(length);
            RestrictedListValidationHelper.ArgumentItemsCount(
                length, Target.GetMinCapacity(), Target.GetMaxCapacity(),
                nameof(length));
        }

        public override void AdjustLengthIfLong(int length)
        {
            BaseValidator?.AdjustLengthIfLong(length);
            RestrictedListValidationHelper.ArgumentItemsCount(
                length, Target.GetMinCapacity(), Target.GetMaxCapacity(),
                nameof(length));
        }

        public override void Reset(IReadOnlyList<TIn> items)
        {
            BaseValidator?.Reset(items);
            RestrictedListValidationHelper.ArgumentItemsCount(
                items.Count, Target.GetMinCapacity(), Target.GetMaxCapacity(),
                nameof(items));
        }
    }

    /// <summary>
    ///     リスト編集メソッドの引数汎用検証処理実施クラス
    /// </summary>
    [Obsolete]
    internal class RestrictedCapacityListValidator<T> : WodiLibListValidatorTemplate<T>
    {
        private new IRestrictedCapacityList<T> Target { get; }

        protected override IWodiLibListValidator<T>? BaseValidator { get; }

        public RestrictedCapacityListValidator(IRestrictedCapacityList<T> target) : base(target.AsReadableList())
        {
            Target = target;
            BaseValidator = new CommonListValidator<T>(target.AsReadableList());
        }

        public override void Constructor(IReadOnlyList<T> initItems)
        {
#if DEBUG
            try
            {
                RestrictedListValidationHelper.CapacityConfig(
                    Target.GetMinCapacity(), Target.GetMaxCapacity());
            }
            catch (Exception ex)
            {
                throw new TypeInitializationException(GetType().Name, ex);
            }
#endif

            BaseValidator?.Constructor(initItems);
            RestrictedListValidationHelper.ArgumentItemsCount(
                initItems.Count, Target.GetMinCapacity(), Target.GetMaxCapacity());
        }

        public override void Insert(int index, T item) => Insert(index, new[] { item });

        public override void Insert(int index, IReadOnlyList<T> items)
        {
            BaseValidator?.Insert(index, items);
            RestrictedListValidationHelper.ItemMaxCount(
                Target.Count + items.Count, Target.GetMaxCapacity());
        }

        public override void Overwrite(int index, IReadOnlyList<T> items)
        {
            BaseValidator?.Overwrite(index, items);
            if (index + items.Count > Target.Count)
            {
                RestrictedListValidationHelper.ItemMaxCount(
                    index + items.Count, Target.GetMaxCapacity());
            }
        }

        public override void Remove(T? item)
        {
            if (item is null) return;
            var index = Target.IndexOf(item);
            if (index == -1) return;

            BaseValidator?.Remove(item);
            RestrictedListValidationHelper.ItemMinCount(
                Target.Count - 1, Target.GetMinCapacity());
        }

        public override void Remove(int index, int count)
        {
            BaseValidator?.Remove(index, count);
            RestrictedListValidationHelper.ItemMinCount(
                Target.Count - count, Target.GetMinCapacity());
        }

        public override void AdjustLength(int length)
        {
            BaseValidator?.AdjustLength(length);
            RestrictedListValidationHelper.ArgumentItemsCount(
                length, Target.GetMinCapacity(), Target.GetMaxCapacity(),
                nameof(length));
        }

        public override void AdjustLengthIfShort(int length)
        {
            BaseValidator?.AdjustLengthIfShort(length);
            RestrictedListValidationHelper.ArgumentItemsCount(
                length, Target.GetMinCapacity(), Target.GetMaxCapacity(),
                nameof(length));
        }

        public override void AdjustLengthIfLong(int length)
        {
            BaseValidator?.AdjustLengthIfLong(length);
            RestrictedListValidationHelper.ArgumentItemsCount(
                length, Target.GetMinCapacity(), Target.GetMaxCapacity(),
                nameof(length));
        }

        public override void Reset(IReadOnlyList<T> items)
        {
            BaseValidator?.Reset(items);
            RestrictedListValidationHelper.ArgumentItemsCount(
                items.Count, Target.GetMinCapacity(), Target.GetMaxCapacity(),
                nameof(items));
        }
    }
}
