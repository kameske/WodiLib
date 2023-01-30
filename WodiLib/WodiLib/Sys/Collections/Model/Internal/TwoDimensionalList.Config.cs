// ========================================
// Project Name : WodiLib
// File Name    : TwoDimensionalList.Config.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;

namespace WodiLib.Sys.Collections
{
    internal partial class TwoDimensionalList<TRow, TItem>
    {
        /// <summary>
        ///     二次元リスト設定
        /// </summary>
        public record Config(
            Func<IEnumerable<TItem>, TRow> RowFactoryFromItems,
            Func<int, int, TItem> ItemFactory,
            Func<TItem, TItem, bool> ItemComparer,
            Func<TwoDimensionalList<TRow, TItem>, ITwoDimensionalListValidator<TRow, TItem>?>
                ValidatorFactory
        )
        {
            public int MaxRowCapacity { get; init; } = int.MaxValue;
            public int MinRowCapacity { get; init; } = 0;
            public int MaxColumnCapacity { get; init; } = int.MaxValue;
            public int MinColumnCapacity { get; init; } = 0;
        }
    }
}
