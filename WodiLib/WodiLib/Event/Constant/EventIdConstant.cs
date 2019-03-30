// ========================================
// Project Name : WodiLib
// File Name    : EventIdConstant.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;

namespace WodiLib.Event
{
    /// <summary>
    ///     イベントIDに関する定数クラス
    /// </summary>
    [Obsolete]
    public static class EventIdConstant
    {
        /// <summary>「このイベント」を表す数値</summary>
        public static int ThisEventId = -1;

        /// <summary>「主人公」を表す数値</summary>
        public static int Hero = -2;

        /// <summary>「仲間1」を表す数値</summary>
        public static int Member1 = -3;

        /// <summary>「仲間2」を表す数値</summary>
        public static int Member2 = -4;

        /// <summary>「仲間3」を表す数値</summary>
        public static int Member3 = -5;

        /// <summary>「仲間4」を表す数値</summary>
        public static int Member4 = -6;

        /// <summary>「仲間5」を表す数値</summary>
        public static int Member5 = -7;

        /// <summary>コモンイベント指定時のオフセット</summary>
        public static int CommonEventOffset = 500000;

        /// <summary>コモンイベントを相対指定したときのオフセット（基準値）</summary>
        public static int CommonEventRelativeOffset = 600100;

        /// <summary>コモンイベントを相対指定したときのオフセット（最小値）</summary>
        public static int CommonEventRelativeOffset_Min = 600050;

        /// <summary>コモンイベントを相対指定したときのオフセット（最大値）</summary>
        public static int CommonEventRelativeOffset_Max = 600150;

        /// <summary>コモンイベントID（最小値）</summary>
        public static int CommonEventId_Min = 0;

        /// <summary>コモンイベントID（最大値）</summary>
        public static int CommonEventId_Max = 9999;

        /// <summary>マップイベントID（最小値）</summary>
        public static int MapEventId_Min = 0;

        /// <summary>マップイベントID（最大値）</summary>
        public static int MapEventId_Max = 9999;
    }
}