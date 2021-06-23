// ========================================
// Project Name : WodiLib
// File Name    : TwoDimensionalList.Operation.Remove.cs
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
            private abstract class Remove : OperationBase
            {
                protected override TwoDimensionalCollectionChangeEventArgsFactory<T>
                    TwoDimensionalCollectionChangeEventArgsFactory { get; }

                protected Remove(TwoDimensionalList<T> target, int index, int count, Direction direction,
                    Action coreAction) : base(target, coreAction)
                {
                    var removeItems = RemoveItems(target, index, count);
                    var oldItems = target.ToTwoDimensionalArray()
                        .ToTransposedArrayIf(direction == Direction.Column);

                    TwoDimensionalCollectionChangeEventArgsFactory =
                        TwoDimensionalCollectionChangeEventArgsFactory<T>.CreateRemove(
                            target, index, oldItems, removeItems, direction);
                }

                protected abstract T[][] RemoveItems(TwoDimensionalList<T> target, int index, int count);
            }

            private class RemoveRow : Remove
            {
                protected override CollectionChangeEventArgsFactory<IReadOnlyList<T>>
                    CollectionChangeEventArgsFactory { get; }

                protected override string[] NotifyProperties { get; }

                public RemoveRow(TwoDimensionalList<T> target, int index, int count, Action coreAction) : base(target,
                    index, count, Direction.Row, coreAction)
                {
                    var removeItems = target.Get_Impl(index, count)
                        .ToTwoDimensionalArray();

                    CollectionChangeEventArgsFactory =
                        CollectionChangeEventArgsFactory<IReadOnlyList<T>>.CreateRemove(target, index, removeItems);

                    var isToEmpty = count == target.Count;
                    NotifyProperties =
                        isToEmpty
                            ? new[]
                            {
                                ListConstant.IndexerName, nameof(target.Count), nameof(target.ItemCount),
                                nameof(target.AllCount), nameof(target.IsEmpty)
                            }
                            : new[] {ListConstant.IndexerName, nameof(target.Count), nameof(target.AllCount)};
                }

                protected override T[][] RemoveItems(TwoDimensionalList<T> target, int index, int count)
                    => target.Get_Impl(index, count).ToTwoDimensionalArray();
            }

            private class RemoveColumn : Remove
            {
                protected override CollectionChangeEventArgsFactory<IReadOnlyList<T>>
                    CollectionChangeEventArgsFactory { get; }

                protected override string[] NotifyProperties { get; }

                public RemoveColumn(TwoDimensionalList<T> target, int index, int count, Action coreAction) : base(
                    target, index, count, Direction.Column, coreAction)
                {
                    var oldItems = Target.Items.DeepClone().ToTwoDimensionalList();
                    var newItems = oldItems.Select(line =>
                    {
                        var result = new ExtendedList<T>(line);
                        result.RemoveRange(index, count);
                        return result;
                    }).ToList();

                    CollectionChangeEventArgsFactory =
                        CollectionChangeEventArgsFactory<IReadOnlyList<T>>.CreateSet(
                            Target, 0, oldItems, newItems);

                    NotifyProperties = new[]
                        {ListConstant.IndexerName, nameof(Target.ItemCount), nameof(Target.AllCount)};
                }

                protected override T[][] RemoveItems(TwoDimensionalList<T> target, int index, int count)
                    => target.Get_Impl(0, target.Count, index, count, Direction.Column);
            }
        }
    }
}
