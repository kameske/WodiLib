// ========================================
// Project Name : WodiLib
// File Name    : NotifyCollectionChangedEventArgsHelper.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections;
using System.Collections.Specialized;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     <see cref="NotifyCollectionChangedEventArgs"/> インスタンス生成 Helper クラス
    /// </summary>
    internal static class NotifyCollectionChangedEventArgsHelper
    {
        /// <summary>
        ///     SetRangeイベント時の変更通知
        /// </summary>
        /// <param name="newItems">変更後オブジェクト</param>
        /// <param name="oldItems">変更前オブジェクト</param>
        /// <param name="startingIndex">対象インデックス</param>
        /// <returns>イベント引数</returns>
        public static NotifyCollectionChangedEventArgs Set(IList newItems, IList oldItems, int startingIndex)
            => new(NotifyCollectionChangedAction.Replace, newItems, oldItems,
                startingIndex);

        /// <summary>
        ///     Insertイベント時の変更通知
        /// </summary>
        /// <param name="items">挿入オブジェクト</param>
        /// <param name="index">対象インデックス</param>
        /// <returns>イベント引数</returns>
        public static NotifyCollectionChangedEventArgs Insert(IList items, int index)
            => new(NotifyCollectionChangedAction.Add, items, index);

        /// <summary>
        ///     MoveRangeイベント時の変更通知
        /// </summary>
        /// <param name="items">挿入オブジェクト</param>
        /// <param name="newIndex">移動先インデックス</param>
        /// <param name="oldIndex">移動元インデックス</param>
        /// <returns>イベント引数</returns>
        public static NotifyCollectionChangedEventArgs Move(IList items, int newIndex, int oldIndex)
            => new(NotifyCollectionChangedAction.Move, items, newIndex, oldIndex);

        /// <summary>
        ///     RemoveRangeイベント時の変更通知
        /// </summary>
        /// <param name="items">除去オブジェクト</param>
        /// <param name="index">対象インデックス</param>
        /// <returns>イベント引数</returns>
        public static NotifyCollectionChangedEventArgs Remove(IList items, int index)
            => new(NotifyCollectionChangedAction.Remove, items, index);

        /// <summary>
        ///     Clearイベント時の変更通知
        /// </summary>
        /// <returns>イベント引数</returns>
        public static NotifyCollectionChangedEventArgs Clear()
            => new(NotifyCollectionChangedAction.Reset);
    }
}
