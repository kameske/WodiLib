// ========================================
// Project Name : WodiLib
// File Name    : ChoiceCaseList.CustomValidator.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using WodiLib.Sys;
using WodiLib.Sys.Collections;

namespace WodiLib.Event.EventCommand
{
    public partial class ChoiceCaseList
    {
        private class CustomValidator : WodiLibListValidatorTemplate<string>
        {
            protected override IWodiLibListValidator<string>? BaseValidator { get; }

            private new ChoiceCaseList Target { get; }

            public CustomValidator(ChoiceCaseList target) : base(target)
            {
                Target = target;
                BaseValidator = new FixedLengthListValidator<string>(target, Capacity);
            }

            public override void Get(NamedValue<int> index, NamedValue<int> count)
            {
                BaseValidator!.Get(index, count);
                ListValidationHelper.SelectIndex(index, (nameof(Target.CaseValue), Target.CaseValue));
            }

            public override void Set(NamedValue<int> index, NamedValue<IEnumerable<string>> items)
            {
                BaseValidator!.Set(index, items);
                ListValidationHelper.SelectIndex(index, (nameof(Target.CaseValue), Target.CaseValue));
            }

            public override void Insert(NamedValue<int> index, NamedValue<IEnumerable<string>> items)
                => throw new NotSupportedException();

            public override void Overwrite(NamedValue<int> index, NamedValue<IEnumerable<string>> items)
                => throw new NotSupportedException();

            public override void Remove(NamedValue<int> index, NamedValue<int> count)
                => throw new NotSupportedException();

            public override void AdjustLength(NamedValue<int> length)
                => throw new NotSupportedException();
        }
    }
}
