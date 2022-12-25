// ========================================
// Project Name : WodiLib
// File Name    : ReadOnlyExtendedList.cs
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
    ///     読取専用のList基底クラス
    /// </summary>
    /// <remarks>
    ///     機能概要は <seealso cref="IReadOnlyExtendedList{T}"/> 参照。
    /// </remarks>
    /// <typeparam name="T">リスト要素型</typeparam>
    /// <typeparam name="TImpl">リスト実装型</typeparam>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class ReadOnlyExtendedList<T, TImpl> : ModelBase<TImpl>,
        IReadOnlyExtendedList<T>
        where T : notnull
        where TImpl : ReadOnlyExtendedList<T, TImpl>
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

        /// <inheritdoc cref="IReadOnlyExtendedList{T}.this"/>
        public T this[int index] => Items[index];

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
        /// <param name="initItems">初期要素</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="initItems"/> が <see langword="null"/> の場合、
        ///     または <paramref name="initItems"/> 中に <see langword="null"/> が含まれる場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="initItems"/> の要素数が <see cref="Count"/> と一致しない場合。
        /// </exception>
        protected ReadOnlyExtendedList(IEnumerable<T> initItems)
        {
            ThrowHelper.ValidateArgumentNotNull(initItems is null, nameof(initItems));

            var validator = GenerateValidatorForItems();
            ThrowHelper.ValidateArgumentNotNull(validator is null, $"{nameof(GenerateValidatorForItems)}");

            Items = new ExtendedList<T>(_ => default!, validator, initItems);

            PropagatePropertyChangeEvent(Items);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <remarks>
        /// 別のリストからリスト実装インスタンスの参照を保ったままインスタンスを作成したい場合に利用する。
        /// </remarks>
        /// <param name="itemsImpl">リスト実装インスタンス</param>
        internal ReadOnlyExtendedList(IExtendedList<T> itemsImpl)
        {
            Items = itemsImpl;

            PropagatePropertyChangeEvent(Items);
        }

        /// <summary>
        ///     ディープコピーコンストラクタ
        /// </summary>
        /// <param name="src"></param>
        protected ReadOnlyExtendedList(IReadOnlyExtendedList<T> src)
            : this((IEnumerable<T>)src)
        {
        }

        /// <summary>
        /// <see cref="Items"/>.<see cref="IExtendedList{T}.Validator"/> に設定するためのバリデーション実装を返す。
        /// </summary>
        /// <remarks>
        ///     このメソッドはコンストラクタから呼ばれる。
        ///     継承先のクラスで実装しない場合、CommonListValidator を返す。
        /// </remarks>
        /// <returns>バリデーション実装</returns>
        protected virtual IWodiLibListValidator<T> GenerateValidatorForItems()
        {
            return new CommonListValidator<T>(this);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Public Methods
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc/>
        public IEnumerator<T> GetEnumerator() => Items.GetEnumerator();

        /// <inheritdoc/>
        public IEnumerable<T> GetRange(int index, int count) => Items.GetRange(index, count);

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void ValidateGet(int index) => Items.ValidateGet(index);

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void ValidateGetRange(int index, int count) => Items.ValidateGetRange(index, count);

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public T GetCore(int index) => Items.GetCore(index);

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public IEnumerable<T> GetRangeCore(int index, int count) => Items.GetRangeCore(index, count);

        /// <inheritdoc/>
        public bool ItemEquals(IReadOnlyExtendedList<T>? other)
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

        IReadOnlyExtendedList<T> IDeepCloneable<IReadOnlyExtendedList<T>>.DeepClone()
            => DeepClone();

        #endregion
    }
}
