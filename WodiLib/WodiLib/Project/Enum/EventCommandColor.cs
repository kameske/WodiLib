// ========================================
// Project Name : WodiLib
// File Name    : EventCommandColor.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Drawing;
using WodiLib.Sys;

namespace WodiLib.Project
{
    /// <summary>
    ///     イベントコマンド色
    /// </summary>
    public record EventCommandColor : TypeSafeEnum<EventCommandColor>
    {
        /// <summary>黒</summary>
        public static readonly EventCommandColor Black;

        /// <summary>緑</summary>
        public static readonly EventCommandColor Green;

        /// <summary>暗い緑</summary>
        public static readonly EventCommandColor ShadowGreen;

        /// <summary>藤</summary>
        public static readonly EventCommandColor Wisteria;

        /// <summary>紅</summary>
        public static readonly EventCommandColor DeepRed;

        /// <summary>暗い赤</summary>
        public static readonly EventCommandColor Caliente;

        /// <summary>茶</summary>
        public static readonly EventCommandColor Brawn;

        /// <summary>エメラルドグリーン</summary>
        public static readonly EventCommandColor EmeraldGreen;

        /// <summary>フォグブルー</summary>
        public static readonly EventCommandColor FogBlue;

        /// <summary>マゼンタ</summary>
        public static readonly EventCommandColor Magenta;

        /// <summary>山吹色</summary>
        public static readonly EventCommandColor BrightYellow;

        /// <summary>ベゴニア</summary>
        public static readonly EventCommandColor Begonia;

        /// <summary>朱</summary>
        public static readonly EventCommandColor Vermilion;

        /// <summary>青</summary>
        public static readonly EventCommandColor Blue;

        /// <summary>金</summary>
        public static readonly EventCommandColor Gold;

        /// <summary>トパーズ</summary>
        public static readonly EventCommandColor Topaz;

        /// <summary>若竹色</summary>
        public static readonly EventCommandColor BrightGreen;

        /// <summary>薄い黄緑</summary>
        public static readonly EventCommandColor LightYellowGreen;

        /// <summary>明るい黄緑</summary>
        public static readonly EventCommandColor BrightYellowGreen;

        /// <summary>黄緑</summary>
        public static readonly EventCommandColor YellowGreen;

        /// <summary>暗黄緑</summary>
        public static readonly EventCommandColor DarkYellowGreen;

        /// <summary>暗黄緑2</summary>
        public static readonly EventCommandColor ShadowYellowGreen;

        /// <summary>桔梗</summary>
        public static readonly EventCommandColor DarkViolet;

        /// <summary>コバルトブルー</summary>
        public static readonly EventCommandColor CobaltBlue;

        /// <summary>紫</summary>
        public static readonly EventCommandColor Purple;

        /// <summary>焦げ茶</summary>
        public static readonly EventCommandColor DarkBrown;

        /// <summary>暗い黄色</summary>
        public static readonly EventCommandColor ShadowYellow;

        /// <summary>灰</summary>
        public static readonly EventCommandColor Gray;

        /// <summary>色</summary>
        public Color Color { get; }

        static EventCommandColor()
        {
            Black = new EventCommandColor(nameof(Black),
                Color.FromArgb(0x282832));
            Green = new EventCommandColor(nameof(Green),
                Color.FromArgb(0x00a000));
            ShadowGreen = new EventCommandColor(nameof(ShadowGreen),
                Color.FromArgb(0x3c913c));
            Wisteria = new EventCommandColor(nameof(Wisteria),
                Color.FromArgb(0x9696be));
            DeepRed = new EventCommandColor(nameof(DeepRed),
                Color.FromArgb(0x820000));
            Caliente = new EventCommandColor(nameof(Caliente),
                Color.FromArgb(0x8c2828));
            Brawn = new EventCommandColor(nameof(Brawn),
                Color.FromArgb(0x734b00));
            EmeraldGreen = new EventCommandColor(nameof(EmeraldGreen),
                Color.FromArgb(0x3ca578));
            FogBlue = new EventCommandColor(nameof(FogBlue),
                Color.FromArgb(0x828d9b));
            Magenta = new EventCommandColor(nameof(Magenta),
                Color.FromArgb(0xff0ab9));
            BrightYellow = new EventCommandColor(nameof(BrightYellow),
                Color.FromArgb(0xffb414));
            Begonia = new EventCommandColor(nameof(Begonia),
                Color.FromArgb(0xff6464));
            Vermilion = new EventCommandColor(nameof(Vermilion),
                Color.FromArgb(0xff4646));
            Blue = new EventCommandColor(nameof(Blue),
                Color.FromArgb(0x005fff));
            Gold = new EventCommandColor(nameof(Gold),
                Color.FromArgb(0xc8a000));
            Topaz = new EventCommandColor(nameof(Topaz),
                Color.FromArgb(0xc88200));
            BrightGreen = new EventCommandColor(nameof(BrightGreen),
                Color.FromArgb(0xb4b400));
            LightYellowGreen = new EventCommandColor(nameof(LightYellowGreen),
                Color.FromArgb(0xb4c814));
            BrightYellowGreen = new EventCommandColor(nameof(BrightYellowGreen),
                Color.FromArgb(0xb4b414));
            YellowGreen = new EventCommandColor(nameof(YellowGreen),
                Color.FromArgb(0xafaf13));
            DarkYellowGreen = new EventCommandColor(nameof(DarkYellowGreen),
                Color.FromArgb(0x747427));
            ShadowYellowGreen = new EventCommandColor(nameof(ShadowYellowGreen),
                Color.FromArgb(0x7d7d30));
            DarkViolet = new EventCommandColor(nameof(DarkViolet),
                Color.FromArgb(0x6469c8));
            CobaltBlue = new EventCommandColor(nameof(CobaltBlue),
                Color.FromArgb(0x0087dc));
            Purple = new EventCommandColor(nameof(Purple),
                Color.FromArgb(0xa000a5));
            DarkBrown = new EventCommandColor(nameof(DarkBrown),
                Color.FromArgb(0x785010));
            ShadowYellow = new EventCommandColor(nameof(ShadowYellow),
                Color.FromArgb(0x787828));
            Gray = new EventCommandColor(nameof(Gray),
                Color.FromArgb(0x828282));
        }

        private EventCommandColor(string id, Color color) : base(id)
        {
            Color = color;
        }

        /// <inheritdoc/>
        public override string ToString()
            => base.ToString();
    }
}
