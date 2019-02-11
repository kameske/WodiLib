// ========================================
// Project Name : WodiLib
// File Name    : MapEventBootCondition.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Event;
using WodiLib.Sys;

namespace WodiLib.Map
{
    /// <summary>
    ///     マップイベント起動条件実装クラス
    /// </summary>
    public class MapEventBootCondition
    {
        /// <summary>右辺最大値</summary>
        public static int RightSide_Max => 999999;

        /// <summary>右辺最小値</summary>
        public static int RightSide_Min => -999999;

        /// <summary>条件設定ONフラグ</summary>
        private static byte FlgHasCondition => 0x01;

        /// <summary>左辺</summary>
        public int LeftSide { get; set; } = 1000000;

        private CriteriaOperator operation = CriteriaOperator.Equal;

        /// <summary>[NotNull] 条件演算子</summary>
        public CriteriaOperator Operation
        {
            get => operation;
            set => operation = value ?? throw new PropertyNullException(ErrorMessage.NotNull(nameof(Operation)));
        }

        /// <summary>条件使用フラグ</summary>
        public bool UseCondition { get; set; }

        private int rightSide;

        /// <summary>[Range(-999999～999999)] 右辺</summary>
        /// <exception cref="PropertyOutOfRangeException">指定範囲外の値を設定した場合</exception>
        public int RightSide
        {
            get => rightSide;
            set
            {
                if (value < RightSide_Min || RightSide_Max < value)
                    throw new PropertyOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(RightSide), RightSide_Min, RightSide_Max, value));
                rightSide = value;
            }
        }

        /// <summary>
        ///     条件演算子＆使用フラグ用のbyte生成
        /// </summary>
        /// <returns>バイトデータ</returns>
        public byte MakeEventBootConditionByte()
        {
            byte result = 0x00;
            result += Operation.Code;
            result += UseCondition ? FlgHasCondition : (byte) 0x00;
            return result;
        }
    }
}