// ========================================
// Project Name : WodiLib
// File Name    : TwoDimensionalList.Operation.cs
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
            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
            //      Public Interface
            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

            public interface IOperation
            {
                public void Execute();
            }

            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
            //      Public Static Methods
            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

            // ______________________ Create ______________________

            public static IOperation CreateSet(TwoDimensionalList<T> target,
                int row, int column, T[][] items, Direction direction, Action coreAction)
                => new Set(target, row, column, items, direction, coreAction);

            // CS0173 回避のため三項演算子は使わない

            public static IOperation CreateInsert(TwoDimensionalList<T> target,
                int index, T[][] items, Direction direction, Action coreAction)
            {
                if (direction == Direction.Row) return new InsertRow(target, index, items, coreAction);
                return new InsertColumn(target, index, items, coreAction);
            }

            public static IOperation CreateOverwrite(TwoDimensionalList<T> target,
                int index, T[][] items, Direction direction, Action coreAction)
            {
                if (direction == Direction.Row) return new OverwriteRow(target, index, items, coreAction);
                return new OverwriteColumn(target, index, items, coreAction);
            }

            public static IOperation CreateMove(TwoDimensionalList<T> target,
                int oldIndex, int newIndex, int count, Direction direction, Action coreAction)
            {
                if (direction == Direction.Row) return new MoveRow(target, oldIndex, newIndex, count, coreAction);
                return new MoveColumn(target, oldIndex, newIndex, count, coreAction);
            }

            public static IOperation CreateRemove(TwoDimensionalList<T> target,
                int index, int count, Direction direction, Action coreAction)
            {
                if (direction == Direction.Row) return new RemoveRow(target, index, count, coreAction);
                return new RemoveColumn(target, index, count, coreAction);
            }

            public static IOperation CreateAdjustLength(TwoDimensionalList<T> target,
                int rowLength, int columnLength, T[][] addRowItems, T[][] addColumnItems,
                Action coreAction)
                => new AdjustLength(target, rowLength, columnLength, addRowItems,
                    addColumnItems, coreAction);

            public static IOperation CreateReset(TwoDimensionalList<T> target,
                T[][] newItems, Action coreAction)
                => new Reset(target, newItems, Direction.Row, coreAction);

            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
            //      Classes
            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

            private abstract class OperationBase : IOperation
            {
                protected TwoDimensionalList<T> Target { get; }

                private Action CoreAction { get; }

                protected abstract CollectionChangeEventArgsFactory<IReadOnlyList<T>>
                    CollectionChangeEventArgsFactory { get; }

                protected abstract TwoDimensionalCollectionChangeEventArgsFactory<T>
                    TwoDimensionalCollectionChangeEventArgsFactory { get; }

                protected abstract string[] NotifyProperties { get; }

                protected OperationBase(TwoDimensionalList<T> target, Action coreAction)
                {
                    Target = target;
                    CoreAction = coreAction;
                }

                public void Execute()
                {
                    var notifyManager = Target.MakeNotifyManager(CollectionChangeEventArgsFactory,
                        TwoDimensionalCollectionChangeEventArgsFactory, NotifyProperties);

                    notifyManager.NotifyBeforeEvent();

                    CoreAction();

                    notifyManager.NotifyAfterEvent();
                }
            }
        }
    }
}
