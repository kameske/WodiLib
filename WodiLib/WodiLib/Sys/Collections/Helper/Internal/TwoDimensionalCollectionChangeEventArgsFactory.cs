// ========================================
// Project Name : WodiLib
// File Name    : TwoDimensionalCollectionChangeEventArgsFactory.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Linq;

namespace WodiLib.Sys.Collections
{
    internal partial class TwoDimensionalCollectionChangeEventArgsFactory<T>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Static Methods
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        public static TwoDimensionalCollectionChangeEventArgsFactory<T> CreateSet(
            INotifiableTwoDimensionalListChangeInternal<T> target, int row, int column,
            T[][] oldItems, T[][] newItems, Direction direction)
            => new(target, row, column, oldItems, newItems, direction);

        public static TwoDimensionalCollectionChangeEventArgsFactory<T> CreateInsert(
            INotifiableTwoDimensionalListChangeInternal<T> target, int index, T[][] oldItems,
            T[][] insertItems, Direction direction)
            => new(target, TwoDimensionalCollectionChangeAction.Add, index, oldItems, insertItems, direction);

        public static TwoDimensionalCollectionChangeEventArgsFactory<T> CreateOverwrite(
            INotifiableTwoDimensionalListChangeInternal<T> target, T[][] targetItems, int index, T[][] replaceOldItems,
            T[][] replaceNewItems, T[][] addNewItems, Direction direction)
            => new(target, targetItems, index, replaceOldItems, replaceNewItems, addNewItems, direction);

        public static TwoDimensionalCollectionChangeEventArgsFactory<T> CreateMove(
            INotifiableTwoDimensionalListChangeInternal<T> target, int oldIndex, int newIndex,
            T[][] items, Direction direction)
            => new(target, oldIndex, newIndex, items, direction);

        public static TwoDimensionalCollectionChangeEventArgsFactory<T> CreateRemove(
            INotifiableTwoDimensionalListChangeInternal<T> target, int index, T[][] oldItems,
            T[][] removeItems, Direction direction)
            => new(target, TwoDimensionalCollectionChangeAction.Remove, index, oldItems, removeItems, direction);

        public static TwoDimensionalCollectionChangeEventArgsFactory<T> CreateAdjustLength(
            INotifiableTwoDimensionalListChangeInternal<T> target, T[][] oldItems, T[][] newItems, Direction direction)
            => new(target, oldItems, newItems, true, direction);

        public static TwoDimensionalCollectionChangeEventArgsFactory<T> CreateReset(
            INotifiableTwoDimensionalListChangeInternal<T> target, T[][] oldItems,
            T[][] newItems, Direction direction)
            => new(target, oldItems, newItems, false, direction);

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Static Methods
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        // ______________________ Need ______________________

        private static bool NeedTranspose(Direction direction1, Direction direction2)
            => direction1 == Direction.Row && direction2 == Direction.Column
               || direction1 == Direction.Column && direction2 == Direction.Row;

        // ______________________ Determine ______________________

        private static ArgsCreateType DetermineArgsCreateType_SingleAction(
            NotifyTwoDimensionalListChangeEventType type, Direction direction,
            IReadOnlyCollection<T[]> items)
        {
            if (!type.IsNotify) return ArgsCreateType.Empty;

            if (items.Count == 0) return ArgsCreateType.Empty;

            if (type.GroupingType == NotifyTwoDimensionalListChangeEventGroupingType.All) return ArgsCreateType.All;
            if (type.GroupingType == NotifyTwoDimensionalListChangeEventGroupingType.Row) return ArgsCreateType.Row;
            if (type.GroupingType == NotifyTwoDimensionalListChangeEventGroupingType.Column)
                return ArgsCreateType.Column;
            if (type.GroupingType == NotifyTwoDimensionalListChangeEventGroupingType.None) return ArgsCreateType.None;

            // type.GroupingType == NotifyTwoDimensionalListChangeEventGroupingType.Direct
            if (direction == Direction.None) return ArgsCreateType.None;
            return direction == Direction.Row
                ? ArgsCreateType.Row
                : ArgsCreateType.Column;
        }

