// ========================================
// Project Name : WodiLib
// File Name    : CommonEventSpecialArgCase.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using WodiLib.Sys;

namespace WodiLib.Common
{
    /// <summary>
    /// コモンイベント特殊指定選択肢
    /// </summary>
    public struct CommonEventSpecialArgCase
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>選択肢番号最大値</summary>
        public static readonly int CaseNumberMaxValue = WoditorInt.MaxValue;

        /// <summary>選択肢番号最小値</summary>
        public static readonly int CaseNumberMinValue = WoditorInt.MinValue;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 空要素
        /// </summary>
        public static CommonEventSpecialArgCase Empty { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// [Range(-2000000000, 2000000000)] 選択肢番号
        /// </summary>
        public int CaseNumber { get; }

        /// <summary>
        /// [NotNull][NotNewLine] 選択肢文字列
        /// </summary>
        public string Description { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        static CommonEventSpecialArgCase()
        {
            Empty = default(CommonEventSpecialArgCase);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="caseNumber">[Range(-2000000000, 2000000000)] 選択肢番号</param>
        /// <param name="description">[NotNull][NotNewLine] 選択肢文字列</param>
        /// <exception cref="ArgumentOutOfRangeException">caseNumberが指定範囲外の場合</exception>
        /// <exception cref="ArgumentNullException">descriptionがnullの場合</exception>
        /// <exception cref="ArgumentNewLineException">descriptionに改行を含む場合</exception>
        public CommonEventSpecialArgCase(int caseNumber, string description)
        {
            if (caseNumber < CaseNumberMinValue || CaseNumberMaxValue < caseNumber)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(caseNumber), CaseNumberMinValue, CaseNumberMaxValue, caseNumber));
            if (description == null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(description)));
            if (description.HasNewLine())
                throw new ArgumentNewLineException(
                    ErrorMessage.NotNewLine(nameof(description), description));

            CaseNumber = caseNumber;
            Description = description;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Override Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override string ToString()
        {
            return $"CaseNumber = {CaseNumber}, Description = {Description}";
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is CommonEventSpecialArgCase other &&
                   (CaseNumber == other.CaseNumber
                    && Description.Equals(other.Description));
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                return (CaseNumber * 397) ^ (Description != null ? Description.GetHashCode() : 0);
            }
        }

        /// <summary>
        /// 自身が空オブジェクトかどうかを返す。
        /// </summary>
        /// <returns>空オブジェクトの場合true</returns>
        public bool IsEmpty()
        {
            return this == Empty;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Explicit
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// Tuple&lt;int, string> -> CommonEventSpecialArgCase 型変換
        /// </summary>
        /// <param name="tuple">変換元</param>
        /// <returns>変換した値</returns>
        public static explicit operator CommonEventSpecialArgCase(Tuple<int, string> tuple)
        {
            return new CommonEventSpecialArgCase(tuple.Item1, tuple.Item2);
        }

        /// <summary>
        /// (int, string) -> CommonEventSpecialArgCase 型変換
        /// </summary>
        /// <param name="tuple">変換元</param>
        /// <returns>変換した値</returns>
        public static explicit operator CommonEventSpecialArgCase(ValueTuple<int, string> tuple)
        {
            return new CommonEventSpecialArgCase(tuple.Item1, tuple.Item2);
        }

        /// <summary>
        /// CommonEventSpecialArgCase -> Tuple&lt;int, string> 型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換した値</returns>
        public static explicit operator Tuple<int, string>(CommonEventSpecialArgCase src)
        {
            return new Tuple<int, string>(src.CaseNumber, src.Description);
        }

        /// <summary>
        /// CommonEventSpecialArgCase -> (int, string) 型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換した値</returns>
        public static explicit operator ValueTuple<int, string>(CommonEventSpecialArgCase src)
        {
            return (src.CaseNumber, src.Description);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Operator
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// ==
        /// </summary>
        /// <param name="left">左辺</param>
        /// <param name="right">右辺</param>
        /// <returns>左辺と右辺の</returns>
        public static bool operator ==(CommonEventSpecialArgCase left, CommonEventSpecialArgCase right)
        {
            // 選択肢番号比較
            if (left.CaseNumber != right.CaseNumber) return false;

            // 文字列比較
            if (!string.Equals(left.Description, right.Description)) return false;

            return true;
        }

        /// <summary>
        /// !=
        /// </summary>
        /// <param name="left">左辺</param>
        /// <param name="right">右辺</param>
        /// <returns>左辺!=右辺の場合true</returns>
        public static bool operator !=(CommonEventSpecialArgCase left, CommonEventSpecialArgCase right)
        {
            return !(left == right);
        }
    }
}