// ========================================
// Project Name : WodiLib
// File Name    : IConvertibleString.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.Sys
{
    /// <summary>
    /// int に変換できることを示すインタフェース
    /// </summary>
    public interface IConvertibleString
    {
        /// <summary>
        /// string に変換する。
        /// </summary>
        /// <returns>string値</returns>
        string ToString();
    }
}