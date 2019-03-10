// ========================================
// Project Name : WodiLib
// File Name    : CommonEventBootCondition.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
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
        //     Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>右辺最大値</summary>
        public static int RightSide_Max => 999999;

        /// <summary>右辺最小値</summary>
        public static int RightSide_Min => -999999;

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

        /// <summary>左辺</summary>
        public int LeftSide { get; set; } = 1000000;

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