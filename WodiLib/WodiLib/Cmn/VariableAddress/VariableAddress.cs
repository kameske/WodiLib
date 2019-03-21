// ========================================
// Project Name : WodiLib
// File Name    : VariableAddress.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using WodiLib.Sys;
using WodiLib.Sys.Cmn;

namespace WodiLib.Cmn
{
    /// <summary>
    /// 変数アドレス値基底クラス
    /// </summary>
    public abstract class VariableAddress : IConvertibleInt
    {
        /**
         * 演算子をオーバーロードしたいため、インタフェースは使用しない
         */

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Abstract Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>最小値</summary>
        protected abstract int _MinValue { get; }

        /// <summary>安全に使用できる最小値</summary>
        protected abstract int _SafetyMinValue { get; }

        /// <summary>安全に使用できる最大値</summary>
        protected abstract int _SafetyMaxValue { get; }

        /// <summary>最大値</summary>
        protected abstract int _MaxValue { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Static Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>ロガー</summary>
        private static readonly WodiLibLogger Logger = WodiLibLogger.GetInstance();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>アドレス値</summary>
        protected int Value { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="value">
        ///     [Range(_MinValue, _MaxValue)]
        ///     [SafetyRange(_SafetyMinValue, _SafetyMaxValue)]
        ///     変数アドレス値
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">value変数アドレス値として不適切な場合</exception>
        protected VariableAddress(int value)
        {
            if( value < _MinValue || _MaxValue < value) throw new ArgumentOutOfRangeException(
                ErrorMessage.OutOfRange(nameof(value), _MinValue, _MaxValue, value));

            if (value < _SafetyMinValue || _SafetyMaxValue < value)
                Logger.Warning(
                    WarningMessage.OutOfRange(nameof(value), _SafetyMinValue, _SafetyMaxValue, value));

            Value = value;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// int に変換する。
        /// </summary>
        /// <returns>int値</returns>
        public virtual int ToInt() => (int) this;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Explicit
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// int -> VariableAddress への明示的な型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換したインスタンス</returns>
        public static explicit operator VariableAddress(int src)
        {
            return VariableAddressFactory.Create(src);
        }

        /// <summary>
        /// VariableAddress -> int への明示的な型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換したインスタンス</returns>
        public static explicit operator int(VariableAddress src)
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
        public static VariableAddress operator +(VariableAddress src, int value)
        {
            try
            {
                return (VariableAddress)(src.Value + value);
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
        public static VariableAddress operator -(VariableAddress src, int value)
        {
            try
            {
                return (VariableAddress)(src.Value - value);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new InvalidOperationException(
                    $"可変DB変数アドレス値として不適切な値です。(value = {src.Value - value})", ex);
            }
        }

        #endregion

        #region VariableAddress

        /// <summary>
        /// 可変DB変数アドレス値 - 可変DB変数アドレス値 を計算し、アドレス値の差を返す。
        /// </summary>
        /// <param name="left">可変DB変数アドレス左辺</param>
        /// <param name="right">可変DB変数アドレス右辺</param>
        /// <returns>可変DB変数アドレス値の差</returns>
        public static int operator -(VariableAddress left, VariableAddress right)
        {
            return left.Value - right.Value;
        }

        #endregion


    }
}