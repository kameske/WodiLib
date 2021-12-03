// ========================================
// Project Name : WodiLib
// File Name    : TwoDimensionalList.cs
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
    ///     WodiLib内部で使用する二次元リスト実装クラス
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         外側のリストを「行（Row）」、内側のリストを「列（Column）」として扱う。<br/>
    ///         すべての行について列数は常に同じ値を取り続ける。
    ///     </para>
    ///     <para>
    ///         このクラスは WodiLib 内部でのみ使用する。WodiLib外部に見せる際はWrapperクラスを別途定義する。
    ///         使用箇所によって「行」や「列」の呼び方を変えたいことがあり、それに合わせて
    ///         メソッド名も適切なものを公開したいため。
    ///     </para>
    ///     <para>
    ///         内部状態について、
    ///         行数 > 0 かつ 列数 == 0 の状況にはなりうるが、行数 == 0 かつ 列数 > 0 の状況にはなりえない。
    ///     </para>
    /// </remarks>
    /// <typeparam name="TRow">リスト行データ型</typeparam>
    /// <typeparam name="TRowInternal">リスト行データ実装型</typeparam>
    /// <typeparam name="TItem">リスト要素型</typeparam>
    internal partial class TwoDimensionalList<TRow, TRowInternal, TItem>
        : ModelBase<TwoDimensionalList<TRow, TRowInternal, TItem>>,
            ITwoDimensionalList<TRow, TRow, TItem, TItem>,
            IDeepCloneableTwoDimensionalListInternal<TwoDimensionalList<TRow, TRowInternal, TItem>, TItem>
        where TRow : IFixedLengthList<TItem>
        where TRowInternal : class, IExtendedList<TItem>, TRow, IDeepCloneable<TRowInternal>
    {
        /*
         * このクラスの実装観点は ExtendedList<T> と同じ。
         * ExtendedList<T> とは違いこのクラス自身が二次元リストの実装となり、
         * イベント通知なども行う。
         */
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Events
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        public event EventHandler<NotifyCollectionChangedEventArgsEx<TRow>> CollectionChanging
        {
            add
            {
                ThrowHelper.ValidateArgumentNotNull(value is null, nameof(CollectionChanging));
                if (collectionChangingEvents.Contains(value))
                {
                    return;
                }

                collectionChangingEvents.Add(value);
            }
            remove
            {
                ThrowHelper.ValidateArgumentNotNull(value is null, nameof(CollectionChanging));
                collectionChangingEvents.Remove(value);
            }
        }

        public event EventHandler<NotifyCollectionChangedEventArgsEx<TRow>> CollectionChanged
        {
            add
            {
                ThrowHelper.ValidateArgumentNotNull(value is null, nameof(CollectionChanged));
                if (collectionChangedEvents.Contains(value))
                {
                    return;
                }

                collectionChangedEvents.Add(value);
            }
            remove
            {
                ThrowHelper.ValidateArgumentNotNull(value is null, nameof(CollectionChanged));
                collectionChangedEvents.Remove(value);
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Public Properties
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        public TItem this[int rowIndex, int columnIndex]
        {
            get
            {
                Validator?.GetItem(rowIndex, columnIndex);
                return Get_Impl(rowIndex, columnIndex);
            }
            set
            {
                ThrowHelper.ValidatePropertyNotNull(
                    value is null,
                    typeof(TwoDimensionalList<TRow, TRowInternal, TItem>).Name
                );
                Validator?.SetItem(rowIndex, columnIndex, value);
                SetItem_Impl(rowIndex, columnIndex, value);
            }
        }

        public bool IsEmpty => RowCount == 0;
        public int RowCount => Items.Count;
        public int ColumnCount => RowCount > 0 ? Items[0].Count : 0;
        public int AllCount => RowCount * ColumnCount;

        public override NotifyPropertyChangeEventType NotifyPropertyChangingEventType
        {
            get => Items.NotifyPropertyChangingEventType;
            set
            {
                Items.NotifyPropertyChangingEventType = value;
                ApplyPropertyChangingEventType(Items);
            }
        }

        public override NotifyPropertyChangeEventType NotifyPropertyChangedEventType
        {
            get => Items.NotifyPropertyChangedEventType;
            set
            {
                Items.NotifyPropertyChangedEventType = value;
                ApplyPropertyChangedEventType(Items);
            }
        }

        public NotifyCollectionChangeEventType NotifyCollectionChangingEventType
        {
            get => Items.NotifyCollectionChangingEventType;
            set
            {
                Items.NotifyCollectionChangingEventType = value;
                ApplyCollectionChangingEventType(Items);
            }
        }

        public NotifyCollectionChangeEventType NotifyCollectionChangedEventType
        {
            get => Items.NotifyCollectionChangedEventType;
            set
            {
                Items.NotifyCollectionChangedEventType = value;
                ApplyCollectionChangedEventType(Items);
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Private Properties
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private ExtendedList<TRowInternal> Items { get; }

        private Func<IEnumerable<TItem>, TRowInternal> FuncMakeDefaultRowFromItems => config.RowFactoryFromItems;
        private Func<TRow, TRowInternal> FuncMakeRowFromInType => config.ToInternalRow;
        private Func<int, int, TItem> FuncMakeDefaultItem => config.ItemFactory;

        private ITwoDimensionalListValidator<TRow, TItem>? Validator { get; }

        private int MaxRowCapacity => config.MaxRowCapacity;
        private int MinRowCapacity => config.MinRowCapacity;
        private int MaxColumnCapacity => config.MaxColumnCapacity;
        private int MinColumnCapacity => config.MinColumnCapacity;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Fields
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private readonly List<EventHandler<NotifyCollectionChangedEventArgsEx<TRow>>> collectionChangingEvents;
        private readonly List<EventHandler<NotifyCollectionChangedEventArgsEx<TRow>>> collectionChangedEvents;

        private readonly Config config;

        private readonly InnerListNotifyPropertyChangeAction notifyPropertyChangingAction;
        private readonly InnerListNotifyPropertyChangeAction notifyPropertyChangedAction;

        private readonly RowEventHandlers rowEventHandlers;

        private readonly IEqualityComparer<TItem> itemComparer;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Constructors
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        internal TwoDimensionalList(IEnumerable<TRow> values, Config config)
        {
            this.config = config;
            Validator = config.ValidatorFactory(this);

            var valuesArray = values.ToArray();

            Validator?.Constructor(valuesArray);

            var initItems = valuesArray.Select(FuncMakeRowFromInType);
            Items = new ExtendedList<TRowInternal>(initItems)
            {
                FuncMakeItems = (start, count)
                    => Enumerable.Range(0, count).Select(r => FuncMakeDefaultRowFromItems(
                        Enumerable.Range(0, ColumnCount).Select(c => FuncMakeDefaultItem(start + r, c))
                    ))
            };
            ApplyChangeEventType(Items);

            collectionChangingEvents = new List<EventHandler<NotifyCollectionChangedEventArgsEx<TRow>>>();
            collectionChangedEvents = new List<EventHandler<NotifyCollectionChangedEventArgsEx<TRow>>>();

            notifyPropertyChangingAction = new InnerListNotifyPropertyChangeAction(NotifyPropertyChanging);
            notifyPropertyChangedAction = new InnerListNotifyPropertyChangeAction(NotifyPropertyChanged);

            rowEventHandlers = new RowEventHandlers(this);

            itemComparer = EqualityComparerFactory.Create(config.ItemComparer);

            PropagatePropertyChangeEvent();
        }

        internal TwoDimensionalList(IEnumerable<TRowInternal> values, Config config)
            : this(values.Cast<TRow>(), config)
        {
        }

        internal TwoDimensionalList(IEnumerable<IEnumerable<TItem>> items, Config config)
            : this(
                items.Select(line => config.RowFactoryFromItems(line)),
                config
            )
        {
        }

        internal TwoDimensionalList(int rowLength, int columnLength, Config config)
            : this(
                MakeDefaultItem(rowLength, columnLength, config.RowFactoryFromItems, config.ItemFactory),
                config
            )
        {
        }

        internal TwoDimensionalList(Config config) : this(
            MakeDefaultItem(
                config.MinRowCapacity,
                config.MinColumnCapacity,
                config.RowFactoryFromItems,
                config.ItemFactory
            ),
            config
        )
        {
        }

        private TwoDimensionalList(TwoDimensionalList<TRow, TRowInternal, TItem> src)
            : this(
                src.Select(row => row is IDeepCloneable deepCloneable
                    ? ((TRow)deepCloneable.DeepClone()).ToArray()
                    : row.ToArray()
                ),
                src.config
            )
        {
            NotifyPropertyChangingEventType = src.NotifyPropertyChangingEventType;
            NotifyPropertyChangedEventType = src.NotifyPropertyChangingEventType;
            NotifyCollectionChangingEventType = src.NotifyCollectionChangingEventType;
            NotifyCollectionChangedEventType = src.NotifyCollectionChangedEventType;
        }

        /// <summary>
        /// 各変更通知を自身に伝播させる。
        /// </summary>
        private void PropagatePropertyChangeEvent()
        {
            Items.PropertyChanging += ItemsOnPropertyChanging;
            Items.PropertyChanged += ItemsOnPropertyChanged;
            Items.CollectionChanging += RaiseCollectionChanging;
            Items.CollectionChanged += RaiseCollectionChanged;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Public Methods
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        public int GetMaxRowCapacity() => MaxRowCapacity;

        public int GetMinRowCapacity() => MinRowCapacity;

        public int GetMaxColumnCapacity() => MaxColumnCapacity;

        public int GetMinColumnCapacity() => MinColumnCapacity;

        public IEnumerable<TRow> GetRow(int rowIndex, int rowCount)
        {
            Validator?.GetRow(rowIndex, rowCount);
            return GetRow_Impl(rowIndex, rowCount);
        }

        public IEnumerable<IEnumerable<TItem>> GetColumn(int columnIndex, int columnCount)
        {
            Validator?.GetColumn(columnIndex, columnCount);
            return GetColumn_Impl(columnIndex, columnCount);
        }

        public IEnumerable<IEnumerable<TItem>> GetItem(int rowIndex, int rowCount, int columnIndex, int columnCount)
        {
            Validator?.GetItem(rowIndex, rowCount, columnIndex, columnCount);
            return Get_Impl(rowIndex, rowCount, columnIndex, columnCount, Direction.Row);
        }

        public IEnumerator<TRow> GetEnumerator()
            => Items.GetEnumerator();

        public void SetRow(int rowIndex, params TRow[] items)
        {
            Validator?.SetRow(rowIndex, items);
            var setRows = items.Select(FuncMakeRowFromInType).ToArray();
            SetRow_Impl(rowIndex, setRows);
        }

        public void SetColumn(int columnIndex, params IEnumerable<TItem>[] items)
        {
            Validator?.SetColumn(columnIndex, items);
            SetColumn_Impl(columnIndex, items.ToTwoDimensionalArray());
        }

        public void AddRow(params TRow[] items)
            => InsertRow(RowCount, items);

        public void AddColumn(params IEnumerable<TItem>[] items)
            => InsertColumn(ColumnCount, items);

        public void AddRowPropertyChanging(PropertyChangingEventHandler handler)
        {
            ThrowHelper.ValidateArgumentNotNull(handler is null, nameof(handler));

            rowEventHandlers.AddPropertyChangingEventHandler(handler);
        }

        public void AddRowPropertyChanged(PropertyChangedEventHandler handler)
        {
            ThrowHelper.ValidateArgumentNotNull(handler is null, nameof(handler));

            rowEventHandlers.AddPropertyChangedEventHandler(handler);
        }

        public void AddRowCollectionChanging(EventHandler<NotifyCollectionChangedEventArgs> handler)
        {
            ThrowHelper.ValidateArgumentNotNull(handler is null, nameof(handler));

            rowEventHandlers.AddCollectionChangingEventHandler(handler);
        }

        public void AddRowCollectionChanging(EventHandler<NotifyCollectionChangedEventArgsEx<TItem>> handler)
        {
            ThrowHelper.ValidateArgumentNotNull(handler is null, nameof(handler));

            rowEventHandlers.AddCollectionChangingEventHandler(handler);
        }

        public void AddRowCollectionChanged(NotifyCollectionChangedEventHandler handler)
        {
            ThrowHelper.ValidateArgumentNotNull(handler is null, nameof(handler));

            rowEventHandlers.AddCollectionChangedEventHandler(handler);
        }

        public void AddRowCollectionChanged(EventHandler<NotifyCollectionChangedEventArgsEx<TItem>> handler)
        {
            ThrowHelper.ValidateArgumentNotNull(handler is null, nameof(handler));

            rowEventHandlers.AddCollectionChangedEventHandler(handler);
        }

        public void InsertRow(int rowIndex, params TRow[] items)
        {
            Validator?.InsertRow(rowIndex, items);

            var insertItems = items.Select(FuncMakeRowFromInType).ToArray();
            InsertRow_Impl(rowIndex, insertItems);
        }

        public void InsertColumn(int columnIndex, params IEnumerable<TItem>[] items)
        {
            Validator?.InsertColumn(columnIndex, items);

            InsertColumn_Impl(columnIndex, items.ToTwoDimensionalArray());
        }

        public void OverwriteRow(int rowIndex, params TRow[] items)
        {
            Validator?.OverwriteRow(rowIndex, items);

            OverwriteRow_Impl(rowIndex, items);
        }

        public void OverwriteColumn(int columnIndex, params IEnumerable<TItem>[] items)
        {
            Validator?.OverwriteColumn(columnIndex, items);

            OverwriteColumn_Impl(columnIndex, items);
        }

        public void MoveRow(int oldRowIndex, int newRowIndex, int count = 1)
        {
            Validator?.MoveRow(oldRowIndex, newRowIndex, count);

            MoveRow_Impl(oldRowIndex, newRowIndex, count);
        }

        public void MoveColumn(int oldColumnIndex, int newColumnIndex, int count = 1)
        {
            Validator?.MoveColumn(oldColumnIndex, newColumnIndex, count);

            MoveColumn_Impl(oldColumnIndex, newColumnIndex, count);
        }

        public void RemoveRow(int rowIndex, int count = 1)
        {
            Validator?.RemoveRow(rowIndex, count);

            RemoveRow_Impl(rowIndex, count);
        }

        public void RemoveColumn(int columnIndex, int count = 1)
        {
            Validator?.RemoveColumn(columnIndex, count);

            RemoveColumn_Impl(columnIndex, count);
        }

        public void RemoveRowPropertyChanging(PropertyChangingEventHandler handler)
        {
            ThrowHelper.ValidateArgumentNotNull(handler is null, nameof(handler));

            rowEventHandlers.RemovePropertyChangingEventHandler(handler);
        }

        public void RemoveRowPropertyChanged(PropertyChangedEventHandler handler)
        {
            ThrowHelper.ValidateArgumentNotNull(handler is null, nameof(handler));

            rowEventHandlers.RemovePropertyChangedEventHandler(handler);
        }

        public void RemoveRowCollectionChanging(EventHandler<NotifyCollectionChangedEventArgs> handler)
        {
            ThrowHelper.ValidateArgumentNotNull(handler is null, nameof(handler));

            rowEventHandlers.RemoveCollectionChangingEventHandler(handler);
        }

        public void RemoveRowCollectionChanging(EventHandler<NotifyCollectionChangedEventArgsEx<TItem>> handler)
        {
            ThrowHelper.ValidateArgumentNotNull(handler is null, nameof(handler));

            rowEventHandlers.RemoveCollectionChangingEventHandler(handler);
        }

        public void RemoveRowCollectionChanged(NotifyCollectionChangedEventHandler handler)
        {
            ThrowHelper.ValidateArgumentNotNull(handler is null, nameof(handler));

            rowEventHandlers.RemoveCollectionChangedEventHandler(handler);
        }

        public void RemoveRowCollectionChanged(EventHandler<NotifyCollectionChangedEventArgsEx<TItem>> handler)
        {
            ThrowHelper.ValidateArgumentNotNull(handler is null, nameof(handler));

            rowEventHandlers.RemoveCollectionChangedEventHandler(handler);
        }

        public void AdjustLength(int rowLength, int columnLength)
        {
            Validator?.AdjustLength(rowLength, columnLength);

            AdjustLength_Impl(rowLength, columnLength);
        }

        public void AdjustLengthIfShort(int rowLength, int columnLength)
        {
            Validator?.AdjustLength(rowLength, columnLength);

            var fixedRowLength = Math.Max(rowLength, RowCount);
            var fixedColumnLength = Math.Max(columnLength, ColumnCount);

            AdjustLength_Impl(fixedRowLength, fixedColumnLength);
        }

        public void AdjustLengthIfLong(int rowLength, int columnLength)
        {
            Validator?.AdjustLength(rowLength, columnLength);

            var fixedRowLength = Math.Min(rowLength, RowCount);
            var fixedColumnLength = Math.Min(columnLength, ColumnCount);

            AdjustLength_Impl(fixedRowLength, fixedColumnLength);
        }

        public void AdjustRowLength(int length)
            => AdjustLength(length, ColumnCount);

        public void AdjustRowLengthIfShort(int length)
            => AdjustLengthIfShort(length, ColumnCount);

        public void AdjustRowLengthIfLong(int length)
            => AdjustLengthIfLong(length, ColumnCount);

        public void AdjustColumnLength(int length)
            => AdjustLength(RowCount, length);

        public void AdjustColumnLengthIfShort(int length)
            => AdjustLengthIfShort(RowCount, length);

        public void AdjustColumnLengthIfLong(int length)
            => AdjustLengthIfLong(RowCount, length);

        public void Reset()
        {
            var newItems = MakeDefaultItem(RowCount, ColumnCount, FuncMakeDefaultRowFromItems, FuncMakeDefaultItem);

            Reset_Impl(newItems);
        }

        public void Reset(IEnumerable<TRow> initItems)
        {
            ThrowHelper.ValidateArgumentNotNull(initItems is null, nameof(initItems));
            var initItemArray = initItems.ToArray();

            Validator?.Reset(initItemArray);

            var rows = initItemArray.Select(FuncMakeRowFromInType);

            Reset_Impl(rows);
        }

        public void Clear()
        {
            var newItems = MakeDefaultItem(
                MinRowCapacity,
                MinColumnCapacity,
                FuncMakeDefaultRowFromItems,
                FuncMakeDefaultItem
            );

            Reset_Impl(newItems);
        }

        public override bool ItemEquals(TwoDimensionalList<TRow, TRowInternal, TItem>? other)
            => ItemEquals(other);

        public bool ItemEquals(ITwoDimensionalList<TRow, TRow, TItem, TItem>? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            if (RowCount != other.RowCount || ColumnCount != other.ColumnCount)
            {
                return false;
            }

            var isSameAllRow = this.Zip(other)
                .All(zip =>
                {
                    var (l, r) = zip;
                    return l.ItemEquals(r, itemComparer);
                });
            return isSameAllRow;
        }

        public TItem[][] ToTwoDimensionalArray(bool isTranspose = false)
            => isTranspose
                ? Items.ToTransposedArray()
                : Items.ToTwoDimensionalArray();

        public override TwoDimensionalList<TRow, TRowInternal, TItem>
            DeepClone() => new(this);

        public TwoDimensionalList<TRow, TRowInternal, TItem> DeepCloneWith(
            int? rowLength = null, int? colLength = null,
            IReadOnlyDictionary<(int row, int col), TItem>? values = null)
        {
            var result =
                new TwoDimensionalList<TRow, TRowInternal, TItem>(this);

            switch (rowLength, colLength)
            {
                case (not null, not null):
                    result.AdjustLength(rowLength.Value, colLength.Value);
                    break;
                case (not null, null):
                    result.AdjustRowLength(rowLength.Value);
                    break;

                case (null, not null):
                    result.AdjustColumnLength(colLength.Value);
                    break;

                case (null, null):
                    break;
            }

            var resultRowLength = rowLength ?? RowCount;
            var resultColumnLength = colLength ?? ColumnCount;
            values?.ForEach(pair
                =>
            {
                var (row, col) = pair.Key;
                var value = pair.Value;

                if (-1 < row && row < resultRowLength
                             && -1 < col && col < resultColumnLength)
                {
                    result[row, col] = value;
                }
            });

            return result;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Interface Implementation
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        // @formatter:off
        #region CollectionChanging

        event EventHandler<NotifyCollectionChangedEventArgs> INotifiableCollectionChange.CollectionChanging
        {
            add => ((INotifiableCollectionChange)Items).CollectionChanging += value;
            remove => ((INotifiableCollectionChange)Items).CollectionChanging -= value;
        }

        #endregion

        #region CollectionChanged

        event NotifyCollectionChangedEventHandler INotifyCollectionChanged.CollectionChanged
        {
            add => ((INotifyCollectionChanged)Items).CollectionChanged += value;
            remove => ((INotifyCollectionChanged)Items).CollectionChanged -= value;
        }

        #endregion

        #region GetEnumerator

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion

        #region DeepClone

        ITwoDimensionalList<TRow, TRow, TItem, TItem> IDeepCloneable<ITwoDimensionalList<TRow, TRow, TItem, TItem>>.DeepClone() => DeepClone();

        #endregion

        #region DeepCloneWith

        ITwoDimensionalList<TRow, TRow, TItem, TItem> IDeepCloneableTwoDimensionalListInternal<ITwoDimensionalList<TRow, TRow, TItem, TItem>, TItem>.DeepCloneWith(int? rowLength, int? colLength, IReadOnlyDictionary<(int row, int col), TItem>? values) => DeepCloneWith(rowLength, colLength, values);

        #endregion
        // @formatter:on

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Private Methods
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private static IEnumerable<TRowInternal> MakeDefaultItem(int rowCount, int columnCount,
            Func<IEnumerable<TItem>, TRowInternal> rowFactory, Func<int, int, TItem> itemFactory)
        {
            return Enumerable.Range(0, rowCount)
                .Select(row => rowFactory(
                    Enumerable.Range(0, columnCount).Select(column => itemFactory(row, column))
                ));
        }

        private void ItemsOnPropertyChanging(object sender, PropertyChangingEventArgs e)
        {
            var propName = e.PropertyName;
            var notifyPropertyNames = ConvertInnerListNotifiedPropertyName(propName);

            notifyPropertyNames.ForEach(notifyPropertyChangingAction.Notify);
        }

        private void ItemsOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var propName = e.PropertyName;
            var notifyPropertyNames = ConvertInnerListNotifiedPropertyName(propName);

            notifyPropertyNames.ForEach(notifyPropertyChangedAction.Notify);
        }

        private void RaiseCollectionChanging(object sender, NotifyCollectionChangedEventArgsEx<TRowInternal> args)
        {
            if (collectionChangingEvents.Count == 0)
            {
                return;
            }

            var notifyArgs = NotifyCollectionChangedEventArgsEx<TRow>.CreateFromOtherType(args, row => row);
            collectionChangingEvents.ForEach(handler => handler.Invoke(this, notifyArgs));
        }

        private void RaiseCollectionChanged(object sender, NotifyCollectionChangedEventArgsEx<TRowInternal> args)
        {
            if (collectionChangedEvents.Count == 0)
            {
                return;
            }

            var notifyArgs = NotifyCollectionChangedEventArgsEx<TRow>.CreateFromOtherType(args, row => row);
            collectionChangedEvents.ForEach(handler => handler.Invoke(this, notifyArgs));
        }

        /// <summary>
        /// 行数・列数変化時に変更通知するプロパティ名一覧を算出する。
        /// </summary>
        /// <remarks>
        /// 現在の状態を用いて決定するため、内部状態変更前に実行する必要がある。
        /// </remarks>
        /// <param name="rowLength">変更行数</param>
        /// <param name="columnLength">変更列数</param>
        /// <returns>要通知プロパティ名一覧</returns>
        private List<string> ComputeNotifyPropertyChangeName(int rowLength, int columnLength)
        {
            // 行数・列数が同じなら追加通知なし
            if (rowLength == RowCount && columnLength == ColumnCount)
            {
                return new List<string>();
            }

            var result = new List<string>();

            if (rowLength != RowCount)
            {
                // 行数が異なる場合
                result.Add(nameof(RowCount));
            }

            if (columnLength != ColumnCount)
            {
                // 列数が異なる場合
                result.Add(nameof(ColumnCount));
            }

            // rowLength == RowCount && columnLength == ColumnCount ではないので、総件数は必ず変化する
            result.Add(nameof(AllCount));

            switch (IsEmpty)
            {
                // 現在空で、行数1以上の値が設定される場合は空ではなくなるので IsEmpty の通知が必要
                case true when rowLength > 0:
                // 現在空ではなく、行数0の値が設定される場合は空になるので IsEmpty の通知が必要
                case false when rowLength == 0:
                    result.Add(nameof(IsEmpty));
                    break;
            }

            return result;
        }

        private IEnumerable<TRow> GetRow_Impl(int row, int count)
            => Items.GetRange(row, count);

        private IEnumerable<TItem[]> GetColumn_Impl(int column, int count)
        {
            return Items.Select(row => row.GetRange(column, count))
                .ToTransposedArray();
        }

        private TItem Get_Impl(int row, int column)
            => Items[row][column];

        private IEnumerable<TItem[]> Get_Impl(int row, int rowCount, int column, int columnCount, Direction direction)
        {
            var items = Items.Range(row, rowCount).Select(line => line.Range(column, columnCount));
            return direction != Direction.Column
                ? items.ToTwoDimensionalArray()
                : items.ToTransposedArray();
        }

        private void SetRow_Impl(int row, params TRowInternal[] items)
        {
            /*
             * このメソッドは列操作を行う各種実装メソッド (XXXColumn_Impl) から呼ばれる。
             * そのため パラメータ items 各要素の要素数は ColumnCount とは異なる場合がある。
             */
            if (items.Length == 0) return;

            var setRange = items.Length;
            var oldItems = Items.GetRange(row, setRange);

            var additionalNotifyPropertyNames = ComputeNotifyPropertyChangeName(
                Math.Max(row + items.Length - 1, RowCount),
                items[0].Count
            ).ToArray();

            StartInnerListNotifiedPropertyNameDuplicateCheck();

            additionalNotifyPropertyNames.ForEach(notifyPropertyChangingAction.Notify);

            rowEventHandlers.RemoveEventHandlers(oldItems);
            Items.SetRange(row, items);
            ApplyChangeEventType(items);
            rowEventHandlers.AddEventHandlers(items);

            additionalNotifyPropertyNames.ForEach(notifyPropertyChangedAction.Notify);

            FinishInnerListNotifiedPropertyNameDuplicateCheck();
        }

        private void SetColumn_Impl(int column, params TItem[][] items)
        {
            if (items.Length == 0 || items[0].Length == 0) return;

            var setItems = items.ToTransposedArray();

            var newItems = Items.DeepClone();
            newItems.ForEach((row, r) => row.SetRange(column, setItems[r]));

            SetRow_Impl(0, newItems.ToArray());
        }

        private void SetItem_Impl(int row, int column, TItem item)
        {
            var newItem = Items[row].DeepClone();
            newItem[column] = item;

            SetRow_Impl(row, newItem);
        }

        private void InsertRow_Impl(int index, params TRowInternal[] items)
        {
            if (items.Length == 0) return;

            var additionalNotifyPropertyNames =
                ComputeNotifyPropertyChangeName(items.Length, items[0].Count);

            StartInnerListNotifiedPropertyNameDuplicateCheck();

            additionalNotifyPropertyNames.ForEach(notifyPropertyChangingAction.Notify);

            Items.InsertRange(index, items);
            ApplyChangeEventType(items);
            rowEventHandlers.AddEventHandlers(items);

            additionalNotifyPropertyNames.ForEach(notifyPropertyChangedAction.Notify);

            FinishInnerListNotifiedPropertyNameDuplicateCheck();
        }

        private void InsertColumn_Impl(int index, params TItem[][] items)
        {
            if (items.Length == 0 || items[0].Length == 0) return;

            var insertItems = items.ToTransposedArray();

            if (!IsEmpty)
            {
                var newItems = Items.DeepClone();
                newItems.ForEach((row, r) => row.InsertRange(index, insertItems[r]));

                SetRow_Impl(0, newItems.ToArray());
            }
            else
            {
                // 現在空データの場合、「行データの変更」ではなく「行データの追加」とみなす

                var newItems = insertItems.Select(FuncMakeDefaultRowFromItems);

                InsertRow_Impl(0, newItems.ToArray());
            }
        }

        private void OverwriteRow_Impl(int index, params TRow[] items)
        {
            if (items.Length == 0) return;

            var rowItems = items.Select(row => FuncMakeRowFromInType(row))
                .ToArray();

            OverwriteRow_Impl(index, rowItems);
        }

        private void OverwriteRow_Impl(int index, params TRowInternal[] items)
        {
            if (items.Length == 0) return;

            var rowItems = items.Select(row => FuncMakeRowFromInType(row))
                .ToArray();

            var param = OverwriteParam<TRowInternal>.Factory.Create(Items, index, rowItems);

            var afterRowLength = Math.Max(RowCount, index + items.Length - 1);
            var additionalNotifyPropertyNames =
                ComputeNotifyPropertyChangeName(afterRowLength, items[0].Count);

            StartInnerListNotifiedPropertyNameDuplicateCheck();

            additionalNotifyPropertyNames.ForEach(notifyPropertyChangingAction.Notify);

            rowEventHandlers.RemoveEventHandlers(param.ReplaceOldItems);
            Items.Overwrite(index, rowItems);
            ApplyChangeEventType(param.ReplaceNewItems);
            ApplyChangeEventType(param.InsertItems);
            rowEventHandlers.AddEventHandlers(param.ReplaceNewItems);
            rowEventHandlers.AddEventHandlers(param.InsertItems);

            additionalNotifyPropertyNames.ForEach(notifyPropertyChangedAction.Notify);

            FinishInnerListNotifiedPropertyNameDuplicateCheck();
        }

        private void OverwriteColumn_Impl(int index, params IEnumerable<TItem>[] items)
        {
            if (items.Length == 0 || items[0].ToArray().Length == 0) return;

            var overwriteItems = items.ToTransposedArray();

            if (!IsEmpty)
            {
                var newItems = Items.DeepClone();
                newItems.ForEach((row, r) => row.Overwrite(index, overwriteItems[r]));

                SetRow_Impl(0, newItems.ToArray());
            }
            else
            {
                // 現在空データの場合、「行データの変更」ではなく「行データの追加」とみなす

                var newItems = overwriteItems.Select(FuncMakeDefaultRowFromItems);

                InsertRow_Impl(0, newItems.ToArray());
            }
        }

        private void MoveRow_Impl(int oldIndex, int newIndex, int count = 1)
        {
            if (oldIndex == newIndex) return;
            if (count == 0) return;

            StartInnerListNotifiedPropertyNameDuplicateCheck();

            Items.MoveRange(oldIndex, newIndex, count);

            FinishInnerListNotifiedPropertyNameDuplicateCheck();
        }

        private void MoveColumn_Impl(int oldIndex, int newIndex, int count = 1)
        {
            if (oldIndex == newIndex) return;
            if (count == 0) return;

            var newItems = Items.DeepClone();
            newItems.ForEach(row => row.MoveRange(oldIndex, newIndex, count));

            SetRow_Impl(0, newItems.ToArray());
        }

        private void RemoveRow_Impl(int index, int count = 1)
        {
            if (count == 0) return;

            var removeItems = Items.GetRange(index, count);

            var afterRowLength = RowCount - count;
            var afterColumnLength = afterRowLength == 0 ? 0 : ColumnCount;
            var additionalNotifyPropertyNames =
                ComputeNotifyPropertyChangeName(afterRowLength, afterColumnLength);

            StartInnerListNotifiedPropertyNameDuplicateCheck();

            additionalNotifyPropertyNames.ForEach(notifyPropertyChangingAction.Notify);

            rowEventHandlers.RemoveEventHandlers(removeItems);
            Items.RemoveRange(index, count);

            additionalNotifyPropertyNames.ForEach(notifyPropertyChangedAction.Notify);

            FinishInnerListNotifiedPropertyNameDuplicateCheck();
        }

        private void RemoveColumn_Impl(int index, int count = 1)
        {
            if (count == 0) return;

            var newItems = Items.DeepClone();
            newItems.ForEach(row => row.RemoveRange(index, count));

            SetRow_Impl(0, newItems.ToArray());
        }

        private void AdjustLength_Impl(int rowLength, int columnLength)
        {
            if (rowLength == RowCount && columnLength == ColumnCount)
            {
                return;
            }

            if (columnLength == ColumnCount)
            {
                // 行数のみ変化
                AdjustRowLength_Impl(rowLength);
                return;
            }

            if (rowLength == RowCount)
            {
                // 列数のみ変化
                AdjustColumnLength_Impl(columnLength);
                return;
            }

            // 行数・列数ともに変化

            var newItems = Items.DeepClone();

            newItems.AdjustLength(rowLength);
            newItems.ForEach(row => row.AdjustLength(columnLength));

            Reset_Impl(newItems);
        }

        private void AdjustRowLength_Impl(int rowLength)
        {
            var oldRowCount = RowCount;

            StartInnerListNotifiedPropertyNameDuplicateCheck();

            var additionalNotifyPropertyNames =
                ComputeNotifyPropertyChangeName(rowLength, ColumnCount);

            var addRowLength = rowLength - RowCount;
            if (addRowLength < 0)
            {
                // 削除行のイベントハンドラ解除
                var removeStartIndex = rowLength;
                var removeCount = -addRowLength;
                var removeItems = Items.GetRange(removeStartIndex, removeCount);
                rowEventHandlers.RemoveEventHandlers(removeItems);
            }

            additionalNotifyPropertyNames.ForEach(notifyPropertyChangingAction.Notify);

            // 実行
            Items.AdjustLength(rowLength);

            additionalNotifyPropertyNames.ForEach(notifyPropertyChangedAction.Notify);

            if (addRowLength > 0)
            {
                // 追加行のイベントハンドラ登録
                var addStartIndex = oldRowCount;
                var addCount = addRowLength;
                var addItems = Items.GetRange(addStartIndex, addCount);
                rowEventHandlers.AddEventHandlers(addItems);
            }

            FinishInnerListNotifiedPropertyNameDuplicateCheck();
        }

        private void AdjustColumnLength_Impl(int columnLength)
        {
            if (columnLength == ColumnCount)
            {
                return;
            }

            var setItems = Items.DeepClone();
            setItems.ForEach(row => row.AdjustLength(columnLength));

            SetRow_Impl(0, setItems.ToArray());
        }

        private void Reset_Impl(IEnumerable<TRowInternal> newItems)
        {
            var newItemArray = newItems.ToArray();

            var columnLength = newItemArray.Length > 0 ? newItemArray[0].Count : 0;
            var additionalNotifyPropertyNames =
                ComputeNotifyPropertyChangeName(newItemArray.Length, columnLength);

            StartInnerListNotifiedPropertyNameDuplicateCheck();

            additionalNotifyPropertyNames.ForEach(notifyPropertyChangingAction.Notify);

            rowEventHandlers.RemoveEventHandlers(Items);

            Items.Reset(newItemArray);

            ApplyChangeEventType(newItemArray);
            rowEventHandlers.AddEventHandlers(newItemArray);

            additionalNotifyPropertyNames.ForEach(notifyPropertyChangedAction.Notify);

            FinishInnerListNotifiedPropertyNameDuplicateCheck();
        }

        private void ApplyChangeEventType(IEnumerable<TRowInternal> innerRows)
        {
            innerRows.ForEach(row =>
            {
                row.NotifyPropertyChangingEventType = NotifyPropertyChangingEventType;
                row.NotifyPropertyChangedEventType = NotifyPropertyChangedEventType;
                row.NotifyCollectionChangingEventType = NotifyCollectionChangingEventType;
                row.NotifyCollectionChangedEventType = NotifyCollectionChangedEventType;
            });
        }

        private void ApplyPropertyChangingEventType(IEnumerable<TRowInternal> innerRows)
        {
            innerRows.ForEach(row => { row.NotifyPropertyChangingEventType = NotifyPropertyChangingEventType; });
        }

        private void ApplyPropertyChangedEventType(IEnumerable<TRowInternal> innerRows)
        {
            innerRows.ForEach(row => { row.NotifyPropertyChangedEventType = NotifyPropertyChangedEventType; });
        }

        private void ApplyCollectionChangingEventType(IEnumerable<TRowInternal> innerRows)
        {
            innerRows.ForEach(row => { row.NotifyCollectionChangingEventType = NotifyCollectionChangingEventType; });
        }

        private void ApplyCollectionChangedEventType(IEnumerable<TRowInternal> innerRows)
        {
            innerRows.ForEach(row => { row.NotifyCollectionChangedEventType = NotifyCollectionChangedEventType; });
        }

        private void StartInnerListNotifiedPropertyNameDuplicateCheck()
        {
            notifyPropertyChangingAction.StartCheckDuplicate();
            notifyPropertyChangedAction.StartCheckDuplicate();
        }

        private void FinishInnerListNotifiedPropertyNameDuplicateCheck()
        {
            notifyPropertyChangingAction.FinishCheckDuplicate();
            notifyPropertyChangedAction.FinishCheckDuplicate();
        }

        private IEnumerable<string> ConvertInnerListNotifiedPropertyName(string propertyName)
        {
            // Items の "Count" が変化した場合、行数と総数の変更通知に変換する
            if (propertyName.Equals(nameof(ExtendedList<TItem>.Count)))
            {
                return new[]
                {
                    nameof(RowCount),
                    nameof(AllCount)
                };
            }

            // それ以外はそのままの名称で通知
            return new[] { propertyName };
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private class InnerListNotifyPropertyChangeAction
        {
            private bool isChecking = false;
            private readonly List<string> notifiedPropertyNames;

            private readonly Action<string> actionNotifyPropertyChange;

            public InnerListNotifyPropertyChangeAction(Action<string> actionNotifyPropertyChange)
            {
                this.actionNotifyPropertyChange = actionNotifyPropertyChange;
                notifiedPropertyNames = new List<string>();
            }

            public void Notify(string propertyName)
            {
                if (!isChecking)
                {
                    actionNotifyPropertyChange(propertyName);
                    return;
                }

                // 重複しないものだけを通知する
                if (notifiedPropertyNames.Contains(propertyName)) return;

                notifiedPropertyNames.Add(propertyName);
                actionNotifyPropertyChange(propertyName);
            }

            public void StartCheckDuplicate()
            {
                isChecking = true;
            }

            public void FinishCheckDuplicate()
            {
                isChecking = false;
                notifiedPropertyNames.Clear();
            }
        }
    }
}
