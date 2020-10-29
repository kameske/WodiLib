// ========================================
// Project Name : WodiLib
// File Name    : EquatableCompareHelper.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;

namespace WodiLib.Sys
{
    /// <summary>
    /// IEquatableインスタンスの比較Helper
    /// </summary>
    internal static class EquatableCompareHelper
    {
        /// <summary>
        /// IEquatableインタフェースを持つ2つのインスタンスを比較する。
        /// </summary>
        /// <param name="left">左辺</param>
        /// <param name="right">右辺</param>
        /// <typeparam name="T">比較対象型</typeparam>
        /// <returns>一致する場合true</returns>
        public static bool Equals<T>(IEquatable<T>? left, T right)
        {
            if (ReferenceEquals(left, right)) return true;
            // left == null && right == null を含む

            if (ReferenceEquals(left, null) ^ ReferenceEquals(right, null)) return false;
            // left, right はどちらも null ではない

            return left!.Equals(right);
        }

        public static bool Equals<T>(T left, T right)
        {
            if (ReferenceEquals(left, right)) return true;
            // left == null && right == null を含む

            if (ReferenceEquals(left, null) ^ ReferenceEquals(right, null)) return false;
            // left, right はどちらも null ではない

            if (left is IEquatable<T> equatable)
            {
                return equatable.Equals(right);
            }

            return left!.Equals(right);
        }
    }
}
