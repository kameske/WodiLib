// ========================================
// Project Name : WodiLib
// File Name    : IConvertibleByte.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.Sys
{
    /// <summary>
    /// <see cref="byte"/>に変換できることを示すインタフェース。
    /// </summary>
    public interface IConvertibleByte
    {
        /// <summary>
        /// <see cref="byte"/>に変換する。
        /// </summary>
        /// <returns><see cref="byte"/>値</returns>
        public byte ToByte();
    }
}
