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

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     読み取り可能なリストであることを表すインタフェース
    /// </summary>
    /// <typeparam name="T">リスト要素型</typeparam>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IReadableList<out T> :
        IEnumerable<T>,
        IListProperty
    {
        /// <summary>
        ///     インデクサによるアクセス
        /// </summary>
        /// <param name="index">[Range(0, <see cref="IListProperty.Count"/> - 1)] インデックス</param>
        /// <returns>指定したインデックスの要素</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/>が指定範囲外の場合。</exception>
        public T this[int index] { get; }

        /// <summary>
        ///     指定範囲の要素を簡易コピーしたリストを取得する。
        /// </summary>
        /// <param name="index">[Range(0, <see cref="IListProperty.Count"/> - 1)] インデックス</param>
        /// <param name="count">[Range(0, <see cref="IListProperty.Count"/>)] 要素数</param>
        /// <returns>指定範囲の要素簡易コピーリスト</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="index"/>, <paramref name="count"/>が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">有効な範囲外の要素を取得しようとした場合</exception>
        public IEnumerable<T> GetRange(int index, int count);
    }
}
