// ========================================
// Project Name : WodiLib.Test
// File Name    : MockBase.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using WodiLib.Sys;

namespace WodiLib.Test.Tools
{
    /// <summary>
    ///     モック用基底クラス
    /// </summary>
    public abstract class MockBase<TChild> : IDeepCloneable
        where TChild : class
    {
        public IReadOnlyList<string> CalledMemberHistory => calledMemberHistory;

        public WodiLibContainerKeyName? ContainerKeyName { get; set; }

        private readonly List<string> calledMemberHistory = new();

        protected void AddCalledHistory(string methodName)
            => calledMemberHistory.Add(methodName);

        public void ClearCalledHistory()
            => calledMemberHistory.Clear();

        public event PropertyChangedEventHandler? PropertyChanged
        {
            // ReSharper disable once ValueParameterNotUsed
            add => AddCalledHistory(nameof(PropertyChanged));
            // ReSharper disable once ValueParameterNotUsed
            remove => AddCalledHistory(nameof(PropertyChanged));
        }

        public event NotifyCollectionChangedEventHandler? CollectionChanged
        {
            // ReSharper disable once ValueParameterNotUsed
            add => AddCalledHistory(nameof(CollectionChanged));
            // ReSharper disable once ValueParameterNotUsed
            remove => AddCalledHistory(nameof(CollectionChanged));
        }

        public bool ItemEquals(TChild? other)
        {
            AddCalledHistory(nameof(ItemEquals));
            return false;
        }

        public bool ItemEquals(object? other)
        {
            AddCalledHistory(nameof(ItemEquals));
            return false;
        }

        public virtual TChild DeepClone()
        {
            AddCalledHistory(nameof(ItemEquals));
            return (this as TChild)!;
        }

        object IDeepCloneable.DeepClone() => DeepClone();
    }
}
