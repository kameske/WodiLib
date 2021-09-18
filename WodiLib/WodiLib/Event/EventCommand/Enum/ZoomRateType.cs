// ========================================
// Project Name : WodiLib
// File Name    : ZoomRateType.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Linq;
using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <summary>
    ///     拡大率種別
    /// </summary>
    public class ZoomRateType : TypeSafeEnum<ZoomRateType>
    {
        /// <summary>通常</summary>
        public static readonly ZoomRateType Normal;

        /// <summary>縦のみ</summary>
        public static readonly ZoomRateType OnlyDepth;

        /// <summary>横のみ</summary>
        public static readonly ZoomRateType OnlyWidth;

        /// <summary>縦横別々</summary>
        public static readonly ZoomRateType Different;

        /// <summary>同値</summary>
        public static readonly ZoomRateType Same;

        static ZoomRateType()
        {
            Normal = new ZoomRateType(nameof(Normal), 0x00);
            OnlyDepth = new ZoomRateType(nameof(OnlyDepth), 0x10);
            OnlyWidth = new ZoomRateType(nameof(OnlyWidth), 0x20);
            Different = new ZoomRateType(nameof(Different), 0x30);
            Same = new ZoomRateType(nameof(Same), 0x40);
        }

        private ZoomRateType(string id, byte code) : base(id)
        {
            Code = code;
        }

        /// <summary>値</summary>
        public byte Code { get; }

        /// <summary>
        ///     バイト値からインスタンスを取得する。
        /// </summary>
        /// <param name="code">バイト値</param>
        /// <returns>インスタンス</returns>
        public static ZoomRateType FromByte(byte code)
        {
            return AllItems.First(x => x.Code == code);
        }
    }
}
