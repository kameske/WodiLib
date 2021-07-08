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
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using WodiLib.Sys.Cmn;

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
    internal partial class TwoDimensionalList<T> : ModelBase<TwoDimensionalList<T>>,
        ITwoDimensionalList<T>, IDeepCloneableTwoDimensionalListInternal<TwoDimensionalList<T>, T>
        where T : IEqualityComparable<T>
    {
        /*
         * このクラスの実装観点は ExtendedList<T> と同じ。
         * ExtendedList<T> とは違いこのクラス自身が二次元リストの実装となり、
         * イベント通知なども行う。
         */
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Public Delegate
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     各操作の検証処理実施クラスを注入する。
        /// </summary>
        /// <param name="self">自分自身</param>
        public delegate ITwoDimensionalListValidator<T>? InjectValidator(TwoDimensionalList<T> self);

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Events
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        public event NotifyCollectionChangedEventHandler CollectionChanging
        {
            add
            {
                if (collectionChanging != null
                    && collectionChanging.GetInvocationList().Contains(value)) return;
                collectionChanging += value;
            }
            remove => collectionChanging -= value;
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add
            {
                if (collectionChanged != null
                    && collectionChanged.GetInvocationList().Contains(value)) return;
                collectionChanged += value;
            }
            remove => collectionChanged -= value;
        }

        public event EventHandler<TwoDimensionalCollectionChangeEventInternalArgs<T>> TwoDimensionalListChanging
        {
            add
            {
                if (twoDimensionalListChanging != null
                    && twoDimensionalListChanging.GetInvocationList().Contains(value)) return;
                twoDimensionalListChanging += value;
            }
            remove => twoDimensionalListChanging -= value;
        }

        public event EventHandler<TwoDimensionalCollectionChangeEventInternalArgs<T>> TwoDimensionalListChanged
        {
            add
            {
                if (twoDimensionalListChanged != null
                    && twoDimensionalListChanged.GetInvocationList().Contains(value)) return;
                twoDimensionalListChanged += value;
            }
            remove => twoDimensionalListChanged -= value;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Public Properties
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        public IEnumerable<T> this[int index]
        {
            get => GetRange(index, 1, 0, ColumnCount, Direction.Row).First();
            set => SetRange(index, 0, new[] {value}, Direction.Row, true);
        }

        public T this[int row, int column]
        {
            get => GetRange(row, 1, column, 1, Direction.Row).First().First();
            set => SetRange(row, column, new[] {new[] {value}}, Direction.None, false);
        }

        public bool IsEmpty => RowCount == 0;
        public int RowCount => Items.Count;
        public int ColumnCount => RowCount > 0 ? Get_Impl(0, 1)[0].Count : 0;
        public int AllCount => RowCount * ColumnCount;

        public NotifyCollectionChangeEventType NotifyCollectionChangingEventType
        {
            get => notifyCollectionChangingEventType;
            set
            {
                ThrowHelper.ValidatePropertyNotNull(value is null, nameof(NotifyCollectionChangingEventType));
                notifyCollectionChangingEventType = value;
            }
        }

        public NotifyCollectionChangeEventType NotifyCollectionChangedEventType
        {
            get => notifyCollectionChangedEventType;
            set
            {
                ThrowHelper.ValidatePropertyNotNull(value is null, nameof(NotifyCollectionChangedEventType));
                notifyCollectionChangedEventType = value;
            }
        }

        public NotifyTwoDimensionalListChangeEventType NotifyTwoDimensionalListChangingEventType
        {
            get => notifyTwoDimensionalListChangingEventType;
            set
            {
                ThrowHelper.ValidatePropertyNotNull(value is null, nameof(NotifyTwoDimensionalListChangingEventType));
                notifyTwoDimensionalListChangingEventType = value;
            }
        }

        public NotifyTwoDimensionalListChangeEventType NotifyTwoDimensionalListChangedEventType
        {
            get => notifyTwoDimensionalListChangedEventType;
            set
            {
                ThrowHelper.ValidatePropertyNotNull(value is null, nameof(NotifyTwoDimensionalListChangedEventType));
                notifyTwoDimensionalListChangedEventType = value;
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Private Properties
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private SimpleList<Inner> Items { get; }

        private Inner.Factory InnerItemFactory { get; }

        private Func<int, int, T> FuncMakeDefaultItem { get; }

        private ITwoDimensionalListValidator<T>? Validator { get; }

        private int MaxRowCapacity { get; }
        private int MinRowCapacity { get; }
        private int MaxColumnCapacity { get; }
        private int MinColumnCapacity { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Fields
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /* マルチスレッドを考慮して、イベントハンドラ本体の実装は自動実装に任せる。 */

        private event NotifyCollectionChangedEventHandler? collectionChanging;
        private event NotifyCollectionChangedEventHandler? collectionChanged;

        private event EventHandler<TwoDimensionalCollectionChangeEventInternalArgs<T>>? twoDimensionalListChanging;
        private event EventHandler<TwoDimensionalCollectionChangeEventInternalArgs<T>>? twoDimensionalListChanged;

        private NotifyCollectionChangeEventType notifyCollectionChangingEventType;
        private NotifyCollectionChangeEventType notifyCollectionChangedEventType;
        private NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangingEventType;
        private NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangedEventType;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Constructors
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        internal TwoDimensionalList(IEnumerable<IEnumerable<T>> values,
            InjectValidator? injection, Func<int, int, T> funcMakeDefaultItem,
            int minRowCapacity = 0, int maxRowCapacity = int.MaxValue,
            int minColumnCapacity = 0, int maxColumnCapacity = int.MaxValue)
        {
            ThrowHelper.ValidateArgumentNotNull(values is null, nameof(values));
            ThrowHelper.ValidateArgumentNotNull(funcMakeDefaultItem is null, nameof(funcMakeDefaultItem));

            FuncMakeDefaultItem = funcMakeDefaultItem;
            InnerItemFactory = new Inner.Factory(MakeInnerDefaultItem);
            MaxRowCapacity = maxRowCapacity;
            MinRowCapacity = minRowCapacity;
            MaxColumnCapacity = maxColumnCapacity;
            MinColumnCapacity = minColumnCapacity;

            var valuesArray = values.ToTwoDimensionalArray();

            Validator = injection?.Invoke(this);

            Validator?.Constructor(valuesArray);

            var initItems = valuesArray.Select(line => InnerItemFactory.Create(line));
            Items = new SimpleList<Inner>(initItems);

            notifyCollectionChangingEventType = WodiLibConfig.GetDefaultNotifyBeforeCollectionChangeEventType();
            notifyCollectionChangedEventType = WodiLibConfig.GetDefaultNotifyAfterCollectionChangeEventType();
            notifyTwoDimensionalListChangingEventType =
                WodiLibConfig.GetDefaultNotifyBeforeTwoDimensionalListChangeEventType();
            notifyTwoDimensionalListChangedEventType =
                WodiLibConfig.GetDefaultNotifyAfterTwoDimensionalListChangeEventType();
        }

        internal TwoDimensionalList(int rowLength, int columnLength,
            InjectValidator? injection,
            Func<int, int, T> funcMakeDefaultItem,
            int minRowCapacity = 0, int maxRowCapacity = int.MaxValue,
            int minColumnCapacity = 0, int maxColumnCapacity = int.MaxValue) : this(
            ((Func<IEnumerable<IEnumerable<T>>>) (() =>
                Enumerable.Range(0, rowLength).Select(rowIdx
                    => Enumerable.Range(0, columnLength).Select(colIdx
                        => funcMakeDefaultItem(rowIdx, colIdx)))))(),
            injection, funcMakeDefaultItem,
            minRowCapacity, maxRowCapacity,
            minColumnCapacity, maxColumnCapacity)
        {
        }

        internal TwoDimensionalList(InjectValidator? injection,
            Func<int, int, T> funcMakeDefaultItem,
            int minRowCapacity = 0, int maxRowCapacity = int.MaxValue,
            int minColumnCapacity = 0, int maxColumnCapacity = int.MaxValue) : this(
            Enumerable.Range(0, minRowCapacity).Select(row =>
                Enumerable.Range(0, minColumnCapacity).Select(col => funcMakeDefaultItem(row, col))),
            injection, funcMakeDefaultItem, minRowCapacity, maxRowCapacity, minColumnCapacity, maxColumnCapacity)
        {
        }

        private TwoDimensionalList(TwoDimensionalList<T> src) : this(src.Items.Select(line => line.DeepClone()),
            self => src.Validator?.CreateAnotherFor(self), src.FuncMakeDefaultItem,
            src.GetMinRowCapacity(), src.GetMaxRowCapacity(), src.GetMinColumnCapacity(), src.GetMaxColumnCapacity())
        {
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Public Methods
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        public bool Contains([AllowNull] T item, IEqualityComparer<T>? comparer = null,
            bool shouldScanRowDirection = false)
        {
            if (item is null) return false;

            return shouldScanRowDirection
                ? Items.ToTransposedArray().Any(line => line.Contains(item, comparer))
                : Items.Any(line => line.Contains(item, comparer));
        }

        public bool ContainsRow(IEnumerable<T>? item, IEqualityComparer<T>? comparer = null)
        {
            if (item is null) return false;

            return Items.Any(items => items.SequenceEqual(item, comparer));
        }

        public bool ContainsColumn(IEnumerable<T>? item, IEqualityComparer<T>? comparer = null)
        {
            if (item is null) return false;

            return Items.ToTransposedArray()
                .Any(items => items.SequenceEqual(item, comparer));
        }

        public IEnumerator<IEnumerable<T>> GetEnumerator()
            => Items.GetEnumerator();

        public int GetMaxRowCapacity() => MaxRowCapacity;

        public int GetMinRowCapacity() => MinRowCapacity;

        public int GetMaxColumnCapacity() => MaxColumnCapacity;

        public int GetMinColumnCapacity() => MinColumnCapacity;

        public IEnumerable<IEnumerable<T>> GetRowRange(int row, int count)
            => GetRange(row, count, 0, ColumnCount, Direction.Row);

        public IEnumerable<IEnumerable<T>> GetRowRange(int row, int rowCount, int column, int columnCount)
            => GetRange(row, rowCount, column, columnCount, Direction.Row);

        public IEnumerable<T> GetColumn(int column)
            => GetColumnRange(column, 1).First();

        public IEnumerable<IEnumerable<T>> GetColumnRange(int column, int count)
            => GetRange(0, RowCount, column, count, Direction.Column);

        public IEnumerable<IEnumerable<T>> GetColumnRange(int column, int columnCount, int row, int rowCount)
            => GetRange(row, rowCount, column, columnCount, Direction.Column);

        public (int row, int column) IndexOf([AllowNull] T item, IEqualityComparer<T>? comparer = null,
            bool shouldScanRowDirection = false)
        {
            if (item is null)
            {
                return (-1, -1);
            }

            var transposedIfNeed =
                shouldScanRowDirection
                    ? (IEnumerable<IEnumerable<T>>) Items.ToTransposedArray()
                    : Items;
            var search = transposedIfNeed.Select((inner, row) => (row, inner.FindIndex(item, comparer)))
                .Where(cell => cell.Item2 >= 0)
                .ToArray();

            if (search.Length == 0)
            {
                return (-1, -1);
            }

            var (first, second) = search[0];
            return shouldScanRowDirection
                ? (second, first)
                : (first, second);
        }

        public int RowIndexOf(IEnumerable<T>? item, IEqualityComparer<T>? comparer = null)
        {
            if (item is null)
            {
                return -1;
            }

            return Items.FindIndex(row => row.ItemEquals(item, comparer));
        }

        public int ColumnIndexOf(IEnumerable<T>? item, IEqualityComparer<T>? comparer = null)
        {
            if (item is null)
            {
                return -1;
            }

            return Items.ToTransposedArray()
                .FindIndex(column => column.SequenceEqual(item, comparer));
        }

        public void CopyTo(T[] array, int index, bool shouldTakeRowDirection = false)
        {
            var direction = shouldTakeRowDirection ? Direction.Row : Direction.Column;

            Validator?.CopyTo(array, index, direction);

            (shouldTakeRowDirection
                    ? Items.ToTransposedArray().SelectMany(inner => inner).ToArray()
                    : Items.SelectMany(inner => inner).ToArray()
                ).ForEach((item, idx) => array[index + idx] = item);
        }

        public void CopyTo(T[,] array, int row, int column)
        {
            Validator?.CopyTo(array, row, column);
            Items.Select((inner, i) => (inner, i))
                .ForEach(line => line.inner.ForEach((item, j)
                    => array[row + line.i, column + j] = item));
        }

        public void CopyTo(T[][] array, int row, int column)
        {
            Validator?.CopyTo(array, row, column);
            Items.ForEach((inner, i) => inner.CopyTo(array[i + row], column));
        }

        public void SetRowRange(int row, IEnumerable<IEnumerable<T>> items)
            => SetRange(row, 0, items, Direction.Row, true);

        public void SetColumn(int column, IEnumerable<T> items)
            => SetRange(0, column, new[] {items}, Direction.Column, true);

        public void SetColumnRange(int column, IEnumerable<IEnumerable<T>> items)
            => SetRange(0, column, items, Direction.Column, true);

        public void AddRow(IEnumerable<T> item)
            => AddRowRange(new[] {item});

        public void AddRowRange(IEnumerable<IEnumerable<T>> items)
        {
            ThrowHelper.ValidateNotNull(items is null);
            var itemArray = items.ToTwoDimensionalArray();

            Validator?.Insert(RowCount, itemArray, Direction.Row);
            Insert_Impl(RowCount, itemArray, Direction.Row);
        }

        public void AddColumn(IEnumerable<T> columnItems)
            => AddColumnRange(new[] {columnItems});

        public void AddColumnRange(IEnumerable<IEnumerable<T>> items)
        {
            ThrowHelper.ValidateNotNull(items is null);
            var itemArray = items.ToTwoDimensionalArray();

            Validator?.Insert(ColumnCount, itemArray, Direction.Column);
            Insert_Impl(ColumnCount, itemArray, Direction.Column);
        }

        public void InsertRow(int row, IEnumerable<T> items)
            => InsertRowRange(row, new[] {items});

        public void InsertRowRange(int row, IEnumerable<IEnumerable<T>> items)
        {
            ThrowHelper.ValidateNotNull(items is null);
            var itemArray = items.ToTwoDimensionalArray();

            Validator?.Insert(row, itemArray, Direction.Row);
            Insert_Impl(row, itemArray, Direction.Row);
        }

        public void InsertColumn(int column, IEnumerable<T> columnItems)
            => InsertColumnRange(column, new[] {columnItems});

        public void InsertColumnRange(int column, IEnumerable<IEnumerable<T>> items)
        {
            ThrowHelper.ValidateNotNull(items is null);
            var itemArray = items.ToTwoDimensionalArray();

            Validator?.Insert(column, itemArray, Direction.Column);
            Insert_Impl(column, itemArray, Direction.Column);
        }

        public void OverwriteRow(int row, IEnumerable<IEnumerable<T>> items)
        {
            ThrowHelper.ValidateNotNull(items is null);
            var itemArray = items.ToTwoDimensionalArray();

            Validator?.Overwrite(row, itemArray, Direction.Row);
            Overwrite_Impl(row, itemArray, Direction.Row);
        }

        public void OverwriteColumn(int column, IEnumerable<IEnumerable<T>> items)
        {
            ThrowHelper.ValidateNotNull(items is null);
            var itemArray = items.ToTwoDimensionalArray();

            Validator?.Overwrite(column, itemArray, Direction.Column);
            Overwrite_Impl(column, itemArray, Direction.Column);
        }

        public void MoveRow(int oldRow, int newRow)
            => MoveRowRange(oldRow, newRow, 1);

        public void MoveRowRange(int oldRow, int newRow, int count)
        {
            Validator?.Move(oldRow, newRow, count, Direction.Row);
            Move_Impl(oldRow, newRow, count, Direction.Row);
        }

        public void MoveColumn(int oldColumn, int newColumn)
            => MoveColumnRange(oldColumn, newColumn, 1);

        public void MoveColumnRange(int oldColumn, int newColumn, int count)
        {
            Validator?.Move(oldColumn, newColumn, count, Direction.Column);
            Move_Impl(oldColumn, newColumn, count, Direction.Column);
        }

        public void RemoveRow(int index)
            => RemoveRowRange(index, 1);

        public void RemoveRowRange(int index, int count)
        {
            Validator?.Remove(index, count, Direction.Row);
            Remove_Impl(index, count, Direction.Row);
        }

        public void RemoveColumn(int column)
            => RemoveColumnRange(column, 1);

        public void RemoveColumnRange(int column, int count)
        {
            Validator?.Remove(column, count, Direction.Column);
            Remove_Impl(column, count, Direction.Column);
        }

        public void AdjustRowLength(int length)
            => AdjustLength(length, ColumnCount);

        public void AdjustRowLengthIfShort(int length)
        {
            var columnLength = ColumnCount;
            Validator?.AdjustLength(length, columnLength);

            var fixedRowLength = Math.Max(length, RowCount);

            AdjustLength_Impl(fixedRowLength, columnLength);
        }

        public void AdjustRowLengthIfLong(int length)
        {
            var columnLength = ColumnCount;
            Validator?.AdjustLength(length, columnLength);

            var fixedRowLength = Math.Min(length, RowCount);

            AdjustLength_Impl(fixedRowLength, columnLength);
        }

        public void AdjustColumnLength(int length)
            => AdjustLength(RowCount, length);

        public void AdjustColumnLengthIfShort(int length)
        {
            var rowLength = RowCount;
            Validator?.AdjustLength(rowLength, length);

            var fixedColumnLength = Math.Max(length, ColumnCount);

            AdjustLength_Impl(rowLength, fixedColumnLength);
        }

        public void AdjustColumnLengthIfLong(int length)
        {
            var rowLength = RowCount;
            Validator?.AdjustLength(rowLength, length);

            var fixedColumnLength = Math.Min(length, ColumnCount);

            AdjustLength_Impl(rowLength, fixedColumnLength);
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

        public void Reset()
        {
            var resetItems = MakeDefaultItem(GetMinRowCapacity(), GetMinColumnCapacity())
                .ToTwoDimensionalArray();
            Reset_Impl(resetItems);
        }

        public void Reset(IEnumerable<IEnumerable<T>> initItems)
        {
            ThrowHelper.ValidateArgumentNotNull(initItems is null, nameof(initItems));
            var itemArray = initItems.ToTwoDimensionalArray();

            Validator?.Reset(itemArray);
            Reset_Impl(itemArray);
        }

        public void Clear()
        {
            var resetItems = MakeDefaultItem(GetMinRowCapacity(), GetMinColumnCapacity())
                .ToTwoDimensionalArray();
            Reset_Impl(resetItems);
        }

        public override bool ItemEquals(TwoDimensionalList<T>? other)
            => ItemEquals(other);

        public bool ItemEquals(IEnumerable<IEnumerable<T>>? other)
            => ItemEquals(other, null);

        public bool ItemEquals(IEnumerable<IEnumerable<T>>? other,
            IEqualityComparer<T>? itemComparer)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            var otherArray = other.ToArray();

            if (otherArray.HasNullItem()) return false;

            var otherArrays = otherArray.ToTwoDimensionalArray();
            if (RowCount != otherArray.Length) return false;
            if (RowCount == 0) return true;
            if (ColumnCount != otherArrays[0].Length) return false;

            var innerItemComparer = itemComparer is null
                ? EqualityComparerFactory.Create<T>()
                : null;
            return this.Zip(otherArray)
                .All(zip => zip.Item1.SequenceEqual(zip.Item2, innerItemComparer));
        }

        public T[][] ToTwoDimensionalArray(bool isTranspose = false)
            => isTranspose
                ? Items.ToTransposedArray()
                : Items.ToTwoDimensionalArray();

        public IReadOnlyTwoDimensionalList<T> AsReadableList()
            => this;

        public override TwoDimensionalList<T> DeepClone() => new(this);

        public TwoDimensionalList<T> DeepCloneWith(int? rowLength = null, int? colLength = null,
            IReadOnlyDictionary<(int row, int col), T>? values = null)
        {
            var result = new TwoDimensionalList<T>(this);

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

        #region GetEnumerator

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion

        #region AsWritableList

        IFixedLengthTwoDimensionalList<T>
            ISizeChangeableTwoDimensionalList<T, ITwoDimensionalList<T>, IFixedLengthTwoDimensionalList<T>,
                IReadOnlyTwoDimensionalList<T>>.AsWritableList()
            => this;

        IFixedLengthTwoDimensionalList<T>
            IColumnSizeChangeableTwoDimensionalList<T, ITwoDimensionalList<T>, IFixedLengthTwoDimensionalList<T>,
                IReadOnlyTwoDimensionalList<T>>.AsWritableList()
            => this;

        IFixedLengthTwoDimensionalList<T>
            IRowSizeChangeableTwoDimensionalList<T, ITwoDimensionalList<T>, IFixedLengthTwoDimensionalList<T>,
                IReadOnlyTwoDimensionalList<T>>.AsWritableList()
            => this;

        #endregion

        #region DeepClone

        IReadOnlyTwoDimensionalList<T> IDeepCloneable<IReadOnlyTwoDimensionalList<T>>.DeepClone()
            => DeepClone();

        #endregion

        #region DeepCloneWith

        IReadOnlyTwoDimensionalList<T> IDeepCloneableTwoDimensionalListInternal<IReadOnlyTwoDimensionalList<T>, T>.
            DeepCloneWith(int? rowLength, int? colLength,
                IReadOnlyDictionary<(int row, int col), T>? values)
            => DeepCloneWith(rowLength, colLength, values);

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Private Methods
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private void RaiseCollectionChanging(NotifyCollectionChangedEventArgs args)
            => collectionChanging?.Invoke(this, args);

        private void RaiseCollectionChanged(NotifyCollectionChangedEventArgs args)
            => collectionChanged?.Invoke(this, args);

        private void RaiseTowDimensionalListChanging(TwoDimensionalCollectionChangeEventInternalArgs<T> internalArgs)
            => twoDimensionalListChanging?.Invoke(this, internalArgs);

        private void RaiseTowDimensionalListChanged(TwoDimensionalCollectionChangeEventInternalArgs<T> internalArgs)
            => twoDimensionalListChanged?.Invoke(this, internalArgs);

        private IEnumerable<IEnumerable<T>> GetRange(int row, int count,
            int column, int itemCount, Direction direction)
        {
            Validator?.Get(row, count, column, itemCount, direction);
            return Get_Impl(row, count, column, itemCount, direction);
        }

        private IFixedLengthList<T>[] Get_Impl(int row, int rowCount)
        {
            return Items.Range(row, rowCount).Cast<IFixedLengthList<T>>().ToArray();
        }

        private T[][] Get_Impl(int row, int rowCount, int column, int columnCount, Direction direction)
        {
            var items = Items.Range(row, rowCount).Select(line => line.Range(column, columnCount));
            return direction != Direction.Column
                ? items.ToTwoDimensionalArray()
                : items.ToTransposedArray();
        }

        private void SetRange(int row, int column, IEnumerable<IEnumerable<T>> items,
            Direction direction, bool needFitItemsInnerSize)
        {
            ThrowHelper.ValidateNotNull(items is null);
            var itemArray = items.ToTwoDimensionalArray();

            Validator?.Set(row, column, itemArray, direction, needFitItemsInnerSize);
            Set_Impl(row, column, itemArray, direction);
        }

        private void Set_Impl(int row, int column, T[][] items, Direction direction)
        {
            if (items.Length == 0) return;

            var op = Operation.CreateSet(this, row, column, items, direction,
                () => Set_Core(row, column, items, direction));
            op.Execute();
        }

        private void Set_Core(int row, int column, T[][] items, Direction direction)
            => items.ToTransposedArrayIf(direction == Direction.Column)
                .ForEach((line, rowIdx) => Items[row + rowIdx].SetRange(column, line));

        private void Insert_Impl(int index, T[][] items, Direction direction)
        {
            if (items.Length == 0) return;

            var op = Operation.CreateInsert(this, index, items, direction,
                () => Insert_Core(index, items, direction));
            op.Execute();
        }

        private void Insert_Core(int index, IEnumerable<T[]> items, Direction direction)
        {
            if (direction == Direction.Row) Insert_Core_Row(index, items);
            else Insert_Core_Column(index, items);
        }

        private void Insert_Core_Row(int index, IEnumerable<T[]> items)
            => Items.Insert(index, items.Select(ConvertInnerList).ToArray());

        private void Insert_Core_Column(int index, IEnumerable<T[]> items)
        {
            var actionInsertLine = new Action<(Inner, T[])>(zip =>
            {
                var (inner, insertItems) = zip;
                inner.InsertRange(index, insertItems);
            });

            var transposed = items.ToTransposedArray();
            Items.Zip(transposed).ForEach(actionInsertLine);
        }

        private void Overwrite_Impl(int index, T[][] items, Direction direction)
        {
            if (items.Length == 0) return;

            var op = Operation.CreateOverwrite(this, index, items, direction,
                () => Overwrite_Core(index, items, direction));
            op.Execute();
        }

        private void Overwrite_Core(int index, T[][] items, Direction direction)
        {
            if (direction == Direction.Row) Overwrite_Core_Row(index, items);
            else Overwrite_Core_Column(index, items);
        }

        private void Overwrite_Core_Row(int index, T[][] items)
        {
            var param = OverwriteParam<T[]>.Factory.Create(ToTwoDimensionalArray(), index, items);

            if (param.ReplaceNewItems.Length > 0)
            {
                var setItems = param.ReplaceNewItems
                    .Select(ConvertInnerList)
                    .ToArray();
                Items.Set(index, setItems);
            }

            if (param.InsertItems.Length > 0)
            {
                var addItems = param.InsertItems
                    .Select(ConvertInnerList)
                    .ToArray();
                Items.Insert(param.InsertStartIndex, addItems);
            }
        }

        private void Overwrite_Core_Column(int index, IEnumerable<T[]> items)
        {
            var transposedItems = items.ToTransposedArray();

            if (RowCount != 0)
            {
                // 通常の上書き
                transposedItems.ForEach((line, idx)
                    => Items[idx].Overwrite(index, line));
            }
            else
            {
                // 空リストの Overwrite == Add
                Insert_Core(0, transposedItems, Direction.Row);
            }
        }

        private void Move_Impl(int oldIndex, int newIndex, int count, Direction direction)
        {
            if (oldIndex == newIndex) return;
            if (count == 0) return;

            var op = Operation.CreateMove(this, oldIndex, newIndex, count, direction,
                () => Move_Core(oldIndex, newIndex, count, direction));
            op.Execute();
        }

        private void Move_Core(int oldIndex, int newIndex, int count, Direction direction)
        {
            if (direction == Direction.Row) Move_Core_Row(oldIndex, newIndex, count);
            else Move_Core_Column(oldIndex, newIndex, count);
        }

        private void Move_Core_Row(int oldIndex, int newIndex, int count)
            => Items.Move(oldIndex, newIndex, count);

        private void Move_Core_Column(int oldIndex, int newIndex, int count)
            => Items.ForEach(line => line.MoveRange(oldIndex, newIndex, count));

        private void Remove_Impl(int index, int count, Direction direction)
        {
            if (count == 0) return;

            var op = Operation.CreateRemove(this, index, count, direction,
                () => Remove_Core(index, count, direction));
            op.Execute();
        }

        private void Remove_Core(int index, int count, Direction direction)
        {
            if (direction == Direction.Row) Remove_Core_Row(index, count);
            else Remove_Core_Column(index, count);
        }

        private void Remove_Core_Row(int index, int count)
            => Items.Remove(index, count);

        private void Remove_Core_Column(int index, int count)
        {
            foreach (var line in Items)
            {
                line.RemoveRange(index, count);
            }
        }

        private void AdjustLength_Impl(int rowLength, int columnLength)
        {
            var addRowItems = AdjustLength_Impl_MakeAddRowItems(rowLength, columnLength);
            var addColumnItems = AdjustLength_Impl_MakeAddColumnItems(rowLength, columnLength);

            var op = Operation.CreateAdjustLength(this, rowLength, columnLength, addRowItems, addColumnItems,
                () => AdjustLength_Core(rowLength, columnLength, addRowItems, addColumnItems));
            op.Execute();
        }

        private T[][] AdjustLength_Impl_MakeAddRowItems(int rowLength, int columnLength)
        {
            var addRowLength = rowLength - RowCount;

            if (addRowLength <= 0) return Array.Empty<T[]>();

            return Enumerable.Range(RowCount, addRowLength)
                .Select(row => Enumerable.Range(0, columnLength)
                    .Select(column => FuncMakeDefaultItem(row, column)))
                .ToTwoDimensionalArray();
        }

        private T[][] AdjustLength_Impl_MakeAddColumnItems(int rowLength, int columnLength)
        {
            var addColumnLength = columnLength - ColumnCount;

            if (addColumnLength <= 0) return Array.Empty<T[]>();

            return Enumerable.Range(0, rowLength)
                .Select(row => Enumerable.Range(ColumnCount, addColumnLength)
                    .Select(column => FuncMakeDefaultItem(row, column)))
                .ToTwoDimensionalArray();
        }

        private void AdjustLength_Core(int rowLength, int columnLength,
            IReadOnlyCollection<T[]> addRowItems, T[][] addColumnItems)
        {
            // 除去が必要なら追加より前に行う
            {
                if (rowLength < RowCount)
                {
                    AdjustLength_Core_RemoveRow(rowLength);
                }

                if (columnLength < ColumnCount)
                {
                    AdjustLength_Core_RemoveColumn(columnLength);
                }
            }

            {
                if (addRowItems.Count > 0)
                {
                    // 行列ともに追加する場合、 addRowItems には現在の列数を超える要素数が含まれているので
                    // これを除去した状態で処理する必要がある。

                    var items = addRowItems.Select(line => line.Take(ColumnCount));
                    AdjustLength_Core_AddRow(items);
                }

                if (addColumnItems.GetInnerArrayLength() > 0)
                {
                    AdjustLength_Core_AddColumn(addColumnItems);
                }
            }
        }

        private void AdjustLength_Core_AddRow(IEnumerable<IEnumerable<T>> items)
        {
            var addItems = items.Select(line => InnerItemFactory.Create(line))
                .ToArray();
            Items.Add(addItems);
        }

        private void AdjustLength_Core_AddColumn(IEnumerable<T[]> items)
        {
            Items.Zip(items).ForEach(zip
                => zip.Item1.AddRange(zip.Item2));
        }

        private void AdjustLength_Core_RemoveRow(int length)
        {
            var removeLength = RowCount - length;
            Items.Remove(length, removeLength);
        }

        private void AdjustLength_Core_RemoveColumn(int length)
        {
            var removeLength = ColumnCount - length;
            Items.ForEach(line => line.RemoveRange(length, removeLength));
        }

        private void Reset_Impl(T[][] newItems)
        {
            var op = Operation.CreateReset(this, newItems,
                () => Reset_Core(newItems));
            op.Execute();
        }

        private void Reset_Core(IEnumerable<T[]> items)
        {
            var resetItems = items.Select(line => InnerItemFactory.Create(line))
                .ToArray();
            Items.Reset(resetItems);
        }

        private Inner ConvertInnerList(IEnumerable<T> src) => InnerItemFactory.Create(src);

        private IEnumerable<IEnumerable<T>> MakeDefaultItem(int rowCount, int columnCount)
        {
            return Enumerable.Range(0, rowCount)
                .Select(row => Enumerable.Range(0, columnCount)
                    .Select(column => FuncMakeDefaultItem(row, column)));
        }

        private IEnumerable<T> MakeInnerDefaultItem(Inner inner, int column, int count)
        {
            var row = Items.FindIndex(item => ReferenceEquals(item, inner));
            return Enumerable.Range(column, count).Select(col => FuncMakeDefaultItem(row, col));
        }
    }
}
