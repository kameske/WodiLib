// ========================================
// Project Name : WodiLib
// File Name    : Position.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Sys;

namespace WodiLib.Map
{
    /// <summary>
    ///     座標
    /// </summary>
    [CommonMultiValueObject]
    public partial record Position
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>X座標</summary>
        public MapCoordinate X { get; init; }

        /// <summary>Y座標</summary>
        public MapCoordinate Y { get; init; }
    }
}
