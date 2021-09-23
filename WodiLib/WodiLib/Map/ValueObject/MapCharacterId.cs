// ========================================
// Project Name : WodiLib
// File Name    : MapCharacterId.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using WodiLib.Sys;

namespace WodiLib.Map
{
    /// <summary>
    ///     [Range(int.MinValue, int.MaxValue)] マップキャラクタID
    /// </summary>
    [CommonIntValueObject]
    public partial class MapCharacterId
    {
        /// <summary>"このマップイベントID"を示すインスタンス</summary>
        public static MapCharacterId ThisMapEvent = new(-1);

        /// <summary>"主人公"を示すインスタンス</summary>
        public static MapCharacterId Hero = new(-2);

        /// <summary>"仲間1"を示すインスタンス</summary>
        public static MapCharacterId Member1 = new(-3);

        /// <summary>"仲間2"を示すインスタンス</summary>
        public static MapCharacterId Member2 = new(-4);

        /// <summary>"仲間3"を示すインスタンス</summary>
        public static MapCharacterId Member3 = new(-5);

        /// <summary>"仲間4"を示すインスタンス</summary>
        public static MapCharacterId Member4 = new(-6);

        /// <summary>"仲間5"を示すインスタンス</summary>
        public static MapCharacterId Member5 = new(-7);

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
