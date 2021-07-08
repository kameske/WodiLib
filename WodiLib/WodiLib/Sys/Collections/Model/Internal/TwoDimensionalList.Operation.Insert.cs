// ========================================
// Project Name : WodiLib
// File Name    : TwoDimensionalList.Operation.Insert.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Linq;

namespace WodiLib.Sys.Collections
{
    internal partial class TwoDimensionalList<T>
    {
        private static partial class Operation
        {
            private abstract class Insert : OperationBase
            {
                protected override TwoDimensionalCollectionChangeEventArgsFactory<T>
                    TwoDimensionalCollectionChangeEventArgsFactory { get; }

                protected Insert(TwoDimensionalList<T> target,
                    int index, T[][] items, Direction direction, Action coreAction) : base(target, coreAction)
                {
                    var oldItems = target.ToTwoDimensionalArray()
                        .ToTransposedArrayIf(direction == Direction.Column);
                    TwoDimensionalCollectionChangeEventArgsFactory =
                        TwoDimensionalCollectionChangeEventArgsFactory<T>.CreateInsert(
                            target, index, oldItems, items, direction);
                }
            }

            private class InsertRow : Insert
            {
                protected override CollectionChangeEventArgsFactory<IReadOnlyList<T>>
                    CollectionChangeEventArgsFactory { get; }

                protected override string[] NotifyProperties { get; }

                public InsertRow(TwoDimensionalList<T> target, int index, T[][] items, Action coreAction)
                    : base(target, index, items, Direction.Row, coreAction)
                {
                    CollectionChangeEventArgsFactory
                        = CollectionChangeEventArgsFactory<IReadOnlyList<T>>.CreateInsert(Target, index, items);

                    var isFromEmpty = target.RowCount == 0;
                    var columnLengthIsZero = items.GetInnerArrayLength() == 0;
                    NotifyProperties =
                        isFromEmpty
                            ? columnLengthIsZero
                                ? new[]
                                {
                                    ListConstant.IndexerName, nameof(Target.IsEmpty), nameof(Target.RowCount),
                                    nameof(Target.AllCount)
                                }
                                : new[]
                                {
                                    ListConstant.IndexerName, nameof(Target.IsEmpty), nameof(Target.RowCount),
                                    nameof(Target.ColumnCount), nameof(Target.AllCount)
                                }
                            : new[] {ListConstant.IndexerName, nameof(Target.RowCount), nameof(Target.AllCount)};
                }
            }

            private class InsertColumn : Insert
            {
                protected override CollectionChangeEventArgsFactory<IReadOnlyList<T>>
                    CollectionChangeEventArgsFactory { get; }

                protected override string[] NotifyProperties { get; }

                public InsertColumn(TwoDimensionalList<T> target, int index, T[][] items, Action coreAction) : base(
                    target, index, items, Direction.Column, coreAction)
                {
                    var isFromEmpty = target.RowCount == 0;

                    (CollectionChangeEventArgsFactory, NotifyProperties) =
                        isFromEmpty
                            ? SetupFromEmpty(items)
                            : SetupFromNotEmpty(index, items);
                }

                private (CollectionChangeEventArgsFactory<IReadOnlyList<T>>, string[])
                    SetupFromEmpty(IEnumerable<T[]> items)
                {
                    var transposedItems = items.ToTransposedArray();
                    var result1
                        = CollectionChangeEventArgsFactory<IReadOnlyList<T>>.CreateInsert(Target, 0, transposedItems);
                    var result2 = new[]
                    {
                        ListConstant.IndexerName, nameof(Target.IsEmpty), nameof(Target.RowCount),
                        nameof(Target.ColumnCount), nameof(Target.AllCount)
                    };

                    return (result1, result2);
                }

                private (CollectionChangeEventArgsFactory<IReadOnlyList<T>>, string[])
                    SetupFromNotEmpty(int index, IEnumerable<T[]> items)
                {
                    var oldItems = Target.ToTwoDimensionalArray();

                    var transposedItems = items.ToTransposedArray();
                    var newItems = oldItems.Zip(transposedItems)
                        .Select(zip =>
                        {
                            var (oldArray, insertArray) = zip;
                            var result = oldArray.ToList();
                            result.InsertRange(index, insertArray);
                            return result.ToArray();
                        }).ToArray();

                    var result1
                        = CollectionChangeEventArgsFactory<IReadOnlyList<T>>.CreateSet(Target, 0, oldItems, newItems);
                    var result2 = new[] {ListConstant.IndexerName, nameof(Target.ColumnCount), nameof(Target.AllCount)};

                    return (result1, result2);
                }
            }
        }
    }
}
