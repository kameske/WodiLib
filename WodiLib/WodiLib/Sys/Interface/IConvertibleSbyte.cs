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
    /// sbyte に変換できることを示すインタフェース
    /// </summary>
    public interface IConvertibleSbyte
    {
        /// <summary>
        /// sbyte に変換する。
        /// </summary>
        /// <returns>sbyte値</returns>
        sbyte ToSbyte();
    }
}