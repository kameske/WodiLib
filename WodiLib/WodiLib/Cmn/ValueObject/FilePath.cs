// ========================================
// Project Name : WodiLib
// File Name    : FilePath.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.IO;
using WodiLib.Sys;

namespace WodiLib.Cmn
{
    /// <summary>
    /// [NotNull][NotNewLine] ファイルパス
    /// </summary>
    [Serializable]
    public class FilePath : IConvertibleString, IEquatable<FilePath>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>ファイル名最大長</summary>
        public static int MaxLength = 260;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>デフォルトの空文字許可フラグ</summary>
        private const bool DefaultAllowEmptyStringFlag = true;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>ファイル名</summary>
        protected string Value { get; }

        /// <summary>空文字許可フラグ</summary>
        protected virtual bool IsAllowEmptyString => DefaultAllowEmptyStringFlag;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="value">[NotNull][NotNewLine] ファイル名</param>
        /// <exception cref="ArgumentNullException">valueがnullの場合</exception>
        /// <exception cref="ArgumentNewLineException">
        ///     valueに改行が含まれる場合、
        ///     または255byteを超える場合
        /// </exception>
        /// <exception cref="ArgumentException">valueがファイル名として不適切な場合</exception>
        public FilePath(string value)
        {
            if (value is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(value)));
            if (value.HasNewLine())
                throw new ArgumentNewLineException(
                    ErrorMessage.NotNewLine(nameof(value), value));

            // 空文字チェック
            if (value.IsEmpty())
            {
                if (!IsAllowEmptyString)
                    throw new ArgumentException(
                        ErrorMessage.Unsuitable("ファイル名", $"（パス：{value}）"));

                // 空文字許可の場合、これ以上のチェック不要
                Value = value;
                return;
            }

            var woditorString = new WoditorString(value);

            if (woditorString.ByteLength > MaxLength)
                throw new ArgumentException(
                    ErrorMessage.OverDataSize(MaxLength));

            // フルパスチェック
            try
            {
                var _ = Path.GetFullPath(value);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(
                    ErrorMessage.Unsuitable("ファイル名", $"（パス：{value}）"), ex);
            }

            // ファイル名が適切かどうかチェック
            var fileName = Path.GetFileName(value);
            if (fileName.HasInvalidFileNameChars())
            {
                throw new ArgumentException(
                    ErrorMessage.Unsuitable("ファイル名", $"（パス：{value}）"));
            }

            Value = value;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Override Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// string に変換する。
        /// </summary>
        /// <returns>string値</returns>
        public override string ToString() => Value;

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((FilePath) obj);
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
        public bool Equals(FilePath other)
        {
            if (other is null) return false;
            return Value.Equals(other.Value);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Implicit
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// string -> FilePath への暗黙的な型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換したインスタンス</returns>
        public static implicit operator FilePath(string src)
        {
            if (src is null) return null;
            var result = new FilePath(src);
            return result;
        }

        /// <summary>
        /// FilePath -> string への暗黙的な型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換したインスタンス</returns>
        public static implicit operator string(FilePath src)
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
        public static bool operator ==(FilePath left, FilePath right)
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
        public static bool operator !=(FilePath left, FilePath right)
        {
            return !(left == right);
        }
    }
}