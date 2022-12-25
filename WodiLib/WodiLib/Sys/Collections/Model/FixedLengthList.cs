// ========================================
// Project Name : WodiLib
// File Name    : FixedLengthList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     容量固定のList基底クラス
    /// </summary>
    /// <remarks>
    ///     機能概要は <seealso cref="IFixedLengthList{T}"/> 参照。
    /// </remarks>
    /// <typeparam name="T">リスト要素型</typeparam>
    /// <typeparam name="TImpl">リスト実装型</typeparam>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class FixedLengthList<T, TImpl> : ModelBase<TImpl>,
        IFixedLengthList<T>
        where T : notnull
        where TImpl : FixedLengthList<T, TImpl>
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

        /// <inheritdoc cref="IFixedLengthList{T}.this"/>
        public T this[int index]
        {
            get => Items[index];
            set => Items[index] = value;
        }

        /// <inheritdoc/>
        public int Count => Items.Count;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Protected Properties
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private protected virtual IExtendedList<T> Items { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Constructors
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        protected FixedLengthList(int count)
        {
            var validator = GenerateValidatorForItems();
            ThrowHelper.ValidateArgumentNotNull(validator is null, $"{nameof(GenerateValidatorForItems)}");

            var initItems = count.Iterate(MakeDefaultItem);
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
        protected FixedLengthList(IEnumerable<T> initItems)
        {
            ThrowHelper.ValidateArgumentNotNull(initItems is null, nameof(initItems));

            var validator = GenerateValidatorForItems();
            ThrowHelper.ValidateArgumentNotNull(validator is null, $"{nameof(GenerateValidatorForItems)}");

            Items = new ExtendedList<T>(MakeDefaultItem, validator, initItems);

            PropagatePropertyChangeEvent(Items);
        }

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="initItems">初期要素</param>
        /// <param name="capacity">要素数</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="initItems"/> が <see langword="null"/> の場合、
        ///     または <paramref name="initItems"/> 中に <see langword="null"/> が含まれる場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="initItems"/> の要素数が <see cref="capacity"/> と一致しない場合。
        /// </exception>
        protected FixedLengthList(IEnumerable<T> initItems, int capacity)
        {
            ThrowHelper.ValidateArgumentNotNull(initItems is null, nameof(initItems));

            var initItemArray = initItems.ToArray();
            ThrowHelper.ValidateArgumentItemsHasNotNull(initItemArray.HasNullItem(), nameof(initItems));
            ThrowHelper.ValidateArgumentNotMatch(
                initItemArray.Length != capacity,
                $"{nameof(initItems)}の要素数",
                capacity
            );

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
        internal FixedLengthList(IExtendedList<T> itemsImpl)
        {
            Items = itemsImpl;

            PropagatePropertyChangeEvent(Items);
        }

        /// <summary>
        ///     ディープコピーコンストラクタ
        /// </summary>
        /// <param name="src"></param>
        protected FixedLengthList(IFixedLengthList<T> src)
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
        /// </remarks>
        /// <returns>バリデーション実装</returns>
        protected abstract IWodiLibListValidator<T> GenerateValidatorForItems();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Public Methods
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc/>
        public IEnumerable<T> GetRange(int index, int count) => Items.GetRange(index, count);

        /// <inheritdoc/>
        public void SetRange(int index, IEnumerable<T> items) => Items.SetRange(index, items);

        /// <inheritdoc/>
        public void Move(int oldIndex, int newIndex) => Items.Move(oldIndex, newIndex);

        /// <inheritdoc/>
        public void MoveRange(int oldIndex, int newIndex, int count) => Items.MoveRange(oldIndex, newIndex, newIndex);

        /// <inheritdoc/>
        public void Reset(IEnumerable<T> initItems) => Items.Reset(initItems);

        /// <inheritdoc/>
        public void Reset() => Reset(Count.Iterate(MakeDefaultItem));

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
        public void ValidateMove(int oldIndex, int newIndex) => Items.ValidateMove(oldIndex, newIndex);

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void ValidateMoveRange(int oldIndex, int newIndex, int count)
            => Items.ValidateMoveRange(oldIndex, newIndex, newIndex);

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void ValidateReset(IEnumerable<T> items) => Items.ValidateReset(items);

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
        public void MoveCore(int oldIndex, int newIndex) => Items.MoveCore(oldIndex, newIndex);

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void MoveRangeCore(int oldIndex, int newIndex, int count)
            => Items.MoveRangeCore(oldIndex, newIndex, newIndex);

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void ResetCore(IEnumerable<T> items) => Items.ResetCore(items);

        /// <inheritdoc/>
        public IEnumerator<T> GetEnumerator() => Items.GetEnumerator();

        /// <inheritdoc/>
        public bool ItemEquals(IFixedLengthList<T>? other)
            => ItemEquals((IEnumerable<T>?)other);

        /// <inheritdoc/>
        public override bool ItemEquals(TImpl? other)
            => ItemEquals((IEnumerable<T>?)other);

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

        IFixedLengthList<T> IDeepCloneable<IFixedLengthList<T>>.DeepClone()
            => DeepClone();

        #endregion
    }
}
