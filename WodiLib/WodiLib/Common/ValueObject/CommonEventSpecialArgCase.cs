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
    [Serializable]
    public class CommonEventSpecialArgCase : IEquatable<CommonEventSpecialArgCase>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>選択肢番号最大値</summary>
        public static readonly int CaseNumberMaxValue = WoditorInt.MaxValue;

        /// <summary>選択肢番号最小値</summary>
        public static readonly int CaseNumberMinValue = WoditorInt.MinValue;

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
            if (description is null)
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

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is CommonEventSpecialArgCase other && Equals(other);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                return (CaseNumber * 397) ^ (!(Description is null) ? Description.GetHashCode() : 0);
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public bool Equals(CommonEventSpecialArgCase other)
        {
            if (other is null) return false;
            return CaseNumber == other.CaseNumber
                   && Description.Equals(other.Description);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Implicit
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// Tuple&lt;int, string> -> CommonEventSpecialArgCase 型変換
        /// </summary>
        /// <param name="tuple">変換元</param>
        /// <returns>変換した値</returns>
        public static implicit operator CommonEventSpecialArgCase(Tuple<int, string> tuple)
        {
            if (tuple is null) return null;
            return new CommonEventSpecialArgCase(tuple.Item1, tuple.Item2);
        }

        /// <summary>
        /// (int, string) -> CommonEventSpecialArgCase 型変換
        /// </summary>
        /// <param name="tuple">変換元</param>
        /// <returns>変換した値</returns>
        public static implicit operator CommonEventSpecialArgCase(ValueTuple<int, string> tuple)
        {
            return new CommonEventSpecialArgCase(tuple.Item1, tuple.Item2);
        }

        /// <summary>
        /// CommonEventSpecialArgCase -> Tuple&lt;int, string> 型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換した値</returns>
        public static implicit operator Tuple<int, string>(CommonEventSpecialArgCase src)
        {
            if (src is null) return null;
            return new Tuple<int, string>(src.CaseNumber, src.Description);
        }

        /// <summary>
        /// CommonEventSpecialArgCase -> (int, string) 型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換した値</returns>
        /// <exception cref="InvalidCastException">
        ///     src が null の場合
        /// </exception>
        public static implicit operator ValueTuple<int, string>(CommonEventSpecialArgCase src)
        {
            if (src == null)
                throw new InvalidCastException(
                    ErrorMessage.InvalidCastFromNull(nameof(src), nameof(ValueTuple<int, string>)));

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
            if (ReferenceEquals(left, right)) return true;

            if (left is null || right is null) return false;

            return left.Equals(right);
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