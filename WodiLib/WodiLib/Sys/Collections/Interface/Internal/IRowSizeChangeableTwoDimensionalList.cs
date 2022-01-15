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
    /// <typeparam name="TInRow">リスト行データ入力型</typeparam>
    /// <typeparam name="TOutRow">リスト行データ出力型</typeparam>
    /// <typeparam name="TInItem">リスト要素入力型</typeparam>
    internal interface IRowSizeChangeableTwoDimensionalList<in TInRow, out TOutRow, in TInItem> :
        ITwoDimensionalListProperty,
        IEnumerable<TOutRow>
        where TInRow : IEnumerable<TInItem>
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
        ///     または <paramref name="items"/> の要素数が <see cref="ITwoDimensionalListProperty.ColumnCount"/> と異なる場合。
        /// </exception>
        public void AddRow(params TInRow[] items);

        /// <summary>
        ///     指定した行インデックスの位置に要素を挿入する。
        /// </summary>
        /// <param name="rowIndex">[Range(0, <see cref="ITwoDimensionalListProperty.RowCount"/>)] 行インデックス</param>
        /// <param name="items">挿入する要素</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="items"/> が <see langword="null"/> の場合、
        ///     または <paramref name="items"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     操作によって要素数が <see cref="GetMaxRowCapacity"/> を上回る場合、
        ///     または <paramref name="items"/> の要素数が <see cref="ITwoDimensionalListProperty.ColumnCount"/> と異なる場合。
        /// </exception>
        public void InsertRow(int rowIndex, params TInRow[] items);

        /// <summary>
        ///     指定した行インデックスを起点として、要素の上書き/追加を行う。
        /// </summary>
        /// <remarks>
        ///     サンプルコードは <seealso cref="IExtendedList{T}.Overwrite"/> 参照。
        /// </remarks>
        /// <param name="rowIndex">[Range(0, <see cref="ITwoDimensionalListProperty.RowCount"/>)] 行インデックス</param>
        /// <param name="items">上書き/追加リスト</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="items"/> が <see langword="null"/> の場合。
        ///     または <paramref name="items"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="rowIndex"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     操作によって行数が <see cref="GetMaxRowCapacity"/> を超える場合、
        ///     または <paramref name="items"/> のいずれかの要素の要素数が
        ///     <see cref="ITwoDimensionalListProperty.ColumnCount"/> と異なる場合。
        /// </exception>
        public void OverwriteRow(int rowIndex, params TInRow[] items);

        /// <summary>
        ///     要素の範囲を削除する。
        /// </summary>
        /// <param name="rowIndex">[Range(0, <see cref="ITwoDimensionalListProperty.RowCount"/> - 1)] インデックス</param>
        /// <param name="count">[Range(0, <see cref="ITwoDimensionalListProperty.RowCount"/>)] 削除する要素数</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="rowIndex"/>, <paramref name="count"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     有効な範囲外の要素を削除しようとした場合、
        ///     または 操作によって要素数が <see cref="GetMinRowCapacity"/> を下回る場合。
        /// </exception>
        public void RemoveRow(int rowIndex, int count = 1);

        /// <summary>
        ///     行数を指定の数に合わせる。
        /// </summary>
        /// <param name="rowLength">
        ///     [Range(<see cref="GetMinRowCapacity"/>, <see cref="GetMaxRowCapacity"/>)]
        ///     調整する行数
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="rowLength"/> が指定範囲外の場合。
        /// </exception>
        public void AdjustRowLength(int rowLength);

        /// <summary>
        ///     行数が不足している場合、行数を指定の数に合わせる。
        /// </summary>
        /// <param name="rowLength">
        ///     [Range(<see cref="GetMinRowCapacity"/>, <see cref="GetMaxRowCapacity"/>)]
        ///     調整する行数
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="rowLength"/> が指定範囲外の場合。
        /// </exception>
        public void AdjustRowLengthIfShort(int rowLength);

        /// <summary>
        ///     行数が超過している場合、行数を指定の数に合わせる。
        /// </summary>
        /// <param name="rowLength">
        ///     [Range(<see cref="GetMinRowCapacity"/>, <see cref="GetMaxRowCapacity"/>)]
        ///     調整する行数
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="rowLength"/> が指定範囲外の場合。
        /// </exception>
        public void AdjustRowLengthIfLong(int rowLength);
    }
}
