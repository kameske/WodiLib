// ========================================
// Project Name : WodiLib
// File Name    : TwoDimensionalList.RowEventHandlers.cs
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
    internal partial class TwoDimensionalList<TRow, TRowInternal, TItem>
    {
        /// <summary>
        /// 行データイベントハンドラリスト
        /// </summary>
        /// <remarks>
        /// AddRowXXX メソッドを通じて登録されたイベントハンドラを管理する。
        /// </remarks>
        private class RowEventHandlers
        {
            private readonly List<PropertyChangingEventHandler> propertyChangingEventHandlers;
            private readonly List<PropertyChangedEventHandler> propertyChangedEventHandlers;

            private readonly List<EventHandler<NotifyCollectionChangedEventArgs>>
                collectionChangingEventNoGenericHandlers;

            private readonly List<EventHandler<NotifyCollectionChangedEventArgsEx<TItem>>>
                collectionChangingEventGenericHandlers;

            private readonly List<NotifyCollectionChangedEventHandler>
                collectionChangedEventNoGenericHandlers;

            private readonly List<EventHandler<NotifyCollectionChangedEventArgsEx<TItem>>>
                collectionChangedEventGenericHandlers;

            private readonly TwoDimensionalList<TRow, TRowInternal, TItem> parent;

            public RowEventHandlers(TwoDimensionalList<TRow, TRowInternal, TItem> parent)
            {
                this.parent = parent;

                propertyChangingEventHandlers = new List<PropertyChangingEventHandler>();
                propertyChangedEventHandlers = new List<PropertyChangedEventHandler>();
                collectionChangingEventNoGenericHandlers = new List<EventHandler<NotifyCollectionChangedEventArgs>>();
                collectionChangingEventGenericHandlers =
                    new List<EventHandler<NotifyCollectionChangedEventArgsEx<TItem>>>();
                collectionChangedEventNoGenericHandlers =
                    new List<NotifyCollectionChangedEventHandler>();
                collectionChangedEventGenericHandlers =
                    new List<EventHandler<NotifyCollectionChangedEventArgsEx<TItem>>>();
            }

            public void AddPropertyChangingEventHandler(PropertyChangingEventHandler handler)
            {
                AddEventHandlerImpl(
                    handler,
                    propertyChangingEventHandlers,
                    h => parent.Items.ForEach(row => row.PropertyChanging += h)
                );
            }

            public void RemovePropertyChangingEventHandler(PropertyChangingEventHandler handler)
            {
                RemoveEventHandlerImpl(
                    handler,
                    propertyChangingEventHandlers,
                    h => parent.Items.ForEach(row => row.PropertyChanging -= h)
                );
            }

            public void AddPropertyChangedEventHandler(PropertyChangedEventHandler handler)
            {
                AddEventHandlerImpl(
                    handler,
                    propertyChangedEventHandlers,
                    h => parent.Items.ForEach(row => row.PropertyChanged += h)
                );
            }

            public void RemovePropertyChangedEventHandler(PropertyChangedEventHandler handler)
            {
                RemoveEventHandlerImpl(
                    handler,
                    propertyChangedEventHandlers,
                    h => parent.Items.ForEach(row => row.PropertyChanged -= h)
                );
            }

            public void AddCollectionChangingEventHandler(EventHandler<NotifyCollectionChangedEventArgs> handler)
            {
                AddEventHandlerImpl(
                    handler,
                    collectionChangingEventNoGenericHandlers,
                    h => parent.Items.ForEach(row => ((INotifiableCollectionChange)row).CollectionChanging += h)
                );
            }

            public void RemoveCollectionChangingEventHandler(EventHandler<NotifyCollectionChangedEventArgs> handler)
            {
                RemoveEventHandlerImpl(
                    handler,
                    collectionChangingEventNoGenericHandlers,
                    h => parent.Items.ForEach(row => ((INotifiableCollectionChange)row).CollectionChanging -= h)
                );
            }

            public void AddCollectionChangingEventHandler(
                EventHandler<NotifyCollectionChangedEventArgsEx<TItem>> handler)
            {
                AddEventHandlerImpl(
                    handler,
                    collectionChangingEventGenericHandlers,
                    h => parent.Items.ForEach(row => row.CollectionChanging += h)
                );
            }

            public void RemoveCollectionChangingEventHandler(
                EventHandler<NotifyCollectionChangedEventArgsEx<TItem>> handler)
            {
                RemoveEventHandlerImpl(
                    handler,
                    collectionChangingEventGenericHandlers,
                    h => parent.Items.ForEach(row => row.CollectionChanging -= h)
                );
            }

            public void AddCollectionChangedEventHandler(NotifyCollectionChangedEventHandler handler)
            {
                AddEventHandlerImpl(
                    handler,
                    collectionChangedEventNoGenericHandlers,
                    h => parent.Items.ForEach(row => ((INotifiableCollectionChange)row).CollectionChanged += h)
                );
            }

            public void RemoveCollectionChangedEventHandler(NotifyCollectionChangedEventHandler handler)
            {
                RemoveEventHandlerImpl(
                    handler,
                    collectionChangedEventNoGenericHandlers,
                    h => parent.Items.ForEach(row => ((INotifiableCollectionChange)row).CollectionChanged -= h)
                );
            }

            public void AddCollectionChangedEventHandler(
                EventHandler<NotifyCollectionChangedEventArgsEx<TItem>> handler)
            {
                AddEventHandlerImpl(
                    handler,
                    collectionChangedEventGenericHandlers,
                    h => parent.Items.ForEach(row => row.CollectionChanged += h)
                );
            }

            public void RemoveCollectionChangedEventHandler(
                EventHandler<NotifyCollectionChangedEventArgsEx<TItem>> handler)
            {
                RemoveEventHandlerImpl(
                    handler,
                    collectionChangedEventGenericHandlers,
                    h => parent.Items.ForEach(row => row.CollectionChanged -= h)
                );
            }

            public void AddEventHandlers(IEnumerable<TRowInternal> targets)
            {
                targets.ForEach(target =>
                {
                    propertyChangingEventHandlers.ForEach(h => target.PropertyChanging += h);
                    propertyChangedEventHandlers.ForEach(h => target.PropertyChanged += h);
                    collectionChangingEventNoGenericHandlers.ForEach(h =>
                        ((INotifiableCollectionChange)target).CollectionChanging += h);
                    collectionChangingEventGenericHandlers.ForEach(h => target.CollectionChanging += h);
                    collectionChangedEventNoGenericHandlers.ForEach(h =>
                        ((INotifiableCollectionChange)target).CollectionChanged += h);
                    collectionChangedEventGenericHandlers.ForEach(h => target.CollectionChanged += h);
                });
            }

            public void RemoveEventHandlers(IEnumerable<TRowInternal> targets)
            {
                targets.ForEach(target =>
                {
                    propertyChangingEventHandlers.ForEach(h => target.PropertyChanging -= h);
                    propertyChangedEventHandlers.ForEach(h => target.PropertyChanged -= h);
                    collectionChangingEventNoGenericHandlers.ForEach(h =>
                        ((INotifiableCollectionChange)target).CollectionChanging -= h);
                    collectionChangingEventGenericHandlers.ForEach(h => target.CollectionChanging -= h);
                    collectionChangedEventNoGenericHandlers.ForEach(h =>
                        ((INotifiableCollectionChange)target).CollectionChanged -= h);
                    collectionChangedEventGenericHandlers.ForEach(h => target.CollectionChanged -= h);
                });
            }

            private static void AddEventHandlerImpl<THandler>(
                THandler handler,
                ICollection<THandler> handlers,
                Action<THandler> handlerAddAction
            )
            {
                if (handlers.Contains(handler))
                {
                    return;
                }

                handlers.Add(handler);
                handlerAddAction(handler);
            }

            private static void RemoveEventHandlerImpl<THandler>(
                THandler handler,
                ICollection<THandler> handlers,
                Action<THandler> handlerRemoveAction
            )
            {
                if (!handlers.Contains(handler))
                {
                    return;
                }

                handlers.Remove(handler);
                handlerRemoveAction(handler);
            }
        }
    }
}
