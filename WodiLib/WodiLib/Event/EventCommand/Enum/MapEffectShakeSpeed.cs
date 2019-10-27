// ========================================
// Project Name : WodiLib
// File Name    : MapEffectShakeSpeed.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.ComponentModel;
using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <summary>
    /// マップエフェクト・揺れの速さ
    /// </summary>
    public class MapEffectShakeSpeed : TypeSafeEnum<MapEffectShakeSpeed>
    {
        /// <summary>速度1</summary>
        public static readonly MapEffectShakeSpeed Speed1;

        /// <summary>速度2</summary>
        public static readonly MapEffectShakeSpeed Speed2;

        /// <summary>速度3</summary>
        public static readonly MapEffectShakeSpeed Speed3;

        /// <summary>速度4</summary>
        public static readonly MapEffectShakeSpeed Speed4;

        /// <summary>速度5</summary>
        public static readonly MapEffectShakeSpeed Speed5;

        /// <summary>速度6</summary>
        public static readonly MapEffectShakeSpeed Speed6;

        /// <summary>速度7</summary>
        public static readonly MapEffectShakeSpeed Speed7;

        /// <summary>値</summary>
        public byte Code { get; }

        /// <summary>イベントコマンド文</summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal string EventCommandSentence { get; }

        static MapEffectShakeSpeed()
        {
            Speed1 = new MapEffectShakeSpeed(nameof(Speed1), 0x00,
                "速度1");
            Speed2 = new MapEffectShakeSpeed(nameof(Speed2), 0x10,
                "速度2");
            Speed3 = new MapEffectShakeSpeed(nameof(Speed3), 0x20,
                "速度3");
            Speed4 = new MapEffectShakeSpeed(nameof(Speed4), 0x30,
                "速度4");
            Speed5 = new MapEffectShakeSpeed(nameof(Speed5), 0x40,
                "速度5");
            Speed6 = new MapEffectShakeSpeed(nameof(Speed6), 0x50,
                "速度6");
            Speed7 = new MapEffectShakeSpeed(nameof(Speed7), 0x60,
                "速度7");
        }

        private MapEffectShakeSpeed(string id, byte code,
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
        public static MapEffectShakeSpeed FromByte(byte code)
        {
            return _FindFirst(x => x.Code == code);
        }
    }
}