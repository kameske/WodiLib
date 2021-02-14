// ========================================
// Project Name : WodiLib
// File Name    : Endian.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;

namespace WodiLib.Sys
{
    /// <summary>
    ///     エンディアン
    /// </summary>
    public record Endian : TypeSafeEnum<Endian>
    {
        /// <summary>ビッグエンディアン</summary>
        public static readonly Endian Big;

        /// <summary>リトルエンディアン</summary>
        public static readonly Endian Little;

        static Endian()
        {
            Big = new Endian("Big");
            Little = new Endian("Little");
        }

        private Endian(string id) : base(id)
        {
        }

        /// <summary>
        ///     現在の環境で使用されているエンディアン
        /// </summary>
        public static Endian Environment
        {
            get
            {
                if (BitConverter.IsLittleEndian) return Little;
                return Big;
            }
        }

        /// <summary>
        ///     ウディタ内部で使用されるエンディアン
        /// </summary>
        public static Endian Woditor => Little;

        /// <inheritdoc/>
        public override string ToString()
            => base.ToString();
    }
}
