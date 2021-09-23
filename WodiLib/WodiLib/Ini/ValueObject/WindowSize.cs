// ========================================
// Project Name : WodiLib
// File Name    : WindowSize.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

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
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>サイズX</summary>
        public SideLength X { get; init; }

        /// <summary>サイズY</summary>
        public SideLength Y { get; init; }
    }
}
