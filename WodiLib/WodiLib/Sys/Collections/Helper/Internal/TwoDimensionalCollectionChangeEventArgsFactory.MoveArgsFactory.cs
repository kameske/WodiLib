// ========================================
// Project Name : WodiLib
// File Name    : TwoDimensionalCollectionChangeEventArgsFactory.MoveArgsFactory.cs
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
        private static class MoveArgsFactory
        {
            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
            //     Public Static Methods
            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

            public static IEnumerable<TwoDimensionalCollectionChangeEventInternalArgs<T>> CreateSingle(
                Direction execDirection, int oldIndex, int newIndex, T[][] items)
            {
                return new[]
                {
                    TwoDimensionalCollectionChangeEventInternalArgs<T>.CreateMoveArgs(
                        oldIndex, newIndex, items, execDirection)
                };
            }

            public static IEnumerable<TwoDimensionalCollectionChangeEventInternalArgs<T>> CreateLine(
                Direction execDirection, Direction notifyArgsDirection, int oldIndex, int newIndex, T[][] items)
            {
                var needTranspose = NeedTranspose(execDirection, notifyArgsDirection);
                var fixedItems = items.ToTransposedArrayIf(needTranspose);

                return fixedItems.Select((item, idx)
                    => TwoDimensionalCollectionChangeEventInternalArgs<T>.CreateMoveArgs(
                        oldIndex + idx, newIndex + idx, item, notifyArgsDirection));
            }

            public static IEnumerable<TwoDimensionalCollectionChangeEventInternalArgs<T>> CreateMulti(
                Direction execDirection, int oldIndex, int newIndex, T[][] items)
            {
                var needTranspose = NeedTranspose(execDirection, Direction.Row);
                var fixedItems = items.ToTransposedArrayIf(needTranspose);

                var oldRow = execDirection == Direction.Row ? oldIndex : 0;
                var oldColumn = execDirection == Direction.Column ? oldIndex : 0;
                var newRow = execDirection == Direction.Row ? newIndex : 0;
                var newColumn = execDirection == Direction.Column ? newIndex : 0;

                return fixedItems.SelectMany((line, outerIdx)
                    => line.Select((item, innerIdx)
                        => TwoDimensionalCollectionChangeEventInternalArgs<T>.CreateMoveArgs(
                            oldRow + outerIdx, oldColumn + innerIdx,
                            newRow + outerIdx, newColumn + innerIdx, item)));
            }
        }
    }
}
