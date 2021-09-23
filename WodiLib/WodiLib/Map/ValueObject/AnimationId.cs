// ========================================
// Project Name : WodiLib
// File Name    : AnimationId.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Sys;

namespace WodiLib.Map
{
    /// <summary>
    ///     [Range(0, 4)] アニメーションID
    /// </summary>
    [CommonByteValueObject(MinValue = 0, MaxValue = 4)]
    public partial class AnimationId
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     byte に変換する。
        /// </summary>
        /// <returns>byte値</returns>
        public byte ToByte() => this;
    }
}
