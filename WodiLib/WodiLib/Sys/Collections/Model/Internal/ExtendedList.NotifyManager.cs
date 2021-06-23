// ========================================
// Project Name : WodiLib
// File Name    : ExtendedList.NotifyManager.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace WodiLib.Sys.Collections
{
    internal partial class ExtendedList<T>
    {
        /// <summary>
        ///     <see cref="NotifyManager"/> インスタンスを生成する。
        /// </summary>
        /// <remarks>
        ///     プロパティ変更通知およびコレクション変更通知を行う可能性がある。
        /// </remarks>
        /// <param name="collectionChangingEventArgs">コレクション変更前通知イベント引数</param>
        /// <param name="collectionChangedEventArgs">コレクション変更後通知イベント引数</param>
        /// <param name="notifyProperties">変更通知プロパティ名</param>
        /// <returns><see cref="NotifyManager"/> インスタンス</returns>
        private NotifyManager MakeNotifyManager(
            IEnumerable<NotifyCollectionChangedEventArgsEx<T>> collectionChangingEventArgs,
            IEnumerable<NotifyCollectionChangedEventArgsEx<T>> collectionChangedEventArgs,
            params string[] notifyProperties)
            => new(NotifyPropertyChanging,
                NotifyPropertyChanged,
                CallCollectionChanging,
                CallCollectionChanged,
                collectionChangingEventArgs,
                collectionChangedEventArgs,
                notifyProperties);

        /// <summary>
        ///     各種 Notify イベント管理クラス
        /// </summary>
        private class NotifyManager
        {
            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
            //     Private Property
            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

            /// <summary>
            ///     <see cref="INotifyPropertyChanging.PropertyChanging"/> 通知アクション
            /// </summary>
            private Action<string> PropertyChangingAction { get; }

            /// <summary>
            ///     <see cref="INotifyPropertyChanged.PropertyChanged"/> 通知アクション
            /// </summary>
            private Action<string> PropertyChangedAction { get; }

            /// <summary>
            ///     <see cref="INotifyCollectionChange.CollectionChanging"/> 通知アクション
            /// </summary>
            private Action<NotifyCollectionChangedEventArgs>? CollectionChangingAction { get; }

            /// <summary>
            ///     <see cref="INotifyCollectionChanged.CollectionChanged"/> 通知アクション
            /// </summary>
            private Action<NotifyCollectionChangedEventArgs>? CollectionChangedAction { get; }

            private string[] NotifyProperties { get; }

            /// <summary>
            ///     <see cref="INotifyCollectionChange.CollectionChanging"/>
            ///     通知引数
            /// </summary>
            /// <summary>
            ///     <see langword="null"/> 非許容型だが不要な場合は <see langword="null"/> が設定される。
            /// </summary>
            private IEnumerable<NotifyCollectionChangedEventArgsEx<T>>? CollectionChangingEventArgs { get; }

            /// <summary>
            ///     <see cref="INotifyCollectionChange.CollectionChanged"/>
            ///     通知引数
            /// </summary>
            private IEnumerable<NotifyCollectionChangedEventArgsEx<T>>? CollectionChangedEventArgs { get; }

            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
            //     Constructor
            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

            public NotifyManager(
                Action<string> propertyChangingAction,
                Action<string> propertyChangedAction,
                Action<NotifyCollectionChangedEventArgs>? collectionChangingAction,
                Action<NotifyCollectionChangedEventArgs>? collectionChangedAction,
                IEnumerable<NotifyCollectionChangedEventArgsEx<T>>? collectionChangingEventArgs,
                IEnumerable<NotifyCollectionChangedEventArgsEx<T>>? collectionChangedEventArgs,
                params string[] notifyProperties)
            {
                PropertyChangingAction = propertyChangingAction;
                PropertyChangedAction = propertyChangedAction;
                CollectionChangingAction = collectionChangingAction;
                CollectionChangedAction = collectionChangedAction;

                NotifyProperties = notifyProperties;

                CollectionChangingEventArgs = collectionChangingEventArgs;
                CollectionChangedEventArgs = collectionChangedEventArgs;
            }

            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
            //     Public Method
            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

            /// <summary>
            ///     変更前通知を行う。
            /// </summary>
            public void NotifyBeforeEvent()
            {
                /* CollectionChange -> PropertyChange の順で通知（Afterと逆） */

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
                /* PropertyChange -> CollectionChange の順で通知（Beforeと逆） */

                NotifyProperties.ForEach(prop => PropertyChangedAction(prop));

                if (CollectionChangedAction is not null)
                {
                    CollectionChangedEventArgs?.ForEach(args => CollectionChangedAction(args));
                }
            }
        }
    }
}
