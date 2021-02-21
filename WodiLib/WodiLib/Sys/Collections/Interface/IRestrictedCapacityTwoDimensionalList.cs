// ========================================
// Project Name : WodiLib
// File Name    : IRestrictedCapacityTwoDimensionalList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;

namespace WodiLib.Sys
{
    /// <summary>
    ///     容量制限のある二次元リスト
    /// </summary>
    /// <remarks>
    ///     外側のリストを「行（Row）」、内側のリストを「列（Column）」として扱う。
    ///     すべての行について列数は常に同じ値を取り続ける。<br/>
    ///     行数、列数ともに制限が設けられている。
    /// </remarks>
    /// <typeparam name="T">リスト内包クラス</typeparam>
    public interface IRestrictedCapacityTwoDimensionalList<T> : IModelBase<IRestrictedCapacityTwoDimensionalList<T>>,
        IReadOnlyRestrictedCapacityTwoDimensionalList<T>
    {
        /// <summary>
        ///     インデクサによるアクセス
        /// </summary>
        /// <param name="row">
        ///     [Range(0, <see cref="IReadOnlyTwoDimensionalList{T}.RowCount"/> - 1)] 行番号
        /// </param>
        /// <param name="column">
        ///     [Range(0, <see cref="IReadOnlyTwoDimensionalList{T}.ColumnCount"/> - 1)] 列番号
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="row"/>, <paramref name="column"/> が指定範囲外の場合。
        /// </exception>
        new T this[int row, int column] { get; set; }

        /// <summary>
        ///     最終行に要素を追加する。
        /// </summary>
        /// <param name="rowItems">追加する要素</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="rowItems"/> が <see langword="null"/> の場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="rowItems"/>中に<see langword="null"/>要素が含まれる場合、
        ///     または <paramref name="rowItems"/> の要素数が0の場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <see cref="IReadOnlyTwoDimensionalList{T}.IsEmpty"/> == <see langword="false"/>
        ///     かつ <paramref name="rowItems"/> の要素数が
        ///     <see cref="IReadOnlyTwoDimensionalList{T}.ColumnCount"/> と一致しない場合、
        ///     または操作によって <see cref="IReadOnlyTwoDimensionalList{T}.RowCount"/> が
        ///     <see cref="IReadOnlyRestrictedCapacityTwoDimensionalList{T}.GetMaxRowCapacity"/> を
        ///     超過する場合。
        /// </exception>
        void AddRow(IEnumerable<T> rowItems);

        /// <summary>
        ///     最終列に要素を追加する。
        /// </summary>
        /// <param name="columnItems">追加する要素</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="columnItems"/> が <see langword="null"/> の場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="columnItems"/>中に<see langword="null"/>要素が含まれる場合、
        ///     または <paramref name="columnItems"/> の要素数が0の場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <see cref="IReadOnlyTwoDimensionalList{T}.IsEmpty"/> == <see langword="false"/>
        ///     かつ <paramref name="columnItems"/> の要素数が
        ///     <see cref="IReadOnlyTwoDimensionalList{T}.ColumnCount"/> と一致しない場合。
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     <see cref="IReadOnlyTwoDimensionalList{T}.IsEmpty"/> == <see langword="true"/> の場合、
        ///     または操作によって <see cref="IReadOnlyTwoDimensionalList{T}.ColumnCount"/> が
        ///     <see cref="IReadOnlyRestrictedCapacityTwoDimensionalList{T}.GetMaxColumnCapacity"/> を
        ///     超過する場合。
        /// </exception>
        void AddColumn(IEnumerable<T> columnItems);

        /// <summary>
        ///     最終行に要素を追加する。
        /// </summary>
        /// <param name="items">追加する要素</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="items"/> が <see langword="null"/> の場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="items"/>中に<see langword="null"/>要素が含まれる場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <see cref="IReadOnlyTwoDimensionalList{T}.IsEmpty"/> == <see langword="false"/> かつ
        ///     <paramref name="items"/> いずれかの要素の要素数が
        ///     <see cref="IReadOnlyTwoDimensionalList{T}.ColumnCount"/> と一致しない場合、
        ///     または操作によって <see cref="IReadOnlyTwoDimensionalList{T}.RowCount"/> が
        ///     <see cref="IReadOnlyRestrictedCapacityTwoDimensionalList{T}.GetMaxRowCapacity"/> を
        ///     超過する場合。
        /// </exception>
        void AddRowRange(IEnumerable<IEnumerable<T>> items);

        /// <summary>
        ///     最終列に要素を追加する。
        /// </summary>
        /// <param name="items">追加する要素</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="items"/> が <see langword="null"/> の場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="items"/>中に<see langword="null"/>要素が含まれる場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <see cref="IReadOnlyTwoDimensionalList{T}.IsEmpty"/> == <see langword="false"/> かつ
        ///     <paramref name="items"/> いずれかの要素の要素数が
        ///     <see cref="IReadOnlyTwoDimensionalList{T}.ColumnCount"/> と一致しない場合。
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     <see cref="IReadOnlyTwoDimensionalList{T}.IsEmpty"/> == <see langword="true"/> の場合、
        ///     または操作によって <see cref="IReadOnlyTwoDimensionalList{T}.ColumnCount"/> が
        ///     <see cref="IReadOnlyRestrictedCapacityTwoDimensionalList{T}.GetMaxColumnCapacity"/> を
        ///     超過する場合。
        /// </exception>
        void AddColumnRange(IEnumerable<IEnumerable<T>> items);

        /// <summary>
        ///     行を挿入する。
        /// </summary>
        /// <param name="row">挿入行番号</param>
        /// <param name="rowItems">挿入する要素</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="rowItems"/> が <see langword="null"/> の場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="rowItems"/>中に<see langword="null"/>要素が含まれる場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <see cref="IReadOnlyTwoDimensionalList{T}.IsEmpty"/> == <see langword="false"/>
        ///     かつ <paramref name="rowItems"/> の要素数が
        ///     <see cref="IReadOnlyTwoDimensionalList{T}.ColumnCount"/> と一致しない場合、
        ///     または操作によって <see cref="IReadOnlyTwoDimensionalList{T}.RowCount"/> が
        ///     <see cref="IReadOnlyRestrictedCapacityTwoDimensionalList{T}.GetMaxRowCapacity"/> を
        ///     超過する場合。
        /// </exception>
        void InsertRow(int row, IEnumerable<T> rowItems);

        /// <summary>
        ///     列を挿入する。
        /// </summary>
        /// <param name="column">挿入列番号</param>
        /// <param name="columnItems">挿入する要素</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="columnItems"/> が <see langword="null"/> の場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="columnItems"/>中に<see langword="null"/>要素が含まれる場合、
        ///     または <paramref name="columnItems"/> の要素数が0の場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <see cref="IReadOnlyTwoDimensionalList{T}.IsEmpty"/> == <see langword="false"/>
        ///     かつ <paramref name="columnItems"/> の要素数が
        ///     <see cref="IReadOnlyTwoDimensionalList{T}.RowCount"/> と一致しない場合。
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     <see cref="IReadOnlyTwoDimensionalList{T}.IsEmpty"/> == <see langword="true"/> の場合、
        ///     または操作によって <see cref="IReadOnlyTwoDimensionalList{T}.ColumnCount"/> が
        ///     <see cref="IReadOnlyRestrictedCapacityTwoDimensionalList{T}.GetMaxColumnCapacity"/> を
        ///     超過する場合。
        /// </exception>
        void InsertColumn(int column, IEnumerable<T> columnItems);

        /// <summary>
        ///     行を挿入する。
        /// </summary>
        /// <param name="row">挿入行番号</param>
        /// <param name="items">挿入する要素</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="items"/> が <see langword="null"/> の場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="items"/>中に<see langword="null"/>要素が含まれる場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <see cref="IReadOnlyTwoDimensionalList{T}.IsEmpty"/> == <see langword="false"/>
        ///     かつ <paramref name="items"/> いずれかの要素の要素数が
        ///     <see cref="IReadOnlyTwoDimensionalList{T}.ColumnCount"/> と一致しない場合、
        ///     または操作によって <see cref="IReadOnlyTwoDimensionalList{T}.RowCount"/> が
        ///     <see cref="IReadOnlyRestrictedCapacityTwoDimensionalList{T}.GetMaxRowCapacity"/> を
        ///     超過する場合。
        /// </exception>
        void InsertRowRange(int row, IEnumerable<IEnumerable<T>> items);

        /// <summary>
        ///     列を挿入する。
        /// </summary>
        /// <param name="column">挿入列番号</param>
        /// <param name="items">挿入する要素</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="items"/> が <see langword="null"/> の場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="items"/>中に<see langword="null"/>要素が含まれる場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <see cref="IReadOnlyTwoDimensionalList{T}.IsEmpty"/> == <see langword="false"/>
        ///     かつ <paramref name="items"/> いずれかの要素の要素数が
        ///     <see cref="IReadOnlyTwoDimensionalList{T}.ColumnCount"/> と一致しない場合。
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     <see cref="IReadOnlyTwoDimensionalList{T}.IsEmpty"/> == <see langword="true"/> の場合、
        ///     または操作によって <see cref="IReadOnlyTwoDimensionalList{T}.ColumnCount"/> が
        ///     <see cref="IReadOnlyRestrictedCapacityTwoDimensionalList{T}.GetMaxColumnCapacity"/> を
        ///     超過する場合。
        /// </exception>
        void InsertColumnRange(int column, IEnumerable<IEnumerable<T>> items);

        /// <summary>
        ///     指定した行番号を起点として、要素の上書き/追加を行う。
        /// </summary>
        /// <remarks>
        ///     一次元の場合のサンプルコード：<seealso cref="IExtendedList{T}.Overwrite"/>
        /// </remarks>
        /// <param name="row">
        ///     [Range(0, <see cref="IReadOnlyTwoDimensionalList{T}.RowCount"/>)] 開始行番号
        /// </param>
        /// <param name="items">上書き/追加リスト</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="row"/> が指定範囲外の場合、
        ///     または <see cref="IReadOnlyTwoDimensionalList{T}.IsEmpty"/> == <see langword="false"/>
        ///     かつ <paramref name="items"/> いずれかの要素の要素数が
        ///     <see cref="IReadOnlyTwoDimensionalList{T}.ColumnCount"/> と一致しない場合。
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="items"/> が <see langword="null"/> の場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="items"/> 中に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        void OverwriteRow(int row, IEnumerable<IEnumerable<T>> items);

        /// <summary>
        ///     指定した列番号を起点として、要素の上書き/追加を行う。
        /// </summary>
        /// <remarks>
        ///     一次元の場合のサンプルコード：<seealso cref="IExtendedList{T}.Overwrite"/>
        /// </remarks>
        /// <param name="column">
        ///     [Range(0, <see cref="IReadOnlyTwoDimensionalList{T}.ColumnCount"/>)] 開始列番号
        /// </param>
        /// <param name="items">上書き/追加リスト</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="column"/> が指定範囲外の場合、
        ///     または <paramref name="items"/> いずれかの要素の要素数が
        ///     <see cref="IReadOnlyTwoDimensionalList{T}.RowCount"/> と一致しない場合。
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="items"/> が <see langword="null"/> の場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="items"/> 中に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     <see cref="IReadOnlyTwoDimensionalList{T}.RowCount"/> == 0 の場合。
        /// </exception>
        void OverwriteColumn(int column, IEnumerable<IEnumerable<T>> items);

        /// <summary>
        ///     指定した行にあるすべての要素をリスト内の新しい場所へ移動する。
        /// </summary>
        /// <param name="oldRow">
        ///     [Range(0, <see cref="IReadOnlyTwoDimensionalList{T}.RowCount"/> - 1)] 移動する項目の行番号
        /// </param>
        /// <param name="newRow">
        ///     [Range(0, <see cref="IReadOnlyTwoDimensionalList{T}.RowCount"/> - 1)] 移動先の行番号
        /// </param>
        /// <exception cref="InvalidOperationException">
        ///     <see cref="IReadOnlyTwoDimensionalList{T}.IsEmpty"/> == <see langword="true"/> の場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     oldRow, newRow が指定範囲外の場合。
        /// </exception>
        void MoveRow(int oldRow, int newRow);

        /// <summary>
        ///     指定した列にある全ての要素をリスト内の新しい場所へ移動する。
        /// </summary>
        /// <param name="oldColumn">
        ///     [Range(0, <see cref="IReadOnlyTwoDimensionalList{T}.ColumnCount"/> - 1)] 移動する項目の列番号
        /// </param>
        /// <param name="newColumn">
        ///     [Range(0, <see cref="IReadOnlyTwoDimensionalList{T}.ColumnCount"/> - 1)] 移動先の列番号
        /// </param>
        /// <exception cref="InvalidOperationException">
        ///     <see cref="IReadOnlyTwoDimensionalList{T}.IsEmpty"/> == <see langword="true"/> の場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     oldColumn, newColumn が指定範囲外の場合。
        /// </exception>
        void MoveColumn(int oldColumn, int newColumn);

        /// <summary>
        ///     指定した行番号から始まる連続した行要素をリスト内の新しい場所へ移動する。
        /// </summary>
        /// <param name="oldRow">
        ///     [Range(0, <see cref="IReadOnlyTwoDimensionalList{T}.RowCount"/> - 1)] 移動する項目の行番号
        /// </param>
        /// <param name="newRow">
        ///     [Range(0, <see cref="IReadOnlyTwoDimensionalList{T}.RowCount"/> - count)] 移動先の行番号
        /// </param>
        /// <param name="count">
        ///     [Range(0, <see cref="IReadOnlyTwoDimensionalList{T}.RowCount"/> - oldRow)] 移動させる行数
        /// </param>
        /// <exception cref="InvalidOperationException">
        ///     <see cref="IReadOnlyTwoDimensionalList{T}.IsEmpty"/> == <see langword="true"/> の場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     oldRow, newRow, count が指定範囲外の場合。
        /// </exception>
        void MoveRowRange(int oldRow, int newRow, int count);

        /// <summary>
        ///     指定した列番号から始まる連続した列要素をリスト内の新しい場所へ移動する。
        /// </summary>
        /// <param name="oldColumn">
        ///     [Range(0, <see cref="IReadOnlyTwoDimensionalList{T}.ColumnCount"/> - 1)] 移動する項目の列番号
        /// </param>
        /// <param name="newColumn">
        ///     [Range(0, <see cref="IReadOnlyTwoDimensionalList{T}.ColumnCount"/> - count)] 移動先の列番号
        /// </param>
        /// <param name="count">
        ///     [Range(0, <see cref="IReadOnlyTwoDimensionalList{T}.ColumnCount"/> - oldColumn)] 移動させる列数
        /// </param>
        /// <exception cref="InvalidOperationException">
        ///     <see cref="IReadOnlyTwoDimensionalList{T}.IsEmpty"/> == <see langword="true"/> の場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     oldColumn, newColumn, count が指定範囲外の場合。
        /// </exception>
        void MoveColumnRange(int oldColumn, int newColumn, int count);

        /// <summary>
        ///     指定した行番号の要素を削除する。
        /// </summary>
        /// <param name="row">
        ///     [Range(0, <see cref="IReadOnlyTwoDimensionalList{T}.RowCount"/> - 1)] 削除する行番号
        /// </param>
        /// <exception cref="InvalidOperationException">
        ///     <see cref="IReadOnlyTwoDimensionalList{T}.IsEmpty"/> == <see langword="true"/> の場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="row"/> が指定範囲外の場合。
        /// </exception>
        void RemoveRow(int row);

        /// <summary>
        ///     指定した列番号の要素を削除する。
        /// </summary>
        /// <param name="column">
        ///     [Range(0, <see cref="IReadOnlyTwoDimensionalList{T}.ColumnCount"/> - 1)] 削除する列番号
        /// </param>
        /// <exception cref="InvalidOperationException">
        ///     <see cref="IReadOnlyTwoDimensionalList{T}.IsEmpty"/> == <see langword="true"/> の場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="column"/> が指定範囲外の場合。
        /// </exception>
        void RemoveColumn(int column);

        /// <summary>
        ///     指定した行番号を起点として複数行要素を削除する。
        /// </summary>
        /// <param name="row">
        ///     [Range(0, <see cref="IReadOnlyTwoDimensionalList{T}.RowCount"/> - 1)] 削除開始行番号
        /// </param>
        /// <param name="count">
        ///     [Range(0, <see cref="IReadOnlyTwoDimensionalList{T}.RowCount"/>)] 削除する行数
        /// </param>
        /// <exception cref="InvalidOperationException">
        ///     <see cref="IReadOnlyTwoDimensionalList{T}.IsEmpty"/> == <see langword="true"/> の場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="row"/>, count が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     有効な範囲外の要素を削除しようとした場合。
        /// </exception>
        void RemoveRowRange(int row, int count);

        /// <summary>
        ///     指定した列番号を起点として複数列要素を削除する。
        /// </summary>
        /// <param name="column">
        ///     [Range(0, <see cref="IReadOnlyTwoDimensionalList{T}.ColumnCount"/> - 1)] 削除開始列番号
        /// </param>
        /// <param name="count">
        ///     [Range(0, <see cref="IReadOnlyTwoDimensionalList{T}.ColumnCount"/>)] 削除する列数
        /// </param>
        /// <exception cref="InvalidOperationException">
        ///     <see cref="IReadOnlyTwoDimensionalList{T}.IsEmpty"/> == <see langword="true"/> の場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="column"/>, count が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     有効な範囲外の要素を削除しようとした場合。
        /// </exception>
        void RemoveColumnRange(int column, int count);

        /// <summary>
        ///     行数および列数を指定の数に合わせる。
        /// </summary>
        /// <remarks>
        ///     【CollectionChanging, CollectionChangedイベント発火仕様について】<br/>
        ///     行数・列数どちらも増加する場合に通知されるイベントについては <seealso cref="AdjustLengthIfShort"/> 参照。<br/>
        ///     行数・列数どちらも減少する場合に通知されるイベントについては <seealso cref="AdjustLengthIfLong"/> 参照。<br/>
        ///     行数または列数いずれか一方のみ調整する場合、イベントは1度だけ発火する。<br/>
        ///     行数・列数いずれか一方は増加、他方は減少する場合、イベントは2度発火する。
        ///     追加操作、除去操作それぞれに対して1度ずつ発火するためである。
        /// </remarks>
        /// <param name="rowLength">
        ///     [Range(0, int.MaxValue)] 調整する行数
        /// </param>
        /// <param name="columnLength">
        ///     [Range(0, int.MaxValue)] 調整する列数
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="rowLength"/>, または <paramref name="columnLength"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="rowLength"/> != 0 かつ <paramref name="columnLength"/> == 0 の場合。
        /// </exception>
        void AdjustLength(int rowLength, int columnLength);

        /// <summary>
        ///     行数および列数それぞれについて、不足している場合、指定の数に合わせる。
        /// </summary>
        /// <remarks>
        ///     【CollectionChanging, CollectionChangedイベント発火仕様について】<br/>
        ///     行数または列数いずれか一方のみ調整する場合、イベントは1度だけ発火する。<br/>
        ///     行数および列数どちらも調整する場合、イベントは3度に分かれて発火する。
        ///     “行要素に対する追加対象となる要素通知（イベントA）“、“列要素に対する追加対象となる要素通知（イベントB）”、
        ///     “「行要素追加操作」「列要素追加操作」どちらにおいても追加対象となる要素通知（イベントC）”である。
        ///     “イベントC” にて通知される要素は “イベントA” や “イベントB” の要素には含まれない。
        ///     これは同一要素を複数回通知しないことを目的としている。
        /// </remarks>
        /// <param name="rowLength">
        ///     [Range(0, int.MaxValue)] 調整する行数
        /// </param>
        /// <param name="columnLength">
        ///     [Range(0, int.MaxValue)] 調整する列数
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="rowLength"/>, または <paramref name="columnLength"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="rowLength"/> != 0 かつ <paramref name="columnLength"/> == 0 の場合。
        /// </exception>
        void AdjustLengthIfShort(int rowLength, int columnLength);

        /// <summary>
        ///     行数および列数それぞれについて、超過している場合、指定の数に合わせる。
        /// </summary>
        /// <remarks>
        ///     【CollectionChanging, CollectionChangedイベント発火仕様について】<br/>
        ///     行数または列数いずれか一方のみ調整する場合、イベントは1度だけ発火する。<br/>
        ///     行数および列数どちらも調整する場合、イベントは3度に分かれて発火する。
        ///     “行要素に対する除去対象となる要素通知（イベントA）“、“列要素に対する除去対象となる要素通知（イベントB）”、
        ///     “「行要素除去操作」「列要素除去操作」どちらにおいても除去対象となる要素通知（イベントC）”である。
        ///     “イベントC” にて通知される要素は “イベントA” や “イベントB” の要素には含まれない。
        ///     これは同一要素を複数回通知しないことを目的としている。
        /// </remarks>
        /// <param name="rowLength">
        ///     [Range(0, int.MaxValue)] 調整する行数
        /// </param>
        /// <param name="columnLength">
        ///     [Range(0, int.MaxValue)] 調整する列数
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="rowLength"/>, または <paramref name="columnLength"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="rowLength"/>, <paramref name="columnLength"/> の片方のみ 0 の場合。
        /// </exception>
        void AdjustLengthIfLong(int rowLength, int columnLength);

        /// <summary>
        ///     行数を指定の数に合わせる。
        /// </summary>
        /// <remarks>
        ///     自身が空リストの場合、この処理は <see cref="InvalidOperationException"/> を発生させる。
        ///     自身が空リストである場合列数が確定しないため。
        /// </remarks>
        /// <param name="length">
        ///     [Range(0, int.MaxValue)] 調整する行数
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     lengthが指定範囲外の場合。
        /// </exception>
        void AdjustRowLength(int length);

        /// <summary>
        ///     列数を指定の数に合わせる。
        /// </summary>
        /// <remarks>
        ///     自身が空リストの場合、この処理は <see cref="AdjustRowLength"/> 同様
        ///     <see cref="InvalidOperationException"/> を発生させる。
        /// </remarks>
        /// <param name="length">
        ///     [Range(0, int.MaxValue)] 調整する列数
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     lengthが指定範囲外の場合。
        /// </exception>
        void AdjustColumnLength(int length);

        /// <summary>
        ///     行数が不足している場合、行数を指定の数に合わせる。
        /// </summary>
        /// <remarks>
        ///     自身が空リストの場合、この処理は <see cref="AdjustRowLength"/> 同様
        ///     <see cref="InvalidOperationException"/> を発生させる。
        /// </remarks>
        /// <param name="length">
        ///     [Range(0, int.MaxValue)] 調整する行数
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     lengthが指定範囲外の場合。
        /// </exception>
        void AdjustRowLengthIfShort(int length);

        /// <summary>
        ///     列数が不足している場合、列数を指定の数に合わせる。
        /// </summary>
        /// <remarks>
        ///     自身が空リストの場合、この処理は <see cref="InvalidOperationException"/> を発生させる。
        ///     自身が空リストである場合行数が確定しないため。
        /// </remarks>
        /// <param name="length">
        ///     [Range(0, int.MaxValue)] 調整する列数
        /// </param>
        /// <exception cref="InvalidOperationException">
        ///     <see cref="IReadOnlyTwoDimensionalList{T}.IsEmpty"/> == <see langword="true"/> の場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     lengthが指定範囲外の場合。
        /// </exception>
        void AdjustColumnLengthIfShort(int length);

        /// <summary>
        ///     行数が超過している場合、行数を指定の数に合わせる。
        /// </summary>
        /// <remarks>
        ///     自身が空リストの場合、この処理は <see cref="AdjustColumnLength"/> 同様
        ///     <see cref="InvalidOperationException"/> を発生させる。
        /// </remarks>
        /// <param name="length">
        ///     [Range(0, int.MaxValue)] 調整する行数
        /// </param>
        /// <exception cref="InvalidOperationException">
        ///     <see cref="IReadOnlyTwoDimensionalList{T}.IsEmpty"/> == <see langword="true"/> の場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     lengthが指定範囲外の場合。
        /// </exception>
        void AdjustRowLengthIfLong(int length);

        /// <summary>
        ///     列数が超過している場合、列数を指定の数に合わせる。
        /// </summary>
        /// <remarks>
        ///     自身が空リストの場合、この処理は <see cref="AdjustColumnLength"/> 同様
        ///     <see cref="InvalidOperationException"/> を発生させる。
        /// </remarks>
        /// <param name="length">
        ///     [Range(0, int.MaxValue)] 調整する列数
        /// </param>
        /// <exception cref="InvalidOperationException">
        ///     <see cref="IReadOnlyTwoDimensionalList{T}.IsEmpty"/> == <see langword="true"/> の場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     lengthが指定範囲外の場合。
        /// </exception>
        void AdjustColumnLengthIfLong(int length);

        /// <summary>
        ///     すべての要素を除去する。
        /// </summary>
        void Clear();

        /// <summary>
        ///     要素を与えられた内容で一新する。
        /// </summary>
        /// <param name="initItems">リストに詰め直す要素</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="initItems"/> が <see langword="null"/> の場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="initItems"/>中に<see langword="null"/>要素が含まれる場合、
        ///     または <paramref name="initItems"/> 内側シーケンスの要素数が0の場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="initItems"/> いずれかの要素の要素数が
        ///     <see cref="IReadOnlyTwoDimensionalList{T}.RowCount"/> と一致しない場合。
        /// </exception>
        void Reset(IEnumerable<IEnumerable<T>> initItems);
    }

    /// <summary>
    ///     容量制限のある読み取り専用二次元リスト
    /// </summary>
    /// <remarks>
    ///     外側のリストを「行（Row）」、内側のリストを「列（Column）」として扱う。
    ///     すべての行について列数は常に同じ値を取り続ける。<br/>
    ///     行数、列数ともに制限が設けられている。
    /// </remarks>
    /// <typeparam name="T">リスト内包クラス</typeparam>
    public interface IReadOnlyRestrictedCapacityTwoDimensionalList<T> : IReadOnlyTwoDimensionalList<T>
    {
        /// <summary>
        ///     最大行数を取得する。
        /// </summary>
        /// <returns>最大行数</returns>
        public int GetMaxRowCapacity();

        /// <summary>
        ///     最小行数を取得する。
        /// </summary>
        /// <returns>最小行数</returns>
        public int GetMinRowCapacity();

        /// <summary>
        ///     最大列数を取得する。
        /// </summary>
        /// <returns>最大列数</returns>
        public int GetMaxColumnCapacity();

        /// <summary>
        ///     最小列数を取得する。
        /// </summary>
        /// <returns>最小列数</returns>
        public int GetMinColumnCapacity();
    }
}
