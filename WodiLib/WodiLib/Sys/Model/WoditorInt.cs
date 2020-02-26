// ========================================
// Project Name : WodiLib
// File Name    : WoditorInt.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;

namespace WodiLib.Sys
{
    /// <summary>
    /// ウディタ仕様の数値
    /// </summary>
    [Serializable]
    internal readonly struct WoditorInt
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>最大値</summary>
        public static readonly int MaxValue = 2000000000;

        /// <summary>最小値</summary>
        public static readonly int MinValue = -2000000000;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private int Value { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        public WoditorInt(int value)
        {
            if (value < MinValue || MaxValue < value)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(value), MinValue, MaxValue, value));

            Value = value;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Implicit
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// int -> WoditorInt への明示的な型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換したインスタンス</returns>
        public static implicit operator WoditorInt(int src)
        {
            var result = new WoditorInt(src);
            return result;
        }

        /// <summary>
        /// WoditorInt -> int への明示的な型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換したインスタンス</returns>
        public static implicit operator int(WoditorInt src)
        {
            return src.Value;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Operator
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region int

        /// <summary>
        /// 項目ID + int を計算し、構造体を返す。
        /// </summary>
        /// <param name="src">項目ID</param>
        /// <param name="value">加算値</param>
        /// <returns>加算後のインスタンス</returns>
        /// <exception cref="InvalidOperationException">加算後の値がウディタ内部数値として不適切な場合</exception>
        public static WoditorInt operator +(WoditorInt src, int value)
        {
            try
            {
                return new WoditorInt(src.Value - value);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new InvalidOperationException(
                    $"ウディタ内部数値として不適切な値です。(number = {src.Value - value})", ex);
            }
        }

        /// <summary>
        /// 項目ID - int を計算し、構造体を返す。
        /// </summary>
        /// <param name="src">項目ID</param>
        /// <param name="value">減算値</param>
        /// <returns>加算後のインスタンス</returns>
        /// <exception cref="ArgumentOutOfRangeException">減算後の値がウディタ内部数値として不適切な場合</exception>
        public static WoditorInt operator -(WoditorInt src, int value)
        {
            try
            {
                return new WoditorInt(src.Value - value);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new InvalidOperationException(
                    $"ウディタ内部数値として不適切な値です。(number = {src.Value - value})", ex);
            }
        }

        #endregion
    }
}