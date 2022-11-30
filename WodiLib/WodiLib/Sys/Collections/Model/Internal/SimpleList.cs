// ========================================
// Project Name : WodiLib
// File Name    : SimpleList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     WodiLib 内部で使用する基本リストクラス。
    ///     基本的なメソッドを定義したクラス。
    /// </summary>
    internal class SimpleList<T> : ObservableCollection<T>,
        ISimpleList<T>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Public Properties
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        public WodiLibContainerKeyName? ContainerKeyName { get; set; }

        public DelegateMakeListDefaultItem<T> MakeDefaultItem { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Constructors
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        internal SimpleList(DelegateMakeListDefaultItem<T> makeDefaultItem, IEnumerable<T>? initValues = null) : base(
            initValues ?? Array.Empty<T>()
        )
        {
            MakeDefaultItem = makeDefaultItem;
            initValues?.ForEach(PostInItem);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Public Methods
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc/>
        public IEnumerable<T> Get(int index, int count)
            => Items.Skip(index).Take(count);

        /// <inheritdoc/>
        public void Set(int index, params T[] items)
        {
            switch (items.Length)
            {
                case 0:
                    return;
                case 1:
                    SetItem(index, items[0]);
                    return;
            }

            CheckReentrancy();
            items.ForEach(
                (item, offset) => { Items[index + offset] = item; }
            );

            OnPropertyChanged(PropertyChangedEventArgsCache.GetInstance(ListConstant.IndexerName));
            OnCollectionReset();
        }

        /// <inheritdoc/>
        public void Add(params T[] items)
            => Insert(Count, items);

        /// <inheritdoc/>
        public void Insert(int index, params T[] items)
        {
            switch (items.Length)
            {
                case 0:
                    return;
                case 1:
                    InsertItem(index, items[0]);
                    return;
            }

            CheckReentrancy();
            items.ForEach(
                (item, offset) => { Items.Insert(index + offset, item); }
            );

            OnPropertyChanged(PropertyChangedEventArgsCache.GetInstance(nameof(Count)));
            OnPropertyChanged(PropertyChangedEventArgsCache.GetInstance(ListConstant.IndexerName));
            OnCollectionReset();
        }

        /// <inheritdoc/>
        public void Overwrite(int index, params T[] items)
        {
            switch (items.Length)
            {
                case 0:
                    return;
                case 1 when index < Count:
                    SetItem(index, items[0]);
                    return;
                case 1 when index == Count:
                    InsertItem(index, items[0]);
                    return;
            }

            var overwriteParam = OverwriteParam<T>.Factory.Create(Items, index, items);

            CheckReentrancy();

            overwriteParam.ReplaceNewItems.ForEach(
                (item, offset) => { Items[index + offset] = item; }
            );
            overwriteParam.InsertItems.ForEach(
                item => { Items.Add(item); }
            );

            if (overwriteParam.InsertItems.Length > 0)
            {
                OnPropertyChanged(PropertyChangedEventArgsCache.GetInstance(nameof(Count)));
            }

            OnPropertyChanged(PropertyChangedEventArgsCache.GetInstance(ListConstant.IndexerName));
            OnCollectionReset();
        }

        /// <inheritdoc/>
        public void Move(int oldIndex, int newIndex, int count)
        {
            switch (count)
            {
                case 0:
                    return;
                case 1:
                    MoveItem(oldIndex, newIndex);
                    return;
            }

            CheckReentrancy();

            var movedItems = Get(oldIndex, count).ToList();
            count.Range()
                .ForEach(
                    _ => { Items.RemoveAt(oldIndex); }
                );
            movedItems.ForEach(
                (moveItem, offset) => { Items.Insert(newIndex + offset, moveItem); }
            );

            OnPropertyChanged(PropertyChangedEventArgsCache.GetInstance(ListConstant.IndexerName));
            OnCollectionReset();
        }

        /// <inheritdoc/>
        public void Remove(int index, int count)
        {
            switch (count)
            {
                case 0:
                    return;
                case 1:
                    RemoveItem(index);
                    return;
            }

            count.Range()
                .ForEach(
                    _ => { Items.RemoveAt(index); }
                );


            OnPropertyChanged(PropertyChangedEventArgsCache.GetInstance(nameof(Count)));
            OnPropertyChanged(PropertyChangedEventArgsCache.GetInstance(ListConstant.IndexerName));
            OnCollectionReset();
        }

        /// <inheritdoc/>
        public void Adjust(int length)
        {
            if (Count == length) return;
            if (Count > length)
            {
                AdjustIfLong(length);
            }

            // Count < length
            AdjustIfShort(length);
        }

        /// <inheritdoc/>
        public void AdjustIfLong(int length)
        {
            if (Count <= length) return;
            Remove(length, Count - length);
        }

        /// <inheritdoc/>
        public void AdjustIfShort(int length)
        {
            if (Count >= length) return;

            var addItems = (length - Count).Range().Select(i => MakeDefaultItem(Count + i));
            Add(addItems.ToArray());
        }

        /// <inheritdoc/>
        public void Reset(params T[] items)
        {
            CheckReentrancy();

            var isCountChange = Count != items.Length;

            Items.Clear();
            items.ForEach(
                item => { Items.Add(item); }
            );

            if (isCountChange)
            {
                OnPropertyChanged(PropertyChangedEventArgsCache.GetInstance(nameof(Count)));
            }

            OnPropertyChanged(PropertyChangedEventArgsCache.GetInstance(ListConstant.IndexerName));
            OnCollectionReset();
        }

        /// <inheritdoc/>
        public bool ItemEquals(ISimpleList<T>? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return this.SequenceEqual(other);
        }

        /// <inheritdoc/>
        public bool ItemEquals(object? other)
        {
            if (other is SimpleList<T> castedSimpleList)
            {
                return ItemEquals(castedSimpleList);
            }

            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            if (other is IEnumerable<T> castedEnumerable)
            {
                return this.SequenceEqual(castedEnumerable);
            }

            return Equals(other);
        }

        /// <inheritdoc/>
        public ISimpleList<T> DeepClone()
        {
            var result = new SimpleList<T>(MakeDefaultItem, this);
            return result;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Interface Implements
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        object IDeepCloneable.DeepClone()
        {
            return DeepClone();
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Protected Methods
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        protected override void SetItem(int index, T item)
        {
            var oldItem = Items[index];
            PreOutItem(oldItem);
            
            base.SetItem(index, item);
            PostInItem(item);
        }

        protected override void InsertItem(int index, T item)
        {
            base.InsertItem(index, item);
            PostInItem(item);
        }

        protected override void RemoveItem(int index)
        {
            PreOutItem(Items[index]);
            base.RemoveItem(index);
        }

        protected override void ClearItems()
        {
            Items.ForEach(PreOutItem);
            base.ClearItems();
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Private Methods
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private void PostInItem(T item)
        {
            if (item is INotifyPropertyChanged notifyPropertyChanged)
            {
                notifyPropertyChanged.PropertyChanged += NotifyInnerItemPropertyChanged;
            }
        }

        private void PreOutItem(T item)
        {
            if (item is INotifyPropertyChanged notifyPropertyChanged)
            {
                notifyPropertyChanged.PropertyChanged -= NotifyInnerItemPropertyChanged;
            }
        }

        private void OnCollectionReset()
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        private void NotifyInnerItemPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            var notifyArgs = PropertyChangedEventArgsCache.GetInstance(ListConstant.IndexerName);
            OnPropertyChanged(notifyArgs);
        }
    }
}
