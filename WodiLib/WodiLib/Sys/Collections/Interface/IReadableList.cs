// ========================================
// Project Name : WodiLib
// File Name    : IReadableList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     読み取り可能なリストであることを表すインタフェース
    /// </summary>
    /// <typeparam name="TItem">リスト要素型</typeparam>
    /// <typeparam name="TImpl">実装型</typeparam>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IReadableList<TItem, out TImpl> :
        IReadOnlyList<TItem>, INotifiableCollectionChange
        where TImpl : IReadableList<TItem, TImpl>
    {
        /// <summary>
        ///     指定範囲の要素を簡易コピーしたリストを取得する。
        /// </summary>
        /// <param name="index">[Range(0, <see cref="IReadOnlyList{T}.Count"/> - 1)] インデックス</param>
        /// <param name="count">[Range(0, <see cref="IReadOnlyList{T}.Count"/>)] 要素数</param>
        /// <returns>指定範囲の要素簡易コピーリスト</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="index"/>, <paramref name="count"/>が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">有効な範囲外の要素を取得しようとした場合</exception>
        public IEnumerable<TItem> GetRange(int index, int count);

        /// <summary>
        ///     指定の要素が含まれているか判断する。
        /// </summary>
        /// <param name="item">対象要素</param>
        /// <returns>指定の要素が含まれる場合は <see langword="true"/>、含まれない場合は <see langword="false"/>。</returns>
        public bool Contains([AllowNull] TItem item);

        /// <summary>
        ///     指定の要素が含まれているか判断する。
        /// </summary>
        /// <param name="item">対象要素</param>
        /// <param name="itemComparer">子要素比較処理</param>
        /// <returns>指定の要素が含まれる場合は <see langword="true"/>、含まれない場合は <see langword="false"/>。</returns>
        public bool Contains([AllowNull] TItem item, IEqualityComparer<TItem>? itemComparer);

        /// <summary>
        ///     指定したオブジェクトを検索し、最初に出現する位置のインデックスを返す。
        /// </summary>
        /// <param name="item">対象要素</param>
        /// <returns>要素が含まれていない場合、-1</returns>
        public int IndexOf([AllowNull] TItem item);

        /// <summary>
        ///     指定したオブジェクトを検索し、最初に出現する位置のインデックスを返す。
        /// </summary>
        /// <param name="item">対象要素</param>
        /// <param name="itemComparer">子要素比較処理</param>
        /// <returns>要素が含まれていない場合、-1</returns>
        public int IndexOf([AllowNull] TItem item, IEqualityComparer<TItem>? itemComparer);

        /// <summary>
        ///     すべての要素を、指定された配列のインデックスから始まる部分にコピーする。
        /// </summary>
        /// <param name="array">コピー先の配列</param>
        /// <param name="index">[Range(0, <see cref="IReadOnlyCollection{T}.Count"/> - 1)] コピー開始インデックス</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="array"/> が <see langword="null"/> の場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="index"/> が 0 未満の場合。
        /// </exception>
        /// <exception cref="ArgumentException">コピー先の領域が不足する場合</exception>
        public void CopyTo(TItem[] array, int index);

        /// <summary>
        ///     現在のオブジェクトが、別のオブジェクトと同値であるかどうかを示す。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>
        ///     同値 または 同一 である場合 <see langword="true"/>。
        /// </returns>
        public bool ItemEquals(IEnumerable<TItem>? other);

        /// <summary>
        ///     現在のオブジェクトが、別のオブジェクトと同値であるかどうかを示す。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <param name="itemComparer">子要素比較処理</param>
        /// <returns>
        ///     同値 または 同一 である場合 <see langword="true"/>。
        /// </returns>
        public bool ItemEquals(IEnumerable<TItem>? other, IEqualityComparer<TItem>? itemComparer);
    }
}
