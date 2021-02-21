// ========================================
// Project Name : WodiLib
// File Name    : TwoDimensionalArrayExtension.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;

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
        public static T[][] ToTransposedArray<T>(this T[][] src)
        {
            /*
             * もとの配列の 行数 != 0 かつ 列数 == 0 の場合、
             * 行数==0 かつ 列数!=0 の配列を作成できないため行列数ともに0のインスタンスを返す。
             *     => 行数または列数が 0 の場合 空配列が返却される
             */
            if (src.Length == 0 || src[0].Length == 0) return Array.Empty<T[]>();

            var newRowLength = src[0].Length;
            var newColumnLength = src.Length;
            var result = new T[newRowLength][];
            for (var i = 0; i < newRowLength; i++)
            {
                result[i] = new T[newColumnLength];
            }

            src.ForEach((line, i) => { line.ForEach((item, j) => { result[j][i] = item; }); });

            return result;
        }

        /// <summary>
        ///     内側配列の長さを取得する。<br/>
        ///     外側配列の要素数が0の場合、0を返却する。
        /// </summary>
        /// <param name="src">対象</param>
        /// <typeparam name="T">配列内包型</typeparam>
        /// <returns>内側配列の長さ</returns>
        public static int GetInnerArrayLength<T>(this T[][] src)
        {
            if (src.Length == 0) return 0;
            return src[0].Length;
        }
    }
}
