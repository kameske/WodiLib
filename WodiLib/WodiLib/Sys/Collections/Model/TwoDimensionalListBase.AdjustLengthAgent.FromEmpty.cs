// ========================================
// Project Name : WodiLib
// File Name    : TwoDimensionalListBase.AdjustLengthAgent.FromEmpty.cs
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
            ///     空リストから変化させる処理の代理クラス
            /// </summary>
            /// <remarks>
            ///     AdjustLength, AdjustLengthIfShort, AdjustLengthIfLong メソッドの処理で
            ///     変更前の状態が空リストの場合に使用する。<br/>
            ///     ただし変更後の状態も空リストの場合は <see cref="ToEmpty"/> で扱う。
            /// </remarks>
            private class FromEmpty : IAdjustLengthAgent
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

                /// <summary>呼び出し元メソッド種別</summary>
                private MethodType MethodType { get; }

                // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
                //     Private Lazy Property
                // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

                private bool? _isCountChange;

                /// <summary>要素数変更フラグ</summary>
                private bool IsCountChange
                    => _isCountChange ??= MethodType switch
                    {
                        MethodType.None => true,
                        MethodType.IfLong => false,
                        MethodType.IfShort => true,
                        _ => throw new InvalidOperationException()
                    };

                private TwoDimensionalCollectionChangeEventArgs<T>? _collectionChangeEventArgs;

                /// <summary>CollectionChangeEvent 引数</summary>
                private TwoDimensionalCollectionChangeEventArgs<T> CollectionChangeEventArgs
                    => _collectionChangeEventArgs ??=
                        CreateCollectionChangeEventArgs();

                // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
                //     Constructor
                // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

                /// <summary>
                ///     コンストラクタ
                /// </summary>
                /// <param name="outer">処理対象</param>
                /// <param name="rowLength">調整行数</param>
                /// <param name="columnLength">調整列数</param>
                /// <param name="methodType">呼び出し元メソッド種別</param>
                public FromEmpty(TwoDimensionalListBase<T> outer, int rowLength,
                    int columnLength, MethodType methodType)
                {
                    Outer = outer;
                    RowLength = rowLength;
                    ColumnLength = columnLength;
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
                    if (!IsCountChange)
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
                    var itemArray = CollectionChangeEventArgs.NewItems!
                        .ToTwoDimensionalArray();
                    Outer.InsertRow_Core(0, itemArray);
                }

                /// <summary>
                ///     要素変更後通知を行う
                /// </summary>
                private void CallCollectionChanged()
                {
                    Outer.CallCollectionChanged(CollectionChangeEventArgs);
                }

                /// <summary>
                ///     要素変更通知イベント引数を生成する。
                /// </summary>
                /// <returns>イベント引数の列挙子</returns>
                private TwoDimensionalCollectionChangeEventArgs<T> CreateCollectionChangeEventArgs()
                {
                    var startRow = Outer.RowCount;
                    var items = Outer.MakeDefaultItemArray(0, RowLength, 0, ColumnLength);

                    return TwoDimensionalCollectionChangeEventArgs<T>.Factory.AddRow(startRow, items);
                }
            }
        }
    }
}
