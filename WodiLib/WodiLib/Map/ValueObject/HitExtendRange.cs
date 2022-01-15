// ========================================
// Project Name : WodiLib
// File Name    : HitExtendRange.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using WodiLib.Sys;

namespace WodiLib.Map
{
    /// <summary>
    ///     接触範囲拡張
    /// </summary>
    [CommonMultiValueObject]
    public partial record HitExtendRange
    {
        /// <summary>幅</summary>
        public HitExtendSideLength Width { get; init; } = 0;

        /// <summary>高さ</summary>
        public HitExtendSideLength Height { get; init; } = 0;

        [Obsolete]
        public HitExtendRange(HitExtendSideLength width, HitExtendSideLength height)
        {
            Width = width;
            Height = height;
        }
    }
}
