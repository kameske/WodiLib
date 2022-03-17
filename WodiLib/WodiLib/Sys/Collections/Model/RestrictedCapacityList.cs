// ========================================
// Project Name : WodiLib
// File Name    : RestrictedCapacityList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     容量制限のあるListクラス
    /// </summary>
    /// <remarks>
    ///     機能概要は <seealso cref="IRestrictedCapacityList{TIn, TOut}"/> 参照。
    /// </remarks>
    /// <typeparam name="TIn">リスト要素入力型</typeparam>
    /// <typeparam name="TOut">リスト要素出力型</typeparam>
    /// <typeparam name="TInternal">リスト内包型</typeparam>
    /// <typeparam name="TImpl">リスト実装型</typeparam>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class RestrictedCapacityList<TIn, TOut, TInternal, TImpl> :
        RestrictedCapacityListBase<TInternal, TImpl>,
        IRestrictedCapacityList<TIn, TOut>
        where TImpl : RestrictedCapacityList<TIn, TOut, TInternal, TImpl>
        where TOut : TIn
        where TInternal : TOut
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Events
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc/>
        public new virtual event EventHandler<NotifyCollectionChangedEventArgsEx<TOut>>? CollectionChanging
        {
            add
            {
                if (value is not null)
                {
                    CollectionChangingEventHandlers.Add(value);
                }
            }
            remove
            {
                if (value is not null)
                {
                    CollectionChangingEventHandlers.Remove(value);
                }
            }
        }

        /// <inheritdoc/>
        public new virtual event EventHandler<NotifyCollectionChangedEventArgsEx<TOut>>? CollectionChanged
        {
            add
            {
                if (value is not null)
                {
                    CollectionChangedEventHandlers.Add(value);
                }
            }
            remove
            {
                if (value is not null)
                {
                    CollectionChangedEventHandlers.Remove(value);
                }
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Properties
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private List<EventHandler<NotifyCollectionChangedEventArgsEx<TOut>>>
            CollectionChangingEventHandlers { get; } = new();

        private List<EventHandler<NotifyCollectionChangedEventArgsEx<TOut>>>
            CollectionChangedEventHandlers { get; } = new();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Constructors
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        protected RestrictedCapacityList()
        {
            PropagateCollectionChangeChangeEvent();
        }

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="length">要素数</param>
        protected RestrictedCapacityList(int length) : base(length)
        {
            PropagateCollectionChangeChangeEvent();
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
        ///     <paramref name="initItems"/> の要素数が <see cref="RestrictedCapacityListBase{T,TImpl}.GetMinCapacity"/> 未満
        ///     または <see cref="RestrictedCapacityListBase{T,TImpl}.GetMaxCapacity"/> を超える場合。
        /// </exception>
        // ReSharper disable PossibleMultipleEnumeration
        protected RestrictedCapacityList(IEnumerable<TIn> initItems) : base(initItems.Count())
        {
            ThrowHelper.ValidateArgumentNotNull(initItems is null, nameof(initItems));

            Overwrite(0, CastInternal(initItems));
            PropagateCollectionChangeChangeEvent();
        }
        // ReSharper restore PossibleMultipleEnumeration

        /// <summary>
        ///     ディープコピーコンストラクタ
        /// </summary>
        /// <param name="src"></param>
        protected RestrictedCapacityList(IReadOnlyExtendedList<TInternal, TInternal> src)
            : this((IEnumerable<TIn>)src)
        {
            NotifyPropertyChangingEventType = src.NotifyPropertyChangingEventType;
            NotifyPropertyChangedEventType = src.NotifyPropertyChangedEventType;
            NotifyCollectionChangingEventType = src.NotifyCollectionChangingEventType;
            NotifyCollectionChangedEventType = src.NotifyCollectionChangedEventType;
        }

        /// <summary>
        ///     本来のCollectionChangeイベントを自身のCollectionChangeイベントに伝播させる。
        /// </summary>
        private void PropagateCollectionChangeChangeEvent()
        {
            base.CollectionChanging += (_, args) =>
            {
                CollectionChangingEventHandlers.ForEach(
                    handler =>
                    {
                        var myArgs = NotifyCollectionChangedEventArgsEx<TOut>.CreateFromOtherType(args, item => item);
                        handler.Invoke(this, myArgs);
                    }
                );
            };

            base.CollectionChanged += (_, args) =>
            {
                CollectionChangedEventHandlers.ForEach(
                    handler =>
                    {
                        var myArgs = NotifyCollectionChangedEventArgsEx<TOut>.CreateFromOtherType(args, item => item);
                        handler.Invoke(this, myArgs);
                    }
                );
            };
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Public Methods
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc/>
        public void SetRange(int index, IEnumerable<TIn> items)
        {
            ThrowHelper.ValidateArgumentNotNull(items is null, nameof(items));

            SetRange(index, CastInternal(items));
        }

        /// <inheritdoc/>
        public void Add(TIn item)
        {
            ThrowHelper.ValidateArgumentNotNull(item is null, nameof(item));

            base.Add(CastInternal(item));
        }

        /// <inheritdoc/>
        public void AddRange(IEnumerable<TIn> items)
        {
            ThrowHelper.ValidateArgumentNotNull(items is null, nameof(items));

            AddRange(CastInternal(items));
        }

        /// <inheritdoc/>
        public void Insert(int index, TIn item)
        {
            ThrowHelper.ValidateArgumentNotNull(item is null, nameof(item));

            base.Insert(index, CastInternal(item));
        }

        /// <inheritdoc/>
        public void InsertRange(int index, IEnumerable<TIn> items)
        {
            ThrowHelper.ValidateArgumentNotNull(items is null, nameof(items));

            InsertRange(index, CastInternal(items));
        }

        /// <inheritdoc/>
        public void Overwrite(int index, IEnumerable<TIn> items)
        {
            ThrowHelper.ValidateArgumentNotNull(items is null, nameof(items));

            Overwrite(index, CastInternal(items));
        }

        /// <inheritdoc/>
        public bool Remove(TIn? item)
        {
            if (item is null)
            {
                return false;
            }

            return base.Remove(CastInternal(item));
        }

        /// <inheritdoc cref="ISizeChangeableList{TIn,TOut}.Reset"/>
        public void Reset(IEnumerable<TIn> initItems)
        {
            ThrowHelper.ValidateArgumentNotNull(initItems is null, nameof(initItems));

            Reset(CastInternal(initItems));
        }

        void IWritableList<TIn, TOut>.Reset(IEnumerable<TIn> initItems)
        {
            ThrowHelper.ValidateArgumentNotNull(initItems is null, nameof(initItems));

            ResetAsWritableList(CastInternal(initItems));
        }

        /// <inheritdoc/>
        public bool ItemEquals(IReadOnlyExtendedList<TIn, TOut>? other)
            => ItemEquals(other, null);

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Interface Implementation
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Indexer

        TIn IWritableList<TIn, TOut>.this[int index]
        {
            set
            {
                ThrowHelper.ValidateArgumentNotNull(value is null, nameof(value));

                this[index] = CastInternal(value);
            }
        }

        TIn IFixedLengthList<TIn, TOut>.this[int index]
        {
            get => this[index];
            set
            {
                ThrowHelper.ValidateArgumentNotNull(value is null, nameof(value));

                this[index] = CastInternal(value);
            }
        }

        TOut IReadableList<TOut>.this[int index] => this[index];

        #endregion

        #region GetEnumerator

        IEnumerator<TOut> IEnumerable<TOut>.GetEnumerator() =>
            GetRange(0, Count).Select(item => (TOut)item).GetEnumerator();

        #endregion

        #region GetRange

        IEnumerable<TOut> IReadableList<TOut>.GetRange(int index, int count) => GetRange(index, count).Cast<TOut>();

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Protected Methods
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     <typeparamref name="TIn"/> を <typeparamref name="TInternal"/> に変換したディープクローンを生成する。
        /// </summary>
        /// <param name="src">クローン元</param>
        /// <returns>クローンインスタンス</returns>
        protected abstract TInternal CloneToInternal(TIn src);

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Private Methods
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private TInternal CastInternal(TIn src)
            => CloneToInternal(src);

        private IEnumerable<TInternal> CastInternal(IEnumerable<TIn> src)
            => src.Select(CloneToInternal);
    }

    /// <summary>
    ///     容量制限のあるListクラス
    /// </summary>
    /// <remarks>
    ///     機能概要は <seealso cref="IRestrictedCapacityList{TIn, TOut}"/> 参照。
    /// </remarks>
    /// <typeparam name="T">リスト要素入力型</typeparam>
    /// <typeparam name="TImpl">リスト実装型</typeparam>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class RestrictedCapacityList<T, TImpl> : RestrictedCapacityList<T, T, T, TImpl>
        where TImpl : RestrictedCapacityList<T, TImpl>
    {
        /// <summary>
        ///     コンストラクタ
        /// </summary>
        protected RestrictedCapacityList()
        {
        }

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="length">要素数</param>
        protected RestrictedCapacityList(int length) : base(length)
        {
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
        ///     <paramref name="initItems"/> の要素数が <see cref="RestrictedCapacityListBase{T,TImpl}.GetMinCapacity"/> 未満
        ///     または <see cref="RestrictedCapacityListBase{T,TImpl}.GetMaxCapacity"/> を超える場合。
        /// </exception>
        protected RestrictedCapacityList(IEnumerable<T> initItems) : base(
            ((Func<IEnumerable<T>>)(() =>
            {
                ThrowHelper.ValidateArgumentNotNull(initItems is null, nameof(initItems));
                var initItemArray = initItems.ToArray();
                ThrowHelper.ValidateArgumentItemsHasNotNull(initItemArray.HasNullItem(), nameof(initItems));

                return initItemArray;
            }))()
        )
        {
            SetRange(0, initItems);
        }

        /// <inheritdoc/>
        protected override T CloneToInternal(T src) => src;
    }
}
