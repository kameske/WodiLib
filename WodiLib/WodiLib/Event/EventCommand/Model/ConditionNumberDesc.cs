// ========================================
// Project Name : WodiLib
// File Name    : ConditionNumberDesc.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <summary>
    /// 条件（変数）条件オブジェクトクラス
    /// </summary>
    public class ConditionNumberDesc
    {
        /// <summary>左辺</summary>
        public int LeftSide { get; set; }

        /// <summary>右辺</summary>
        public int RightSide { get; set; }

        private NumberConditionalOperator condition = NumberConditionalOperator.Equal;

        /// <summary>[NotNull] 条件演算子</summary>
        /// <exception cref="PropertyNullException">nullをセットしようとした場合</exception>
        public NumberConditionalOperator Condition
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

        /// <summary>条件右辺）データを呼ばない</summary>
        public bool IsNotReferX { get; set; }

        /// <summary>
        /// 右辺データ呼ばないフラグ＆比較演算子のバイトを返す。
        /// </summary>
        /// <returns>バイナリデータ</returns>
        public byte ToConditionFlag()
        {
            byte referXFlag = (byte) (IsNotReferX ? 0x10 : 0x00);
            return (byte) (referXFlag + Condition.Code);
        }
    }
}