// ========================================
// Project Name : WodiLib
// File Name    : ConditionNumberList.cs
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
    /// 条件（変数）条件リスト
    /// </summary>
    public class ConditionNumberList
    {
        private int conditionValue;

        /// <summary>[Range(1, 3)] 条件数</summary>
        /// <exception cref="PropertyOutOfRangeException">0～3以外の値をセットした場合</exception>
        public int ConditionValue
        {
            get => conditionValue;
            set
            {
                if (value < 1 || 3 < value) throw new PropertyOutOfRangeException();
                conditionValue = value;
            }
        }

        private readonly List<ConditionNumberDesc> list = new List<ConditionNumberDesc>
        {
            new ConditionNumberDesc(), new ConditionNumberDesc(),
            new ConditionNumberDesc()
        };

        /// <summary>
        /// 条件（変数）を取得する。
        /// </summary>
        /// <param name="index">[Range(0, ConditionValue - 1)] 条件番号</param>
        /// <returns>条件変数</returns>
        /// <exception cref="ArgumentOutOfRangeException">indexに0～条件数以外の値が指定された場合</exception>
        public ConditionNumberDesc Get(int index)
        {
            if (index < 0 || ConditionValue <= index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), 0, $"条件数-1(={ConditionValue - 1})", index));
            return list[index];
        }

        /// <summary>
        /// 条件（変数）を設定
        /// </summary>
        /// <param name="index">[Range(0, ConditionValue - 1)] 条件番号</param>
        /// <param name="src">[NotNull] 設定条件</param>
        /// <exception cref="ArgumentOutOfRangeException">indexに0～条件数以外の値が指定された場合</exception>
        /// <exception cref="ArgumentNullException">srcにnullが指定された場合</exception>
        public void Set(int index, ConditionNumberDesc src)
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
        /// 条件左辺を設定
        /// </summary>
        /// <param name="index">[Range(0, ConditionValue - 1)] 条件番号</param>
        /// <param name="leftSide">左辺</param>
        /// <exception cref="ArgumentOutOfRangeException">indexに0～条件数以外の値が指定された場合</exception>
        public void SetLeftSide(int index, int leftSide)
        {
            if (index < 0 || ConditionValue <= index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), 0, $"条件数-1(={ConditionValue - 1})", index));
            list[index].LeftSide = leftSide;
        }

        /// <summary>
        /// 条件右辺を設定
        /// </summary>
        /// <param name="index">[Range(0, ConditionValue - 1)] 条件番号</param>
        /// <param name="rightSide">左辺</param>
        /// <exception cref="ArgumentOutOfRangeException">indexに0～条件数以外の値が指定された場合</exception>
        public void SetRightSide(int index, int rightSide)
        {
            if (index < 0 || ConditionValue <= index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), 0, $"条件数-1(={ConditionValue - 1})", index));
            list[index].RightSide = rightSide;
        }

        /// <summary>
        /// 条件比較演算子を設定
        /// </summary>
        /// <param name="index">[Range(0, ConditionValue - 1)] 条件番号</param>
        /// <param name="condition">[NotNull] 比較演算子</param>
        /// <exception cref="ArgumentOutOfRangeException">indexに0～条件数以外の値が指定された場合</exception>
        public void SetCondition(int index, NumberConditionalOperator condition)
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
        /// 条件右辺）データを呼ばないフラグを設定
        /// </summary>
        /// <param name="index">[Range(0, ConditionValue - 1)] 条件番号</param>
        /// <param name="isNotReferX">件右辺）データを呼ばないフラグ</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void SetIsNotReferX(int index, bool isNotReferX)
        {
            if (index < 0 || ConditionValue <= index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), 0, $"条件数-1(={ConditionValue - 1})", index));
            list[index].IsNotReferX = isNotReferX;
        }
    }
}