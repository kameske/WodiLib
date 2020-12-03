// ========================================
// Project Name : WodiLib
// File Name    : ITwoDimensionalList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;

namespace WodiLib.Sys
{
    /// <summary>
    /// 二次元リスト
    /// </summary>
    /// <remarks>
    /// 外側のリストを「行（Row）」、内側のリストを「列（Column）」として扱う。
    /// すべての行について列数は常に同じ値を取り続ける。
    /// </remarks>
    /// <typeparam name="T">リスト内包クラス</typeparam>
    public interface ITwoDimensionalList<T> : IReadOnlyTwoDimensionalList<T>
    {
        /// <summary>
        /// 要素変更前通知
        /// </summary>
        /// <remarks>
        ///     同じイベントを重複して登録することはできない。
        /// </remarks>
        event EventHandler<TwoDimensionalCollectionChangeEventArgs<T>> TwoDimensionListChanging;

        /// <summary>
        /// インデクサによるアクセス
        /// </summary>
        /// <param name="row">[Range(0, RowCount - 1)] 行番号</param>
        /// <param name="column">[Range(0, ColumnCount - 1)] 列番号</param>
        /// <exception cref="ArgumentOutOfRangeException">row, column が指定範囲外の場合</exception>
        new T this[int row, int column] { get; set; }

        /// <summary>
        /// 最終行に要素を追加する。
        /// </summary>
        /// <param name="rowItems">追加する要素</param>
        /// <exception cref="ArgumentNullException">rowItems が null の場合</exception>
        /// <exception cref="ArgumentException">
        ///     rowItems中にnull要素が含まれる場合、
        ///     または rowItems の要素数が0の場合
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     IsEmpty == false かつ rowItems の要素数が ColumnCount と一致しない場合
        /// </exception>
        void AddRow(IEnumerable<T> rowItems);

        /// <summary>
        /// 最終列に要素を追加する。
        /// </summary>
        /// <param name="columnItems">追加する要素</param>
        /// <exception cref="ArgumentNullException">columnItems が null の場合</exception>
        /// <exception cref="ArgumentException">
        ///     columnItems中にnull要素が含まれる場合、
        ///     または columnItems の要素数が0の場合
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     IsEmpty == false かつ columnItems の要素数が ColumnCount と一致しない場合
        /// </exception>
        /// <exception cref="InvalidOperationException">IsEmpty == true の場合</exception>
        void AddColumn(IEnumerable<T> columnItems);

        /// <summary>
        /// 最終行に要素を追加する。
        /// </summary>
        /// <param name="items">追加する要素</param>
        /// <exception cref="ArgumentNullException">items が null の場合</exception>
        /// <exception cref="ArgumentException">
        ///     items中にnull要素が含まれる場合
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     IsEmpty == false かつ items いずれかの要素の要素数が ColumnCount と一致しない場合
        /// </exception>
        void AddRowRange(IEnumerable<IEnumerable<T>> items);

        /// <summary>
        /// 最終列に要素を追加する。
        /// </summary>
        /// <param name="items">追加する要素</param>
        /// <exception cref="ArgumentNullException">items が null の場合</exception>
        /// <exception cref="ArgumentException">
        ///     items中にnull要素が含まれる場合
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     IsEmpty == false かつ items いずれかの要素の要素数が RowCount と一致しない場合
        /// </exception>
        /// <exception cref="InvalidOperationException">IsEmpty == true の場合</exception>
        void AddColumnRange(IEnumerable<IEnumerable<T>> items);

        /// <summary>
        /// 行を挿入する。
        /// </summary>
        /// <param name="row">挿入行番号</param>
        /// <param name="rowItems">挿入する要素</param>
        /// <exception cref="ArgumentNullException">rowItems が null の場合</exception>
        /// <exception cref="ArgumentException">
        ///     rowItems中にnull要素が含まれる場合
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     IsEmpty == false かつ rowItems の要素数が ColumnCount と一致しない場合
        /// </exception>
        void InsertRow(int row, IEnumerable<T> rowItems);

