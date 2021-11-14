// ========================================
// Project Name : WodiLib
// File Name    : NotifyCollectionChangedEventArgsEx.cs
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
    /// <summary>
    ///     <see cref="NotifyCollectionChangedEventArgs"/> カスタマイズクラス
    /// </summary>
    /// <remarks>
    ///     <see cref="NotifyCollectionChangedEventArgs"/> と比較して以下のような違いがある。
    ///     <ul>
    ///         <li><see cref="NewItems"/>, <see cref="OldItems"/> の要素型が指定されている。</li>
    ///         <li>
    ///             <see cref="Action"/> が <see cref="NotifyCollectionChangedAction.Reset"/> の場合に
    ///             <see cref="NewItems"/>, <see cref="OldItems"/> いずれも指定されている。
    ///         </li>
    ///     </ul>
    /// </remarks>
    public class NotifyCollectionChangedEventArgsEx<T> : NotifyCollectionChangedEventArgs
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Static Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     Set イベントの引数を生成する。
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <param name="oldItem">置換前要素</param>
        /// <param name="newItem">置換後要素</param>
        /// <returns>イベント引数インスタンス</returns>
        public static NotifyCollectionChangedEventArgsEx<T> CreateSetArgs(int index, T oldItem, T newItem)
            => new(oldItem, newItem, index);

        /// <summary>
        ///     SetRange イベントの引数を生成する。
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <param name="oldItems">置換前要素</param>
        /// <param name="newItems">置換後要素</param>
        /// <returns>イベント引数インスタンス</returns>
        public static NotifyCollectionChangedEventArgsEx<T> CreateSetArgs(int index, IReadOnlyList<T> oldItems,
            IReadOnlyList<T> newItems)
            => new(oldItems, newItems, index);

        /// <summary>
        ///     Add, Insert イベントの引数を生成する。
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <param name="item">追加要素</param>
        /// <returns>イベント引数インスタンス</returns>
        public static NotifyCollectionChangedEventArgsEx<T> CreateInsertArgs(int index, T item)
            => new(NotifyCollectionChangedAction.Add, item, index);

        /// <summary>
        ///     AddRange, InsertRange イベントの引数を生成する。
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <param name="items">追加要素</param>
        /// <returns>イベント引数インスタンス</returns>
        public static NotifyCollectionChangedEventArgsEx<T> CreateInsertArgs(int index, IReadOnlyList<T> items)
            => new(NotifyCollectionChangedAction.Add, items, index);

        /// <summary>
        ///     Move イベントの引数を生成する。
        /// </summary>
        /// <param name="oldIndex">移動前インデックス</param>
        /// <param name="newIndex">移動後インデックス</param>
        /// <param name="item">移動要素</param>
        /// <returns></returns>
        public static NotifyCollectionChangedEventArgsEx<T> CreateMoveArgs(int oldIndex, int newIndex, T item)
            => new(item, newIndex, oldIndex);

        /// <summary>
        ///     MoveRange イベントの引数を生成する。
        /// </summary>
        /// <param name="oldIndex">移動前インデックス</param>
        /// <param name="newIndex">移動後インデックス</param>
        /// <param name="items">移動要素</param>
        /// <returns></returns>
        public static NotifyCollectionChangedEventArgsEx<T> CreateMoveArgs(int oldIndex, int newIndex,
            IReadOnlyList<T> items)
            => new(items, newIndex, oldIndex);

        /// <summary>
        ///     Remove イベントの引数を生成する。
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <param name="item">除去要素</param>
        /// <returns>イベント引数インスタンス</returns>
        public static NotifyCollectionChangedEventArgsEx<T> CreateRemoveArgs(int index, T item)
            => new(NotifyCollectionChangedAction.Remove, item, index);

        /// <summary>
        ///     RemoveRange イベントの引数を生成する。
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <param name="items">除去要素</param>
        /// <returns>イベント引数インスタンス</returns>
        public static NotifyCollectionChangedEventArgsEx<T> CreateRemoveArgs(int index, IReadOnlyList<T> items)
            => new(NotifyCollectionChangedAction.Remove, items, index);

        /// <summary>
        ///     Reset, Clear イベントの引数を生成する。
        /// </summary>
        /// <param name="index">初期化開始インデックス</param>
        /// <param name="oldItems">初期化前要素</param>
        /// <param name="newItems">初期化後要素</param>
        /// <returns>イベント引数インスタンス</returns>
        public static NotifyCollectionChangedEventArgsEx<T> CreateResetArgs(int index, IEnumerable<T> oldItems,
            IEnumerable<T> newItems)
            => new(index, oldItems, newItems);

        /// <summary>
        /// <see cref="NewItems"/>, <see cref="OldItems"/> を別の型にキャストした新たな
        /// <see cref="NotifyCollectionChangedEventArgsEx{T}"/> インスタンスを生成する。
        /// </summary>
        /// <param name="other">キャスト元</param>
        /// <param name="caster">キャスト処理</param>
        /// <typeparam name="TOther">キャスト元型</typeparam>
        /// <returns>キャストした新たなインスタンス</returns>
        internal static NotifyCollectionChangedEventArgsEx<T> CreateFromOtherType<TOther>(
            NotifyCollectionChangedEventArgsEx<TOther> other, Func<TOther, T> caster)
        {
            switch (other.Action)
            {
                case NotifyCollectionChangedAction.Replace:
                    return CreateSetArgs(
                        other.NewStartingIndex,
                        other.OldItems!.Select(caster).ToList(),
                        other.NewItems!.Select(caster).ToList()
                    );

                case NotifyCollectionChangedAction.Add:
                    return CreateInsertArgs(
                        other.NewStartingIndex,
                        other.NewItems!.Select(caster).ToList()
                    );

                case NotifyCollectionChangedAction.Move:
                    return CreateMoveArgs(
                        other.OldStartingIndex,
                        other.NewStartingIndex,
                        other.NewItems!.Select(caster).ToList()
                    );

                case NotifyCollectionChangedAction.Remove:
                    return CreateRemoveArgs(
                        other.OldStartingIndex,
                        other.OldItems!.Select(caster).ToList()
                    );

                case NotifyCollectionChangedAction.Reset:
                    return CreateResetArgs(
                        other.NewStartingIndex,
                        other.OldItems!.Select(caster).ToList(),
                        other.NewItems!.Select(caster).ToList()
                    );

                default:
                    // 通常ここへは来ない
                    throw new InvalidOperationException();
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc cref="NotifyCollectionChangedEventArgs.NewItems"/>
        public new IReadOnlyExtendedList<T>? NewItems { get; }

        /// <inheritdoc cref="NotifyCollectionChangedEventArgs.OldItems"/>
        public new IReadOnlyExtendedList<T>? OldItems { get; }

        /// <inheritdoc cref="NotifyCollectionChangedEventArgs.NewStartingIndex"/>
        public new int NewStartingIndex { get; }

        /// <inheritdoc cref="NotifyCollectionChangedEventArgs.OldStartingIndex"/>
        public new int OldStartingIndex { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     Replace 用コンストラクタ（単一要素）
        /// </summary>
        /// <param name="oldItem">置換前要素</param>
        /// <param name="newItem">置換後要素</param>
        /// <param name="index">インデックス</param>
        private NotifyCollectionChangedEventArgsEx(T oldItem, T newItem, int index) : base(
            NotifyCollectionChangedAction.Replace, newItem, oldItem, index)
        {
            NewItems = new ExtendedList<T>(new[] { newItem });
            OldItems = new ExtendedList<T>(new[] { oldItem });
            NewStartingIndex = base.NewStartingIndex;
            OldStartingIndex = base.OldStartingIndex;
        }

        /// <summary>
        ///     Replace 用コンストラクタ（複数要素）
        /// </summary>
        /// <param name="oldItems">置換前要素</param>
        /// <param name="newItems">置換後要素</param>
        /// <param name="index">インデックス</param>
        private NotifyCollectionChangedEventArgsEx(IReadOnlyList<T> oldItems, IReadOnlyList<T> newItems, int index) :
            base(
                NotifyCollectionChangedAction.Replace, new List<T>(newItems), new List<T>(oldItems), index)
        {
            NewItems = new ExtendedList<T>(newItems);
            OldItems = new ExtendedList<T>(oldItems);
            NewStartingIndex = base.NewStartingIndex;
            OldStartingIndex = base.OldStartingIndex;
        }

        /// <summary>
        ///     Add, Remove 用 コンストラクタ（単一要素）。
        /// </summary>
        /// <param name="action">アクション</param>
        /// <param name="item">操作要素</param>
        /// <param name="index">操作インデックス</param>
        private NotifyCollectionChangedEventArgsEx(NotifyCollectionChangedAction action, T item, int index) : base(
            action, item, index)
        {
            switch (action)
            {
                case NotifyCollectionChangedAction.Add:
                    NewItems = new ExtendedList<T>(new[] { item });
                    break;

                case NotifyCollectionChangedAction.Remove:
                    OldItems = new ExtendedList<T>(new[] { item });
                    break;

                default:
                    throw new ArgumentException();
            }

            NewStartingIndex = base.NewStartingIndex;
            OldStartingIndex = base.OldStartingIndex;
        }

        /// <summary>
        ///     Add, Remove 用 コンストラクタ（複数要素）。
        /// </summary>
        /// <param name="action">アクション</param>
        /// <param name="items">操作要素</param>
        /// <param name="index">操作インデックス</param>
        private NotifyCollectionChangedEventArgsEx(NotifyCollectionChangedAction action, IReadOnlyList<T> items,
            int index) : base(action, new List<T>(items), index)
        {
            switch (action)
            {
                case NotifyCollectionChangedAction.Add:
                    NewItems = new ExtendedList<T>(items);
                    break;

                case NotifyCollectionChangedAction.Remove:
                    OldItems = new ExtendedList<T>(items);
                    break;

                default:
                    throw new ArgumentException();
            }

            NewStartingIndex = base.NewStartingIndex;
            OldStartingIndex = base.OldStartingIndex;
        }

        /// <summary>
        ///     Move 用コンストラクタ（単一要素）。
        /// </summary>
        /// <param name="item">操作要素</param>
        /// <param name="newIndex">移動後インデックス</param>
        /// <param name="oldIndex">移動前インデックス</param>
        private NotifyCollectionChangedEventArgsEx(T item, int newIndex, int oldIndex) : base(
            NotifyCollectionChangedAction.Move, item, newIndex, oldIndex)
        {
            NewItems = new ExtendedList<T>(new[] { item });
            OldItems = new ExtendedList<T>(new[] { item });
            NewStartingIndex = base.NewStartingIndex;
            OldStartingIndex = base.OldStartingIndex;
        }

        /// <summary>
        ///     Move 用コンストラクタ（複数要素）。
        /// </summary>
        /// <param name="items">操作要素</param>
        /// <param name="newIndex">移動後インデックス</param>
        /// <param name="oldIndex">移動前インデックス</param>
        private NotifyCollectionChangedEventArgsEx(IReadOnlyList<T> items, int newIndex, int oldIndex) : base(
            NotifyCollectionChangedAction.Move, new List<T>(items), newIndex, oldIndex)
        {
            NewItems = new ExtendedList<T>(items);
            OldItems = new ExtendedList<T>(items);
            NewStartingIndex = base.NewStartingIndex;
            OldStartingIndex = base.OldStartingIndex;
        }

        /// <summary>
        ///     Reset 用コンストラクタ
        /// </summary>
        /// <param name="index">リセット開始インデックス</param>
        /// <param name="oldItems">リセット前要素</param>
        /// <param name="newItems">リセット後要素</param>
        private NotifyCollectionChangedEventArgsEx(int index, IEnumerable<T> oldItems, IEnumerable<T> newItems) :
            base(
                NotifyCollectionChangedAction.Reset)
        {
            NewItems = new ExtendedList<T>(newItems);
            OldItems = new ExtendedList<T>(oldItems);
            NewStartingIndex = index;
            OldStartingIndex = index;
        }
    }
}
