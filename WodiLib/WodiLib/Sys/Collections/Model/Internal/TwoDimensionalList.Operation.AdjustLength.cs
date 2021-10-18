// ========================================
// Project Name : WodiLib
// File Name    : TwoDimensionalList.Operation.AdjustLength.cs
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
            private class AdjustLength : OperationBase
            {
                protected override CollectionChangeEventArgsFactory<IReadOnlyList<T>>
                    CollectionChangeEventArgsFactory { get; }

                protected override TwoDimensionalCollectionChangeEventArgsFactory<T>
                    TwoDimensionalCollectionChangeEventArgsFactory { get; }

                protected override string[] NotifyProperties { get; }

                public AdjustLength(TwoDimensionalList<T> target, int rowLength, int columnLength,
                    T[][] addRowItems, T[][] addColumnItems, Action coreAction) :
                    base(target, coreAction)
                {
                    {
                        var replaceLength = Math.Min(target.RowCount, rowLength);
                        var replaceItems = MakeReplaceItems(target, replaceLength, columnLength, addColumnItems);
                        CollectionChangeEventArgsFactory
                            = target.RowCount < rowLength
                                ? CreateCollectionChangeEventArgsFactoryIfShort(target,
                                    replaceItems, addRowItems)
                                : CreateCollectionChangeEventArgsFactoryIfLong(target,
                                    rowLength, replaceItems);
                    }

                    TwoDimensionalCollectionChangeEventArgsFactory
                        = CreateTwoDimensionalCollectionChangeEventArgsFactory(
                            target, addRowItems, addColumnItems, rowLength, columnLength, Direction.Row);

                    NotifyProperties = CreateNotifyPropertyList(target, rowLength, columnLength);
                }

                private static (IReadOnlyList<T[]>, IReadOnlyList<T[]>)? MakeReplaceItems(
                    TwoDimensionalList<T> target, int rowLength, int columnLength,
                    IReadOnlyCollection<T[]> addColumnItems)
                {
                    if (addColumnItems.Count > 0)
                    {
                        // add
                        var oldItems = target.GetRow_Impl(0, rowLength);
                        var newItems = oldItems.Zip(addColumnItems)
                            .Select(zip => zip.Item1.Concat(zip.Item2))
                            .ToTwoDimensionalArray();

                        return (oldItems, newItems);
                    }

                    var removeLength = target.ColumnCount - columnLength;
                    if (removeLength > 0)
                    {
                        // remove
                        var oldItems = target.GetRow_Impl(0, rowLength);
                        var newItems = oldItems.Select(line => line.Range(0, columnLength))
                            .ToTwoDimensionalArray();
                        return (oldItems, newItems);
                    }

                    // not change
                    return null;
                }

                private static CollectionChangeEventArgsFactory<IReadOnlyList<T>>
                    CreateCollectionChangeEventArgsFactoryIfShort(
                        TwoDimensionalList<T> target, (IReadOnlyList<T[]>, IReadOnlyList<T[]>)? replaceItems,
                        IReadOnlyList<T[]> addRowItems)
                {
                    return CollectionChangeEventArgsFactory<IReadOnlyList<T>>
                        .CreateAdjustLengthIfShort(target, replaceItems, target.RowCount, addRowItems);
                }

                private static CollectionChangeEventArgsFactory<IReadOnlyList<T>>
                    CreateCollectionChangeEventArgsFactoryIfLong(TwoDimensionalList<T> target,
                        int rowLength, (IReadOnlyList<T[]>, IReadOnlyList<T[]>)? replaceItems)
                {
                    var removeLength = target.RowCount - rowLength;
                    var removeItems = removeLength == 0
                        ? Array.Empty<T[]>()
                        : target.GetRow_Impl(rowLength, removeLength);

                    return CollectionChangeEventArgsFactory<IReadOnlyList<T>>
                        .CreateAdjustLengthIfLong(target, replaceItems, rowLength, removeItems);
                }

                private static TwoDimensionalCollectionChangeEventArgsFactory<T>
                    CreateTwoDimensionalCollectionChangeEventArgsFactory(
                        ITwoDimensionalList<T, T> target, IEnumerable<T[]> addRowItems,
                        IReadOnlyList<T[]> addColumnItems, int rowLength, int columnLength, Direction direction)
                {
                    var oldItems = target.ToTwoDimensionalArray();
                    var newItems = CreateTwoDimensionalCollectionChangeEventArgsFactory_MakeNewItems
                        (oldItems, rowLength, columnLength, addRowItems, addColumnItems);

                    return TwoDimensionalCollectionChangeEventArgsFactory<T>.CreateAdjustLength(target, oldItems,
                        newItems, direction);
                }

                private static T[][] CreateTwoDimensionalCollectionChangeEventArgsFactory_MakeNewItems(
                    T[][] oldItems, int rowLength, int columnLength,
                    IEnumerable<T[]> addRowItems, IReadOnlyList<T[]> addColumnItems)
                {
                    var oldItemsInnerLength = oldItems.GetInnerArrayLength();
                    var isAddedRow = oldItems.Length < rowLength;
                    var isRemovedRow = oldItems.Length > rowLength;
                    var isRemovedColumn = oldItemsInnerLength > columnLength;
                    var isAddedColumn = oldItemsInnerLength < columnLength;

                    var newItemsRowFixed = (
                            isAddedRow
                                ? oldItems.Concat(addRowItems.Select(line => line.Range(0, oldItemsInnerLength)))
                                : isRemovedRow
                                    ? oldItems.Take(rowLength)
                                    : oldItems)
                        .ToArray();

                    var newItems = newItemsRowFixed.Select((line, idx) => (
                            isAddedColumn
                                ? line.Concat(addColumnItems[idx])
                                : isRemovedColumn
                                    ? line.Take(columnLength)
                                    : line
                        ).ToArray()
                    ).ToArray();

                    return newItems;
                }

                private static string[] CreateNotifyPropertyList(ITwoDimensionalList<T, T> target,
                    int rowLength, int columnLength)
                {
                    var isChangeRowLength = target.RowCount != rowLength;
                    var isChangeColLength = target.ColumnCount != columnLength;
                    var isAllCountChange = target.AllCount != rowLength * columnLength;
                    var isEmptyFrom = target.RowCount == 0;
                    var isEmptyTo = rowLength == 0;

                    List<string> notifyProps = new();

                    if (isChangeRowLength || isChangeColLength)
                    {
                        notifyProps.Add(ListConstant.IndexerName);
                    }

                    if (isChangeRowLength)
                    {
                        notifyProps.Add(nameof(target.RowCount));
                    }

                    if (isChangeColLength)
                    {
                        notifyProps.Add(nameof(target.ColumnCount));
                    }

                    if (isAllCountChange)
                    {
                        notifyProps.Add(nameof(target.AllCount));
                    }

                    if (isEmptyFrom ^ isEmptyTo)
                    {
                        notifyProps.Add(nameof(target.IsEmpty));
                    }

                    return notifyProps.ToArray();
                }
            }
        }
    }
}