        /// <summary>
        /// 列を挿入する。
        /// </summary>
        /// <param name="column">挿入列番号</param>
        /// <param name="columnItems">挿入する要素</param>
        /// <exception cref="ArgumentNullException">columnItems が null の場合</exception>
        /// <exception cref="ArgumentException">
        ///     columnItems中にnull要素が含まれる場合、
        ///     または columnItems の要素数が0の場合
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     IsEmpty == false かつ columnItems の要素数が RowCount と一致しない場合
        /// </exception>
        /// <exception cref="InvalidOperationException">IsEmpty == true の場合</exception>
        void InsertColumn(int column, IEnumerable<T> columnItems);

        /// <summary>
        /// 行を挿入する。
        /// </summary>
        /// <param name="row">挿入行番号</param>
        /// <param name="items">挿入する要素</param>
        /// <exception cref="ArgumentNullException">items が null の場合</exception>
        /// <exception cref="ArgumentException">
        ///     items中にnull要素が含まれる場合
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     IsEmpty == false かつ items いずれかの要素の要素数が ColumnCount と一致しない場合
        /// </exception>
        void InsertRowRange(int row, IEnumerable<IEnumerable<T>> items);

        /// <summary>
        /// 列を挿入する。
        /// </summary>
        /// <param name="column">挿入列番号</param>
        /// <param name="items">挿入する要素</param>
        /// <exception cref="ArgumentNullException">items が null の場合</exception>
        /// <exception cref="ArgumentException">
        ///     items中にnull要素が含まれる場合
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     IsEmpty == false かつ items いずれかの要素の要素数が RowCount と一致しない場合
        /// </exception>
        /// <exception cref="InvalidOperationException">IsEmpty == true の場合</exception>
        void InsertColumnRange(int column, IEnumerable<IEnumerable<T>> items);

        /// <summary>
        /// 指定した行番号を起点として、要素の上書き/追加を行う。
        /// </summary>
        /// <remarks>
        /// 一次元の場合のサンプルコード：<seealso cref="IExtendedList{T}.Overwrite"/>
        /// </remarks>
        /// <param name="row">[Range(0, RowCount)] 開始行番号</param>
        /// <param name="items">上書き/追加リスト</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     row が指定範囲外の場合、
        ///     または IsEmpty == false かつ items いずれかの要素の要素数が ColumnCount と一致しない場合
        /// </exception>
        /// <exception cref="ArgumentNullException">items が null の場合</exception>
        /// <exception cref="ArgumentException">
        ///     items 中に null 要素が含まれる場合
        /// </exception>
        void OverwriteRow(int row, IEnumerable<IEnumerable<T>> items);

        /// <summary>
        /// 指定した列番号を起点として、要素の上書き/追加を行う。
        /// </summary>
        /// <remarks>
        /// 一次元の場合のサンプルコード：<seealso cref="IExtendedList{T}.Overwrite"/>
        /// </remarks>
        /// <param name="column">[Range(0, ColumnCount)] 開始列番号</param>
        /// <param name="items">上書き/追加リスト</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     column が指定範囲外の場合、
        ///     または items いずれかの要素の要素数が RowCount と一致しない場合
        /// </exception>
        /// <exception cref="ArgumentNullException">items が null の場合</exception>
        /// <exception cref="ArgumentException">
        ///     items 中に null 要素が含まれる場合
        /// </exception>
        /// <exception cref="InvalidOperationException">RowCount == 0 の場合</exception>
        void OverwriteColumn(int column, IEnumerable<IEnumerable<T>> items);

        /// <summary>
        /// 指定した行にあるすべての要素をリスト内の新しい場所へ移動する。
        /// </summary>
        /// <param name="oldRow">[Range(0, RowCount - 1)] 移動する項目の行番号</param>
        /// <param name="newRow">[Range(0, RowCount - 1)] 移動先の行番号</param>
        /// <exception cref="InvalidOperationException">IsEmpty == true の場合</exception>
        /// <exception cref="ArgumentOutOfRangeException">oldRow, newRow が指定範囲外の場合</exception>
        void MoveRow(int oldRow, int newRow);

