// ========================================
// Project Name : WodiLib
// File Name    : IConvertibleString.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;

namespace WodiLib.Sys
{
    /// <summary>
    /// string に変換できることを示すインタフェース
    /// </summary>
    [Obsolete("無用なインタフェースのため Ver 1.5 にて削除します。")]
    public interface IConvertibleString
    {
        /// <summary>
        /// string に変換する。
        /// </summary>
        /// <returns>string値</returns>
        string ToString();
    }
}
