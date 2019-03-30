// ========================================
// Project Name : WodiLib
// File Name    : IConvertibleInt.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.Sys
{
    /// <summary>
    /// byte に変換できることを示すインタフェース
    /// </summary>
    public interface IConvertibleByte
    {
        /// <summary>
        /// byte に変換する。
        /// </summary>
        /// <returns>byte値</returns>
        byte ToByte();
    }
}