        /// <summary>
        /// 指定した列にある全ての要素をリスト内の新しい場所へ移動する。
        /// </summary>
        /// <param name="oldColumn">[Range(0, ColumnCount - 1)] 移動する項目の列番号</param>
        /// <param name="newColumn">[Range(0, ColumnCount - 1)] 移動先の列番号</param>
        /// <exception cref="InvalidOperationException">IsEmpty == true の場合</exception>
        /// <exception cref="ArgumentOutOfRangeException">oldColumn, newColumn が指定範囲外の場合</exception>
        void MoveColumn(int oldColumn, int newColumn);

        /// <summary>
        /// 指定した行番号から始まる連続した行要素をリスト内の新しい場所へ移動する。
        /// </summary>
        /// <param name="oldRow">[Range(0, RowCount - 1)] 移動する項目の行番号</param>
        /// <param name="newRow">[Range(0, RowCount - count)] 移動先の行番号</param>
        /// <param name="count">[Range(0, RowCount - oldRow)] 移動させる行数</param>
        /// <exception cref="InvalidOperationException">IsEmpty == true の場合</exception>
        /// <exception cref="ArgumentOutOfRangeException">oldRow, newRow, count が指定範囲外の場合</exception>
        void MoveRowRange(int oldRow, int newRow, int count);

        /// <summary>
        /// 指定した列番号から始まる連続した列要素をリスト内の新しい場所へ移動する。
        /// </summary>
        /// <param name="oldColumn">[Range(0, ColumnCount - 1)] 移動する項目の列番号</param>
        /// <param name="newColumn">[Range(0, ColumnCount - count)] 移動先の列番号</param>
        /// <param name="count">[Range(0, ColumnCount - oldColumn)] 移動させる列数</param>
        /// <exception cref="InvalidOperationException">IsEmpty == true の場合</exception>
        /// <exception cref="ArgumentOutOfRangeException">oldColumn, newColumn, count が指定範囲外の場合</exception>
        void MoveColumnRange(int oldColumn, int newColumn, int count);

        /// <summary>
        /// 指定した行番号の要素を削除する。
        /// </summary>
        /// <param name="row">[Range(0, RowCount - 1)] 削除する行番号</param>
        /// <exception cref="InvalidOperationException">IsEmpty == true の場合</exception>
        /// <exception cref="ArgumentOutOfRangeException">row が指定範囲外の場合</exception>
        void RemoveRow(int row);

        /// <summary>
        /// 指定した列番号の要素を削除する。
        /// </summary>
        /// <param name="column">[Range(0, ColumnCount - 1)] 削除する列番号</param>
        /// <exception cref="InvalidOperationException">IsEmpty == true の場合</exception>
        /// <exception cref="ArgumentOutOfRangeException">column が指定範囲外の場合</exception>
        void RemoveColumn(int column);

        /// <summary>
        /// 指定した行番号を起点として複数行要素を削除する。
        /// </summary>
        /// <param name="row">[Range(0, RowCount - 1)] 削除開始行番号</param>
        /// <param name="count">[Range(0, RowCount)] 削除する行数</param>
        /// <exception cref="InvalidOperationException">IsEmpty == true の場合</exception>
        /// <exception cref="ArgumentOutOfRangeException">row, count が指定範囲外の場合</exception>
        /// <exception cref="ArgumentException">有効な範囲外の要素を削除しようとした場合</exception>
        void RemoveRowRange(int row, int count);

        /// <summary>
        /// 指定した列番号を起点として複数列要素を削除する。
        /// </summary>
        /// <param name="column">[Range(0, ColumnCount - 1)] 削除開始列番号</param>
        /// <param name="count">[Range(0, ColumnCount)] 削除する列数</param>
        /// <exception cref="InvalidOperationException">IsEmpty == true の場合</exception>
        /// <exception cref="ArgumentOutOfRangeException">column, count が指定範囲外の場合</exception>
        /// <exception cref="ArgumentException">有効な範囲外の要素を削除しようとした場合</exception>
        void RemoveColumnRange(int column, int count);

