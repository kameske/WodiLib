// ========================================
// Project Name : WodiLib
// File Name    : SetVariableChangeableDB.cs
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
    /// イベントコマンド・変数操作（可変DB）
    /// </summary>
    [Serializable]
    public class SetVariableChangeableDB : SetVariableBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>左辺（代入先）</summary>
        public new static readonly int LeftSide = 1100000000;

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

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override byte NumberVariableCount => 0x08;

        /// <inheritdoc />
        /// <summary>
        /// インデックスを指定して数値変数を取得する。
        /// ウディタ標準仕様でサポートしているインデックスのみ取得可能。
        /// </summary>
        /// <param name="index">[Range(0, 7)] インデックス</param>
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
                    return new byte[] {byte0, byte1, 0x00, 0x00}.ToInt32(Endian.Little);
                }

                case 5:
                    return CDBTypeNumber;

                case 6:
                    return CDBDataNumber;

                case 7:
                    return CDBItemNumber;

                default:
                    throw new ArgumentOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(index), 0, 7, index));
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// 数値変数を設定する。
        /// </summary>
        /// <param name="index">[Range(1, 7)] インデックス</param>
        /// <param name="value">設定値</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override void SetSafetyNumberVariable(int index, int value)
        {
            switch (index)
            {
                case 1:
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
                    return;
                }

                case 5:
                    CDBTypeNumber = value;
                    return;

                case 6:
                    CDBDataNumber = value;
                    return;

                case 7:
                    CDBItemNumber = value;
                    return;

                default:
                    throw new ArgumentOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(index), 1, 7, index));
            }
        }

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override string MakeLeftSideStr(EventCommandSentenceResolver resolver,
            EventCommandSentenceType type, EventCommandSentenceResolveDesc desc)
        {
            var leftSideStr = resolver.GetDatabaseCommandSentenceForSetVariable(CDBTypeNumber,
                CDBDataNumber, CDBItemNumber, type, desc);

            if (IsUseVariableXLeft) leftSideStr = $"V[{leftSideStr}]";

            return leftSideStr;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private int cDBTypeNumber;

        /// <summary>可変DBタイプ番号</summary>
        public int CDBTypeNumber
        {
            get => cDBTypeNumber;
            set
            {
                cDBTypeNumber = value;
                NotifyPropertyChanged();
            }
        }

        private int cDBDataNumber;

        /// <summary>可変DBデータ番号</summary>
        public int CDBDataNumber
        {
            get => cDBDataNumber;
            set
            {
                cDBDataNumber = value;
                NotifyPropertyChanged();
            }
        }

        private int cDBItemNumber;

        /// <summary>可変DB項目番号</summary>
        public int CDBItemNumber
        {
            get => cDBItemNumber;
            set
            {
                cDBItemNumber = value;
                NotifyPropertyChanged();
            }
        }
    }
}