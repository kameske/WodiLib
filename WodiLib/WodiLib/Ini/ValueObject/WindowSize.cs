// ========================================
// Project Name : WodiLib
// File Name    : WindowSize.cs
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
    ///     ウィンドウサイズ
    /// </summary>
    [CommonMultiValueObject]
    public partial record WindowSize
    {
        /// <summary>サイズX</summary>
        public SideLength X { get; init; } = 0;

        /// <summary>サイズY</summary>
        public SideLength Y { get; init; } = 0;

        [Obsolete]
        public WindowSize(SideLength x, SideLength y)
        {
            X = x;
            Y = y;
        }
    }
}
