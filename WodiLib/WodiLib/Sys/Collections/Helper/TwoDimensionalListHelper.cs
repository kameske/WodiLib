// ========================================
// Project Name : WodiLib
// File Name    : TwoDimensionalListHelper.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using System.Linq;

namespace WodiLib.Sys
{
    /// <summary>
    /// 二次元リスト Helper クラス
    /// </summary>
    internal static class TwoDimensionalListHelper
    {
        /// <summary>
        /// 単一要素を二次元配列に変換する。
        /// </summary>
        /// <param name="value">変換元</param>
        /// <returns>二次元配列</returns>
        public static T[][] ToTwoDimensionalArray<T>(T value) => new[] {new[] {value}};

        /// <summary>
        /// 二次元リストを二次元配列に変換する。
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>二次元配列</returns>
        public static T[][] ToTwoDimensionalArray<T>(List<List<T>> src)
            => src.Select(x => x.ToArray()).ToArray();

        /// <summary>
        /// 配列シーケンスを二次元配列に変換する。
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>二次元配列</returns>
        public static T[][] ToTwoDimensionalArray<T>(IEnumerable<T[]> src)
            => src.ToArray();

        /// <summary>
        /// 二重シーケンスを二次元配列に変換する。
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>二次元配列</returns>
        public static T[][] ToTwoDimensionalArray<T>(IEnumerable<IEnumerable<T>> src)
            => src.Select(x => x.ToArray()).ToArray();

        /// <summary>
        /// 二重シーケンスを行列を入れ替えた二次元配列に変換する。
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>二次元配列（転置行列）</returns>
        public static T[][] TransposeTwoDimensionalArray<T>(IEnumerable<IEnumerable<T>> src)
            => ToTwoDimensionalArray(src).ToTransposedArray();

        /// <summary>
        /// 配列内に持つ最初のシーケンスのサイズを取得する。
        /// </summary>
        /// <param name="src">対象</param>
        /// <returns>シーケンスの要素数。自身の要素数が0の場合、0。</returns>
        public static int GetInnerCount<T>(IEnumerable<T>[] src)
        {
            if (src.Length == 0) return 0;

            var items = src[0].ToArray();
            return items.Length;
        }

        /// <summary>
        /// 要素0の二次元配列を生成する。
        /// </summary>
        /// <returns>空の二次元配列</returns>
        public static T[][] MakeEmptyInstance<T>()
            => new[] {new T[] { }};
    }
}
