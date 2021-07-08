// ========================================
// Project Name : WodiLib
// File Name    : EqualsHelper.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace WodiLib.Sys
{
    /// <summary>
    /// 比較処理Helperクラス
    /// </summary>
    internal static class EqualsHelper
    {
        /// <summary>
        /// <see langword="null"/> の可能性がある二つのインスタンスを比較する。
        /// </summary>
        /// <remarks>
        ///     引数 <paramref name="comparer"/> を指定した場合、これを用いて比較処理を行う。
        ///     指定しなかった場合は独自に <see cref="IEqualityComparer{T}"/> インスタンスを作成する。
        /// </remarks>
        /// <param name="left">左項</param>
        /// <param name="right">右項</param>
        /// <param name="comparer">比較処理実施クラスインスタンス</param>
        /// <typeparam name="T">比較インスタンス型</typeparam>
        /// <returns>差項と右項が一致する場合 <see langword="true"/></returns>
        public static bool NullableEquals<T>([AllowNull] T left, [AllowNull] T right,
            IEqualityComparer<T>? comparer = null)
            where T : IEqualityComparable<T>
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

            // EqualityComparer に判断を任せる
            var useComparer = comparer ?? EqualityComparerFactory.Create<T>();
            return useComparer.Equals(left, right!);
        }
    }
}
