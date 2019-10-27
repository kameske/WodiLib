// ========================================
// Project Name : WodiLib
// File Name    : MapEventBootCondition.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Cmn;
using WodiLib.Event;
using WodiLib.Sys;

namespace WodiLib.Map
{
    /// <summary>
    ///     マップイベント起動条件実装クラス
    /// </summary>
    public class MapEventBootCondition
    {
        /// <summary>条件設定ONフラグ</summary>
        private static byte FlgHasCondition => 0x01;

        private static int DefaultValue => 1000000;

        /// <summary>
        ///     左辺
        /// </summary>
        public int LeftSide { get; set; } = DefaultValue;

        private CriteriaOperator operation = CriteriaOperator.Equal;

        /// <summary>[NotNull] 条件演算子</summary>
        public CriteriaOperator Operation
        {
            get => operation;
            set => operation = value ?? throw new PropertyNullException(ErrorMessage.NotNull(nameof(Operation)));
        }

        /// <summary>条件使用フラグ</summary>
        public bool UseCondition { get; set; }

        /// <summary>右辺</summary>
        public ConditionRight RightSide { get; set; }

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