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
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;

namespace WodiLib.Sys
{
    /// <summary>
    /// 二次元リスト基底クラス
    /// </summary>
    /// <remarks>
    /// 外側のリストを「行（Row）」、内側のリストを「列（Column）」として扱う。
    /// すべての行について列数は常に同じ値を取り続ける。<br/>
    /// <br/>
    /// 要素の除去によって行数または列数が0となる場合、行数および列数どちらも0（=空の2次元リスト）となる。<br/>
    /// コンストラクタに n * 0 のリストなどを設定しても
    /// TwoDimensionalList 内では 0 * 0 の 空リストとして扱う。
    /// </remarks>
    /// <typeparam name="T">リスト内包クラス</typeparam>
    [Serializable]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract partial class TwoDimensionalListBase<T> : ModelBase<TwoDimensionalListBase<T>>,
        IReadOnlyTwoDimensionalList<T>
    {
        /*
         * このクラスの実装観点は ExtendedListBase<T> と同じ。
         * 行数または列数が0となる操作を行った場合、
         * コンストラクタ以外では Clear_Core が呼ばれ、他の Core メソッドは実行されない。
         * コンストラクタの場合は Constructor_Core が実行される。このときの引数は空配列となる。
         */

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Public Virtual Event
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public virtual event EventHandler<TwoDimensionalCollectionChangeEventArgs<T>>? TwoDimensionListChanged
        {
            add => TwoDimensionListChanged_Impl += value;
            remove => TwoDimensionListChanged_Impl -= value;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Event
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /* マルチスレッドを考慮して、イベントハンドラ本体の実装は自動実装に任せる。 */
        [field: NonSerialized]
        private event EventHandler<TwoDimensionalCollectionChangeEventArgs<T>>? _twoDimensionListChanging;

        /// <summary>
        /// 要素変更前通知
        /// </summary>
        /// <remarks>
        ///     同じイベントを重複して登録することはできない。
        /// </remarks>
        protected event EventHandler<TwoDimensionalCollectionChangeEventArgs<T>> TwoDimensionListChanging_Impl
        {
            add
            {
                if (_twoDimensionListChanging != null
                    && _twoDimensionListChanging.GetInvocationList().Contains(value)) return;
                _twoDimensionListChanging += value;
            }
            remove => _twoDimensionListChanging -= value;
        }

        [field: NonSerialized]
        private event EventHandler<TwoDimensionalCollectionChangeEventArgs<T>>? _twoDimensionListChanged;

        /// <summary>
        /// 要素変更後通知
        /// </summary>
        /// <remarks>
        ///     同じイベントを重複して登録することはできない。
        /// </remarks>
        protected event EventHandler<TwoDimensionalCollectionChangeEventArgs<T>> TwoDimensionListChanged_Impl
        {
            add
            {
                if (_twoDimensionListChanged != null
                    && _twoDimensionListChanged.GetInvocationList().Contains(value)) return;
                _twoDimensionListChanged += value;
            }
            remove => _twoDimensionListChanged -= value;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Abstract Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public T this[int row, int column] => Get_Impl(row, 1, column, 1).First().First();

        /// <inheritdoc />
        public abstract int RowCount { get; }

        /// <inheritdoc />
        public abstract int ColumnCount { get; }

        /// <inheritdoc />
        public bool IsEmpty => RowCount == 0;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 各種操作時の検証処理実装
        /// </summary>
        private ITwoDimensionalListValidator<T>? Validator { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        protected TwoDimensionalListBase()
        {
            Validator = MakeValidator();
            var initItems = MakeInitItems();
            Constructor_Impl(initItems);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="initItems">初期要素</param>
        /// <exception cref="ArgumentNullException">
        ///     initItems が null の場合
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     items中にnull要素が含まれる場合、
        ///     または items 内側シーケンスの要素数が0の場合、
        ///     または items 内側シーケンスに要素数が異なる要素が存在する場合
        /// </exception>
        protected TwoDimensionalListBase(IEnumerable<IEnumerable<T>> initItems)
        {
            if (initItems is null)
            {
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(initItems)));
            }

            Validator = MakeValidator();

            var itemArray = initItems.Cast<T[]>().ToArray();
            Constructor_Impl(itemArray);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Abstract Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public abstract IEnumerator<IEnumerable<T>> GetEnumerator();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public IEnumerable<T> GetRow(int row)
            => GetRow_Impl(row, 1).First();

        /// <inheritdoc />
        public IEnumerable<T> GetColumn(int column)
            => GetColumn_Impl(column, 1).First();

        /// <inheritdoc />
        public IEnumerable<IEnumerable<T>> GetRowRange(int row, int count)
            => GetRow_Impl(row, count);

        /// <inheritdoc />
        public IEnumerable<IEnumerable<T>> GetColumnRange(int column, int count)
            => GetColumn_Impl(column, count);

        /// <inheritdoc />
        public IEnumerable<IEnumerable<T>> GetRange(
            int row, int rowCount, int column, int columnCount)
            => Get_Impl(row, rowCount, column, columnCount);

        /// <inheritdoc />
        public bool Equals(IEnumerable<IEnumerable<T>>? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            var comparer = DoubleSequenceEqualityComparer<T>.GetInstance();

            return comparer.Equals(this, other);
        }

        /// <inheritdoc />
        public bool Equals(IReadOnlyTwoDimensionalList<T>? other)
        {
            if (ReferenceEquals(null, other)) return false;
            return Equals(other.AsEnumerable());
        }

        /// <inheritdoc />
        public override bool Equals(TwoDimensionalListBase<T>? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            var comparer = DoubleSequenceEqualityComparer<T>.GetInstance();

            return comparer.Equals(this, other);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Interface Implements
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Abstract Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 要素のデフォルト値を生成する。
        /// </summary>
        /// <remarks>
        /// AdjustLength で要素を追加する必要がある際に
        /// このメソッドで作成された値を追加する。
        /// </remarks>
        /// <param name="row">行番号</param>
        /// <param name="column">列番号</param>
        /// <returns>追加要素</returns>
        protected abstract T MakeDefaultItem(int row, int column);

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Virtual Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// ITwoDimensionalListValidator&gt;T&lt; インスタンスを作成する。
        /// </summary>
        /// <remarks>
        /// このメソッドはコンストラクタの冒頭で呼ばれる。
        /// </remarks>
        /// <returns>検証処理インスタンス</returns>
        protected virtual ITwoDimensionalListValidator<T> MakeValidator()
        {
            return new CommonTwoDimensionalListValidator<T>(this);
        }

        #region Action Core

        /// <summary>
        /// コンストラクタ処理本体
        /// </summary>
        /// <param name="initItems">初期化要素</param>
        protected virtual void Constructor_Core(params T[][] initItems)
            => throw new NotSupportedException();

        /// <summary>
        /// Get, GetRange メソッドの処理本体
        /// </summary>
        /// <param name="row">行番号</param>
        /// <param name="rowCount">行数</param>
        /// <param name="column">列番号</param>
        /// <param name="columnCount">列数</param>
        /// <returns>取得結果</returns>
        protected virtual IEnumerable<IEnumerable<T>> Get_Core(int row, int rowCount, int column, int columnCount)
            => throw new NotSupportedException();

        /// <summary>
        /// GetRow, GetRowRange メソッドの処理本体
        /// </summary>
        /// <param name="row">行番号</param>
        /// <param name="count">行数</param>
        /// <returns>取得結果</returns>
        protected virtual IEnumerable<IEnumerable<T>> GetRow_Core(int row, int count)
            => throw new NotSupportedException();

        /// <summary>
        /// GetColumn, GetColumnRange メソッドの処理本体
        /// </summary>
        /// <param name="column">列番号</param>
        /// <param name="count">列数</param>
        /// <returns>取得結果</returns>
        protected virtual IEnumerable<IEnumerable<T>> GetColumn_Core(int column, int count)
            => throw new NotSupportedException();

        /// <summary>
        /// Set, SetRange メソッドの処理本体
        /// </summary>
        /// <param name="row">更新開始行番号</param>
        /// <param name="column">更新開始列番号</param>
        /// <param name="items">更新要素</param>
        protected virtual void Set_Core(int row, int column, T[][] items)
            => throw new NotSupportedException();

        /// <summary>
        /// AddRow, AddRowRange, InsertRow, InsertRowRange メソッドの処理本体
        /// </summary>
        /// <param name="row">挿入行番号</param>
        /// <param name="items">挿入要素</param>
        protected virtual void InsertRow_Core(int row, T[][] items)
            => throw new NotSupportedException();

        /// <summary>
        /// AddColumn, AddColumnRange, InsertColumn, InsertColumnRange メソッドの処理本体
        /// </summary>
        /// <param name="column">挿入列番号</param>
        /// <param name="items">挿入要素</param>
        protected virtual void InsertColumn_Core(int column, T[][] items)
            => throw new NotSupportedException();

        /// <summary>
        /// MoveRow, MoveRowRange メソッドの処理本体
        /// </summary>
        /// <param name="oldRow">移動前行番号</param>
        /// <param name="newRow">移動後行番号</param>
        /// <param name="count">移動行数</param>
        protected virtual void MoveRow_Core(int oldRow, int newRow, int count)
            => throw new NotSupportedException();

        /// <summary>
        /// MoveColumn, MoveColumnRange メソッドの処理本体
        /// </summary>
        /// <param name="oldColumn">移動前列番号</param>
        /// <param name="newColumn">移動後列番号</param>
        /// <param name="count">移動列数</param>
        protected virtual void MoveColumn_Core(int oldColumn, int newColumn, int count)
            => throw new NotSupportedException();

        /// <summary>
        /// RemoveRow, RemoveRowRange メソッドの処理本体
        /// </summary>
        /// <param name="row">削除開始行番号</param>
        /// <param name="count">削除行数</param>
        protected virtual void RemoveRow_Core(int row, int count)
            => throw new NotSupportedException();

        /// <summary>
        /// RemoveColumn, RemoveColumnRange メソッドの処理本体
        /// </summary>
        /// <param name="column">削除開始列番号</param>
        /// <param name="count">削除列数</param>
        protected virtual void RemoveColumn_Core(int column, int count)
            => throw new NotSupportedException();

        /// <summary>
        /// Reset メソッドの処理本体<br/>
        /// AddRow, AddRowRange, InsertRow, InsertRowRange,
        /// AddColumn, AddColumnRange, InsertColumn, InsertColumnRange メソッドにおいて
        /// 自身が空リストの場合にも呼び出される
        /// </summary>
        /// <param name="items">初期化要素</param>
        protected virtual void Reset_Core(T[][] items)
            => throw new NotSupportedException();

        /// <summary>
        /// Clear メソッドの処理本体<br/>
        /// 何らかの操作によって行数または列数が0になる場合も呼び出される。
        /// </summary>
        protected virtual void Clear_Core()
            => throw new NotSupportedException();

        #endregion

        /// <summary>
        /// 引数なしコンストラクタ、 Clear メソッド実行時に配列を初期化するための要素を作成する。
        /// </summary>
        /// <returns>初期化要素。デフォルトでは空配列</returns>
        protected virtual T[][] MakeInitItems()
        {
            return Array.Empty<T[]>();
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Action Implements

        /// <summary>
        /// コンストラクタの処理実装
        /// </summary>
        /// <param name="initItems">初期要素</param>
        protected void Constructor_Impl(params T[][] initItems)
        {
            Validator?.Constructor(initItems);
            Constructor_Core(initItems);
        }

        /// <summary>
        /// Get, GetRange メソッドの処理実装
        /// </summary>
        /// <param name="row">行番号</param>
        /// <param name="rowCount">行数</param>
        /// <param name="column">列番号</param>
        /// <param name="columnCount">列数</param>
        protected IEnumerable<IEnumerable<T>> Get_Impl(int row, int rowCount, int column, int columnCount)
        {
            Validator?.Get(row, rowCount, column, columnCount);
            return Get_Core(row, rowCount, column, columnCount);
        }

        /// <summary>
        /// GetRow, GetRowRange メソッドの処理実装
        /// </summary>
        /// <param name="row">行番号</param>
        /// <param name="count">行数</param>
        protected IEnumerable<IEnumerable<T>> GetRow_Impl(int row, int count)
        {
            Validator?.GetRow(row, count);
            return GetRow_Core(row, count);
        }

        /// <summary>
        /// GetColumn, GetColumnRange メソッドの処理実装
        /// </summary>
        /// <param name="column">列番号</param>
        /// <param name="count">列数</param>
        protected IEnumerable<IEnumerable<T>> GetColumn_Impl(int column, int count)
        {
            Validator?.GetColumn(column, count);
            return GetColumn_Core(column, count);
        }

        /// <summary>
        /// Set, SetRange メソッドの処理実装
        /// </summary>
        /// <param name="row">更新開始行番号</param>
        /// <param name="column">更新開始列番号</param>
        /// <param name="items">更新要素</param>
        protected void Set_Impl(int row, int column, T[][] items)
        {
            Validator?.Set(row, column, items);

            var rowLength = items.Length;
            var colLength = items.GetInnerArrayLength();
            var oldItems = Get_Core(row, rowLength, column, colLength)
                .ToTwoDimensionalArray();
            var eventArgs = TwoDimensionalCollectionChangeEventArgs<T>.Factory.Set(row, column, oldItems, items);

            CallCollectionChanging(eventArgs);

            Set_Core(row, column, items);

            NotifyPropertyChanged(ListConstant.IndexerName);
            CallCollectionChanged(eventArgs);
        }


        /// <summary>
        /// AddRow, AddRowRange, InsertRow, InsertRowRange メソッドの処理実装
        /// </summary>
        /// <param name="row">挿入行番号</param>
        /// <param name="items">挿入要素</param>
        protected void InsertRow_Impl(int row, T[][] items)
        {
            Validator?.InsertRow(row, items);

            var isBeforeEmpty = IsEmpty;
            var isAfterEmpty = items.Length == 0;

            var eventArgs = TwoDimensionalCollectionChangeEventArgs<T>.Factory.AddRow(row, items);

            CallCollectionChanging(eventArgs);

            InsertRow_Core(row, items);

            NotifyPropertyChanged(nameof(RowCount));
            if (isBeforeEmpty && !isAfterEmpty) NotifyPropertyChanged(nameof(ColumnCount));
            NotifyPropertyChanged(ListConstant.IndexerName);

            CallCollectionChanged(eventArgs);
        }

        /// <summary>
        /// AddColumn, AddColumnRange, InsertColumn, InsertColumnRange メソッドの処理実装
        /// </summary>
        /// <param name="column">挿入列番号</param>
        /// <param name="items">挿入要素</param>
        protected void InsertColumn_Impl(int column, T[][] items)
        {
            Validator?.InsertColumn(column, items);

            var eventArgs = TwoDimensionalCollectionChangeEventArgs<T>.Factory.AddColumn(column, items);

            CallCollectionChanging(eventArgs);

            InsertColumn_Core(column, items);

            NotifyPropertyChanged(nameof(ColumnCount));
            NotifyPropertyChanged(ListConstant.IndexerName);

            CallCollectionChanged(eventArgs);
        }

        /// <summary>
        /// OverwriteRow メソッドの処理実装
        /// </summary>
        /// <param name="row">開始行番号</param>
        /// <param name="items">上書き/追加リスト</param>
        protected void OverwriteRow_Impl(int row, T[][] items)
        {
            Validator?.OverwriteRow(row, items);

            var agent = OverwriteAgent.ForOverwriteRow(this, row, items);
            agent.Execute();
        }

        /// <summary>
        /// OverwriteColumn メソッドの処理実装
        /// </summary>
        /// <param name="column">開始列番号</param>
        /// <param name="items">上書き/追加リスト</param>
        protected void OverwriteColumn_Impl(int column, T[][] items)
        {
            Validator?.OverwriteColumn(column, items);

            var agent = OverwriteAgent.ForOverwriteColumn(this, column, items);
            agent.Execute();
        }

        /// <summary>
        /// MoveRow, MoveRowRange メソッドの処理実装
        /// </summary>
        /// <param name="oldRow">移動前行番号</param>
        /// <param name="newRow">移動後行番号</param>
        /// <param name="count">移動行数</param>
        protected void MoveRow_Impl(int oldRow, int newRow, int count)
        {
            Validator?.MoveRow(oldRow, newRow, count);

            var movedItems = GetRow_Core(oldRow, count)
                .ToTwoDimensionalArray();
            var eventArgs = TwoDimensionalCollectionChangeEventArgs<T>.Factory.MoveRow(oldRow, newRow, movedItems);

            CallCollectionChanging(eventArgs);

            MoveRow_Core(oldRow, newRow, count);

            NotifyPropertyChanged(ListConstant.IndexerName);
            CallCollectionChanged(eventArgs);
        }

        /// <summary>
        /// MoveColumn, MoveColumnRange メソッドの処理実装
        /// </summary>
        /// <param name="oldColumn">移動前列番号</param>
        /// <param name="newColumn">移動後列番号</param>
        /// <param name="count">移動列数</param>
        protected void MoveColumn_Impl(int oldColumn, int newColumn, int count)
        {
            Validator?.MoveColumn(oldColumn, newColumn, count);

            var movedItem = GetColumn_Core(oldColumn, count)
                .ToTwoDimensionalArray();
            var eventArgs =
                TwoDimensionalCollectionChangeEventArgs<T>.Factory.MoveColumn(oldColumn, newColumn, movedItem);

            CallCollectionChanging(eventArgs);

            MoveColumn_Core(oldColumn, newColumn, count);

            NotifyPropertyChanged(ListConstant.IndexerName);
            CallCollectionChanged(eventArgs);
        }

        /// <summary>
        /// RemoveRow, RemoveRowRange メソッドの処理実装
        /// </summary>
        /// <param name="row">削除開始行番号</param>
        /// <param name="count">削除行数</param>
        protected void RemoveRow_Impl(int row, int count)
        {
            Validator?.RemoveRow(row, count);

            var removedItem = GetRow_Core(row, count)
                .ToTwoDimensionalArray();
            var eventArgs = TwoDimensionalCollectionChangeEventArgs<T>.Factory.RemoveRow(row, removedItem);

            CallCollectionChanging(eventArgs);

            var removedRowCount = RowCount - count;
            if (removedRowCount == 0)
            {
                Clear_Core();

                NotifyPropertyChanged(nameof(RowCount));
                NotifyPropertyChanged(nameof(ColumnCount));
                NotifyPropertyChanged(ListConstant.IndexerName);
            }
            else
            {
                RemoveRow_Core(row, count);

                NotifyPropertyChanged(nameof(RowCount));
                NotifyPropertyChanged(ListConstant.IndexerName);
            }

            CallCollectionChanged(eventArgs);
        }

        /// <summary>
        /// RemoveColumn, RemoveColumnRange メソッドの処理実装
        /// </summary>
        /// <param name="column">削除開始列番号</param>
        /// <param name="count">削除列数</param>
        protected void RemoveColumn_Impl(int column, int count)
        {
            Validator?.RemoveColumn(column, count);

            var removedItem = GetColumn_Core(column, count)
                .ToTwoDimensionalArray();
            var eventArgs = TwoDimensionalCollectionChangeEventArgs<T>.Factory.RemoveColumn(column, removedItem);

            CallCollectionChanging(eventArgs);

            RemoveColumn_Core(column, count);

            NotifyPropertyChanged(nameof(ColumnCount));
            NotifyPropertyChanged(ListConstant.IndexerName);
            CallCollectionChanged(eventArgs);
        }

        /// <summary>
        /// AdjustLength メソッドの処理実装
        /// </summary>
        /// <param name="rowLength">調整行数</param>
        /// <param name="columnLength">調整列数</param>
        protected void AdjustLength_Impl(int rowLength, int columnLength)
        {
            Validator?.AdjustLength(rowLength, columnLength);

            var agent = AdjustLengthAgent.ForAdjustLength(this, rowLength, columnLength);
            agent.Execute();
        }

        /// <summary>
        /// AdjustLengthIfShort メソッドの処理実装
        /// </summary>
        /// <param name="rowLength">調整行数</param>
        /// <param name="columnLength">調整列数</param>
        protected void AdjustLengthIfShort_Impl(int rowLength, int columnLength)
        {
            Validator?.AdjustLengthIfShort(rowLength, columnLength);

            var agent = AdjustLengthAgent.ForAdjustLengthIfShort(this, rowLength, columnLength);
            agent.Execute();
        }

        /// <summary>
        /// AdjustLengthIfLong メソッドの処理実装
        /// </summary>
        /// <param name="rowLength">調整行数</param>
        /// <param name="columnLength">調整列数</param>
        protected void AdjustLengthIfLong_Impl(int rowLength, int columnLength)
        {
            Validator?.AdjustLengthIfLong(rowLength, columnLength);

            var agent = AdjustLengthAgent.ForAdjustLengthIfLong(this, rowLength, columnLength);
            agent.Execute();
        }

        /// <summary>
        /// AdjustRowLength メソッドの処理実装
        /// </summary>
        /// <param name="length">調整要素数</param>
        protected void AdjustRowLength_Impl(int length)
        {
            Validator?.AdjustRowLength(length);

            var agent = AdjustLengthAgent.ForAdjustRowLength(this, length);
            agent.Execute();
        }

        /// <summary>
        /// AdjustColumnLength メソッドの処理実装
        /// </summary>
        /// <param name="length">調整要素数</param>
        protected void AdjustColumnLength_Impl(int length)
        {
            Validator?.AdjustColumnLength(length);

            var agent = AdjustLengthAgent.ForAdjustColumnLength(this, length);
            agent.Execute();
        }

        /// <summary>
        /// AdjustRowLengthIfShort メソッドの処理実装
        /// </summary>
        /// <param name="length">調整要素数</param>
        protected void AdjustRowLengthIfShort_Impl(int length)
        {
            Validator?.AdjustRowLengthIfShort(length);

            var agent = AdjustLengthAgent.ForAdjustRowLengthIfShort(this, length);
            agent.Execute();
        }

        /// <summary>
        /// AdjustColumnLengthIfShort メソッドの処理実装
        /// </summary>
        /// <param name="length">調整要素数</param>
        protected void AdjustColumnLengthIfShort_Impl(int length)
        {
            Validator?.AdjustColumnLengthIfShort(length);

            var agent = AdjustLengthAgent.ForAdjustColumnLengthIfShort(this, length);
            agent.Execute();
        }

        /// <summary>
        /// AdjustRowLengthIfLong メソッドの処理実装
        /// </summary>
        /// <param name="length">調整要素数</param>
        protected void AdjustRowLengthIfLong_Impl(int length)
        {
            Validator?.AdjustRowLengthIfLong(length);

            var agent = AdjustLengthAgent.ForAdjustRowLengthIfLong(this, length);
            agent.Execute();
        }

        /// <summary>
        /// AdjustColumnLengthIfLong メソッドの処理実装
        /// </summary>
        /// <param name="length">調整要素数</param>
        protected void AdjustColumnLengthIfLong_Impl(int length)
        {
            Validator?.AdjustColumnLengthIfLong(length);

            var agent = AdjustLengthAgent.ForAdjustColumnLengthIfLong(this, length);
            agent.Execute();
        }

        /// <summary>
        /// Clear メソッドの処理実装
        /// </summary>
        protected void Clear_Impl()
        {
            var items = MakeInitItems();
            Reset_Impl(items);
        }

        /// <summary>
        /// Reset メソッドの処理実装
        /// </summary>
        /// <param name="items">初期化要素</param>
        protected void Reset_Impl(T[][] items)
        {
            Validator?.Reset(items);

            var eventArgs = TwoDimensionalCollectionChangeEventArgs<T>.Factory.Reset();

            CallCollectionChanging(eventArgs);

            Reset_Core(items);

            NotifyPropertyChanged(nameof(RowCount));
            NotifyPropertyChanged(nameof(ColumnCount));
            NotifyPropertyChanged(ListConstant.IndexerName);
            CallCollectionChanged(eventArgs);
        }

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// デフォルト要素配列を作成する。
        /// </summary>
        /// <param name="row">開始行番号</param>
        /// <param name="rowCount">行数</param>
        /// <param name="column">開始列番号</param>
        /// <param name="columnCount">列数</param>
        /// <returns></returns>
        private T[][] MakeDefaultItemArray(int row, int rowCount, int column, int columnCount)
        {
            var colRange = Enumerable.Range(column, columnCount);
            return Enumerable.Range(row, rowCount)
                .Select(rowIdx =>
                    colRange.Select(colIdx =>
                        MakeDefaultItem(rowIdx, colIdx)
                    ).ToArray()
                ).ToArray();
        }

        /// <summary>
        /// TwoDimensionalCollectionChanging イベントを発火する。
        /// </summary>
        /// <param name="args">イベント引数</param>
        private void CallCollectionChanging(TwoDimensionalCollectionChangeEventArgs<T> args)
            => _twoDimensionListChanging?.Invoke(this, args);

        /// <summary>
        /// TwoDimensionalCollectionChanged イベントを発火する。
        /// </summary>
        /// <param name="args">イベント引数</param>
        private void CallCollectionChanged(TwoDimensionalCollectionChangeEventArgs<T> args)
            => _twoDimensionListChanged?.Invoke(this, args);

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Serializable
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public abstract void GetObjectData(SerializationInfo info, StreamingContext context);

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="info">デシリアライズ情報</param>
        /// <param name="context">コンテキスト</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected TwoDimensionalListBase(SerializationInfo info, StreamingContext context)
        {
            Validator = MakeValidator();
        }
    }
}
