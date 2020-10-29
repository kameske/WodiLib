// ========================================
// Project Name : WodiLib
// File Name    : IReadOnlyRestrictedCapacityList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Diagnostics.CodeAnalysis;

namespace WodiLib.Sys
{
    /// <summary>
    /// 【読み取り専用】容量制限のあるList基底クラス
    /// </summary>
    /// <typeparam name="T">要素の型</typeparam>
    [Obsolete("不適切な名前のため Ver 2.6 で削除します。 IFixedLengthList<T> を使用してください。")]
    public interface IReadOnlyRestrictedCapacityCollection<T> : IReadOnlyRestrictedCapacityList<T>
    {
    }

    /// <summary>
    /// 【読み取り専用】容量制限のあるList基底クラス
    /// </summary>
    /// <typeparam name="T">要素の型</typeparam>
    public interface IReadOnlyRestrictedCapacityList<T> : IModelBase<IReadOnlyRestrictedCapacityList<T>>,
        IReadOnlyExtendedList<T>
    {
        /// <summary>
        /// 容量最大値を返す。
        /// </summary>
        /// <returns>容量最大値</returns>
        int GetMaxCapacity();

        /// <summary>
        /// 容量最小値を返す。
        /// </summary>
        /// <returns>容量最小値</returns>
        int GetMinCapacity();

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
