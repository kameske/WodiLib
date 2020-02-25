// ========================================
// Project Name : WodiLib
// File Name    : SetStringBase.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;
using WodiLib.Project;
using WodiLib.Sys;
using WodiLib.Sys.Cmn;

namespace WodiLib.Event.EventCommand
{
    /// <inheritdoc />
    /// <summary>
    /// イベントコマンド・文字列操作ベース
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class SetStringBase : EventCommandBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private const string EventCommandSentenceFormat = "■文字列操作：{0} {1} {2}";

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override EventCommandCode EventCommandCode => EventCommandCode.SetString;

        /// <inheritdoc />
        protected override EventCommandColorSet EventCommandColorSet
            => EventCommandColorSet.DeepRed;

        /// <inheritdoc />
        /// <summary>
        /// インデックスを指定して数値変数を取得する。
        /// ウディタ標準仕様でサポートしているインデックスのみ取得可能。
        /// </summary>
        /// <param name="index">[Range(0, 2～3)] インデックス</param>
        /// <returns>インデックスに対応した値</returns>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override int GetSafetyNumberVariable(int index)
        {
            if (index < 0 || NumberVariableCount <= index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), 0, NumberVariableCount, index));
            switch (index)
            {
                case 0:
                    return EventCommandCode.Code;

                case 1:
                    return LeftSide;

                case 2:
                {
                    byte byte0 = 0x00;
                    if (IsIndicateNumberVariable) byte0 += FlgIndicateNumberVariable;
                    byte0 += RightSidePropertyCode;
                    var byte1 = (byte) (AssignmentOperator.Code + RightSideSpecialSettingsCode);
                    return new byte[] {byte0, byte1, 0x00, 0x00}.ToInt32(Endian.Environment);
                }

                case 3:
                    return RightSideOption;

                default:
                    throw new ArgumentOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(index), 0, NumberVariableCount, index));
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// 数値変数を設定する。
        /// </summary>
        /// <param name="index">[Range(1, 2～3)] インデックス</param>
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
                    var bytes = value.ToBytes(Endian.Environment);
                    IsIndicateNumberVariable = (bytes[0] & 0xF0) != 0;
                    RightSideSpecialSettingsCode = (byte) (bytes[1] & 0xF0);
                    AssignmentOperator = StringAssignmentOperator.FromByte((byte) (bytes[1] & 0x0F));
                    return;
                }

                case 3:
                    RightSideOption = value;
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
        /// <param name="index">[Range(0, -1～1)] インデックス</param>
        /// <returns>インデックスに対応した値</returns>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override string GetSafetyStringVariable(int index)
        {
            if (index < 0 || StringVariableCount <= index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), 0, StringVariableCount, index));
            switch (index)
            {
                case 0:
                    return RightSideString;

                case 1:
                    return RightSideReplaceString;

                default:
                    throw new ArgumentOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(index), 0, StringVariableCount, index));
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// 文字列変数を設定する。
        /// </summary>
        /// <param name="index">[Range(0, -1～0)] インデックス</param>
        /// <param name="value">[NotNull] 設定値</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外</exception>
        /// <exception cref="ArgumentNullException">valueがnull</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override void SetSafetyStringVariable(int index, string value)
        {
            if (value is null) throw new ArgumentNullException(ErrorMessage.NotNull(nameof(value)));
            if (index < 0 || StringVariableCount <= index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), 0, StringVariableCount - 1, index));
            switch (index)
            {
                case 0:
                    RightSideString = value;
                    return;

                case 1:
                    RightSideReplaceString = value;
                    return;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override string MakeEventCommandMainSentence(
            EventCommandSentenceResolver resolver, EventCommandSentenceType type,
            EventCommandSentenceResolveDesc desc)
        {
            var leftSideStr = resolver.GetVariableAddressStringIfVariableAddress(LeftSide, type, desc);
            if (IsIndicateNumberVariable) leftSideStr = $"V[{leftSideStr}]";
            var rightSideStr = MakeEventCommandRightSideSentence(resolver, type, desc);
            return string.Format(EventCommandSentenceFormat,
                leftSideStr, AssignmentOperator.EventCommandSentence, rightSideStr);
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

        /// <summary>左辺（代入先）</summary>
        public int LeftSide { get; set; }

        /// <summary>代入先を変数で指定フラグ</summary>
        public bool IsIndicateNumberVariable { get; set; }

        private StringAssignmentOperator assignmentOperator = StringAssignmentOperator.Assign;

        /// <summary>[NotNull] 代入演算子</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public StringAssignmentOperator AssignmentOperator
        {
            get => assignmentOperator;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(AssignmentOperator)));
                assignmentOperator = value;
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Abstract Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>右辺設定コード値</summary>
        protected abstract byte RightSidePropertyCode { get; }

        /// <summary>右辺特殊設定コード値</summary>
        protected abstract byte RightSideSpecialSettingsCode { get; set; }

        /// <summary>右辺オプション</summary>
        protected abstract int RightSideOption { get; set; }

        /// <summary>右辺文字列</summary>
        protected abstract string RightSideString { get; set; }

        /// <summary>右辺置換文字列</summary>
        protected abstract string RightSideReplaceString { get; set; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Const
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private const byte FlgIndicateNumberVariable = 0x10;


        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     VersionCheck
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        /// <summary>
        /// VersionConfigにセットされたバージョンとイベントコマンドの内容を確認し、
        /// イベントコマンドの内容が設定バージョンに対応していないものであれば警告ログを出力する。
        /// </summary>
        public override void OutputVersionWarningLogIfNeed()
        {
            if (VersionConfig.IsUnderVersion(WoditorVersion.Ver2_02))
            {
                OutputVersionWarningLogIfNeed_UnderVer2_02();
            }
        }

        /// <summary>
        /// 設定バージョン = 2.02未満 の場合の警告
        /// </summary>
        private void OutputVersionWarningLogIfNeed_UnderVer2_02()
        {
            if (AssignmentOperator == StringAssignmentOperator.CutUp)
            {
                Logger.Warning(VersionWarningMessage.NotUnderInCommandSetting(
                    $"{nameof(SetStringBase)}.{nameof(AssignmentOperator)}",
                    $"{StringAssignmentOperator.CutUp}",
                    VersionConfig.GetConfigWoditorVersion(),
                    WoditorVersion.Ver2_02));
            }

            if (AssignmentOperator == StringAssignmentOperator.CutAfter)
            {
                Logger.Warning(VersionWarningMessage.NotUnderInCommandSetting(
                    $"{nameof(SetStringBase)}.{nameof(AssignmentOperator)}",
                    $"{StringAssignmentOperator.CutAfter}",
                    VersionConfig.GetConfigWoditorVersion(),
                    WoditorVersion.Ver2_02));
            }
        }
    }
}