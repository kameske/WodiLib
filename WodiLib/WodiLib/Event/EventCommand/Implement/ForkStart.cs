// ========================================
// Project Name : WodiLib
// File Name    : ForkStart.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;
using System.Text;
using USEncoder;
using WodiLib.Project;
using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <inheritdoc />
    /// <summary>
    /// イベントコマンド・選択肢始端 または
    /// イベントコマンド・条件（変数/文字列）・分岐始端
    /// </summary>
    [Serializable]
    public class ForkStart : ForkingStartBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private const string EventCommandSentenceFormat_Choice
            = "-◇選択肢：【{0}】 {1} の場合↓";

        private const int EventCommandSentenceCaseMinLength = 40;

        private const string EventCommandSentenceFormat_ConditionNumber
            = "-◇分岐： 【{0}】  [ {1}  ]の場合↓";

        private const string EventCommandSentenceFormat_ConditionString
            = "-◇分岐： 【{0}】 [ {1} ]の場合↓";

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private const int CaseNumberMax = 14;
        private const int CaseNumberMin = 0;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override EventCommandCode EventCommandCode => EventCommandCode.ChoiceStartForkingNumber;

        /// <inheritdoc />
        /// <summary>[Range(0, 14)] 選択肢番号</summary>
        /// <exception cref="T:WodiLib.Sys.PropertyOutOfRangeException">指定範囲以外の値をセットした場合</exception>
        public override int CaseNumber
        {
            get => ChoiceCodeRaw - 2;
            set
            {
                if (value < CaseNumberMin
                    || CaseNumberMax < value)
                {
                    throw new PropertyOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(CaseNumber), CaseNumberMin, CaseNumberMax, value));
                }

                ChoiceCodeRaw = (byte) (value + 2);
            }
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

            var branchType = desc.CurrentBranchType;

            if (branchType == BranchType.Choice)
            {
                var caseString = GetCaseStr(desc, CaseNumber, true);
                return string.Format(EventCommandSentenceFormat_Choice,
                    CaseNumber + 1, caseString);
            }

            if (branchType == BranchType.ConditionNumber)
            {
                var caseNumber = CaseNumber + 1;
                var caseString = GetCaseStr(desc, caseNumber, false);
                return string.Format(EventCommandSentenceFormat_ConditionNumber,
                    caseNumber + 1, caseString);
            }

            if (branchType == BranchType.ConditionString)
            {
                var caseNumber = CaseNumber + 1;
                var caseString = GetCaseStr(desc, caseNumber, false);
                return string.Format(EventCommandSentenceFormat_ConditionString,
                    caseNumber + 1, caseString);
            }

            // 通常ここへは来ない
            throw new InvalidOperationException();
        }

        private string GetCaseStr(EventCommandSentenceResolveDesc desc, int caseNumber,
            bool isPadRight)
        {
            var str = desc.GetCase(caseNumber);

            var sb = new StringBuilder();
            sb.Append(str);

            if (isPadRight)
            {
                var correctLength = EventCommandSentenceCaseMinLength - ToEncoding.ToSJIS(str).Length;
                if (correctLength < 0) correctLength = 0;

                for (var i = 0; i < correctLength; i++)
                {
                    sb.Append(' ');
                }
            }

            return sb.ToString();
        }
    }
}