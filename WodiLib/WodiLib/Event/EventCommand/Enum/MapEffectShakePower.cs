// ========================================
// Project Name : WodiLib
// File Name    : MapEffectShakePower.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.ComponentModel;
using System.Linq;
using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <summary>
    /// マップエフェクト・揺れの強さ
    /// </summary>
    public class MapEffectShakePower : TypeSafeEnum<MapEffectShakePower>
    {
        /// <summary>強さ1</summary>
        public static readonly MapEffectShakePower Power1;

        /// <summary>強さ2</summary>
        public static readonly MapEffectShakePower Power2;

        /// <summary>強さ3</summary>
        public static readonly MapEffectShakePower Power3;

        /// <summary>強さ4</summary>
        public static readonly MapEffectShakePower Power4;

        /// <summary>強さ5</summary>
        public static readonly MapEffectShakePower Power5;

        /// <summary>強さ6</summary>
        public static readonly MapEffectShakePower Power6;

        /// <summary>強さ7</summary>
        public static readonly MapEffectShakePower Power7;

        /// <summary>値</summary>
        public byte Code { get; }

        /// <summary>イベントコマンド文</summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal string EventCommandSentence { get; }

        static MapEffectShakePower()
        {
            Power1 = new MapEffectShakePower(nameof(Power1), 0x00,
                "強さ1");
            Power2 = new MapEffectShakePower(nameof(Power2), 0x01,
                "強さ2");
            Power3 = new MapEffectShakePower(nameof(Power3), 0x02,
                "強さ3");
            Power4 = new MapEffectShakePower(nameof(Power4), 0x03,
                "強さ4");
            Power5 = new MapEffectShakePower(nameof(Power5), 0x04,
                "強さ5");
            Power6 = new MapEffectShakePower(nameof(Power6), 0x05,
                "強さ6");
            Power7 = new MapEffectShakePower(nameof(Power7), 0x06,
                "強さ7");
        }

        private MapEffectShakePower(string id, byte code,
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
        public static MapEffectShakePower FromByte(byte code)
        {
            return AllItems.First(x => x.Code == code);
        }
    }
}
