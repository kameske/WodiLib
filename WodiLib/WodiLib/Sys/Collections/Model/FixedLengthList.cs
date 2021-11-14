// ========================================
// Project Name : WodiLib
// File Name    : FixedLengthList.cs
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
    ///     容量固定のList基底クラス
    /// </summary>
    /// <remarks>
    ///     機能概要は <seealso cref="IFixedLengthList{T, T}"/> 参照。
    /// </remarks>
    /// <typeparam name="TIn">リスト要素入力型</typeparam>
    /// <typeparam name="TOut">リスト要素出力型</typeparam>
    /// <typeparam name="TInternal">リスト内包型</typeparam>
    /// <typeparam name="TImpl">リスト実装型</typeparam>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class FixedLengthList<TIn, TOut, TInternal, TImpl> : FixedLengthListBase<TInternal, TImpl>,
        IFixedLengthList<TIn, TOut>
        where TImpl : FixedLengthList<TIn, TOut, TInternal, TImpl>
        where TOut : TIn
        where TInternal : TOut
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Events
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc/>
        public new event EventHandler<NotifyCollectionChangedEventArgsEx<TOut>>? CollectionChanging
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
        public new event EventHandler<NotifyCollectionChangedEventArgsEx<TOut>>? CollectionChanged
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
        /// <param name="length">要素数</param>
        protected FixedLengthList(int length) : base(length)
        {
            PropagateCollectionChangeChangeEvent();
        }

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="initItems">初期リスト</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="initItems"/> が <see langword="null"/> の場合、
        ///     または <paramref name="initItems"/> 中に <see langword="null"/> が含まれる場合。
        /// </exception>
        protected FixedLengthList(IEnumerable<TInternal> initItems) : base(initItems)
        {
            PropagateCollectionChangeChangeEvent();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="initParam">初期化パラメータ</param>
        protected FixedLengthList(ListInitParam<TIn> initParam) : this(initParam.InitItems)
        {
        }

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="initItems">初期リスト</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="initItems"/> が <see langword="null"/> の場合、
        ///     または <paramref name="initItems"/> 中に <see langword="null"/> が含まれる場合。
        /// </exception>
        protected FixedLengthList(IEnumerable<TIn> initItems) : base(((Func<int>)(() =>
        {
            ThrowHelper.ValidateArgumentNotNull(initItems is null, nameof(initItems));
            var initItemArray = initItems.ToArray();
            ThrowHelper.ValidateArgumentItemsHasNotNull(initItemArray.HasNullItem(), nameof(initItems));

            return initItemArray.Length;
        }))())
        {
            SetRange(0, CastInternal(initItems));
            PropagateCollectionChangeChangeEvent();
        }

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="initItems">初期リスト</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="initItems"/> が <see langword="null"/> の場合、
        ///     または <paramref name="initItems"/> 中に <see langword="null"/> が含まれる場合。
        /// </exception>
        protected FixedLengthList(IEnumerable<TOut> initItems) : base(((Func<int>)(() =>
        {
            ThrowHelper.ValidateArgumentNotNull(initItems is null, nameof(initItems));
            var initItemArray = initItems.ToArray();
            ThrowHelper.ValidateArgumentItemsHasNotNull(initItemArray.HasNullItem(), nameof(initItems));

            return initItemArray.Length;
        }))())
        {
            SetRange(0, CastInternal(initItems));
            PropagateCollectionChangeChangeEvent();
        }

        /// <summary>
        /// 本来のCollectionChangeイベントを自身のCollectionChangeイベントに伝播させる。
        /// </summary>
        private void PropagateCollectionChangeChangeEvent()
        {
            base.CollectionChanging += (_, args) =>
            {
                CollectionChangingEventHandlers.ForEach(handler =>
                {
                    var myArgs = NotifyCollectionChangedEventArgsEx<TOut>.CreateFromOtherType(args, item => item);
                    handler.Invoke(this, myArgs);
                });
            };

            base.CollectionChanged += (_, args) =>
            {
                CollectionChangedEventHandlers.ForEach(handler =>
                {
                    var myArgs = NotifyCollectionChangedEventArgsEx<TOut>.CreateFromOtherType(args, item => item);
                    handler.Invoke(this, myArgs);
                });
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
        public void Reset(IEnumerable<TIn> items)
        {
            ThrowHelper.ValidateArgumentNotNull(items is null, nameof(items));

            Reset(CastInternal(items));
        }

        /// <inheritdoc/>
        public bool ItemEquals(IReadOnlyExtendedList<TIn, TOut>? other)
            => ItemEquals((IReadOnlyExtendedList<TInternal, TInternal>?)other);

        /// <inheritdoc cref="IDeepCloneableList{T,TIn}.DeepCloneWith"/>
        public new TImpl DeepCloneWith<TItem>(IReadOnlyDictionary<int, TItem>? values, int? length = null)
            where TItem : TIn
        {
            if (values is null) return DeepCloneWith(length);

            var cloneValues = new Dictionary<int, TInternal>();
            values.ForEach(pair =>
            {
                var cloneValue = CastInternal(pair.Value);
                cloneValues[pair.Key] = cloneValue;
            });

            return base.DeepCloneWith(cloneValues, length);
        }

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
        /// <typeparamref name="TIn"/> を <typeparamref name="TInternal"/> に変換したディープクローンを生成する。
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

        private IEnumerable<TInternal> CastInternal(IEnumerable<TOut> src)
            => src.Select(item => CloneToInternal(item));
    }


    /// <summary>
    ///     容量固定のList基底クラス
    /// </summary>
    /// <remarks>
    ///     機能概要は <seealso cref="IFixedLengthList{T, T}"/> 参照。
    /// </remarks>
    /// <typeparam name="T">リスト内包型</typeparam>
    /// <typeparam name="TImpl">リスト実装型</typeparam>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class FixedLengthList<T, TImpl> : FixedLengthList<T, T, T, TImpl>,
        IFixedLengthList<T>
        where TImpl : FixedLengthList<T, TImpl>
    {
        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="length">要素数</param>
        protected FixedLengthList(int length) : base(length)
        {
        }

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="initItems">初期リスト</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="initItems"/> が <see langword="null"/> の場合、
        ///     または <paramref name="initItems"/> 中に <see langword="null"/> が含まれる場合。
        /// </exception>
        protected FixedLengthList(IEnumerable<T> initItems) : base(((Func<int>)(() =>
        {
            ThrowHelper.ValidateArgumentNotNull(initItems is null, nameof(initItems));
            var initItemArray = initItems.ToArray();
            ThrowHelper.ValidateArgumentItemsHasNotNull(initItemArray.HasNullItem(), nameof(initItems));

            var itemLength = initItemArray.Length;

            return itemLength;
        }))())
        {
            SetRange(0, initItems);
        }

        /// <inheritdoc/>
        protected override T CloneToInternal(T src) => src;
    }
}
