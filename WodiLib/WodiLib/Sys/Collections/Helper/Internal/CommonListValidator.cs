// ========================================
// Project Name : WodiLib
// File Name    : CommonListValidator.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     リスト編集メソッドの引数汎用検証処理実施クラス
    /// </summary>
    /// <typeparam name="TIn">リスト要素入力型</typeparam>
    /// <typeparam name="TOut">リスト要素出力型</typeparam>
    internal class CommonListValidator<TIn, TOut> : WodiLibListValidatorTemplate<TIn, TOut>
        where TOut : TIn
    {
        protected override IWodiLibListValidator<TIn>? BaseValidator => null;

        public CommonListValidator(IReadOnlyExtendedList<TIn, TOut> target) : base(target)
        {
        }

        public override void Constructor(NamedValue<IReadOnlyList<TIn>> initItems)
        {
            ThrowHelper.ValidateArgumentNotNull(initItems.Value is null, nameof(initItems));
            ListValidationHelper.ItemsHasNotNull(initItems.Cast<IEnumerable<TIn>>());
        }

        public override void Get(NamedValue<int> index, NamedValue<int> count)
        {
            ListValidationHelper.SelectIndex(index, ($"{nameof(Target)}.{nameof(Target.Count)}", Target.Count));
            ListValidationHelper.Count(count, ($"{nameof(Target)}.{nameof(Target.Count)}", Target.Count));
            ListValidationHelper.Range(index, count, ($"{nameof(Target)}.{nameof(Target.Count)}", Target.Count));
        }

        public override void Set(NamedValue<int> index, NamedValue<TIn> item)
        {
            ThrowHelper.ValidateArgumentNotNull(item.Value is null, nameof(item));
            ListValidationHelper.SelectIndex(index, ($"{nameof(Target)}.{nameof(Target.Count)}", Target.Count));
        }

        public override void Set(NamedValue<int> index, NamedValue<IReadOnlyList<TIn>> items)
        {
            ListValidationHelper.SelectIndex(index, ($"{nameof(Target)}.{nameof(Target.Count)}", Target.Count));
            ListValidationHelper.ItemsHasNotNull(items.Cast<IEnumerable<TIn>>());
            ListValidationHelper.Range(
                index,
                ($"{nameof(items)}.{nameof(items.Value.Count)}", items.Value.Count),
                ($"{nameof(Target)}.{nameof(Target.Count)}", Target.Count)
            );
        }

        public override void Insert(NamedValue<int> index, NamedValue<TIn> item)
        {
            ThrowHelper.ValidateArgumentNotNull(item.Value is null, nameof(item));
            ListValidationHelper.InsertIndex(index, ($"{nameof(Target)}.{nameof(Target.Count)}", Target.Count));
        }

        public override void Insert(NamedValue<int> index, NamedValue<IReadOnlyList<TIn>> items)
        {
            ThrowHelper.ValidateArgumentNotNull(items.Value is null, items.Name);
            ListValidationHelper.InsertIndex(index, ($"{nameof(Target)}.{nameof(Target.Count)}", Target.Count));
            ListValidationHelper.ItemsHasNotNull(items.Cast<IEnumerable<TIn>>());
        }

        public override void Overwrite(NamedValue<int> index, NamedValue<IReadOnlyList<TIn>> items)
        {
            ThrowHelper.ValidateArgumentNotNull(items is null, nameof(items));
            ListValidationHelper.InsertIndex(index, ($"{nameof(Target)}.{nameof(Target.Count)}", Target.Count));
            ListValidationHelper.ItemsHasNotNull(items.Cast<IEnumerable<TIn>>());
        }

        public override void Move(NamedValue<int> oldIndex, NamedValue<int> newIndex, NamedValue<int> count)
        {
            ListValidationHelper.ItemCountNotZero(($"{nameof(Target)}.{nameof(Target.Count)}", Target.Count));
            ListValidationHelper.SelectIndex(oldIndex, ($"{nameof(Target)}.{nameof(Target.Count)}", Target.Count));
            ListValidationHelper.InsertIndex(newIndex, ($"{nameof(Target)}.{nameof(Target.Count)}", Target.Count));
            ListValidationHelper.Count(count, ($"{nameof(Target)}.{nameof(Target.Count)}", Target.Count));
            ListValidationHelper.Range(oldIndex, count, ($"{nameof(Target)}.{nameof(Target.Count)}", Target.Count));
            ListValidationHelper.Range(count, newIndex, ($"{nameof(Target)}.{nameof(Target.Count)}", Target.Count));
        }

        public override void Remove(NamedValue<int> index, NamedValue<int> count)
        {
            ListValidationHelper.SelectIndex(index, ($"{nameof(Target)}.{nameof(Target.Count)}", Target.Count));
            ListValidationHelper.Count(count, ($"{nameof(Target)}.{nameof(Target.Count)}", Target.Count));
            ListValidationHelper.Range(index, count, ($"{nameof(Target)}.{nameof(Target.Count)}", Target.Count));
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

        public override void AdjustLengthIfShort(NamedValue<int> length)
        {
            ThrowHelper.ValidateArgumentValueGreaterOrEqual(
                length.Value < 0,
                nameof(length),
                0,
                length.Value
            );
        }

        public override void AdjustLengthIfLong(NamedValue<int> length)
        {
            ThrowHelper.ValidateArgumentValueGreaterOrEqual(
                length.Value < 0,
                nameof(length),
                0,
                length.Value
            );
        }

        public override void Reset(NamedValue<IReadOnlyList<TIn>> items)
        {
            ThrowHelper.ValidateArgumentNotNull(items is null, nameof(items));
            ListValidationHelper.ItemsHasNotNull(items.Cast<IEnumerable<TIn>>());
        }
    }
}
