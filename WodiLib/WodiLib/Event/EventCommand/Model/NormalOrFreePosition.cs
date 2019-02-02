// ========================================
// Project Name : WodiLib
// File Name    : NormalOrFreePosition.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.Event.EventCommand
{
    /// <summary>
    /// ピクチャ表示位置・通常または自由変形
    /// </summary>
    public class NormalOrFreePosition
    {
        /// <summary>通常座標X</summary>
        public int NormalPositionX { get; set; }

        /// <summary>通常座標Y</summary>
        public int NormalPositionY { get; set; }

        /// <summary>自由変形座標左上X</summary>
        public int FreePositionLeftUpX { get; set; }

        /// <summary>自由変形座標左上Y</summary>
        public int FreePositionLeftUpY { get; set; }

        /// <summary>自由変形座標左下X</summary>
        public int FreePositionLeftDownX { get; set; }

        /// <summary>自由変形座標左上Y</summary>
        public int FreePositionLeftDownY { get; set; }

        /// <summary>自由変形座標右上X</summary>
        public int FreePositionRightUpX { get; set; }

        /// <summary>自由変形座標右上Y</summary>
        public int FreePositionRightUpY { get; set; }

        /// <summary>自由変形座標右下X</summary>
        public int FreePositionRightDownX { get; set; }

        /// <summary>自由変形座標右下Y</summary>
        public int FreePositionRightDownY { get; set; }
    }
}