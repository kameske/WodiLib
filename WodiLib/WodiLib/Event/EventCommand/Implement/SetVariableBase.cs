// ========================================
// Project Name : WodiLib
// File Name    : SetVariableBase.cs
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
    /// イベントコマンド・変数操作基底クラス
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class SetVariableBase : EventCommandBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private const string EventCommandSentenceFormat = "■変数操作: {3}{0} {1} {2} ";

        private const string EventCommandSentenceRightSideAngle = "X:  {0}   Y:  {1}";
        private const string EventCommandSentenceRightSideEtc = "{0} {1} {2}";

        private const string EventCommandSentenceRound = "リ";
        private const string EventCommandSentenceNotRound = "";

        private const string EventCommandSentenceReal = "実";
        private const string EventCommandSentenceNotReal = "";

        private const string EventCommandSentenceLeftHasOption = "[{0}{1}] ";
        private const string EventCommandSentenceLeftNonOption = "";

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override EventCommandCode EventCommandCode => EventCommandCode.SetVariable;

        /// <inheritdoc />
        public override byte StringVariableCount => 0x00;

        /// <inheritdoc />
        protected override EventCommandColorSet EventCommandColorSet
            => EventCommandColorSet.DeepRed;

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
            var leftSideStr = MakeLeftSideStr(resolver, type, desc);
            var rightSideStr = MakeRightSideStr(resolver, type, desc);
            var optionStr = MakeOptionString();

            return string.Format(EventCommandSentenceFormat,
                leftSideStr, AssignmentOperator.EventCommandSentence, rightSideStr,
                optionStr);
        }

        /// <summary>
        /// イベントコマンド文字列の左辺部分を生成する。
        /// </summary>
        /// <param name="resolver">[NotNull] 名前解決クラスインスタンス</param>
        /// <param name="type">[NotNull] イベント種別</param>
        /// <param name="desc">[Nullable] 付加情報</param>
        /// <returns>イベントコマンド文字列の左辺部分</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected abstract string MakeLeftSideStr(EventCommandSentenceResolver resolver,
            EventCommandSentenceType type, EventCommandSentenceResolveDesc desc);

        private string MakeRightSideStr(EventCommandSentenceResolver resolver,
            EventCommandSentenceType type, EventCommandSentenceResolveDesc desc)
        {
            string rightSideStr1;
            if (IsNotReferRight1) rightSideStr1 = RightSide1.ToString();
            else
            {
                rightSideStr1 = resolver.GetVariableAddressStringIfVariableAddress(RightSide1, type, desc);
                if (IsUseVariableXRight1) rightSideStr1 = $"V[{rightSideStr1}]";
            }

            string rightSideStr2;
            if (IsNotReferRight2) rightSideStr2 = RightSide2.ToString();
            else
            {
                rightSideStr2 = resolver.GetVariableAddressStringIfVariableAddress(RightSide2, type, desc);
                if (IsUseVariableXRight2) rightSideStr2 = $"V[{rightSideStr2}]";
            }

            if (AssignmentOperator == NumberAssignmentOperator.Angle)
            {
                return string.Format(EventCommandSentenceRightSideAngle,
                    rightSideStr1, rightSideStr2);
            }

            return string.Format(EventCommandSentenceRightSideEtc,
                rightSideStr1, CalculateOperator.EventCommandSentence, rightSideStr2);
        }

        private string MakeOptionString()
        {
            if (!IsRoundMillion && !IsCalcReal)
            {
                return EventCommandSentenceLeftNonOption;
            }

            var roundStr = IsRoundMillion
                ? EventCommandSentenceRound
                : EventCommandSentenceNotRound;
            var realStr = IsCalcReal
                ? EventCommandSentenceReal
                : EventCommandSentenceNotReal;

            return string.Format(EventCommandSentenceLeftHasOption,
                roundStr, realStr);
        }

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

        private NumberAssignmentOperator assignmentOperator = NumberAssignmentOperator.Assign;

        /// <summary>[NotNull] 代入演算子</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public NumberAssignmentOperator AssignmentOperator
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

        private int rightSide1;

        /// <summary>右辺その1</summary>
        public int RightSide1
        {
            get => rightSide1;
            set
            {
                rightSide1 = value;
                NotifyPropertyChanged();
            }
        }

        private CalculateOperator calculateOperator = CalculateOperator.Addition;

        /// <summary>[NotNull] 計算演算子</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public CalculateOperator CalculateOperator
        {
            get => calculateOperator;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(CalculateOperator)));
                calculateOperator = value;
                NotifyPropertyChanged();
            }
        }

        private int rightSide2;

        /// <summary>右辺その2</summary>
        public int RightSide2
        {
            get => rightSide2;
            set
            {
                rightSide2 = value;
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

        private bool isCalcReal;

        /// <summary>実数計算</summary>
        public bool IsCalcReal
        {
            get => isCalcReal;
            set
            {
                isCalcReal = value;
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

        private bool isNotReferRight1;

        /// <summary>右辺1）データを呼ばない</summary>
        public bool IsNotReferRight1
        {
            get => isNotReferRight1;
            set
            {
                isNotReferRight1 = value;
                NotifyPropertyChanged();
            }
        }

        private bool isUseVariableXRight1;

        /// <summary>右辺1）X番の変数呼び出し</summary>
        public bool IsUseVariableXRight1
        {
            get => isUseVariableXRight1;
            set
            {
                isUseVariableXRight1 = value;
                NotifyPropertyChanged();
            }
        }

        private bool isNotReferRight2;

        /// <summary>右辺2）データを呼ばない</summary>
        public bool IsNotReferRight2
        {
            get => isNotReferRight2;
            set
            {
                isNotReferRight2 = value;
                NotifyPropertyChanged();
            }
        }

        private bool isUseVariableXRight2;

        /// <summary>右辺2）X番の変数呼び出し</summary>
        public bool IsUseVariableXRight2
        {
            get => isUseVariableXRight2;
            set
            {
                isUseVariableXRight2 = value;
                NotifyPropertyChanged();
            }
        }

        private bool isMultiTarget;

        /// <summary>連続変数操作フラグ</summary>
        public bool IsMultiTarget
        {
            get => isMultiTarget;
            set
            {
                isMultiTarget = value;
                NotifyPropertyChanged();
            }
        }

        private int operationSeqValue;

        /// <summary>連続操作変数の数</summary>
        public int OperationSeqValue
        {
            get => operationSeqValue;
            set
            {
                operationSeqValue = value;
                NotifyPropertyChanged();
            }
        }
    }
}