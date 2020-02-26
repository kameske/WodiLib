// ========================================
// Project Name : WodiLib
// File Name    : CommonEventVariableIndex.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using WodiLib.Sys;

namespace WodiLib.Cmn
{
    /// <summary>
    /// [Range(0, 99)] コモンイベントセルフ変数インデックス
    /// </summary>
    [Serializable]
    public readonly struct CommonEventVariableIndex : IConvertibleInt, IEquatable<CommonEventVariableIndex>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>最大値</summary>
        public static readonly int MaxValue = 99;

        /// <summary>文字列セルフ変数インデックス最大値</summary>
        public static readonly int StringIndex_MaxValue = 9;

        /// <summary>文字列セルフ変数インデックス最小値</summary>
        public static readonly int StringIndex_MinValue = 5;

        /// <summary>最小値</summary>
        public static readonly int MinValue = 0;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>文字列フラグ</summary>
        public bool IsStringIndex { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>コモンイベントセルフ変数インデックス</summary>
        private int Value { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="value">[Range(0, 99)] コモンイベントセルフ変数インデックス</param>
        /// <exception cref="ArgumentOutOfRangeException">valueがコモンイベントセルフ変数インデックスとして不適切な場合</exception>
        public CommonEventVariableIndex(int value)
        {
            if (value < MinValue || MaxValue < value)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(value), MinValue, MaxValue, value));
            Value = value;

            IsStringIndex = StringIndex_MinValue <= value && value <= StringIndex_MaxValue;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Override Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override string ToString()
        {
            return Value.ToString();
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is CommonEventVariableIndex other && Equals(other);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return Value;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// int に変換する。
        /// </summary>
        /// <returns>int値</returns>
        public int ToInt() => this;

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public bool Equals(CommonEventVariableIndex other)
        {
            return Value == other.Value;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Implicit
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// int -> CommonEventVariableIndex への暗黙的な型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換したインスタンス</returns>
        public static implicit operator CommonEventVariableIndex(int src)
        {
            var result = new CommonEventVariableIndex(src);
            return result;
        }

        /// <summary>
        /// CommonEventVariableIndex -> int への暗黙的な型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換したインスタンス</returns>
        public static implicit operator int(CommonEventVariableIndex src)
        {
            return src.Value;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Operator
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// ==
        /// </summary>
        /// <param name="left">左辺</param>
        /// <param name="right">右辺</param>
        /// <returns>左辺==右辺の場合true</returns>
        public static bool operator ==(CommonEventVariableIndex left, CommonEventVariableIndex right)
        {
            return left.Value == right.Value;
        }

        /// <summary>
        /// !=
        /// </summary>
        /// <param name="left">左辺</param>
        /// <param name="right">右辺</param>
        /// <returns>左辺!=右辺の場合true</returns>
        public static bool operator !=(CommonEventVariableIndex left, CommonEventVariableIndex right)
        {
            return !(left == right);
        }
    }
}