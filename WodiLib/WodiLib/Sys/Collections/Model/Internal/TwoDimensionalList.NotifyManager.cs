// ========================================
// Project Name : WodiLib
// File Name    : TwoDimensionalList.NotifyManager.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace WodiLib.Sys.Collections
{
    internal partial class TwoDimensionalList<T>
    {
        /// <summary>
        ///     <see cref="NotifyManager"/> インスタンスを生成する。
        /// </summary>
        /// <param name="collectionChangeEventArgsFactory">リスト変更通知Factory</param>
        /// <param name="twoDimensionalCollectionChangeEventArgsFactory">二次元リスト変更通知Factory</param>
        /// <param name="notifyProperties">変更通知プロパティ名</param>
        /// <returns><see cref="NotifyManager"/> インスタンス</returns>
        private NotifyManager MakeNotifyManager(
            CollectionChangeEventArgsFactory<IReadOnlyList<T>> collectionChangeEventArgsFactory,
            TwoDimensionalCollectionChangeEventArgsFactory<T> twoDimensionalCollectionChangeEventArgsFactory,
            params string[] notifyProperties)
            => new(
                NotifyPropertyChanging,
                NotifyPropertyChanged,
                RaiseCollectionChanging,
                RaiseCollectionChanged,
                RaiseTowDimensionalListChanging,
                RaiseTowDimensionalListChanged,
                collectionChangeEventArgsFactory.CollectionChangingEventArgs,
                collectionChangeEventArgsFactory.CollectionChangedEventArgs,
                twoDimensionalCollectionChangeEventArgsFactory.CollectionChangingEventArgs,
                twoDimensionalCollectionChangeEventArgsFactory.CollectionChangedEventArgs,
                notifyProperties);

        private class NotifyManager
        {
            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
            //      Private Properties
            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

            private Action<string> PropertyChangingAction { get; }
            private Action<string> PropertyChangedAction { get; }

            private Action<NotifyCollectionChangedEventArgs>? CollectionChangingAction { get; }
            private Action<NotifyCollectionChangedEventArgs>? CollectionChangedAction { get; }

            private Action<TwoDimensionalCollectionChangeEventInternalArgs<T>>?
                TwoDimensionalListChangingAction { get; }

            private Action<TwoDimensionalCollectionChangeEventInternalArgs<T>>?
                TwoDimensionalListChangedAction { get; }

            private string[] NotifyProperties { get; }

            private IEnumerable<NotifyCollectionChangedEventArgsEx<IReadOnlyList<T>>>?
                CollectionChangingEventArgs { get; }


            private IEnumerable<NotifyCollectionChangedEventArgsEx<IReadOnlyList<T>>>?
                CollectionChangedEventArgs { get; }

            private IEnumerable<TwoDimensionalCollectionChangeEventInternalArgs<T>>?
                TwoDimensionalListChangingEventArgs { get; }


            private IEnumerable<TwoDimensionalCollectionChangeEventInternalArgs<T>>?
                TwoDimensionalListChangedEventArgs { get; }

            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
            //      Constructors
            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

            public NotifyManager(
                Action<string> propertyChangingAction,
                Action<string> propertyChangedAction,
                Action<NotifyCollectionChangedEventArgs>? collectionChangingAction,
                Action<NotifyCollectionChangedEventArgs>? collectionChangedAction,
                Action<TwoDimensionalCollectionChangeEventInternalArgs<T>>? twoDimensionalListChangingAction,
                Action<TwoDimensionalCollectionChangeEventInternalArgs<T>>? twoDimensionalListChangedAction,
                IEnumerable<NotifyCollectionChangedEventArgsEx<IReadOnlyList<T>>>? collectionChangingEventArgs,
                IEnumerable<NotifyCollectionChangedEventArgsEx<IReadOnlyList<T>>>? collectionChangedEventArgs,
                IEnumerable<TwoDimensionalCollectionChangeEventInternalArgs<T>>?
                    twoDimensionalListChangingEventArgs,
                IEnumerable<TwoDimensionalCollectionChangeEventInternalArgs<T>>? twoDimensionalListChangedEventArgs,
                params string[] notifyProperties)
            {
                PropertyChangingAction = propertyChangingAction;
                PropertyChangedAction = propertyChangedAction;
                CollectionChangingAction = collectionChangingAction;
                CollectionChangedAction = collectionChangedAction;
                TwoDimensionalListChangingAction = twoDimensionalListChangingAction;
                TwoDimensionalListChangedAction = twoDimensionalListChangedAction;

                NotifyProperties = notifyProperties;

                CollectionChangingEventArgs = collectionChangingEventArgs;
                CollectionChangedEventArgs = collectionChangedEventArgs;
                TwoDimensionalListChangingEventArgs = twoDimensionalListChangingEventArgs;
                TwoDimensionalListChangedEventArgs = twoDimensionalListChangedEventArgs;
            }

            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
            //      Public Methods
            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

            /// <summary>
            ///     変更前通知を行う。
            /// </summary>
            public void NotifyBeforeEvent()
            {
                /* TwoDimensionalListChange -> CollectionChange -> PropertyChange の順で通知（Afterと逆） */

                if (TwoDimensionalListChangingAction is not null)
                {
                    TwoDimensionalListChangingEventArgs?.ForEach(args => TwoDimensionalListChangingAction(args));
                }

                if (CollectionChangingAction is not null)
                {
                    CollectionChangingEventArgs?.ForEach(args => CollectionChangingAction(args));
                }

                NotifyProperties.ForEach(prop => PropertyChangingAction(prop));
            }

            /// <summary>
            ///     変更後通知を行う。
            /// </summary>
            public void NotifyAfterEvent()
            {
                /* PropertyChange -> CollectionChange -> TwoDimensionalListChange の順で通知（Beforeと逆） */

                NotifyProperties.ForEach(prop => PropertyChangedAction(prop));

                if (CollectionChangedAction is not null)
                {
                    CollectionChangedEventArgs?.ForEach(args => CollectionChangedAction(args));
                }

                if (TwoDimensionalListChangedAction is not null)
                {
                    TwoDimensionalListChangedEventArgs?.ForEach(args => TwoDimensionalListChangedAction(args));
                }
            }
        }
    }
}
