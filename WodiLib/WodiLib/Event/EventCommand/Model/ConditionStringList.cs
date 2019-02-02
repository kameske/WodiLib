// ========================================
// Project Name : WodiLib
// File Name    : ConditionStringList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <summary>
    /// 条件（文字列）条件リスト
    /// </summary>
    public class ConditionStringList
    {
        /// <summary>条件最大数</summary>
        public static int LengthMax => 4;

        private int conditionValue;

        /// <summary>[Range(1, 4)] 条件数</summary>
        /// <exception cref="PropertyOutOfRangeException">1～4以外の値を設定しようとした場合</exception>
        public int ConditionValue
        {
            get => conditionValue;
            set
            {
                if (value < 1 || LengthMax < value)
                    throw new PropertyOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(ConditionValue), 1, 4, value));
                conditionValue = value;
            }
        }

        private readonly List<ConditionStringDesc> list = new List<ConditionStringDesc>
        {
            new ConditionStringDesc(), new ConditionStringDesc(),
            new ConditionStringDesc(), new ConditionStringDesc()
        };

        /// <summary>
        /// 条件（文字列）を取得する。
        /// </summary>
        /// <param name="index">[Range(0, 条件数 - 1)] 条件番号</param>
        /// <returns>条件（文字列）</returns>
        /// <exception cref="ArgumentOutOfRangeException">indexに0～条件数以外の値を設定した場合</exception>
        public ConditionStringDesc Get(int index)
        {
            if (index < 0 || ConditionValue <= index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), 0, $"条件数-1(={ConditionValue - 1})", index));
            return list[index];
        }

        /// <summary>
        /// 条件（文字列）をセットする。
        /// </summary>
        /// <param name="index">[Range(0, 条件数 - 1)] 条件番号</param>
        /// <param name="src">[NotNull] 条件</param>
        /// <exception cref="ArgumentOutOfRangeException">indexに0～条件数以外の値を設定した場合</exception>
        /// <exception cref="ArgumentNullException">indexに0～条件数以外の値を設定した場合</exception>
        public void Set(int index, ConditionStringDesc src)
        {
            if (index < 0 || ConditionValue <= index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), 0, $"条件数-1(={ConditionValue - 1})", index));
            if (src == null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(src)));
            list[index] = src;
        }

        /// <summary>
        /// 条件左辺をセットする。
        /// </summary>
        /// <param name="index">[Range(0, 条件数 - 1)] 条件番号</param>
        /// <param name="leftSide">左辺</param>
        /// <exception cref="ArgumentOutOfRangeException">indexに0～条件数以外の値を設定した場合</exception>
        public void SetLeftSide(int index, int leftSide)
        {
            if (index < 0 || ConditionValue <= index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), 0, $"条件数-1(={ConditionValue - 1})", index));
            list[index].LeftSide = leftSide;
        }

        /// <summary>
        /// 条件右辺をセットする。
        /// </summary>
        /// <param name="index">[Range(0, 条件数 - 1)] 条件番号</param>
        /// <param name="rightSide">[NotNull] 右辺</param>
        /// <exception cref="ArgumentOutOfRangeException">indexに0～条件数以外の値を設定した場合</exception>
        /// <exception cref="ArgumentNullException">rightSideにnullを設定した場合</exception>
        public void SetRightSide(int index, IntOrStr rightSide)
        {
            if (index < 0 || ConditionValue <= index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), 0, $"条件数-1(={ConditionValue - 1})", index));
            if (rightSide == null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(rightSide)));
            list[index].RightSide.Merge(rightSide);
        }

        /// <summary>
        /// 条件比較演算子をセットする。
        /// </summary>
        /// <param name="index">[Range(0, 条件数 - 1)] 条件番号</param>
        /// <param name="condition">[NotNull] 比較演算子</param>
        /// <exception cref="ArgumentOutOfRangeException">indexに0～条件数以外の値を設定した場合</exception>
        /// <exception cref="ArgumentNullException">conditionにnullを設定した場合</exception>
        public void SetCondition(int index, StringConditionalOperator condition)
        {
            if (index < 0 || ConditionValue <= index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), 0, $"条件数-1(={ConditionValue - 1})", index));
            if (condition == null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(condition)));
            list[index].Condition = condition;
        }

        /// <summary>
        /// 右辺数値変数使用フラグをセットする。
        /// </summary>
        /// <param name="index">[Range(0, 条件数 - 1)] 条件番号</param>
        /// <param name="isUseNumberVariable">数値変数使用フラグ</param>
        /// <exception cref="ArgumentOutOfRangeException">indexに0～条件数以外の値を設定した場合</exception>
        public void SetIsUseNumberVariable(int index, bool isUseNumberVariable)
        {
            if (index < 0 || ConditionValue <= index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), 0, $"条件数-1(={ConditionValue - 1})", index));
            list[index].IsUseNumberVariable = isUseNumberVariable;
        }

        /// <summary>
        /// 条件右辺をマージする。
        /// </summary>
        /// <param name="index">[Range(0, 条件数 - 1)] 条件番号</param>
        /// <param name="rightSide">[NotNull] 右辺</param>
        /// <exception cref="ArgumentOutOfRangeException">indexに0～条件数以外の値を設定した場合</exception>
        /// <exception cref="ArgumentNullException">rightSideにnullを設定した場合</exception>
        public void MergeRightSide(int index, IntOrStr rightSide)
        {
            if (index < 0 || ConditionValue <= index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), 0, $"条件数-1(={ConditionValue - 1})", index));
            if (rightSide == null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(rightSide)));
            list[index].RightSide.Merge(rightSide);
        }

        /// <summary>
        /// 条件右辺をマージする。
        /// 条件数を無視して設定する内部処理用
        /// </summary>
        /// <param name="index">[Range(0, 3)] 条件番号</param>
        /// <param name="rightSide">[NotNull] 右辺</param>
        /// <exception cref="ArgumentOutOfRangeException">indexに0～3以外の値を設定した場合</exception>
        /// <exception cref="ArgumentNullException">rightSideにnullを設定した場合</exception>
        public void MergeRightSideNonCheckIndex(int index, IntOrStr rightSide)
        {
            if (index < 0 || LengthMax < index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), 0, 3, index));
            if (rightSide == null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(rightSide)));
            list[index].RightSide.Merge(rightSide);
        }

        /// <summary>
        /// 右辺に数値変数を設定している条件を探し、その最大のインデックスを返す。
        /// </summary>
        /// <returns>右辺に数値変数を設定している条件のインデックス最大値（0～4）</returns>
        public int SearchUseNumberVariableForRightSideMax()
        {
            for (var i = ConditionValue - 1; i >= 0; i--)
            {
                if (list[i].IsUseNumberVariable) return i + 1;
            }

            return 0;
        }
    }
}