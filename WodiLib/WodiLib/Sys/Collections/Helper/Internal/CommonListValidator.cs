// ========================================
// Project Name : WodiLib
// File Name    : CommonListValidator.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using System.Linq;

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

        public override void Constructor(NamedValue<IEnumerable<TIn>> initItems)
        {
            ThrowHelper.ValidateArgumentNotNull(initItems.Value is null, initItems.Name);
            ListValidationHelper.ItemsHasNotNull(initItems);
        }

        public override void Get(NamedValue<int> index, NamedValue<int> count)
        {
            var namedCount = ($"{nameof(Target)}.{nameof(Target.Count)}", Target.Count);
            ListValidationHelper.SelectIndex(index, namedCount);
            ListValidationHelper.Count(count, namedCount);
            ListValidationHelper.Range(index, count, namedCount);
        }

        public override void Set(NamedValue<int> index, NamedValue<TIn> item)
        {
            ThrowHelper.ValidateArgumentNotNull(item.Value is null, item.Name);
            ListValidationHelper.SelectIndex(index, ($"{nameof(Target)}.{nameof(Target.Count)}", Target.Count));
        }

        public override void Set(NamedValue<int> index, NamedValue<IEnumerable<TIn>> items)
        {
            var namedCount = ($"{nameof(Target)}.{nameof(Target.Count)}", Target.Count);
            ListValidationHelper.SelectIndex(index, namedCount);
            ThrowHelper.ValidateArgumentNotNull(items.Value is null, items.Name);
            ListValidationHelper.ItemsHasNotNull(items);
            ListValidationHelper.Range(
                index,
                ($"{nameof(items)}.{nameof(items.Value)}の要素数", items.Value.Count()),
                namedCount
            );
        }

        public override void Insert(NamedValue<int> index, NamedValue<TIn> item)
        {
            ThrowHelper.ValidateArgumentNotNull(item.Value is null, item.Name);
            ListValidationHelper.InsertIndex(index, ($"{nameof(Target)}.{nameof(Target.Count)}", Target.Count));
        }

        public override void Insert(NamedValue<int> index, NamedValue<IEnumerable<TIn>> items)
        {
            ThrowHelper.ValidateArgumentNotNull(items.Value is null, items.Name);
            ListValidationHelper.ItemsHasNotNull(items);
            ListValidationHelper.InsertIndex(index, ($"{nameof(Target)}.{nameof(Target.Count)}", Target.Count));
        }

        public override void Overwrite(NamedValue<int> index, NamedValue<IEnumerable<TIn>> items)
        {
            ThrowHelper.ValidateArgumentNotNull(items.Value is null, items.Name);
            ListValidationHelper.ItemsHasNotNull(items);
            ListValidationHelper.InsertIndex(index, ($"{nameof(Target)}.{nameof(Target.Count)}", Target.Count));
        }

        public override void Move(NamedValue<int> oldIndex, NamedValue<int> newIndex, NamedValue<int> count)
        {
            var namedCount = ($"{nameof(Target)}.{nameof(Target.Count)}", Target.Count);
            ListValidationHelper.ItemCountNotZero(namedCount);
            ListValidationHelper.SelectIndex(oldIndex, namedCount);
            ListValidationHelper.InsertIndex(newIndex, namedCount);
            ListValidationHelper.Count(count, namedCount);
            ListValidationHelper.Range(oldIndex, count, namedCount);
            ListValidationHelper.Range(count, newIndex, namedCount);
        }

        public override void Remove(NamedValue<int> index, NamedValue<int> count)
        {
            var namedCount = ($"{nameof(Target)}.{nameof(Target.Count)}", Target.Count);
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

        public override void Reset(NamedValue<IEnumerable<TIn>> items)
        {
            ThrowHelper.ValidateArgumentNotNull(items.Value is null, items.Name);
            ListValidationHelper.ItemsHasNotNull(items);
        }
    }
}
