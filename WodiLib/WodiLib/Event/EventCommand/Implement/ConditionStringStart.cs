// ========================================
// Project Name : WodiLib
// File Name    : ConditionStringStart.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;
using System.Linq;
using WodiLib.Project;
using WodiLib.Sys;
using WodiLib.Sys.Cmn;

namespace WodiLib.Event.EventCommand
{
    /// <inheritdoc />
    /// <summary>
    /// イベントコマンド・条件（文字列）・始端
    /// </summary>
    [Serializable]
    public class ConditionStringStart : EventCommandBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>数値変数の数最大値</summary>
        private static readonly int NumberVariableCountMax = 31;

        /// <summary>代入コードシフト係数</summary>
        private static readonly int OperationCodeShift = 24;

        /// <summary>右辺種別シフト係数</summary>
        private static readonly int RightSideFlagShift = 24;

        private const string EventCommandSentenceFormatMain
            = "■条件分岐(文字): {0}";

        private const string EventCommandSentenceFormatFork
            = " 【{0}】 {1}が {2} {3}";

        private const string EventCommandSentenceFormatForkCondition
            = "{0} {1} {2}";

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override EventCommandCode EventCommandCode => EventCommandCode.ConditionStringStart;

        /// <inheritdoc />
        public override byte NumberVariableCount
        {
            get
            {
                var numRightMax = conditionList.SearchUseNumberVariableForRightSideMax();
                return (byte) (CaseValue + 2 + numRightMax);
            }
        }

        /// <inheritdoc />
        public override byte StringVariableCount
        {
            get
            {
                var numRightMax = (byte) conditionList.SearchUseNumberVariableForRightSideMax();

                return numRightMax >= 0x04 ? numRightMax : (byte) 0x04;
            }
        }

        /// <inheritdoc />
        /// <summary>数値変数最小個数</summary>
        public override byte NumberVariableCountMin => 0x03;

        /// <inheritdoc />
        /// <summary>数値変数最小個数</summary>
        public override byte StringVariableCountMin => 0x00;

        /// <inheritdoc />
        protected override EventCommandColorSet EventCommandColorSet
            => EventCommandColorSet.Black;

