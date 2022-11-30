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
    internal partial class TwoDimensionalList<TRow, TItem>
    {
        /// <summary>
        /// 行データイベントハンドラリスト
        /// </summary>
        /// <remarks>
        /// AddRowXXX メソッドを通じて登録されたイベントハンドラを管理する。
        /// </remarks>
        private class RowEventHandlers
        {
            private readonly List<PropertyChangedEventHandler> propertyChangedEventHandlers;

            private readonly List<NotifyCollectionChangedEventHandler>
                collectionChangedEventHandlers;

            private readonly TwoDimensionalList<TRow, TItem> parent;

            public RowEventHandlers(TwoDimensionalList<TRow, TItem> parent)
            {
                this.parent = parent;

                propertyChangedEventHandlers = new List<PropertyChangedEventHandler>();
                collectionChangedEventHandlers =
                    new List<NotifyCollectionChangedEventHandler>();
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

            public void AddCollectionChangedEventHandler(NotifyCollectionChangedEventHandler handler)
            {
                AddEventHandlerImpl(
                    handler,
                    collectionChangedEventHandlers,
                    h => parent.Items.ForEach(row => row.CollectionChanged += h)
                );
            }

            public void RemoveCollectionChangedEventHandler(NotifyCollectionChangedEventHandler handler)
            {
                RemoveEventHandlerImpl(
                    handler,
                    collectionChangedEventHandlers,
                    h => parent.Items.ForEach(row => row.CollectionChanged -= h)
                );
            }

            public void AddEventHandlers(IEnumerable<TRow> targets)
            {
                targets.ForEach(
                    target =>
                    {
                        propertyChangedEventHandlers.ForEach(h => target.PropertyChanged += h);
                        collectionChangedEventHandlers.ForEach(h => target.CollectionChanged += h);
                    }
                );
            }

            public void RemoveEventHandlers(IEnumerable<TRow> targets)
            {
                targets.ForEach(
                    target =>
                    {
                        propertyChangedEventHandlers.ForEach(h => target.PropertyChanged -= h);
                        collectionChangedEventHandlers.ForEach(h => target.CollectionChanged -= h);
                    }
                );
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
