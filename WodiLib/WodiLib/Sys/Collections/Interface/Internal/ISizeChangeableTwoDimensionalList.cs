// ========================================
// Project Name : WodiLib
// File Name    : ISizeChangeableTwoDimensionalList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     行および列サイズ変更可能であることを表すインタフェース。
    ///     書き込み可能でもある。
    /// </summary>
    /// <typeparam name="TIn">リスト要素入力型</typeparam>
    /// <typeparam name="TOut">リスト要素出力型</typeparam>
    internal interface ISizeChangeableTwoDimensionalList<in TIn, out TOut> :
        IRowSizeChangeableTwoDimensionalList<TIn, TOut>,
        IColumnSizeChangeableTwoDimensionalList<TIn, TOut>
        where TOut : TIn
    {
        /// <summary>
        ///     行数および列数を指定の数に合わせる。
        /// </summary>
        /// <param name="rowLength">
        ///     [Range(<see cref="IRowSizeChangeableTwoDimensionalList{TIn, TOut}.GetMinRowCapacity"/>,
        ///     <see cref="IRowSizeChangeableTwoDimensionalList{TIn, TOut}.GetMaxRowCapacity"/>)]
        ///     調整する行数
        /// </param>
        /// <param name="columnLength">
        ///     [Range(<see cref="IColumnSizeChangeableTwoDimensionalList{TIn, TOut}.GetMinColumnCapacity"/>,
        ///     <see cref="IColumnSizeChangeableTwoDimensionalList{TIn, TOut}.GetMaxColumnCapacity"/>)]
        ///     調整する列数
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="rowLength"/>, <paramref name="columnLength"/> が指定範囲外の場合。
        /// </exception>
        public void AdjustLength(int rowLength, int columnLength);

        /// <summary>
        ///     行数が不足している場合、行数を指定の数に合わせ、
        ///     列数が不足している場合、列数を指定の数に合わせる。
        /// </summary>
        /// <param name="rowLength">
        ///     [Range(<see cref="IRowSizeChangeableTwoDimensionalList{TIn, TOut}.GetMinRowCapacity"/>,
        ///     <see cref="IRowSizeChangeableTwoDimensionalList{TIn, TOut}.GetMaxRowCapacity"/>)]
        ///     調整する行数
        /// </param>
        /// <param name="columnLength">
        ///     [Range(<see cref="IColumnSizeChangeableTwoDimensionalList{TIn, TOut}.GetMinColumnCapacity"/>,
        ///     <see cref="IColumnSizeChangeableTwoDimensionalList{TIn, TOut}.GetMaxColumnCapacity"/>)]
        ///     調整する列数
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="rowLength"/>, <paramref name="columnLength"/> が指定範囲外の場合。
        /// </exception>
        public void AdjustLengthIfShort(int rowLength, int columnLength);

        /// <summary>
        ///     行数が超過している場合、行数を指定の数に合わせ、
        ///     列数が超過している場合、列数を指定の数に合わせる。
        /// </summary>
        /// <param name="rowLength">
        ///     [Range(<see cref="IRowSizeChangeableTwoDimensionalList{TIn, TOut}.GetMinRowCapacity"/>,
        ///     <see cref="IRowSizeChangeableTwoDimensionalList{TIn, TOut}.GetMaxRowCapacity"/>)]
        ///     調整する行数
        /// </param>
        /// <param name="columnLength">
        ///     [Range(<see cref="IColumnSizeChangeableTwoDimensionalList{TIn, TOut}.GetMinColumnCapacity"/>,
        ///     <see cref="IColumnSizeChangeableTwoDimensionalList{TIn, TOut}.GetMaxColumnCapacity"/>)]
        ///     調整する列数
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="rowLength"/>, <paramref name="columnLength"/> が指定範囲外の場合。
        /// </exception>
        public void AdjustLengthIfLong(int rowLength, int columnLength);

        /// <summary>
        ///     要素を与えられた内容で一新する。
        /// </summary>
        /// <param name="initItems">リストに詰め直す要素</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="initItems"/> が <see langword="null"/> の場合、
        ///     または <paramref name="initItems"/>中に<see langword="null"/>要素が含まれる場合
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="initItems"/> のすべての要素の要素数が統一されていない場合。
        /// </exception>
        public void Reset(IEnumerable<IEnumerable<TIn>> initItems);

        /// <summary>
        ///     自分自身を初期化する。
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         既存の要素はすべて除去され、
        ///         <typeparam name="TOut">
        ///             のデフォルト要素で埋められた
        ///         </typeparam>
        ///         <see cref="IRowSizeChangeableTwoDimensionalList{TIn, TOut}.GetMinRowCapacity"/> 行
        ///         <see cref="IColumnSizeChangeableTwoDimensionalList{TIn, TOut}.GetMinColumnCapacity"/> 列の
        ///         二次元リストとなる。
        ///     </para>
        /// </remarks>
        public void Clear();
    }
}
