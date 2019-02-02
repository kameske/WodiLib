// ========================================
// Project Name : WodiLib
// File Name    : PictureEffectType.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <summary>
    /// ピクチャエフェクト種別
    /// </summary>
    public class PictureEffectType : TypeSafeEnum<PictureEffectType>
    {
        /// <summary>フラッシュ</summary>
        public static readonly PictureEffectType Flush;

        /// <summary>色調整</summary>
        public static readonly PictureEffectType ColorCorrect;

        /// <summary>描画座標シフト</summary>
        public static readonly PictureEffectType DrawPositionShift;

        /// <summary>シェイク</summary>
        public static readonly PictureEffectType Shake;

        /// <summary>ズーム</summary>
        public static readonly PictureEffectType Zoom;

        /// <summary>点滅（明滅）</summary>
        public static readonly PictureEffectType SwitchFlicker;

        /// <summary>点滅（自動フラッシュ）</summary>
        public static readonly PictureEffectType SwitchAutoFlush;

        /// <summary>自動拡大縮小</summary>
        public static readonly PictureEffectType AutoEnlarge;

        /// <summary>パターン切り替え（1回）</summary>
        public static readonly PictureEffectType AutoPatternSwitchOnce;

        /// <summary>パターン切り替え（ループ）</summary>
        public static readonly PictureEffectType AutoPatternSwitchLoop;

        /// <summary>パターン切り替え（往復）</summary>
        public static readonly PictureEffectType AutoPatternSwitchRoundTrip;

        /// <summary>値</summary>
        public byte Code { get; }

        static PictureEffectType()
        {
            Flush = new PictureEffectType(nameof(Flush), 0x00);
            ColorCorrect = new PictureEffectType(nameof(ColorCorrect), 0x10);
            DrawPositionShift = new PictureEffectType(nameof(DrawPositionShift), 0x20);
            Shake = new PictureEffectType(nameof(Shake), 0x30);
            Zoom = new PictureEffectType(nameof(Zoom), 0x40);
            SwitchFlicker = new PictureEffectType(nameof(SwitchFlicker), 0x50);
            SwitchAutoFlush = new PictureEffectType(nameof(SwitchAutoFlush), 0x60);
            AutoEnlarge = new PictureEffectType(nameof(AutoEnlarge), 0x70);
            AutoPatternSwitchOnce = new PictureEffectType(nameof(AutoPatternSwitchOnce), 0x80);
            AutoPatternSwitchLoop = new PictureEffectType(nameof(AutoPatternSwitchLoop), 0x90);
            AutoPatternSwitchRoundTrip = new PictureEffectType(nameof(AutoPatternSwitchRoundTrip), 0xA0);
        }

        private PictureEffectType(string id, byte code) : base(id)
        {
            Code = code;
        }

        /// <summary>
        /// バイト値からインスタンスを取得する。
        /// </summary>
        /// <param name="code">バイト値</param>
        /// <returns>インスタンス</returns>
        public static PictureEffectType FromByte(byte code)
        {
            return _FindFirst(x => x.Code == code);
        }
    }
}