        private static (ArgsCreateType, MultiActionType) DetermineArgsCreateType_MultiAction(
            NotifyTwoDimensionalListChangeEventType type, Direction direction,
            T[][] replaceOldItems, T[][] addNewItems)
        {
            if (!type.IsNotify)
            {
                return (ArgsCreateType.Empty, MultiActionType.None);
            }

            if (replaceOldItems.Length == 0 && addNewItems.Length == 0)
            {
                return (ArgsCreateType.Empty, MultiActionType.None);
            }

            var notifyDirection = type.GroupingType.Id switch
            {
                nameof(NotifyTwoDimensionalListChangeEventGroupingType.Row) => Direction.Row,
                nameof(NotifyTwoDimensionalListChangeEventGroupingType.Column) => Direction.Column,
                nameof(NotifyTwoDimensionalListChangeEventGroupingType.All) => direction,
                nameof(NotifyTwoDimensionalListChangeEventGroupingType.Direct) => direction,
                nameof(NotifyTwoDimensionalListChangeEventGroupingType.None) => Direction.None,
                _ => throw new ArgumentOutOfRangeException()
            };

            var needTranspose = NeedTranspose(direction, notifyDirection);

            var isSingleAction = replaceOldItems.Length == 0 ^ addNewItems.Length == 0;
            if (isSingleAction)
            {
                var isNotifyReplace = replaceOldItems.Length != 0;

                // 実行方向と通知方向が異なる場合 Replace / Add を入れ替える必要がある
                var argsCreateType = DetermineArgsCreateType_SingleAction(type, direction,
                    isNotifyReplace ? replaceOldItems : addNewItems);
                var actionType = (isNotifyReplace, needTranspose) switch
                {
                    (true, false) => MultiActionType.Replace,
                    (true, true) => MultiActionType.Add,
                    (false, false) => MultiActionType.Add,
                    (false, true) => MultiActionType.Replace,
                };

                return (argsCreateType, actionType);
            }

            // Raise multiAction

            if (needTranspose)
            {
                // 複数アクションかつ通知方向が異なる場合は ActionType = Replace 固定
                return type.GroupingType.Id switch
                {
                    nameof(NotifyTwoDimensionalListChangeEventGroupingType.All) => (ArgsCreateType.All,
                        MultiActionType.Replace),
                    nameof(NotifyTwoDimensionalListChangeEventGroupingType.Row) => (ArgsCreateType.Row,
                        MultiActionType.Replace),
                    nameof(NotifyTwoDimensionalListChangeEventGroupingType.Column) => (ArgsCreateType.Column,
                        MultiActionType.Replace),
                    nameof(NotifyTwoDimensionalListChangeEventGroupingType.None) => (ArgsCreateType.None,
                        MultiActionType.Replace),
                    // type.GroupingType == NotifyTwoDimensionalListChangeEventGroupingType.Direct
                    _ => (direction == Direction.Row ? ArgsCreateType.Row : ArgsCreateType.Column,
                        MultiActionType.Replace)
                };
            }
            else
            {
                if (!type.IsMultiAction)
                {
                    return (ArgsCreateType.All, MultiActionType.Reset);
                }

                return type.GroupingType.Id switch
                {
                    nameof(NotifyTwoDimensionalListChangeEventGroupingType.All) => (ArgsCreateType.All,
                        MultiActionType.Both),
                    nameof(NotifyTwoDimensionalListChangeEventGroupingType.Row) => (ArgsCreateType.Row,
                        MultiActionType.Both),
                    nameof(NotifyTwoDimensionalListChangeEventGroupingType.Column) => (ArgsCreateType.Column,
                        MultiActionType.Both),
                    nameof(NotifyTwoDimensionalListChangeEventGroupingType.None) => (ArgsCreateType.None,
                        MultiActionType.Both),
                    // type.GroupingType == NotifyTwoDimensionalListChangeEventGroupingType.Direct
                    _ => (direction == Direction.Row ? ArgsCreateType.Row : ArgsCreateType.Column, MultiActionType.Both)
                };
            }
        }

        // ______________________ Create ______________________

