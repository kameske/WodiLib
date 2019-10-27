// ========================================
// Project Name : WodiLib
// File Name    : TileSetDataFilePath.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.IO;
using System.Text.RegularExpressions;
using WodiLib.Cmn;
using WodiLib.Sys;

namespace WodiLib.IO
{
    /// <summary>
    /// TileSetData.dat ファイルパス
    /// </summary>
    public class TileSetDataFilePath : FilePath
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>ファイルパスフォーマット</summary>
        public static readonly Regex FilePathRegex = new Regex(@"^TileSetData\.dat$", RegexOptions.IgnoreCase);

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Override Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>空文字許可フラグ</summary>
        protected override bool IsAllowEmptyString { get; } = false;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="value">[NotNull][NotNewLine] ファイルパス</param>
        /// <exception cref="ArgumentNullException">valueがnullの場合</exception>
        /// <exception cref="ArgumentNewLineException">
        ///     valueに改行が含まれる場合、
        ///     または255byteを超える場合
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     valueがファイルパスとして不適切な場合、
        ///     またはファイル名が"XXX.dat"ではない場合
        /// </exception>
        public TileSetDataFilePath(string value) : base(value)
        {
            var fileName = Path.GetFileName(value);
            if (fileName == null)
                throw new ArgumentException(
                    ErrorMessage.Unsuitable("ファイルパス", $"（パス：{value}）"));

            if (!FilePathRegex.IsMatch(fileName))
            {
                throw new ArgumentException(
                    $"ファイル名の形式は{FilePathRegex}でなければなりません。");
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Override Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// string に変換する。
        /// </summary>
        /// <returns>string値</returns>
        public override string ToString() => this;

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((TileSetDataFilePath) obj);
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
        public bool Equals(TileSetDataFilePath other)
        {
            if (other == null) return false;
            return Value.Equals(other.Value);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Implicit
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// string -> TileSetDataFilePath への暗黙的な型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換したインスタンス</returns>
        public static implicit operator TileSetDataFilePath(string src)
        {
            var result = new TileSetDataFilePath(src);
            return result;
        }

        /// <summary>
        /// TileSetDataFilePath -> string への暗黙的な型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換したインスタンス</returns>
        public static implicit operator string(TileSetDataFilePath src)
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
        public static bool operator ==(TileSetDataFilePath left, TileSetDataFilePath right)
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
        public static bool operator !=(TileSetDataFilePath left, TileSetDataFilePath right)
        {
            return !(left == right);
        }
    }
}