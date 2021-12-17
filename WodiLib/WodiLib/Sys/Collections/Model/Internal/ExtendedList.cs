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
using System.ComponentModel;
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
    internal partial class ExtendedList<T> : ModelBase<ExtendedList<T>>,
        IExtendedList<T>
    {
        /*
         * WodiLib 内部で使用する独自汎用リスト。
         * リストとしての機能は SimpleList<T> に委譲。
         * ExtendedList はコレクション変更通知を行うためのラッパークラスとする。
         *
         * 引数の検証も当クラスでは行わない。
         */
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Constants
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private const int MaxCapacity = int.MaxValue;

        private const int MinCapacity = 0;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Events
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc/>
        public event EventHandler<NotifyCollectionChangedEventArgsEx<T>> CollectionChanging
        {
            add
            {
                if (_collectionChanging != null
                    && _collectionChanging.GetInvocationList().Contains(value)) return;
                _collectionChanging += value;
            }
            remove => _collectionChanging -= value;
        }

        /// <inheritdoc/>
        public event EventHandler<NotifyCollectionChangedEventArgsEx<T>> CollectionChanged
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
        //      Public Properties
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc cref="IRestrictedCapacityList{T, T}.this"/>
        public T this[int index]
        {
            get => Get_Impl(index, 1).First();
            set => Set_Impl(index, value);
        }

        /// <inheritdoc cref="IExtendedList{T}.Count"/>
        public int Count => Items.Count;

        /// <summary>
        ///     要素初期化関数
        /// </summary>
        /// <remarks>
        ///     第一引数：開始インデックス<br/>
        ///     第二引数：必要要素数<br/>
        ///     呼び出し元で設定必須。
        /// </remarks>
        public Func<int, int, IEnumerable<T>> FuncMakeItems
        {
            get => Items.FuncMakeItems;
            init => Items.FuncMakeItems = value;
        }

        /// <inheritdoc cref="INotifiableCollectionChange{T}.NotifyCollectionChangingEventType"/>
        public NotifyCollectionChangeEventType NotifyCollectionChangingEventType
        {
            get => notifyCollectionChangingEventType;
            set
            {
                ThrowHelper.ValidatePropertyNotNull(value is null, nameof(NotifyCollectionChangingEventType));
                notifyCollectionChangingEventType = value;
                ApplyPropertyChangingEventType(this);
            }
        }

        /// <inheritdoc cref="INotifiableCollectionChange{T}.NotifyCollectionChangedEventType"/>
        public NotifyCollectionChangeEventType NotifyCollectionChangedEventType
        {
            get => notifyCollectionChangedEventType;
            set
            {
                ThrowHelper.ValidatePropertyNotNull(value is null, nameof(NotifyCollectionChangedEventType));
                notifyCollectionChangedEventType = value;
                ApplyPropertyChangedEventType(this);
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Fields
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /* マルチスレッドを考慮して、イベントハンドラ本体の実装は自動実装に任せる。 */
        private event EventHandler<NotifyCollectionChangedEventArgsEx<T>>? _collectionChanging;
        private event EventHandler<NotifyCollectionChangedEventArgsEx<T>>? _collectionChanged;
        private event EventHandler<NotifyCollectionChangedEventArgs>? _originalCollectionChanging;
        private event NotifyCollectionChangedEventHandler? _originalCollectionChanged;

        private NotifyCollectionChangeEventType notifyCollectionChangingEventType
            = WodiLibConfig.GetDefaultNotifyBeforeCollectionChangeEventType();

        private NotifyCollectionChangeEventType notifyCollectionChangedEventType
            = WodiLibConfig.GetDefaultNotifyAfterCollectionChangeEventType();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Private Properties
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>リスト本体</summary>
        private SimpleList<T> Items { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Constructors
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="initItems">初期要素</param>
        internal ExtendedList(IEnumerable<T>? initItems = null)
        {
            var initItemArray = initItems?.ToArray() ?? Array.Empty<T>();

            Items = new SimpleList<T>(initItemArray);

            AddInnerItemPropertyChangeEvent(Items.ToArray());
            PropagatePropertyChangeEvent(Items);
        }

        /// <summary>
        ///     ディープコピーコンストラクタ
        /// </summary>
        /// <param name="src">コピー元</param>
        /// <param name="length">コピー後の要素数</param>
        /// <param name="values">コピー時上書き要素</param>
        /// <param name="funcMakeItem">デフォルト要素生成関数</param>
        private ExtendedList(ExtendedList<T> src, int? length, IReadOnlyDictionary<int, T>? values,
            Func<int, int, IEnumerable<T>> funcMakeItem) : base(src)
        {
            Items = new SimpleList<T>(src, true);
            FuncMakeItems = funcMakeItem;
            NotifyCollectionChangingEventType = src.NotifyCollectionChangingEventType;
            NotifyCollectionChangedEventType = src.NotifyCollectionChangedEventType;

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

            AddInnerItemPropertyChangeEvent(Items.ToArray());
            PropagatePropertyChangeEvent(Items);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Public Methods
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc/>
        public int GetMaxCapacity() => MaxCapacity;

        /// <inheritdoc/>
        public int GetMinCapacity() => MinCapacity;

        /// <inheritdoc/>
        public IEnumerator<T> GetEnumerator() => Items.GetEnumerator();

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
        public bool Remove(T? item)
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
        public void Reset()
            => Reset_Impl(FuncMakeItems(0, Count).ToArray());

        /// <inheritdoc cref="ISizeChangeableList{TIn,TOut}"/>
        public void Reset(IEnumerable<T> initItems)
            => Reset_Impl(initItems.ToArray());

        /// <inheritdoc/>
        public void Clear()
            => Clear_Impl();

        /// <inheritdoc/>
        public override bool ItemEquals(ExtendedList<T>? other)
            => ItemEquals(other, null);

        /// <inheritdoc/>
        public override ExtendedList<T> DeepClone()
        {
            return new ExtendedList<T>(this, null, null, FuncMakeItems);
        }

        /// <inheritdoc/>
        public bool ItemEquals<TOther>(IEnumerable<TOther>? other, IEqualityComparer<TOther>? itemComparer)
            => Items.ItemEquals(other, itemComparer);

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Interface Implementation
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region CollectionChanged

        event EventHandler<NotifyCollectionChangedEventArgs>? INotifiableCollectionChange.CollectionChanging
        {
            add
            {
                if (_originalCollectionChanging != null
                    && _originalCollectionChanging.GetInvocationList().Contains(value)) return;
                _originalCollectionChanging += value;
            }
            remove => _originalCollectionChanging -= value;
        }

        event NotifyCollectionChangedEventHandler INotifyCollectionChanged.CollectionChanged
        {
            add
            {
                if (_originalCollectionChanged != null
                    && _originalCollectionChanged.GetInvocationList().Contains(value)) return;
                _originalCollectionChanged += value;
            }
            remove => _originalCollectionChanged -= value;
        }

        #endregion

        #region GetEnumerator

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        #endregion

        #region ItemEquals

        bool IEqualityComparable<IReadOnlyExtendedList<T, T>>.ItemEquals(IReadOnlyExtendedList<T, T>? other)
            => ItemEquals(other, null);

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Private Methods
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     PreCollectionChanged イベントを発火する。
        /// </summary>
        /// <param name="args">イベント引数</param>
        private void CallCollectionChanging(NotifyCollectionChangedEventArgsEx<T> args)
        {
            _collectionChanging?.Invoke(this, args);
            _originalCollectionChanging?.Invoke(this, args);
        }

        /// <summary>
        ///     CollectionChanged イベントを発火する。
        /// </summary>
        /// <param name="args">イベント引数</param>
        private void CallCollectionChanged(NotifyCollectionChangedEventArgsEx<T> args)
        {
            _collectionChanged?.Invoke(this, args);
            _originalCollectionChanged?.Invoke(this, args);
        }

        private void InnerItemOnPropertyChanging(object sender, PropertyChangingEventArgs e)
        {
            NotifyPropertyChanging(ListConstant.IndexerName);
        }

        private void InnerItemOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            NotifyPropertyChanged(ListConstant.IndexerName);
        }

        private void InnerItemOnCollectionChanging(object sender, NotifyCollectionChangedEventArgs e)
        {
            NotifyPropertyChanging(ListConstant.IndexerName);
        }

        private void InnerItemOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            NotifyPropertyChanged(ListConstant.IndexerName);
        }

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

            var isAllItemReferenceEqual = oldItems.Zip(items).All(zip => ReferenceEquals(zip.Item1, zip.Item2));
            if (isAllItemReferenceEqual)
            {
                return;
            }

            var collectionChangeEventArgsFactory =
                CollectionChangeEventArgsFactory<T>.CreateSet(this, index, oldItems, items);

            var notifyManager = MakeNotifyManager(
                collectionChangeEventArgsFactory.CollectionChangingEventArgs,
                collectionChangeEventArgsFactory.CollectionChangedEventArgs,
                ListConstant.IndexerName);

            RemoveInnerItemPropertyChangeEvent(oldItems);
            notifyManager.NotifyBeforeEvent();

            Items.Set(index, items);

            notifyManager.NotifyAfterEvent();
            ApplyPropertyChangeEventType(items);
            AddInnerItemPropertyChangeEvent(items);
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
            ApplyPropertyChangeEventType(items);
            AddInnerItemPropertyChangeEvent(items);
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
            RemoveInnerItemPropertyChangeEvent(param.ReplaceOldItems);
            notifyManager.NotifyBeforeEvent();

            Items.Overwrite(index, param);

            notifyManager.NotifyAfterEvent();
            ApplyPropertyChangeEventType(items);
            AddInnerItemPropertyChangeEvent(items);
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
        private bool Remove_Impl(T? item)
        {
            if (item is null) return false;

            var index = Items.IndexOf(item);
            if (index < 0) return false;

            var collectionChangeEventArgsFactory =
                CollectionChangeEventArgsFactory<T>.CreateRemove(this, index, new List<T> { item });

            var notifyManager = MakeNotifyManager(
                collectionChangeEventArgsFactory.CollectionChangingEventArgs,
                collectionChangeEventArgsFactory.CollectionChangedEventArgs,
                nameof(IList.Count), ListConstant.IndexerName);

            RemoveInnerItemPropertyChangeEvent(item);
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

            RemoveInnerItemPropertyChangeEvent(removeItems);
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

            Items.Add(items);

            notifyManager.NotifyAfterEvent();
            ApplyPropertyChangeEventType(items);
            AddInnerItemPropertyChangeEvent(items);
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

            RemoveInnerItemPropertyChangeEvent(removeItems);
            notifyManager.NotifyBeforeEvent();

            Items.AdjustIfLong(length);

            notifyManager.NotifyAfterEvent();
        }

        /// <summary>
        ///     Reset メソッドの実装処理
        /// </summary>
        /// <param name="items">初期化要素</param>
        private void Reset_Impl(params T[] items)
        {
            var oldItems = this.ToArray();

            var collectionChangeEventArgsFactory =
                CollectionChangeEventArgsFactory<T>.CreateReset(this, oldItems, items);

            var notifyManager = MakeNotifyManager(
                collectionChangeEventArgsFactory.CollectionChangingEventArgs,
                collectionChangeEventArgsFactory.CollectionChangedEventArgs,
                nameof(IList.Count), ListConstant.IndexerName);

            RemoveInnerItemPropertyChangeEvent(oldItems);
            notifyManager.NotifyBeforeEvent();

            Items.Reset(items);

            notifyManager.NotifyAfterEvent();
            ApplyPropertyChangeEventType(items);
            AddInnerItemPropertyChangeEvent(items);
        }

        /// <summary>
        ///     Clear メソッドの実装処理
        /// </summary>
        private void Clear_Impl()
        {
            Reset_Impl(Array.Empty<T>());
        }

        /// <summary>
        ///     指定した要素が <see cref="INotifiablePropertyChange"/> を実装する場合、
        ///     各要素の <see cref="INotifiablePropertyChange.NotifyPropertyChangingEventType"/>,
        ///     <see cref="INotifiablePropertyChange.NotifyPropertyChangedEventType"/> に
        ///     自身と同じ値を反映する。
        /// </summary>
        /// <param name="items">処理対象</param>
        private void ApplyPropertyChangeEventType(IEnumerable<T> items)
        {
            var itemArray = items.ToArray();
            if (itemArray.Length == 0)
            {
                return;
            }

            if (itemArray[0] is not INotifiablePropertyChange)
            {
                return;
            }

            itemArray.ForEach(item =>
            {
                ((INotifiablePropertyChange)item!).NotifyPropertyChangingEventType =
                    NotifyPropertyChangingEventType;
                ((INotifiablePropertyChange)item).NotifyPropertyChangedEventType = NotifyPropertyChangedEventType;
            });
        }

        /// <summary>
        ///     指定した要素が <see cref="INotifiablePropertyChange"/> を実装する場合、
        ///     各要素の <see cref="INotifiablePropertyChange.NotifyPropertyChangingEventType"/> に
        ///     自身の <see cref="INotifiablePropertyChange.NotifyPropertyChangingEventType"/> の値を反映する。
        /// </summary>
        /// <param name="items">処理対象</param>
        private void ApplyPropertyChangingEventType(IEnumerable<T> items)
        {
            var itemArray = items.ToArray();
            if (itemArray.Length == 0)
            {
                return;
            }

            if (itemArray[0] is not INotifiablePropertyChange)
            {
                return;
            }

            itemArray.ForEach(item =>
            {
                ((INotifiablePropertyChange)item!).NotifyPropertyChangingEventType =
                    NotifyPropertyChangingEventType;
            });
        }

        /// <summary>
        ///     指定した要素が <see cref="INotifiablePropertyChange"/> を実装する場合、
        ///     各要素の <see cref="INotifiablePropertyChange.NotifyPropertyChangedEventType"/> に
        ///     自身の <see cref="INotifiablePropertyChange.NotifyPropertyChangedEventType"/> の値を反映する。
        /// </summary>
        /// <param name="items">処理対象</param>
        private void ApplyPropertyChangedEventType(IEnumerable<T> items)
        {
            var itemArray = items.ToArray();
            if (itemArray.Length == 0)
            {
                return;
            }

            if (itemArray[0] is not INotifiablePropertyChange)
            {
                return;
            }

            itemArray.ForEach(item =>
            {
                ((INotifiablePropertyChange)item!).NotifyPropertyChangedEventType = NotifyPropertyChangedEventType;
            });
        }

        /// <summary>
        ///     リストに追加した要素に対し変更通知イベントを登録する。
        /// </summary>
        /// <param name="items">追加した要素</param>
        private void AddInnerItemPropertyChangeEvent(params T[] items)
        {
            items.ForEach(item =>
            {
                switch (item)
                {
                    case INotifiableCollectionChange notifiableCollectionChange:
                        notifiableCollectionChange.CollectionChanging += InnerItemOnCollectionChanging;
                        notifiableCollectionChange.CollectionChanged += InnerItemOnCollectionChanged;
                        break;
                    case INotifyCollectionChanged notifyCollectionChanged:
                        notifyCollectionChanged.CollectionChanged += InnerItemOnCollectionChanged;
                        break;
                }

                if (item is INotifiablePropertyChange notifiablePropertyChange)
                {
                    notifiablePropertyChange.PropertyChanging += InnerItemOnPropertyChanging;
                    notifiablePropertyChange.PropertyChanged += InnerItemOnPropertyChanged;
                }
                else
                {
                    if (item is INotifyPropertyChanging notifyPropertyChanging)
                    {
                        notifyPropertyChanging.PropertyChanging += InnerItemOnPropertyChanging;
                    }

                    if (item is INotifyPropertyChanged notifyPropertyChanged)
                    {
                        notifyPropertyChanged.PropertyChanged += InnerItemOnPropertyChanged;
                    }
                }
            });
        }

        /// <summary>
        ///     リストから除去する要素に対し登録した変更通知イベントを解除する。
        /// </summary>
        /// <param name="items">除去する要素</param>
        private void RemoveInnerItemPropertyChangeEvent(params T[] items)
        {
            items.ForEach(item =>
            {
                switch (item)
                {
                    case INotifiableCollectionChange notifiableCollectionChange:
                        notifiableCollectionChange.CollectionChanging -= InnerItemOnCollectionChanging;
                        notifiableCollectionChange.CollectionChanged -= InnerItemOnCollectionChanged;
                        break;
                    case INotifyCollectionChanged notifyCollectionChanged:
                        notifyCollectionChanged.CollectionChanged -= InnerItemOnCollectionChanged;
                        break;
                }

                if (item is INotifiablePropertyChange notifiablePropertyChange)
                {
                    notifiablePropertyChange.PropertyChanging -= InnerItemOnPropertyChanging;
                    notifiablePropertyChange.PropertyChanged -= InnerItemOnPropertyChanged;
                }
                else
                {
                    if (item is INotifyPropertyChanging notifyPropertyChanging)
                    {
                        notifyPropertyChanging.PropertyChanging -= InnerItemOnPropertyChanging;
                    }

                    if (item is INotifyPropertyChanged notifyPropertyChanged)
                    {
                        notifyPropertyChanged.PropertyChanged -= InnerItemOnPropertyChanged;
                    }
                }
            });
        }
    }
}