        /// <summary>
        /// 行数および列数を指定の数に合わせる。
        /// </summary>
        /// <remarks>
        /// 【CollectionChanging, CollectionChangedイベント発火仕様について】<br/>
        /// 行数・列数どちらも増加する場合に通知されるイベントについては <seealso cref="AdjustLengthIfShort"/> 参照。<br/>
        /// 行数・列数どちらも減少する場合に通知されるイベントについては <seealso cref="AdjustLengthIfLong"/> 参照。<br/>
        /// 行数または列数いずれか一方のみ調整する場合、イベントは1度だけ発火する。<br/>
        /// 行数・列数いずれか一方は増加、他方は減少する場合、イベントは2度発火する。
        /// 追加操作、除去操作それぞれに対して1度ずつ発火するためである。
        /// </remarks>
        /// <param name="rowLength">[Range(0, int.MaxValue)] 調整する行数</param>
        /// <param name="columnLength">[Range(0, int.MaxValue)] 調整する列数</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     rowLength, または columnLength が指定範囲外の場合
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     rowLength != 0 かつ columnLength == 0 の場合
        /// </exception>
        void AdjustLength(int rowLength, int columnLength);

        /// <summary>
        /// 行数および列数それぞれについて、不足している場合、指定の数に合わせる。
        /// </summary>
        /// <remarks>
        /// 【CollectionChanging, CollectionChangedイベント発火仕様について】<br/>
        /// 行数または列数いずれか一方のみ調整する場合、イベントは1度だけ発火する。<br/>
        /// 行数および列数どちらも調整する場合、イベントは3度に分かれて発火する。
        /// “行要素に対する追加対象となる要素通知（イベントA）“、“列要素に対する追加対象となる要素通知（イベントB）”、
        /// “「行要素追加操作」「列要素追加操作」どちらにおいても追加対象となる要素通知（イベントC）”である。
        /// “イベントC” にて通知される要素は “イベントA” や “イベントB” の要素には含まれない。
        /// これは同一要素を複数回通知しないことを目的としている。
        /// </remarks>
        /// <param name="rowLength">[Range(0, int.MaxValue)] 調整する行数</param>
        /// <param name="columnLength">[Range(0, int.MaxValue)] 調整する列数</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     rowLength, または columnLength が指定範囲外の場合
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     rowLength != 0 かつ columnLength == 0 の場合
        /// </exception>
        void AdjustLengthIfShort(int rowLength, int columnLength);

        /// <summary>
        /// 行数および列数それぞれについて、超過している場合、指定の数に合わせる。
        /// </summary>
        /// <remarks>
        /// 【CollectionChanging, CollectionChangedイベント発火仕様について】<br/>
        /// 行数または列数いずれか一方のみ調整する場合、イベントは1度だけ発火する。<br/>
        /// 行数および列数どちらも調整する場合、イベントは3度に分かれて発火する。
        /// “行要素に対する除去対象となる要素通知（イベントA）“、“列要素に対する除去対象となる要素通知（イベントB）”、
        /// “「行要素除去操作」「列要素除去操作」どちらにおいても除去対象となる要素通知（イベントC）”である。
        /// “イベントC” にて通知される要素は “イベントA” や “イベントB” の要素には含まれない。
        /// これは同一要素を複数回通知しないことを目的としている。
        /// </remarks>
        /// <param name="rowLength">[Range(0, int.MaxValue)] 調整する行数</param>
        /// <param name="columnLength">[Range(0, int.MaxValue)] 調整する列数</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     rowLength, または columnLength が指定範囲外の場合
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     rowLength, columnLengthの片方のみ 0 の場合
        /// </exception>
        void AdjustLengthIfLong(int rowLength, int columnLength);

