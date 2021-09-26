// ========================================
// Project Name : WodiLib
// File Name    : CollectionChangeEventArgsFactory.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace WodiLib.Sys.Collections
{
    internal class CollectionChangeEventArgsFactory<T>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Static Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        public static CollectionChangeEventArgsFactory<T> CreateSet(
            INotifiableCollectionChange target, int index, IReadOnlyList<T> oldItems, IReadOnlyList<T> newItems)
            => new(target, index, oldItems, newItems);

        public static CollectionChangeEventArgsFactory<T> CreateInsert(
            INotifiableCollectionChange target, int index, IReadOnlyList<T> items)
            => new(target, NotifyCollectionChangedAction.Add, index, items);

        public static CollectionChangeEventArgsFactory<T> CreateOverwrite(
            INotifiableCollectionChange target, int index, IReadOnlyList<T> replaceOldItems,
            IReadOnlyList<T> replaceNewItems, IReadOnlyList<T> addNewItems)
            => new(target, index, replaceOldItems, replaceNewItems, addNewItems);

        public static CollectionChangeEventArgsFactory<T> CreateMove(
            INotifiableCollectionChange target, int oldIndex, int newIndex,
            IReadOnlyList<T> items)
            => new(target, oldIndex, newIndex, items);

        public static CollectionChangeEventArgsFactory<T> CreateRemove(
            INotifiableCollectionChange target, int index, IReadOnlyList<T> items)
            => new(target, NotifyCollectionChangedAction.Remove, index, items);

        public static CollectionChangeEventArgsFactory<T> CreateAdjustLengthIfShort(
            INotifiableCollectionChange target, int index, IReadOnlyList<T> items)
            => new(target, true, index, items, null);

        public static CollectionChangeEventArgsFactory<T> CreateAdjustLengthIfLong(
            INotifiableCollectionChange target, int index, IReadOnlyList<T> items)
            => new(target, false, index, items, null);

        public static CollectionChangeEventArgsFactory<T> CreateAdjustLengthIfShort(
            INotifiableCollectionChange target, (IReadOnlyList<T>, IReadOnlyList<T>)? replaceItems,
            int addIndex, IReadOnlyList<T> addItems)
            => new(target, true, addIndex, addItems, replaceItems);

        public static CollectionChangeEventArgsFactory<T> CreateAdjustLengthIfLong(
            INotifiableCollectionChange target, (IReadOnlyList<T>, IReadOnlyList<T>)? replaceItems,
            int removeIndex, IReadOnlyList<T> removeItems)
            => new(target, false, removeIndex, removeItems, replaceItems);

        public static CollectionChangeEventArgsFactory<T> CreateReset(
            INotifiableCollectionChange target, IReadOnlyList<T> oldItems,
            IReadOnlyList<T> newItems)
            => new(target, oldItems, newItems);

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        public IEnumerable<NotifyCollectionChangedEventArgsEx<T>> CollectionChangingEventArgs { get; }
        public IEnumerable<NotifyCollectionChangedEventArgsEx<T>> CollectionChangedEventArgs { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     Set, SetRange 用コンストラクタ
        /// </summary>
        private CollectionChangeEventArgsFactory(
            INotifiableCollectionChange target, int index,
            IReadOnlyList<T> oldItems, IReadOnlyList<T> newItems)
        {
            CollectionChangingEventArgs = CreateSet(target.NotifyCollectionChangingEventType,
                index, oldItems, newItems);

            CollectionChangedEventArgs =
                target.NotifyCollectionChangingEventType == target.NotifyCollectionChangedEventType
                    ? CollectionChangingEventArgs
                    : CreateSet(target.NotifyCollectionChangedEventType,
                        index, oldItems, newItems);
        }

        /// <summary>
        ///     Insert, InsertRange, Remove, RemoveRange 用コンストラクタ
        /// </summary>
        private CollectionChangeEventArgsFactory(INotifiableCollectionChange target,
            NotifyCollectionChangedAction action,
            int index, IReadOnlyList<T> items)
        {
            switch (action)
            {
                case NotifyCollectionChangedAction.Add:
                    CollectionChangingEventArgs = CreateInsert(target.NotifyCollectionChangingEventType,
                        index, items);
                    CollectionChangedEventArgs = target.NotifyCollectionChangingEventType ==
                                                 target.NotifyCollectionChangedEventType
                        ? CollectionChangingEventArgs
                        : CreateInsert(target.NotifyCollectionChangedEventType,
                            index, items);
                    break;

                case NotifyCollectionChangedAction.Remove:
                    CollectionChangingEventArgs = CreateRemove(target.NotifyCollectionChangingEventType,
                        index, items);
                    CollectionChangedEventArgs = target.NotifyCollectionChangingEventType ==
                                                 target.NotifyCollectionChangedEventType
                        ? CollectionChangingEventArgs
                        : CreateRemove(target.NotifyCollectionChangedEventType,
                            index, items);
                    break;

                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        ///     Overwrite 用コンストラクタ
        /// </summary>
        private CollectionChangeEventArgsFactory(INotifiableCollectionChange target,
            int index, IReadOnlyList<T> replaceOldItems, IReadOnlyList<T> replaceNewItems,
            IReadOnlyList<T> addNewItems)
        {
            CollectionChangingEventArgs = CreateOverwrite(target.NotifyCollectionChangingEventType, index,
                replaceOldItems, replaceNewItems, addNewItems);
            CollectionChangedEventArgs =
                target.NotifyCollectionChangingEventType == target.NotifyCollectionChangedEventType
                    ? CollectionChangingEventArgs
                    : CreateOverwrite(target.NotifyCollectionChangedEventType, index,
                        replaceOldItems, replaceNewItems, addNewItems);
        }

        /// <summary>
        ///     Move, MoveRange 用コンストラクタ
        /// </summary>
        private CollectionChangeEventArgsFactory(INotifiableCollectionChange target,
            int oldIndex, int newIndex, IReadOnlyList<T> items)
        {
            CollectionChangingEventArgs =
                CreateMove(target.NotifyCollectionChangingEventType, oldIndex, newIndex, items);
            CollectionChangedEventArgs =
                target.NotifyCollectionChangingEventType == target.NotifyCollectionChangedEventType
                    ? CollectionChangingEventArgs
                    : CreateMove(target.NotifyCollectionChangedEventType, oldIndex, newIndex, items);
        }

        /// <summary>
        ///     AdjustLength, AdjustLengthIfShort, AdjustLengthIfLong 用コンストラクタ
        /// </summary>
        private CollectionChangeEventArgsFactory(INotifiableCollectionChange target, bool adjustIfShort,
            int index, IReadOnlyList<T> items, (IReadOnlyList<T>, IReadOnlyList<T>)? replaceItems)
        {
            if (adjustIfShort)
            {
                CollectionChangingEventArgs =
                    CreateAdjustLengthIfShort(target.NotifyCollectionChangingEventType,
                        index, items, replaceItems);
                CollectionChangedEventArgs =
                    target.NotifyCollectionChangingEventType == target.NotifyCollectionChangedEventType
                        ? CollectionChangingEventArgs
                        : CreateAdjustLengthIfShort(target.NotifyCollectionChangedEventType,
                            index, items, replaceItems);
            }
            else
            {
                CollectionChangingEventArgs =
                    CreateAdjustLengthIfLong(target.NotifyCollectionChangingEventType,
                        index, items, replaceItems);
                CollectionChangedEventArgs =
                    target.NotifyCollectionChangingEventType == target.NotifyCollectionChangedEventType
                        ? CollectionChangingEventArgs
                        : CreateAdjustLengthIfLong(target.NotifyCollectionChangedEventType,
                            index, items, replaceItems);
            }
        }

        /// <summary>
        ///     Reset, Clear 用コンストラクタ
        /// </summary>
        private CollectionChangeEventArgsFactory(INotifiableCollectionChange target,
            IReadOnlyList<T> oldItems, IReadOnlyList<T> newItems)
        {
            CollectionChangingEventArgs = CreateReset(target.NotifyCollectionChangingEventType, oldItems, newItems);
            CollectionChangedEventArgs =
                target.NotifyCollectionChangingEventType == target.NotifyCollectionChangedEventType
                    ? CollectionChangingEventArgs
                    : CreateReset(target.NotifyCollectionChangedEventType, oldItems, newItems);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Static Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private static IEnumerable<NotifyCollectionChangedEventArgsEx<T>> CreateSet(
            NotifyCollectionChangeEventType type, int index, IReadOnlyList<T> oldItems, IReadOnlyList<T> newItems)
        {
            if (!type.IsNotify) return Array.Empty<NotifyCollectionChangedEventArgsEx<T>>();

            if (oldItems.Count == 0)
            {
                return Array.Empty<NotifyCollectionChangedEventArgsEx<T>>();
            }

            if (!type.IsMultipart)
            {
                return new[]
                {
                    NotifyCollectionChangedEventArgsEx<T>.CreateSetArgs(index, oldItems, newItems)
                };
            }

            return oldItems.Zip(newItems).Select((zip, idx) =>
                NotifyCollectionChangedEventArgsEx<T>.CreateSetArgs(index + idx, zip.Item1, zip.Item2));
        }

        private static IEnumerable<NotifyCollectionChangedEventArgsEx<T>> CreateInsert(
            NotifyCollectionChangeEventType type, int index, IReadOnlyList<T> items)
        {
            if (!type.IsNotify) return Array.Empty<NotifyCollectionChangedEventArgsEx<T>>();

            if (items.Count == 0)
            {
                return Array.Empty<NotifyCollectionChangedEventArgsEx<T>>();
            }

            if (!type.IsMultipart)
            {
                return new[]
                {
                    NotifyCollectionChangedEventArgsEx<T>.CreateInsertArgs(index, items)
                };
            }

            return items.Select((item, idx) =>
                NotifyCollectionChangedEventArgsEx<T>.CreateInsertArgs(index + idx, item));
        }

        private static IEnumerable<NotifyCollectionChangedEventArgsEx<T>> CreateOverwrite(
            NotifyCollectionChangeEventType type, int index, IReadOnlyList<T> replaceOldItems,
            IReadOnlyList<T> replaceNewItems, IReadOnlyList<T> addNewItems)
        {
            if (!type.IsNotify) return Array.Empty<NotifyCollectionChangedEventArgsEx<T>>();

            if (replaceOldItems.Count == 0 && addNewItems.Count == 0)
            {
                return Array.Empty<NotifyCollectionChangedEventArgsEx<T>>();
            }

            {
                var replaceLength = replaceOldItems.Count;
                var addLength = addNewItems.Count;

                var isNotChange = replaceLength == 0 && addLength == 0;
                if (isNotChange) return Array.Empty<NotifyCollectionChangedEventArgsEx<T>>();

                var isOnlyReplace = replaceLength != 0 && addLength == 0;
                if (isOnlyReplace)
                {
                    return CreateSet(type, index, replaceOldItems, replaceNewItems);
                }

                var isOnlyAdd = replaceLength == 0 && addLength != 0;
                if (isOnlyAdd)
                {
                    return CreateInsert(type, index, addNewItems);
                }
            }

            if (!type.IsMultiAction)
            {
                return new[]
                {
                    NotifyCollectionChangedEventArgsEx<T>.CreateResetArgs(index,
                        replaceOldItems, replaceNewItems.Concat(addNewItems).ToList())
                };
            }

            var insertStartIndex = index + replaceOldItems.Count;

            if (!type.IsMultipart)
            {
                return new[]
                {
                    NotifyCollectionChangedEventArgsEx<T>.CreateSetArgs(index, replaceOldItems, replaceNewItems),
                    NotifyCollectionChangedEventArgsEx<T>.CreateInsertArgs(insertStartIndex, addNewItems)
                };
            }

            var replaceEventArgs = replaceOldItems.Zip(replaceNewItems).Select((zip, idx) =>
                NotifyCollectionChangedEventArgsEx<T>.CreateSetArgs(index + idx, zip.Item1, zip.Item2));
            var addEventArgs = addNewItems.Select((item, idx) =>
                NotifyCollectionChangedEventArgsEx<T>.CreateInsertArgs(insertStartIndex + idx, item));

            return replaceEventArgs.Concat(addEventArgs);
        }

        private static IEnumerable<NotifyCollectionChangedEventArgsEx<T>> CreateMove(
            NotifyCollectionChangeEventType type, int oldIndex, int newIndex,
            IReadOnlyList<T> items)
        {
            if (!type.IsNotify) return Array.Empty<NotifyCollectionChangedEventArgsEx<T>>();

            if (items.Count == 0)
            {
                return Array.Empty<NotifyCollectionChangedEventArgsEx<T>>();
            }

            if (!type.IsMultipart)
            {
                return new[]
                {
                    NotifyCollectionChangedEventArgsEx<T>.CreateMoveArgs(oldIndex, newIndex, items)
                };
            }

            return items.Select((item, idx) =>
                NotifyCollectionChangedEventArgsEx<T>.CreateMoveArgs(oldIndex + idx, newIndex + idx, item));
        }

        private static IEnumerable<NotifyCollectionChangedEventArgsEx<T>> CreateRemove(
            NotifyCollectionChangeEventType type, int index, IReadOnlyList<T> items)
        {
            if (!type.IsNotify)
            {
                return Array.Empty<NotifyCollectionChangedEventArgsEx<T>>();
            }

            if (items.Count == 0)
            {
                return Array.Empty<NotifyCollectionChangedEventArgsEx<T>>();
            }

            if (!type.IsMultipart)
            {
                return new[]
                {
                    NotifyCollectionChangedEventArgsEx<T>.CreateRemoveArgs(index, items)
                };
            }

            return items.Select((item, idx) =>
                NotifyCollectionChangedEventArgsEx<T>.CreateRemoveArgs(index + idx, item));
        }

        private static IEnumerable<NotifyCollectionChangedEventArgsEx<T>> CreateAdjustLengthIfShort(
            NotifyCollectionChangeEventType type, int index,
            IReadOnlyList<T> items, (IReadOnlyList<T>, IReadOnlyList<T>)? replaceItems)
        {
            var insertArgs = CreateInsert(type, index, items);
            if (!replaceItems.HasValue)
            {
                return insertArgs;
            }

            var (replaceOldItems, replaceNewItems) = replaceItems.Value;
            var replaceArgs = CreateSet(type, 0, replaceOldItems, replaceNewItems);

            return replaceArgs.Concat(insertArgs);
        }

        private static IEnumerable<NotifyCollectionChangedEventArgsEx<T>> CreateAdjustLengthIfLong(
            NotifyCollectionChangeEventType type, int index,
            IReadOnlyList<T> items, (IReadOnlyList<T>, IReadOnlyList<T>)? replaceItems)
        {
            var removeArgs = CreateRemove(type, index, items);
            if (!replaceItems.HasValue)
            {
                return removeArgs;
            }

            var (replaceOldItems, replaceNewItems) = replaceItems.Value;
            var replaceArgs = CreateSet(type, 0, replaceOldItems, replaceNewItems);

            return replaceArgs.Concat(removeArgs);
        }

        private static IEnumerable<NotifyCollectionChangedEventArgsEx<T>> CreateReset(
            NotifyCollectionChangeEventType type, IReadOnlyList<T> oldItems,
            IReadOnlyList<T> newItems)
        {
            if (!type.IsNotify) return Array.Empty<NotifyCollectionChangedEventArgsEx<T>>();

            if (oldItems.Count == 0 && newItems.Count == 0)
            {
                return Array.Empty<NotifyCollectionChangedEventArgsEx<T>>();
            }

            return new[]
            {
                NotifyCollectionChangedEventArgsEx<T>.CreateResetArgs(0, oldItems, newItems)
            };
        }
    }
}
