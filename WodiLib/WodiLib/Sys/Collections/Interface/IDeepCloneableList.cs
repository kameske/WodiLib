// ========================================
// Project Name : WodiLib
// File Name    : IDeepCloneableList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     ディープクローン可能なリストであることを表すインタフェース
    /// </summary>
    /// <typeparam name="T">出力型</typeparam>
    /// <typeparam name="TIn">リスト内包型</typeparam>
    public interface IDeepCloneableList<out T, TIn> : IDeepCloneable<T>
        where T : IEnumerable<TIn>
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
        ///         引数 <paramref name="length"/> を指定した場合、返却する列挙子の要素数を指定された数にする。<br/>
        ///         <paramref name="length"/> &lt; <see cref="IReadOnlyCollection{T}.Count"/> の場合、超過する要素は切り捨てられる。<br/>
        ///         <paramref name="length"/> &gt; <see cref="IReadOnlyCollection{T}.Count"/> の場合、不足する要素は内包型ごとに定められたデフォルト値（
        ///         <see langword="null"/>ではない）が格納される。
        ///     </para>
        ///     <para>
        ///         引数 <paramref name="values"/> を指定した場合、<paramref name="values"/> のキーに指定されたインデックスの要素を <paramref name="values"/>
        ///         の値で上書きする。<br/>
        ///         返却する要素数を上回るインデックスが指定されている場合、その要素は無視される。
        ///     </para>
        /// </remarks>
        /// <param name="length">ディープコピー後の要素数</param>
        /// <param name="values">ディープコピー時の上書きインデックスと値のペア列挙子</param>
        /// <returns>自身をディープコピーしたインスタンス</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="length"/> が 0 未満の場合。
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="values"/> に <see langword="null"/> 要素が含まれる場合。
        ///     または <typeparamref name="T"/> が <see cref="ISizeChangeableList{TItem,TImpl,TWritable,TReadable}"/> を実装しており
        ///     <paramref name="length"/> が <see cref="ISizeChangeableList{TItem,TImpl,TWritable,TReadable}.GetMaxCapacity"/> 超 または
        ///     <see cref="ISizeChangeableList{TItem,TImpl,TWritable,TReadable}.GetMinCapacity"/> 未満の場合、
        ///     または <typeparamref name="T"/> が <see cref="IWritableList{TItem,TImpl,TReadable}"/> を実装しており
        ///     <paramref name="length"/> が <see cref="IWritableList{TItem,TImpl,TReadable}.Count"/> 以外の場合。
        /// </exception>
        public T DeepCloneWith(int? length = null, IReadOnlyDictionary<int, TIn>? values = null);
    }
}
