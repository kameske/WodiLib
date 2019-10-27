// ========================================
// Project Name : WodiLib
// File Name    : ScrollSpeed.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.ComponentModel;
using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <summary>
    /// 画面スクロールスピード
    /// </summary>
    public class ScrollSpeed : TypeSafeEnum<ScrollSpeed>
    {
        /// <summary>1/8倍速</summary>
        public static readonly ScrollSpeed Speed1of8;

        /// <summary>1/4倍速</summary>
        public static readonly ScrollSpeed Speed1of4x;

        /// <summary>1/2倍速</summary>
        public static readonly ScrollSpeed Speed1of2x;

        /// <summary>1倍速</summary>
        public static readonly ScrollSpeed Speed1x;

        /// <summary>2倍速</summary>
        public static readonly ScrollSpeed Speed2x;

        /// <summary>4倍速</summary>
        public static readonly ScrollSpeed Speed4x;

        /// <summary>8倍速</summary>
        public static readonly ScrollSpeed Speed8x;

        /// <summary>16倍速</summary>
        public static readonly ScrollSpeed Speed16;

        /// <summary>32倍速</summary>
        public static readonly ScrollSpeed Speed32;

        /// <summary>64倍速</summary>
        public static readonly ScrollSpeed Speed64;

        /// <summary>瞬間</summary>
        public static readonly ScrollSpeed Instant;

        /// <summary>値</summary>
        public byte Code { get; }

        /// <summary>イベントコマンド文字列</summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal string EventCommandSentence { get; }

        static ScrollSpeed()
        {
            Speed1of8 = new ScrollSpeed(nameof(Speed1of8), 0x00,
                "1/8倍速");
            Speed1of4x = new ScrollSpeed(nameof(Speed1of4x), 0x10,
                "1/4倍速");
            Speed1of2x = new ScrollSpeed(nameof(Speed1of2x), 0x20,
                "1/2倍速");
            Speed1x = new ScrollSpeed(nameof(Speed1x), 0x30,
                "1倍速");
            Speed2x = new ScrollSpeed(nameof(Speed2x), 0x40,
                "2倍速");
            Speed4x = new ScrollSpeed(nameof(Speed4x), 0x50,
                "4倍速");
            Speed8x = new ScrollSpeed(nameof(Speed8x), 0x60,
                "8倍速");
            Speed16 = new ScrollSpeed(nameof(Speed16), 0x70,
                "16倍速");
            Speed32 = new ScrollSpeed(nameof(Speed32), 0x90,
                "32倍速");
            Speed64 = new ScrollSpeed(nameof(Speed64), 0xA0,
                "64倍速");
            Instant = new ScrollSpeed(nameof(Instant), 0x80,
                "瞬間倍速");
        }

        private ScrollSpeed(string id, byte code,
            string eventCommandSentence) : base(id)
        {
            Code = code;
            EventCommandSentence = eventCommandSentence;
        }

        /// <summary>
        /// バイト値からインスタンスを取得する。
        /// </summary>
        /// <param name="code">バイト値</param>
        /// <returns>インスタンス</returns>
        public static ScrollSpeed FromByte(byte code)
        {
            return _FindFirst(x => x.Code == code);
        }
    }
}