        /// <inheritdoc />
        /// <summary>
        /// インデックスを指定して数値変数を取得する。
        /// ウディタ標準仕様でサポートしているインデックスのみ取得可能。
        /// </summary>
        /// <param name="index">[Range(0, 1～31)] インデックス</param>
        /// <returns>インデックスに対応した値</returns>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override int GetSafetyNumberVariable(int index)
        {
            if (index < 0 || NumberVariableCount < index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), 0, NumberVariableCount - 1, index));

            switch (index)
            {
                case 0:
                    return EventCommandCode.Code;

                case 1:
                {
                    var byte0 = (byte) ((IsElseCase ? 0x10 : 0x00) + CaseValue);
                    var bytes = new byte[]
                    {
                        byte0, 0x00, 0x00, 0x00
                    };
                    return bytes.ToInt32(Endian.Little);
                }

                default:
                    var tmpIndex = index - 2;
                    if (tmpIndex < CaseValue)
                    {
                        // tmpIndex が選択肢数より少ないので、左辺 & 右辺変数指定フラグを参照
                        var condition = conditionList[tmpIndex];
                        var operationFlag = condition.Condition.Code << OperationCodeShift;
                        var rightVarFlag = (condition.IsUseNumberVariable ? 1 : 0) << RightSideFlagShift;
                        return condition.LeftSide + operationFlag + rightVarFlag;
                    }

                    // 残りは条件右辺の変数指定のみのハズ
                    tmpIndex -= CaseValue;
                    if (tmpIndex > conditionList.SearchUseNumberVariableForRightSideMax())
                    {
                        // 右辺数値引数指定最大インデックス以上の値の場合はロジックエラー
                        throw new InvalidOperationException();
                    }

                    var checkRightSide = conditionList[tmpIndex].RightSide;
                    if (checkRightSide.HasInt)
                    {
                        // 対象の右辺が数値設定されているならその値を返す
                        return checkRightSide.ToInt();
                    }

                    // 文字列指定されているので、デフォルト値（S[0]）を返す
                    return 3000000;
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// 数値変数を設定する。
        /// </summary>
        /// <param name="index">[Range(1, 1～31)] インデックス</param>
        /// <param name="value">設定値</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override void SetSafetyNumberVariable(int index, int value)
        {
            if (index < 1 || NumberVariableCountMax < index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), 1, NumberVariableCountMax, index));
            switch (index)
            {
                case 1:
                {
                    // 上記以外フラグ & 分岐数
                    var bytes = value.ToBytes(Endian.Woditor);
                    IsElseCase = (bytes[0] & 0xF0) != 0;
                    CaseValue = bytes[0] & 0x0F;
                    return;
                }

                default:
                {
                    var tmpIndex = index - 2;
                    if (tmpIndex < CaseValue)
                    {
                        // 左辺 & その他フラグ
                        var leftSide = value & 0x00_FF_FF_FF;
                        var bytes = value.ToBytes(Endian.Environment);
                        var ope = (byte) (bytes[3] & 0xF0);
                        var isVar = (bytes[3] & 0x0F) == 1;

                        conditionList[tmpIndex].LeftSide = leftSide;
                        conditionList[tmpIndex].Condition = StringConditionalOperator.ForByte(ope);
                        conditionList[tmpIndex].IsUseNumberVariable = isVar;

                        return;
                    }

                    // 残りは右辺（正しいかどうかチェックが必要）
                    {
                        tmpIndex -= CaseValue;
                        if (tmpIndex >= CaseValue)
                        {
                            if (index < 1 || NumberVariableCountMax < index)
                                throw new ArgumentOutOfRangeException(
                                    ErrorMessage.OutOfRange(nameof(index), 1, NumberVariableCountMax, index));
                        }

                        conditionList[tmpIndex].RightSide.Merge(value);
                        return;
                    }
                }
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// インデックスを指定して文字列変数を取得する。
        /// ウディタ標準仕様でサポートしているインデックスのみ取得可能。
        /// </summary>
        /// <param name="index">[Range(0, 3~14)] インデックス</param>
        /// <returns>インデックスに対応した値</returns>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override string GetSafetyStringVariable(int index)
        {
            if (index < 0 || StringVariableCount - 1 < index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), 0, StringVariableCount - 1, index));
            // 右辺文字列
            if (index >= CaseValue)
            {
                // 選択肢に含まれない場合は空文字
                return "";
            }

            return conditionList[index].GetRightSideString();
        }

        /// <inheritdoc />
        /// <summary>
        /// 文字列変数を設定する。
        /// </summary>
        /// <param name="index">[Range(0, 3~14)] インデックス</param>
        /// <param name="value">[NotNull] 設定値</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外</exception>
        /// <exception cref="ArgumentNullException">valueがnull</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override void SetSafetyStringVariable(int index, string value)
        {
            if (index < 0 || StringVariableCount - 1 < index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), 0, StringVariableCount - 1, index));
            // 右辺文字列を書き換える
            conditionList[index].RightSide.Merge(value);
        }

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override string MakeEventCommandMainSentence(
            EventCommandSentenceResolver resolver, EventCommandSentenceType type,
            EventCommandSentenceResolveDesc desc)
        {
            if (desc is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(desc)));

            var forkStrList = ConditionList.Select((x, idx) =>
            {
                var leftVarName = resolver.GetStringVariableAddressString(x.LeftSide, type, desc);
                var rightVarName = x.IsUseNumberVariable
                    ? resolver.GetStringVariableAddressString(x.RightSide.ToInt(), type, desc)
                    : $"\"{x.RightSide.ToStr()}\"";

                var myStr = string.Format(EventCommandSentenceFormatFork,
                    idx + 1, leftVarName, rightVarName, x.Condition.EventCommandSentence);
                var branchStr = string.Format(EventCommandSentenceFormatForkCondition,
                    leftVarName, rightVarName, x.Condition.EventCommandSentence);

                return (myStr, branchStr);
            }).ToList();
            var thisFortSrtList = forkStrList.Select(x => x.myStr).ToList();
            var childForkStrList = forkStrList.Select(x => x.branchStr).ToList();

            desc.StartBranch(BranchType.ConditionString, childForkStrList);

            return string.Format(EventCommandSentenceFormatMain,
                string.Join("", thisFortSrtList));
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>「上記以外」分岐フラグ</summary>
        public bool IsElseCase { get; set; }

        private readonly ConditionStringList conditionList = new ConditionStringList();

        /// <summary>分岐条件リスト</summary>
        public ConditionStringList ConditionList => conditionList;

        /// <summary>[Range(1, 15)] 分岐数</summary>
        /// <exception cref="PropertyOutOfRangeException">指定範囲以外の値をセットした場合</exception>
        public int CaseValue
        {
            get => conditionList.ConditionValue;
            set
            {
                if (value < 1 || 15 < value)
                    throw new PropertyOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(CaseValue), 1, 15, value));
                conditionList.ConditionValue = value;
            }
        }

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
            if (VersionConfig.IsUnderVersion(WoditorVersion.Ver2_00))
            {
                OutputVersionWarningLogIfNeed_UnderVer2_00();
            }
        }

        /// <summary>
        /// 設定バージョン = 2.00未満 の場合の警告
        /// </summary>
        private void OutputVersionWarningLogIfNeed_UnderVer2_00()
        {
            for (var i = 0; i < conditionList.ConditionValue; i++)
            {
                var con = conditionList[i];
                if (con.Condition == StringConditionalOperator.StartWith)
                {
                    Logger.Warning(VersionWarningMessage.NotUnderInCommandSetting(
                        $"{nameof(ConditionStringStart)}.{nameof(conditionList)}[{i}]",
                        $"{StringConditionalOperator.StartWith}",
                        VersionConfig.GetConfigWoditorVersion(),
                        WoditorVersion.Ver2_00));
                }
            }
        }
    }
}