// ========================================
// Project Name : WodiLib
// File Name    : ChoiceCaseList.CustomValidator.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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

            public override void Get(int index, int count)
            {
                BaseValidator!.Get(index, count);
                ListValidationHelper.SelectIndex(index, Target.CaseValue);
            }

            public override void Set(int index, string item)
            {
                BaseValidator!.Set(index, item);
                ListValidationHelper.SelectIndex(index, Target.CaseValue);
            }

            public override void Set(int index, IReadOnlyList<string> items)
            {
                BaseValidator!.Set(index, items);
                ListValidationHelper.SelectIndex(index, Target.CaseValue);
            }

            public override void Insert(int index, string items)
                => throw new NotSupportedException();

            public override void Insert(int index, IReadOnlyList<string> items)
                => throw new NotSupportedException();

            public override void Overwrite(int index, IReadOnlyList<string> items)
                => throw new NotSupportedException();

            public override void Remove([AllowNull] string item)
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
}
