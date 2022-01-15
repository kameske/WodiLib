// ========================================
// Project Name : WodiLib
// File Name    : Position.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using WodiLib.Sys;

namespace WodiLib.Map
{
    /// <summary>
    ///     座標
    /// </summary>
    [CommonMultiValueObject]
    public partial record Position
    {
        /// <summary>X座標</summary>
        public MapCoordinate X { get; init; } = 0;

        /// <summary>Y座標</summary>
        public MapCoordinate Y { get; init; } = 0;

        [Obsolete]
        public Position(MapCoordinate x, MapCoordinate y)
        {
            X = x;
            Y = y;
        }
    }
}
