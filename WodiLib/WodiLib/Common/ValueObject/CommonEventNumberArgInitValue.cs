// ========================================
// Project Name : WodiLib
// File Name    : CommonEventNumberArgInitValue.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using WodiLib.Sys;

namespace WodiLib.Common
{
    /// <summary>
    ///     [Range(int.MinValue, int.MaxValue)]
    ///     [SafetyRange(-1400000000, 1400000000)]
    ///     コモンイベント数値引数初期値
    /// </summary>
    [CommonIntValueObject(
        MinValue = int.MinValue, MaxValue = int.MaxValue,
        SafetyMinValue = -1400000000, SafetyMaxValue = 1400000000)]
    public partial class CommonEventNumberArgInitValue
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
