// ========================================
// Project Name : WodiLib
// File Name    : TwoDimensionByteEnumerableExtension.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;

namespace WodiLib.Sys
{
    /// <summary>
    /// 二次元byteリストの拡張クラス
    /// </summary>
    internal static class TwoDimensionByteListExtension
    {
        public static byte[] ConvertOneDimensionByteArray(this IEnumerable<List<byte>> src)
        {
            var result = new List<byte>();

            foreach (var byteList in src)
            {
                result.AddRange(byteList.ToArray());
            }

            return result.ToArray();
        }

    }
}