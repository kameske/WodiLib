// ========================================
// Project Name : WodiLib
// File Name    : CommonEventBootCondition.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using WodiLib.Cmn;
using WodiLib.Event;
using WodiLib.Sys;

namespace WodiLib.Common
{
    /// <summary>
    /// コモンイベント起動条件実装クラス
    /// </summary>
    public class CommonEventBootCondition
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        private CommonEventBootType commonEventBootType = CommonEventBootType.OnlyCall;

        /// <summary>[NotNull] イベント起動タイプ</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public CommonEventBootType CommonEventBootType
        {
            get => commonEventBootType;
            set
            {
                if (value == null) throw new PropertyNullException(ErrorMessage.NotNull(nameof(CommonEventBootType)));
                commonEventBootType = value;
            }
        }

        private VariableAddress leftSide = (NormalNumberVariableAddress) 2000000;

        /// <summary>
        ///     [NotNull]
        ///     [Convertible(NormalNumberVariableAddress)]
        ///     [Convertible(SpareNumberVariableAddress)]
        ///     左辺
        /// </summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        /// <exception cref="InvalidCastException">
        ///     NormalNumberVariableAddressまたは
        ///     SpareNumberVariableAddressにキャストできない場合
        /// </exception>
        public VariableAddress LeftSide
        {
            get => leftSide;
            set
            {
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(LeftSide)));

                if (!(value is NormalNumberVariableAddress)
                    && !(value is SpareNumberVariableAddress))
                {
                    throw new InvalidCastException(
                        ErrorMessage.InvalidAnyCast(nameof(LeftSide),
                            nameof(NormalNumberVariableAddress),
                            nameof(SpareNumberVariableAddress)));
                }

                leftSide = value;
            }
        }

        private CriteriaOperator operation = CriteriaOperator.Equal;

        /// <summary>[NotNull] 条件演算子</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public CriteriaOperator Operation
        {
            get => operation;
            set
            {
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(Operation)));
                operation = value;
            }
        }

        /// <summary>右辺</summary>
        public ConditionRight RightSide { get; set; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Common
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// バイナリ変換する。
        /// </summary>
        /// <returns>バイナリデータ</returns>
        public byte[] ToBinary()
        {
            var result = new List<byte>();

            // 比較演算子 & 起動条件
            result.Add((byte) (Operation.Code + CommonEventBootType.Code));

            // 左辺
            result.AddRange(LeftSide.ToBytes(Endian.Woditor));

            // 右辺
            result.AddRange(RightSide.ToBytes(Endian.Woditor));

            return result.ToArray();
        }
    }
}