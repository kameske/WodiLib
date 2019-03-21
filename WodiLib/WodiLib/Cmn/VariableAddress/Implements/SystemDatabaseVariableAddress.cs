// ========================================
// Project Name : WodiLib
// File Name    : SystemDatabaseVariableAddress.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;

namespace WodiLib.Cmn
{
    /// <summary>
    /// [Range(1300000000, 1399999999)]
    /// [SafetyRange(1300000000, 1399999920)]
    /// システムDB変数アドレス値
    /// </summary>
    public class SystemDatabaseVariableAddress : VariableAddress
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>最小値</summary>
        public static int MinValue => 1300000000;

        /// <summary>安全に使用できる最小値</summary>
        public static int SafetyMinValue => MinValue;

        /// <summary>安全に使用できる最大値</summary>
        public static int SafetyMaxValue => 1399999920;

        /// <summary>最大値</summary>
        public static int MaxValue => 1399999999;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Override Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        /// <summary>最小値</summary>
        protected override int _MinValue => MinValue;

        /// <inheritdoc />
        /// <summary>安全に使用できる最小値</summary>
        protected override int _SafetyMinValue => SafetyMinValue;

        /// <inheritdoc />
        /// <summary>安全に使用できる最大値</summary>
        protected override int _SafetyMaxValue => SafetyMaxValue;

        /// <inheritdoc />
        /// <summary>最大値</summary>
        protected override int _MaxValue => MaxValue;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="value">
        ///     [Range(1300000000, 1399999999)]
        ///     [SafetyRange(1300000000, 1399999920)]
        ///     変数アドレス値
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">valueがシステムDB変数アドレス値として不適切な場合</exception>
        public SystemDatabaseVariableAddress(int value) : base(value)
        {
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Explicit
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// int -> SystemDatabaseVariableAddress への明示的な型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換したインスタンス</returns>
        public static explicit operator SystemDatabaseVariableAddress(int src)
        {
            var result = new SystemDatabaseVariableAddress(src);
            return result;
        }

        /// <summary>
        /// SystemDatabaseVariableAddress -> int への明示的な型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換したインスタンス</returns>
        public static explicit operator int(SystemDatabaseVariableAddress src)
        {
            return src.Value;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Operator
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region int

        /// <summary>
        /// システムDB変数アドレス値 + int を計算し、構造体を返す。
        /// </summary>
        /// <param name="src">変数アドレス</param>
        /// <param name="value">加算値</param>
        /// <returns>加算後のインスタンス</returns>
        /// <exception cref="InvalidOperationException">加算後の値がシステムDB変数アドレス値として不適切な場合</exception>
        public static SystemDatabaseVariableAddress operator +(SystemDatabaseVariableAddress src, int value)
        {
            try
            {
                return new SystemDatabaseVariableAddress(src.Value + value);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new InvalidOperationException(
                    $"システムDB変数アドレス値として不適切な値です。(value = {src.Value + value})", ex);
            }
        }

        /// <summary>
        /// システムDB変数アドレス値 - int を計算し、構造体を返す。
        /// </summary>
        /// <param name="src">変数アドレス</param>
        /// <param name="value">減算値</param>
        /// <returns>減算後のインスタンス</returns>
        /// <exception cref="InvalidOperationException">減算後の値がシステムDB変数アドレス値値として不適切な場合</exception>
        public static SystemDatabaseVariableAddress operator -(SystemDatabaseVariableAddress src, int value)
        {
            try
            {
                return new SystemDatabaseVariableAddress(src.Value - value);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new InvalidOperationException(
                    $"システムDB変数アドレス値として不適切な値です。(value = {src.Value - value})", ex);
            }
        }

        #endregion

        #region SystemDatabaseVariableAddress

        /// <summary>
        /// システムDB変数アドレス値 - システムDB変数アドレス値 を計算し、アドレス値の差を返す。
        /// </summary>
        /// <param name="left">システムDB変数アドレス左辺</param>
        /// <param name="right">システムDB変数アドレス右辺</param>
        /// <returns>システムDB変数アドレス値の差</returns>
        public static int operator -(SystemDatabaseVariableAddress left, SystemDatabaseVariableAddress right)
        {
            return left.Value - right.Value;
        }

        #endregion
    }
}