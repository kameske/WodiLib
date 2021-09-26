// ========================================
// Project Name : WodiLib
// File Name    : TwoDimensionalList.Operation.Overwrite.cs
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
            private class OverwriteRow : OperationBase
            {
                protected override CollectionChangeEventArgsFactory<IReadOnlyList<T>>
                    CollectionChangeEventArgsFactory { get; }

                protected override TwoDimensionalCollectionChangeEventArgsFactory<T>
                    TwoDimensionalCollectionChangeEventArgsFactory { get; }

                protected override string[] NotifyProperties { get; }

                public OverwriteRow(TwoDimensionalList<T> target,
                    int index, IEnumerable<T[]> items, Action coreAction) : base(target, coreAction)
                {
                    var overwriteParams = OverwriteParam<IEnumerable<T>>.Factory.Create(
                        target.ToTwoDimensionalArray(), index,
                        items.Select(line => line.AsEnumerable()).ToArray());

                    var replaceOldItems = overwriteParams.ReplaceOldItems.ToTwoDimensionalArray();
                    var replaceNewItems = overwriteParams.ReplaceNewItems.ToTwoDimensionalArray();
                    var insertItems = overwriteParams.InsertItems.ToTwoDimensionalArray();

                    CollectionChangeEventArgsFactory =
                        CollectionChangeEventArgsFactory<IReadOnlyList<T>>.CreateOverwrite(target, index,
                            replaceOldItems, replaceNewItems, insertItems);

                    TwoDimensionalCollectionChangeEventArgsFactory =
                        TwoDimensionalCollectionChangeEventArgsFactory<T>.CreateOverwrite(
                            target, target.ToTwoDimensionalArray(), index,
                            replaceOldItems, replaceNewItems, insertItems, Direction.Row);

                    var notifyPropertyList = new List<string>
                    {
                        ListConstant.IndexerName
                    };
                    if (insertItems.Length > 0)
                    {
                        notifyPropertyList.AddRange(new[] { nameof(target.RowCount), nameof(Target.AllCount) });
                    }

                    NotifyProperties = notifyPropertyList.ToArray();
                }
            }

            private class OverwriteColumn : OperationBase
            {
                protected override CollectionChangeEventArgsFactory<IReadOnlyList<T>>
                    CollectionChangeEventArgsFactory { get; }

                protected override TwoDimensionalCollectionChangeEventArgsFactory<T>
                    TwoDimensionalCollectionChangeEventArgsFactory { get; }

                protected override string[] NotifyProperties { get; }

                public OverwriteColumn(TwoDimensionalList<T> target,
                    int index, IReadOnlyCollection<T[]> items, Action coreAction) : base(target, coreAction)
                {
                    var notifyProps = new List<string>
                    {
                        ListConstant.IndexerName
                    };

                    {
                        var oldItems = target.ToTwoDimensionalArray();
                        var transposedItems = items.ToTransposedArray();
                        var replaceNewItems = target.ToTwoDimensionalArray().Select((line, idx) =>
                        {
                            var list = new SimpleList<T>(line);
                            var overwriteParam = OverwriteParam<T>.Factory.Create(list, index, transposedItems[idx]);
                            list.Overwrite(index, overwriteParam);
                            return list.ToArray();
                        }).ToTwoDimensionalArray();
                        CollectionChangeEventArgsFactory =
                            CollectionChangeEventArgsFactory<IReadOnlyList<T>>.CreateOverwrite(
                                target, 0, oldItems, replaceNewItems, Array.Empty<T[]>());
                    }

                    {
                        var targetItemCount = target.ColumnCount;
                        var itemLength = items.Count;
                        var replaceLength = Math.Min(targetItemCount - index, itemLength);
                        var replaceOldItems = target.ToTwoDimensionalArray().ToTransposedArray()
                            .Range(index, replaceLength)
                            .ToArray();
                        var replaceNewItems = items.Take(replaceLength)
                            .ToArray();
                        var addItems = items.Skip(replaceLength)
                            .ToArray();
                        TwoDimensionalCollectionChangeEventArgsFactory =
                            TwoDimensionalCollectionChangeEventArgsFactory<T>.CreateOverwrite(
                                target, target.ToTwoDimensionalArray(), index,
                                replaceOldItems, replaceNewItems, addItems, Direction.Column);

                        if (addItems.Length > 0)
                        {
                            notifyProps.AddRange(new[] { nameof(target.ColumnCount), nameof(Target.AllCount) });
                        }
                    }

                    NotifyProperties = notifyProps.ToArray();
                }
            }
        }
    }
}
