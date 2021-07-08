// ========================================
// Project Name : WodiLib
// File Name    : IRowSizeChangeableTwoDimensionalList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     行サイズ変更可能であることを表すインタフェース。
    ///     書き込み可能でもある。
    /// </summary>
    /// <typeparam name="TItem">リスト要素型</typeparam>
    /// <typeparam name="TImpl">実装型</typeparam>
    /// <typeparam name="TWritable"><see cref="IWritableTwoDimensionalList{TItem,TImpl,TReadable}"/>実装型</typeparam>
    /// <typeparam name="TReadable"><see cref="IReadableTwoDimensionalList{TItem,TImpl}"/>実装型</typeparam>
    internal interface IRowSizeChangeableTwoDimensionalList<TItem, out TImpl, out TWritable, out TReadable> :
        IWritableTwoDimensionalList<TItem, TWritable, TReadable>
        where TImpl : IRowSizeChangeableTwoDimensionalList<TItem, TImpl, TWritable, TReadable>,
        IWritableTwoDimensionalList<TItem, TWritable, TReadable>,
        IReadableTwoDimensionalList<TItem, TReadable>
        where TWritable : IWritableTwoDimensionalList<TItem, TWritable, TReadable>
        where TReadable : IReadableTwoDimensionalList<TItem, TReadable>
    {
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
        ///     行の末尾に要素を追加する。
        /// </summary>
        /// <param name="items">追加する要素</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="items"/> が <see langword="null"/> の場合、
        ///     または <paramref name="items"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     操作によって要素数が <see cref="GetMaxRowCapacity"/> を上回る場合、
        ///     または <paramref name="items"/> の要素数が <see cref="IReadableTwoDimensionalList{TItem,TImpl}.ColumnCount"/> と異なる場合。
        /// </exception>
        public void AddRow(IEnumerable<TItem> items);

        /// <summary>
        ///     行の末尾に要素を追加する。
        /// </summary>
        /// <param name="items">追加する要素</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="items"/> が <see langword="null"/> の場合、
        ///     または <paramref name="items"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     操作によって要素数が <see cref="GetMaxRowCapacity"/> を上回る場合、
        ///     または <paramref name="items"/> の要素数が <see cref="IReadableTwoDimensionalList{TItem,TImpl}.ColumnCount"/> と異なる場合。
        /// </exception>
        public void AddRowRange(IEnumerable<IEnumerable<TItem>> items);

        /// <summary>
        ///     指定した行インデックスの位置に要素を挿入する。
        /// </summary>
        /// <param name="row">[Range(0, <see cref="IReadableTwoDimensionalList{TItem,TImpl}.RowCount"/>)] 行インデックス</param>
        /// <param name="items">挿入する要素</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="items"/> が <see langword="null"/> の場合、
        ///     または <paramref name="items"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     操作によって要素数が <see cref="GetMaxRowCapacity"/> を上回る場合、
        ///     または <paramref name="items"/> の要素数が <see cref="IReadableTwoDimensionalList{TItem,TImpl}.ColumnCount"/> と異なる場合。
        /// </exception>
        public void InsertRow(int row, IEnumerable<TItem> items);

        /// <summary>
        ///     指定した行インデックスの位置に要素を挿入する。
        /// </summary>
        /// <param name="row">[Range(0, <see cref="IReadableTwoDimensionalList{TItem,TImpl}.RowCount"/>)] 行インデックス</param>
        /// <param name="items">挿入する要素</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="items"/> が <see langword="null"/> の場合、
        ///     または <paramref name="items"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     操作によって要素数が <see cref="GetMaxRowCapacity"/> を上回る場合、
        ///     または <paramref name="items"/> の要素数が <see cref="IReadableTwoDimensionalList{TItem,TImpl}.ColumnCount"/> と異なる場合。
        /// </exception>
        public void InsertRowRange(int row, IEnumerable<IEnumerable<TItem>> items);

        /// <summary>
        ///     指定した行インデックスを起点として、要素の上書き/追加を行う。
        /// </summary>
        /// <remarks>
        ///     サンプルコードは <seealso cref="IExtendedList{T}.Overwrite"/> 参照。
        /// </remarks>
        /// <param name="row">[Range(0, <see cref="IReadableTwoDimensionalList{TItem,TImpl}.RowCount"/>)] 行インデックス</param>
        /// <param name="items">上書き/追加リスト</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="items"/> が <see langword="null"/> の場合。
        ///     または <paramref name="items"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="row"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     操作によって行数が <see cref="GetMaxRowCapacity"/> を超える場合、
        ///     または <paramref name="items"/> のいずれかの要素の要素数が
        ///     <see cref="IReadableTwoDimensionalList{TItem,TImpl}.ColumnCount"/> と異なる場合。
        /// </exception>
        public void OverwriteRow(int row, IEnumerable<IEnumerable<TItem>> items);

        /// <summary>
        ///     指定したインデックスの要素を削除する。
        /// </summary>
        /// <param name="index">[Range(0, <see cref="IReadableTwoDimensionalList{TItem,TImpl}.RowCount"/> - 1)] 行インデックス</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="index"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     操作によって要素数が <see cref="GetMinRowCapacity"/> を下回る場合。
        /// </exception>
        public void RemoveRow(int index);

        /// <summary>
        ///     要素の範囲を削除する。
        /// </summary>
        /// <param name="index">[Range(0, <see cref="IReadableTwoDimensionalList{TItem,TImpl}.RowCount"/> - 1)] インデックス</param>
        /// <param name="count">[Range(0, <see cref="IReadableTwoDimensionalList{TItem,TImpl}.RowCount"/>)] 削除する要素数</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="index"/>, <paramref name="count"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     有効な範囲外の要素を削除しようとした場合、
        ///     または 操作によって要素数が <see cref="GetMinRowCapacity"/> を下回る場合。
        /// </exception>
        public void RemoveRowRange(int index, int count);

        /// <summary>
        ///     行数を指定の数に合わせる。
        /// </summary>
        /// <param name="row">
        ///     [Range(<see cref="GetMinRowCapacity"/>, <see cref="GetMaxRowCapacity"/>)]
        ///     調整する行数
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="row"/> が指定範囲外の場合。
        /// </exception>
        public void AdjustRowLength(int row);

        /// <summary>
        ///     行数が不足している場合、行数を指定の数に合わせる。
        /// </summary>
        /// <param name="row">
        ///     [Range(<see cref="GetMinRowCapacity"/>, <see cref="GetMaxRowCapacity"/>)]
        ///     調整する行数
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="row"/> が指定範囲外の場合。
        /// </exception>
        public void AdjustRowLengthIfShort(int row);

        /// <summary>
        ///     行数が超過している場合、行数を指定の数に合わせる。
        /// </summary>
        /// <param name="row">
        ///     [Range(<see cref="GetMinRowCapacity"/>, <see cref="GetMaxRowCapacity"/>)]
        ///     調整する行数
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="row"/> が指定範囲外の場合。
        /// </exception>
        public void AdjustRowLengthIfLong(int row);

        /// <summary>
        ///     自身を書き込み可能型にキャストする。
        /// </summary>
        /// <returns><typeparamref name="TWritable"/> にキャストした自分自身</returns>
        public TWritable AsWritableList();
    }
}
