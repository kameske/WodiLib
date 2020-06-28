// ========================================
// Project Name : WodiLib
// File Name    : IConvertibleSByte.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.Sys
{
    /// <summary>
    /// <see cref="sbyte"/>に変換できることを示すインタフェース。
    /// </summary>
    public interface IConvertibleSByte
    {
        /// <summary>
        /// <see cref="sbyte"/>に変換する。
        /// </summary>
        /// <returns><see cref="sbyte"/>値</returns>
        public sbyte ToSByte();
    }
}
