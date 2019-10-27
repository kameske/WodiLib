// ========================================
// Project Name : WodiLib
// File Name    : SbyteExtension.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.Sys
{
    /// <summary>
    /// sbyte 拡張クラス
    /// </summary>
    internal static class SbyteExtension
    {
        /// <summary>
        /// byte に変換する。
        /// </summary>
        /// <param name="src">対象</param>
        /// <returns>byte値</returns>
        public static byte ToByte(this sbyte src) => (byte) src;
    }
}