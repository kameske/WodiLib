// ========================================
// Project Name : WodiLib
// File Name    : CommonEventVariableAddress.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using WodiLib.Sys;

namespace WodiLib.Cmn
{
    /// <summary>
    /// [Range(15000000, 15999999)] コモンイベントセルフ変数アドレス値
    /// </summary>
    public class CommonEventVariableAddress : VariableAddress, IEquatable<CommonEventVariableAddress>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>最小値</summary>
        public static int MinValue => 15000000;

        /// <summary>最大値</summary>
        public static int MaxValue => 15999999;

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
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// セルフ変数インデックス
        /// </summary>
        public CommonEventVariableIndex Index { get; }

        /// <summary>
        /// 文字列変数フラグ
        /// </summary>
        public bool IsStringVariable => Index.IsStringIndex;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="value">[Range(15000000, 15999999)] 変数アドレス値</param>
        /// <exception cref="ArgumentOutOfRangeException">valueがコモンイベントセルフ変数アドレス値として不適切な場合</exception>
        public CommonEventVariableAddress(int value) : base(value)
        {
            Index = (CommonEventVariableIndex) value.SubInt(0, 2);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Override Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj is VariableAddress other) return Equals(other);
            return false;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public bool Equals(CommonEventVariableAddress other)
        {
            return other != null && Value == other.Value;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Explicit
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// int -> CommonEventVariableAddress への明示的な型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換したインスタンス</returns>
        public static explicit operator CommonEventVariableAddress(int src)
        {
            var result = new CommonEventVariableAddress(src);
            return result;
        }

        /// <summary>
        /// CommonEventVariableAddress -> int への明示的な型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換したインスタンス</returns>
        public static explicit operator int(CommonEventVariableAddress src)
        {
            return src.Value;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Operator
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region int

        /// <summary>
        /// コモンイベントセルフ変数アドレス値 + int を計算し、構造体を返す。
        /// </summary>
        /// <param name="src">変数アドレス</param>
        /// <param name="value">加算値</param>
        /// <returns>加算後のインスタンス</returns>
        /// <exception cref="InvalidOperationException">加算後の値がコモンイベントセルフ変数アドレス値として不適切な場合</exception>
        public static CommonEventVariableAddress operator +(CommonEventVariableAddress src, int value)
        {
            try
            {
                return new CommonEventVariableAddress(src.Value + value);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new InvalidOperationException(
                    $"コモンイベントセルフ変数アドレス値として不適切な値です。(value = {src.Value + value})", ex);
            }
        }

        /// <summary>
        /// コモンイベントセルフ変数アドレス値 - int を計算し、構造体を返す。
        /// </summary>
        /// <param name="src">変数アドレス</param>
        /// <param name="value">減算値</param>
        /// <returns>減算後のインスタンス</returns>
        /// <exception cref="InvalidOperationException">減算後の値がコモンイベントセルフ変数アドレス値値として不適切な場合</exception>
        public static CommonEventVariableAddress operator -(CommonEventVariableAddress src, int value)
        {
            try
            {
                return new CommonEventVariableAddress(src.Value - value);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new InvalidOperationException(
                    $"コモンイベントセルフ変数アドレス値として不適切な値です。(value = {src.Value - value})", ex);
            }
        }

        #endregion

        #region CommonEventVariableAddress

        /// <summary>
        /// コモンイベントセルフ変数アドレス値 - コモンイベントセルフ変数アドレス値 を計算し、アドレス値の差を返す。
        /// </summary>
        /// <param name="left">コモンイベントセルフ変数アドレス左辺</param>
        /// <param name="right">コモンイベントセルフ変数アドレス右辺</param>
        /// <returns>コモンイベントセルフ変数アドレス値の差</returns>
        public static int operator -(CommonEventVariableAddress left, CommonEventVariableAddress right)
        {
            return left.Value - right.Value;
        }

        /// <summary>
        /// ==
        /// </summary>
        /// <param name="left">左辺</param>
        /// <param name="right">右辺</param>
        /// <returns>左辺==右辺の場合true</returns>
        public static bool operator ==(CommonEventVariableAddress left, CommonEventVariableAddress right)
        {
            if (ReferenceEquals(left, right)) return true;

            if ((object) left == null || (object) right == null) return false;

            return left.Equals(right);
        }

        /// <summary>
        /// !=
        /// </summary>
        /// <param name="left">左辺</param>
        /// <param name="right">右辺</param>
        /// <returns>左辺!=右辺の場合true</returns>
        public static bool operator !=(CommonEventVariableAddress left, CommonEventVariableAddress right)
        {
            return !(left == right);
        }

        #endregion
    }
}