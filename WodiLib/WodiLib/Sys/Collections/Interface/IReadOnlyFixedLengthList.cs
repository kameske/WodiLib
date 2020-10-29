// ========================================
// Project Name : WodiLib
// File Name    : IReadOnlyFixedLengthList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Diagnostics.CodeAnalysis;

namespace WodiLib.Sys
{
    /// <summary>
    /// 【読み取り専用】長さが固定されたListインタフェース
    /// </summary>
    /// <typeparam name="T">リスト内包クラス</typeparam>
    [Obsolete("不適切な名前のため Ver 2.6 で削除します。 IFixedLengthList<T> を使用してください。")]
    public interface IReadOnlyFixedLengthCollection<T> : IReadOnlyFixedLengthList<T>
    {
    }

    /// <summary>
    /// 【読み取り専用】長さが固定されたListインタフェース
    /// </summary>
    /// <typeparam name="T">リスト内包クラス</typeparam>
    public interface IReadOnlyFixedLengthList<T> : IModelBase<IReadOnlyFixedLengthList<T>>,
        IReadOnlyExtendedList<T>
    {
        /// <summary>
        /// 容量を返す。
        /// </summary>
        /// <returns>容量</returns>
        int GetCapacity();

        /// <summary>
        /// 指定の要素が含まれているか判断する。
        /// </summary>
        /// <param name="item">対象要素</param>
        /// <returns>指定の要素が含まれる場合はtrue</returns>
        bool Contains([AllowNull] T item);

        /// <summary>
        /// 指定したオブジェクトを検索し、最初に出現する位置のインデックスを返す。
        /// </summary>
        /// <param name="item">対象要素</param>
        /// <returns>要素が含まれていない場合、-1</returns>
        int IndexOf([AllowNull] T item);

        /// <summary>
        /// すべての要素を、指定された配列のインデックスから始まる部分にコピーする。
        /// </summary>
        /// <param name="array">コピー先の配列</param>
        /// <param name="index">[Range(0, Count - 1)] コピー開始インデックス</param>
        /// <exception cref="ArgumentNullException">arrayがnullの場合</exception>
        /// <exception cref="ArgumentOutOfRangeException">indexが0未満の場合</exception>
        /// <exception cref="ArgumentException">コピー先の領域が不足する場合</exception>
        void CopyTo(T[] array, int index);
    }
}
