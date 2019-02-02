// ========================================
// Project Name : WodiLib
// File Name    : SetVariable.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;
using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <inheritdoc />
    /// <summary>
    /// イベントコマンド・変数操作
    /// </summary>
    public class SetVariable : EventCommandBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override int EventCommandCode => EventCommand.EventCommandCode.SetVariable;

        /// <inheritdoc />
        public override byte NumberVariableCount => (byte) (IsMultiTarget ? 0x06 : 0x05);

        /// <inheritdoc />
        public override byte StringVariableCount => 0x00;

        /// <inheritdoc />
        /// <summary>
        /// インデックスを指定して数値変数を取得する。
        /// </summary>
        /// <param name="index">[Range(0, 4～5)] インデックス</param>
        /// <returns>インデックスに対応した値</returns>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override int GetNumberVariable(int index)
        {
            switch (index)
            {
                case 0:
                    return EventCommandCode;

                case 1:
                    return LeftSide;

                case 2:
                    return RightSide1;

                case 3:
                    return RightSide2;

                case 4:
                {
                    byte byte0 = 0x00;
                    {
                        // byte0
                        if (IsRoundMillion) byte0 += FlgRoundMillion;
                        if (IsCalcReal) byte0 += FlgCalcReal;
                        if (IsUseVariableXLeft) byte0 += FlgUseVariableXLeft;
                        if (IsNotReferRight1) byte0 += FlgNotReferRight1;
                        if (IsUseVariableXRight1) byte0 += FlgUseVariableXRight1;
                        if (IsNotReferRight2) byte0 += FlgNotReferRight2;
                        if (IsUseVariableXRight2) byte0 += FlgUseVariableXRight2;
                    }
                    var byte1 = (byte) (CalculateOperator.Code + AssignmentOperator.Code);
                    var byte2 = (byte) (IsMultiTarget ? FlgMultiTarget : 0);
                    return new byte[] {byte0, byte1, byte2, 0x00}.ToInt32(Endian.Little);
                }

                case 5:
                    return OperationSeqValue;

                default:
                    throw new ArgumentOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(index), 0, 5, index));
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// 数値変数を設定する。
        /// </summary>
        /// <param name="index">[Range(1, 4～5)] インデックス</param>
        /// <param name="value">設定値</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override void SetNumberVariable(int index, int value)
        {
            switch (index)
            {
                case 1:
                    LeftSide = value;
                    return;

                case 2:
                    RightSide1 = value;
                    return;

                case 3:
                    RightSide2 = value;
                    return;

                case 4:
                {
                    var bytes = value.ToBytes(Endian.Little);

                    IsRoundMillion = (bytes[0] & FlgRoundMillion) != 0;
                    IsCalcReal = (bytes[0] & FlgCalcReal) != 0;
                    IsUseVariableXLeft = (bytes[0] & FlgUseVariableXLeft) != 0;
                    IsNotReferRight1 = (bytes[0] & FlgNotReferRight1) != 0;
                    IsUseVariableXRight1 = (bytes[0] & FlgUseVariableXRight1) != 0;
                    IsNotReferRight2 = (bytes[0] & FlgNotReferRight2) != 0;
                    IsUseVariableXRight2 = (bytes[0] & FlgUseVariableXRight2) != 0;

                    CalculateOperator = CalculateOperator.FromByte((byte) (bytes[1] & 0xF0));
                    AssignmentOperator = NumberAssignmentOperator.FromByte((byte) (bytes[1] & 0x0F));

                    IsMultiTarget = (bytes[2] & FlgMultiTarget) != 0;

                    return;
                }
                case 5:
                    OperationSeqValue = value;
                    return;

                default:
                    throw new ArgumentOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(index), 1, NumberVariableCount, index));
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// インデックスを指定して文字列変数を取得する。
        /// </summary>
        /// <param name="index">[Range(0, -)] インデックス</param>
        /// <returns>インデックスに対応した値</returns>
        /// <exception cref="ArgumentOutOfRangeException">常に</exception>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string GetStringVariable(int index)
        {
            throw new ArgumentOutOfRangeException();
        }

        /// <inheritdoc />
        /// <summary>
        /// 文字列変数を設定する。
        /// </summary>
        /// <param name="index">[Range(0, -)] インデックス</param>
        /// <param name="value">[NotNull] 設定値</param>
        /// <exception cref="ArgumentOutOfRangeException">常に</exception>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override void SetStringVariable(int index, string value)
        {
            throw new ArgumentOutOfRangeException();
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>左辺（代入先）</summary>
        public int LeftSide { get; set; }

        private NumberAssignmentOperator assignmentOperator = NumberAssignmentOperator.Assign;

        /// <summary>[NotNull] 代入演算子</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public NumberAssignmentOperator AssignmentOperator
        {
            get => assignmentOperator;
            set
            {
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(AssignmentOperator)));
                assignmentOperator = value;
            }
        }

        /// <summary>右辺その1</summary>
        public int RightSide1 { get; set; }

        private CalculateOperator calculateOperator = CalculateOperator.Addition;

        /// <summary>[NotNull] 計算演算子</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public CalculateOperator CalculateOperator
        {
            get => calculateOperator;
            set
            {
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(CalculateOperator)));
                calculateOperator = value;
            }
        }

        /// <summary>右辺その2</summary>
        public int RightSide2 { get; set; }

        /// <summary>計算結果を±999999以内に収める</summary>
        public bool IsRoundMillion { get; set; }

        /// <summary>実数計算</summary>
        public bool IsCalcReal { get; set; }

        /// <summary>左辺）X番の変数呼び出し</summary>
        public bool IsUseVariableXLeft { get; set; }

        /// <summary>右辺1）データを呼ばない</summary>
        public bool IsNotReferRight1 { get; set; }

        /// <summary>右辺1）X番の変数呼び出し</summary>
        public bool IsUseVariableXRight1 { get; set; }

        /// <summary>右辺2）データを呼ばない</summary>
        public bool IsNotReferRight2 { get; set; }

        /// <summary>右辺2）X番の変数呼び出し</summary>
        public bool IsUseVariableXRight2 { get; set; }

        /// <summary>連続変数操作フラグ</summary>
        public bool IsMultiTarget { get; set; }

        /// <summary>連続操作変数の数</summary>
        public int OperationSeqValue { get; set; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Const
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private const byte FlgRoundMillion = 0x01;
        private const byte FlgCalcReal = 0x02;
        private const byte FlgUseVariableXLeft = 0x10;
        private const byte FlgNotReferRight1 = 0x04;
        private const byte FlgUseVariableXRight1 = 0x20;
        private const byte FlgNotReferRight2 = 0x08;
        private const byte FlgUseVariableXRight2 = 0x40;

        private const byte FlgMultiTarget = 0x01;
    }
}