// ========================================
// Project Name : WodiLib
// File Name    : EventCommandColorSet.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Drawing;
using WodiLib.Ini;
using WodiLib.Sys;

namespace WodiLib.Project
{
    /// <summary>
    /// イベントコマンド色
    /// </summary>
    public class EventCommandColorSet : TypeSafeEnum<EventCommandColorSet>
    {
        /// <summary>黒セット</summary>
        public static readonly EventCommandColorSet Black;

        /// <summary>緑セット</summary>
        public static readonly EventCommandColorSet Green;

        /// <summary>紅</summary>
        public static readonly EventCommandColorSet DeepRed;

        /// <summary>エメラルドグリーンセット</summary>
        public static readonly EventCommandColorSet EmeraldGreen;

        /// <summary>マゼンタセット</summary>
        public static readonly EventCommandColorSet Magenta;

        /// <summary>朱セット</summary>
        public static readonly EventCommandColorSet Vermilion;

        /// <summary>金セット</summary>
        public static readonly EventCommandColorSet Gold;

        /// <summary>若竹色セット</summary>
        public static readonly EventCommandColorSet BrightGreen;

        /// <summary>黄緑セット</summary>
        public static readonly EventCommandColorSet YellowGreen;

        /// <summary>桔梗セット</summary>
        public static readonly EventCommandColorSet DarkViolet;

        /// <summary>紫セット</summary>
        public static readonly EventCommandColorSet Purple;

        /// <summary>灰セット</summary>
        public static readonly EventCommandColorSet Gray;

        /// <summary>旧バージョン色</summary>
        public EventCommandColor OldColor { get; }

        /// <summary>タイプ1色</summary>
        public EventCommandColor Type1Color { get; }

        /// <summary>タイプ2色</summary>
        public EventCommandColor Type2Color { get; }

        static EventCommandColorSet()
        {
            Black = new EventCommandColorSet(nameof(Black),
                EventCommandColor.Black,
                EventCommandColor.Black, EventCommandColor.Black);
            Green = new EventCommandColorSet(nameof(Green),
                EventCommandColor.ShadowGreen,
                EventCommandColor.Green, EventCommandColor.Wisteria);
            DeepRed = new EventCommandColorSet(nameof(DeepRed),
                EventCommandColor.Caliente,
                EventCommandColor.DeepRed, EventCommandColor.Brawn);
            Magenta = new EventCommandColorSet(nameof(Magenta),
                EventCommandColor.Magenta, // Ver 2.00 より前には存在しないコマンド色
                EventCommandColor.Magenta, EventCommandColor.BrightYellow);
            EmeraldGreen = new EventCommandColorSet(nameof(EmeraldGreen),
                EventCommandColor.EmeraldGreen, // Ver 2.00 より前には存在しないコマンド色
                EventCommandColor.EmeraldGreen, EventCommandColor.FogBlue);
            Vermilion = new EventCommandColorSet(nameof(Vermilion),
                EventCommandColor.Begonia,
                EventCommandColor.Vermilion, EventCommandColor.Blue);
            Gold = new EventCommandColorSet(nameof(Gold),
                EventCommandColor.YellowGreen,
                EventCommandColor.Gold, EventCommandColor.Topaz);
            BrightGreen = new EventCommandColorSet(nameof(BrightGreen),
                EventCommandColor.DarkYellowGreen,
                EventCommandColor.BrightGreen, EventCommandColor.Topaz);
            YellowGreen = new EventCommandColorSet(nameof(YellowGreen),
                EventCommandColor.Black,
                EventCommandColor.LightYellowGreen, EventCommandColor.BrightYellowGreen);
            DarkViolet = new EventCommandColorSet(nameof(DarkViolet),
                EventCommandColor.ShadowYellowGreen,
                EventCommandColor.DarkViolet, EventCommandColor.CobaltBlue);
            Purple = new EventCommandColorSet(nameof(Purple),
                EventCommandColor.Black,
                EventCommandColor.Purple, EventCommandColor.DarkBrown);
            Gray = new EventCommandColorSet(nameof(Gray),
                EventCommandColor.Gray,
                EventCommandColor.Gray, EventCommandColor.Gray);
        }

        private EventCommandColorSet(string id, EventCommandColor oldColor,
            EventCommandColor type1Color,
            EventCommandColor type2Color) : base(id)
        {
            OldColor = oldColor;
            Type1Color = type1Color;
            Type2Color = type2Color;
        }

        /// <summary>
        /// コマンドカラー種別からコマンドカラーを取得する。
        /// </summary>
        /// <param name="type">[NotNull] コマンドカラー種別</param>
        /// <returns>コマンドカラー</returns>
        /// <exception cref="ArgumentNullException">type が null の場合</exception>
        public Color GetCommandColor(CommandColorType type)
        {
            if (type is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(type)));

            if (type == CommandColorType.Old) return OldColor.Color;
            if (type == CommandColorType.Type0) return Type1Color.Color;
            if (type == CommandColorType.Type1) return Type2Color.Color;

            // 通常ここへは来ない
            throw new InvalidOperationException();
        }

        /// <summary>
        /// IDからインスタンスを取得する。
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>IDで検索したインスタンス</returns>
        public static EventCommandColorSet FromId(string id)
        {
            return _FindFirst(x => x.Id.Equals(id));
        }
    }
}