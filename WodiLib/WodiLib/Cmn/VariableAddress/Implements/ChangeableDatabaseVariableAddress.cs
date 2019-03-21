// ========================================
// Project Name : WodiLib
// File Name    : ChangeableDatabaseVariableAddress.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;

namespace WodiLib.Cmn
{
    /// <summary>
    /// [Range(1100000000, 1199999999)] 可変DB変数アドレス値
    /// </summary>
    public class ChangeableDatabaseVariableAddress : VariableAddress
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>最小値</summary>
        public static int MinValue => 1100000000;

        /// <summary>最大値</summary>
        public static int MaxValue => 1199999999;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Override Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        /// <summary>最小値</summary>
        protected override int _MinValue => MinValue;

        /// <inheritdoc />
        /// <summary>安全に使用できる最小値</summary>
        protected override int _SafetyMinValue => MinValue;

        /// <inheritdoc />
        /// <summary>安全に使用できる最大値</summary>
        protected override int _SafetyMaxValue => MaxValue;

        /// <inheritdoc />
        /// <summary>最大値</summary>
        protected override int _MaxValue => MaxValue;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="value">[Range(1100000000, 1199999999)] 変数アドレス値</param>
        /// <exception cref="ArgumentOutOfRangeException">valueが可変DB変数アドレス値として不適切な場合</exception>
        public ChangeableDatabaseVariableAddress(int value) : base(value)
        {
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Explicit
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// int -> ChangeableDatabaseVariableAddress への明示的な型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換したインスタンス</returns>
        public static explicit operator ChangeableDatabaseVariableAddress(int src)
        {
            var result = new ChangeableDatabaseVariableAddress(src);
            return result;
        }

        /// <summary>
        /// ChangeableDatabaseVariableAddress -> int への明示的な型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換したインスタンス</returns>
        public static explicit operator int(ChangeableDatabaseVariableAddress src)
        {
            return src.Value;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Operator
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region int

        /// <summary>
        /// 可変DB変数アドレス値 + int を計算し、構造体を返す。
        /// </summary>
        /// <param name="src">変数アドレス</param>
        /// <param name="value">加算値</param>
        /// <returns>加算後のインスタンス</returns>
        /// <exception cref="InvalidOperationException">加算後の値が可変DB変数アドレス値として不適切な場合</exception>
        public static ChangeableDatabaseVariableAddress operator +(ChangeableDatabaseVariableAddress src, int value)
        {
            try
            {
                return new ChangeableDatabaseVariableAddress(src.Value + value);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new InvalidOperationException(
                    $"可変DB変数アドレス値として不適切な値です。(value = {src.Value + value})", ex);
            }
        }

        /// <summary>
        /// 可変DB変数アドレス値 - int を計算し、構造体を返す。
        /// </summary>
        /// <param name="src">変数アドレス</param>
        /// <param name="value">減算値</param>
        /// <returns>減算後のインスタンス</returns>
        /// <exception cref="InvalidOperationException">減算後の値が可変DB変数アドレス値値として不適切な場合</exception>
        public static ChangeableDatabaseVariableAddress operator -(ChangeableDatabaseVariableAddress src, int value)
        {
            try
            {
                return new ChangeableDatabaseVariableAddress(src.Value - value);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new InvalidOperationException(
                    $"可変DB変数アドレス値として不適切な値です。(value = {src.Value - value})", ex);
            }
        }

        #endregion

        #region ChangeableDatabaseVariableAddress

        /// <summary>
        /// 可変DB変数アドレス値 - 可変DB変数アドレス値 を計算し、アドレス値の差を返す。
        /// </summary>
        /// <param name="left">可変DB変数アドレス左辺</param>
        /// <param name="right">可変DB変数アドレス右辺</param>
        /// <returns>可変DB変数アドレス値の差</returns>
        public static int operator -(ChangeableDatabaseVariableAddress left, ChangeableDatabaseVariableAddress right)
        {
            return left.Value - right.Value;
        }

        #endregion
    }
}