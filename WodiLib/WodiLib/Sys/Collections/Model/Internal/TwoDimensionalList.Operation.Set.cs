// ========================================
// Project Name : WodiLib
// File Name    : TwoDimensionalList.Operation.Set.cs
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
            private class Set : OperationBase
            {
                protected override CollectionChangeEventArgsFactory<IReadOnlyList<T>>
                    CollectionChangeEventArgsFactory { get; }

                protected override TwoDimensionalCollectionChangeEventArgsFactory<T>
                    TwoDimensionalCollectionChangeEventArgsFactory { get; }

                protected override string[] NotifyProperties { get; }

                public Set(TwoDimensionalList<T> target,
                    int row, int column, T[][] items, Direction direction,
                    Action coreAction) : base(target, coreAction)
                {
                    var fixedNewItems = items.ToTransposedArrayIf(direction == Direction.Column);
                    var rowLen = fixedNewItems.Length;
                    var colLen = fixedNewItems.GetInnerArrayLength();

                    var fixedOldItems = target.Items.Get(row, rowLen).Select(line => line.GetRange(column, colLen))
                        .ToTwoDimensionalArray();
                    var oldItems = fixedOldItems.ToTransposedArrayIf(direction == Direction.Column);

                    CollectionChangeEventArgsFactory =
                        CollectionChangeEventArgsFactory<IReadOnlyList<T>>.CreateSet(
                            target, row, fixedOldItems, fixedNewItems);
                    TwoDimensionalCollectionChangeEventArgsFactory =
                        TwoDimensionalCollectionChangeEventArgsFactory<T>.CreateSet(
                            target, row, column, oldItems, items, direction);
                    NotifyProperties = new[] {ListConstant.IndexerName};
                }
            }
        }
    }
}
