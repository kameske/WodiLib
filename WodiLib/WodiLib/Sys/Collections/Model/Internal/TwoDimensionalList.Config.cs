// ========================================
// Project Name : WodiLib
// File Name    : TwoDimensionalList.Config.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;

namespace WodiLib.Sys.Collections
{
    internal partial class TwoDimensionalList<T>
    {
        /// <summary>
        ///     二次元リスト設定
        /// </summary>
        public class Config
        {
            public Func<int, int, T> ItemFactory { get; init; } = default!;

            public int MaxRowCapacity { get; init; } = int.MaxValue;
            public int MinRowCapacity { get; init; }
            public int MaxColumnCapacity { get; init; } = int.MaxValue;
            public int MinColumnCapacity { get; init; }

            public Func<TwoDimensionalList<T>, ITwoDimensionalListValidator<T, T>?>
                ValidatorFactory { get; init; } = default!;
        }
    }
}
