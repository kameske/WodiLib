// ========================================
// Project Name : WodiLib
// File Name    : TwoDimensionalList.Operation.Move.cs
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
            private abstract class Move : OperationBase
            {
                protected override TwoDimensionalCollectionChangeEventArgsFactory<T>
                    TwoDimensionalCollectionChangeEventArgsFactory { get; }

                protected override string[] NotifyProperties { get; }

                protected Move(TwoDimensionalList<T> target,
                    int oldIndex, int newIndex, int count, Direction direction, Action coreAction) : base(target,
                    coreAction)
                {
                    var moveItems = MoveItems(target, oldIndex, count);

                    TwoDimensionalCollectionChangeEventArgsFactory =
                        TwoDimensionalCollectionChangeEventArgsFactory<T>.CreateMove(target, oldIndex, newIndex,
                            moveItems, direction);

                    NotifyProperties = new[] {ListConstant.IndexerName};
                }

                protected abstract T[][] MoveItems(TwoDimensionalList<T> target, int oldIndex, int count);
            }

            private class MoveRow : Move
            {
                protected override CollectionChangeEventArgsFactory<IReadOnlyList<T>>
                    CollectionChangeEventArgsFactory { get; }

                public MoveRow(TwoDimensionalList<T> target, int oldIndex, int newIndex, int count, Action coreAction)
                    : base(target, oldIndex, newIndex, count, Direction.Row, coreAction)
                {
                    var moveItems = MoveItems(target, oldIndex, count);

                    CollectionChangeEventArgsFactory =
                        CollectionChangeEventArgsFactory<IReadOnlyList<T>>.CreateMove(target, oldIndex, newIndex,
                            moveItems);
                }

                protected override T[][] MoveItems(TwoDimensionalList<T> target, int oldIndex, int count)
                    => target.Get_Impl(oldIndex, count).ToTwoDimensionalArray();
            }

            private class MoveColumn : Move
            {
                protected override CollectionChangeEventArgsFactory<IReadOnlyList<T>>
                    CollectionChangeEventArgsFactory { get; }

                public MoveColumn(TwoDimensionalList<T> target, int oldIndex, int newIndex, int count,
                    Action coreAction)
                    : base(target, oldIndex, newIndex, count, Direction.Column, coreAction)
                {
                    var oldItems = Target.Items.DeepClone().ToTwoDimensionalList();
                    var newItems = oldItems.Select(line =>
                    {
                        var result = new ExtendedList<T>(line);
                        result.MoveRange(oldIndex, newIndex, count);
                        return result;
                    }).ToList();

                    CollectionChangeEventArgsFactory =
                        CollectionChangeEventArgsFactory<IReadOnlyList<T>>.CreateSet(
                            Target, 0, oldItems, newItems);
                }

                protected override T[][] MoveItems(TwoDimensionalList<T> target, int oldIndex, int count)
                    => target.Get_Impl(0, target.RowCount, oldIndex, count, Direction.Column);
            }
        }
    }
}
