// ========================================
// Project Name : WodiLib
// File Name    : TwoDimensionalListBase.AdjustLengthOneDirectionAgent.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.Sys.Collections
{
    public partial class TwoDimensionalListBase<T>
    {
        /// <summary>
        ///     AdjustLength 系メソッドの処理代理インタフェース
        /// </summary>
        private interface IAdjustLengthAgent
        {
            /// <summary>
            ///     コンストラクタで与えられた情報をもとに、必要な処理を実施する。
            /// </summary>
            void Execute();
        }

        /// <summary>
        ///     AdjustLength 系メソッドの処理代理クラス
        /// </summary>
        private partial class AdjustLengthAgent : IAdjustLengthAgent
        {
            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
            //     Private Property
            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

            private IAdjustLengthAgent MyAgent { get; }

            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
            //     Constructor
            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

            /// <summary>
            ///     コンストラクタ
            /// </summary>
            /// <remarks>
            ///     【事前条件】<br/>
            ///     ・<paramref name="direction"/> が <see cref="Direction.Row"/> または
            ///     <see cref="Direction.Column"/> の場合、 <paramref name="outer"/> の
            ///     行列数が0でないこと。<br/>
            /// </remarks>
            /// <param name="outer">処理対象</param>
            /// <param name="rowLength">調整行数</param>
            /// <param name="columnLength">調整列数</param>
            /// <param name="direction">調整方向</param>
            /// <param name="methodType">メソッド種別</param>
            private AdjustLengthAgent(TwoDimensionalListBase<T> outer, int rowLength, int columnLength,
                Direction direction, MethodType methodType)
            {
                if (rowLength == 0)
                {
                    MyAgent = new ToEmpty(outer, direction, methodType);
                }
                else if (outer.IsEmpty)
                {
                    MyAgent = new FromEmpty(outer, rowLength, columnLength, methodType);
                }
                else
                {
                    MyAgent = new NoneEmpty(outer, rowLength, columnLength, direction, methodType);
                }
            }

            /// <summary>
            ///     AdjustLength メソッド用のインスタンスを作成する。
            /// </summary>
            /// <param name="outer">処理対象</param>
            /// <param name="rowLength">調整行数</param>
            /// <param name="columnLength">調整列数</param>
            /// <returns>AdjustLengthAgent インスタンス</returns>
            public static AdjustLengthAgent ForAdjustLength(
                TwoDimensionalListBase<T> outer, int rowLength, int columnLength)
            {
                return new(outer, rowLength, columnLength,
                    Direction.None, MethodType.None);
            }

            /// <summary>
            ///     AdjustLengthIfShort メソッド用のインスタンスを作成する。
            /// </summary>
            /// <param name="outer">処理対象</param>
            /// <param name="rowLength">調整行数</param>
            /// <param name="columnLength">調整列数</param>
            /// <returns>AdjustLengthAgent インスタンス</returns>
            public static AdjustLengthAgent ForAdjustLengthIfShort(
                TwoDimensionalListBase<T> outer, int rowLength, int columnLength)
            {
                return new(outer, rowLength, columnLength,
                    Direction.None, MethodType.IfShort);
            }

            /// <summary>
            ///     AdjustLengthIfLong メソッド用のインスタンスを作成する。
            /// </summary>
            /// <param name="outer">処理対象</param>
            /// <param name="rowLength">調整行数</param>
            /// <param name="columnLength">調整列数</param>
            /// <returns>AdjustLengthAgent インスタンス</returns>
            public static AdjustLengthAgent ForAdjustLengthIfLong(
                TwoDimensionalListBase<T> outer, int rowLength, int columnLength)
            {
                return new(outer, rowLength, columnLength,
                    Direction.None, MethodType.IfLong);
            }

            /// <summary>
            ///     AdjustRowLength メソッド用のインスタンスを作成する。
            /// </summary>
            /// <param name="outer">処理対象</param>
            /// <param name="length">調整行数</param>
            /// <returns>AdjustLengthAgent インスタンス</returns>
            public static AdjustLengthAgent ForAdjustRowLength(
                TwoDimensionalListBase<T> outer, int length)
            {
                return new(outer, length, outer.ColumnCount,
                    Direction.Row, MethodType.None);
            }

            /// <summary>
            ///     AdjustRowLengthIfShort メソッド用のインスタンスを作成する。
            /// </summary>
            /// <param name="outer">処理対象</param>
            /// <param name="length">調整行数</param>
            /// <returns>AdjustLengthAgent インスタンス</returns>
            public static AdjustLengthAgent ForAdjustRowLengthIfShort(
                TwoDimensionalListBase<T> outer, int length)
            {
                return new(outer, length, outer.ColumnCount,
                    Direction.Row, MethodType.IfShort);
            }

            /// <summary>
            ///     AdjustRowLengthIfLong メソッド用のインスタンスを作成する。
            /// </summary>
            /// <param name="outer">処理対象</param>
            /// <param name="length">調整行数</param>
            /// <returns>AdjustLengthAgent インスタンス</returns>
            public static AdjustLengthAgent ForAdjustRowLengthIfLong(
                TwoDimensionalListBase<T> outer, int length)
            {
                return new(outer, length, outer.ColumnCount,
                    Direction.Row, MethodType.IfLong);
            }

            /// <summary>
            ///     AdjustColumnLength メソッド用のインスタンスを作成する。
            /// </summary>
            /// <param name="outer">処理対象</param>
            /// <param name="length">調整列数</param>
            /// <returns>AdjustLengthAgent インスタンス</returns>
            public static AdjustLengthAgent ForAdjustColumnLength(
                TwoDimensionalListBase<T> outer, int length)
            {
                return new(outer, outer.RowCount, length,
                    Direction.Column, MethodType.None);
            }

            /// <summary>
            ///     AdjustColumnLengthIfShort メソッド用のインスタンスを作成する。
            /// </summary>
            /// <param name="outer">処理対象</param>
            /// <param name="length">調整列数</param>
            /// <returns>AdjustLengthAgent インスタンス</returns>
            public static AdjustLengthAgent ForAdjustColumnLengthIfShort(
                TwoDimensionalListBase<T> outer, int length)
            {
                return new(outer, outer.RowCount, length,
                    Direction.Column, MethodType.IfShort);
            }

            /// <summary>
            ///     AdjustColumnLengthIfLong メソッド用のインスタンスを作成する。
            /// </summary>
            /// <param name="outer">処理対象</param>
            /// <param name="length">調整列数</param>
            /// <returns>AdjustLengthAgent インスタンス</returns>
            public static AdjustLengthAgent ForAdjustColumnLengthIfLong(
                TwoDimensionalListBase<T> outer, int length)
            {
                return new(outer, outer.RowCount, length,
                    Direction.Column, MethodType.IfLong);
            }

            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
            //     Public Method
            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

            /// <summary>
            ///     コンストラクタで与えられた情報をもとに、必要な処理を実施する。
            /// </summary>
            public void Execute()
            {
                MyAgent.Execute();
            }

            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
            //     Private Enum
            // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

            /// <summary>
            ///     メソッド種別
            /// </summary>
            internal enum MethodType
            {
                /// <summary>指定なし</summary>
                None,

                /// <summary>IfLong</summary>
                IfLong,

                /// <summary>IfShort</summary>
                IfShort,
            }
        }
    }
}
