// ========================================
// Project Name : WodiLib
// File Name    : TwoDimensionalListBase.AdjustLengthAgent.NoneEmpty.cs
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
        private partial class AdjustLengthAgent
        {
            /// <summary>
            /// 空リストが関与しない処理の代理クラス
            /// </summary>
            /// <remarks>
            /// すべての処理で、変更前後のリストの状態がどちらも空ではない場合このクラスを使用する。
            /// </remarks>
            private class NoneEmpty : IAdjustLengthAgent
            {
                // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
                //     Private Property
                // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

                /// <summary>処理対象</summary>
                private TwoDimensionalListBase<T> Outer { get; }

                /// <summary>調整行数</summary>
                private int RowLength { get; }

                /// <summary>調整列数</summary>
                private int ColumnLength { get; }

                /// <summary>処理方向</summary>
                private Direction Direction { get; }

                /// <summary>メソッド種別</summary>
                private MethodType MethodType { get; }

                // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
                //     Private Lazy Property
                // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

                private bool? _mightRowCountChangeable;

                /// <summary>行数変化可能性</summary>
                private bool MightRowCountChangeable
                    => _mightRowCountChangeable ??= Direction != Direction.Column;

                private bool? _mightColumnCountChangeable;

                /// <summary>列数変化可能性</summary>
                private bool MightColumnCountChangeable
                    => _mightColumnCountChangeable ??= Direction != Direction.Row;

                private bool? _isRowAdd;

                private bool IsRowAdd => _isRowAdd ??= new Func<bool>(() =>
                {
                    if (Direction == Direction.Column) return false;
                    return MethodType switch
                    {
                        MethodType.None => Outer.RowCount < RowLength,
                        MethodType.IfLong => false,
                        MethodType.IfShort => Outer.RowCount < RowLength,
                        _ => throw new InvalidOperationException()
                    };
                })();

                private bool? _isRowRemove;

                private bool IsRowRemove => _isRowRemove ??= new Func<bool>(() =>
                {
                    if (Direction == Direction.Column) return false;
                    return MethodType switch
                    {
                        MethodType.None => Outer.RowCount > RowLength,
                        MethodType.IfLong => Outer.RowCount > RowLength,
                        MethodType.IfShort => false,
                        _ => throw new InvalidOperationException()
                    };
                })();

                private bool? _isRowCountChange;

                /// <summary>行数変化フラグ</summary>
                private bool IsRowCountChange
                    => _isRowCountChange ??= IsRowAdd || IsRowRemove;

                private bool? _isColumnAdd;

                private bool IsColumnAdd => _isColumnAdd ??= new Func<bool>(() =>
                {
                    if (Direction == Direction.Row) return false;
                    return MethodType switch
                    {
                        MethodType.None => Outer.ColumnCount < ColumnLength,
                        MethodType.IfLong => false,
                        MethodType.IfShort => Outer.ColumnCount < ColumnLength,
                        _ => throw new InvalidOperationException()
                    };
                })();

                private bool? _isColumnRemove;

                private bool IsColumnRemove => _isColumnRemove ??= new Func<bool>(() =>
                {
                    if (Direction == Direction.Row) return false;
                    return MethodType switch
                    {
                        MethodType.None => Outer.ColumnCount > ColumnLength,
                        MethodType.IfLong => Outer.ColumnCount > ColumnLength,
                        MethodType.IfShort => false,
                        _ => throw new InvalidOperationException()
                    };
                })();

                private bool? _isColumnCountChange;

                /// <summary>行数変化フラグ</summary>
                private bool IsColumnCountChange
                    => _isColumnCountChange ??= IsColumnAdd || IsColumnRemove;

                private int? _changedRowCount;

                /// <summary>行要素調整後の行数</summary>
                private int ChangedRowCount
                    => _changedRowCount ??= IsRowCountChange
                        ? RowLength
                        : Outer.RowCount;

                private int? _changedColumnCount;

                /// <summary>行要素調整後の列数</summary>
                private int ChangedColumnCount
                    => _changedColumnCount ??= IsColumnCountChange
                        ? ColumnLength
                        : Outer.ColumnCount;

                private int? _addRowCount;

                /// <summary>行要素を追加する場合の追加行数</summary>
                private int AddRowCount
                    => _addRowCount ??= ChangedRowCount - Outer.RowCount;

                private int? _addColumnCount;

                /// <summary>列要素を追加する場合の追加列数</summary>
                private int AddColumnCount
                    => _addColumnCount ??= ChangedColumnCount - Outer.ColumnCount;

                private int? removeRowCount;

                /// <summary>行要素を除去する場合の除去行数</summary>
                private int RemoveRowCount
                    => removeRowCount ??= Outer.RowCount - ChangedRowCount;

                private int? _removeColumnCount;

                /// <summary>列要素を除去する場合の除去列数</summary>
                private int RemoveColumnCount
                    => _removeColumnCount ??= Outer.ColumnCount - ChangedColumnCount;

                private int? _removedRowCount;

                /// <summary>行除去後の行数</summary>
                private int RemovedRowCount
                    => _removedRowCount ??= IsRowRemove
                        ? RowLength
                        : Outer.RowCount;

                private int? _removedColumnCount;

                /// <summary>行除去後の行数</summary>
                private int RemovedColumnCount
                    => _removedColumnCount ??= IsColumnRemove
                        ? ColumnLength
                        : Outer.ColumnCount;

                private TwoDimensionalCollectionChangeEventArgs<T>[]? _collectionChangeEventArgArray;

                /// <summary>CollectionChangeEvent 引数</summary>
                private TwoDimensionalCollectionChangeEventArgs<T>[] CollectionChangeEventArgArray
                    => _collectionChangeEventArgArray ??=
                        new Func<TwoDimensionalCollectionChangeEventArgs<T>[]>(() =>
                            CreateCollectionChangeEventArgs().ToArray())();

                // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
                //     Constructor
                // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="outer">処理対象</param>
                /// <param name="rowLength">調整行数</param>
                /// <param name="columnLength">調整列数</param>
                /// <param name="direction">調整方向</param>
                /// <param name="methodType">呼び出し元メソッド種別</param>
                public NoneEmpty(TwoDimensionalListBase<T> outer, int rowLength,
                    int columnLength, Direction direction, MethodType methodType)
                {
                    Outer = outer;
                    RowLength = rowLength;
                    ColumnLength = columnLength;
                    Direction = direction;
                    MethodType = methodType;
                }

                // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
                //     Public Method
                // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

                /// <summary>
                /// コンストラクタで与えられた情報をもとに、必要な処理を実施する。
                /// </summary>
                public void Execute()
                {
                    CallCollectionChanging();
                    ExecuteMain();
                    NotifyPropertyChanged();
                    CallCollectionChanged();
                }

                // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
                //     Private Method
                // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

                /// <summary>
                /// プロパティ変更通知を行う
                /// </summary>
                private void NotifyPropertyChanged()
                {
                    if (MightRowCountChangeable)
                    {
                        Outer.NotifyPropertyChanged(nameof(RowCount));
                    }

                    if (MightColumnCountChangeable)
                    {
                        Outer.NotifyPropertyChanged(nameof(ColumnCount));
                    }

                    Outer.NotifyPropertyChanged(ListConstant.IndexerName);
                }

                /// <summary>
                /// 要素変更前通知を行う
                /// </summary>
                private void CallCollectionChanging()
                {
                    CollectionChangeEventArgArray.ForEach(args => Outer.CallCollectionChanging(args));
                }

                /// <summary>
                /// メイン処理を行う
                /// </summary>
                private void ExecuteMain()
                {
                    CollectionChangeEventArgArray.ForEach(args =>
                    {
                        if (args.Direction == Direction.Row)
                        {
                            if (args.Action == TwoDimensionalCollectionChangeAction.Add)
                            {
                                Outer.InsertRow_Core(args.NewStartRow, args.NewItems!.ToTwoDimensionalArray());
                            }
                            else
                            {
                                Outer.RemoveRow_Core(args.OldStartRow, args.OldItems!.Count);
                            }
                        }
                        else
                        {
                            if (args.Action == TwoDimensionalCollectionChangeAction.Add)
                            {
                                Outer.InsertColumn_Core(args.NewStartColumn, args.NewItems!.ToTwoDimensionalArray());
                            }
                            else
                            {
                                Outer.RemoveColumn_Core(args.OldStartColumn, args.OldItems!.Count);
                            }
                        }
                    });
                }

                /// <summary>
                /// 要素変更後通知を行う
                /// </summary>
                private void CallCollectionChanged()
                {
                    CollectionChangeEventArgArray.ForEach(args => Outer.CallCollectionChanged(args));
                }

                #region Argument Create Methods

                /// <summary>
                /// 要素変更通知イベント引数を生成する。
                /// </summary>
                /// <returns>イベント引数の列挙子</returns>
                private IEnumerable<TwoDimensionalCollectionChangeEventArgs<T>> CreateCollectionChangeEventArgs()
                {
                    // 除去要素
                    {
                        if (IsRowRemove)
                        {
                            yield return CreateRemoveRowEventArgs();
                        }

                        if (IsColumnRemove)
                        {
                            yield return CreateRemoveColumnEventArgs();
                        }
                    }

                    // 追加要素
                    {
                        if (IsRowAdd)
                        {
                            yield return CreateAddRowEventArgs();
                        }

                        if (IsColumnAdd)
                        {
                            yield return CreateAddColumnEventArgs();
                        }
                    }
                }

                /// <summary>
                /// 行除去イベント用の引数を生成する。
                /// 行列どちらも除去される場合、重複する領域の要素が含まれる。
                /// </summary>
                /// <returns>イベント引数</returns>
                private TwoDimensionalCollectionChangeEventArgs<T> CreateRemoveRowEventArgs()
                {
                    var removeItems = Outer.Get_Core(RowLength, RemoveRowCount, 0, Outer.ColumnCount)
                        .ToTwoDimensionalArray();
                    return TwoDimensionalCollectionChangeEventArgs<T>.Factory.RemoveRow(RowLength, removeItems);
                }

                /// <summary>
                /// 列除去イベント用の引数を生成する。
                /// 行列どちらも除去される場合、重複する領域の要素は含まれない。
                /// </summary>
                /// <returns>イベント引数</returns>
                private TwoDimensionalCollectionChangeEventArgs<T> CreateRemoveColumnEventArgs()
                {
                    var removeItems = Outer.Get_Core(0, RemovedRowCount, ColumnLength, RemoveColumnCount)
                        .ToTransposedArray();
                    return TwoDimensionalCollectionChangeEventArgs<T>.Factory.RemoveColumn(ColumnLength, removeItems);
                }

                /// <summary>
                /// 行追加イベント用の引数を生成する。
                /// 行列どちらも追加される場合、重複する領域の要素は含まれない。
                /// </summary>
                /// <returns>イベント引数</returns>
                private TwoDimensionalCollectionChangeEventArgs<T> CreateAddRowEventArgs()
                {
                    var items = Outer.MakeDefaultItemArray(Outer.RowCount, AddRowCount,
                        0, RemovedColumnCount);

                    return TwoDimensionalCollectionChangeEventArgs<T>.Factory.AddRow(Outer.RowCount, items);
                }

                /// <summary>
                /// 列追加イベント用の引数を生成する。
                /// 行列どちらも追加される場合、重複する領域の要素が含まれる。
                /// </summary>
                /// <returns>イベント引数</returns>
                private TwoDimensionalCollectionChangeEventArgs<T> CreateAddColumnEventArgs()
                {
                    var items = Outer.MakeDefaultItemArray(0, ChangedRowCount,
                            Outer.ColumnCount, AddColumnCount)
                        .ToTransposedArray();

                    return TwoDimensionalCollectionChangeEventArgs<T>.Factory.AddColumn(Outer.ColumnCount, items);
                }

                #endregion
            }
        }
    }
}
