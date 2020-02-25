// ========================================
// Project Name : WodiLib
// File Name    : ListExtension.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Linq;

namespace WodiLib.Sys
{
    /// <summary>
    /// リストの拡張クラス
    /// </summary>
    internal static class ListExtension
    {
        /// <summary>
        /// リストの長さを調整する。
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="length">[Range(1, int.MaxValue)] 調整する長さ</param>
        /// <param name="makeDefaultItemFunc">[NotNull] 要素数を増やす場合にデフォルトで設定する値を返すメソッド</param>
        /// <exception cref="ArgumentNullException">makeDefaultItemFuncがnullの場合</exception>
        /// <exception cref="ArgumentException">makeDefaultItemFuncがnullを返却する場合</exception>
        public static void AdjustLength<T>(this List<T> target, int length, Func<int, T> makeDefaultItemFunc)
        {
            if (makeDefaultItemFunc is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(makeDefaultItemFunc)));

            var count = target.Count;

            if (count == length) return;

            if (count < length)
            {
                // 長さが足りないので追加
                //   途中でMakeDefaultItemの結果がnullとなった場合に備え、予めすべてのデフォルト要素を取得してから処理する

                var addItemList = new List<T>();

                for (var i = target.Count; i < length; i++)
                {
                    var addItem = makeDefaultItemFunc(i);
                    if (addItem == null)
                        throw new ArgumentException(
                            ErrorMessage.NotExecute($"{nameof(makeDefaultItemFunc)}({i})の結果がnullのため、"));

                    addItemList.Add(addItem);
                }

                target.AddRange(addItemList);

                return;
            }

            // 長すぎるので除去
            target.RemoveRange(length, target.Count - length);
        }

        /// <summary>
        /// 等価比較子を用いて、Listの現在のハッシュ値を取得する。
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="equalityComparer">[NotNull] 比較演算子</param>
        /// <typeparam name="T">対象の型</typeparam>
        /// <returns>現在の要素から計算したハッシュ値</returns>
        public static int GetHashCode<T>(this List<T> target, IEqualityComparer<T> equalityComparer)
        {
            if (equalityComparer is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(equalityComparer)));

            return target.GetHashCode(equalityComparer.GetHashCode);
        }

        /// <summary>
        /// 要素のハッシュ値取得関数を指定して、Listの現在のハッシュ値を取得する。
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="funcGetHashCode">[NotNull] ハッシュ値取得関数</param>
        /// <typeparam name="T">対象の型</typeparam>
        /// <returns>現在の要素から計算したハッシュ値</returns>
        public static int GetHashCode<T>(this List<T> target, Func<T, int> funcGetHashCode)
        {
            if (funcGetHashCode is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(funcGetHashCode)));

            const int prime = 31;
            if (target.Count == 0) return 0;

            return target.Aggregate(1, (current, item) => (current * prime) ^ funcGetHashCode(item));
        }
    }
}