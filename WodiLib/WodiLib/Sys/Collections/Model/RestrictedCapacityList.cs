// ========================================
// Project Name : WodiLib
// File Name    : RestrictedCapacityList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     容量制限のあるListクラス
    /// </summary>
    /// <remarks>
    ///     機能概要は <seealso cref="IRestrictedCapacityList{T}"/> 参照。
    /// </remarks>
    /// <typeparam name="T">リスト要素入力型</typeparam>
    /// <typeparam name="TImpl">リスト実装型</typeparam>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class RestrictedCapacityList<T, TImpl> : ModelBase<TImpl>,
        IRestrictedCapacityList<T>
        where T : notnull
        where TImpl : RestrictedCapacityList<T, TImpl>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Events
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc/>
        public event NotifyCollectionChangedEventHandler? CollectionChanged
        {
            add => Items.CollectionChanged += value;
            remove => Items.CollectionChanged -= value;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Public Properties
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc cref="IRestrictedCapacityList{T}.this"/>
        public T this[int index]
        {
            get => Items[index];
            set => Items[index] = value;
        }

        /// <inheritdoc/>
        public int Count => Items.Count;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Private Properties
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private protected virtual IExtendedList<T> Items { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Constructors
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <remarks>
        ///     デフォルトの要素数は <see cref="GetMinCapacity"/> となる。
        /// </remarks>
        protected RestrictedCapacityList()
        {
            var validator = GenerateValidatorForItems();
            ThrowHelper.ValidateArgumentNotNull(validator is null, $"{nameof(GenerateValidatorForItems)}");

            var initItems = GetMinCapacity().Iterate(MakeDefaultItem);
            Items = new ExtendedList<T>(MakeDefaultItem, validator, initItems);

            PropagatePropertyChangeEvent(Items);
        }

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="length">[Range(<see cref="GetMinCapacity"/>, <see cref="GetMaxCapacity"/>)] 要素数</param>
        /// <exception cref="ArgumentException">
        ///     <paramref name="length"/> が <see cref="GetMinCapacity"/> 未満
        ///     または <see cref="GetMaxCapacity"/> を超える場合。
        /// </exception>
        protected RestrictedCapacityList(int length)
        {
            var validator = GenerateValidatorForItems();
            ThrowHelper.ValidateArgumentNotNull(validator is null, $"{nameof(GenerateValidatorForItems)}");

            var initItems = length.Iterate(MakeDefaultItem);
            Items = new ExtendedList<T>(MakeDefaultItem, validator, initItems);

            PropagatePropertyChangeEvent(Items);
        }

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="initItems">初期要素</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="initItems"/> が <see langword="null"/> の場合、
        ///     または <paramref name="initItems"/> 中に <see langword="null"/> が含まれる場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="initItems"/> の要素数が <see cref="GetMinCapacity"/> 未満
        ///     または <see cref="GetMaxCapacity"/> を超える場合。
        /// </exception>
        protected RestrictedCapacityList(IEnumerable<T> initItems)
        {
            ThrowHelper.ValidateArgumentNotNull(initItems is null, nameof(initItems));

            var validator = GenerateValidatorForItems();
            ThrowHelper.ValidateArgumentNotNull(validator is null, $"{nameof(GenerateValidatorForItems)}");

            Items = new ExtendedList<T>(MakeDefaultItem, validator, initItems);

            PropagatePropertyChangeEvent(Items);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <remarks>
        /// 別のリストからリスト実装インスタンスの参照を保ったままインスタンスを作成したい場合に利用する。
        /// </remarks>
        /// <param name="itemsImpl">リスト実装インスタンス</param>
        internal RestrictedCapacityList(IExtendedList<T> itemsImpl)
        {
            Items = itemsImpl;

            PropagatePropertyChangeEvent(Items);
        }

        /// <summary>
        ///     ディープコピーコンストラクタ
        /// </summary>
        /// <param name="src"></param>
        protected RestrictedCapacityList(IRestrictedCapacityList<T> src)
            : this((IEnumerable<T>)src)
        {
        }

        /// <summary>
        /// 格納対象のデフォルトインスタンスを生成する。
        /// </summary>
        /// <param name="index">挿入インデックス</param>
        /// <returns>デフォルトインスタンス</returns>
        protected abstract T MakeDefaultItem(int index);

        /// <summary>
        /// <see cref="Items"/>.<see cref="IExtendedList{T}.Validator"/> に設定するためのバリデーション実装を返す。
        /// </summary>
        /// <remarks>
        ///     このメソッドはコンストラクタから呼ばれる。
        ///     継承先のクラスで実装しない場合、RestrictedCapacityListValidator を返す。
        /// </remarks>
        /// <returns>バリデーション実装</returns>
        protected virtual IWodiLibListValidator<T> GenerateValidatorForItems()
        {
            return new RestrictedCapacityListValidator<T>(this, GetMinCapacity(), GetMaxCapacity());
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Public Methods
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc/>
        public abstract int GetMaxCapacity();

        /// <inheritdoc/>
        public abstract int GetMinCapacity();

        /// <inheritdoc/>
        public IEnumerable<T> GetRange(int index, int count) => Items.GetRange(index, count);

        /// <inheritdoc/>
        public void SetRange(int index, IEnumerable<T> items) => Items.SetRange(index, items);

        /// <inheritdoc/>
        public void Add(T item) => Items.Add(item);

        /// <inheritdoc/>
        public void AddRange(IEnumerable<T> items) => Items.AddRange(items);

        /// <inheritdoc/>
        public void Insert(int index, T item) => Items.Insert(index, item);

        /// <inheritdoc/>
        public void InsertRange(int index, IEnumerable<T> items) => Items.InsertRange(index, items);

        /// <inheritdoc/>
        public void Overwrite(int index, IEnumerable<T> items) => Items.Overwrite(index, items);

        /// <inheritdoc/>
        public void Move(int oldIndex, int newIndex) => Items.Move(oldIndex, newIndex);

        /// <inheritdoc/>
        public void MoveRange(int oldIndex, int newIndex, int count) => Items.MoveRange(oldIndex, newIndex, newIndex);

        /// <inheritdoc/>
        public void Remove(int index) => Items.Remove(index);

        /// <inheritdoc/>
        public void RemoveRange(int index, int count) => Items.RemoveRange(index, count);

        /// <inheritdoc/>
        public void AdjustLength(int length) => Items.AdjustLength(length);

        /// <inheritdoc/>
        public void AdjustLengthIfShort(int length) => Items.AdjustLengthIfShort(length);

        /// <inheritdoc/>
        public void AdjustLengthIfLong(int length) => Items.AdjustLengthIfLong(length);

        /// <inheritdoc/>
        public void Reset(IEnumerable<T> initItems) => Items.Reset(initItems);

        /// <inheritdoc/>
        public void Clear() => Items.Clear();

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void ValidateGet(int index) => Items.ValidateGet(index);

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void ValidateGetRange(int index, int count) => Items.ValidateGetRange(index, count);

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void ValidateSet(int index, T item) => Items.ValidateSet(index, item);

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void ValidateSetRange(int index, IEnumerable<T> items) => Items.ValidateSetRange(index, items);

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void ValidateAdd(T item) => Items.ValidateAdd(item);

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void ValidateAddRange(IEnumerable<T> items) => Items.ValidateAddRange(items);

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void ValidateInsert(int index, T item) => Items.ValidateInsert(index, item);

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void ValidateInsertRange(int index, IEnumerable<T> items) => Items.ValidateInsertRange(index, items);

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void ValidateOverwrite(int index, IEnumerable<T> items) => Items.ValidateOverwrite(index, items);

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void ValidateMove(int oldIndex, int newIndex) => Items.ValidateMove(oldIndex, newIndex);

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void ValidateMoveRange(int oldIndex, int newIndex, int count)
            => Items.ValidateMoveRange(oldIndex, newIndex, newIndex);

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void ValidateRemove(int index) => Items.ValidateRemove(index);

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void ValidateRemoveRange(int index, int count) => Items.ValidateRemoveRange(index, count);

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void ValidateAdjustLength(int length) => Items.ValidateAdjustLength(length);

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void ValidateAdjustLengthIfShort(int length) => Items.ValidateAdjustLength(length);

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void ValidateAdjustLengthIfLong(int length) => Items.ValidateAdjustLength(length);

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void ValidateReset(IEnumerable<T> items) => Items.ValidateReset(items);

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void ValidateClear() => Items.ValidateClear();

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public T GetCore(int index) => Items.GetCore(index);

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public IEnumerable<T> GetRangeCore(int index, int count) => Items.GetRangeCore(index, count);

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void SetCore(int index, T item) => Items.SetCore(index, item);

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void SetRangeCore(int index, IEnumerable<T> items) => Items.SetRangeCore(index, items);

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void AddCore(T item) => Items.AddCore(item);

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void AddRangeCore(IEnumerable<T> items) => Items.AddRangeCore(items);

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void InsertCore(int index, T item) => Items.InsertCore(index, item);

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void InsertRangeCore(int index, IEnumerable<T> items) => Items.InsertRangeCore(index, items);

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void OverwriteCore(int index, IEnumerable<T> items) => Items.OverwriteCore(index, items);

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void MoveCore(int oldIndex, int newIndex) => Items.MoveCore(oldIndex, newIndex);

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void MoveRangeCore(int oldIndex, int newIndex, int count)
            => Items.MoveRangeCore(oldIndex, newIndex, newIndex);

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void RemoveCore(int index) => Items.RemoveCore(index);

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void RemoveRangeCore(int index, int count) => Items.RemoveRangeCore(index, count);

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void AdjustLengthCore(int length) => Items.AdjustLengthCore(length);

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void AdjustLengthIfShortCore(int length) => Items.AdjustLengthIfShortCore(length);

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void AdjustLengthIfLongCore(int length) => Items.AdjustLengthIfLongCore(length);

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void ResetCore(IEnumerable<T> items) => Items.ResetCore(items);

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void ClearCore() => Items.ClearCore();

        /// <inheritdoc/>
        public IEnumerator<T> GetEnumerator() => Items.GetEnumerator();

        /// <inheritdoc/>
        public bool ItemEquals(IRestrictedCapacityList<T>? other)
            => ItemEquals((IEnumerable<T>?)other);

        /// <inheritdoc/>
        public override bool ItemEquals(TImpl? other)
            => ItemEquals(other);

        /// <inheritdoc cref="IEqualityComparable{T}.ItemEquals(T?)"/>
        public bool ItemEquals(IEnumerable<T>? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Items.ItemEquals(other);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Interface Implementation
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region GetEnumerator

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion

        #region GetEnumerator

        IRestrictedCapacityList<T> IDeepCloneable<IRestrictedCapacityList<T>>.DeepClone()
            => DeepClone();

        #endregion
    }
}
