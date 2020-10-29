// ========================================
// Project Name : WodiLib
// File Name    : ChoiceCaseList.CustomValidator.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Diagnostics.CodeAnalysis;
using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    public partial class ChoiceCaseList
    {
        private class CustomValidator : IExtendedListValidator<string>
        {
            private ChoiceCaseList Target { get; }

            private FixedLengthListValidator<string> PreConditionValidator { get; }

            public CustomValidator(ChoiceCaseList target)
            {
                Target = target;
                PreConditionValidator = new FixedLengthListValidator<string>(target);
            }

            public void Constructor(params string[] initItems)
            {
                PreConditionValidator.Constructor(initItems);
            }

            public void Get(int index, int count)
            {
                PreConditionValidator.Get(index, count);
                ListValidationHelper.SelectIndex(index, Target.CaseValue);
            }

            public void Set(int index, params string[] items)
            {
                PreConditionValidator.Set(index, items);
                ListValidationHelper.SelectIndex(index, Target.CaseValue);
            }

            public void Move(int oldIndex, int newIndex, int count)
            {
                PreConditionValidator.Move(oldIndex, newIndex, count);
            }

            public void Reset(params string[] items)
            {
                PreConditionValidator.Reset(items);
            }

            public void Insert(int index, params string[] items)
                => throw new NotSupportedException();

            public void Overwrite(int index, params string[] items)
                => throw new NotSupportedException();

            public void Remove([AllowNull] string item)
                => throw new NotSupportedException();

            public void Remove(int index, int count)
                => throw new NotSupportedException();

            public void AdjustLength(int length)
                => throw new NotSupportedException();

            public void AdjustLengthIfShort(int length)
                => throw new NotSupportedException();

            public void AdjustLengthIfLong(int length)
                => throw new NotSupportedException();
        }
    }
}
