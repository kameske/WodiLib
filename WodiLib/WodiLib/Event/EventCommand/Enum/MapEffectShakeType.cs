// ========================================
// Project Name : WodiLib
// File Name    : MapEffectShakeType.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <summary>
    /// マップエフェクト・揺れ種別
    /// </summary>
    public class MapEffectShakeType : TypeSafeEnum<MapEffectShakeType>
    {
        /// <summary>縦揺れ</summary>
        public static readonly MapEffectShakeType Vertical;

        /// <summary>横揺れ</summary>
        public static readonly MapEffectShakeType Horizontal;

        /// <summary>揺れ停止</summary>
        public static readonly MapEffectShakeType Stop;

        /// <summary>値</summary>
        public byte Code { get; }

        static MapEffectShakeType()
        {
            Vertical = new MapEffectShakeType(nameof(Vertical), 0x00);
            Horizontal = new MapEffectShakeType(nameof(Horizontal), 0x01);
            Stop = new MapEffectShakeType(nameof(Stop), 0x02);
        }

        private MapEffectShakeType(string id, byte code) : base(id)
        {
            Code = code;
        }

        /// <summary>
        /// バイト値からインスタンスを取得する。
        /// </summary>
        /// <param name="code">バイト値</param>
        /// <returns>インスタンス</returns>
        public static MapEffectShakeType FromByte(byte code)
        {
            return _FindFirst(x => x.Code == code);
        }
    }
}