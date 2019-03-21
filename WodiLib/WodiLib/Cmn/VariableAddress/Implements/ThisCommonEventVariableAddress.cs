// ========================================
// Project Name : WodiLib
// File Name    : ThisCommonEventVariableAddress.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;

namespace WodiLib.Cmn
{
    /// <summary>
    /// [Range(1600000, 1600099)] このコモンイベントセルフ変数アドレス値
    /// </summary>
    public class ThisCommonEventVariableAddress : VariableAddress
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>最小値</summary>
        public static int MinValue => 1600000;

        /// <summary>最大値</summary>
        public static int MaxValue => 1600099;

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
        /// <param name="value">[Range(1600000, 1600099)] 変数アドレス値</param>
        /// <exception cref="ArgumentOutOfRangeException">valueがこのコモンイベントセルフ変数アドレス値として不適切な場合</exception>
        public ThisCommonEventVariableAddress(int value) : base(value)
        {
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Explicit
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// int -> ThisCommonEventVariableAddress への明示的な型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換したインスタンス</returns>
        public static explicit operator ThisCommonEventVariableAddress(int src)
        {
            var result = new ThisCommonEventVariableAddress(src);
            return result;
        }

        /// <summary>
        /// ThisCommonEventVariableAddress -> int への明示的な型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換したインスタンス</returns>
        public static explicit operator int(ThisCommonEventVariableAddress src)
        {
            return src.Value;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Operator
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region int

        /// <summary>
        /// このコモンイベントセルフ変数アドレス値 + int を計算し、構造体を返す。
        /// </summary>
        /// <param name="src">変数アドレス</param>
        /// <param name="value">加算値</param>
        /// <returns>加算後のインスタンス</returns>
        /// <exception cref="InvalidOperationException">加算後の値がこのコモンイベントセルフ変数アドレス値として不適切な場合</exception>
        public static ThisCommonEventVariableAddress operator +(ThisCommonEventVariableAddress src, int value)
        {
            try
            {
                return new ThisCommonEventVariableAddress(src.Value + value);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new InvalidOperationException(
                    $"このコモンイベントセルフ変数アドレス値として不適切な値です。(value = {src.Value + value})", ex);
            }
        }

        /// <summary>
        /// このコモンイベントセルフ変数アドレス値 - int を計算し、構造体を返す。
        /// </summary>
        /// <param name="src">変数アドレス</param>
        /// <param name="value">減算値</param>
        /// <returns>減算後のインスタンス</returns>
        /// <exception cref="InvalidOperationException">減算後の値がこのコモンイベントセルフ変数アドレス値値として不適切な場合</exception>
        public static ThisCommonEventVariableAddress operator -(ThisCommonEventVariableAddress src, int value)
        {
            try
            {
                return new ThisCommonEventVariableAddress(src.Value - value);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new InvalidOperationException(
                    $"このコモンイベントセルフ変数アドレス値として不適切な値です。(value = {src.Value - value})", ex);
            }
        }

        #endregion

        #region ThisCommonEventVariableAddress

        /// <summary>
        /// このコモンイベントセルフ変数アドレス値 - このコモンイベントセルフ変数アドレス値 を計算し、アドレス値の差を返す。
        /// </summary>
        /// <param name="left">このコモンイベントセルフ変数アドレス左辺</param>
        /// <param name="right">このコモンイベントセルフ変数アドレス右辺</param>
        /// <returns>このコモンイベントセルフ変数アドレス値の差</returns>
        public static int operator -(ThisCommonEventVariableAddress left, ThisCommonEventVariableAddress right)
        {
            return left.Value - right.Value;
        }

        #endregion
    }
}