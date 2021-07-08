// ========================================
// Project Name : WodiLib
// File Name    : IDeepCloneableTwoDimensionalListInternal.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     ディープクローン可能な二次元リストであることを表すインタフェース
    /// </summary>
    /// <typeparam name="T">出力型</typeparam>
    /// <typeparam name="TIn">リスト内包型</typeparam>
    internal interface IDeepCloneableTwoDimensionalListInternal<out T, TIn> : IDeepCloneable<T>
        where T : IEnumerable<IEnumerable<TIn>>
    {
        /// <summary>
        ///     自身の要素をコピーした新たなインスタンスを返却する。
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         自身の内包する要素が <see cref="IDeepCloneable{T}"/>を実装している場合 または 構造体の場合、
        ///         返却するインスタンスの要素はすべてディープコピーされた状態で格納される。
        ///         それ以外の場合はシャローコピーされた要素が格納される。
        ///     </para>
        ///     <para>
        ///         引数 <paramref name="rowLength"/> を指定した場合、返却する列挙子の行数を指定された数にする。<br/>
        ///         <paramref name="rowLength"/> &lt; <see cref="IReadOnlyCollection{T}.Count"/> の場合、超過する行は切り捨てられる。<br/>
        ///         <paramref name="rowLength"/> &gt; <see cref="IReadOnlyCollection{T}.Count"/> の場合、不足する行は内包型ごとに定められたデフォルト値（
        ///         <see langword="null"/>ではない）が格納される。
        ///     </para>
        ///     <para>
        ///         引数 <paramref name="colLength"/> を指定した場合、返却する列挙子の列数を指定された数にする。<br/>
        ///         <paramref name="colLength"/> &lt; <see cref="IReadOnlyCollection{T}.Count"/> の場合、超過する列は切り捨てられる。<br/>
        ///         <paramref name="colLength"/> &gt; <see cref="IReadOnlyCollection{T}.Count"/> の場合、不足する列は内包型ごとに定められたデフォルト値（
        ///         <see langword="null"/>ではない）が格納される。
        ///     </para>
        ///     <para>
        ///         引数 <paramref name="values"/> を指定した場合、<paramref name="values"/> のキーに指定されたインデックスの要素を <paramref name="values"/>
        ///         の値で上書きする。<br/>
        ///         返却する要素数を上回るインデックスが指定されている場合、その要素は無視される。
        ///     </para>
        /// </remarks>
        /// <param name="rowLength">ディープコピー後の行数</param>
        /// <param name="colLength">ディープコピー後の列数</param>
        /// <param name="values">ディープコピー時の上書きインデックスと値のペア列挙子</param>
        /// <returns>自身をディープコピーしたインスタンス</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="rowLength"/>, <paramref name="colLength"/> がクローンした二次元リストの容量制限の範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="values"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        public T DeepCloneWith(int? rowLength = null, int? colLength = null,
            IReadOnlyDictionary<(int row, int col), TIn>? values = null);
    }
}
