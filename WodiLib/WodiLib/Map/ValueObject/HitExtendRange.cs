// ========================================
// Project Name : WodiLib
// File Name    : HitExtendRange.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Sys;

namespace WodiLib.Map
{
    /// <summary>
    ///     接触範囲拡張
    /// </summary>
    [CommonMultiValueObject]
    public partial record HitExtendRange
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>幅</summary>
        public HitExtendSideLength Width { get; init; }

        /// <summary>高さ</summary>
        public HitExtendSideLength Height { get; init; }
    }
}
