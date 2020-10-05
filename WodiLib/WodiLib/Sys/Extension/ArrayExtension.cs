// ========================================
// Project Name : WodiLib
// File Name    : ArrayExtension.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using System.Linq;

namespace WodiLib.Sys
{
    /// <summary>
    /// 配列拡張クラス
    /// </summary>
    internal static class ArrayExtension
    {
        /// <summary>
        /// 指定したインデックスから始まる連続した要素を取得する。
        /// </summary>
        /// <param name="src"></param>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> GetRange<T>(this T[] src, int index, int count)
        {
            ListValidationHelper.SelectIndex(index, src.Length);
            ListValidationHelper.Range(index, count, src.Length);

            return Enumerable.Range(index, count)
                .Select(i => src[i]);
        }
    }
}
