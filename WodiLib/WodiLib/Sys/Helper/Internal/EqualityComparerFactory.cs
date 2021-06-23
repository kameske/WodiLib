// ========================================
// Project Name : WodiLib
// File Name    : EqualityComparerFactory.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WodiLib.Sys
{
    /// <summary>
    ///     WodiLib で定義したインタフェースやクラスの同値性を比較するための
    ///     <see cref="IEqualityComparer{T}"/> を生成、提供するための Factory クラス。
    /// </summary>
    internal static class EqualityComparerFactory
    {
        private static IEqualityComparer EqualityComparer { get; } = new Comparer();

        private static Dictionary<Type, IEqualityComparer> EqualityGenericComparerDic { get; } =
            new();

        /// <summary>
        ///     <see cref="IEqualityComparable{T}"/> を実装する2つのインスタンスを比較するための
        ///     <see cref="IEqualityComparer{T}"/> を生成する。
        /// </summary>
        /// <typeparam name="T">比較対象型</typeparam>
        /// <returns>比較処理実装クラスインスタンス</returns>
        public static IEqualityComparer<T> Create<T>() where T : IEqualityComparable<T>
        {
            IEqualityComparer<T> result;
            var type = typeof(T);

            if (EqualityGenericComparerDic.ContainsKey(type))
            {
                result = (IEqualityComparer<T>) EqualityGenericComparerDic[type];
            }
            else
            {
                result = new Comparer<T>();
                EqualityGenericComparerDic.Add(type, (IEqualityComparer) result);
            }

            return result;
        }

        /// <summary>
        ///     <see cref="IEqualityComparable{T}"/> を実装するクラスの列挙を比較するための
        ///     <see cref="IEqualityComparer{T}"/> を生成する。
        /// </summary>
        /// <typeparam name="TEnum">比較対象型</typeparam>
        /// <typeparam name="TItem"><typeparamref name="TEnum"/>の要素型</typeparam>
        /// <returns>比較処理実装クラスインスタンス</returns>
        public static IEqualityComparer<TEnum> CreateForEqualityComparableItems<TEnum, TItem>()
            where TEnum : IEnumerable<TItem>
            where TItem : IEqualityComparable<TItem>
        {
            IEqualityComparer<TEnum> result;
            var type = typeof(TEnum);

            if (EqualityGenericComparerDic.ContainsKey(type))
            {
                result = (IEqualityComparer<TEnum>) EqualityGenericComparerDic[type];
            }
            else
            {
                result = new EqualityComparableItemsComparer<TEnum, TItem>();
                EqualityGenericComparerDic.Add(type, (IEqualityComparer) result);
            }

            return result;
        }

        /// <summary>
        ///     <see cref="IEqualityComparer"/>を実装するインスタンスを比較するための
        ///     <see cref="IEqualityComparer"/> を生成する。
        /// </summary>
        /// <returns>比較処理実装クラスインスタンス</returns>
        public static IEqualityComparer Create()
            => EqualityComparer;

        /// <summary>
        ///     <see cref="IEqualityComparable{T}"/> を実装した要素の
        ///     列挙型インスタンス比較用
        ///     <see cref="IEqualityComparer{T}"/> 簡易実装クラス
        /// </summary>
        /// <typeparam name="TEnum">比較対象型</typeparam>
        /// <typeparam name="TItem">要素の型</typeparam>
        private class EqualityComparableItemsComparer<TEnum, TItem> : EqualityComparer<TEnum>
            where TEnum : IEnumerable<TItem>
            where TItem : IEqualityComparable<TItem>
        {
            public override bool Equals(TEnum x, TEnum y)
            {
                var itemComparer = Create<TItem>();
                return x.SequenceEqual(y, itemComparer);
            }

            public override int GetHashCode(TEnum obj)
            {
                return obj.GetHashCode();
            }
        }

        /// <summary>
        ///     <see cref="IEqualityComparable{T}"/> を実装するインスタンス比較用
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

        /// <summary>
        ///     <see cref="IEqualityComparable{T}"/> を実装しないインスタンス比較用
        ///     <see cref="IEqualityComparer{T}"/> 簡易実装クラス
        /// </summary>
        private class Comparer : EqualityComparer<object>
        {
            public override bool Equals(object x, object y)
            {
                if (x is IEqualityComparable comparableX)
                {
                    return comparableX.ItemEquals(x);
                }

                return x.Equals(y);
            }

            public override int GetHashCode(object obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}
