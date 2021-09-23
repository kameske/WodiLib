// ========================================
// Project Name : WodiLib
// File Name    : MapEventPageIndex.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using WodiLib.SourceGenerator.ValueObject.Enums;
using WodiLib.Sys;

namespace WodiLib.Event
{
    /// <summary>
    ///     [Range(1, 10)] マップイベントページインデックス
    /// </summary>
    [CommonIntValueObject(
        MinValue = 1, MaxValue = 10,
        CastType = CastType.Explicit
    )]
    public partial class MapEventPageIndex
    {
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
