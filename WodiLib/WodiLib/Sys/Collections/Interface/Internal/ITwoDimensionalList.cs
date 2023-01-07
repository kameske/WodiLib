// ========================================
// Project Name : WodiLib
// File Name    : ITwoDimensionalList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     WodiLib内部で使用する二次元リスト
    /// </summary>
    /// <remarks>
    ///     外部公開用の二次元リストはこのインタフェースを継承せずに作成する。
    ///     データベースにおける「データ」と「項目」のような、「行」や「列」の呼び方が変わる場合があることを考慮。
    /// </remarks>
    /// <typeparam name="TRow">リスト行データ型</typeparam>
    /// <typeparam name="TItem">リスト要素型</typeparam>
    internal interface ITwoDimensionalList<TRow, TItem> :
        IModelBase<ITwoDimensionalList<TRow, TItem>>,
        IEnumerable<TRow>,
        INotifyCollectionChanged
        where TRow : IEnumerable<TItem>
    {
        /// <summary>
        ///     インデクサによるアクセス
        /// </summary>
        /// <param name="rowIndex">[Range(0, <see cref="RowCount"/> - 1)] 行インデックス</param>
        /// <returns>指定したインデックスの行要素</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="rowIndex"/>が指定範囲外の場合。</exception>
        public TRow this[int rowIndex] { get; set; }

        /// <summary>
        ///     インデクサによるアクセス
        /// </summary>
        /// <param name="rowIndex">[Range(0, <see cref="RowCount"/> - 1)] 行インデックス</param>
        /// <param name="columnIndex">[Range(0, <see cref="ColumnCount"/> - 1)] 列インデックス</param>
        /// <returns>指定したインデックスの要素</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="rowIndex"/>, <paramref name="columnIndex"/> が指定範囲外の場合。
        /// </exception>
        public TItem this[int rowIndex, int columnIndex] { get; set; }

        /// <summary>
        ///     空フラグ
        /// </summary>
        /// <remarks>
        ///     <see cref="RowCount"/> == 0 かつ <see cref="ColumnCount"/> == 0 の場合に <see langword="true"/> を、
        ///     それ以外の場合に <see langword="false"/> を返す。
        /// </remarks>
        public bool IsEmpty { get; }

        /// <summary>行数</summary>
        public int RowCount { get; }

        /// <summary>列数</summary>
        public int ColumnCount { get; }

        /// <summary>
        ///     総数
        /// </summary>
        /// <remarks>
        ///     <see cref="RowCount"/> * <see cref="ColumnCount"/> を返す。
        /// </remarks>
        public int AllCount { get; }

        /// <summary>検証処理実装</summary>
        public ITwoDimensionalListValidator<TRow, TItem> Validator { get; }

        /// <summary>
        /// すべての行要素に対し <see cref="INotifyPropertyChanged"/> イベントを登録する。
        /// </summary>
        /// <remarks>
        /// <para>
        /// このメソッドで登録したイベントは、要素がリストから除去されるときに同時に解除される。
        ///     また、新規行データが追加された場合には自動でイベントが付与される。
        /// </para>
        /// <para>
        ///     <see cref="AddRowPropertyChanged"/> メソッドで登録したイベントを任意のタイミングで解除するには
        ///     <see cref="RemoveRowPropertyChanged"/> を実行する。
        /// </para>
        /// </remarks>
        /// <param name="handler">登録するイベント</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="handler"/> が <see langword="null"/> の場合。
        /// </exception>
        public void AddRowPropertyChanged(PropertyChangedEventHandler handler);

        /// <summary>
        /// すべての行要素にから登録した <see cref="INotifyPropertyChanged"/> イベントを解除する。
        /// </summary>
        /// <remarks>
        /// <para>
        /// <paramref name="handler"/> が <see cref="AddRowPropertyChanged"/> を通して登録されたものでない場合はなにもしない。
        /// </para>
        /// </remarks>
        /// <param name="handler">解除するイベント</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="handler"/> が <see langword="null"/> の場合。
        /// </exception>
        public void RemoveRowPropertyChanged(PropertyChangedEventHandler handler);

        /// <summary>
        /// すべての行要素に対し <see cref="INotifyCollectionChanged.CollectionChanged"/> イベントを登録する。
        /// </summary>
        /// <remarks>
        /// <para>
        ///     このメソッドで登録したイベントは、要素がリストから除去されるときに同時に解除される。
        ///     また、新規行データが追加された場合には自動でイベントが付与される。
        /// </para>
        /// <para>
        ///     AddRowCollectionChanging メソッドで登録したイベントを任意のタイミングで解除するには
        ///     RemoveRowCollectionChanging を実行する。
        /// </para>
        /// </remarks>
        /// <param name="handler">登録するイベント</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="handler"/> が <see langword="null"/> の場合。
        /// </exception>
        public void AddRowCollectionChanged(NotifyCollectionChangedEventHandler handler);

        /// <summary>
        /// すべての行要素から登録した <see cref="INotifyCollectionChanged.CollectionChanged"/> イベントを解除する。
        /// </summary>
        /// <remarks>
        /// <para>
        /// <paramref name="handler"/> が <see cref="AddRowCollectionChanged"/> を通して登録されたものでない場合はなにもしない。
        /// </para>
        /// </remarks>
        /// <param name="handler">解除するイベント</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="handler"/> が <see langword="null"/> の場合。
        /// </exception>
        public void RemoveRowCollectionChanged(NotifyCollectionChangedEventHandler handler);

        /// <summary>
        ///     行容量最大値を返す。
        /// </summary>
        /// <returns>行容量最大値</returns>
        public int GetMaxRowCapacity();

        /// <summary>
        ///     行容量最小値を返す。
        /// </summary>
        /// <returns>行容量最小値</returns>
        public int GetMinRowCapacity();

        /// <summary>
        ///     列容量最大値を返す。
        /// </summary>
        /// <returns>列容量最大値</returns>
        public int GetMaxColumnCapacity();

        /// <summary>
        ///     列容量最小値を返す。
        /// </summary>
        /// <returns>列容量最小値</returns>
        public int GetMinColumnCapacity();

        #region Validate

        #endregion

        #region CRUD Core

        /// <summary>
        ///     <see cref="TwoDimensionalListInterfaceExtension.GetRowRange{TRow,TItem}"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="TwoDimensionalListInterfaceExtension.GetRowRange{TRow,TItem}" path="param"/>
        public IEnumerable<TRow> GetRowRangeCore(int rowIndex, int rowCount);

        /// <summary>
        ///     <see cref="TwoDimensionalListInterfaceExtension.GetColumnRange{TRow,TItem}"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="TwoDimensionalListInterfaceExtension.GetColumnRange{TRow,TItem}" path="param"/>
        public IEnumerable<IEnumerable<TItem>> GetColumnRangeCore(int columnIndex, int columnCount);

        /// <summary>
        ///     <see cref="TwoDimensionalListInterfaceExtension.GetItem{TRow,TItem}"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="TwoDimensionalListInterfaceExtension.GetItem{TRow,TItem}" path="param"/>
        public IEnumerable<IEnumerable<TItem>> GetItemCore(
            int rowIndex,
            int rowCount,
            int columnIndex,
            int columnCount
        );

        /// <summary>
        ///     <see cref="TwoDimensionalListInterfaceExtension.SetRowRange{TRow,TItem}"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="TwoDimensionalListInterfaceExtension.SetRowRange{TRow,TItem}" path="param"/>
        public void SetRowRangeCore(int rowIndex, IEnumerable<TRow> items);

        /// <summary>
        ///     <see cref="TwoDimensionalListInterfaceExtension.SetColumnRange{TRow,TItem}"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="TwoDimensionalListInterfaceExtension.SetColumnRange{TRow,TItem}" path="param"/>
        public void SetColumnRangeCore(int columnIndex, IEnumerable<IEnumerable<TItem>> items);

        /// <summary>
        ///     <see cref="TwoDimensionalListInterfaceExtension.SetItem{TRow,TItem}"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="TwoDimensionalListInterfaceExtension.SetItem{TRow,TItem}" path="param"/>
        public void SetItemCore(
            int rowIndex,
            int columnIndex,
            TItem item
        );

        /// <summary>
        ///     <see cref="TwoDimensionalListInterfaceExtension.AddRowRange{TRow,TItem}"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="TwoDimensionalListInterfaceExtension.AddRowRange{TRow,TItem}" path="param"/>
        public void AddRowRangeCore(IEnumerable<TRow> items);

        /// <summary>
        ///     <see cref="TwoDimensionalListInterfaceExtension.AddColumnRange{TRow,TItem}"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="TwoDimensionalListInterfaceExtension.AddColumnRange{TRow,TItem}" path="param"/>
        public void AddColumnRangeCore(IEnumerable<IEnumerable<TItem>> items);

        /// <summary>
        ///     <see cref="TwoDimensionalListInterfaceExtension.InsertRowRange{TRow,TItem}"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="TwoDimensionalListInterfaceExtension.InsertRowRange{TRow,TItem}" path="param"/>
        public void InsertRowRangeCore(int rowIndex, IEnumerable<TRow> items);

        /// <summary>
        ///     <see cref="TwoDimensionalListInterfaceExtension.InsertColumnRange{TRow,TItem}"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="TwoDimensionalListInterfaceExtension.InsertColumnRange{TRow,TItem}" path="param"/>
        public void InsertColumnRangeCore(int columnIndex, IEnumerable<IEnumerable<TItem>> items);

        /// <summary>
        ///     <see cref="TwoDimensionalListInterfaceExtension.OverwriteRow{TRow,TItem}"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="TwoDimensionalListInterfaceExtension.OverwriteRow{TRow,TItem}" path="param"/>
        public void OverwriteRowCore(int rowIndex, IEnumerable<TRow> items);

        /// <summary>
        ///     <see cref="TwoDimensionalListInterfaceExtension.OverwriteColumn{TRow,TItem}"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="TwoDimensionalListInterfaceExtension.OverwriteColumn{TRow,TItem}" path="param"/>
        public void OverwriteColumnCore(int columnIndex, IEnumerable<IEnumerable<TItem>> items);

        /// <summary>
        ///     <see cref="TwoDimensionalListInterfaceExtension.MoveRowRange{TRow,TItem}"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="TwoDimensionalListInterfaceExtension.MoveRowRange{TRow,TItem}" path="param"/>
        public void MoveRowRangeCore(int oldRowIndex, int newRowIndex, int count);

        /// <summary>
        ///     <see cref="TwoDimensionalListInterfaceExtension.MoveColumnRange{TRow,TItem}"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="TwoDimensionalListInterfaceExtension.MoveColumnRange{TRow,TItem}" path="param"/>
        public void MoveColumnRangeCore(int oldColumnIndex, int newColumnIndex, int count);

        /// <summary>
        ///     <see cref="TwoDimensionalListInterfaceExtension.RemoveRowRange{TRow,TItem}"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="TwoDimensionalListInterfaceExtension.RemoveRowRange{TRow,TItem}" path="param"/>
        public void RemoveRowRangeCore(int rowIndex, int count);

        /// <summary>
        ///     <see cref="TwoDimensionalListInterfaceExtension.RemoveColumnRange{TRow,TItem}"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="TwoDimensionalListInterfaceExtension.RemoveColumnRange{TRow,TItem}" path="param"/>
        public void RemoveColumnRangeCore(int columnIndex, int count);

        /// <summary>
        ///     <see cref="TwoDimensionalListInterfaceExtension.AdjustLength{TRow,TItem}"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="TwoDimensionalListInterfaceExtension.AdjustLength{TRow,TItem}" path="param"/>
        public void AdjustLengthCore(int rowLength, int columnLength);

        /// <summary>
        ///     <see cref="TwoDimensionalListInterfaceExtension.Reset{TRow,TItem}"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="TwoDimensionalListInterfaceExtension.Reset{TRow,TItem}" path="param"/>
        public void ResetCore(IEnumerable<TRow> rows);

        /// <summary>
        ///     <see cref="TwoDimensionalListInterfaceExtension.Clear{TRow,TItem}"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="TwoDimensionalListInterfaceExtension.Clear{TRow,TItem}" path="param"/>
        public void ClearCore();

        #endregion

        /// <summary>
        ///     自身の全要素を簡易コピーした二次元配列を返す。
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         返す二次元配列の状態は <paramref name="isTranspose"/> によって変化する。<br/>
        ///         <paramref name="isTranspose"/> が <see langword="false"/> の場合、
        ///         自身の要素をそのまま返す。<br/>
        ///         <paramref name="isTranspose"/> が <see langword="true"/> の場合、
        ///         自身を転置した状態の要素を返す。<br/>
        ///     </para>
        /// </remarks>
        /// <param name="isTranspose">転置フラグ</param>
        /// <returns>自身の全要素簡易コピー配列</returns>
        public TItem[][] ToTwoDimensionalArray(bool isTranspose = false);
    }

    internal static class TwoDimensionalListInterfaceExtension
    {
        #region CRUD

        /// <summary>
        ///     指定行の要素を取得する。
        /// </summary>
        /// <param name="list">list</param>
        /// <param name="rowIndex">[Range(0, <see cref="ITwoDimensionalList{TRow,ITtem}.RowCount"/> - 1)] 行インデックス</param>
        /// <returns>指定行範囲の要素簡易コピーリスト</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="rowIndex"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">有効な範囲外の要素を取得しようとした場合。</exception>
        public static TRow GetRow<TRow, TItem>(this ITwoDimensionalList<TRow, TItem> list, int rowIndex)
            where TRow : IEnumerable<TItem>
            => list.GetRowRange(rowIndex, 1).First();

        /// <summary>
        ///     指定範囲の要素を簡易コピーしたリストを取得する。
        /// </summary>
        /// <param name="list">list</param>
        /// <param name="rowIndex">[Range(0, <see cref="ITwoDimensionalList{TRow,TItem}.RowCount"/> - 1)] 行インデックス</param>
        /// <param name="rowCount">[Range(0, <see cref="ITwoDimensionalList{TRow,TItem}.RowCount"/>)] 行数</param>
        /// <returns>指定行範囲の要素簡易コピーリスト</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="rowIndex"/>, <paramref name="rowCount"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">有効な範囲外の要素を取得しようとした場合。</exception>
        public static IEnumerable<TRow> GetRowRange<TRow, TItem>(
            this ITwoDimensionalList<TRow, TItem> list,
            int rowIndex,
            int rowCount
        )
            where TRow : IEnumerable<TItem>
        {
            list.ValidateGetRowRange(rowIndex, rowCount);
            return list.GetRowRangeCore(rowIndex, rowCount);
        }

        /// <summary>
        ///     指定範囲の要素を簡易コピーしたリストを取得する。
        /// </summary>
        /// <param name="list">list</param>
        /// <param name="columnIndex">[Range(0, <see cref="ITwoDimensionalList{TRow,ITtem}.ColumnCount"/> - 1] 列インデックス</param>
        /// <returns>指定列範囲の要素簡易コピーリスト</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="columnIndex"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">有効な範囲外の要素を取得しようとした場合。</exception>
        public static IEnumerable<TItem> GetColumn<TRow, TItem>(
            this ITwoDimensionalList<TRow, TItem> list,
            int columnIndex
        )
            where TRow : IEnumerable<TItem>
            => list.GetColumnRange(columnIndex, 1).First();

        /// <summary>
        ///     指定範囲の要素を簡易コピーしたリストを取得する。
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         取得結果の外側シーケンスは列要素を、内側シーケンスは行要素を示す。
        ///     </para>
        /// </remarks>
        /// <param name="list">list</param>
        /// <param name="columnIndex">[Range(0, <see cref="ITwoDimensionalList{TRow,TItem}.ColumnCount"/> - 1] 列インデックス</param>
        /// <param name="columnCount">[Range(0, <see cref="ITwoDimensionalList{TRow,TItem}.ColumnCount"/>)] 列数</param>
        /// <returns>指定列範囲の要素簡易コピーリスト</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="columnIndex"/>, <paramref name="columnCount"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">有効な範囲外の要素を取得しようとした場合。</exception>
        public static IEnumerable<IEnumerable<TItem>> GetColumnRange<TRow, TItem>(
            this ITwoDimensionalList<TRow, TItem> list,
            int columnIndex,
            int columnCount
        )
            where TRow : IEnumerable<TItem>
        {
            list.ValidateGetColumnRange(columnIndex, columnCount);
            return list.GetColumnRangeCore(columnIndex, columnCount);
        }

        /// <summary>
        ///     指定範囲の要素を簡易コピーしたリストを取得する。
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         取得結果の外側シーケンスは列要素を、内側シーケンスは行要素を示す。
        ///     </para>
        /// </remarks>
        /// <param name="list">list</param>
        /// <param name="rowIndex">[Range(0, <see cref="ITwoDimensionalList{TRow,TItem}.RowCount"/> - 1)] 行インデックス</param>
        /// <param name="rowCount">[Range(0, <see cref="ITwoDimensionalList{TRow,TItem}.RowCount"/>)] 行数</param>
        /// <param name="columnIndex">[Range(0, <see cref="ITwoDimensionalList{TRow,TItem}.ColumnCount"/> - 1] 列インデックス</param>
        /// <param name="columnCount">[Range(0, <see cref="ITwoDimensionalList{TRow,TItem}.ColumnCount"/>)] 列数</param>
        /// <returns>指定範囲の要素簡易コピーリスト</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="columnIndex"/>, <paramref name="columnCount"/>,
        ///     <paramref name="rowIndex"/>, <paramref name="rowCount"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">有効な範囲外の要素を取得しようとした場合。</exception>
        public static IEnumerable<IEnumerable<TItem>> GetItem<TRow, TItem>(
            this ITwoDimensionalList<TRow, TItem> list,
            int rowIndex,
            int rowCount,
            int columnIndex,
            int columnCount
        )
            where TRow : IEnumerable<TItem>
        {
            list.ValidateGetItem(rowIndex, rowCount, columnIndex, columnCount);
            return list.GetItemCore(rowIndex, rowCount, columnIndex, columnCount);
        }

        /// <summary>
        ///     リストの指定した要素を更新する。
        /// </summary>
        /// <param name="list">list</param>
        /// <param name="rowIndex">[Range(0, <see cref="ITwoDimensionalList{TRow,ITtem}.RowCount"/> - 1)] 更新開始行インデックス</param>
        /// <param name="item">更新要素</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="item"/> が <see langword="null"/> の場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="rowIndex"/>が指定範囲外の場合。</exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="item"/> の要素数が <see cref="ITwoDimensionalList{TRow,ITtem}.ColumnCount"/> と一致しない場合、
        ///     または 有効な範囲外の要素を編集しようとした場合。
        /// </exception>
        public static void SetRow<TRow, TItem>(
            this ITwoDimensionalList<TRow, TItem> list,
            int rowIndex,
            TRow item
        )
            where TRow : IEnumerable<TItem>
            => list.SetRowRange(rowIndex, new[] { item });

        /// <summary>
        ///     リストの連続した行要素を更新する。
        /// </summary>
        /// <param name="list">list</param>
        /// <param name="rowIndex">[Range(0, <see cref="ITwoDimensionalList{TRow,TItem}.RowCount"/> - 1)] 更新開始行インデックス</param>
        /// <param name="items">更新要素</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="items"/> が <see langword="null"/> の場合、
        ///     または <paramref name="items"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="rowIndex"/>が指定範囲外の場合。</exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="items"/> のいずれかの要素について 要素数が
        ///     <see cref="ITwoDimensionalList{TRow,TItem}.ColumnCount"/> と一致しない場合、
        ///     または 有効な範囲外の要素を編集しようとした場合。
        /// </exception>
        public static void SetRowRange<TRow, TItem>(
            this ITwoDimensionalList<TRow, TItem> list,
            int rowIndex,
            IEnumerable<TRow> items
        )
            where TRow : IEnumerable<TItem>
        {
            list.ValidateSetRowRange(rowIndex, items);
            list.SetRowRangeCore(rowIndex, items);
        }

        /// <summary>
        ///     リストの指定した列要素を更新する。
        /// </summary>
        /// <param name="columnIndex">
        ///     [Range(0, <see cref="ITwoDimensionalList{TRow,ITtem}.ColumnCount"/> - 1)]
        ///     更新開始列インデックス
        /// </param>
        /// <param name="list">list</param>
        /// <param name="items">更新要素</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="items"/> が <see langword="null"/> の場合、
        ///     または <paramref name="items"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="columnIndex"/>が指定範囲外の場合。</exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="items"/> の要素数が <see cref="ITwoDimensionalList{TRow,ITtem}.RowCount"/> と一致しない場合、
        ///     または 有効な範囲外の要素を編集しようとした場合。
        /// </exception>
        public static void SetColumn<TRow, TItem>(
            this ITwoDimensionalList<TRow, TItem> list,
            int columnIndex,
            IEnumerable<TItem> items
        )
            where TRow : IEnumerable<TItem>
            => list.SetColumnRange(columnIndex, new[] { items });

        /// <summary>
        ///     リストの連続した列要素を更新する。
        /// </summary>
        /// <param name="list">list</param>
        /// <param name="columnIndex">
        ///     [Range(0, <see cref="ITwoDimensionalList{TRow,TItem}.ColumnCount"/> - 1)]
        ///     更新開始列インデックス
        /// </param>
        /// <param name="items">更新要素</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="items"/> が <see langword="null"/> の場合、
        ///     または <paramref name="items"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="columnIndex"/>が指定範囲外の場合。</exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="items"/> のいずれかの要素について 要素数が
        ///     <see cref="ITwoDimensionalList{TRow,TItem}.RowCount"/> と一致しない場合、
        ///     または 有効な範囲外の要素を編集しようとした場合。
        /// </exception>
        public static void SetColumnRange<TRow, TItem>(
            this ITwoDimensionalList<TRow, TItem> list,
            int columnIndex,
            IEnumerable<IEnumerable<TItem>> items
        )
            where TRow : IEnumerable<TItem>
        {
            list.ValidateSetColumnRange(columnIndex, items);
            list.SetColumnRangeCore(columnIndex, items);
        }

        /// <summary>
        ///     リストの連続した列要素を更新する。
        /// </summary>
        /// <param name="list">list</param>
        /// <param name="rowIndex">
        ///     [Range(0, <see cref="ITwoDimensionalList{TRow,TItem}.RowCount"/> - 1)]
        ///     更新開始列インデックス
        /// </param>
        /// <param name="columnIndex">
        ///     [Range(0, <see cref="ITwoDimensionalList{TRow,TItem}.ColumnCount"/> - 1)]
        ///     更新開始列インデックス
        /// </param>
        /// <param name="item">更新要素</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="item"/> が <see langword="null"/> の場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="rowIndex"/>, <paramref name="columnIndex"/>が指定範囲外の場合。
        /// </exception>
        public static void SetItem<TRow, TItem>(
            this ITwoDimensionalList<TRow, TItem> list,
            int rowIndex,
            int columnIndex,
            TItem item
        )
            where TRow : IEnumerable<TItem>
        {
            list.ValidateSetItem(rowIndex, columnIndex, item);
            list.SetItemCore(rowIndex, columnIndex, item);
        }

        /// <summary>
        ///     行の末尾に要素を追加する。
        /// </summary>
        /// <param name="list">list</param>
        /// <param name="item">追加する要素</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="item"/> が <see langword="null"/> の場合、
        ///     または <paramref name="item"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="item"/> の要素数が <see cref="ITwoDimensionalList{TRow,ITtem}.ColumnCount"/> と異なる場合。
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     操作によって行数が <see cref="ITwoDimensionalList{TRow,ITtem}.GetMaxRowCapacity"/> を上回る場合。
        /// </exception>
        public static void AddRow<TRow, TItem>(
            this ITwoDimensionalList<TRow, TItem> list,
            TRow item
        )
            where TRow : IEnumerable<TItem>
            => list.AddRowRange(new[] { item });

        /// <summary>
        ///     行の末尾に要素を追加する。
        /// </summary>
        /// <param name="list">list</param>
        /// <param name="items">追加する要素</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="items"/> が <see langword="null"/> の場合、
        ///     または <paramref name="items"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="items"/> の要素数が <see cref="ITwoDimensionalList{TRow,TItem}.ColumnCount"/> と異なる場合。
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     操作によって行数が <see cref="ITwoDimensionalList{TRow,TItem}.GetMaxRowCapacity"/> を上回る場合。
        /// </exception>
        public static void AddRowRange<TRow, TItem>(this ITwoDimensionalList<TRow, TItem> list, IEnumerable<TRow> items)
            where TRow : IEnumerable<TItem>
        {
            list.ValidateAddRowRange(items);
            list.AddRowRangeCore(items);
        }

        /// <summary>
        ///     列の末尾に要素を追加する。
        /// </summary>
        /// <param name="list">list</param>
        /// <param name="items">追加する要素</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="items"/> が <see langword="null"/> の場合、
        ///     または <paramref name="items"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="items"/> の要素数が <see cref="ITwoDimensionalList{TRow,ITtem}.RowCount"/> と異なる場合。
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     操作によって列数が <see cref="ITwoDimensionalList{TRow,TItem}.GetMaxColumnCapacity"/> を上回る場合。
        /// </exception>
        public static void AddColumn<TRow, TItem>(
            this ITwoDimensionalList<TRow, TItem> list,
            IEnumerable<TItem> items
        )
            where TRow : IEnumerable<TItem>
            => list.AddColumnRange(new[] { items });

        /// <summary>
        ///     列の末尾に要素を追加する。
        /// </summary>
        /// <param name="list">list</param>
        /// <param name="items">追加する要素</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="items"/> が <see langword="null"/> の場合、
        ///     または <paramref name="items"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="items"/> のいずれかの要素の要素数が <see cref="ITwoDimensionalList{TRow,TItem}.RowCount"/> と異なる場合。
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     操作によって列数が <see cref="ITwoDimensionalList{TRow,TItem}.GetMaxColumnCapacity"/> を上回る場合。
        /// </exception>
        public static void AddColumnRange<TRow, TItem>(
            this ITwoDimensionalList<TRow, TItem> list,
            IEnumerable<IEnumerable<TItem>> items
        )
            where TRow : IEnumerable<TItem>
        {
            list.ValidateAddColumnRange(items);
            list.AddColumnRangeCore(items);
        }

        /// <summary>
        ///     指定した行インデックスの位置に要素を挿入する。
        /// </summary>
        /// <param name="list">list</param>
        /// <param name="rowIndex">[Range(0, <see cref="ITwoDimensionalList{TRow,ITtem}.RowCount"/>)] 行インデックス</param>
        /// <param name="item">挿入する要素</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="item"/> が <see langword="null"/> の場合、
        ///     または <paramref name="item"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="item"/> の要素数が <see cref="ITwoDimensionalList{TRow,ITtem}.ColumnCount"/> と異なる場合。
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     操作によって行数が <see cref="ITwoDimensionalList{TRow,ITtem}.GetMaxRowCapacity"/> を上回る場合。
        /// </exception>
        public static void InsertRow<TRow, TItem>(
            this ITwoDimensionalList<TRow, TItem> list,
            int rowIndex,
            TRow item
        )
            where TRow : IEnumerable<TItem>
            => list.InsertRowRange(rowIndex, new[] { item });

        /// <summary>
        ///     指定した行インデックスの位置に要素を挿入する。
        /// </summary>
        /// <param name="list">list</param>
        /// <param name="rowIndex">[Range(0, <see cref="ITwoDimensionalList{TRow,TItem}.RowCount"/>)] 行インデックス</param>
        /// <param name="items">挿入する要素</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="items"/> が <see langword="null"/> の場合、
        ///     または <paramref name="items"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="items"/> の要素数が <see cref="ITwoDimensionalList{TRow,TItem}.ColumnCount"/> と異なる場合。
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     操作によって行数が <see cref="ITwoDimensionalList{TRow,ITtem}.GetMaxRowCapacity"/> を上回る場合。
        /// </exception>
        public static void InsertRowRange<TRow, TItem>(
            this ITwoDimensionalList<TRow, TItem> list,
            int rowIndex,
            IEnumerable<TRow> items
        )
            where TRow : IEnumerable<TItem>
        {
            list.ValidateInsertRowRange(rowIndex, items);
            list.InsertRowRangeCore(rowIndex, items);
        }

        /// <summary>
        ///     指定した列インデックスの位置に要素を挿入する。
        /// </summary>
        /// <param name="list">list</param>
        /// <param name="columnIndex">[Range(0, <see cref="ITwoDimensionalList{TRow,ITtem}.ColumnCount"/>)] 列インデックス</param>
        /// <param name="items">挿入する要素</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="items"/> が <see langword="null"/> の場合、
        ///     または <paramref name="items"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="items"/> の要素数が <see cref="ITwoDimensionalList{TRow,ITtem}.RowCount"/> と異なる場合。
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     操作によって列数が <see cref="ITwoDimensionalList{TRow,TItem}.GetMaxColumnCapacity"/> を上回る場合。
        /// </exception>
        public static void InsertColumn<TRow, TItem>(
            this ITwoDimensionalList<TRow, TItem> list,
            int columnIndex,
            IEnumerable<TItem> items
        )
            where TRow : IEnumerable<TItem>
            => list.InsertColumnRange(columnIndex, new[] { items });

        /// <summary>
        ///     指定した列インデックスの位置に要素を挿入する。
        /// </summary>
        /// <param name="list">list</param>
        /// <param name="columnIndex">[Range(0, <see cref="ITwoDimensionalList{TRow,TItem}.ColumnCount"/>)] 列インデックス</param>
        /// <param name="items">挿入する要素</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="items"/> が <see langword="null"/> の場合、
        ///     または <paramref name="items"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="items"/> のいずれかの要素の要素数が <see cref="ITwoDimensionalList{TRow,TItem}.RowCount"/> と異なる場合。
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     操作によって列数が <see cref="ITwoDimensionalList{TRow,TItem}.GetMaxColumnCapacity"/> を上回る場合。
        /// </exception>
        public static void InsertColumnRange<TRow, TItem>(
            this ITwoDimensionalList<TRow, TItem> list,
            int columnIndex,
            IEnumerable<IEnumerable<TItem>> items
        )
            where TRow : IEnumerable<TItem>
        {
            list.ValidateInsertColumnRange(columnIndex, items);
            list.InsertColumnRangeCore(columnIndex, items);
        }

        /// <summary>
        ///     指定した行インデックスを起点として、要素の上書き/追加を行う。
        /// </summary>
        /// <param name="list">list</param>
        /// <remarks>
        ///     サンプルコードは <seealso cref="IRestrictedCapacityList{T}.Overwrite"/> 参照。
        /// </remarks>
        /// <param name="rowIndex">[Range(0, <see cref="ITwoDimensionalList{TRow,TItem}.RowCount"/>)] 行インデックス</param>
        /// <param name="items">上書き/追加リスト</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="items"/> が <see langword="null"/> の場合。
        ///     または <paramref name="items"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="rowIndex"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="items"/> のいずれかの要素の要素数が
        ///     <see cref="ITwoDimensionalList{TRow,TItem}.RowCount"/> と異なる場合。
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     操作によって行数が <see cref="ITwoDimensionalList{TRow,TItem}.GetMaxRowCapacity"/> を上回る場合。
        /// </exception>
        public static void OverwriteRow<TRow, TItem>(
            this ITwoDimensionalList<TRow, TItem> list,
            int rowIndex,
            IEnumerable<TRow> items
        )
            where TRow : IEnumerable<TItem>
        {
            list.ValidateOverwriteRow(rowIndex, items);
            list.OverwriteRowCore(rowIndex, items);
        }

        /// <summary>
        ///     指定した列インデックスを起点として、要素の上書き/追加を行う。
        /// </summary>
        /// <param name="list">list</param>
        /// <remarks>
        ///     サンプルコードは <seealso cref="IRestrictedCapacityList{T}.Overwrite"/> 参照。
        /// </remarks>
        /// <param name="columnIndex">[Range(0, <see cref="ITwoDimensionalList{TRow,TItem}.ColumnCount"/>)] 列インデックス</param>
        /// <param name="items">上書き/追加リスト</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="items"/> が <see langword="null"/> の場合。
        ///     または <paramref name="items"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="columnIndex"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="items"/> のいずれかの要素の要素数が
        ///     <see cref="ITwoDimensionalList{TRow,TItem}.RowCount"/> と異なる場合。
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     操作によって列数が <see cref="ITwoDimensionalList{TRow,TItem}.GetMaxColumnCapacity"/> を上回る場合。
        /// </exception>
        public static void OverwriteColumn<TRow, TItem>(
            this ITwoDimensionalList<TRow, TItem> list,
            int columnIndex,
            IEnumerable<IEnumerable<TItem>> items
        )
            where TRow : IEnumerable<TItem>
        {
            list.ValidateOverwriteColumn(columnIndex, items);
            list.OverwriteColumnCore(columnIndex, items);
        }

        /// <summary>
        ///     指定した行番号の項目をコレクション内の新しい場所へ移動する。
        /// </summary>
        /// <param name="list">list</param>
        /// <param name="oldRowIndex">
        ///     [Range(0, <see cref="ITwoDimensionalList{TRow,ITtem}.RowCount"/> - 1)]
        ///     移動する項目の行番号開始位置
        /// </param>
        /// <param name="newRowIndex">
        ///     [Range(0, <see cref="ITwoDimensionalList{TRow,ITtem}.RowCount"/> - 1)]
        ///     移動先の行番号開始位置
        /// </param>
        /// <exception cref="InvalidOperationException">
        ///     自身の行数が0の場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="oldRowIndex"/>, <paramref name="newRowIndex"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">有効な範囲外の要素を移動しようとした場合。</exception>
        public static void MoveRow<TRow, TItem>(
            this ITwoDimensionalList<TRow, TItem> list,
            int oldRowIndex,
            int newRowIndex
        )
            where TRow : IEnumerable<TItem>
            => list.MoveRowRange(oldRowIndex, newRowIndex, 1);

        /// <summary>
        ///     指定した行番号から始まる連続した項目をコレクション内の新しい場所へ移動する。
        /// </summary>
        /// <param name="list">list</param>
        /// <param name="oldRowIndex">
        ///     [Range(0, <see cref="ITwoDimensionalList{TRow,TItem}.RowCount"/> - 1)]
        ///     移動する項目の行番号開始位置
        /// </param>
        /// <param name="newRowIndex">
        ///     [Range(0, <see cref="ITwoDimensionalList{TRow,TItem}.RowCount"/> - 1)]
        ///     移動先の行番号開始位置
        /// </param>
        /// <param name="count">
        ///     [Range(0, <see cref="ITwoDimensionalList{TRow,TItem}.RowCount"/>)]
        ///     移動させる要素数
        /// </param>
        /// <exception cref="InvalidOperationException">
        ///     自身の行数が0の場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="oldRowIndex"/>, <paramref name="newRowIndex"/>, <paramref name="count"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">有効な範囲外の要素を移動しようとした場合。</exception>
        public static void MoveRowRange<TRow, TItem>(
            this ITwoDimensionalList<TRow, TItem> list,
            int oldRowIndex,
            int newRowIndex,
            int count
        )
            where TRow : IEnumerable<TItem>
        {
            list.ValidateMoveRowRange(oldRowIndex, newRowIndex, count);
            list.MoveRowRangeCore(oldRowIndex, newRowIndex, count);
        }

        /// <summary>
        ///     指定した列番号の項目をコレクション内の新しい場所へ移動する。
        /// </summary>
        /// <param name="list">list</param>
        /// <param name="oldColumnIndex">
        ///     [Range(0, <see cref="ITwoDimensionalList{TRow,ITtem}.ColumnCount"/> - 1)]
        ///     移動する項目の列番号開始位置
        /// </param>
        /// <param name="newColumnIndex">
        ///     [Range(0, <see cref="ITwoDimensionalList{TRow,ITtem}.ColumnCount"/> - 1)]
        ///     移動先の列番号開始位置
        /// </param>
        /// <exception cref="InvalidOperationException">
        ///     自身の列数が0の場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="oldColumnIndex"/>, <paramref name="newColumnIndex"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">有効な範囲外の要素を移動しようとした場合。</exception>
        public static void MoveColumn<TRow, TItem>(
            this ITwoDimensionalList<TRow, TItem> list,
            int oldColumnIndex,
            int newColumnIndex
        )
            where TRow : IEnumerable<TItem>
            => list.MoveColumnRange(oldColumnIndex, newColumnIndex, 1);

        /// <summary>
        ///     指定した列番号から始まる連続した項目をコレクション内の新しい場所へ移動する。
        /// </summary>
        /// <param name="list">list</param>
        /// <param name="oldColumnIndex">
        ///     [Range(0, <see cref="ITwoDimensionalList{TRow,TItem}.ColumnCount"/> - 1)]
        ///     移動する項目の列番号開始位置
        /// </param>
        /// <param name="newColumnIndex">
        ///     [Range(0, <see cref="ITwoDimensionalList{TRow,TItem}.ColumnCount"/> - 1)]
        ///     移動先の列番号開始位置
        /// </param>
        /// <param name="count">
        ///     [Range(0, <see cref="ITwoDimensionalList{TRow,TItem}.ColumnCount"/>)]
        ///     移動させる要素数
        /// </param>
        /// <exception cref="InvalidOperationException">
        ///     自身の要素数が0の場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="oldColumnIndex"/>, <paramref name="newColumnIndex"/>, <paramref name="count"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">有効な範囲外の要素を移動しようとした場合。</exception>
        public static void MoveColumnRange<TRow, TItem>(
            this ITwoDimensionalList<TRow, TItem> list,
            int oldColumnIndex,
            int newColumnIndex,
            int count
        )
            where TRow : IEnumerable<TItem>
        {
            list.ValidateMoveColumnRange(oldColumnIndex, newColumnIndex, count);
            list.MoveColumnRangeCore(oldColumnIndex, newColumnIndex, count);
        }

        /// <summary>
        ///     指定した行インデックスの要素を削除する。
        /// </summary>
        /// <param name="list">list</param>
        /// <param name="rowIndex">[Range(0, <see cref="ITwoDimensionalList{TRow,ITtem}.RowCount"/> - 1)] インデックス</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="rowIndex"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     操作によって行数が <see cref="ITwoDimensionalList{TRow,ITtem}.GetMinRowCapacity"/> を下回る場合。
        /// </exception>
        public static void RemoveRow<TRow, TItem>(
            this ITwoDimensionalList<TRow, TItem> list,
            int rowIndex
        )
            where TRow : IEnumerable<TItem>
            => list.RemoveRowRange(rowIndex, 1);

        /// <summary>
        ///     指定した範囲の行要素を削除する。
        /// </summary>
        /// <param name="list">list</param>
        /// <param name="rowIndex">[Range(0, <see cref="ITwoDimensionalList{TRow,TItem}.RowCount"/> - 1)] インデックス</param>
        /// <param name="count">[Range(0, <see cref="ITwoDimensionalList{TRow,TItem}.RowCount"/>)] 削除する行数</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="rowIndex"/>, <paramref name="count"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     有効な範囲外の要素を削除しようとした場合。
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     操作によって行数が <see cref="ITwoDimensionalList{TRow,ITtem}.GetMinRowCapacity"/> を下回る場合。
        /// </exception>
        public static void RemoveRowRange<TRow, TItem>(
            this ITwoDimensionalList<TRow, TItem> list,
            int rowIndex,
            int count
        )
            where TRow : IEnumerable<TItem>
        {
            list.ValidateRemoveRowRange(rowIndex, count);
            list.RemoveRowRangeCore(rowIndex, count);
        }

        /// <summary>
        ///     要素の範囲を削除する。
        /// </summary>
        /// <param name="list">list</param>
        /// <param name="columnIndex">[Range(0, <see cref="ITwoDimensionalList{TRow,ITtem}.ColumnCount"/> - 1)] インデックス</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="columnIndex"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     有効な範囲外の要素を削除しようとした場合。
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     操作によって列数が <see cref="ITwoDimensionalList{TRow,ITtem}.GetMinColumnCapacity"/> を下回る場合。
        /// </exception>
        public static void RemoveColumn<TRow, TItem>(
            this ITwoDimensionalList<TRow, TItem> list,
            int columnIndex
        )
            where TRow : IEnumerable<TItem>
            => list.RemoveColumnRange(columnIndex, 1);

        /// <summary>
        ///     要素の範囲を削除する。
        /// </summary>
        /// <param name="list">list</param>
        /// <param name="columnIndex">[Range(0, <see cref="ITwoDimensionalList{TRow,TItem}.ColumnCount"/> - 1)] インデックス</param>
        /// <param name="count">[Range(0, <see cref="ITwoDimensionalList{TRow,TItem}.ColumnCount"/>)] 削除する列数</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="columnIndex"/>, <paramref name="count"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     有効な範囲外の要素を削除しようとした場合。
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     操作によって列数が <see cref="ITwoDimensionalList{TRow,ITtem}.GetMinColumnCapacity"/> を下回る場合。
        /// </exception>
        public static void RemoveColumnRange<TRow, TItem>(
            this ITwoDimensionalList<TRow, TItem> list,
            int columnIndex,
            int count
        )
            where TRow : IEnumerable<TItem>
        {
            list.ValidateRemoveColumnRange(columnIndex, count);
            list.RemoveColumnRangeCore(columnIndex, count);
        }

        /// <summary>
        ///     行数および列数を指定の数に合わせる。
        /// </summary>
        /// <param name="list">list</param>
        /// <param name="rowLength">
        ///     [Range(<see cref="ITwoDimensionalList{TRow,TItem}.GetMinRowCapacity"/>,
        ///     <see cref="ITwoDimensionalList{TRow,TItem}.GetMaxRowCapacity"/>)]
        ///     調整する行数
        /// </param>
        /// <param name="columnLength">
        ///     [Range(<see cref="ITwoDimensionalList{TRow,TItem}.GetMinColumnCapacity"/>,
        ///     <see cref="ITwoDimensionalList{TRow,TItem}.GetMaxColumnCapacity"/>)]
        ///     調整する列数
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="rowLength"/>, <paramref name="columnLength"/> が指定範囲外の場合。
        /// </exception>
        public static void AdjustLength<TRow, TItem>(
            this ITwoDimensionalList<TRow, TItem> list,
            int rowLength,
            int columnLength
        )
            where TRow : IEnumerable<TItem>
        {
            list.ValidateAdjustLength(rowLength, columnLength);
            list.AdjustLengthCore(rowLength, columnLength);
        }

        /// <summary>
        ///     行数が不足している場合、行数を指定の数に合わせる。
        ///     列数が不足している場合、列数を指定の数に合わせる。
        /// </summary>
        /// <param name="list">list</param>
        /// <param name="rowLength">
        ///     [Range(<see cref="ITwoDimensionalList{TRow,ITtem}.GetMinRowCapacity"/>,
        ///     <see cref="ITwoDimensionalList{TRow,ITtem}.GetMaxRowCapacity"/>)]
        ///     調整する行数
        /// </param>
        /// <param name="columnLength">
        ///     [Range(<see cref="ITwoDimensionalList{TRow,ITtem}.GetMinColumnCapacity"/>,
        ///     <see cref="ITwoDimensionalList{TRow,ITtem}.GetMaxColumnCapacity"/>)]
        ///     調整する列数
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="rowLength"/>, <paramref name="columnLength"/> が指定範囲外の場合。
        /// </exception>
        public static void AdjustLengthIfShort<TRow, TItem>(
            this ITwoDimensionalList<TRow, TItem> list,
            int rowLength,
            int columnLength
        )
            where TRow : IEnumerable<TItem>
            => list.AdjustLength(Math.Max(rowLength, list.RowCount), Math.Max(columnLength, list.ColumnCount));

        /// <summary>
        ///     行数が超過している場合、行数を指定の数に合わせる。
        ///     列数が超過している場合、列数を指定の数に合わせる。
        /// </summary>
        /// <param name="list">list</param>
        /// <param name="rowLength">
        ///     [Range(<see cref="ITwoDimensionalList{TRow,ITtem}.GetMinRowCapacity"/>,
        ///     <see cref="ITwoDimensionalList{TRow,ITtem}.GetMaxRowCapacity"/>)]
        ///     調整する行数
        /// </param>
        /// <param name="columnLength">
        ///     [Range(<see cref="ITwoDimensionalList{TRow,ITtem}.GetMinColumnCapacity"/>,
        ///     <see cref="ITwoDimensionalList{TRow,ITtem}.GetMaxColumnCapacity"/>)]
        ///     調整する列数
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="rowLength"/>, <paramref name="columnLength"/> が指定範囲外の場合。
        /// </exception>
        public static void AdjustLengthIfLong<TRow, TItem>(
            this ITwoDimensionalList<TRow, TItem> list,
            int rowLength,
            int columnLength
        )
            where TRow : IEnumerable<TItem>
            => list.AdjustLength(Math.Min(rowLength, list.RowCount), Math.Min(columnLength, list.ColumnCount));

        /// <summary>
        ///     行数を指定の数に合わせる。
        /// </summary>
        /// <param name="list">list</param>
        /// <param name="rowLength">
        ///     [Range(<see cref="ITwoDimensionalList{TRow,ITtem}.GetMinRowCapacity"/>, <see cref="ITwoDimensionalList{TRow,ITtem}.GetMaxRowCapacity"/>)]
        ///     調整する行数
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="rowLength"/> が指定範囲外の場合。
        /// </exception>
        public static void AdjustRowLength<TRow, TItem>(
            this ITwoDimensionalList<TRow, TItem> list,
            int rowLength
        )
            where TRow : IEnumerable<TItem>
            => list.AdjustLength(rowLength, list.ColumnCount);

        /// <summary>
        ///     行数が不足している場合、行数を指定の数に合わせる。
        /// </summary>
        /// <param name="list">list</param>
        /// <param name="rowLength">
        ///     [Range(<see cref="ITwoDimensionalList{TRow,ITtem}.GetMinRowCapacity"/>, <see cref="ITwoDimensionalList{TRow,ITtem}.GetMaxRowCapacity"/>)]
        ///     調整する行数
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="rowLength"/> が指定範囲外の場合。
        /// </exception>
        public static void AdjustRowLengthIfShort<TRow, TItem>(
            this ITwoDimensionalList<TRow, TItem> list,
            int rowLength
        )
            where TRow : IEnumerable<TItem>
            => list.AdjustLength(Math.Max(rowLength, list.RowCount), list.ColumnCount);

        /// <summary>
        ///     行数が超過している場合、行数を指定の数に合わせる。
        /// </summary>
        /// <param name="list">list</param>
        /// <param name="rowLength">
        ///     [Range(<see cref="ITwoDimensionalList{TRow,ITtem}.GetMinRowCapacity"/>, <see cref="ITwoDimensionalList{TRow,ITtem}.GetMaxRowCapacity"/>)]
        ///     調整する行数
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="rowLength"/> が指定範囲外の場合。
        /// </exception>
        public static void AdjustRowLengthIfLong<TRow, TItem>(
            this ITwoDimensionalList<TRow, TItem> list,
            int rowLength
        )
            where TRow : IEnumerable<TItem>
            => list.AdjustLength(Math.Min(rowLength, list.RowCount), list.ColumnCount);

        /// <summary>
        ///     列数を指定の数に合わせる。
        /// </summary>
        /// <param name="list">list</param>
        /// <param name="columnLength">
        ///     [Range(<see cref="ITwoDimensionalList{TRow,ITtem}.GetMinColumnCapacity"/>, <see cref="ITwoDimensionalList{TRow,ITtem}.GetMaxColumnCapacity"/>)]
        ///     調整する列数
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="columnLength"/> が指定範囲外の場合。
        /// </exception>
        public static void AdjustColumnLength<TRow, TItem>(
            this ITwoDimensionalList<TRow, TItem> list,
            int columnLength
        )
            where TRow : IEnumerable<TItem>
            => list.AdjustLength(list.RowCount, columnLength);

        /// <summary>
        ///     列数が不足している場合、列数を指定の数に合わせる。
        /// </summary>
        /// <param name="list">list</param>
        /// <param name="columnLength">
        ///     [Range(<see cref="ITwoDimensionalList{TRow,ITtem}.GetMinColumnCapacity"/>, <see cref="ITwoDimensionalList{TRow,ITtem}.GetMaxColumnCapacity"/>)]
        ///     調整する列数
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="columnLength"/> が指定範囲外の場合。
        /// </exception>
        public static void AdjustColumnLengthIfShort<TRow, TItem>(
            this ITwoDimensionalList<TRow, TItem> list,
            int columnLength
        )
            where TRow : IEnumerable<TItem>
            => list.AdjustLength(list.RowCount, Math.Max(columnLength, list.ColumnCount));

        /// <summary>
        ///     列数が超過している場合、列数を指定の数に合わせる。
        /// </summary>
        /// <param name="list">list</param>
        /// <param name="columnLength">
        ///     [Range(<see cref="ITwoDimensionalList{TRow,ITtem}.GetMinColumnCapacity"/>, <see cref="ITwoDimensionalList{TRow,ITtem}.GetMaxColumnCapacity"/>)]
        ///     調整する列数
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="columnLength"/> が指定範囲外の場合。
        /// </exception>
        public static void AdjustColumnLengthIfLong<TRow, TItem>(
            this ITwoDimensionalList<TRow, TItem> list,
            int columnLength
        )
            where TRow : IEnumerable<TItem>
            => list.AdjustLength(list.RowCount, Math.Min(columnLength, list.ColumnCount));

        /// <summary>
        ///     要素を与えられた内容で一新する。
        /// </summary>
        /// <param name="list">list</param>
        /// <param name="rows">リストに詰め直す要素</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="rows"/> が <see langword="null"/> の場合、
        ///     または <paramref name="rows"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="rows"/> のすべての要素の要素数が統一されていない場合、
        ///     または <paramref name="rows"/> の行数または列数が不適切な場合。
        /// </exception>
        public static void Reset<TRow, TItem>(this ITwoDimensionalList<TRow, TItem> list, IEnumerable<TRow> rows)
            where TRow : IEnumerable<TItem>
        {
            list.ValidateReset(rows);
            list.ResetCore(rows);
        }

        /// <summary>
        ///     自身を初期化する。
        /// </summary>
        /// <param name="list">list</param>
        public static void Clear<TRow, TItem>(this ITwoDimensionalList<TRow, TItem> list)
            where TRow : IEnumerable<TItem>
        {
            list.ValidateClear();
            list.ClearCore();
        }

        #endregion

        #region Validate

        /// <summary>
        ///     <see cref="GetRow{TRow, TItem}"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="GetRow{TRow,TItem}" path="param"/>
        public static void ValidateGetRow<TRow, TItem>(this ITwoDimensionalList<TRow, TItem> list, int rowIndex)
            where TRow : IEnumerable<TItem>
            => list.ValidateGetRowRange(rowIndex, 1);

        /// <summary>
        ///     <see cref="GetRowRange{TRow,TItem}"/> メソッドの検証処理
        /// </summary>
        /// <inheritdoc cref="GetRowRange{TRow,TItem}" path="param|exception"/>
        public static void ValidateGetRowRange<TRow, TItem>(
            this ITwoDimensionalList<TRow, TItem> list,
            int rowIndex,
            int rowCount
        )
            where TRow : IEnumerable<TItem>
            => list.Validator.GetRow((nameof(rowIndex), rowIndex), (nameof(rowCount), rowCount));

        /// <summary>
        ///     <see cref="GetColumn{TRow, TItem}"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="GetColumn{TRow, TItem}" path="param"/>
        public static void ValidateGetColumn<TRow, TItem>(
            this ITwoDimensionalList<TRow, TItem> list,
            int columnIndex
        )
            where TRow : IEnumerable<TItem>
            => list.ValidateGetColumnRange(columnIndex, 1);

        /// <summary>
        ///     <see cref="GetColumnRange{TRow,TItem}"/> メソッドの検証処理
        /// </summary>
        /// <inheritdoc cref="GetColumnRange{TRow,TItem}" path="param|exception"/>
        public static void ValidateGetColumnRange<TRow, TItem>(
            this ITwoDimensionalList<TRow, TItem> list,
            int columnIndex,
            int columnCount
        )
            where TRow : IEnumerable<TItem>
            => list.Validator.GetColumn((nameof(columnIndex), columnIndex), (nameof(columnCount), columnCount));

        /// <summary>
        ///     <see cref="GetItem{TRow,TItem}"/> メソッドの検証処理
        /// </summary>
        /// <inheritdoc cref="GetItem{TRow,TItem}" path="param|exception"/>
        public static void ValidateGetItem<TRow, TItem>(
            this ITwoDimensionalList<TRow, TItem> list,
            int rowIndex,
            int rowCount,
            int columnIndex,
            int columnCount
        )
            where TRow : IEnumerable<TItem>
            => list.Validator.GetItem(
                (nameof(rowIndex), rowIndex),
                (nameof(rowCount), rowCount),
                (nameof(columnIndex), columnIndex),
                (nameof(columnCount), columnCount)
            );

        /// <summary>
        ///     <see cref="SetRow{TRow, TItem}"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="SetRow{TRow, TItem}" path="param"/>
        public static void ValidateSetRow<TRow, TItem>(
            this ITwoDimensionalList<TRow, TItem> list,
            int rowIndex,
            TRow item
        )
            where TRow : IEnumerable<TItem>
            => list.ValidateSetRowRange(rowIndex, new[] { item });

        /// <summary>
        ///     <see cref="SetRowRange{TRow,TItem}"/> メソッドの検証処理
        /// </summary>
        /// <inheritdoc cref="SetRowRange{TRow,TItem}" path="param|exception"/>
        public static void ValidateSetRowRange<TRow, TItem>(
            this ITwoDimensionalList<TRow, TItem> list,
            int rowIndex,
            IEnumerable<TRow> items
        )
            where TRow : IEnumerable<TItem>
            => list.Validator.SetRow((nameof(rowIndex), rowIndex), (nameof(items), items));

        /// <summary>
        ///     <see cref="SetColumn{TRow, TItem}"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="SetColumn{TRow, TItem}" path="param"/>
        public static void ValidateSetColumn<TRow, TItem>(
            this ITwoDimensionalList<TRow, TItem> list,
            int columnIndex,
            IEnumerable<TItem> items
        )
            where TRow : IEnumerable<TItem>
            => list.ValidateSetColumnRange(columnIndex, new[] { items });

        /// <summary>
        ///     <see cref="SetColumnRange{TRow,TItem}"/> メソッドの検証処理
        /// </summary>
        /// <inheritdoc cref="SetColumnRange{TRow,TItem}" path="param|exception"/>
        public static void ValidateSetColumnRange<TRow, TItem>(
            this ITwoDimensionalList<TRow, TItem> list,
            int columnIndex,
            IEnumerable<IEnumerable<TItem>> items
        )
            where TRow : IEnumerable<TItem>
            => list.Validator.SetColumn((nameof(columnIndex), columnIndex), (nameof(items), items));

        /// <summary>
        ///     <see cref="SetItem{TRow,TItem}"/> メソッドの検証処理
        /// </summary>
        /// <inheritdoc cref="SetItem{TRow,TItem}" path="param|exception"/>
        public static void ValidateSetItem<TRow, TItem>(
            this ITwoDimensionalList<TRow, TItem> list,
            int rowIndex,
            int columnIndex,
            TItem item
        )
            where TRow : IEnumerable<TItem>
            => list.Validator.SetItem(
                (nameof(rowIndex), rowIndex),
                (nameof(columnIndex), columnIndex),
                (nameof(item), item)
            );

        /// <summary>
        ///     <see cref="AddRow{TRow, TItem}"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="AddRow{TRow, TItem}" path="param"/>
        public static void ValidateAddRow<TRow, TItem>(
            this ITwoDimensionalList<TRow, TItem> list,
            TRow item
        )
            where TRow : IEnumerable<TItem>
            => list.ValidateAddRowRange(new[] { item });

        /// <summary>
        ///     <see cref="AddRowRange{TRow,TItem}"/> メソッドの検証処理
        /// </summary>
        /// <inheritdoc cref="AddRowRange{TRow,TItem}" path="param|exception"/>
        public static void ValidateAddRowRange<TRow, TItem>(
            this ITwoDimensionalList<TRow, TItem> list,
            IEnumerable<TRow> items
        )
            where TRow : IEnumerable<TItem> => list.Validator.InsertRow(
            (nameof(list.RowCount), list.RowCount),
            (nameof(items), items)
        );

        /// <summary>
        ///     <see cref="AddColumn{TRow, TItem}"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="AddColumn{TRow, TItem}" path="param"/>
        public static void ValidateAddColumn<TRow, TItem>(
            this ITwoDimensionalList<TRow, TItem> list,
            IEnumerable<TItem> items
        )
            where TRow : IEnumerable<TItem>
            => list.ValidateAddColumnRange(new[] { items });

        /// <summary>
        ///     <see cref="AddColumnRange{TRow,TItem}"/> メソッドの検証処理
        /// </summary>
        /// <inheritdoc cref="AddColumnRange{TRow,TItem}" path="param|exception"/>
        public static void ValidateAddColumnRange<TRow, TItem>(
            this ITwoDimensionalList<TRow, TItem> list,
            IEnumerable<IEnumerable<TItem>> items
        )
            where TRow : IEnumerable<TItem> => list.Validator.InsertColumn(
            (nameof(list.ColumnCount), list.ColumnCount),
            (nameof(items), items)
        );

        /// <summary>
        ///     <see cref="InsertRow{TRow, TItem}"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="InsertRow{TRow, TItem}" path="param"/>
        public static void ValidateInsertRow<TRow, TItem>(
            this ITwoDimensionalList<TRow, TItem> list,
            int rowIndex,
            TRow item
        )
            where TRow : IEnumerable<TItem>
            => list.ValidateInsertRowRange(rowIndex, new[] { item });

        /// <summary>
        ///     <see cref="InsertRowRange{TRow,TItem}"/> メソッドの検証処理
        /// </summary>
        /// <inheritdoc cref="InsertRowRange{TRow,TItem}" path="param|exception"/>
        public static void ValidateInsertRowRange<TRow, TItem>(
            this ITwoDimensionalList<TRow, TItem> list,
            int rowIndex,
            IEnumerable<TRow> items
        )
            where TRow : IEnumerable<TItem> => list.Validator.InsertRow(
            (nameof(rowIndex), rowIndex),
            (nameof(items), items)
        );

        /// <summary>
        ///     <see cref="InsertColumn{TRow, TItem}"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="InsertColumn{TRow, TItem}" path="param"/>
        public static void ValidateInsertColumn<TRow, TItem>(
            this ITwoDimensionalList<TRow, TItem> list,
            int columnIndex,
            IEnumerable<TItem> items
        )
            where TRow : IEnumerable<TItem>
            => list.ValidateInsertColumnRange(columnIndex, new[] { items });

        /// <summary>
        ///     <see cref="InsertColumnRange{TRow,TItem}"/> メソッドの検証処理
        /// </summary>
        /// <inheritdoc cref="InsertColumnRange{TRow,TItem}" path="param|exception"/>
        public static void ValidateInsertColumnRange<TRow, TItem>(
            this ITwoDimensionalList<TRow, TItem> list,
            int columnIndex,
            IEnumerable<IEnumerable<TItem>> items
        )
            where TRow : IEnumerable<TItem> => list.Validator.InsertColumn(
            (nameof(columnIndex), columnIndex),
            (nameof(items), items)
        );

        /// <summary>
        ///     <see cref="OverwriteRow{TRow,TItem}"/> メソッドの検証処理
        /// </summary>
        /// <inheritdoc cref="OverwriteRow{TRow,TItem}" path="param|exception"/>
        public static void ValidateOverwriteRow<TRow, TItem>(
            this ITwoDimensionalList<TRow, TItem> list,
            int rowIndex,
            IEnumerable<TRow> items
        )
            where TRow : IEnumerable<TItem> => list.Validator.OverwriteRow(
            (nameof(rowIndex), rowIndex),
            (nameof(items), items)
        );

        /// <summary>
        ///     <see cref="OverwriteColumn{TRow,TItem}"/> メソッドの検証処理
        /// </summary>
        /// <inheritdoc cref="OverwriteColumn{TRow,TItem}" path="param|exception"/>
        public static void ValidateOverwriteColumn<TRow, TItem>(
            this ITwoDimensionalList<TRow, TItem> list,
            int columnIndex,
            IEnumerable<IEnumerable<TItem>> items
        )
            where TRow : IEnumerable<TItem> => list.Validator.OverwriteColumn(
            (nameof(columnIndex), columnIndex),
            (nameof(items), items)
        );

        /// <summary>
        ///     <see cref="MoveRow{TRow, TItem}"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="MoveRow{TRow, TItem}" path="param"/>
        public static void ValidateMoveRow<TRow, TItem>(
            this ITwoDimensionalList<TRow, TItem> list,
            int oldRowIndex,
            int newRowIndex
        )
            where TRow : IEnumerable<TItem>
            => list.ValidateMoveRowRange(oldRowIndex, newRowIndex, 1);

        /// <summary>
        ///     <see cref="MoveRowRange{TRow,TItem}"/> メソッドの検証処理
        /// </summary>
        /// <inheritdoc cref="MoveRowRange{TRow,TItem}" path="param|exception"/>
        public static void ValidateMoveRowRange<TRow, TItem>(
            this ITwoDimensionalList<TRow, TItem> list,
            int oldRowIndex,
            int newRowIndex,
            int count
        )
            where TRow : IEnumerable<TItem> => list.Validator.MoveRow(
            (nameof(oldRowIndex), oldRowIndex),
            (nameof(newRowIndex), newRowIndex),
            (nameof(count), count)
        );

        /// <summary>
        ///     <see cref="MoveColumn{TRow, TItem}"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="MoveColumn{TRow, TItem}" path="param"/>
        public static void ValidateMoveColumn<TRow, TItem>(
            this ITwoDimensionalList<TRow, TItem> list,
            int oldColumnIndex,
            int newColumnIndex
        )
            where TRow : IEnumerable<TItem>
            => list.ValidateMoveColumnRange(oldColumnIndex, newColumnIndex, 1);

        /// <summary>
        ///     <see cref="MoveColumnRange{TRow,TItem}"/> メソッドの検証処理
        /// </summary>
        /// <inheritdoc cref="MoveColumnRange{TRow,TItem}" path="param|exception"/>
        public static void ValidateMoveColumnRange<TRow, TItem>(
            this ITwoDimensionalList<TRow, TItem> list,
            int oldColumnIndex,
            int newColumnIndex,
            int count
        )
            where TRow : IEnumerable<TItem> => list.Validator.MoveColumn(
            (nameof(oldColumnIndex), oldColumnIndex),
            (nameof(newColumnIndex), newColumnIndex),
            (nameof(count), count)
        );

        /// <summary>
        ///     <see cref="RemoveRow{TRow, TItem}"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="RemoveRow{TRow, TItem}" path="param"/>
        public static void ValidateRemoveRow<TRow, TItem>(
            this ITwoDimensionalList<TRow, TItem> list,
            int rowIndex
        )
            where TRow : IEnumerable<TItem>
            => list.ValidateRemoveRowRange(rowIndex, 1);

        /// <summary>
        ///     <see cref="RemoveRowRange{TRow,TItem}"/> メソッドの検証処理
        /// </summary>
        /// <inheritdoc cref="RemoveRowRange{TRow,TItem}" path="param|exception"/>
        public static void ValidateRemoveRowRange<TRow, TItem>(
            this ITwoDimensionalList<TRow, TItem> list,
            int rowIndex,
            int count
        )
            where TRow : IEnumerable<TItem> => list.Validator.RemoveRow(
            (nameof(rowIndex), rowIndex),
            (nameof(count), count)
        );

        /// <summary>
        ///     <see cref="RemoveColumn{TRow, TItem}"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="RemoveColumn{TRow, TItem}" path="param"/>
        public static void ValidateRemoveColumn<TRow, TItem>(
            this ITwoDimensionalList<TRow, TItem> list,
            int columnIndex
        )
            where TRow : IEnumerable<TItem>
            => list.ValidateRemoveColumnRange(columnIndex, 1);

        /// <summary>
        ///     <see cref="RemoveColumnRange{TRow,TItem}"/> メソッドの検証処理
        /// </summary>
        /// <inheritdoc cref="RemoveColumnRange{TRow,TItem}" path="param|exception"/>
        public static void ValidateRemoveColumnRange<TRow, TItem>(
            this ITwoDimensionalList<TRow, TItem> list,
            int columnIndex,
            int count
        )
            where TRow : IEnumerable<TItem> => list.Validator.RemoveColumn(
            (nameof(columnIndex), columnIndex),
            (nameof(count), count)
        );

        /// <summary>
        ///     <see cref="AdjustLength{TRow,TItem}"/> メソッドの検証処理
        /// </summary>
        /// <inheritdoc cref="AdjustLength{TRow,TItem}" path="param|exception"/>
        public static void ValidateAdjustLength<TRow, TItem>(
            this ITwoDimensionalList<TRow, TItem> list,
            int rowLength,
            int columnLength
        )
            where TRow : IEnumerable<TItem> => list.Validator.AdjustLength(
            (nameof(rowLength), rowLength),
            (nameof(columnLength), columnLength)
        );

        /// <summary>
        ///     <see cref="AdjustLengthIfShort{TRow, TItem}"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="AdjustLengthIfShort{TRow, TItem}" path="param"/>
        public static void ValidateAdjustLengthIfShort<TRow, TItem>(
            this ITwoDimensionalList<TRow, TItem> list,
            int rowLength,
            int columnLength
        )
            where TRow : IEnumerable<TItem>
            => list.ValidateAdjustLength(Math.Max(rowLength, list.RowCount), Math.Max(columnLength, list.ColumnCount));

        /// <summary>
        ///     <see cref="AdjustLengthIfLong{TRow, TItem}"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="AdjustLengthIfLong{TRow, TItem}" path="param"/>
        public static void ValidateAdjustLengthIfLong<TRow, TItem>(
            this ITwoDimensionalList<TRow, TItem> list,
            int rowLength,
            int columnLength
        )
            where TRow : IEnumerable<TItem>
            => list.ValidateAdjustLength(Math.Min(rowLength, list.RowCount), Math.Min(columnLength, list.ColumnCount));

        /// <summary>
        ///     <see cref="AdjustRowLength{TRow, TItem}"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="AdjustRowLength{TRow, TItem}" path="param"/>
        public static void ValidateAdjustRowLength<TRow, TItem>(
            this ITwoDimensionalList<TRow, TItem> list,
            int rowLength
        )
            where TRow : IEnumerable<TItem>
            => list.ValidateAdjustLength(rowLength, list.ColumnCount);

        /// <summary>
        ///     <see cref="AdjustRowLengthIfShort{TRow, TItem}"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="AdjustRowLengthIfShort{TRow, TItem}" path="param"/>
        public static void ValidateAdjustRowLengthIfShort<TRow, TItem>(
            this ITwoDimensionalList<TRow, TItem> list,
            int rowLength
        )
            where TRow : IEnumerable<TItem>
            => list.ValidateAdjustLength(Math.Max(rowLength, list.RowCount), list.ColumnCount);

        /// <summary>
        ///     <see cref="AdjustRowLengthIfLong{TRow, TItem}"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="AdjustRowLengthIfLong{TRow, TItem}" path="param"/>
        public static void ValidateAdjustRowLengthIfLong<TRow, TItem>(
            this ITwoDimensionalList<TRow, TItem> list,
            int rowLength
        )
            where TRow : IEnumerable<TItem>
            => list.ValidateAdjustLength(Math.Min(rowLength, list.RowCount), list.ColumnCount);

        /// <summary>
        ///     <see cref="AdjustColumnLength{TRow, TItem}"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="AdjustColumnLength{TRow, TItem}" path="param"/>
        public static void ValidateAdjustColumnLength<TRow, TItem>(
            this ITwoDimensionalList<TRow, TItem> list,
            int columnLength
        )
            where TRow : IEnumerable<TItem>
            => list.ValidateAdjustLength(list.RowCount, columnLength);

        /// <summary>
        ///     <see cref="AdjustColumnLengthIfShort{TRow, TItem}"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="AdjustColumnLengthIfShort{TRow, TItem}" path="param"/>
        public static void ValidateAdjustColumnLengthIfShort<TRow, TItem>(
            this ITwoDimensionalList<TRow, TItem> list,
            int columnLength
        )
            where TRow : IEnumerable<TItem>
            => list.ValidateAdjustLength(list.RowCount, Math.Max(columnLength, list.ColumnCount));

        /// <summary>
        ///     <see cref="AdjustColumnLengthIfLong{TRow, TItem}"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="AdjustColumnLengthIfLong{TRow, TItem}" path="param"/>
        public static void ValidateAdjustColumnLengthIfLong<TRow, TItem>(
            this ITwoDimensionalList<TRow, TItem> list,
            int columnLength
        )
            where TRow : IEnumerable<TItem>
            => list.ValidateAdjustLength(list.RowCount, Math.Min(columnLength, list.ColumnCount));

        /// <summary>
        ///     <see cref="Reset{TRow,TItem}"/> メソッドの検証処理
        /// </summary>
        /// <inheritdoc cref="Reset{TRow,TItem}" path="param|exception"/>
        public static void ValidateReset<TRow, TItem>(
            this ITwoDimensionalList<TRow, TItem> list,
            IEnumerable<TRow> rows
        )
            where TRow : IEnumerable<TItem> => list.Validator.Reset((nameof(rows), rows));

        /// <summary>
        ///     <see cref="Clear{TRow,TItem}"/> メソッドの検証処理
        /// </summary>
        /// <inheritdoc cref="Clear{TRow,TItem}" path="param|exception"/>
        public static void ValidateClear<TRow, TItem>(this ITwoDimensionalList<TRow, TItem> list)
            where TRow : IEnumerable<TItem> => list.Validator.Clear();

        #endregion
    }
}
