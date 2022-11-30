// ========================================
// Project Name : WodiLib
// File Name    : ExtendedList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     WodiLib 独自リスト
    /// </summary>
    /// <remarks>
    ///     WodiLib内で使用する各種リストの処理転送先となるクラス。
    ///     <typeparamref name="T"/> が変更通知を行うクラスだった場合、
    ///     通知を受け取ると自身の "Items[]" プロパティ変更通知を行う。
    /// </remarks>
    /// <typeparam name="T">リスト内包クラス</typeparam>
    internal class ExtendedList<T> : ModelBase<ExtendedList<T>>,
        IExtendedList<T>
    {
        /*
         * WodiLib 内部で使用する独自汎用リスト。
         * リスト本体の機能は SimpleList<T> に委譲。
         */
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Events
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Public Properties
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        public T this[int index]
        {
            get
            {
                this.ValidateGet(index);
                return this.GetCore(index);
            }
            set
            {
                this.ValidateSet(index, value);
                this.SetCore(index, value);
            }
        }

        public int Count => Items.Count;

        public Func<int, T> MakeDefaultItem { get; }

        public IWodiLibListValidator<T>? Validator { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Private Properties
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>リスト本体</summary>
        protected virtual ISimpleList<T> Items { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Constructors
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="makeListDefaultItem">デフォルト要素生成処理</param>
        /// <param name="validator">検証処理実装</param>
        /// <param name="initItems">初期要素</param>
        public ExtendedList(
            DelegateMakeListDefaultItem<T> makeListDefaultItem,
            IWodiLibListValidator<T>? validator,
            IEnumerable<T>? initItems = null
        )
        {
            ThrowHelper.ValidateNotNull(makeListDefaultItem is null);

            var initItemArray = initItems?.ToArray() ?? Array.Empty<T>();
            Validator = validator;

            Validator?.Constructor((nameof(initItems), initItemArray));

            Items = new SimpleList<T>(makeListDefaultItem, initItemArray);
            MakeDefaultItem = i => Items.MakeDefaultItem(i);

            PropagatePropertyChangeEvent(Items);
            PropagateCollectionChangeEvent();
        }

        private void PropagateCollectionChangeEvent()
        {
            Items.CollectionChanged += (_, args) => { CollectionChanged?.Invoke(this, args); };
        }

        /// <summary>
        ///     ディープコピーコンストラクタ
        /// </summary>
        /// <param name="src">コピー元</param>
        private ExtendedList(IExtendedList<T> src) : this(i => src.MakeDefaultItem(i), src.Validator, src)
        {
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Public Methods
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        public IEnumerator<T> GetEnumerator() => Items.GetEnumerator();

        public IEnumerable<T> GetRangeCore(int index, int count) => Items.Get(index, count);

        public void SetRangeCore(int index, IEnumerable<T> items) => Items.Set(index, items.ToArray());
        public void InsertRangeCore(int index, IEnumerable<T> items) => Items.Insert(index, items.ToArray());
        public void OverwriteCore(int index, IEnumerable<T> items) => Items.Overwrite(index, items.ToArray());
        public void MoveRangeCore(int oldIndex, int newIndex, int count) => Items.Move(oldIndex, newIndex, count);
        public void RemoveRangeCore(int index, int count) => Items.Remove(index, count);
        public void AdjustLengthCore(int length) => Items.Adjust(length);
        public void ResetCore(IEnumerable<T> items) => Items.Reset(items.ToArray());
        public void ClearCore() => Items.Clear();

        public bool ItemEquals(IExtendedList<T>? other)
            => ItemEquals((IEnumerable<T>?)other);

        public override bool ItemEquals(ExtendedList<T>? other)
            => ItemEquals((IEnumerable<T>?)other);

        public bool ItemEquals(IEnumerable<T>? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            var otherItemArray = other.ToArray();
            return Count == otherItemArray.Length
                   && this.Zip(otherItemArray)
                       .All(
                           zip => zip.Item1 is IEqualityComparable equalityComparable
                               ? equalityComparable.ItemEquals(zip.Item2)
                               : zip.Item1!.Equals(zip.Item2)
                       );
        }

        public override bool ItemEquals(object? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            if (other is IEnumerable<T> enumerable)
            {
                return ItemEquals(enumerable);
            }

            return Equals(other);
        }

        public override ExtendedList<T> DeepClone() => new(this);

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Interface Implementation
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region GetEnumerator

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        #endregion

        #region DeepClone

        IExtendedList<T> IDeepCloneable<IExtendedList<T>>.DeepClone()
            => DeepClone();

        #endregion
    }
}
