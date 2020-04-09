// ========================================
// Project Name : WodiLib
// File Name    : IFixedLengthCollection.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;

namespace WodiLib.Sys
{
    /// <summary>
    /// 長さが固定されたListインタフェース
    /// </summary>
    /// <typeparam name="T">リスト内包クラス</typeparam>
    public interface IFixedLengthCollection<T> : IReadOnlyFixedLengthCollection<T>
    {
        /// <summary>
        /// インデクサによるアクセス
        /// </summary>
        /// <param name="index">[Range(0, Count - 1)] インデックス</param>
        /// <returns>指定したインデックスの要素</returns>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲外の場合</exception>
        new T this[int index] { get; set; }

        /// <summary>
        /// 指定したオブジェクトを検索し、最初に出現する位置のインデックスを返す。
        /// </summary>
        /// <param name="item">対象要素</param>
        /// <returns>要素が含まれていない場合、-1</returns>
        int IndexOf(T item);
    }
}