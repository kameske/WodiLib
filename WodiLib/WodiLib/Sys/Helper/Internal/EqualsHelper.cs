// ========================================
// Project Name : WodiLib
// File Name    : EqualsHelper.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;

namespace WodiLib.Sys
{
    /// <summary>
    ///     比較処理Helperクラス
    /// </summary>
    internal static class EqualsHelper
    {
        /// <summary>
        ///     <see langword="null"/> の可能性がある二つのインスタンスを比較する。
        /// </summary>
        /// <remarks>
        ///     どちらも <see langword="null"/> ではなかった場合、
        ///     <ul>
        ///     <li><typeparamref name="T"/> が <see cref="IEqualityComparable"/> を実装していれば <see cref="IEqualityComparable.ItemEquals"/> による比較を行う</li>
        ///     <li><typeparamref name="T"/> が <see cref="IEqualityComparable"/> を実装していなければ <see cref="EqualityComparer{T}.Default"/> による比較を行う</li>
        ///     </ul>
        /// </remarks>
        /// <param name="left">左項</param>
        /// <param name="right">右項</param>
        /// <typeparam name="T">比較インスタンス型</typeparam>
        /// <returns>差項と右項が一致する場合 <see langword="true"/></returns>
        public static bool NullableEquals<T>(T? left, T? right)
        {
            if (ReferenceEquals(left, right))
            {
                // 同一参照
                return true;
            }

            if (ReferenceEquals(left, null) ^ ReferenceEquals(right, null))
            {
                // 片方のみ null
                return false;
            }

            if (ReferenceEquals(left, null))
            {
                // どちらも null
                return true;
            }

            // IEqualityComparable が使用できる場合は使用する
            if (left is IEqualityComparable comparable)
            {
                return comparable.ItemEquals(right);
            }

            // EqualityComparer に判断を任せる
            var useComparer = EqualityComparer<T>.Default;
            return useComparer.Equals(left, right!);
        }
    }
}
