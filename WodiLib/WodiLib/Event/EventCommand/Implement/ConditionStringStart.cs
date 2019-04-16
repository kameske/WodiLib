// ========================================
// Project Name : WodiLib
// File Name    : ConditionStringStart.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;
using WodiLib.Sys;
using WodiLib.Sys.Cmn;

namespace WodiLib.Event.EventCommand
{
    /// <inheritdoc />
    /// <summary>
    /// イベントコマンド・条件（文字列）・始端
    /// </summary>
    public class ConditionStringStart : EventCommandBase
    {
        /// <summary>数値変数の数最大値</summary>
        private static readonly int NumberVariableCountMax = 10;

        /// <summary>代入コードシフト係数</summary>
        private static readonly int OperationCodeShift = 24;

        /// <summary>右辺種別シフト係数</summary>
        private static readonly int RightSideFlagShift = 24;

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
        public override byte StringVariableCount => 0x04;

        /// <inheritdoc />
        /// <summary>
        /// インデックスを指定して数値変数を取得する。
        /// </summary>
        /// <param name="index">[Range(0, 1～10)] インデックス</param>
        /// <returns>インデックスに対応した値</returns>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override int GetNumberVariable(int index)
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
                        var condition = conditionList.Get(tmpIndex);
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

                    var checkRightSide = conditionList.Get(tmpIndex).RightSide;
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
        /// <param name="index">[Range(1, 1～10)] インデックス</param>
        /// <param name="value">設定値</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override void SetNumberVariable(int index, int value)
        {
            if (index < 1 || NumberVariableCountMax < index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), 1, NumberVariableCount, index));
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

                        conditionList.SetLeftSide(tmpIndex, leftSide);
                        conditionList.SetCondition(tmpIndex, StringConditionalOperator.ForByte(ope));
                        conditionList.SetIsUseNumberVariable(tmpIndex, isVar);

                        return;
                    }

                    // 残りは右辺（正しいかどうかチェックが必要）
                    {
                        tmpIndex -= CaseValue;
                        if (tmpIndex >= CaseValue)
                        {
                            if (index < 1 || NumberVariableCountMax < index)
                                throw new ArgumentOutOfRangeException(
                                    ErrorMessage.OutOfRange(nameof(index), 1, NumberVariableCount, index));
                        }

                        conditionList.MergeRightSide(tmpIndex, value);
                        return;
                    }
                }
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// インデックスを指定して文字列変数を取得する。
        /// </summary>
        /// <param name="index">[Range(0, 4)] インデックス</param>
        /// <returns>インデックスに対応した値</returns>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override string GetStringVariable(int index)
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

            return conditionList.GetRightSideString(index);
        }

        /// <inheritdoc />
        /// <summary>
        /// 文字列変数を設定する。
        /// </summary>
        /// <param name="index">[Range(0, 3)] インデックス</param>
        /// <param name="value">[NotNull] 設定値</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外</exception>
        /// <exception cref="ArgumentNullException">valueがnull</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override void SetStringVariable(int index, string value)
        {
            if (index < 0 || StringVariableCount - 1 < index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), 0, StringVariableCount - 1, index));
            // 右辺文字列を書き換える
            conditionList.MergeRightSideNonCheckIndex(index, value);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>「上記以外」分岐フラグ</summary>
        public bool IsElseCase { get; set; }

        private readonly ConditionStringList conditionList = new ConditionStringList();

        /// <summary>[Range(1, 4)] 分岐数</summary>
        /// <exception cref="PropertyOutOfRangeException">指定範囲以外の値をセットした場合</exception>
        public int CaseValue
        {
            get => conditionList.ConditionValue;
            set
            {
                if (value < 1 || 4 < value)
                    throw new PropertyOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(CaseValue), 1, 4, value));
                conditionList.ConditionValue = value;
            }
        }

        /// <summary>[NotNull] 条件1</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public ConditionStringDesc Case1
        {
            get => conditionList.Get(0);
            set
            {
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(Case1)));
                conditionList.Set(0, value);
            }
        }

        /// <summary>[NotNull] 条件2</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public ConditionStringDesc Case2
        {
            get => conditionList.Get(1);
            set
            {
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(Case2)));
                conditionList.Set(1, value);
            }
        }

        /// <summary>[NotNull] 条件3</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public ConditionStringDesc Case3
        {
            get => conditionList.Get(2);
            set
            {
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(Case3)));
                conditionList.Set(2, value);
            }
        }

        /// <summary>[NotNull] 条件4</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public ConditionStringDesc Case4
        {
            get => conditionList.Get(3);
            set
            {
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(Case4)));
                conditionList.Set(3, value);
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
                var con = conditionList.Get(i);
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