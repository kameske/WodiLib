// ========================================
// Project Name : WodiLib
// File Name    : CharaEffectType.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <summary>
    /// キャラエフェクト種別
    /// </summary>
    public class CharaEffectType : TypeSafeEnum<CharaEffectType>
    {
        /// <summary>フラッシュ</summary>
        public static readonly CharaEffectType Flush;

        /// <summary>シェイク</summary>
        public static readonly CharaEffectType Shake;

        /// <summary>点滅（明滅）</summary>
        public static readonly CharaEffectType Flicker;

        /// <summary>点滅（自動フラッシュ）</summary>
        public static readonly CharaEffectType AutoFlush;

        /// <summary>値</summary>
        public byte Code { get; }

        static CharaEffectType()
        {
            Flush = new CharaEffectType(nameof(Flush), 0x00);
            Shake = new CharaEffectType(nameof(Shake), 0x10);
            Flicker = new CharaEffectType(nameof(Flicker), 0x20);
            AutoFlush = new CharaEffectType(nameof(AutoFlush), 0x30);
        }

        private CharaEffectType(string id, byte code) : base(id)
        {
            Code = code;
        }

        /// <summary>
        /// バイト値から種別インスタンスを取得する。
        /// </summary>
        /// <param name="src">コード値</param>
        /// <returns>インスタンス</returns>
        public static CharaEffectType FromByte(byte src)
        {
            return _FindFirst(x => x.Code == src);
        }
    }
}