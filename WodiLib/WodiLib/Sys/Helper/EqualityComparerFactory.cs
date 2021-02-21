// ========================================
// Project Name : WodiLib
// File Name    : EqualityComparerFactory.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections;
using System.Collections.Generic;

namespace WodiLib.Sys
{
    /// <summary>
    ///     WodiLib で定義したインタフェースやクラスの同値性を比較するための
    ///     <see cref="IEqualityComparer{T}"/> を生成、提供するための Factory クラス。
    /// </summary>
    public static class EqualityComparerFactory
    {
        private static readonly IEqualityComparer EqualityComparer = new Comparer();

        /// <summary>
        ///     <see cref="IEqualityComparable{T}"/> を実装する2つのインスタンスを比較するための
        ///     <see cref="IEqualityComparer{T}"/> を生成する。
        /// </summary>
        /// <typeparam name="T">比較対象型</typeparam>
        /// <returns>比較処理実装クラスインスタンス</returns>
        public static IEqualityComparer<T> Create<T>() where T : IEqualityComparable<T>
        {
            return new Comparer<T>();
        }

        /// <summary>
        ///     <see cref="IEqualityComparer"/>を実装するインスタンスを比較するための
        ///     <see cref="IEqualityComparer"/> を生成する。
        /// </summary>
        /// <returns>比較処理実装クラスインスタンス</returns>
        public static IEqualityComparer Create()
            => EqualityComparer;

        /// <summary>
        ///     <see cref="IEqualityComparer{T}"/> 簡易実装クラス
        /// </summary>
        /// <typeparam name="T">比較対象型</typeparam>
        private class Comparer<T> : EqualityComparer<T>
            where T : IEqualityComparable<T>
        {
            public override bool Equals(T x, T y)
            {
                return x.ItemEquals(y);
            }

            public override int GetHashCode(T obj)
            {
                return obj.ToString().GetHashCode();
            }
        }

        private class Comparer : IEqualityComparer
        {
            public new bool Equals(object x, object y)
            {
                if (x is IEqualityComparable comparableX)
                {
                    return comparableX.ItemEquals(x);
                }

                return x.Equals(y);
            }

            public int GetHashCode(object obj)
            {
                return obj.ToString().GetHashCode();
            }
        }
    }
}