        /// <summary>
        /// 行数を指定の数に合わせる。
        /// </summary>
        /// <remarks>
        /// 自身が空リストの場合、この処理は <see cref="InvalidOperationException"/> を発生させる。
        /// 自身が空リストである場合列数が確定しないため。
        /// </remarks>
        /// <param name="length">[Range(0, int.MaxValue)] 調整する行数</param>
        /// <exception cref="ArgumentOutOfRangeException">lengthが指定範囲外の場合</exception>
        void AdjustRowLength(int length);

        /// <summary>
        /// 列数を指定の数に合わせる。
        /// </summary>
        /// <remarks>
        /// 自身が空リストの場合、この処理は <see cref="AdjustRowLength"/> 同様
        /// <see cref="InvalidOperationException"/> を発生させる。
        /// </remarks>
        /// <param name="length">[Range(0, int.MaxValue)] 調整する列数</param>
        /// <exception cref="ArgumentOutOfRangeException">lengthが指定範囲外の場合</exception>
        void AdjustColumnLength(int length);

        /// <summary>
        /// 行数が不足している場合、行数を指定の数に合わせる。
        /// </summary>
        /// <remarks>
        /// 自身が空リストの場合、この処理は <see cref="AdjustRowLength"/> 同様
        /// <see cref="InvalidOperationException"/> を発生させる。
        /// </remarks>
        /// <param name="length">[Range(0, int.MaxValue)] 調整する行数</param>
        /// <exception cref="ArgumentOutOfRangeException">lengthが指定範囲外の場合</exception>
        void AdjustRowLengthIfShort(int length);

        /// <summary>
        /// 列数が不足している場合、列数を指定の数に合わせる。
        /// </summary>
        /// <remarks>
        /// 自身が空リストの場合、この処理は <see cref="InvalidOperationException"/> を発生させる。
        /// 自身が空リストである場合行数が確定しないため。
        /// </remarks>
        /// <param name="length">[Range(0, int.MaxValue)] 調整する列数</param>
        /// <exception cref="InvalidOperationException">IsEmpty == true の場合</exception>
        /// <exception cref="ArgumentOutOfRangeException">lengthが指定範囲外の場合</exception>
        void AdjustColumnLengthIfShort(int length);

        /// <summary>
        /// 行数が超過している場合、行数を指定の数に合わせる。
        /// </summary>
        /// <remarks>
        /// 自身が空リストの場合、この処理は <see cref="AdjustColumnLength"/> 同様
        /// <see cref="InvalidOperationException"/> を発生させる。
        /// </remarks>
        /// <param name="length">[Range(0, int.MaxValue)] 調整する行数</param>
        /// <exception cref="InvalidOperationException">IsEmpty == true の場合</exception>
        /// <exception cref="ArgumentOutOfRangeException">lengthが指定範囲外の場合</exception>
        void AdjustRowLengthIfLong(int length);

        /// <summary>
        /// 列数が超過している場合、列数を指定の数に合わせる。
        /// </summary>
        /// <remarks>
        /// 自身が空リストの場合、この処理は <see cref="AdjustColumnLength"/> 同様
        /// <see cref="InvalidOperationException"/> を発生させる。
        /// </remarks>
        /// <param name="length">[Range(0, int.MaxValue)] 調整する列数</param>
        /// <exception cref="InvalidOperationException">IsEmpty == true の場合</exception>
        /// <exception cref="ArgumentOutOfRangeException">lengthが指定範囲外の場合</exception>
        void AdjustColumnLengthIfLong(int length);

        /// <summary>
        /// すべての要素を除去する。
        /// </summary>
        void Clear();

        /// <summary>
        /// 要素を与えられた内容で一新する。
        /// </summary>
        /// <param name="initItems">リストに詰め直す要素</param>
        /// <exception cref="ArgumentNullException">items が null の場合</exception>
        /// <exception cref="ArgumentException">
        ///     items中にnull要素が含まれる場合、
        ///     または items 内側シーケンスの要素数が0の場合
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     items いずれかの要素の要素数が RowCount と一致しない場合
        /// </exception>
        void Reset(IEnumerable<IEnumerable<T>> initItems);
    }
}
