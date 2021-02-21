// ========================================
// Project Name : WodiLib
// File Name    : TwoDimensionalListBase.AdjustLengthAgent.ToEmpty.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;

namespace WodiLib.Sys
{
    public partial class TwoDimensionalListBase<T>
    {
        private partial class AdjustLengthAgent
        {
            /// <summary>
            ///     空リストに変化させる処理の代理クラス
            /// </summary>
            /// <remarks>
            ///     すべての処理で変更後の状態が空リストの場合に使用する。
            ///     変更前の状態を問わない。
            /// </remarks>
            private class ToEmpty : IAdjustLengthAgent
            {
                // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
                //     Private Property
                // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

                /// <summary>処理対象</summary>
                private TwoDimensionalListBase<T> Outer { get; }

                /// <summary>処理方向</summary>
                private Direction Direction { get; }

                /// <summary>呼び出し元メソッド種別</summary>
                private MethodType MethodType { get; }

                // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
                //     Private Lazy Property
                // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

                private bool? _isEmptyFrom;

                /// <summary>空リストから変化フラグ</summary>
                private bool IsEmptyFrom
                    => _isEmptyFrom ??= Outer.RowCount == 0;

                private bool? _isCountChange;

                private bool IsCountChange
                    => _isCountChange ??= MethodType switch
                    {
                        MethodType.None => !IsEmptyFrom,
                        MethodType.IfLong => !IsEmptyFrom,
                        MethodType.IfShort => false, // "要素0より短い" は常に偽
                        _ => throw new InvalidOperationException()
                    };

                private TwoDimensionalCollectionChangeEventArgs<T>? _collectionChangeEventArgs;

                /// <summary>CollectionChangeEvent 引数</summary>
                private TwoDimensionalCollectionChangeEventArgs<T> CollectionChangeEventArgs
                    => _collectionChangeEventArgs ??= CreateCollectionChangeEventArgs();

                // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
                //     Constructor
                // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

                /// <summary>
                ///     コンストラクタ
                /// </summary>
                /// <param name="outer">処理対象</param>
                /// <param name="direction">調整方向</param>
                /// <param name="methodType">メソッド種別</param>
                public ToEmpty(TwoDimensionalListBase<T> outer, Direction direction,
                    MethodType methodType)
                {
                    Outer = outer;
                    Direction = direction;
                    MethodType = methodType;
                }

                // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
                //     Public Method
                // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

                /// <summary>
                ///     コンストラクタで与えられた情報をもとに、必要な処理を実施する。
                /// </summary>
                public void Execute()
                {
                    if (IsEmptyFrom || !IsCountChange)
                    {
                        NotifyPropertyChanged();
                        return;
                    }

                    CallCollectionChanging();
                    ExecuteMain();
                    NotifyPropertyChanged();
                    CallCollectionChanged();
                }

                // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
                //     Private Method
                // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

                /// <summary>
                ///     プロパティ変更通知を行う
                /// </summary>
                private void NotifyPropertyChanged()
                {
                    Outer.NotifyPropertyChanged(nameof(RowCount));
                    Outer.NotifyPropertyChanged(nameof(ColumnCount));
                    Outer.NotifyPropertyChanged(ListConstant.IndexerName);
                }

                /// <summary>
                ///     要素変更前通知を行う
                /// </summary>
                private void CallCollectionChanging()
                {
                    Outer.CallCollectionChanging(CollectionChangeEventArgs);
                }

                /// <summary>
                ///     メイン処理を行う
                /// </summary>
                private void ExecuteMain()
                {
                    Outer.Clear_Core();
                }

                /// <summary>
                ///     要素変更前通知を行う
                /// </summary>
                private void CallCollectionChanged()
                {
                    Outer.CallCollectionChanged(CollectionChangeEventArgs);
                }

                #region Argument Create Methods

                /// <summary>
                ///     要素変更通知イベント引数を生成する。
                /// </summary>
                /// <returns>イベント引数</returns>
                private TwoDimensionalCollectionChangeEventArgs<T> CreateCollectionChangeEventArgs()
                {
                    if (Direction == Direction.None
                        || Direction == Direction.Row)
                    {
                        return CreateRemoveRowEventArgs();
                    }

                    if (Direction == Direction.Column)
                    {
                        return CreateRemoveColumnEventArgs();
                    }

                    // 通常ここへは来ない
                    throw new InvalidOperationException();
                }

                /// <summary>
                ///     行除去イベント用の引数を生成する。
                /// </summary>
                /// <returns>イベント引数</returns>
                private TwoDimensionalCollectionChangeEventArgs<T> CreateRemoveRowEventArgs()
                {
                    var removeItems = Outer.Get_Core(0, Outer.RowCount, 0, Outer.ColumnCount)
                        .ToTwoDimensionalArray();
                    return TwoDimensionalCollectionChangeEventArgs<T>.Factory.RemoveRow(0, removeItems);
                }

                /// <summary>
                ///     列除去イベント用の引数を生成する。
                /// </summary>
                /// <returns>イベント引数</returns>
                private TwoDimensionalCollectionChangeEventArgs<T> CreateRemoveColumnEventArgs()
                {
                    var removeItems = Outer.Get_Core(0, Outer.RowCount, 0, Outer.ColumnCount)
                        .ToTransposedArray();
                    return TwoDimensionalCollectionChangeEventArgs<T>.Factory.RemoveColumn(0, removeItems);
                }

                #endregion
            }
        }
    }
}
