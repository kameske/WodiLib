// ========================================
// Project Name : WodiLib
// File Name    : CharaMoveCommandValue.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using WodiLib.Sys;

namespace WodiLib.Event.CharaMoveCommand
{
    /// <summary>
    ///     [Range(-2000000000, 2000000000)]
    ///     [SafetyRange(0, 999999999)]
    ///     キャラ動作指定コマンド設定値
    /// </summary>
    [CommonIntValueObject(
        MaxValue = 2000000000, MinValue = -2000000000,
        SafetyMinValue = 0, SafetyMaxValue = 999999999
    )]
    public partial class CharaMoveCommandValue
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
