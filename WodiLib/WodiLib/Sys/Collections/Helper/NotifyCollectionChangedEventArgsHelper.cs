// ========================================
// Project Name : WodiLib
// File Name    : NotifyCollectionChangedEventArgsHelper.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections;
using System.Collections.Specialized;

namespace WodiLib.Sys
{
    /// <summary>
    /// NotifyCollectionChangedEventArgs Helperクラス
    /// </summary>
    internal static class NotifyCollectionChangedEventArgsHelper
    {
        /// <summary>
        /// Setイベント時の変更通知
        /// </summary>
        /// <param name="newItem">変更後オブジェクト</param>
        /// <param name="oldItem">変更前オブジェクト</param>
        /// <param name="index">対象インデックス</param>
        /// <returns>CollectionChangedイベント引数</returns>
        public static NotifyCollectionChangedEventArgs Set(object newItem, object oldItem, int index)
            => new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, newItem, oldItem, index);

        /// <summary>
        /// SetRangeイベント時の変更通知
        /// </summary>
        /// <param name="newItems">変更後オブジェクト</param>
        /// <param name="oldItems">変更前オブジェクト</param>
        /// <param name="startingIndex">対象インデックス</param>
        /// <returns>CollectionChangedイベント引数</returns>
        public static NotifyCollectionChangedEventArgs SetRange(IList newItems, IList oldItems, int startingIndex)
            => new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, newItems, oldItems,
                startingIndex);

        /// <summary>
        /// Insertイベント時の変更通知
        /// </summary>
        /// <param name="item">挿入オブジェクト</param>
        /// <param name="index">対象インデックス</param>
        /// <returns>CollectionChangedイベント引数</returns>
        public static NotifyCollectionChangedEventArgs Insert(object item, int index)
            => new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index);

        /// <summary>
        /// Insertイベント時の変更通知
        /// </summary>
        /// <param name="items">挿入オブジェクト</param>
        /// <param name="index">対象インデックス</param>
        /// <returns>CollectionChangedイベント引数</returns>
        public static NotifyCollectionChangedEventArgs InsertRange(IList items, int index)
            => new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, items, index);

        /// <summary>
        /// Moveイベント時の変更通知
        /// </summary>
        /// <param name="item">挿入オブジェクト</param>
        /// <param name="newIndex">移動先インデックス</param>
        /// <param name="oldIndex">移動元インデックス</param>
        /// <returns>CollectionChangedイベント引数</returns>
        public static NotifyCollectionChangedEventArgs Move(object item, int newIndex, int oldIndex)
            => new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, item, newIndex, oldIndex);

        /// <summary>
        /// MoveRangeイベント時の変更通知
        /// </summary>
        /// <param name="items">挿入オブジェクト</param>
        /// <param name="newIndex">移動先インデックス</param>
        /// <param name="oldIndex">移動元インデックス</param>
        /// <returns>CollectionChangedイベント引数</returns>
        public static NotifyCollectionChangedEventArgs MoveRange(IList items, int newIndex, int oldIndex)
            => new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, items, newIndex, oldIndex);

        /// <summary>
        /// Removeイベント時の変更通知
        /// </summary>
        /// <param name="item">除去オブジェクト</param>
        /// <param name="index">対象インデックス</param>
        /// <returns>CollectionChangedイベント引数</returns>
        public static NotifyCollectionChangedEventArgs Remove(object item, int index)
            => new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index);

        /// <summary>
        /// RemoveRangeイベント時の変更通知
        /// </summary>
        /// <param name="items">除去オブジェクト</param>
        /// <param name="index">対象インデックス</param>
        /// <returns>CollectionChangedイベント引数</returns>
        public static NotifyCollectionChangedEventArgs RemoveRange(IList items, int index)
            => new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, items, index);

        /// <summary>
        /// Clearイベント時の変更通知
        /// </summary>
        /// <returns>CollectionChangedイベント引数</returns>
        public static NotifyCollectionChangedEventArgs Clear()
            => new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
    }
}