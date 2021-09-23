// ========================================
// Project Name : WodiLib
// File Name    : Indent.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using WodiLib.Sys;

namespace WodiLib.Event
{
    /// <summary>
    ///     [Range(-128, 127)] インデント
    /// </summary>
    [CommonIntValueObject(
        MinValue = -128, MaxValue = 127,
        AddAndSubtractTypes = new[] { typeof(int), typeof(byte), typeof(sbyte) },
        CompareOtherTypes = new[] { typeof(int), typeof(byte), typeof(sbyte) }
    )]
    public partial class Indent
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="value">[Range(-128, 127)] インデント</param>
        /// <exception cref="ArgumentOutOfRangeException">numberがインデントとして不適切な場合</exception>
        public Indent(sbyte value) : this((int)value)
        {
        }

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="value">[Range(0, 255)] インデント</param>
        /// <exception cref="ArgumentOutOfRangeException">numberがインデントとして不適切な場合</exception>
        public Indent(byte value) : this(value - 128)
        {
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     sbyte に変換する。
        /// </summary>
        /// <returns>int値</returns>
        public sbyte ToSByte() => (sbyte)RawValue;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Implicit
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     sbyte -> Indent への暗黙的な型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換したインスタンス</returns>
        public static implicit operator Indent(sbyte src)
        {
            var result = new Indent(src);
            return result;
        }

        /// <summary>
        ///     Indent -> sbyte への暗黙的な型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換したインスタンス</returns>
        public static implicit operator sbyte(Indent src)
        {
            return src.ToSByte();
        }
    }
}
