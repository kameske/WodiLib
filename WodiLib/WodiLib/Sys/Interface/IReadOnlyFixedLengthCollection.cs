// ========================================
// Project Name : WodiLib
// File Name    : IReadOnlyFixedLengthCollection.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace WodiLib.Sys
{
    /// <summary>
    /// 【読み取り専用】長さが固定されたListインタフェース
    /// </summary>
    /// <typeparam name="T">リスト内包クラス</typeparam>
    public interface IReadOnlyFixedLengthCollection<T> : IModelBase<IReadOnlyFixedLengthCollection<T>>,
        IReadOnlyList<T>, IEquatable<IReadOnlyList<T>>, INotifyCollectionChanged
    {
        /// <summary>
        /// 指定範囲の要素を簡易コピーしたリストを取得する。
        /// </summary>
        /// <param name="index">[Range(0, Count - 1)] インデックス</param>
        /// <param name="count">[Range(0, Count)] 要素数</param>
        /// <returns>指定範囲の要素簡易コピーリスト</returns>
        /// <exception cref="ArgumentOutOfRangeException">index, countが0未満の場合</exception>
        /// <exception cref="ArgumentException">有効な範囲外の要素を取得しようとした場合</exception>
        IEnumerable<T> GetRange(int index, int count);

        /// <summary>
        /// すべての列挙子を取得する。
        /// </summary>
        /// <returns>すべての列挙子</returns>
        IEnumerable<T> All();
    }
}