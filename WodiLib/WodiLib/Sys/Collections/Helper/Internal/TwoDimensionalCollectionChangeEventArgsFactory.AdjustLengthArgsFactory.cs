// ========================================
// Project Name : WodiLib
// File Name    : TwoDimensionalCollectionChangeEventArgsFactory.AdjustLengthArgsFactory.cs
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
        private static class AdjustLengthArgsFactory
        {
            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
            //     Public Static Methods
            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

            public static IEnumerable<TwoDimensionalCollectionChangeEventInternalArgs<T>> CreateSingle(
                Direction execDirection, Direction notifyDirection, T[][] oldItems, T[][] newItems)
            {
                var needTransposed = NeedTranspose(execDirection, notifyDirection);
                var fixedOldItems = oldItems.ToTransposedArrayIf(needTransposed);
                var fixedNewItems = newItems.ToTransposedArrayIf(needTransposed);

                if (IsInnerLengthChanged(fixedOldItems, fixedNewItems))
                {
                    return CreateSingle_Outer(execDirection, fixedOldItems, fixedNewItems);
                }

                if (IsOuterLengthChanged(fixedOldItems, fixedNewItems))
                {
                    return CreateSingle_Inner(execDirection, fixedOldItems, fixedNewItems);
                }

                return CreateSingle_Both(notifyDirection, fixedOldItems, fixedNewItems);
            }

            public static IEnumerable<TwoDimensionalCollectionChangeEventInternalArgs<T>> CreateLine(
                Direction execDirection, Direction notifyArgsDirection, T[][] oldItems, T[][] newItems)
            {
                var needTransposed = NeedTranspose(execDirection, notifyArgsDirection);
                var fixedOldItems = oldItems.ToTransposedArrayIf(needTransposed);
                var fixedNewItems = newItems.ToTransposedArrayIf(needTransposed);

                if (!IsInnerLengthChanged(fixedOldItems, fixedNewItems))
                {
                    return CreateLine_Outer(execDirection, notifyArgsDirection, fixedOldItems, fixedNewItems);
                }

                if (!IsOuterLengthChanged(fixedOldItems, fixedNewItems))
                {
                    return CreateLine_Inner(execDirection, fixedOldItems, fixedNewItems);
                }

                return CreateLine_Both(execDirection, notifyArgsDirection, fixedOldItems, fixedNewItems);
            }

            public static IEnumerable<TwoDimensionalCollectionChangeEventInternalArgs<T>> CreateMulti(
                Direction execDirection, T[][] oldItems, T[][] newItems)
            {
                var needTranspose = NeedTranspose(execDirection, Direction.Row);
                var fixedOldItems = oldItems.ToTransposedArrayIf(needTranspose);
                var fixedNewItems = newItems.ToTransposedArrayIf(needTranspose);

                var rowArgs = CreateMulti_Row(fixedOldItems, fixedNewItems);
                var columnArgs = CreateMulti_Column(fixedOldItems, fixedNewItems);

                return rowArgs.Concat(columnArgs);
            }

            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
            //     Private Static Methods
            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

            private static bool IsOuterLengthChanged(IReadOnlyCollection<T[]> fixedOldItems,
                IReadOnlyCollection<T[]> fixedNewItems)
            {
                var fixedOldItemsOuterLength = fixedOldItems.Count;
                var fixedNewItemsOuterLength = fixedNewItems.Count;

                return fixedOldItemsOuterLength == fixedNewItemsOuterLength;
            }

            private static bool IsInnerLengthChanged(T[][] fixedOldItems, T[][] fixedNewItems)
            {
                var fixedOldItemsInnerLength = fixedOldItems.GetInnerArrayLength();
                var fixedNewItemsInnerLength = fixedNewItems.GetInnerArrayLength();

                return fixedOldItemsInnerLength == fixedNewItemsInnerLength;
            }

            #region Single

            private static IEnumerable<TwoDimensionalCollectionChangeEventInternalArgs<T>>
                CreateSingle_Outer(Direction execDirection, IReadOnlyCollection<T[]> fixedOldItems,
                    IReadOnlyCollection<T[]> fixedNewItems)
            {
                var isAdded = fixedNewItems.Count > fixedOldItems.Count;
                return isAdded
                    ? CreateSingle_Outer_Add(execDirection, fixedOldItems, fixedNewItems)
                    : CreateSingle_Outer_Remove(execDirection, fixedOldItems, fixedNewItems);
            }

            private static IEnumerable<TwoDimensionalCollectionChangeEventInternalArgs<T>>
                CreateSingle_Outer_Add(Direction execDirection,
                    IReadOnlyCollection<T[]> fixedOldItems, IEnumerable<T[]> fixedNewItems)
            {
                var index = fixedOldItems.Count;
                var addItems = fixedNewItems.Skip(index).ToArray();
                return AddArgsFactory.CreateSingle(execDirection, index, addItems);
            }

            private static IEnumerable<TwoDimensionalCollectionChangeEventInternalArgs<T>>
                CreateSingle_Outer_Remove(Direction execDirection, IEnumerable<T[]> fixedOldItems,
                    IReadOnlyCollection<T[]> fixedNewItems)
            {
                var index = fixedNewItems.Count;
                var removeItems = fixedOldItems.Skip(index).ToArray();
                return RemoveArgsFactory.CreateSingle(execDirection, index, removeItems);
            }

            private static IEnumerable<TwoDimensionalCollectionChangeEventInternalArgs<T>>
                CreateSingle_Inner(Direction execDirection, T[][] fixedOldItems, T[][] fixedNewItems)
            {
                return SetArgsFactory.CreateSingle(execDirection, 0, 0, fixedOldItems, fixedNewItems);
            }

            private static IEnumerable<TwoDimensionalCollectionChangeEventInternalArgs<T>>
                CreateSingle_Both(Direction notifyDirection, T[][] fixedOldItems, T[][] fixedNewItems)
            {
                return new[]
                {
                    TwoDimensionalCollectionChangeEventInternalArgs<T>.CreateResetArgs(
                        fixedOldItems, fixedNewItems, 0, 0, notifyDirection)
                };
            }

            #endregion

            #region Line

            private static IEnumerable<TwoDimensionalCollectionChangeEventInternalArgs<T>>
                CreateLine_Outer(Direction execDirection, Direction notifyArgsDirection,
                    IReadOnlyCollection<T[]> fixedOldItems, IReadOnlyCollection<T[]> fixedNewItems)
            {
                var isAdded = fixedNewItems.Count > fixedOldItems.Count;

                return isAdded
                    ? CreateLine_Outer_Add(execDirection, notifyArgsDirection, fixedOldItems, fixedNewItems)
                    : CreateLine_Outer_Remove(execDirection, notifyArgsDirection, fixedOldItems, fixedNewItems);
            }

            private static IEnumerable<TwoDimensionalCollectionChangeEventInternalArgs<T>>
                CreateLine_Outer_Add(Direction execDirection, Direction notifyArgsDirection,
                    IEnumerable<T[]> fixedOldItems, IReadOnlyCollection<T[]> fixedNewItems)
            {
                var index = fixedNewItems.Count;
                var removeItems = fixedOldItems.Skip(index).ToArray();
                return AddArgsFactory.CreateLine(execDirection, notifyArgsDirection, index, removeItems);
            }

            private static IEnumerable<TwoDimensionalCollectionChangeEventInternalArgs<T>>
                CreateLine_Outer_Remove(Direction execDirection, Direction notifyArgsDirection,
                    IEnumerable<T[]> fixedOldItems, IReadOnlyCollection<T[]> fixedNewItems)
            {
                var index = fixedNewItems.Count;
                var removeItems = fixedOldItems.Skip(index).ToArray();
                return RemoveArgsFactory.CreateLine(execDirection, notifyArgsDirection, index, removeItems);
            }

            private static IEnumerable<TwoDimensionalCollectionChangeEventInternalArgs<T>>
                CreateLine_Inner(Direction execDirection, T[][] fixedOldItems, T[][] fixedNewItems)
            {
                return SetArgsFactory.CreateSingle(execDirection, 0, 0, fixedOldItems, fixedNewItems);
            }

            private static IEnumerable<TwoDimensionalCollectionChangeEventInternalArgs<T>>
                CreateLine_Both(Direction execDirection, Direction notifyArgsDirection,
                    T[][] fixedOldItems, T[][] fixedNewItems)
            {
                var isAdded = fixedNewItems.Length > fixedOldItems.Length;
                return isAdded
                    ? CreateLine_Both_Add(execDirection, notifyArgsDirection, fixedOldItems, fixedNewItems)
                    : CreateLine_Both_Remove(execDirection, notifyArgsDirection, fixedOldItems, fixedNewItems);
            }

            private static IEnumerable<TwoDimensionalCollectionChangeEventInternalArgs<T>>
                CreateLine_Both_Add(Direction execDirection, Direction notifyArgsDirection,
                    IReadOnlyCollection<T[]> fixedOldItems, T[][] fixedNewItems)
            {
                var replaceLength = fixedOldItems.Count;

                var replaceOldItems = fixedOldItems.Take(replaceLength).ToArray();
                var replaceNewItems = fixedNewItems.Take(replaceLength).ToArray();
                var addItems = fixedNewItems.Skip(replaceLength).ToArray();

                var replaceArgs = SetArgsFactory.CreateLine(execDirection, notifyArgsDirection,
                    0, 0, replaceOldItems, replaceNewItems);

                var addOrRemoveArgs = AddArgsFactory.CreateLine(execDirection,
                    notifyArgsDirection, replaceLength, addItems);

                return replaceArgs.Concat(addOrRemoveArgs);
            }

            private static IEnumerable<TwoDimensionalCollectionChangeEventInternalArgs<T>>
                CreateLine_Both_Remove(Direction execDirection, Direction notifyArgsDirection,
                    T[][] fixedOldItems, IReadOnlyCollection<T[]> fixedNewItems)
            {
                var replaceLength = fixedNewItems.Count;

                var replaceOldItems = fixedOldItems.Take(replaceLength).ToArray();
                var replaceNewItems = fixedNewItems.Take(replaceLength).ToArray();
                var removeItems = fixedOldItems.Skip(replaceLength).ToArray();

                var replaceArgs = SetArgsFactory.CreateLine(execDirection,
                    notifyArgsDirection, 0, 0, replaceOldItems, replaceNewItems);

                var addOrRemoveArgs = RemoveArgsFactory.CreateLine(execDirection,
                    notifyArgsDirection, replaceLength, removeItems);

                return replaceArgs.Concat(addOrRemoveArgs);
            }

            #endregion

            #region Multi

            private static IEnumerable<TwoDimensionalCollectionChangeEventInternalArgs<T>>
                CreateMulti_Row(IReadOnlyCollection<T[]> fixedOldItems, IReadOnlyCollection<T[]> fixedNewItems)
            {
                var oldLength = fixedOldItems.Count;
                var newLength = fixedNewItems.Count;

                return oldLength < newLength
                    ? CreateMulti_Row_Add(fixedOldItems, fixedNewItems)
                    : CreateMulti_Row_Remove(fixedOldItems, fixedNewItems);
            }

            private static IEnumerable<TwoDimensionalCollectionChangeEventInternalArgs<T>>
                CreateMulti_Row_Add(IReadOnlyCollection<T[]> fixedOldItems, IReadOnlyCollection<T[]> fixedNewItems)
            {
                var startIndex = fixedNewItems.Count - fixedOldItems.Count;

                return fixedNewItems.Skip(startIndex)
                    .SelectMany((line, rowIdx) => line.Select((item, colIdx)
                        => TwoDimensionalCollectionChangeEventInternalArgs<T>.CreateAddArgs(
                            rowIdx + startIndex, colIdx, item)));
            }

            private static IEnumerable<TwoDimensionalCollectionChangeEventInternalArgs<T>>
                CreateMulti_Row_Remove(IReadOnlyCollection<T[]> fixedOldItems, IReadOnlyCollection<T[]> fixedNewItems)
            {
                var startIndex = fixedOldItems.Count - fixedNewItems.Count;

                return fixedOldItems.Skip(startIndex)
                    .SelectMany((line, rowIdx) => line.Select((item, colIdx)
                        => TwoDimensionalCollectionChangeEventInternalArgs<T>.CreateRemoveArgs(
                            rowIdx + startIndex, colIdx, item)));
            }

            private static IEnumerable<TwoDimensionalCollectionChangeEventInternalArgs<T>>
                CreateMulti_Column(T[][] fixedOldItems, T[][] fixedNewItems)
            {
                var oldLength = fixedOldItems.GetInnerArrayLength();
                var newLength = fixedNewItems.GetInnerArrayLength();

                return oldLength < newLength
                    ? CreateMulti_Column_Add(fixedOldItems, fixedNewItems)
                    : CreateMulti_Column_Remove(fixedOldItems, fixedNewItems);
            }

            private static IEnumerable<TwoDimensionalCollectionChangeEventInternalArgs<T>>
                CreateMulti_Column_Add(T[][] fixedOldItems, T[][] fixedNewItems)
            {
                var startIndex = fixedNewItems.GetInnerArrayLength()
                                 - fixedOldItems.GetInnerArrayLength();

                return fixedNewItems.SelectMany((line, rowIdx)
                    => line.Skip(startIndex).Select((item, colIdx)
                        => TwoDimensionalCollectionChangeEventInternalArgs<T>.CreateAddArgs(
                            rowIdx, colIdx + startIndex, item)));
            }

            private static IEnumerable<TwoDimensionalCollectionChangeEventInternalArgs<T>>
                CreateMulti_Column_Remove(T[][] fixedOldItems, T[][] fixedNewItems)
            {
                var startIndex = fixedOldItems.GetInnerArrayLength()
                                 - fixedNewItems.GetInnerArrayLength();

                return fixedNewItems.SelectMany((line, rowIdx)
                    => line.Skip(startIndex).Select((item, colIdx)
                        => TwoDimensionalCollectionChangeEventInternalArgs<T>.CreateRemoveArgs(
                            rowIdx, colIdx + startIndex, item)));
            }

            #endregion
        }
    }
}