        private static IEnumerable<TwoDimensionalCollectionChangeEventInternalArgs<T>> CreateSet(
            NotifyTwoDimensionalListChangeEventType type, int row, int column,
            T[][] oldItems, T[][] newItems, Direction direction)
        {
            var createType = DetermineArgsCreateType_SingleAction(type, direction, oldItems);

            return createType switch
            {
                ArgsCreateType.Empty => Array.Empty<TwoDimensionalCollectionChangeEventInternalArgs<T>>(),
                ArgsCreateType.All => SetArgsFactory.CreateSingle(direction, row, column, oldItems, newItems),
                ArgsCreateType.Row => SetArgsFactory.CreateLine(direction, Direction.Row, row, column, oldItems,
                    newItems),
                ArgsCreateType.Column => SetArgsFactory.CreateLine(direction, Direction.Column, row, column, oldItems,
                    newItems),
                ArgsCreateType.None => SetArgsFactory.CreateMulti(direction, row, column, oldItems, newItems),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private static IEnumerable<TwoDimensionalCollectionChangeEventInternalArgs<T>> CreateAdd(
            NotifyTwoDimensionalListChangeEventType type, int index,
            T[][] oldItems, T[][] addItems, Direction direction)
        {
            var createType = DetermineArgsCreateType_SingleAction(type, direction, addItems);

            var needTranspose = createType == ArgsCreateType.Row && direction == Direction.Column
                                || createType == ArgsCreateType.Column && direction != Direction.Column;

            return (createType, needTranspose) switch
            {
                (ArgsCreateType.Empty, _) => Array.Empty<TwoDimensionalCollectionChangeEventInternalArgs<T>>(),
                (ArgsCreateType.All, _) => AddArgsFactory.CreateSingle(direction, index, addItems),
                (ArgsCreateType.Row, false) => AddArgsFactory.CreateLine(direction, Direction.Row, index, addItems),
                (ArgsCreateType.Row, true) => SetArgsFactory.CreateLine(direction, Direction.Row, index, 0,
                    oldItems,
                    AcquireInsertedArray(oldItems, index, addItems)),
                (ArgsCreateType.Column, false) => AddArgsFactory.CreateLine(direction, Direction.Column, index,
                    addItems),
                (ArgsCreateType.Column, true) => SetArgsFactory.CreateLine(direction, Direction.Column, 0, index,
                    oldItems,
                    AcquireInsertedArray(oldItems, index, addItems)),
                (ArgsCreateType.None, _) => AddArgsFactory.CreateMulti(direction, index, addItems),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private static IEnumerable<TwoDimensionalCollectionChangeEventInternalArgs<T>> CreateOverwrite(
            NotifyTwoDimensionalListChangeEventType type, T[][] target, int index, T[][] replaceOldItems,
            T[][] replaceNewItems, T[][] addNewItems, Direction direction)
        {
            var (createType, actionType) =
                DetermineArgsCreateType_MultiAction(type, direction, replaceOldItems, addNewItems);

            return (createType, actionType) switch
            {
                /*
                 * MultiActionType == None の場合、 ArgsCreateType が必ず Empty となる。
                 * よってこのパターンは 分岐条件に含めなくて良い
                 */
                (ArgsCreateType.Empty, _) => Array.Empty<TwoDimensionalCollectionChangeEventInternalArgs<T>>(),
                (_, MultiActionType.Reset)
                    => OverwriteArgsFactory.CreateReset(direction, index, replaceOldItems, replaceNewItems,
                        addNewItems),
                (ArgsCreateType.Row, _)
                    => OverwriteArgsFactory.CreateLine(direction, Direction.Row, target,
                        index, replaceOldItems, replaceNewItems, addNewItems),
                (ArgsCreateType.Column, _)
                    => OverwriteArgsFactory.CreateLine(direction, Direction.Column, target,
                        index, replaceOldItems, replaceNewItems, addNewItems),
                (ArgsCreateType.None, _)
                    => OverwriteArgsFactory.CreateMulti(direction, index, replaceOldItems,
                        replaceNewItems, addNewItems),
                (ArgsCreateType.All, MultiActionType.Both) =>
                    OverwriteArgsFactory.CreateSingle(direction,
                        type.GroupingType.Direction ?? direction,
                        target, index, replaceNewItems, addNewItems),
                (ArgsCreateType.All, MultiActionType.Add) =>
                    OverwriteArgsFactory.CreateSingle(direction,
                        type.GroupingType.Direction ?? direction,
                        target, index, replaceNewItems, addNewItems),
                (ArgsCreateType.All, MultiActionType.Replace) =>
                    OverwriteArgsFactory.CreateSingle(direction,
                        type.GroupingType.Direction ?? direction,
                        target, index, replaceNewItems, addNewItems),
                (ArgsCreateType.All, _)
                    => OverwriteArgsFactory.CreateReset(direction, index, replaceOldItems, replaceNewItems,
                        addNewItems),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private static IEnumerable<TwoDimensionalCollectionChangeEventInternalArgs<T>> CreateMove(
            NotifyTwoDimensionalListChangeEventType type, int oldIndex, int newIndex,
            T[][] items, Direction direction)
        {
            var createType = DetermineArgsCreateType_SingleAction(type, direction, items);

            return createType switch
            {
                ArgsCreateType.Empty => Array.Empty<TwoDimensionalCollectionChangeEventInternalArgs<T>>(),
                ArgsCreateType.All => MoveArgsFactory.CreateSingle(direction, oldIndex, newIndex, items),
                ArgsCreateType.Row => MoveArgsFactory.CreateLine(direction, Direction.Row, oldIndex, newIndex, items),
                ArgsCreateType.Column => MoveArgsFactory.CreateLine(direction, Direction.Column, oldIndex, newIndex,
                    items),
                ArgsCreateType.None => MoveArgsFactory.CreateMulti(direction, oldIndex, newIndex, items),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private static IEnumerable<TwoDimensionalCollectionChangeEventInternalArgs<T>> CreateRemove(
            NotifyTwoDimensionalListChangeEventType type, int index,
            T[][] oldItems, T[][] items, Direction direction)
        {
            var createType = DetermineArgsCreateType_SingleAction(type, direction, items);

            var needTranspose = createType == ArgsCreateType.Row && direction == Direction.Column
                                || createType == ArgsCreateType.Column && direction != Direction.Column;

            return (createType, needTranspose) switch
            {
                (ArgsCreateType.Empty, _) => Array.Empty<TwoDimensionalCollectionChangeEventInternalArgs<T>>(),
                (ArgsCreateType.All, _) => RemoveArgsFactory.CreateSingle(direction, index, items),
                (ArgsCreateType.Row, false) => RemoveArgsFactory.CreateLine(direction, Direction.Row, index, items),
                (ArgsCreateType.Row, true) => SetArgsFactory.CreateLine(direction,
                    Direction.Row, index, 0, oldItems, AcquireRemovedArray(oldItems, index, items.Length)),
                (ArgsCreateType.Column, false) => RemoveArgsFactory.CreateLine(direction, Direction.Column, index,
                    items),
                (ArgsCreateType.Column, true) => SetArgsFactory.CreateLine(direction,
                    Direction.Column, 0, index, oldItems, AcquireRemovedArray(oldItems, index, items.Length)),
                (ArgsCreateType.None, _) => RemoveArgsFactory.CreateMulti(direction, index, items),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private static IEnumerable<TwoDimensionalCollectionChangeEventInternalArgs<T>>
            CreateAdjustLength(NotifyTwoDimensionalListChangeEventType type,
                T[][] oldItems, T[][] newItems, Direction direction)
        {
            var createType = DetermineArgsCreateType_SingleAction(type, direction, oldItems);

            return createType switch
            {
                ArgsCreateType.Empty => Array.Empty<TwoDimensionalCollectionChangeEventInternalArgs<T>>(),
                ArgsCreateType.All =>
                    AdjustLengthArgsFactory.CreateSingle(direction, Direction.Row, oldItems, newItems),
                ArgsCreateType.Row => AdjustLengthArgsFactory.CreateLine(direction, Direction.Row, oldItems, newItems),
                ArgsCreateType.Column => AdjustLengthArgsFactory.CreateLine(direction, Direction.Column, oldItems,
                    newItems),
                ArgsCreateType.None => AdjustLengthArgsFactory.CreateMulti(direction, oldItems, newItems),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private static IEnumerable<TwoDimensionalCollectionChangeEventInternalArgs<T>> CreateReset(
            NotifyTwoDimensionalListChangeEventType type, int index,
            T[][] oldItems, T[][] newItems, Direction direction)
        {
            var createType = DetermineArgsCreateType_SingleAction(type, direction, oldItems);

            var needTranspose = createType == ArgsCreateType.Row && direction == Direction.Column
                                || createType == ArgsCreateType.Column && direction != Direction.Column;

            var row = direction == Direction.Row ? index : 0;
            var column = direction == Direction.Column ? index : 0;
            var fixedOldItems = oldItems.ToTransposedArrayIf(needTranspose);
            var fixedNewItems = newItems.ToTransposedArrayIf(needTranspose);

            var notifyDirection = TransformRightAngleDirectionIf(needTranspose, direction);

            return createType switch
            {
                ArgsCreateType.Empty => Array.Empty<TwoDimensionalCollectionChangeEventInternalArgs<T>>(),
                ArgsCreateType.All => new[]
                {
                    TwoDimensionalCollectionChangeEventInternalArgs<T>.CreateResetArgs(
                        fixedOldItems, fixedNewItems, row, column, notifyDirection),
                },
                ArgsCreateType.Row => new[]
                {
                    TwoDimensionalCollectionChangeEventInternalArgs<T>.CreateResetArgs(
                        fixedOldItems, fixedNewItems, row, column, notifyDirection),
                },
                ArgsCreateType.Column => new[]
                {
                    TwoDimensionalCollectionChangeEventInternalArgs<T>.CreateResetArgs(
                        fixedOldItems, fixedNewItems, row, column, notifyDirection),
                },
                ArgsCreateType.None => new[]
                {
                    TwoDimensionalCollectionChangeEventInternalArgs<T>.CreateResetArgs(
                        fixedOldItems, fixedNewItems, row, column, notifyDirection),
                },
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        public IEnumerable<TwoDimensionalCollectionChangeEventInternalArgs<T>> CollectionChangingEventArgs { get; }
        public IEnumerable<TwoDimensionalCollectionChangeEventInternalArgs<T>> CollectionChangedEventArgs { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructors
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     Set 系用コンストラクタ
        /// </summary>
        private TwoDimensionalCollectionChangeEventArgsFactory(
            INotifiableTwoDimensionalListChangeInternal<T> target, int row, int column,
            T[][] oldItems, T[][] newItems, Direction direction)
        {
            CollectionChangingEventArgs = CreateSet(target.NotifyTwoDimensionalListChangingEventType,
                row, column, oldItems, newItems, direction);

            CollectionChangedEventArgs =
                target.NotifyTwoDimensionalListChangingEventType == target.NotifyTwoDimensionalListChangedEventType
                    ? CollectionChangingEventArgs
                    : CreateSet(target.NotifyTwoDimensionalListChangedEventType,
                        row, column, oldItems, newItems, direction);
        }

        /// <summary>
        ///     Insert, Remove 系用コンストラクタ
        /// </summary>
        private TwoDimensionalCollectionChangeEventArgsFactory(INotifiableTwoDimensionalListChangeInternal<T> target,
            TwoDimensionalCollectionChangeAction action,
            int index, T[][] oldItems, T[][] items, Direction direction)
        {
            switch (action)
            {
                case TwoDimensionalCollectionChangeAction.Add:
                    CollectionChangingEventArgs = CreateAdd(target.NotifyTwoDimensionalListChangingEventType,
                        index, oldItems, items, direction);
                    CollectionChangedEventArgs = target.NotifyTwoDimensionalListChangingEventType ==
                                                 target.NotifyTwoDimensionalListChangedEventType
                        ? CollectionChangingEventArgs
                        : CreateAdd(target.NotifyTwoDimensionalListChangedEventType,
                            index, oldItems, items, direction);
                    break;

                case TwoDimensionalCollectionChangeAction.Remove:
                    CollectionChangingEventArgs = CreateRemove(target.NotifyTwoDimensionalListChangingEventType,
                        index, oldItems, items, direction);
                    CollectionChangedEventArgs = target.NotifyTwoDimensionalListChangingEventType ==
                                                 target.NotifyTwoDimensionalListChangedEventType
                        ? CollectionChangingEventArgs
                        : CreateRemove(target.NotifyTwoDimensionalListChangedEventType,
                            index, oldItems, items, direction);
                    break;

                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        ///     Overwrite 系用コンストラクタ
        /// </summary>
        private TwoDimensionalCollectionChangeEventArgsFactory(INotifiableTwoDimensionalListChangeInternal<T> target,
            T[][] targetItems, int index, T[][] replaceOldItems, T[][] replaceNewItems,
            T[][] addNewItems, Direction direction)
        {
            CollectionChangingEventArgs = CreateOverwrite(target.NotifyTwoDimensionalListChangingEventType,
                targetItems, index,
                replaceOldItems, replaceNewItems, addNewItems, direction);
            CollectionChangedEventArgs =
                target.NotifyTwoDimensionalListChangingEventType == target.NotifyTwoDimensionalListChangedEventType
                    ? CollectionChangingEventArgs
                    : CreateOverwrite(target.NotifyTwoDimensionalListChangedEventType,
                        targetItems, index,
                        replaceOldItems, replaceNewItems, addNewItems, direction);
        }

        /// <summary>
        ///     Move 系用コンストラクタ
        /// </summary>
        private TwoDimensionalCollectionChangeEventArgsFactory(INotifiableTwoDimensionalListChangeInternal<T> target,
            int oldIndex, int newIndex, T[][] items, Direction direction)
        {
            CollectionChangingEventArgs =
                CreateMove(target.NotifyTwoDimensionalListChangingEventType, oldIndex, newIndex, items, direction);
            CollectionChangedEventArgs =
                target.NotifyTwoDimensionalListChangingEventType == target.NotifyTwoDimensionalListChangedEventType
                    ? CollectionChangingEventArgs
                    : CreateMove(target.NotifyTwoDimensionalListChangedEventType, oldIndex, newIndex, items, direction);
        }

        /// <summary>
        ///     AdjustLength, Reset, Clear 系用コンストラクタ
        /// </summary>
        private TwoDimensionalCollectionChangeEventArgsFactory(INotifiableTwoDimensionalListChangeInternal<T> target,
            T[][] oldItems, T[][] newItems, bool isAdjustLength, Direction direction)
        {
            if (isAdjustLength)
            {
                CollectionChangingEventArgs =
                    CreateAdjustLength(target.NotifyTwoDimensionalListChangingEventType, oldItems, newItems,
                        direction);
                CollectionChangedEventArgs =
                    target.NotifyTwoDimensionalListChangingEventType == target.NotifyTwoDimensionalListChangedEventType
                        ? CollectionChangingEventArgs
                        : CreateAdjustLength(target.NotifyTwoDimensionalListChangedEventType, oldItems, newItems,
                            direction);
            }
            else
            {
                CollectionChangingEventArgs =
                    CreateReset(target.NotifyTwoDimensionalListChangingEventType, 0, oldItems, newItems, direction);
                CollectionChangedEventArgs =
                    target.NotifyTwoDimensionalListChangingEventType == target.NotifyTwoDimensionalListChangedEventType
                        ? CollectionChangingEventArgs
                        : CreateReset(target.NotifyTwoDimensionalListChangedEventType, 0, oldItems, newItems,
                            direction);
            }
        }

        /// <summary>
        ///     必要に応じて <paramref name="direction"/> の直角方向に変換する。
        /// </summary>
        /// <param name="isExecute">変換フラグ</param>
        /// <param name="direction">変換元方向</param>
        /// <returns>結果</returns>
        private static Direction TransformRightAngleDirectionIf(bool isExecute, Direction direction)
        {
            if (!isExecute) return direction;

            return direction == Direction.Row
                ? Direction.Column
                : Direction.Row;
        }

        /// <summary>
        ///     <paramref name="target"/> に対して <paramref name="src"/> を InsertRange した結果を取得する
        /// </summary>
        /// <param name="target">Insert対象</param>
        /// <param name="index">インデックス</param>
        /// <param name="src">Insert要素</param>
        /// <returns></returns>
        private static T[][] AcquireInsertedArray(IEnumerable<IEnumerable<T>> target, int index,
            IEnumerable<IEnumerable<T>> src)
        {
            var result = target.ToList();
            result.InsertRange(index, src);
            return result.ToTwoDimensionalArray();
        }

        /// <summary>
        ///     <paramref name="target"/> に対して RemoveRange した結果を取得する
        /// </summary>
        /// <param name="target">Remove対象</param>
        /// <param name="index">Index</param>
        /// <param name="count">Remove数</param>
        /// <returns></returns>
        private static T[][] AcquireRemovedArray(IEnumerable<IEnumerable<T>> target, int index,
            int count)
        {
            var result = target.ToList();
            result.RemoveRange(index, count);
            return result.ToTwoDimensionalArray();
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Enums
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private enum ArgsCreateType
        {
            All,
            Row,
            Column,
            None,
            Empty,
        }

        private enum MultiActionType
        {
            Both,
            Add,
            Replace,
            Reset,
            None
        }
    }
}
