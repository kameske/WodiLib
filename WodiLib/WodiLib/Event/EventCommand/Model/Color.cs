// ========================================
// Project Name : WodiLib
// File Name    : Color.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.Event.EventCommand
{
    /// <summary>
    /// 色クラス
    /// </summary>
    public class Color
    {
        /// <summary>赤（int）</summary>
        public int R { get; set; } = 100;

        /// <summary>緑（int）</summary>
        public int G { get; set; } = 100;

        /// <summary>青（int）</summary>
        public int B { get; set; } = 100;

        /// <summary>赤（byte）</summary>
        public byte ByteR
        {
            get => (byte) R;
            set => R = value;
        }

        /// <summary>緑（byte）</summary>
        public byte ByteG
        {
            get => (byte) G;
            set => G = value;
        }

        /// <summary>青（byte）</summary>
        public byte ByteB
        {
            get => (byte) B;
            set => B = value;
        }
    }
}