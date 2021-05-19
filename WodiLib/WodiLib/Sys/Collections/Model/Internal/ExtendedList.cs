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
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using WodiLib.Sys.Cmn;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     WodiLib 独自リスト
    /// </summary>
    /// <remarks>
    ///     機能概要は <seealso cref="IExtendedList{T}"/> 参照。
    /// </remarks>
    /// <typeparam name="T">リスト内包クラス</typeparam>
    internal partial class ExtendedList<T> : ModelBase<ExtendedList<T>>, IExtendedList<T>,
        IDeepCloneableExtendedList<ExtendedList<T>, T>
    {
        /*
         * WodiLib 内部で使用する独自汎用リスト。
         * リストとしての機能は SimpleList<T> に委譲。
         * ExtendedList はコレクション変更通知を行うためのラッパークラスとする。
         *
         * 引数の検証も当クラスでは行わない。
         */

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Event
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /* マルチスレッドを考慮して、イベントハンドラ本体の実装は自動実装に任せる。 */
        [field: NonSerialized] private event NotifyCollectionChangedEventHandler? _collectionChanging;

        /// <inheritdoc/>
        public event NotifyCollectionChangedEventHandler CollectionChanging
        {
            add
            {
                if (_collectionChanging != null
                    && _collectionChanging.GetInvocationList().Contains(value)) return;
                _collectionChanging += value;
            }
            remove => _collectionChanging -= value;
        }

        /* マルチスレッドを考慮して、イベントハンドラ本体の実装は自動実装に任せる。 */
        [field: NonSerialized] private event NotifyCollectionChangedEventHandler? _collectionChanged;

        /// <inheritdoc/>
        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add
            {
                if (_collectionChanged != null
                    && _collectionChanged.GetInvocationList().Contains(value)) return;
                _collectionChanged += value;
            }
            remove => _collectionChanged -= value;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc cref="IExtendedList{T}.this"/>
        public T this[int index]
        {
            get => Items[index];
            set => Items[index] = value;
        }

        T IReadOnlyList<T>.this[int index] => Get_Impl(index, 1).First();

        /// <inheritdoc cref="IExtendedList{T}.Count"/>
        public int Count => Items.Count;

        /// <inheritdoc/>
        public bool IsReadOnly => false;

        /// <summary>
        ///     要素初期化関数
        /// </summary>
        /// <remarks>
        ///     第一引数：開始インデックス<br/>
        ///     第二引数：必要要素数<br/>
        ///     呼び出し元で設定必須。
        /// </remarks>
        public Func<int, int, IEnumerable<T>> FuncMakeItems { get; set; } = default!;

        /// <inheritdoc cref="INotifyCollectionChange.NotifyCollectionChangingEventType"/>
        public NotifyCollectionChangeEventType NotifyCollectionChangingEventType { get; set; }
            = WodiLibConfig.GetDefaultNotifyBeforeCollectionChangeEventType();

        /// <inheritdoc cref="INotifyCollectionChange.NotifyCollectionChangedEventType"/>
        public NotifyCollectionChangeEventType NotifyCollectionChangedEventType { get; set; }
            = WodiLibConfig.GetDefaultNotifyAfterCollectionChangeEventType();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>リスト本体</summary>
        private SimpleList<T> Items { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="initItems">初期要素</param>
        internal ExtendedList(IEnumerable<T>? initItems = null)
        {
            var initItemArray = initItems?.ToArray() ?? Array.Empty<T>();

            Items = new SimpleList<T>(initItemArray);

            PropagatePropertyChangeEvent(Items);
        }

        /// <summary>
        /// ディープコピーコンストラクタ
        /// </summary>
        /// <param name="src">コピー元</param>
        /// <param name="length">コピー後の要素数</param>
        /// <param name="values">コピー時上書き要素</param>
        /// <param name="funcMakeDefaultItem">デフォルト要素生成関数</param>
        internal ExtendedList(IEnumerable<T> src, int? length, IReadOnlyDictionary<int, T>? values,
            Func<int, T> funcMakeDefaultItem)
        {
            Items = new SimpleList<T>(src);
            FuncMakeItems = (index, count) => Enumerable.Range(index, count).Select(funcMakeDefaultItem);

            if (length is not null)
            {
                AdjustLength(length.Value);
            }

            values?.ForEach(pair =>
            {
                var key = pair.Key;
                if (0 <= key && key < Count)
                {
                    this[key] = pair.Value;
                }
            });
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc/>
        public IEnumerator<T> GetEnumerator() => Items.GetEnumerator();

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        /// <inheritdoc cref="IExtendedList{T}.IndexOf"/>
        public int IndexOf([AllowNull] T item)
            => Items.IndexOf(item);

        /// <inheritdoc cref="IExtendedList{T}.Contains"/>
        public bool Contains([AllowNull] T item)
            => Items.Contains(item);

        /// <inheritdoc/>
        public IEnumerable<T> GetRange(int index, int count)
            => Get_Impl(index, count);

        /// <inheritdoc/>
        public void SetRange(int index, IEnumerable<T> items)
            => Set_Impl(index, items.ToArray());

        /// <inheritdoc/>
        public void Add(T item)
            => Insert_Impl(Count, item);

        /// <inheritdoc/>
        public void AddRange(IEnumerable<T> items)
            => Insert_Impl(Count, items.ToArray());

        /// <inheritdoc/>
        public void Insert(int index, T item)
            => Insert_Impl(index, item);

        /// <inheritdoc/>
        public void InsertRange(int index, IEnumerable<T> items)
            => Insert_Impl(index, items.ToArray());

        /// <inheritdoc/>
        public void Overwrite(int index, IEnumerable<T> items)
            => Overwrite_Impl(index, items.ToArray());

        /// <inheritdoc/>
        public void Move(int oldIndex, int newIndex)
            => Move_Impl(oldIndex, newIndex, 1);

        /// <inheritdoc/>
        public void MoveRange(int oldIndex, int newIndex, int count)
            => Move_Impl(oldIndex, newIndex, count);

        /// <inheritdoc/>
        public bool Remove(T item)
            => Remove_Impl(item);

        /// <inheritdoc/>
        public void RemoveAt(int index)
            => Remove_Impl(index, 1);

        /// <inheritdoc/>
        public void RemoveRange(int index, int count)
            => Remove_Impl(index, count);

        /// <inheritdoc/>
        public void AdjustLength(int length)
            => AdjustLength_Impl(length);

        /// <inheritdoc/>
        public void AdjustLengthIfShort(int length)
            => AdjustLengthIfShort_Impl(length);

        /// <inheritdoc/>
        public void AdjustLengthIfLong(int length)
            => AdjustLengthIfLong_Impl(length);

        /// <inheritdoc/>
        public void Reset(IEnumerable<T> initItems)
            => Reset_Impl(initItems.ToArray());

        /// <inheritdoc/>
        public void Clear()
            => Clear_Impl();

        /// <inheritdoc cref="IExtendedList{T}.IndexOf"/>
        public void CopyTo(T[] array, int arrayIndex)
            => Items.CopyTo(array, arrayIndex);

        #region Equals

        /// <inheritdoc/>
        public override bool ItemEquals(ExtendedList<T>? other)
            => ItemEquals((IEnumerable<T>?) other);

        /// <inheritdoc/>
        public bool ItemEquals(IExtendedList<T>? other)
            => ItemEquals((IEnumerable<T>?) other);

        /// <inheritdoc/>
        public bool ItemEquals(IReadOnlyExtendedList<T>? other)
            => ItemEquals((IEnumerable<T>?) other);

        /// <inheritdoc/>
        public bool ItemEquals(IEnumerable<T>? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            var otherList = other.ToList();

            if (Count != otherList.Count) return false;
            if (Count == 0) return true;

            return this.Zip(otherList).All(zip =>
            {
                var (x, y) = zip;
                if (x is IEqualityComparable comparable)
                {
                    return comparable.ItemEquals(y);
                }

                return x!.Equals(y);
            });
        }

        #endregion

        #region Clone

        /// <inheritdoc />
        public override ExtendedList<T> DeepClone()
        {
            return new(this);
        }

        /// <inheritdoc />
        IExtendedList<T> IDeepCloneable<IExtendedList<T>>.DeepClone()
            => DeepClone();

        /// <inheritdoc />
        IReadOnlyExtendedList<T> IDeepCloneable<IReadOnlyExtendedList<T>>.DeepClone()
            => DeepClone();

        /// <inheritdoc cref="IDeepCloneableExtendedList{T,TIn}.DeepCloneWith"/>
        public ExtendedList<T> DeepCloneWith(int? length, IReadOnlyDictionary<int, T>? values)
        {
            var valueList = values?.ToList();
            valueList?.ForEach(pair =>
            {
                ThrowHelper.ValidateArgumentNotNull(pair.Value is null, $"{nameof(values)} の要素 (Key: {pair.Key})");
            });

            var result = DeepClone();

            if (length is not null)
            {
                result.AdjustLength(length.Value);
            }

            valueList?.ForEach(pair =>
            {
                if (result.Count < pair.Key) result[pair.Key] = pair.Value;
            });

            return result;
        }

        /// <inheritdoc />
        IReadOnlyExtendedList<T> IDeepCloneableExtendedList<IReadOnlyExtendedList<T>, T>.DeepCloneWith(int? length,
            IReadOnlyDictionary<int, T>? values)
            => DeepCloneWith(length, values);

        /// <inheritdoc />
        IExtendedList<T> IDeepCloneableExtendedList<IExtendedList<T>, T>.DeepCloneWith(int? length,
            IReadOnlyDictionary<int, T>? values)
            => DeepCloneWith(length, values);

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Action Implements

        /// <summary>
        ///     インデクサによる要素取得、GetRange メソッドの実装処理
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <param name="count">要素数</param>
        /// <returns></returns>
        private IEnumerable<T> Get_Impl(int index, int count)
            => Items.Get(index, count);

        /// <summary>
        ///     インデクサによる要素更新、SetRange メソッドの実装処理
        /// </summary>
        /// <param name="index">更新開始インデックス</param>
        /// <param name="items">更新要素</param>
        private void Set_Impl(int index, params T[] items)
        {
            if (items.Length == 0) return;

            var oldItems = Items.Get(index, items.Length).ToArray();

            var collectionChangeEventArgsFactory =
                CollectionChangeEventArgsFactory<T>.CreateSet(this, index, oldItems, items);

            var notifyManager = MakeNotifyManager(
                collectionChangeEventArgsFactory.CollectionChangingEventArgs,
                collectionChangeEventArgsFactory.CollectionChangedEventArgs,
                ListConstant.IndexerName);

            notifyManager.NotifyBeforeEvent();

            Items.Set(index, items);

            notifyManager.NotifyAfterEvent();
        }

        /// <summary>
        ///     Add, AddRange, Insert, InsertRange メソッドの実装処理
        /// </summary>
        /// <param name="index">挿入先インデックス</param>
        /// <param name="items">挿入要素</param>
        private void Insert_Impl(int index, params T[] items)
        {
            if (items.Length == 0) return;

            var collectionChangeEventArgsFactory =
                CollectionChangeEventArgsFactory<T>.CreateInsert(this, index, items);

            var notifyManager = MakeNotifyManager(
                collectionChangeEventArgsFactory.CollectionChangingEventArgs,
                collectionChangeEventArgsFactory.CollectionChangedEventArgs,
                nameof(IList.Count), ListConstant.IndexerName);

            notifyManager.NotifyBeforeEvent();

            Items.Insert(index, items);

            notifyManager.NotifyAfterEvent();
        }

        /// <summary>
        ///     Overwrite メソッドの実装処理
        /// </summary>
        /// <param name="index">上書き開始インデックス</param>
        /// <param name="items">上書き要素</param>
        private void Overwrite_Impl(int index, params T[] items)
        {
            if (items.Length == 0) return;

            var param = OverwriteParam<T>.Factory.Create(this, index, items);

            var collectionChangeEventArgsFactory =
                CollectionChangeEventArgsFactory<T>.CreateOverwrite(this, index, param.ReplaceOldItems,
                    param.ReplaceNewItems, param.InsertItems);

            var notifyManager = MakeNotifyManager(
                collectionChangeEventArgsFactory.CollectionChangingEventArgs,
                collectionChangeEventArgsFactory.CollectionChangedEventArgs,
                param.NotifyProperties);

            // 処理本体
            notifyManager.NotifyBeforeEvent();

            Items.Set(index, param.ReplaceNewItems);
            Items.Insert(param.InsertStartIndex, param.InsertItems);

            notifyManager.NotifyAfterEvent();
        }

        /// <summary>
        ///     Move, MoveRange メソッドの実装処理
        /// </summary>
        /// <param name="oldIndex">移動する項目のインデックス開始位置</param>
        /// <param name="newIndex">移動先のインデックス開始位置</param>
        /// <param name="count">移動させる要素数</param>
        private void Move_Impl(int oldIndex, int newIndex, int count)
        {
            if (count == 0) return;

            var moveItems = Items.Get(oldIndex, count).ToArray();

            var collectionChangeEventArgsFactory =
                CollectionChangeEventArgsFactory<T>.CreateMove(this, oldIndex, newIndex, moveItems);

            var notifyManager = MakeNotifyManager(
                collectionChangeEventArgsFactory.CollectionChangingEventArgs,
                collectionChangeEventArgsFactory.CollectionChangedEventArgs,
                ListConstant.IndexerName);

            notifyManager.NotifyBeforeEvent();

            Items.Move(oldIndex, newIndex, count);

            notifyManager.NotifyAfterEvent();
        }

        /// <summary>
        ///     Remove メソッドの実装処理
        /// </summary>
        /// <param name="item">除去対象</param>
        /// <returns>削除成否</returns>
        private bool Remove_Impl([AllowNull] T item)
        {
            if (item is null) return false;

            var index = Items.IndexOf(item);
            if (index < 0) return false;

            var collectionChangeEventArgsFactory =
                CollectionChangeEventArgsFactory<T>.CreateRemove(this, index, new List<T> {item});

            var notifyManager = MakeNotifyManager(
                collectionChangeEventArgsFactory.CollectionChangingEventArgs,
                collectionChangeEventArgsFactory.CollectionChangedEventArgs,
                nameof(IList.Count), ListConstant.IndexerName);

            notifyManager.NotifyBeforeEvent();

            Items.Remove(index, 1);

            notifyManager.NotifyAfterEvent();

            return true;
        }

        /// <summary>
        ///     Remove, RemoveRange メソッドの実装処理
        /// </summary>
        /// <param name="index">除去開始インデックス</param>
        /// <param name="count">除去する要素数</param>
        private void Remove_Impl(int index, int count)
        {
            if (count == 0) return;

            var removeItems = Get_Impl(index, count).ToArray();

            var collectionChangeEventArgsFactory =
                CollectionChangeEventArgsFactory<T>.CreateRemove(this, index, removeItems);

            var notifyManager = MakeNotifyManager(
                collectionChangeEventArgsFactory.CollectionChangingEventArgs,
                collectionChangeEventArgsFactory.CollectionChangedEventArgs,
                nameof(IList.Count), ListConstant.IndexerName);

            notifyManager.NotifyBeforeEvent();

            Items.Remove(index, count);

            notifyManager.NotifyAfterEvent();
        }

        /// <summary>
        ///     AdjustLength メソッドの処理実装
        /// </summary>
        /// <param name="length">調整要素数</param>
        private void AdjustLength_Impl(int length)
        {
            if (length == Count)
            {
                return;
            }

            if (Count > length)
            {
                AdjustLengthIfLong_Main(length);
            }
            else // Count < length
            {
                AdjustLengthIfShort_Main(length);
            }
        }

        /// <summary>
        ///     AdjustLengthIfShort メソッドの処理実装
        /// </summary>
        /// <param name="length">調整要素数</param>
        private void AdjustLengthIfShort_Impl(int length)
        {
            if (length <= Count) return;

            AdjustLengthIfShort_Main(length);
        }

        /// <summary>
        ///     AdjustLengthIfLong メソッドの処理実装
        /// </summary>
        /// <param name="length">調整要素数</param>
        private void AdjustLengthIfLong_Impl(int length)
        {
            if (length >= Count) return;

            AdjustLengthIfLong_Main(length);
        }

        /// <summary>
        ///     AdjustLengthIfShort メソッドの処理実装
        /// </summary>
        /// <param name="length">調整要素数</param>
        private void AdjustLengthIfShort_Main(int length)
        {
            var startIndex = Count;
            var items = FuncMakeItems(startIndex, length - startIndex)
                .ToArray();

            var collectionChangeEventArgsFactory =
                CollectionChangeEventArgsFactory<T>.CreateAdjustLengthIfShort(this, startIndex, items);

            var notifyManager = MakeNotifyManager(
                collectionChangeEventArgsFactory.CollectionChangingEventArgs,
                collectionChangeEventArgsFactory.CollectionChangedEventArgs,
                nameof(IList.Count), ListConstant.IndexerName);

            notifyManager.NotifyBeforeEvent();

            Items.Insert(startIndex, items);

            notifyManager.NotifyAfterEvent();
        }

        /// <summary>
        ///     AdjustLengthIfLong メソッドの処理実装
        /// </summary>
        /// <param name="length">調整要素数</param>
        private void AdjustLengthIfLong_Main(int length)
        {
            var index = length;
            var count = Count - length;
            var removeItems = Get_Impl(index, count).ToArray();

            var collectionChangeEventArgsFactory =
                CollectionChangeEventArgsFactory<T>.CreateAdjustLengthIfLong(this, index, removeItems);

            var notifyManager = MakeNotifyManager(
                collectionChangeEventArgsFactory.CollectionChangingEventArgs,
                collectionChangeEventArgsFactory.CollectionChangedEventArgs,
                nameof(IList.Count), ListConstant.IndexerName);

            notifyManager.NotifyBeforeEvent();

            Items.Remove(index, count);

            notifyManager.NotifyAfterEvent();
        }

        /// <summary>
        ///     Clear メソッドの実装処理
        /// </summary>
        private void Clear_Impl()
        {
            Reset_Impl(Array.Empty<T>());
        }

        /// <summary>
        ///     Reset メソッドの実装処理
        /// </summary>
        /// <param name="items">初期化要素</param>
        private void Reset_Impl(params T[] items)
        {
            var collectionChangeEventArgsFactory =
                CollectionChangeEventArgsFactory<T>.CreateReset(this, this, items);

            var notifyManager = MakeNotifyManager(
                collectionChangeEventArgsFactory.CollectionChangingEventArgs,
                collectionChangeEventArgsFactory.CollectionChangedEventArgs,
                nameof(IList.Count), ListConstant.IndexerName);

            notifyManager.NotifyBeforeEvent();

            Items.Reset(items);

            notifyManager.NotifyAfterEvent();
        }

        #endregion

        /// <summary>
        ///     PreCollectionChanged イベントを発火する。
        /// </summary>
        /// <param name="args">イベント引数</param>
        private void CallCollectionChanging(NotifyCollectionChangedEventArgs args)
            => _collectionChanging?.Invoke(this, args);

        /// <summary>
        ///     CollectionChanged イベントを発火する。
        /// </summary>
        /// <param name="args">イベント引数</param>
        private void CallCollectionChanged(NotifyCollectionChangedEventArgs args)
            => _collectionChanged?.Invoke(this, args);
    }
}