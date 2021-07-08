// ========================================
// Project Name : WodiLib
// File Name    : TwoDimensionalCollectionChangeEventInternalArgs.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     <see cref="TwoDimensionalList{T}"/> の要素が変更された際に発火するイベントの引数
    /// </summary>
    /// <remarks>
    ///     <p>
    ///         <seealso cref="NotifyCollectionChangedEventArgsEx{T}"/> と同様の使い方ができる、2次元リストの変更通知引数。
    ///     </p>
    ///     <p>
    ///         WodiLib外部に公開する場合はこれをラップした引数クラスを別途定義する。
    ///         <see cref="ITwoDimensionalList{T}"/> 同様WodiLib外部に見せるプロパティ名を
    ///         用途に合わせて書き換えたいため。
    ///     </p>
    /// </remarks>
    /// <typeparam name="T">イベント発火基二次元リストの内包型</typeparam>
    internal class TwoDimensionalCollectionChangeEventInternalArgs<T> : EventArgs
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Static Methods
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     Set イベントの引数（単一要素）を生成する。
        /// </summary>
        /// <param name="row">行番号</param>
        /// <param name="column">列番号</param>
        /// <param name="oldItem">置換前要素</param>
        /// <param name="newItem">置換後要素</param>
        /// <returns></returns>
        public static TwoDimensionalCollectionChangeEventInternalArgs<T> CreateSetArgs(int row, int column,
            T oldItem, T newItem)
            => new(oldItem, newItem, row, column);

        /// <summary>
        ///     SetRange イベントの引数（単一行/列）を生成する。
        /// </summary>
        /// <param name="row">行番号</param>
        /// <param name="column">列番号</param>
        /// <param name="oldItems">置換前要素</param>
        /// <param name="newItems">置換後要素</param>
        /// <param name="direction">操作方向</param>
        /// <returns>イベント引数インスタンス</returns>
        public static TwoDimensionalCollectionChangeEventInternalArgs<T> CreateSetArgs(int row, int column,
            T[] oldItems, T[] newItems, Direction direction)
            => new(TwoDimensionalCollectionChangeAction.Replace, new[] {oldItems},
                new[] {newItems}, row, column, direction);

        /// <summary>
        ///     SetRange （行方向操作）イベントの引数（複数行/列）を生成する。
        /// </summary>
        /// <param name="row">行番号</param>
        /// <param name="column">列番号</param>
        /// <param name="oldItems">置換前要素</param>
        /// <param name="newItems">置換後要素</param>
        /// <param name="direction">操作方向</param>
        /// <returns>イベント引数インスタンス</returns>
        public static TwoDimensionalCollectionChangeEventInternalArgs<T> CreateSetArgs(int row, int column,
            T[][] oldItems, T[][] newItems, Direction direction)
            => new(TwoDimensionalCollectionChangeAction.Replace, oldItems, newItems, row, column, direction);

        /// <summary>
        ///     Add, Insert イベントの引数（単一要素）を生成する。
        /// </summary>
        /// <param name="row">行番号</param>
        /// <param name="column">列番号</param>
        /// <param name="item">追加要素</param>
        /// <returns>イベント引数インスタンス</returns>
        public static TwoDimensionalCollectionChangeEventInternalArgs<T> CreateAddArgs(int row,
            int column, T item)
            => new(TwoDimensionalCollectionChangeAction.Add, item, row, column);

        /// <summary>
        ///     Add, Insert イベントの引数（単一行/列）を生成する。
        /// </summary>
        /// <param name="index">行/列番号</param>
        /// <param name="items">追加要素</param>
        /// <param name="direction">操作方向</param>
        /// <returns>イベント引数インスタンス</returns>
        public static TwoDimensionalCollectionChangeEventInternalArgs<T> CreateAddArgs(
            int index, T[] items, Direction direction)
            => direction == Direction.Row
                ? new TwoDimensionalCollectionChangeEventInternalArgs<T>(TwoDimensionalCollectionChangeAction.Add,
                    new[] {items}, index, 0, direction)
                : new TwoDimensionalCollectionChangeEventInternalArgs<T>(TwoDimensionalCollectionChangeAction.Add,
                    new[] {items}, 0, index, direction);

        /// <summary>
        ///     Add, Insert イベントの引数（単一行/列）を生成する。
        /// </summary>
        /// <param name="row">行番号</param>
        /// <param name="column">列番号</param>
        /// <param name="items">追加要素</param>
        /// <param name="direction">操作方向</param>
        /// <returns>イベント引数インスタンス</returns>
        public static TwoDimensionalCollectionChangeEventInternalArgs<T> CreateAddArgs(
            int row, int column, T[] items, Direction direction)
            => new(TwoDimensionalCollectionChangeAction.Add,
                new[] {items}, row, column, direction);

        /// <summary>
        ///     Add, Insert イベントの引数（複数行/列）を生成する。
        /// </summary>
        /// <param name="index">行/列番号</param>
        /// <param name="items">追加要素</param>
        /// <param name="direction">操作方向</param>
        /// <returns>イベント引数インスタンス</returns>
        public static TwoDimensionalCollectionChangeEventInternalArgs<T> CreateAddArgs(
            int index, T[][] items, Direction direction)
            => direction == Direction.Row
                ? new TwoDimensionalCollectionChangeEventInternalArgs<T>(TwoDimensionalCollectionChangeAction.Add,
                    items, index, 0, direction)
                : new TwoDimensionalCollectionChangeEventInternalArgs<T>(TwoDimensionalCollectionChangeAction.Add,
                    items, 0, index, direction);

        /// <summary>
        ///     Move イベントの引数（単一要素）を生成する。
        /// </summary>
        /// <param name="oldRow">移動前行番号</param>
        /// <param name="oldColumn">移動前列番号</param>
        /// <param name="newRow">移動後行番号</param>
        /// <param name="newColumn">移動後列番号</param>
        /// <param name="item">移動要素</param>
        /// <returns>イベント引数インスタンス</returns>
        public static TwoDimensionalCollectionChangeEventInternalArgs<T> CreateMoveArgs(int oldRow, int oldColumn,
            int newRow, int newColumn, T item)
            => new(item, oldRow, oldColumn, newRow, newColumn);

        /// <summary>
        ///     Move イベント（単一行/列）の引数を生成する。
        /// </summary>
        /// <param name="oldIndex">移動前行/列番号</param>
        /// <param name="newIndex">移動後行/列番号</param>
        /// <param name="items">移動要素</param>
        /// <param name="direction">操作方向</param>
        /// <returns>イベント引数インスタンス</returns>
        public static TwoDimensionalCollectionChangeEventInternalArgs<T> CreateMoveArgs(int oldIndex,
            int newIndex, T[] items, Direction direction)
            => direction == Direction.Row
                ? new TwoDimensionalCollectionChangeEventInternalArgs<T>(
                    new[] {items}, oldIndex, 0, newIndex, 0, direction)
                : new TwoDimensionalCollectionChangeEventInternalArgs<T>(
                    new[] {items}, 0, oldIndex, 0, newIndex, direction);

        /// <summary>
        ///     Move イベント（複数行/列）の引数を生成する。
        /// </summary>
        /// <param name="oldIndex">移動前行/列番号</param>
        /// <param name="newIndex">移動後行/列番号</param>
        /// <param name="items">移動要素</param>
        /// <param name="direction">操作方向</param>
        /// <returns>イベント引数インスタンス</returns>
        public static TwoDimensionalCollectionChangeEventInternalArgs<T> CreateMoveArgs(int oldIndex,
            int newIndex, T[][] items, Direction direction)
            => direction == Direction.Row
                ? new TwoDimensionalCollectionChangeEventInternalArgs<T>(items, oldIndex, 0, newIndex, 0, direction)
                : new TwoDimensionalCollectionChangeEventInternalArgs<T>(items, 0, oldIndex, 0, newIndex, direction);

        /// <summary>
        ///     Remove イベントの引数（単一要素）を生成する。
        /// </summary>
        /// <param name="row">行番号</param>
        /// <param name="column">列番号</param>
        /// <param name="item">除去要素</param>
        /// <returns>イベント引数インスタンス</returns>
        public static TwoDimensionalCollectionChangeEventInternalArgs<T> CreateRemoveArgs(int row, int column, T item)
            => new(TwoDimensionalCollectionChangeAction.Remove, item, row, column);

        /// <summary>
        ///     Remove イベントの引数（単一行/列）を生成する。
        /// </summary>
        /// <param name="index">行/列番号</param>
        /// <param name="items">除去要素</param>
        /// <param name="direction">操作方向</param>
        /// <returns>イベント引数インスタンス</returns>
        public static TwoDimensionalCollectionChangeEventInternalArgs<T> CreateRemoveArgs(
            int index, T[] items, Direction direction)
            => direction == Direction.Row
                ? new TwoDimensionalCollectionChangeEventInternalArgs<T>(
                    TwoDimensionalCollectionChangeAction.Remove, new[] {items}, index, 0, direction)
                : new TwoDimensionalCollectionChangeEventInternalArgs<T>(
                    TwoDimensionalCollectionChangeAction.Remove, new[] {items}, 0, index, direction);

        /// <summary>
        ///     Remove イベントの引数（複数行/列）を生成する。
        /// </summary>
        /// <param name="index">行/列番号</param>
        /// <param name="items">除去要素</param>
        /// <param name="direction">操作方向</param>
        /// <returns>イベント引数インスタンス</returns>
        public static TwoDimensionalCollectionChangeEventInternalArgs<T> CreateRemoveArgs(
            int index, T[][] items, Direction direction)
            => direction == Direction.Row
                ? new TwoDimensionalCollectionChangeEventInternalArgs<T>(
                    TwoDimensionalCollectionChangeAction.Remove, items, index, 0, direction)
                : new TwoDimensionalCollectionChangeEventInternalArgs<T>(
                    TwoDimensionalCollectionChangeAction.Remove, items, 0, index, direction);

        /// <summary>
        ///     Reset （行方向操作）イベントの引数を生成する。
        /// </summary>
        /// <param name="oldItems">初期化前要素</param>
        /// <param name="newItems">初期化後要素</param>
        /// <param name="row">初期化開始行番号</param>
        /// <param name="column">初期化開始列番号</param>
        /// <param name="direction">操作方向</param>
        /// <returns>イベント引数インスタンス</returns>
        public static TwoDimensionalCollectionChangeEventInternalArgs<T> CreateResetArgs(T[][] oldItems, T[][] newItems,
            int row, int column, Direction direction)
            => new(oldItems, newItems, row, column, direction);

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Properties
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
        ///     この値によって <see cref="OldItems"/>, <see cref="NewItems"/> の順序が決定される。<br/>
        ///     - <see cref="Direction"/> == <see cref="WodiLib.Sys.Collections.Direction.Row"/> の場合
        ///     "外側のリストが行、内側のリストが列" となる。<br/>
        ///     - <see cref="Direction"/> == <see cref="WodiLib.Sys.Collections.Direction.Column"/> の場合
        ///     "外側のリストが列、内側のリストが行" となる。<br/>
        ///     - <see cref="Direction"/> == <see cref="WodiLib.Sys.Collections.Direction.None"/> の場合
        ///     <see cref="OldItems"/>, <see cref="NewItems"/> は 1行1列となる。その逆は必ずしも成立しない。
        /// </remarks>
        public Direction Direction { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     Replace 用コンストラクタ（単一要素）
        /// </summary>
        /// <param name="oldItem">置換前要素</param>
        /// <param name="newItem">置換後要素</param>
        /// <param name="row">行番号</param>
        /// <param name="column">列番号</param>
        private TwoDimensionalCollectionChangeEventInternalArgs(T oldItem, T newItem, int row, int column)
            : this(TwoDimensionalCollectionChangeAction.Replace, new[] {new[] {oldItem}},
                new[] {new[] {newItem}}, row, column, Direction.None)
        {
        }

        /// <summary>
        ///     Replace, Reset 用コンストラクタ（複数要素）
        /// </summary>
        /// <param name="action">操作種別</param>
        /// <param name="oldItems">置換前要素</param>
        /// <param name="newItems">置換後要素</param>
        /// <param name="row">開始行番号</param>
        /// <param name="column">開始列番号</param>
        /// <param name="direction">操作方向</param>
        private TwoDimensionalCollectionChangeEventInternalArgs(TwoDimensionalCollectionChangeAction action,
            IReadOnlyList<IReadOnlyList<T>> oldItems, IReadOnlyList<IReadOnlyList<T>> newItems,
            int row, int column, Direction direction)
        {
            Action = action;
            Direction = direction;
            OldItems = oldItems;
            NewItems = newItems;
            OldStartRow = row;
            OldStartColumn = column;
            NewStartRow = row;
            NewStartColumn = column;
        }

        /// <summary>
        ///     Add, Remove 用コンストラクタ（単一要素）
        /// </summary>
        /// <param name="action">操作種別</param>
        /// <param name="item">追加/削除要素</param>
        /// <param name="row">追加/削除開始行番号</param>
        /// <param name="column">追加/削除開始列番号</param>
        private TwoDimensionalCollectionChangeEventInternalArgs(TwoDimensionalCollectionChangeAction action,
            T item, int row, int column)
            : this(action, new[] {new[] {item}}, row, column, Direction.None)
        {
        }

        /// <summary>
        ///     Add, Remove 用コンストラクタ（複数要素）
        /// </summary>
        /// <param name="action">操作種別</param>
        /// <param name="items">追加/削除要素</param>
        /// <param name="row">追加/削除開始行番号</param>
        /// <param name="column">追加/削除開始列番号</param>
        /// <param name="direction">操作方向</param>
        private TwoDimensionalCollectionChangeEventInternalArgs(TwoDimensionalCollectionChangeAction action,
            IReadOnlyList<IReadOnlyList<T>> items, int row, int column, Direction direction)
        {
            Action = action;
            Direction = direction;
            if (action == TwoDimensionalCollectionChangeAction.Add)
            {
                NewItems = items;
                NewStartRow = row;
                NewStartColumn = column;
            }
            else // action == TwoDimensionalCollectionChangeAction.Remove
            {
                OldItems = items;
                OldStartRow = row;
                OldStartColumn = column;
            }
        }

        /// <summary>
        ///     Move 用コンストラクタ（単一要素）
        /// </summary>
        /// <param name="item">移動要素</param>
        /// <param name="oldRow">移動前開始行番号</param>
        /// <param name="oldColumn">移動前開始列番号</param>
        /// <param name="newRow">移動後開始行番号</param>
        /// <param name="newColumn">移動後開始列番号</param>
        private TwoDimensionalCollectionChangeEventInternalArgs(T item,
            int oldRow, int oldColumn, int newRow, int newColumn)
            : this(new[] {new[] {item}}, oldRow, oldColumn, newRow, newColumn, Direction.None)
        {
        }

        /// <summary>
        ///     Move 用コンストラクタ（複数要素）
        /// </summary>
        /// <param name="items">移動要素</param>
        /// <param name="oldRow">移動前開始行番号</param>
        /// <param name="oldColumn">移動前開始列番号</param>
        /// <param name="newRow">移動後開始行番号</param>
        /// <param name="newColumn">移動後開始列番号</param>
        /// <param name="direction">操作方向</param>
        private TwoDimensionalCollectionChangeEventInternalArgs(IReadOnlyList<IReadOnlyList<T>> items,
            int oldRow, int oldColumn, int newRow, int newColumn, Direction direction)
        {
            Action = TwoDimensionalCollectionChangeAction.Move;
            Direction = direction;
            OldItems = items;
            NewItems = items;
            OldStartRow = oldRow;
            OldStartColumn = oldColumn;
            NewStartRow = newRow;
            NewStartColumn = newColumn;
        }

        /// <summary>
        ///     Reset 用コンストラクタ
        /// </summary>
        /// <param name="oldItems">初期化前要素</param>
        /// <param name="newItems">初期化後要素</param>
        /// <param name="row">初期化開始行番号</param>
        /// <param name="column">初期化開始列番号</param>
        /// <param name="direction">操作方向</param>
        private TwoDimensionalCollectionChangeEventInternalArgs(IReadOnlyList<IReadOnlyList<T>> oldItems,
            IReadOnlyList<IReadOnlyList<T>> newItems, int row, int column, Direction direction)
        {
            Action = TwoDimensionalCollectionChangeAction.Reset;
            Direction = direction;
            OldItems = oldItems;
            NewItems = newItems;
            OldStartRow = row;
            OldStartColumn = column;
            NewStartRow = row;
            NewStartColumn = column;
        }
    }
}
