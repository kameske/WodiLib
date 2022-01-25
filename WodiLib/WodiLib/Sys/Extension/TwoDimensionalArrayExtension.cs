// ========================================
// Project Name : WodiLib
// File Name    : TwoDimensionalArrayExtension.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Linq;

namespace WodiLib.Sys
{
    internal static class TwoDimensionalArrayExtension
    {
        /// <summary>
        ///     行列を入れ替えた二次元配列を返す。<br/>
        ///     【事前条件】<br/>
        ///     すべての行について要素数が一致すること
        /// </summary>
        /// <param name="src">対象</param>
        /// <returns>src の転置行列</returns>
        public static T[][] ToTransposedArray<T>(this IEnumerable<IEnumerable<T>> src)
        {
            /*
             * もとの配列の 行数 != 0 かつ 列数 == 0 の場合、
             * 行数==0 かつ 列数!=0 の配列を作成できないため行列数ともに0のインスタンスを返す。
             *     => 行数または列数が 0 の場合 空配列が返却される
             */
            var outer = src.ToArray();
            var twoArray = outer.Select(line => line.ToArray()).ToArray();
            if (outer.Length == 0 || twoArray[0].Length == 0) return Array.Empty<T[]>();

            var newRowLength = twoArray[0].Length;
            var newColumnLength = twoArray.Length;
            var result = new T[newRowLength][];
            for (var i = 0; i < newRowLength; i++)
            {
                result[i] = new T[newColumnLength];
            }

            twoArray.ForEach((line, i) => { line.ForEach((item, j) => { result[j][i] = item; }); });

            return result;
        }

        /// <summary>
        ///     必要に応じて行列を入れ替えた二次元配列を返す。<br/>
        ///     【事前条件】<br/>
        ///     すべての行について要素数が一致すること
        /// </summary>
        /// <remarks>
        ///     <paramref name="isTranspose"/> が <see langword="true"/> の場合、
        ///     <paramref name="src"/> を転置した行列を返す。
        ///     <paramref name="isTranspose"/> が <see langword="false"/> の場合、
        ///     <paramref name="src"/> をそのまま返す。
        /// </remarks>
        /// <param name="src">対象</param>
        /// <param name="isTranspose">転置実施フラグ</param>
        /// <returns>処理後の二次元配列</returns>
        public static T[][] ToTransposedArrayIf<T>(this T[][] src, bool isTranspose)
            => isTranspose ? src.ToTransposedArray() : src;

        /// <summary>
        ///     行列を入れ替えた二次元配列を返す。<br/>
        ///     【事前条件】<br/>
        ///     すべての行について要素数が一致すること
        /// </summary>
        /// <remarks>
        ///     <paramref name="isTranspose"/> が <see langword="true"/> の場合、
        ///     <paramref name="src"/> を転置した行列を返す。
        ///     <paramref name="isTranspose"/> が <see langword="false"/> の場合、
        ///     <paramref name="src"/> をそのまま返す。
        /// </remarks>
        /// <param name="src">対象</param>
        /// <param name="isTranspose">転置実施フラグ</param>
        /// <returns>処理後の二次元配列</returns>
        public static T[][] ToTransposedArrayIf<T>(this IEnumerable<IEnumerable<T>> src, bool isTranspose)
            => src.ToTwoDimensionalArray().ToTransposedArrayIf(isTranspose);

        /// <summary>
        ///     内側配列の長さを取得する。<br/>
        ///     外側配列の要素数が0の場合、0を返却する。
        /// </summary>
        /// <param name="src">対象</param>
        /// <typeparam name="T">配列内包型</typeparam>
        /// <returns>内側配列の長さ</returns>
        public static int GetInnerArrayLength<T>(this IEnumerable<IEnumerable<T>> src)
        {
            var twoDimArray = src.ToTwoDimensionalArray();
            if (twoDimArray.Length == 0) return 0;
            return twoDimArray[0].Length;
        }

        /// <summary>
        ///     二重シーケンスを二次元配列に変換する。
        /// </summary>
        /// <param name="src">対象</param>
        /// <typeparam name="T">対象シーケンスの内包型</typeparam>
        /// <returns>二次元配列</returns>
        internal static T[][] ToTwoDimensionalArray<T>(this IEnumerable<IEnumerable<T>> src)
        {
            return src.Select(line => line.ToArray()).ToArray();
        }
    }
}
