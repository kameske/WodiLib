// ========================================
// Project Name : WodiLib
// File Name    : AutoTileFileName.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using WodiLib.Sys;

namespace WodiLib.Map
{
    /// <summary>
    /// [NotNull][NotNewLine] オートタイルセットファイル名
    /// </summary>
    [Serializable]
    public class AutoTileFileName : IConvertibleString, IEquatable<AutoTileFileName>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>オートタイルセットファイル名</summary>
        private string Value { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="value">[NotNull][NotNewLine] オートタイルセットファイル名</param>
        /// <exception cref="ArgumentNullException">valueがnullの場合</exception>
        /// <exception cref="ArgumentNewLineException">valueが改行を含む場合</exception>
        public AutoTileFileName(string value)
        {
            if (value is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(value)));
            if (value.HasNewLine())
                throw new ArgumentNewLineException(
                    ErrorMessage.NotNewLine(nameof(value), value));

            Value = value;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// string に変換する。
        /// </summary>
        /// <returns>string値</returns>
        public override string ToString() => Value;

        /// <summary>
        /// ウディタ文字列のbyte配列に変換する。
        /// </summary>
        /// <returns>ウディタ文字列のbyte配列</returns>
        public IEnumerable<byte> ToWoditorStringBytes()
        {
            var woditorStr = new WoditorString(Value);
            return woditorStr.StringByte;
        }

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public bool Equals(AutoTileFileName other)
        {
            if (other is null) return false;
            return Value.Equals(other.Value);
        }

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public bool Equals(string other)
        {
            if (other is null) return false;
            return Value.Equals(other);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Implicit
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// int -> AutoTileFileName への暗黙的な型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換したインスタンス</returns>
        public static implicit operator AutoTileFileName(string src)
        {
            if (src is null) return null;
            var result = new AutoTileFileName(src);
            return result;
        }

        /// <summary>
        /// AutoTileFileName -> int への暗黙的な型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換したインスタンス</returns>
        public static implicit operator string(AutoTileFileName src)
        {
            return src?.Value;
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
        public static bool operator ==(AutoTileFileName left, AutoTileFileName right)
        {
            if (ReferenceEquals(left, right)) return true;

            if (left is null || right is null) return false;

            return left.Value == right.Value;
        }

        /// <summary>
        /// !=
        /// </summary>
        /// <param name="left">左辺</param>
        /// <param name="right">右辺</param>
        /// <returns>左辺!=右辺の場合true</returns>
        public static bool operator !=(AutoTileFileName left, AutoTileFileName right)
        {
            return !(left == right);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            switch (obj)
            {
                case string other:
                    return other == Value;
                case AutoTileFileName other:
                    return this == other;
                default:
                    return false;
            }
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}