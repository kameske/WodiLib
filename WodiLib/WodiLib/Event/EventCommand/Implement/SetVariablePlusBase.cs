// ========================================
// Project Name : WodiLib
// File Name    : SetVariablePlusBase.cs
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
    /// イベントコマンド・変数操作+ベース
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class SetVariablePlusBase : EventCommandBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override int EventCommandCode => EventCommand.EventCommandCode.SetVariablePlus;

        /// <inheritdoc />
        public override byte StringVariableCount => 0x00;

        /// <inheritdoc />
        /// <summary>
        /// インデックスを指定して数値変数を取得する。
        /// </summary>
        /// <param name="index">[Range(0, 3～4)] インデックス</param>
        /// <returns>インデックスに対応した値</returns>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override int GetNumberVariable(int index)
        {
            if (index < 0 || NumberVariableCount <= index) throw new ArgumentOutOfRangeException();
            switch (index)
            {
                case 0:
                    return EventCommandCode;

                case 1:
                    return LeftSide;

                case 2:
                {
                    byte byte0 = 0x00;
                    if (IsUseVariableXLeft) byte0 += FlgUseVariableXLeft;
                    if (IsRoundMillion) byte0 += FlgRoundMillion;
                    byte0 += InfoOptionFlag;

                    var byte1 = (byte) (ExecCode + AssignmentOperator.Code);

                    var byte2 = InfoTypeCode;

                    return new byte[] {byte0, byte1, byte2, 0x00}.ToInt32(Endian.Little);
                }

                case 3:
                    return TargetCode;

                case 4:
                    return TargetDetailCode;

                default:
                    throw new ArgumentOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(index), 0, NumberVariableCount, index));
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// 数値変数を設定する。
        /// </summary>
        /// <param name="index">[Range(1, 3～4)] インデックス</param>
        /// <param name="value">設定値</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override void SetNumberVariable(int index, int value)
        {
            if (index < 1 || NumberVariableCount <= index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), 1, NumberVariableCount, index));
            switch (index)
            {
                case 1:
                    LeftSide = value;
                    return;

                case 2:
                {
                    var bytes = value.ToBytes(Endian.Little);
                    InfoOptionFlag = (byte) (bytes[0] & 0xF0);
                    IsUseVariableXLeft = (bytes[0] & FlgUseVariableXLeft) != 0;
                    IsRoundMillion = (bytes[0] & FlgRoundMillion) != 0;

                    ExecCode = (byte) (bytes[1] & 0xF0);
                    AssignmentOperator = NumberPlusAssignmentOperator.FromByte((byte) (bytes[1] & 0x0F));

                    InfoTypeCode = bytes[2];

                    return;
                }
                case 3:
                    TargetCode = value;
                    return;

                case 4:
                    TargetDetailCode = value;
                    return;

                default:
                    throw new ArgumentOutOfRangeException(ErrorMessage.OutOfRange(
                        nameof(index), 1, NumberVariableCount, index));
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

        /// <summary>左辺）X番の変数呼び出し</summary>
        public bool IsUseVariableXLeft { get; set; }

        /// <summary>計算結果を±999999以内に収める</summary>
        public bool IsRoundMillion { get; set; }

        private NumberPlusAssignmentOperator assignmentOperator = NumberPlusAssignmentOperator.Assign;

        /// <summary>[NotNull] 代入演算子</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public NumberPlusAssignmentOperator AssignmentOperator
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

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>取得情報オプションフラグ</summary>
        protected abstract byte InfoOptionFlag { get; set; }

        /// <summary>取得項目コード値</summary>
        protected abstract byte ExecCode { get; set; }

        /// <summary>取得情報種別コード値</summary>
        protected abstract byte InfoTypeCode { get; set; }

        /// <summary>取得項目コード値</summary>
        protected abstract int TargetCode { get; set; }

        /// <summary>取得情報コード値</summary>
        protected abstract int TargetDetailCode { get; set; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private const byte FlgUseVariableXLeft = 0x10;
        private const byte FlgRoundMillion = 0x01;
    }
}