// ========================================
// Project Name : WodiLib
// File Name    : ListExtension.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace WodiLib.Sys
{
    /// <summary>
    /// NotifyCollectionChangedEventArgs 拡張クラス
    /// </summary>
    internal static class NotifyCollectionChangedEventArgsExtension
    {
        /// <summary>
        /// インスタンスが持つ Action の内容を基に与えられたアクションのいずれかを実行する。
        /// </summary>
        /// <param name="e">NotifyCollectionChangedEventArgsインスタンス</param>
        /// <param name="replaceAction">Replaceイベントである場合に実行するアクション</param>
        /// <param name="addAction">Addイベントである場合に実行するアクション</param>
        /// <param name="moveAction">Moveイベントである場合に実行するアクション</param>
        /// <param name="removeAction">Removeイベントである場合に実行するアクション</param>
        /// <param name="resetAction">Resetイベントである場合に実行するアクション</param>
        /// <typeparam name="T"></typeparam>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void ExecuteByAction<T>(this NotifyCollectionChangedEventArgs e,
            ReplaceAction<T>? replaceAction = null,
            AddAction<T>? addAction = null,
            MoveAction<T>? moveAction = null,
            RemoveAction<T>? removeAction = null,
            ResetAction? resetAction = null)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Replace:
                    replaceAction?.Invoke(e.NewStartingIndex, e.OldItems.Cast<T>(), e.NewItems.Cast<T>());
                    break;

                case NotifyCollectionChangedAction.Add:
                    addAction?.Invoke(e.NewStartingIndex, e.NewItems.Cast<T>());
                    break;
                case NotifyCollectionChangedAction.Move:
                    moveAction?.Invoke(e.OldStartingIndex, e.NewStartingIndex, e.NewItems.Cast<T>());
                    break;

                case NotifyCollectionChangedAction.Remove:
                    removeAction?.Invoke(e.OldStartingIndex, e.OldItems.Cast<T>());
                    break;

                case NotifyCollectionChangedAction.Reset:
                    resetAction?.Invoke();
                    break;

                default:
                    // 通常ここには来ない
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Setイベント
        /// </summary>
        /// <param name="index">更新する要素の先頭インデックス</param>
        /// <param name="oldItems">更新前要素</param>
        /// <param name="newItems">更新後要素</param>
        /// <typeparam name="T">要素の型</typeparam>
        public delegate void ReplaceAction<in T>(int index, IEnumerable<T> oldItems, IEnumerable<T> newItems);

        /// <summary>
        /// Addイベント
        /// </summary>
        /// <param name="index">追加するインデックス</param>
        /// <param name="items">追加要素</param>
        /// <typeparam name="T">要素の型</typeparam>
        public delegate void AddAction<in T>(int index, IEnumerable<T> items);

        /// <summary>
        /// Moveイベント
        /// </summary>
        /// <param name="oldIndex">移動前要素の先頭インデックス</param>
        /// <param name="newIndex">移動後のインデックス</param>
        /// <param name="items">移動する要素</param>
        /// <typeparam name="T">要素の型</typeparam>
        public delegate void MoveAction<in T>(int oldIndex, int newIndex, IEnumerable<T> items);

        /// <summary>
        /// Removeイベント
        /// </summary>
        /// <param name="index">除去する要素の先頭インデックス</param>
        /// <param name="items">除去要素</param>
        /// <typeparam name="T">要素の型</typeparam>
        public delegate void RemoveAction<in T>(int index, IEnumerable<T> items);

        /// <summary>
        /// Clearイベント
        /// </summary>
        public delegate void ResetAction();
    }
}
