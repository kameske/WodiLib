// ========================================
// Project Name : WodiLib
// File Name    : TileTagNumber.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using WodiLib.Sys;

namespace WodiLib.Map
{
    /// <summary>
    ///     [Range(0, 99)] タイルタグ番号
    /// </summary>
    [CommonByteValueObject(MinValue = 0, MaxValue = 99)]
    public partial class TileTagNumber
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     byte配列に変換する。
        /// </summary>
        /// <returns>byte配列</returns>
        public IEnumerable<byte> ToBytes() => new[] { RawValue };
    }
}
