// ========================================
// Project Name : WodiLib
// File Name    : TwoDimensionalCollectionChangeEventArgsFactory.ResetArgsFactory.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;

namespace WodiLib.Sys.Collections
{
    internal partial class TwoDimensionalCollectionChangeEventArgsFactory<T>
    {
        private static class ResetArgsFactory
        {
            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
            //     Public Static Methods
            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

            public static IEnumerable<TwoDimensionalCollectionChangeEventInternalArgs<T>> CreateSingle(
                Direction execDirection, T[][] oldItems, T[][] newItems)
            {
                return new[]
                {
                    TwoDimensionalCollectionChangeEventInternalArgs<T>.CreateResetArgs(
                        oldItems, newItems, 0, 0, execDirection),
                };
            }

            public static IEnumerable<TwoDimensionalCollectionChangeEventInternalArgs<T>> CreateSingle(
                Direction execDirection, T[][] oldItems, T[][] newItems, int row, int column)
            {
                return new[]
                {
                    TwoDimensionalCollectionChangeEventInternalArgs<T>.CreateResetArgs(
                        oldItems, newItems, row, column, execDirection),
                };
            }
        }
    }
}
