// ========================================
// Project Name : WodiLib
// File Name    : MapEventId.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using WodiLib.Sys;

namespace WodiLib.Map
{
    /// <summary>
    ///     [Range(-1, 9999)] マップイベントID
    /// </summary>
    [CommonIntValueObject(MinValue = -1, MaxValue = 9999)]
    public partial class MapEventId
    {
        /// <summary>"このマップイベントID"を示すインスタンス</summary>
        public static MapEventId ThisMapEvent = new(-1);

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     byte配列に変換する。
        /// </summary>
        /// <param name="endian">エンディアン</param>
        /// <returns>byte配列</returns>
        public IEnumerable<byte> ToBytes(Endian endian) => RawValue.ToBytes(endian);
    }
}
