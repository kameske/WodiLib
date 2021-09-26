// ========================================
// Project Name : WodiLib
// File Name    : TwoDimensionalCollectionChangeEventArgsFactory.SetArgsFactory.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using System.Linq;

namespace WodiLib.Sys.Collections
{
    internal partial class TwoDimensionalCollectionChangeEventArgsFactory<T>
    {
        private static class AddArgsFactory
        {
            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
            //     Public Static Methods
            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

            public static IEnumerable<TwoDimensionalCollectionChangeEventInternalArgs<T>> CreateSingle(
                Direction execDirection, int index, T[][] items)
            {
                return new[]
                {
                    TwoDimensionalCollectionChangeEventInternalArgs<T>.CreateAddArgs(
                        index, items, execDirection)
                };
            }

            public static IEnumerable<TwoDimensionalCollectionChangeEventInternalArgs<T>> CreateLine(
                Direction execDirection, Direction notifyArgsDirection, int index, T[][] items)
            {
                var needTranspose = NeedTranspose(execDirection, notifyArgsDirection);
                var fixedItems = items.ToTransposedArrayIf(needTranspose);

                return fixedItems.Select((item, idx)
                    => TwoDimensionalCollectionChangeEventInternalArgs<T>.CreateAddArgs(
                        index + idx, item, notifyArgsDirection));
            }

            public static IEnumerable<TwoDimensionalCollectionChangeEventInternalArgs<T>> CreateMulti(
                Direction execDirection, int index, T[][] items)
            {
                var needTranspose = NeedTranspose(execDirection, Direction.Row);
                var fixedItems = items.ToTransposedArrayIf(needTranspose);

                var row = execDirection != Direction.Column ? index : 0;
                var column = execDirection == Direction.Column ? index : 0;

                return fixedItems.SelectMany((line, outerIdx)
                    => line.Select((item, innerIdx)
                        => TwoDimensionalCollectionChangeEventInternalArgs<T>.CreateAddArgs(
                            row + outerIdx, column + innerIdx, item)));
            }
        }
    }
}
