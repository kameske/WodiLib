// ========================================
// Project Name : WodiLib
// File Name    : TwoDimensionalListBase.OverwriteAgent.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Linq;

namespace WodiLib.Sys
{
    public partial class TwoDimensionalListBase<T>
    {
        /// <summary>
        /// Overwrite メソッドの処理代理クラス
        /// </summary>
        private class OverwriteAgent
        {
            /*
             * 要素を上書きする場合に Set、追加する場合に Insert の処理をそれぞれ実行する。
             * PropertyChanged は "Item[]", "Count" ともに通知するが、
             * CollectionChange は実行するものだけを通知。
             */

            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
            //     Private Property
            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

            /// <summary>処理対象</summary>
            private TwoDimensionalListBase<T> Outer { get; }

            /// <summary>インデックス</summary>
            private int Index { get; }

            /// <summary>上書き要素</summary>
            private T[][] OverwriteItems { get; }

            /// <summary>処理方向</summary>
            private Direction Direction { get; }

            /// <summary>変更前通知アクションリスト</summary>
            private List<Action> CallCollectionChangingActionList { get; } = new List<Action>();

            /// <summary>処理本体リスト</summary>
            private List<Action> CoreActionList { get; } = new List<Action>();

            /// <summary>変更後通知アクションリスト</summary>
            private List<Action> CallCollectionChangedActionList { get; } = new List<Action>();

            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
            //     Private Computed Property
            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

            /// <summary>
            /// 処理基準になる要素数
            /// </summary>
            private int MainCount =>
                Direction == Direction.Row
                    ? Outer.RowCount
                    : Outer.ColumnCount;

            /// <summary>
            /// 更新要素数
            /// </summary>
            private int UpdateCount =>
                Index + OverwriteItems.Length > MainCount
                    ? MainCount - Index
                    : OverwriteItems.Length;

            /// <summary>
            /// 追加要素数
            /// </summary>
            private int AddCount =>
                OverwriteItems.Length - UpdateCount;

            /// <summary>
            /// 行方向処理フラグ<br/>
            /// 列方向処理の場合false
            /// </summary>
            private bool IsRowExecution =>
                Direction == Direction.Row;

            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
            //     Constructor
            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

            public OverwriteAgent(TwoDimensionalListBase<T> outer, int index, T[][] overwriteItems, Direction direction)
            {
                Outer = outer;
                Index = index;
                OverwriteItems = overwriteItems;
                Direction = direction;
            }

            public static OverwriteAgent ForOverwriteRow(TwoDimensionalListBase<T> outer, int row, T[][] items)
            {
                return new OverwriteAgent(outer, row, items, Direction.Row);
            }

            public static OverwriteAgent ForOverwriteColumn(TwoDimensionalListBase<T> outer, int column, T[][] items)
            {
                return new OverwriteAgent(outer, column, items, Direction.Column);
            }

            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
            //     Public Method
            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

            /// <summary>
            /// コンストラクタで与えられた情報をもとに、必要な処理を実施する。
            /// </summary>
            public void Execute()
            {
                PrepareExecuteMain();
                ExecuteMain();
            }

            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
            //     Private Method
            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

            /// <summary>
            /// 処理実行の準備を行う。
            /// </summary>
            private void PrepareExecuteMain()
            {
                PrepareExecuteMain_ReplaceItem();
                PrepareExecuteMain_AddItems();
            }

            /// <summary>
            /// 上書き処理実行の準備を行う。
            /// </summary>
            private void PrepareExecuteMain_ReplaceItem()
            {
                if (UpdateCount == 0) return;

                var replaceOldItems = (IsRowExecution
                        ? Outer.GetRow_Core(Index, UpdateCount)
                        : Outer.GetColumn_Core(Index, UpdateCount))
                    .ToTwoDimensionalArray();
                var replaceItems = OverwriteItems.Take(UpdateCount).ToArray();

                var eventArgs = IsRowExecution
                    ? TwoDimensionalCollectionChangeEventArgs<T>.Factory.Set(Index, 0, replaceOldItems, replaceItems)
                    : TwoDimensionalCollectionChangeEventArgs<T>.Factory.Set(0, Index, replaceOldItems, replaceItems);

                CallCollectionChangingActionList.Add(
                    () => Outer.CallCollectionChanging(eventArgs)
                );

                if (IsRowExecution)
                {
                    CoreActionList.Add(() => Outer.Set_Core(Index, 0, replaceItems));
                }
                else
                {
                    CoreActionList.Add(() => Outer.Set_Core(0, Index, replaceItems.ToTransposedArray()));
                }

                CallCollectionChangedActionList.Add(
                    () => Outer.CallCollectionChanged(eventArgs)
                );
            }

            /// <summary>
            /// 追加処理実行の準備を行う。
            /// </summary>
            private void PrepareExecuteMain_AddItems()
            {
                if (AddCount == 0) return;

                var insertItems = OverwriteItems.Skip(UpdateCount).ToArray();

                var eventArgs = IsRowExecution
                    ? TwoDimensionalCollectionChangeEventArgs<T>.Factory.AddRow(Outer.RowCount, insertItems)
                    : TwoDimensionalCollectionChangeEventArgs<T>.Factory.AddColumn(Outer.ColumnCount, insertItems);

                CallCollectionChangingActionList.Add(
                    () => Outer.CallCollectionChanging(eventArgs)
                );
                if (IsRowExecution)
                {
                    CoreActionList.Add(() => Outer.InsertRow_Core(Outer.RowCount, insertItems));
                }
                else
                {
                    CoreActionList.Add(() => Outer.InsertColumn_Core(Outer.ColumnCount, insertItems));
                }

                CallCollectionChangedActionList.Add(
                    () => Outer.CallCollectionChanged(eventArgs)
                );
            }

            /// <summary>
            /// 処理実行本体
            /// </summary>
            private void ExecuteMain()
            {
                CallCollectionChangingActionList.ForEach(action => action());

                CoreActionList.ForEach(action => action());

                Outer.NotifyPropertyChanged(IsRowExecution ? nameof(RowCount) : nameof(ColumnCount));
                Outer.NotifyPropertyChanged(ListConstant.IndexerName);

                CallCollectionChangedActionList.ForEach(action => action());
            }
        }
    }
}
