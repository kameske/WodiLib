// ========================================
// Project Name : WodiLib
// File Name    : WindowPosition.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using WodiLib.Cmn;
using WodiLib.Sys;

namespace WodiLib.Ini
{
    /// <summary>
    ///     ウィンドウ座標
    /// </summary>
    [CommonMultiValueObject]
    public partial record WindowPosition
    {
        /// <summary>X座標</summary>
        public Coordinate X { get; init; } = 0;

        /// <summary>Y座標</summary>
        public Coordinate Y { get; init; } = 0;

        [Obsolete]
        public WindowPosition(Coordinate x, Coordinate y)
        {
            X = x;
            Y = y;
        }
    }
}
