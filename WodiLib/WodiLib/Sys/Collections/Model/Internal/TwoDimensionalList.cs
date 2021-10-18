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
    internal partial class TwoDimensionalList<T> :
        ModelBase<TwoDimensionalList<T>>,
        ITwoDimensionalList<T, T>,
        IDeepCloneableTwoDimensionalListInternal<TwoDimensionalList<T>, T>,
        ITwoDimensionalList<T>, IReadableTwoDimensionalList<T, TwoDimensionalList<T>>
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
        public delegate ITwoDimensionalListValidator<T, T>? InjectValidator(
            TwoDimensionalList<T> self);

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

        public T this[int rowIndex, int columnIndex]
        {
            get
            {
                Validator?.GetItem(rowIndex, columnIndex);
                return Items[rowIndex][columnIndex];
            }
            set
            {
                ThrowHelper.ValidatePropertyNotNull(value is null, typeof(TwoDimensionalList<>).Name);
                Validator?.SetItem(rowIndex, columnIndex, value);
                Set_Impl(rowIndex, columnIndex, new[] { new[] { value } }, Direction.None);
            }
        }

        public bool IsEmpty => RowCount == 0;
        public int RowCount => Items.Count;
        public int ColumnCount => RowCount > 0 ? Items[0].Count : 0;
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

        private SimpleList<SimpleList<T>> Items { get; }

        private Func<int, int, T> FuncMakeDefaultItem { get; }

        private ITwoDimensionalListValidator<T, T>? Validator { get; }

        private int MaxRowCapacity => config.MaxRowCapacity;
        private int MinRowCapacity => config.MinRowCapacity;
        private int MaxColumnCapacity => config.MaxColumnCapacity;
        private int MinColumnCapacity => config.MinColumnCapacity;

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

        private readonly Config config;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Constructors
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        internal TwoDimensionalList(IEnumerable<IEnumerable<T>> values,
            Config config)
        {
            this.config = config;
            FuncMakeDefaultItem = config.ItemFactory;
            Validator = config.ValidatorFactory(this);

            var valuesArray = values.ToTwoDimensionalArray();

            Validator?.Constructor(valuesArray);

            var initItems = valuesArray.Select(ConvertInnerList);
            Items = new SimpleList<SimpleList<T>>(initItems);

            notifyCollectionChangingEventType = WodiLibConfig.GetDefaultNotifyBeforeCollectionChangeEventType();
            notifyCollectionChangedEventType = WodiLibConfig.GetDefaultNotifyAfterCollectionChangeEventType();
            notifyTwoDimensionalListChangingEventType =
                WodiLibConfig.GetDefaultNotifyBeforeTwoDimensionalListChangeEventType();
            notifyTwoDimensionalListChangedEventType =
                WodiLibConfig.GetDefaultNotifyAfterTwoDimensionalListChangeEventType();
        }

        internal TwoDimensionalList(int rowLength, int columnLength,
            Config config) : this(
            ((Func<IEnumerable<IEnumerable<T>>>)(() =>
                Enumerable.Range(0, rowLength).Select(rowIdx
                    => Enumerable.Range(0, columnLength).Select(colIdx
                        => config.ItemFactory(rowIdx, colIdx)))))(),
            config)
        {
        }

        internal TwoDimensionalList(Config config) : this(
            Enumerable.Range(0, config.MinRowCapacity).Select(row =>
                Enumerable.Range(0, config.MinColumnCapacity).Select(col => config.ItemFactory(row, col))),
            config)
        {
        }

        private TwoDimensionalList(
            TwoDimensionalList<T> src) : this(
            src.Items.Select(line => line.DeepClone()),
            src.config)
        {
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Public Methods
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        public int GetMaxRowCapacity() => MaxRowCapacity;

        public int GetMinRowCapacity() => MinRowCapacity;

        public int GetMaxColumnCapacity() => MaxColumnCapacity;

        public int GetMinColumnCapacity() => MinColumnCapacity;

        public IEnumerator<IEnumerable<T>> GetEnumerator()
            => Items.GetEnumerator();

        public IEnumerable<IEnumerable<T>> GetRow(int rowIndex, int rowCount)
        {
            Validator?.GetRow(rowIndex, rowCount);
            return GetRow_Impl(rowIndex, rowCount);
        }

        public IEnumerable<IEnumerable<T>> GetColumn(int columnIndex, int columnCount)
        {
            Validator?.GetColumn(columnIndex, columnCount);
            return Get_Impl(0, RowCount, columnIndex, columnCount, Direction.Column);
        }

        public IEnumerable<IEnumerable<T>> GetItem(int rowIndex, int rowCount, int columnIndex, int columnCount)
        {
            Validator?.GetItem(rowIndex, rowCount, columnIndex, columnCount);
            return Get_Impl(rowIndex, rowCount, columnIndex, columnCount, Direction.Row);
        }

        public void SetRow(int rowIndex, params IEnumerable<T>[] rows)
        {
            Validator?.SetRow(rowIndex, rows);
            Set_Impl(rowIndex, 0, rows.ToTwoDimensionalArray(), Direction.Row);
        }

        public void SetColumn(int columnIndex, params IEnumerable<T>[] items)
        {
            Validator?.SetColumn(columnIndex, items);
            Set_Impl(0, columnIndex, items.ToTwoDimensionalArray(), Direction.Column);
        }

        public void AddRow(params IEnumerable<T>[] items)
            => InsertRow(RowCount, items);

        public void AddColumn(params IEnumerable<T>[] items)
            => InsertColumn(ColumnCount, items);

        public void InsertRow(int rowIndex, params IEnumerable<T>[] items)
        {
            Validator?.InsertRow(rowIndex, items);
            Insert_Impl(rowIndex, items.ToTwoDimensionalArray(), Direction.Row);
        }

        public void InsertColumn(int columnIndex, params IEnumerable<T>[] items)
        {
            Validator?.InsertColumn(columnIndex, items);
            Insert_Impl(columnIndex, items.ToTwoDimensionalArray(), Direction.Column);
        }

        public void OverwriteRow(int rowIndex, params IEnumerable<T>[] items)
        {
            Validator?.OverwriteRow(rowIndex, items);
            Overwrite_Impl(rowIndex, items.ToTwoDimensionalArray(), Direction.Row);
        }

        public void OverwriteColumn(int columnIndex, params IEnumerable<T>[] items)
        {
            Validator?.OverwriteColumn(columnIndex, items);
            Overwrite_Impl(columnIndex, items.ToTwoDimensionalArray(), Direction.Column);
        }

        public void MoveRow(int oldRowIndex, int newRowIndex, int count = 1)
        {
            Validator?.MoveRow(oldRowIndex, newRowIndex, count);
            Move_Impl(oldRowIndex, newRowIndex, count, Direction.Row);
        }

        public void MoveColumn(int oldColumnIndex, int newColumnIndex, int count = 1)
        {
            Validator?.MoveColumn(oldColumnIndex, newColumnIndex, count);
            Move_Impl(oldColumnIndex, newColumnIndex, count, Direction.Column);
        }

        public void RemoveRow(int rowIndex, int count = 1)
        {
            Validator?.RemoveRow(rowIndex, count);
            Remove_Impl(rowIndex, count, Direction.Row);
        }

        public void RemoveColumn(int columnIndex, int count = 1)
        {
            Validator?.RemoveColumn(columnIndex, count);
            Remove_Impl(columnIndex, count, Direction.Column);
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

        public void Reset()
        {
            var resetItems = MakeDefaultItem(GetMinRowCapacity(), GetMinColumnCapacity())
                .ToTwoDimensionalArray();
            Reset_Impl(resetItems);
        }

        public void Reset(IEnumerable<IEnumerable<T>> initItems)
        {
            ThrowHelper.ValidateArgumentNotNull(initItems is null, nameof(initItems));
            var initItemArray = initItems.ToTwoDimensionalArray();

            Validator?.Reset(initItemArray);
            Reset_Impl(initItemArray);
        }

        public void Clear()
        {
            var resetItems = MakeDefaultItem(GetMinRowCapacity(), GetMinColumnCapacity())
                .ToTwoDimensionalArray();
            Reset_Impl(resetItems);
        }

        public override bool ItemEquals(
            TwoDimensionalList<T>? other)
            => ItemEquals(other, null);

        public bool ItemEquals(ITwoDimensionalList<T>? other)
            => ItemEquals(other, null);

        public bool ItemEquals(ITwoDimensionalList<T, T>? other)
            => ItemEquals(other, null);

        public bool ItemEquals(IEnumerable<IEnumerable<T>>? other,
            IEqualityComparer<T>? itemComparer = null)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            var otherArray = other.ToArray();

            if (otherArray.HasNullItem()) return false;

            var otherArrays = otherArray.ToTwoDimensionalArray();
            if (RowCount != otherArray.Length) return false;
            if (RowCount == 0) return true;
            if (ColumnCount != otherArrays[0].Length) return false;

            return this.Zip(otherArray)
                .All(zip => zip.Item1.SequenceEqual(zip.Item2, itemComparer));
        }

        public ITwoDimensionalList<T>
            AsRowSizeChangeableList()
            => this;

        public ITwoDimensionalList<T>
            AsColumnSizeChangeableList()
            => this;

        public ITwoDimensionalList<T>
            AsWritableList()
            => this;

        public ITwoDimensionalList<T>
            AsReadableList()
            => this;

        public T[][] ToTwoDimensionalArray(bool isTranspose = false)
            => isTranspose
                ? Items.ToTransposedArray()
                : Items.ToTwoDimensionalArray();

        public override TwoDimensionalList<T>
            DeepClone() => new(this);

        public TwoDimensionalList<T> DeepCloneWith(
            int? rowLength = null, int? colLength = null,
            IReadOnlyDictionary<(int row, int col), T>? values = null)
        {
            var result =
                new TwoDimensionalList<T>(this);

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

        #region DeepClone

        ITwoDimensionalList<T> IDeepCloneable<ITwoDimensionalList<T>>.DeepClone()
            => DeepClone();

        ITwoDimensionalList<T, T> IDeepCloneable<ITwoDimensionalList<T, T>>.DeepClone()
            => DeepClone();

        #endregion

        #region DeepCloneWith

        ITwoDimensionalList<T> IDeepCloneableTwoDimensionalListInternal<ITwoDimensionalList<T>, T>.DeepCloneWith(
            int? rowLength, int? colLength, IReadOnlyDictionary<(int row, int col), T>? values)
            => DeepCloneWith(rowLength, colLength, values);

        ITwoDimensionalList<T, T> IDeepCloneableTwoDimensionalListInternal<ITwoDimensionalList<T, T>, T>.DeepCloneWith(
            int? rowLength, int? colLength, IReadOnlyDictionary<(int row, int col), T>? values)
            => DeepCloneWith(rowLength, colLength, values);

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Private Methods
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private void RaiseCollectionChanging(NotifyCollectionChangedEventArgs args)
            => collectionChanging?.Invoke(this, args);

        private void RaiseCollectionChanged(NotifyCollectionChangedEventArgs args)
            => collectionChanged?.Invoke(this, args);

        private void RaiseTowDimensionalListChanging(
            TwoDimensionalCollectionChangeEventInternalArgs<T> internalArgs)
            => twoDimensionalListChanging?.Invoke(this, internalArgs);

        private void RaiseTowDimensionalListChanged(TwoDimensionalCollectionChangeEventInternalArgs<T> internalArgs)
            => twoDimensionalListChanged?.Invoke(this, internalArgs);

        private T[][] GetRow_Impl(int row, int count)
            => Get_Impl(row, count, 0, ColumnCount, Direction.Row);

        private T[][] Get_Impl(int row, int rowCount, int column, int columnCount, Direction direction)
        {
            var items = Items.Range(row, rowCount).Select(line => line.Range(column, columnCount));
            return direction != Direction.Column
                ? items.ToTwoDimensionalArray()
                : items.ToTransposedArray();
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
                .ForEach((line, rowIdx) => Items[row + rowIdx].Set(column, line));

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
            var actionInsertLine = new Action<(SimpleList<T>, T[])>(zip =>
            {
                var (inner, insertItems) = zip;
                inner.Insert(index, insertItems);
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
            => Items.ForEach(line => line.Move(oldIndex, newIndex, count));

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
                line.Remove(index, count);
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
            var addItems = items.Select(ConvertInnerList)
                .ToArray();
            Items.Add(addItems);
        }

        private void AdjustLength_Core_AddColumn(IEnumerable<T[]> items)
        {
            Items.Zip(items).ForEach(zip
                => zip.Item1.Add(zip.Item2));
        }

        private void AdjustLength_Core_RemoveRow(int length)
        {
            var removeLength = RowCount - length;
            Items.Remove(length, removeLength);
        }

        private void AdjustLength_Core_RemoveColumn(int length)
        {
            var removeLength = ColumnCount - length;
            Items.ForEach(line => line.Remove(length, removeLength));
        }

        private void Reset_Impl(T[][] newItems)
        {
            var op = Operation.CreateReset(this, newItems,
                () => Reset_Core(newItems));
            op.Execute();
        }

        private void Reset_Core(IEnumerable<T[]> items)
        {
            var resetItems = items.Select(ConvertInnerList)
                .ToArray();
            Items.Reset(resetItems);
        }

        private SimpleList<T> ConvertInnerList(IEnumerable<T> src) => new(src);

        private IEnumerable<IEnumerable<T>> MakeDefaultItem(int rowCount, int columnCount)
        {
            return Enumerable.Range(0, rowCount)
                .Select(row => Enumerable.Range(0, columnCount)
                    .Select(column => FuncMakeDefaultItem(row, column)));
        }
    }
}
