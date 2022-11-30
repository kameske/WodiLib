// ========================================
// Project Name : WodiLib.Test
// File Name    : MockWodiLibListValidator.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using WodiLib.Sys;
using WodiLib.Sys.Collections;

namespace WodiLib.Test.Tools
{
    /// <summary>
    ///     <see cref="IWodiLibListValidator{T}"/> モック用
    /// </summary>
    public class MockWodiLibListValidator<T> : MockBase<IWodiLibListValidator<T>>,
        IWodiLibListValidator<T>
    {
        public void Constructor(NamedValue<IEnumerable<T>> initItems)
        {
            AddCalledHistory(nameof(Constructor));
        }

        public void Get(NamedValue<int> index, NamedValue<int> count)
        {
            AddCalledHistory(nameof(Get));
        }

        public void Set(NamedValue<int> index, NamedValue<IEnumerable<T>> items)
        {
            AddCalledHistory(nameof(Set));
        }

        public void Insert(NamedValue<int> index, NamedValue<IEnumerable<T>> items)
        {
            AddCalledHistory(nameof(Insert));
        }

        public void Overwrite(NamedValue<int> index, NamedValue<IEnumerable<T>> items)
        {
            AddCalledHistory(nameof(Overwrite));
        }

        public void Move(NamedValue<int> oldIndex, NamedValue<int> newIndex, NamedValue<int> count)
        {
            AddCalledHistory(nameof(Move));
        }

        public void Remove(NamedValue<int> index, NamedValue<int> count)
        {
            AddCalledHistory(nameof(Remove));
        }

        public void AdjustLength(NamedValue<int> length)
        {
            AddCalledHistory(nameof(AdjustLength));
        }

        public void Reset(NamedValue<IEnumerable<T>> items)
        {
            AddCalledHistory(nameof(Reset));
        }

        public void Clear()
        {
            AddCalledHistory(nameof(Clear));
        }
    }
}
