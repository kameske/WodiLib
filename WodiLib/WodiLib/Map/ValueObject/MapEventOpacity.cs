// ========================================
// Project Name : WodiLib
// File Name    : MapEventOpacity.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Sys;

namespace WodiLib.Map
{
    /// <summary>
    ///     [Range(0, 255)] マップイベント透過度
    /// </summary>
    [CommonByteValueObject]
    public partial class MapEventOpacity
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     byte に変換する。
        /// </summary>
        /// <returns>int値</returns>
        public byte ToByte() => this;
    }
}
