// ========================================
// Project Name : WodiLib
// File Name    : ExtendedList.NotifyManager.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Specialized;
using System.ComponentModel;

namespace WodiLib.Sys
{
    internal partial class ExtendedList<T>
    {
        /// <summary>
        /// <see cref="NotifyManager"/> インスタンスを生成する。
        /// </summary>
        /// <remarks>
        /// プロパティ変更通知のみ行う可能性がある。コレクション変更通知は行わない。
        /// </remarks>
        /// <param name="notifyProperties">変更通知プロパティ名</param>
        /// <returns><see cref="NotifyManager"/> インスタンス</returns>
        private NotifyManager MakeNotifyManager(params string[] notifyProperties)
            => new(IsNotifyBeforePropertyChange,
                IsNotifyAfterPropertyChange,
                NotifyPropertyChanging,
                NotifyPropertyChanged,
                notifyProperties);

        /// <summary>
        /// <see cref="NotifyManager"/> インスタンスを生成する。
        /// </summary>
        /// <remarks>
        /// プロパティ変更通知およびコレクション変更通知を行う可能性がある。
        /// </remarks>
        /// <param name="collectionChangedEventArgs">コレクション変更通知引数生成関数</param>
        /// <param name="notifyProperties">変更通知プロパティ名</param>
        /// <returns><see cref="NotifyManager"/> インスタンス</returns>
        private NotifyManager MakeNotifyManager(
            Func<NotifyCollectionChangedEventArgs> collectionChangedEventArgs,
            params string[] notifyProperties)
            => new(IsNotifyBeforePropertyChange,
                IsNotifyAfterPropertyChange,
                IsNotifyBeforeCollectionChange,
                IsNotifyAfterCollectionChange,
                NotifyPropertyChanging,
                NotifyPropertyChanged,
                CallCollectionChanging,
                CallCollectionChanged,
                collectionChangedEventArgs,
                notifyProperties);

        /// <summary>
        /// 各種 Notify イベント管理クラス
        /// </summary>
        private class NotifyManager
        {
            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
            //     Private Property
            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

            /// <summary>
            /// <see cref="INotifyPropertyChanging.PropertyChanging"/> 通知フラグ
            /// </summary>
            private bool IsNotifyBeforePropertyChange { get; }

            /// <summary>
            /// <see cref="INotifyPropertyChanged.PropertyChanged"/> 通知フラグ
            /// </summary>
            private bool IsNotifyAfterPropertyChange { get; }

            /// <summary>
            /// <see cref="INotifyCollectionChange.CollectionChanging"/> 通知フラグ
            /// </summary>
            private bool IsNotifyBeforeCollectionChange { get; }

            /// <summary>
            /// <see cref="INotifyCollectionChanged.CollectionChanged"/> 通知フラグ
            /// </summary>
            private bool IsNotifyAfterCollectionChange { get; }

            /// <summary>
            /// <see cref="INotifyPropertyChanging.PropertyChanging"/> 通知アクション
            /// </summary>
            private Action<string> PropertyChangingAction { get; }

            /// <summary>
            /// <see cref="INotifyPropertyChanged.PropertyChanged"/> 通知アクション
            /// </summary>
            private Action<string> PropertyChangedAction { get; }

            /// <summary>
            /// <see cref="INotifyCollectionChange.CollectionChanging"/> 通知アクション
            /// </summary>
            private Action<NotifyCollectionChangedEventArgs> CollectionChangingAction { get; }

            /// <summary>
            /// <see cref="INotifyCollectionChanged.CollectionChanged"/> 通知アクション
            /// </summary>
            private Action<NotifyCollectionChangedEventArgs> CollectionChangedAction { get; }

            /// <summary>
            /// <see cref="INotifyCollectionChange.CollectionChanging"/>
            /// および <see cref="INotifyCollectionChanged.CollectionChanged"/>
            /// 通知引数生成関数
            /// </summary>
            private Func<NotifyCollectionChangedEventArgs> CollectionChangedEventArgs { get; }

            private string[] NotifyProperties { get; }

            /// <summary>
            /// <see cref="INotifyCollectionChange.CollectionChanging"/>
            /// および <see cref="INotifyCollectionChanged.CollectionChanged"/>
            /// 通知引数
            /// </summary>
            /// <summary>
            /// <see langword="null"/> 非許容型だが不要な場合は <see langword="null"/> が設定される。
            /// </summary>
            private NotifyCollectionChangedEventArgs CollectionChangeEventArgs { get; }

            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
            //     Constructor
            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

            public NotifyManager(
                bool isNotifyBeforePropertyChange,
                bool isNotifyAfterPropertyChange,
                Action<string> propertyChangingAction,
                Action<string> propertyChangedAction,
                params string[] notifyProperties)
                : this(
                    isNotifyBeforePropertyChange,
                    isNotifyAfterPropertyChange,
                    false, false,
                    propertyChangingAction,
                    propertyChangedAction,
                    null!, null!, null!,
                    notifyProperties
                )
            {
            }

            public NotifyManager(
                bool isNotifyBeforePropertyChange,
                bool isNotifyAfterPropertyChange,
                bool isNotifyBeforeCollectionChange,
                bool isNotifyAfterCollectionChange,
                Action<string> propertyChangingAction,
                Action<string> propertyChangedAction,
                Action<NotifyCollectionChangedEventArgs> collectionChangingAction,
                Action<NotifyCollectionChangedEventArgs> collectionChangedAction,
                Func<NotifyCollectionChangedEventArgs> collectionChangedEventArgs,
                params string[] notifyProperties)
            {
                IsNotifyBeforePropertyChange = isNotifyBeforePropertyChange;
                IsNotifyAfterPropertyChange = isNotifyAfterPropertyChange;
                IsNotifyBeforeCollectionChange = isNotifyBeforeCollectionChange;
                IsNotifyAfterCollectionChange = isNotifyAfterCollectionChange;

                PropertyChangingAction = propertyChangingAction;
                PropertyChangedAction = propertyChangedAction;
                CollectionChangingAction = collectionChangingAction;
                CollectionChangedAction = collectionChangedAction;

                CollectionChangedEventArgs = collectionChangedEventArgs;
                NotifyProperties = notifyProperties;

                CollectionChangeEventArgs =
                    IsNotifyBeforeCollectionChange || IsNotifyAfterCollectionChange
                        ? CollectionChangedEventArgs()
                        : null!;
            }

            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
            //     Public Method
            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

            /// <summary>
            /// 変更前通知を行う。
            /// </summary>
            public void NotifyBeforeEvent()
            {
                /* CollectionChange -> PropertyChange の順で通知（Afterと逆） */

                if (IsNotifyBeforeCollectionChange)
                {
                    CollectionChangingAction(CollectionChangeEventArgs);
                }

                if (IsNotifyBeforePropertyChange)
                {
                    NotifyProperties.ForEach(prop => PropertyChangingAction(prop));
                }
            }

            /// <summary>
            /// 変更後通知を行う。
            /// </summary>
            public void NotifyAfterEvent()
            {
                /* PropertyChange -> CollectionChange の順で通知（Beforeと逆） */

                if (IsNotifyAfterPropertyChange)
                {
                    NotifyProperties.ForEach(prop => PropertyChangedAction(prop));
                }

                if (IsNotifyAfterCollectionChange)
                {
                    CollectionChangedAction(CollectionChangeEventArgs);
                }
            }
        }
    }
}
