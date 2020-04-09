// ========================================
// Project Name : WodiLib
// File Name    : SetVariable.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;
using WodiLib.Project;
using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <inheritdoc />
    /// <summary>
    /// イベントコマンド・変数操作
    /// </summary>
    [Serializable]
    public class SetVariable : SetVariableBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private const byte FlgRoundMillion = 0x01;
        private const byte FlgCalcReal = 0x02;
        private const byte FlgUseVariableXLeft = 0x10;
        private const byte FlgNotReferRight1 = 0x04;
        private const byte FlgUseVariableXRight1 = 0x20;
        private const byte FlgNotReferRight2 = 0x08;
        private const byte FlgUseVariableXRight2 = 0x40;

        private const byte FlgMultiTarget = 0x01;

        private const string EventCommandSentenceSingleLeftSide = "{0}";
        private const string EventCommandSentenceMultiLeftSide = "{0}～{1}";

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override byte NumberVariableCount => (byte) (IsMultiTarget ? 0x06 : 0x05);

        /// <inheritdoc />
        /// <summary>数値変数最小個数</summary>
        public override byte NumberVariableCountMin => 0x05;

        /// <inheritdoc />
        /// <summary>
        /// インデックスを指定して数値変数を取得する。
        /// ウディタ標準仕様でサポートしているインデックスのみ取得可能。
        /// </summary>
        /// <param name="index">[Range(0, 4～5)] インデックス</param>
        /// <returns>インデックスに対応した値</returns>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override int GetSafetyNumberVariable(int index)
        {
            switch (index)
            {
                case 0:
                    return EventCommandCode.Code;

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
        public override void SetSafetyNumberVariable(int index, int value)
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
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override string MakeLeftSideStr(EventCommandSentenceResolver resolver,
            EventCommandSentenceType type, EventCommandSentenceResolveDesc desc)
        {
            string leftSideStr;
            if (IsMultiTarget)
            {
                var leftSideStartStr = resolver.GetVariableAddressStringIfVariableAddress(LeftSide, type, desc);
                var leftSideEndStr = resolver.GetVariableAddressStringIfVariableAddress(
                    LeftSide + OperationSeqValue, type, desc);
                leftSideStr = string.Format(EventCommandSentenceMultiLeftSide,
                    leftSideStartStr, leftSideEndStr);
            }
            else
            {
                var leftSideStartStr = resolver.GetVariableAddressStringIfVariableAddress(LeftSide, type, desc);
                leftSideStr = string.Format(EventCommandSentenceSingleLeftSide, leftSideStartStr);
            }

            if (IsUseVariableXLeft) leftSideStr = $"V[{leftSideStr}]";

            return leftSideStr;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     InnerNotifyChanged
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 自身のプロパティ変更通知
        /// </summary>
        /// <param name="sender">送信元</param>
        /// <param name="args">情報</param>
        private void OnThisPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            switch (args.PropertyName)
            {
                case nameof(IsMultiTarget):
                    NotifyPropertyChanged(nameof(NumberVariableCount));
                    break;
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SetVariable()
        {
            PropertyChanged += OnThisPropertyChanged;
        }
    }
}