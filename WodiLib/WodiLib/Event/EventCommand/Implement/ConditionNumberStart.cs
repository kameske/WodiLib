// ========================================
// Project Name : WodiLib
// File Name    : ConditionNumberStart.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;
using System.Linq;
using WodiLib.Project;
using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <inheritdoc />
    /// <summary>
    /// イベントコマンド・条件（変数）・始端
    /// </summary>
    [Serializable]
    public class ConditionNumberStart : EventCommandBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private const string EventCommandSentenceFormatMain
            = "■条件分岐(変数): {0}";

        private const string EventCommandSentenceFormatForkConditionForMe
            = "{0}  が   {1} {2} ";

        private const string EventCommandSentenceFormatForkConditionForBranch
            = "{0}  が  {1} {2}";

        private const string EventCommandSentenceFormatFork
            = " 【{0}】 {1}";

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override EventCommandCode EventCommandCode => EventCommandCode.ConditionNumberStart;

        /// <inheritdoc />
        public override byte NumberVariableCount => (byte) (2 + ConditionList.Count * 3);

        /// <inheritdoc />
        public override byte StringVariableCount => 0x00;

        /// <inheritdoc />
        /// <summary>数値変数最小個数</summary>
        public override byte NumberVariableCountMin => 0x05;

        /// <inheritdoc />
        /// <summary>
        /// インデックスを指定して数値変数を取得する。
        /// ウディタ標準仕様でサポートしているインデックスのみ取得可能。
        /// </summary>
        /// <param name="index">[Range(0, 4~46)] インデックス</param>
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
                    byte[] bytes =
                    {
                        (byte) ConditionList.Count, 0x00, 0x00, 0x00
                    };
                    if (IsElseCase) bytes[0] += 0x10;
                    return bytes.ToInt32(Endian.Environment);

                default:
                    // 選択肢1~15のLeftSide/RightSide/ConditionFlag

                    var exception = new ArgumentOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(index), 0, NumberVariableCount - 1, index));

                    var caseNum = (index - 2) / 3;
                    if (caseNum >= ConditionList.Count) throw exception;

                    var propNum = (index - 2) % 3;

                    switch (propNum)
                    {
                        case 0:
                            return ConditionList[caseNum].LeftSide;
                        case 1:
                            return ConditionList[caseNum].RightSide;
                        default: // case 2
                            return ConditionList[caseNum].ToConditionFlag();
                    }
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// 数値変数を設定する。
        /// </summary>
        /// <param name="index">[Range(1, 1)] インデックス</param>
        /// <param name="value">設定値</param>
        /// <exception cref="ArgumentOutOfRangeException">indexが指定範囲以外</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public override void SetSafetyNumberVariable(int index, int value)
        {
            switch (index)
            {
                case 1:
                {
                    var bytes = value.ToBytes(Endian.Environment);
                    IsElseCase = (bytes[0] & 0xF0) != 0;
                    ConditionList.AdjustLength(bytes[0] & 0x0F);
                    return;
                }
                default:
                    // 選択肢1~15のLeftSide/RightSide/ConditionFlag

                    var exception = new ArgumentOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(index), 0, NumberVariableCount - 1, index));

                    var caseNum = (index - 2) / 3;
                    if (caseNum >= ConditionList.Count) throw exception;

                    var propNum = (index - 2) % 3;

                    switch (propNum)
                    {
                        case 0:
                            ConditionList[caseNum].LeftSide = value;
                            return;
                        case 1:
                            ConditionList[caseNum].RightSide = value;
                            return;
                        default: // case 2
                        {
                            var bytes = value.ToBytes(Endian.Environment);
                            ConditionList[caseNum].IsNotReferX = (bytes[0] & 0xF0) != 0;
                            ConditionList[caseNum].Condition =
                                NumberConditionalOperator.FromByte((byte) (bytes[0] & 0x0F));
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
            if (desc is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(desc)));

            var forkStringsList = ConditionList.Select((x, idx) =>
            {
                var leftVarName = resolver.GetNumericVariableAddressStringIfVariableAddress(x.LeftSide, type, desc);
                var rightVarName = x.IsNotReferX
                    ? x.RightSide.ToString()
                    : resolver.GetNumericVariableAddressStringIfVariableAddress(x.RightSide, type, desc);
                var myStr = string.Format(EventCommandSentenceFormatForkConditionForMe,
                    leftVarName, rightVarName, x.Condition.EventCommandSentence);
                var branchStr = string.Format(EventCommandSentenceFormatForkConditionForBranch,
                    leftVarName, rightVarName, x.Condition.EventCommandSentence);
                return (myStr, branchStr);
            }).ToList();

            desc.StartBranch(BranchType.ConditionNumber, forkStringsList.Select(x => x.branchStr).ToList());

            var forkStrList = forkStringsList.Select(x => x.myStr)
                .Select((x, idx) =>
                    string.Format(EventCommandSentenceFormatFork,
                        idx + 1, x)
                );

            return string.Format(EventCommandSentenceFormatMain,
                string.Join("", forkStrList));
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private bool isElseCase;

        /// <summary>「上記以外」分岐フラグ</summary>
        public bool IsElseCase
        {
            get => isElseCase;
            set
            {
                isElseCase = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>分岐リスト</summary>
        public ConditionNumberList ConditionList { get; } = new ConditionNumberList();

        /// <inheritdoc />
        protected override EventCommandColorSet EventCommandColorSet
            => EventCommandColorSet.Black;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     InnerNotifyChanged
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private void OnConditionListPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            switch (args.PropertyName)
            {
                case nameof(ConditionList.Count):
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
        public ConditionNumberStart()
        {
            ConditionList.PropertyChanged += OnConditionListPropertyChanged;
        }
    }
}