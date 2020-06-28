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
    /// int に変換できることを示すインタフェース
    /// </summary>
    public interface IConvertibleInt
    {
        /// <summary>
        /// int に変換する。
        /// </summary>
        /// <returns>int値</returns>
        int ToInt();
    }
}