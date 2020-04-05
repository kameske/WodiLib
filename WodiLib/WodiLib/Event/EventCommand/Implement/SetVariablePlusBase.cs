// ========================================
// Project Name : WodiLib
// File Name    : SetVariablePlusBase.cs
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
    /// イベントコマンド・変数操作+ベース
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class SetVariablePlusBase : EventCommandBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private const string EventCommandSentenceFormat = "■変数操作+: {3}{0} {1} {2}";

        private const string EventCommandSentenceRound = "[ﾘ]";
        private const string EventCommandSentenceNotRound = "";

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override EventCommandCode EventCommandCode => EventCommandCode.SetVariablePlus;

        /// <inheritdoc />
        public override byte StringVariableCount => 0x00;

        /// <inheritdoc />
        protected override EventCommandColorSet EventCommandColorSet
            => EventCommandColorSet.DeepRed;

        /// <inheritdoc />
        /// <summary>
        /// インデックスを指定して数値変数を取得する。
        /// ウディタ標準仕様でサポートしているインデックスのみ取得可能。
        /// </summary>
        /// <param name="index">[Range(0, 3～4)] インデックス</param>
        /// <returns>インデックスに対応した値</returns>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override int GetSafetyNumberVariable(int index)
        {
            if (index < 0 || NumberVariableCount <= index) throw new ArgumentOutOfRangeException();
            switch (index)
            {
                case 0:
                    return EventCommandCode.Code;

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
        public override void SetSafetyNumberVariable(int index, int value)
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
        /// ウディタ標準仕様でサポートしているインデックスのみ取得可能。
        /// </summary>
        /// <param name="index">[Range(0, -)] インデックス</param>
        /// <returns>インデックスに対応した値</returns>
        /// <exception cref="ArgumentOutOfRangeException">常に</exception>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string GetSafetyStringVariable(int index)
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
        public override void SetSafetyStringVariable(int index, string value)
        {
            throw new ArgumentOutOfRangeException();
        }

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override string MakeEventCommandMainSentence(
            EventCommandSentenceResolver resolver, EventCommandSentenceType type,
            EventCommandSentenceResolveDesc desc)
        {
            var leftSideStr = resolver.GetNumericVariableAddressStringIfVariableAddress(LeftSide, type, desc);
            if (IsUseVariableXLeft) leftSideStr = $"V[{leftSideStr}]";
            var rightSideStr = MakeEventCommandRightSideSentence(resolver, type, desc);
            var roundStr = IsRoundMillion
                ? EventCommandSentenceRound
                : EventCommandSentenceNotRound;

            return string.Format(EventCommandSentenceFormat,
                leftSideStr, AssignmentOperator.EventCommandSentence, rightSideStr,
                roundStr);
        }

        /// <summary>
        /// イベントコマンド文字列の右辺部分を生成する。
        /// </summary>
        /// <param name="resolver">[NotNull] 名前解決クラスインスタンス</param>
        /// <param name="type">[NotNull] イベント種別</param>
        /// <param name="desc">[Nullable] 付加情報</param>
        /// <returns>イベントコマンド文字列の右辺部分</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected abstract string MakeEventCommandRightSideSentence(
            EventCommandSentenceResolver resolver, EventCommandSentenceType type,
            EventCommandSentenceResolveDesc desc);

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private int leftSide;

        /// <summary>左辺（代入先）</summary>
        public int LeftSide
        {
            get => leftSide;
            set
            {
                leftSide = value;
                NotifyPropertyChanged();
            }
        }

        private bool isUseVariableXLeft;

        /// <summary>左辺）X番の変数呼び出し</summary>
        public bool IsUseVariableXLeft
        {
            get => isUseVariableXLeft;
            set
            {
                isUseVariableXLeft = value;
                NotifyPropertyChanged();
            }
        }

        private bool isRoundMillion;

        /// <summary>計算結果を±999999以内に収める</summary>
        public bool IsRoundMillion
        {
            get => isRoundMillion;
            set
            {
                isRoundMillion = value;
                NotifyPropertyChanged();
            }
        }

        private NumberPlusAssignmentOperator assignmentOperator = NumberPlusAssignmentOperator.Assign;

        /// <summary>[NotNull] 代入演算子</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public NumberPlusAssignmentOperator AssignmentOperator
        {
            get => assignmentOperator;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(AssignmentOperator)));
                assignmentOperator = value;
                NotifyPropertyChanged();
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