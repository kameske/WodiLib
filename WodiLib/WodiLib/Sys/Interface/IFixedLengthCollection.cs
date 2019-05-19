// ========================================
// Project Name : WodiLib
// File Name    : IFixedLengthCollection.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;

namespace WodiLib.Sys
{
    /// <summary>
    /// 長さが固定されたListインタフェース
    /// </summary>
    /// <typeparam name="T">リスト内包クラス</typeparam>
    public interface IFixedLengthCollection<T> : IReadOnlyCollection<T>
    {
        /// <summary>
        /// インデクサによるアクセス
        /// </summary>
        /// <param name="index">[Range(0, Count - 1)] インデックス</param>
        /// <returns>指定したインデックスの要素</returns>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲外の場合</exception>
        T this[int index] { get; set; }

        /// <summary>
        /// 指定範囲の要素を簡易コピーしたリストを取得する。
        /// </summary>
        /// <param name="index">[Range(0, Count - 1)] インデックス</param>
        /// <param name="count">[Range(0, Count)] 要素数</param>
        /// <returns>指定範囲の要素簡易コピーリスト</returns>
        /// <exception cref="ArgumentOutOfRangeException">index, countが0未満の場合</exception>
        /// <exception cref="ArgumentException">有効な範囲外の要素を取得しようとした場合</exception>
        IEnumerable<T> GetRange(int index, int count);
    }
}