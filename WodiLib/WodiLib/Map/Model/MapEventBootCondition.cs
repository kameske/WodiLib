// ========================================
// Project Name : WodiLib
// File Name    : MapEventBootCondition.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
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

        private VariableAddress leftSide = (VariableAddress) DefaultValue;

        /// <summary>
        ///     [Convertible(ThisMapEventVariableAddress)]
        ///     [Convertible(NormalNumberVariableAddress)]
        ///     [Convertible(SpareNumberVariableAddress)]
        ///     [Allow(1000000)]
        ///     左辺
        /// </summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        /// <exception cref="InvalidCastException">
        ///     ThisMapEventVariableAddress、
        ///     NormalNumberVariableAddressまたは
        ///     SpareNumberVariableAddressにキャストできない、
        ///     かつ 1000000 ではない場合
        /// </exception>
        public VariableAddress LeftSide
        {
            get => leftSide;
            set
            {
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(LeftSide)));

                switch (value)
                {
                    case ThisMapEventVariableAddress _:
                    case NormalNumberVariableAddress _:
                    case SpareNumberVariableAddress _:
                        break;

                    default:
                        if ((int) value != DefaultValue)
                        {
                            throw new InvalidCastException(
                                ErrorMessage.InvalidAnyCast(nameof(LeftSide),
                                    nameof(ThisMapEventVariableAddress),
                                    nameof(NormalNumberVariableAddress),
                                    nameof(SpareNumberVariableAddress),
                                    $"{DefaultValue}"));
                        }

                        break;
                }

                leftSide = value;
            }
        }

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