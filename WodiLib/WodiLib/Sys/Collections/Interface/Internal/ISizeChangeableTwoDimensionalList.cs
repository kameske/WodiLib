// ========================================
// Project Name : WodiLib
// File Name    : ISizeChangeableTwoDimensionalList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     行および列サイズ変更可能であることを表すインタフェース。
    ///     書き込み可能でもある。
    /// </summary>
    /// <typeparam name="TItem">リスト要素型</typeparam>
    /// <typeparam name="TImpl">実装型</typeparam>
    /// <typeparam name="TWritable"><see cref="IWritableTwoDimensionalList{TItem,TImpl,TReadable}"/>実装型</typeparam>
    /// <typeparam name="TReadable"><see cref="IReadableTwoDimensionalList{TItem,TImpl}"/>実装型</typeparam>
    internal interface ISizeChangeableTwoDimensionalList<TItem, out TImpl, out TWritable, out TReadable> :
        IRowSizeChangeableTwoDimensionalList<TItem, TImpl, TWritable, TReadable>,
        IColumnSizeChangeableTwoDimensionalList<TItem, TImpl, TWritable, TReadable>
        where TImpl : IRowSizeChangeableTwoDimensionalList<TItem, TImpl, TWritable, TReadable>,
        IColumnSizeChangeableTwoDimensionalList<TItem, TImpl, TWritable, TReadable>,
        IWritableTwoDimensionalList<TItem, TWritable, TReadable>,
        IReadableTwoDimensionalList<TItem, TReadable>
        where TWritable : IWritableTwoDimensionalList<TItem, TWritable, TReadable>
        where TReadable : IReadableTwoDimensionalList<TItem, TReadable>
    {
        /// <summary>
        ///     行数および列数を指定の数に合わせる。
        /// </summary>
        /// <param name="row">
        ///     [Range(<see cref="IRowSizeChangeableTwoDimensionalList{TItem,TImpl,TWritable,TReadable}.GetMinRowCapacity"/>,
        ///     <see cref="IRowSizeChangeableTwoDimensionalList{TItem,TImpl,TWritable,TReadable}.GetMaxRowCapacity"/>)]
        ///     調整する行数
        /// </param>
        /// <param name="column">
        ///     [Range(<see cref="IColumnSizeChangeableTwoDimensionalList{TItem,TImpl,TWritable,TReadable}.GetMinColumnCapacity"/>,
        ///     <see cref="IColumnSizeChangeableTwoDimensionalList{TItem,TImpl,TWritable,TReadable}.GetMaxColumnCapacity"/>)]
        ///     調整する列数
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="row"/>, <paramref name="column"/> が指定範囲外の場合。
        /// </exception>
        public void AdjustLength(int row, int column);

        /// <summary>
        ///     行数が不足している場合、行数を指定の数に合わせ、
        ///     列数が不足している場合、列数を指定の数に合わせる。
        /// </summary>
        /// <param name="row">
        ///     [Range(<see cref="IRowSizeChangeableTwoDimensionalList{TItem,TImpl,TWritable,TReadable}.GetMinRowCapacity"/>,
        ///     <see cref="IRowSizeChangeableTwoDimensionalList{TItem,TImpl,TWritable,TReadable}.GetMaxRowCapacity"/>)]
        ///     調整する行数
        /// </param>
        /// <param name="column">
        ///     [Range(<see cref="IColumnSizeChangeableTwoDimensionalList{TItem,TImpl,TWritable,TReadable}.GetMinColumnCapacity"/>,
        ///     <see cref="IColumnSizeChangeableTwoDimensionalList{TItem,TImpl,TWritable,TReadable}.GetMaxColumnCapacity"/>)]
        ///     調整する列数
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="row"/>, <paramref name="column"/> が指定範囲外の場合。
        /// </exception>
        public void AdjustLengthIfShort(int row, int column);

        /// <summary>
        ///     行数が超過している場合、行数を指定の数に合わせ、
        ///     列数が超過している場合、列数を指定の数に合わせる。
        /// </summary>
        /// <param name="row">
        ///     [Range(<see cref="IRowSizeChangeableTwoDimensionalList{TItem,TImpl,TWritable,TReadable}.GetMinRowCapacity"/>,
        ///     <see cref="IRowSizeChangeableTwoDimensionalList{TItem,TImpl,TWritable,TReadable}.GetMaxRowCapacity"/>)]
        ///     調整する行数
        /// </param>
        /// <param name="column">
        ///     [Range(<see cref="IColumnSizeChangeableTwoDimensionalList{TItem,TImpl,TWritable,TReadable}.GetMinColumnCapacity"/>,
        ///     <see cref="IColumnSizeChangeableTwoDimensionalList{TItem,TImpl,TWritable,TReadable}.GetMaxColumnCapacity"/>)]
        ///     調整する列数
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="row"/>, <paramref name="column"/> が指定範囲外の場合。
        /// </exception>
        public void AdjustLengthIfLong(int row, int column);

        /// <summary>
        ///     自分自身を初期化する。
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         既存の要素はすべて除去され、
        ///         <typeparam name="TItem">
        ///             のデフォルト要素で埋められた
        ///         </typeparam>
        ///         <see cref="IRowSizeChangeableTwoDimensionalList{TItem,TImpl,TWritable,TReadable}.GetMinRowCapacity"/> 行
        ///         <see cref="IColumnSizeChangeableTwoDimensionalList{TItem,TImpl,TWritable,TReadable}.GetMinColumnCapacity"/> 列の
        ///         二次元リストとなる。
        ///     </para>
        /// </remarks>
        public void Clear();

        /// <summary>
        ///     自身を書き込み可能型にキャストする。
        /// </summary>
        /// <returns><typeparamref name="TWritable"/> にキャストした自分自身</returns>
        public new TWritable AsWritableList();
    }
}
