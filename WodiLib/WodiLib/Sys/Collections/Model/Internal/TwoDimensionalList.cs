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
    /// <typeparam name="TItem">リスト要素型</typeparam>
    internal partial class TwoDimensionalList<TRow, TItem>
        : ModelBase<TwoDimensionalList<TRow, TItem>>,
            ITwoDimensionalList<TRow, TItem>
        where TRow : IExtendedList<TItem>
    {
        /*
         * このクラスの実装観点は ExtendedList<T> と同じ。
         * ExtendedList<T> とは違いこのクラス自身が二次元リストの実装となり、
         * イベント通知なども行う。
         */
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Events
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add => Items.CollectionChanged += value;
            remove => Items.CollectionChanged -= value;
        }

        public void AddRowPropertyChanged(PropertyChangedEventHandler handler)
        {
            ThrowHelper.ValidateArgumentNotNull(handler is null, nameof(handler));

            rowEventHandlers.AddPropertyChangedEventHandler(handler);
        }

        public void RemoveRowPropertyChanged(PropertyChangedEventHandler handler)
        {
            ThrowHelper.ValidateArgumentNotNull(handler is null, nameof(handler));

            rowEventHandlers.RemovePropertyChangedEventHandler(handler);
        }

        public void AddRowCollectionChanged(NotifyCollectionChangedEventHandler handler)
        {
            ThrowHelper.ValidateArgumentNotNull(handler is null, nameof(handler));

            rowEventHandlers.AddCollectionChangedEventHandler(handler);
        }

        public void RemoveRowCollectionChanged(NotifyCollectionChangedEventHandler handler)
        {
            ThrowHelper.ValidateArgumentNotNull(handler is null, nameof(handler));

            rowEventHandlers.RemoveCollectionChangedEventHandler(handler);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Public Properties
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        public TRow this[int rowIndex]
        {
            get => this.GetRow(rowIndex);
            set => this.SetRow(rowIndex, value);
        }

        public TItem this[int rowIndex, int columnIndex]
        {
            get => this.GetItem(rowIndex, 1, columnIndex, 1).First().First();
            set => this.SetItem(rowIndex, columnIndex, value);
        }

        public bool IsEmpty => RowCount == 0;
        public int RowCount => Items.Count;

        public int ColumnCount => RowCount > 0
            ? Items[0].Count
            : 0;

        public int AllCount => RowCount * ColumnCount;

        public ITwoDimensionalListValidator<TRow, TItem> Validator { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Protected Properties
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private protected virtual IExtendedList<TRow> Items { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Private Properties
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private Func<IEnumerable<TItem>, TRow> FuncMakeDefaultRowFromItems => config.RowFactoryFromItems;
        private Func<int, int, TItem> FuncMakeDefaultItem => config.ItemFactory;

        private int MaxRowCapacity => config.MaxRowCapacity;
        private int MinRowCapacity => config.MinRowCapacity;
        private int MaxColumnCapacity => config.MaxColumnCapacity;
        private int MinColumnCapacity => config.MinColumnCapacity;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Fields
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private readonly Config config;

        private readonly RowEventHandlers rowEventHandlers;

        private readonly InnerListNotifyPropertyChangeActionReserver notifyPropertyChangedActionReserver;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Constructors
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        internal TwoDimensionalList(IEnumerable<TRow> values, Config config)
        {
            this.config = config;
            Validator = config.ValidatorFactory(this);

            var initItems = values.ToArray();

            Validator.Constructor((nameof(values), initItems));

            AddNotifyInnerItemPropertyChanged(initItems);
            Items = new ExtendedList<TRow>(
                makeListDefaultItem: rowIndex => FuncMakeDefaultRowFromItems(
                    ColumnCount.Iterate(columnIndex => FuncMakeDefaultItem(rowIndex, columnIndex))
                ),
                validator: default!, // 利用しないため
                initItems
            );
            notifyPropertyChangedActionReserver =
                new InnerListNotifyPropertyChangeActionReserver(NotifyPropertyChanged);
            rowEventHandlers = new RowEventHandlers(this);

            PropagatePropertyChangeEvent();
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
                rowLength.Iterate(
                    rowIndex => columnLength.Iterate(columnIndex => config.ItemFactory(rowIndex, columnIndex))
                ),
                config
            )
        {
        }

        internal TwoDimensionalList(Config config) : this(
            config.MinRowCapacity.Iterate(
                rowIndex => config.MinColumnCapacity.Iterate(columnIndex => config.ItemFactory(rowIndex, columnIndex))
            ),
            config
        )
        {
        }

        private TwoDimensionalList(TwoDimensionalList<TRow, TItem> src) : this(src, src.config)
        {
        }

        /// <summary>
        /// 各変更通知を自身に伝播させる。
        /// </summary>
        private void PropagatePropertyChangeEvent()
        {
            Items.PropertyChanged += ItemsOnPropertyChanged;
            Items.CollectionChanged += ItemsOnCollectionChanged;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Public Methods
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        public int GetMaxRowCapacity() => MaxRowCapacity;

        public int GetMinRowCapacity() => MinRowCapacity;

        public int GetMaxColumnCapacity() => MaxColumnCapacity;

        public int GetMinColumnCapacity() => MinColumnCapacity;

        public IEnumerable<TRow> GetRowRangeCore(int rowIndex, int rowCount)
            => Items.GetRangeCore(rowIndex, rowCount);

        public IEnumerable<IEnumerable<TItem>> GetColumnRangeCore(int columnIndex, int columnCount)
            => Items.Select(row => row.GetRangeCore(columnIndex, columnCount))
                .ToTransposedArray();

        public IEnumerable<IEnumerable<TItem>> GetItemCore(int rowIndex, int rowCount, int columnIndex, int columnCount)
            => Items.GetRange(rowIndex, rowCount).Select(row => row.GetRangeCore(columnIndex, columnCount));

        public void SetRowRangeCore(int rowIndex, IEnumerable<TRow> items)
        {
            StartReserveInnerListNotifiedPropertyChangedEvents();

            var itemsArray = items.ToArray();
            var beforeItems = Items.GetRangeCore(rowIndex, itemsArray.Length);
            RemoveEventsForRemovalRows(beforeItems);

            Items.SetRangeCore(rowIndex, itemsArray);
            AddEventsForNewRows(itemsArray);

            ReleaseReservedInnerListNotifiedPropertyChangedEvents();
        }

        public void SetColumnRangeCore(int columnIndex, IEnumerable<IEnumerable<TItem>> items)
        {
            StartReserveInnerListNotifiedPropertyChangedEvents();

            var transposedItems = items.ToTransposedArray();
            Items.Zip(transposedItems)
                .ForEach(
                    zip =>
                    {
                        var (row, setItems) = zip;
                        row.SetRangeCore(columnIndex, setItems);
                    }
                );

            ReleaseReservedInnerListNotifiedPropertyChangedEvents();
        }

        public void SetItemCore(int rowIndex, int columnIndex, TItem item)
        {
            StartReserveInnerListNotifiedPropertyChangedEvents();

            var row = Items.GetCore(rowIndex);
            row.SetCore(columnIndex, item);

            ReleaseReservedInnerListNotifiedPropertyChangedEvents();
        }

        public void AddRowRangeCore(IEnumerable<TRow> items)
        {
            StartReserveInnerListNotifiedPropertyChangedEvents();

            var itemsArray = items.ToArray();
            Items.AddRangeCore(itemsArray);
            AddEventsForNewRows(itemsArray);

            ReleaseReservedInnerListNotifiedPropertyChangedEvents();
        }

        public void AddColumnRangeCore(IEnumerable<IEnumerable<TItem>> items)
        {
            StartReserveInnerListNotifiedPropertyChangedEvents();

            var transposedItems = items.ToTransposedArray();
            Items.Zip(transposedItems)
                .ForEach(
                    zip =>
                    {
                        var (row, setItems) = zip;
                        row.AddRangeCore(setItems);
                    }
                );

            ReleaseReservedInnerListNotifiedPropertyChangedEvents();
        }

        public void InsertRowRangeCore(int rowIndex, IEnumerable<TRow> items)
        {
            StartReserveInnerListNotifiedPropertyChangedEvents();

            var itemsArray = items.ToArray();
            Items.InsertRangeCore(rowIndex, itemsArray);
            AddEventsForNewRows(itemsArray);

            ReleaseReservedInnerListNotifiedPropertyChangedEvents();
        }

        public void InsertColumnRangeCore(int columnIndex, IEnumerable<IEnumerable<TItem>> items)
        {
            StartReserveInnerListNotifiedPropertyChangedEvents();

            var transposedItems = items.ToTransposedArray();
            Items.Zip(transposedItems)
                .ForEach(
                    zip =>
                    {
                        var (row, setItems) = zip;
                        row.InsertRangeCore(columnIndex, setItems);
                    }
                );

            ReleaseReservedInnerListNotifiedPropertyChangedEvents();
        }

        public void OverwriteRowCore(int rowIndex, IEnumerable<TRow> items)
        {
            StartReserveInnerListNotifiedPropertyChangedEvents();

            var itemsArray = items.ToArray();
            var overwriteParams = OverwriteParam<TRow>.Factory.Create(Items, rowIndex, itemsArray);

            RemoveEventsForRemovalRows(overwriteParams.ReplaceOldItems);
            Items.OverwriteCore(rowIndex, itemsArray);
            AddEventsForNewRows(overwriteParams.ReplaceNewItems);
            AddEventsForNewRows(overwriteParams.InsertItems);

            ReleaseReservedInnerListNotifiedPropertyChangedEvents();
        }

        public void OverwriteColumnCore(int columnIndex, IEnumerable<IEnumerable<TItem>> items)
        {
            StartReserveInnerListNotifiedPropertyChangedEvents();

            var transposedItems = items.ToTransposedArray();
            Items.Zip(transposedItems)
                .ForEach(
                    zip =>
                    {
                        var (row, overwriteItems) = zip;
                        row.OverwriteCore(columnIndex, overwriteItems);
                    }
                );

            ReleaseReservedInnerListNotifiedPropertyChangedEvents();
        }

        public void MoveRowRangeCore(int oldRowIndex, int newRowIndex, int count)
        {
            StartReserveInnerListNotifiedPropertyChangedEvents();

            Items.MoveRangeCore(oldRowIndex, newRowIndex, count);

            ReleaseReservedInnerListNotifiedPropertyChangedEvents();
        }

        public void MoveColumnRangeCore(int oldColumnIndex, int newColumnIndex, int count)
        {
            StartReserveInnerListNotifiedPropertyChangedEvents();

            Items.ForEach(
                row => { row.MoveRangeCore(oldColumnIndex, newColumnIndex, count); }
            );

            ReleaseReservedInnerListNotifiedPropertyChangedEvents();
        }

        public void RemoveRowRangeCore(int rowIndex, int count)
        {
            StartReserveInnerListNotifiedPropertyChangedEvents();

            Items.RemoveRangeCore(rowIndex, count);

            ReleaseReservedInnerListNotifiedPropertyChangedEvents();
        }

        public void RemoveColumnRangeCore(int columnIndex, int count)
        {
            StartReserveInnerListNotifiedPropertyChangedEvents();

            Items.ForEach(
                row => { row.RemoveRangeCore(columnIndex, count); }
            );

            ReleaseReservedInnerListNotifiedPropertyChangedEvents();
        }

        public void AdjustLengthCore(int rowLength, int columnLength)
        {
            StartReserveInnerListNotifiedPropertyChangedEvents();

            Items.Take(rowLength)
                .ForEach(
                    row => { row.AdjustLengthCore(columnLength); }
                );
            Items.AdjustLengthCore(rowLength);

            ReleaseReservedInnerListNotifiedPropertyChangedEvents();
        }

        public void ResetCore(IEnumerable<TRow> rows)
        {
            StartReserveInnerListNotifiedPropertyChangedEvents();

            var rowArray = rows.ToArray();
            var columnLength = rowArray.Length > 0
                ? rowArray[0].Count
                : 0;
            var isChangedColumnLength = columnLength != ColumnCount;

            Items.ResetCore(rowArray);

            if (isChangedColumnLength)
            {
                notifyPropertyChangedActionReserver.Notify(nameof(ColumnCount));
            }

            ReleaseReservedInnerListNotifiedPropertyChangedEvents();
        }

        public void ClearCore()
        {
            StartReserveInnerListNotifiedPropertyChangedEvents();

            var isChangedColumnLength = config.MinRowCapacity != ColumnCount;

            var newItems =
                config.MinRowCapacity.Iterate(r => config.MinColumnCapacity.Iterate(c => config.ItemFactory(r, c)));
            var newRows = newItems.Select(newRow => config.RowFactoryFromItems(newRow)).ToArray();

            RemoveEventsForRemovalRows(Items);
            Items.ResetCore(newRows);
            AddEventsForNewRows(newRows);

            if (isChangedColumnLength)
            {
                notifyPropertyChangedActionReserver.Notify(nameof(ColumnCount));
            }

            ReleaseReservedInnerListNotifiedPropertyChangedEvents();
        }

        public IEnumerator<TRow> GetEnumerator()
            => Items.GetEnumerator();

        public override bool ItemEquals(TwoDimensionalList<TRow, TItem>? other)
            => ItemEquals(other);

        public bool ItemEquals(ITwoDimensionalList<TRow, TItem>? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (RowCount != other.RowCount || ColumnCount != other.ColumnCount) return false;
            return Items.Zip(other)
                .All(
                    zip =>
                    {
                        var (thisRow, otherRow) = zip;
                        return thisRow.ItemEquals(otherRow);
                    }
                );
        }

        public override bool ItemEquals(object? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            switch (other)
            {
                case IEnumerable<TRow> enumerable:
                {
                    var otherArray = enumerable.ToArray();
                    return RowCount == otherArray.Length
                           && this.Zip(otherArray).All(zip => zip.Item1.ItemEquals(zip.Item2));
                }
                case IEnumerable<IEnumerable<TItem>> enumerable2:
                {
                    var otherTwoDimArray = enumerable2.ToTwoDimensionalArray();
                    return RowCount == otherTwoDimArray.Length
                           && this.Zip(otherTwoDimArray)
                               .All(
                                   zip => zip.Item1.ItemEquals(zip.Item2)
                               );
                }
                default:
                    return Equals(other);
            }
        }

        public TItem[][] ToTwoDimensionalArray(bool isTranspose = false)
            => isTranspose
                ? Items.Select(row => row.AsEnumerable()).ToTransposedArray()
                : Items.Select(row => row.AsEnumerable()).ToTwoDimensionalArray();

        public override TwoDimensionalList<TRow, TItem> DeepClone() => new(this);

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Interface Implementation
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        // @formatter:off
        #region GetEnumerator

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion

        #region DeepClone

        ITwoDimensionalList<TRow, TItem> IDeepCloneable<ITwoDimensionalList<TRow, TItem>>.DeepClone() => DeepClone();

        #endregion
        // @formatter:on

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Private Static Methods
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private static IEnumerable<string> ConvertInnerListNotifiedPropertyName(string propertyName)
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

            // それ以外は要素の更新として通知
            return new[] { ListConstant.IndexerName };
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Private Methods
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 行要素のプロパティ変更通知を自身のプロパティ変更通知として通知する。
        /// </summary>
        /// <param name="sender">発火元</param>
        /// <param name="e">通知イベント引数</param>
        private void ItemsOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var propName = e.PropertyName;
            var notifyPropertyNames = ConvertInnerListNotifiedPropertyName(propName);

            notifyPropertyNames.ForEach(notifyPropertyChangedActionReserver.Notify);
        }

        /// <summary>
        /// 行要素のコレクション変更通知を自身のプロパティ変更通知として通知する。
        /// </summary>
        /// <param name="sender">発火元</param>
        /// <param name="args">通知イベント引数</param>
        private void ItemsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            notifyPropertyChangedActionReserver.Notify(ListConstant.IndexerName);
        }

        private void AddEventsForNewRows(IEnumerable<TRow> addRows)
        {
            var addRowArray = addRows.ToArray();
            rowEventHandlers.AddEventHandlers(addRowArray);
            AddNotifyInnerItemPropertyChanged(addRowArray);
        }

        private void RemoveEventsForRemovalRows(IEnumerable<TRow> removeRows)
        {
            var removeRowArray = removeRows.ToArray();
            rowEventHandlers.RemoveEventHandlers(removeRowArray);
            RemoveNotifyInnerItemPropertyChanged(removeRowArray);
        }

        private void AddNotifyInnerItemPropertyChanged(IEnumerable<TRow> rows)
        {
            rows.ForEach(
                row => { row.PropertyChanged += NotifyInnerItemPropertyChanged; }
            );
        }

        private void RemoveNotifyInnerItemPropertyChanged(IEnumerable<TRow> rows)
        {
            rows.ForEach(
                row => { row.PropertyChanged -= NotifyInnerItemPropertyChanged; }
            );
        }

        private void StartReserveInnerListNotifiedPropertyChangedEvents()
        {
            notifyPropertyChangedActionReserver.StartReserve();
        }

        private void ReleaseReservedInnerListNotifiedPropertyChangedEvents()
        {
            notifyPropertyChangedActionReserver.Release();
            notifyPropertyChangedActionReserver.FinishReserve();
        }

        private void NotifyInnerItemPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName.Equals(nameof(IExtendedList<TItem>.Count)))
            {
                notifyPropertyChangedActionReserver.Notify(nameof(ColumnCount));
                notifyPropertyChangedActionReserver.Notify(nameof(AllCount));
            }
            else
            {
                notifyPropertyChangedActionReserver.Notify(ListConstant.IndexerName);
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Class
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private class InnerListNotifyPropertyChangeActionReserver
        {
            private bool reserving = false;
            private readonly List<string> notifiedPropertyNames;

            private readonly Action<string> actionNotifyPropertyChange;

            public InnerListNotifyPropertyChangeActionReserver(Action<string> actionNotifyPropertyChange)
            {
                this.actionNotifyPropertyChange = actionNotifyPropertyChange;
                notifiedPropertyNames = new List<string>();
            }

            public void Notify(string propertyName)
            {
                if (!reserving)
                {
                    actionNotifyPropertyChange(propertyName);
                    return;
                }

                // 重複しないものだけを溜め込んでおく
                if (notifiedPropertyNames.Contains(propertyName)) return;

                notifiedPropertyNames.Add(propertyName);
            }

            public void StartReserve()
            {
                reserving = true;
            }

            public void Release()
            {
                notifiedPropertyNames.ForEach(actionNotifyPropertyChange);
                notifiedPropertyNames.Clear();
            }

            public void FinishReserve()
            {
                reserving = false;
            }
        }
    }
}
