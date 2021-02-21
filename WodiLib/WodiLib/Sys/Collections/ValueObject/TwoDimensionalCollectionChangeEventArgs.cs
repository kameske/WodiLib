// ========================================
// Project Name : WodiLib
// File Name    : TwoDimensionalCollectionChangeEventArgs.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace WodiLib.Sys
{
    /// <summary>
    ///     TwoDimensionalCollection の要素が変更された際に発火するイベントの引数
    /// </summary>
    /// <remarks>
    ///     <see cref="CollectionChangeEventArgs"/> と同様の使い方ができる、2次元リストの変更通知引数。
    /// </remarks>
    /// <typeparam name="T">イベント発火基二次元リストの内包型</typeparam>
    public partial class TwoDimensionalCollectionChangeEventArgs<T> : EventArgs
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     変更前要素開始行
        /// </summary>
        public int OldStartRow { get; } = -1;

        /// <summary>
        ///     変更後要素開始行
        /// </summary>
        public int NewStartRow { get; } = -1;

        /// <summary>
        ///     変更前要素開始列
        /// </summary>
        public int OldStartColumn { get; } = -1;

        /// <summary>
        ///     変更後要素開始列
        /// </summary>
        public int NewStartColumn { get; } = -1;

        /// <summary>
        ///     変更前要素
        /// </summary>
        public IReadOnlyList<IReadOnlyList<T>>? OldItems { get; }

        /// <summary>
        ///     変更後要素
        /// </summary>
        public IReadOnlyList<IReadOnlyList<T>>? NewItems { get; }

        /// <summary>
        ///     変更アクション
        /// </summary>
        public TwoDimensionalCollectionChangeAction Action { get; }

        /// <summary>
        ///     操作方向
        /// </summary>
        /// <remarks>
        ///     Add, Move, Remove アクション時。"行" または "列" のどちらに対して操作されたかを表す。<br/>
        ///     この値によって OldItems, NewItems の順序が決定される。<br/>
        ///     - <see cref="Direction"/> == Row の場合 "外側のリストが行、内側のリストが列" となる。<br/>
        ///     - <see cref="Direction"/> == Column の場合 "外側のリストが列、内側のリストが行" となる。
        /// </remarks>
        public Direction Direction { get; } = Direction.None;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     コンストラクタ（Add, Remove イベント専用）
        /// </summary>
        /// <param name="index">開始行番号または開始列番号</param>
        /// <param name="items">通知要素</param>
        /// <param name="action">イベント</param>
        /// <param name="direction">操作方向</param>
        private TwoDimensionalCollectionChangeEventArgs(
            int index, IReadOnlyList<IReadOnlyList<T>> items,
            TwoDimensionalCollectionChangeAction action,
            Direction direction)
        {
            Action = action;
            Direction = direction;

            if (action == TwoDimensionalCollectionChangeAction.Add)
            {
                if (direction == Direction.Row)
                {
                    NewStartRow = index;
                    NewStartColumn = 0;
                }
                else
                {
                    NewStartRow = 0;
                    NewStartColumn = index;
                }

                NewItems = items;
                return;
            }

            // action == TwoDimensionalCollectionChangeAction.Remove
            if (direction == Direction.Row)
            {
                OldStartRow = index;
                OldStartColumn = 0;
            }
            else
            {
                OldStartRow = 0;
                OldStartColumn = index;
            }

            OldItems = items;
        }

        /// <summary>
        ///     コンストラクタ（Move イベント専用）
        /// </summary>
        /// <param name="oldIndex">移動前開始行番号/列番号</param>
        /// <param name="newIndex">移動後開始列番号/列番号</param>
        /// <param name="items">移動要素</param>
        /// <param name="direction">操作方向</param>
        private TwoDimensionalCollectionChangeEventArgs(
            int oldIndex, int newIndex, IReadOnlyList<IReadOnlyList<T>> items,
            Direction direction)
        {
            Action = TwoDimensionalCollectionChangeAction.Move;
            Direction = direction;

            if (direction == Direction.Row)
            {
                OldStartRow = oldIndex;
                OldStartColumn = 0;
                NewStartRow = newIndex;
                NewStartColumn = 0;
            }
            else
            {
                OldStartRow = 0;
                OldStartColumn = oldIndex;
                NewStartRow = 0;
                NewStartColumn = newIndex;
            }

            OldItems = items;
            NewItems = items;
        }

        /// <summary>
        ///     コンストラクタ（Replace イベント専用）
        /// </summary>
        /// <param name="row">更新開始行番号</param>
        /// <param name="column">更新開始列番号</param>
        /// <param name="oldItems">更新前要素</param>
        /// <param name="newItems">更新後要素</param>
        private TwoDimensionalCollectionChangeEventArgs(
            int row, int column, IReadOnlyList<IReadOnlyList<T>> oldItems,
            IReadOnlyList<IReadOnlyList<T>> newItems)
        {
            Action = TwoDimensionalCollectionChangeAction.Replace;

            OldStartRow = NewStartRow = row;
            OldStartColumn = NewStartColumn = column;
            OldItems = oldItems;
            NewItems = newItems;
        }

        /// <summary>
        ///     コンストラクタ（Reset イベント専用）
        /// </summary>
        private TwoDimensionalCollectionChangeEventArgs()
        {
            Action = TwoDimensionalCollectionChangeAction.Reset;
        }
    }
}
