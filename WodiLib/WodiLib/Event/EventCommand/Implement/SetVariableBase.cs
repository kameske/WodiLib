// ========================================
// Project Name : WodiLib
// File Name    : SetVariableBase.cs
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
    /// イベントコマンド・変数操作基底クラス
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class SetVariableBase : EventCommandBase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     OverrideMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override EventCommandCode EventCommandCode => EventCommandCode.SetVariable;

        /// <inheritdoc />
        public override byte StringVariableCount => 0x00;

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

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>左辺（代入先）</summary>
        public int LeftSide { get; set; }

        private NumberAssignmentOperator assignmentOperator = NumberAssignmentOperator.Assign;

        /// <summary>[NotNull] 代入演算子</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public NumberAssignmentOperator AssignmentOperator
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

        /// <summary>右辺その1</summary>
        public int RightSide1 { get; set; }

        private CalculateOperator calculateOperator = CalculateOperator.Addition;

        /// <summary>[NotNull] 計算演算子</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public CalculateOperator CalculateOperator
        {
            get => calculateOperator;
            set
            {
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(CalculateOperator)));
                calculateOperator = value;
            }
        }

        /// <summary>右辺その2</summary>
        public int RightSide2 { get; set; }

        /// <summary>計算結果を±999999以内に収める</summary>
        public bool IsRoundMillion { get; set; }

        /// <summary>実数計算</summary>
        public bool IsCalcReal { get; set; }

        /// <summary>左辺）X番の変数呼び出し</summary>
        public bool IsUseVariableXLeft { get; set; }

        /// <summary>右辺1）データを呼ばない</summary>
        public bool IsNotReferRight1 { get; set; }

        /// <summary>右辺1）X番の変数呼び出し</summary>
        public bool IsUseVariableXRight1 { get; set; }

        /// <summary>右辺2）データを呼ばない</summary>
        public bool IsNotReferRight2 { get; set; }

        /// <summary>右辺2）X番の変数呼び出し</summary>
        public bool IsUseVariableXRight2 { get; set; }

        /// <summary>連続変数操作フラグ</summary>
        public bool IsMultiTarget { get; set; }

        /// <summary>連続操作変数の数</summary>
        public int OperationSeqValue { get; set; }
    }
}