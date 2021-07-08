// ========================================
// Project Name : WodiLib
// File Name    : IColumnSizeChangeableTwoDimensionalList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     列サイズ変更可能であることを表すインタフェース。
    ///     書き込み可能でもある。
    /// </summary>
    /// <typeparam name="TItem">リスト要素型</typeparam>
    /// <typeparam name="TImpl">実装型</typeparam>
    /// <typeparam name="TWritable"><see cref="IWritableTwoDimensionalList{TItem,TImpl,TReadable}"/>実装型</typeparam>
    /// <typeparam name="TReadable"><see cref="IReadableTwoDimensionalList{TItem,TImpl}"/>実装型</typeparam>
    internal interface IColumnSizeChangeableTwoDimensionalList<TItem, out TImpl, out TWritable, out TReadable> :
        IWritableTwoDimensionalList<TItem, TWritable, TReadable>
        where TImpl : IColumnSizeChangeableTwoDimensionalList<TItem, TImpl, TWritable, TReadable>,
        IWritableTwoDimensionalList<TItem, TWritable, TReadable>,
        IReadableTwoDimensionalList<TItem, TReadable>
        where TWritable : IWritableTwoDimensionalList<TItem, TWritable, TReadable>
        where TReadable : IReadableTwoDimensionalList<TItem, TReadable>
    {
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

        /// <summary>
        ///     列の末尾に要素を追加する。
        /// </summary>
        /// <param name="items">追加する要素</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="items"/> が <see langword="null"/> の場合、
        ///     または <paramref name="items"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     操作によって要素数が <see cref="GetMaxColumnCapacity"/> を上回る場合、
        ///     または <paramref name="items"/> の要素数が <see cref="IReadableTwoDimensionalList{TItem,TImpl}.ColumnCount"/> と異なる場合。
        /// </exception>
        public void AddColumn(IEnumerable<TItem> items);

        /// <summary>
        ///     列の末尾に要素を追加する。
        /// </summary>
        /// <param name="items">追加する要素</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="items"/> が <see langword="null"/> の場合、
        ///     または <paramref name="items"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     操作によって要素数が <see cref="GetMaxColumnCapacity"/> を上回る場合、
        ///     または <paramref name="items"/> の要素数が <see cref="IReadableTwoDimensionalList{TItem,TImpl}.ColumnCount"/> と異なる場合。
        /// </exception>
        public void AddColumnRange(IEnumerable<IEnumerable<TItem>> items);

        /// <summary>
        ///     指定した列インデックスの位置に要素を挿入する。
        /// </summary>
        /// <param name="column">[Range(0, <see cref="IReadableTwoDimensionalList{TItem,TImpl}.ColumnCount"/>)] 列インデックス</param>
        /// <param name="items">挿入する要素</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="items"/> が <see langword="null"/> の場合、
        ///     または <paramref name="items"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     操作によって要素数が <see cref="GetMaxColumnCapacity"/> を上回る場合、
        ///     または <paramref name="items"/> の要素数が <see cref="IReadableTwoDimensionalList{TItem,TImpl}.ColumnCount"/> と異なる場合。
        /// </exception>
        public void InsertColumn(int column, IEnumerable<TItem> items);

        /// <summary>
        ///     指定した列インデックスの位置に要素を挿入する。
        /// </summary>
        /// <param name="column">[Range(0, <see cref="IReadableTwoDimensionalList{TItem,TImpl}.ColumnCount"/>)] 列インデックス</param>
        /// <param name="items">挿入する要素</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="items"/> が <see langword="null"/> の場合、
        ///     または <paramref name="items"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     操作によって要素数が <see cref="GetMaxColumnCapacity"/> を上回る場合、
        ///     または <paramref name="items"/> の要素数が <see cref="IReadableTwoDimensionalList{TItem,TImpl}.ColumnCount"/> と異なる場合。
        /// </exception>
        public void InsertColumnRange(int column, IEnumerable<IEnumerable<TItem>> items);

        /// <summary>
        ///     指定した列インデックスを起点として、要素の上書き/追加を行う。
        /// </summary>
        /// <remarks>
        ///     サンプルコードは <seealso cref="IExtendedList{T}.Overwrite"/> 参照。
        /// </remarks>
        /// <param name="column">[Range(0, <see cref="IReadableTwoDimensionalList{TItem,TImpl}.ColumnCount"/>)] 列インデックス</param>
        /// <param name="items">上書き/追加リスト</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="items"/> が <see langword="null"/> の場合。
        ///     または <paramref name="items"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="column"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     操作によって列数が <see cref="GetMaxColumnCapacity"/> を超える場合、
        ///     または <paramref name="items"/> のいずれかの要素の要素数が
        ///     <see cref="IReadableTwoDimensionalList{TItem,TImpl}.ColumnCount"/> と異なる場合。
        /// </exception>
        public void OverwriteColumn(int column, IEnumerable<IEnumerable<TItem>> items);

        /// <summary>
        ///     指定したインデックスの要素を削除する。
        /// </summary>
        /// <param name="index">[Range(0, <see cref="IReadableTwoDimensionalList{TItem,TImpl}.ColumnCount"/> - 1)] 列インデックス</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="index"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     操作によって要素数が <see cref="GetMinColumnCapacity"/> を下回る場合。
        /// </exception>
        public void RemoveColumn(int index);

        /// <summary>
        ///     要素の範囲を削除する。
        /// </summary>
        /// <param name="index">[Range(0, <see cref="IReadableTwoDimensionalList{TItem,TImpl}.ColumnCount"/> - 1)] インデックス</param>
        /// <param name="count">[Range(0, <see cref="IReadableTwoDimensionalList{TItem,TImpl}.ColumnCount"/>)] 削除する要素数</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="index"/>, <paramref name="count"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     有効な範囲外の要素を削除しようとした場合、
        ///     または 操作によって要素数が <see cref="GetMinColumnCapacity"/> を下回る場合。
        /// </exception>
        public void RemoveColumnRange(int index, int count);

        /// <summary>
        ///     列数を指定の数に合わせる。
        /// </summary>
        /// <param name="column">
        ///     [Range(<see cref="GetMinColumnCapacity"/>, <see cref="GetMaxColumnCapacity"/>)]
        ///     調整する列数
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="column"/> が指定範囲外の場合。
        /// </exception>
        public void AdjustColumnLength(int column);

        /// <summary>
        ///     列数が不足している場合、列数を指定の数に合わせる。
        /// </summary>
        /// <param name="column">
        ///     [Range(<see cref="GetMinColumnCapacity"/>, <see cref="GetMaxColumnCapacity"/>)]
        ///     調整する列数
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="column"/> が指定範囲外の場合。
        /// </exception>
        public void AdjustColumnLengthIfShort(int column);

        /// <summary>
        ///     列数が超過している場合、列数を指定の数に合わせる。
        /// </summary>
        /// <param name="column">
        ///     [Range(<see cref="GetMinColumnCapacity"/>, <see cref="GetMaxColumnCapacity"/>)]
        ///     調整する列数
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="column"/> が指定範囲外の場合。
        /// </exception>
        public void AdjustColumnLengthIfLong(int column);

        /// <summary>
        ///     自身を書き込み可能型にキャストする。
        /// </summary>
        /// <returns><typeparamref name="TWritable"/> にキャストした自分自身</returns>
        public TWritable AsWritableList();
    }
}
