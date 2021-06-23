// ========================================
// Project Name : WodiLib
// File Name    : TwoDimensionalList.Operation.Reset.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;

namespace WodiLib.Sys.Collections
{
    internal partial class TwoDimensionalList<T>
    {
        private static partial class Operation
        {
            private class Reset : OperationBase
            {
                protected override CollectionChangeEventArgsFactory<IReadOnlyList<T>>
                    CollectionChangeEventArgsFactory { get; }

                protected override TwoDimensionalCollectionChangeEventArgsFactory<T>
                    TwoDimensionalCollectionChangeEventArgsFactory { get; }

                protected override string[] NotifyProperties { get; }

                public Reset(TwoDimensionalList<T> target, T[][] newItems,
                    Direction direction, Action coreAction)
                    : base(target, coreAction)
                {
                    CollectionChangeEventArgsFactory
                        = CollectionChangeEventArgsFactory<IReadOnlyList<T>>.CreateReset(
                            target, target.ToTwoDimensionalList(), newItems);

                    TwoDimensionalCollectionChangeEventArgsFactory
                        = TwoDimensionalCollectionChangeEventArgsFactory<T>.CreateReset(
                            target, target.ToTwoDimensionalArray(), newItems,
                            direction);

                    var isRowLengthChange = target.Count != newItems.Length;
                    var isColumnLengthChange = target.ItemCount != newItems.GetInnerArrayLength();
                    var isAllCountChange = target.AllCount != newItems.Length * newItems.GetInnerArrayLength();
                    var isEmptyFrom = target.Count == 0;
                    var isEmptyTo = newItems.Length == 0;

                    var notifyPropertyList = new List<string>
                    {
                        ListConstant.IndexerName
                    };

                    if (isRowLengthChange)
                    {
                        notifyPropertyList.Add(nameof(target.Count));
                    }

                    if (isColumnLengthChange)
                    {
                        notifyPropertyList.Add(nameof(target.ItemCount));
                    }

                    if (isAllCountChange)
                    {
                        notifyPropertyList.Add(nameof(target.AllCount));
                    }

                    if (isEmptyFrom ^ isEmptyTo)
                    {
                        notifyPropertyList.Add(nameof(target.IsEmpty));
                    }

                    NotifyProperties = notifyPropertyList.ToArray();
                }
            }
        }
    }
}
