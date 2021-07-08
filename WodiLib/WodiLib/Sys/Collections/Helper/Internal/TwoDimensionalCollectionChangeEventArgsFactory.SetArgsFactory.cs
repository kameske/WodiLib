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
        private static class SetArgsFactory
        {
            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
            //     Public Static Methods
            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

            public static IEnumerable<TwoDimensionalCollectionChangeEventInternalArgs<T>> CreateSingle(
                Direction execDirection, int row, int column, T[][] oldItems, T[][] newItems)
            {
                return new[]
                {
                    TwoDimensionalCollectionChangeEventInternalArgs<T>.CreateSetArgs(row, column,
                        oldItems, newItems, execDirection),
                };
            }

            public static IEnumerable<TwoDimensionalCollectionChangeEventInternalArgs<T>> CreateLine(
                Direction execDirection, Direction notifyArgsDirection, int row, int column, T[][] oldItems,
                T[][] newItems)
            {
                var rowIndexCorrect = notifyArgsDirection == Direction.Row ? 1 : 0;
                var colIndexCorrect = notifyArgsDirection == Direction.Column ? 1 : 0;

                var needTranspose = NeedTranspose(execDirection, notifyArgsDirection);
                var fixedOldItems = oldItems.ToTransposedArrayIf(needTranspose);
                var fixedNewItems = newItems.ToTransposedArrayIf(needTranspose);

                return fixedOldItems.Zip(fixedNewItems).Select((zip, idx)
                    => TwoDimensionalCollectionChangeEventInternalArgs<T>.CreateSetArgs(
                        row + idx * rowIndexCorrect, column + idx * colIndexCorrect,
                        zip.Item1, zip.Item2, notifyArgsDirection));
            }

            public static IEnumerable<TwoDimensionalCollectionChangeEventInternalArgs<T>> CreateMulti(
                Direction execDirection, int row, int column, T[][] oldItems, T[][] newItems)
            {
                var needTranspose = NeedTranspose(execDirection, Direction.Row);

                var fixedOldItems = oldItems.ToTransposedArrayIf(needTranspose);
                var fixedNewItems = newItems.ToTransposedArrayIf(needTranspose);

                return fixedOldItems.Zip(fixedNewItems).SelectMany((outerZip, outerIdx)
                    => outerZip.Item1.Zip(outerZip.Item2).Select((innerZip, innerIdx)
                        => TwoDimensionalCollectionChangeEventInternalArgs<T>.CreateSetArgs(
                            row + outerIdx, column + innerIdx,
                            innerZip.Item1, innerZip.Item2)));
            }
        }
    }
}
