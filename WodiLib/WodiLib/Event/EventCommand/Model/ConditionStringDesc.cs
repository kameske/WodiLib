// ========================================
// Project Name : WodiLib
// File Name    : ConditionStringDesc.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <summary>
    /// 条件（文字列）条件オブジェクトクラス
    /// </summary>
    public class ConditionStringDesc
    {
        /// <summary>左辺</summary>
        public int LeftSide { get; set; } = 3000000;

        private IntOrStr rightSide = "";

        /// <summary>[NotNull] 右辺</summary>
        /// <exception cref="PropertyNullException">nullをセットしようとした場合</exception>
        /// <exception cref="PropertyException">IntOrStrType.None をセットしようとした場合</exception>
        public IntOrStr RightSide
        {
            get => rightSide;
            set
            {
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(RightSide)));
                if (value.InstanceIntOrStrType == IntOrStrType.None)
                    throw new PropertyException(
                        ErrorMessage.NotEmpty(nameof(RightSide)));
                rightSide = value;
            }
        }

        private StringConditionalOperator condition = StringConditionalOperator.Equal;

        /// <summary>[NotNull] 条件演算子</summary>
        /// <exception cref="PropertyNullException">nullを設定しようとした場合</exception>
        public StringConditionalOperator Condition
        {
            get => condition;
            set
            {
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(Condition)));
                condition = value;
            }
        }

        /// <summary>数値変数使用フラグ</summary>
        public bool IsUseNumberVariable { get; set; }

        /// <summary>
        /// 右辺の文字列を返す。
        /// </summary>
        /// <returns>右辺文字列。右辺が数値指定の場合、空文字。</returns>
        public string GetRightSideString()
        {
            if (IsUseNumberVariable) return "";
            return RightSide.ToStr();
        }
    }
}