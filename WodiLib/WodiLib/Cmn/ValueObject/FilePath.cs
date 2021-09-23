// ========================================
// Project Name : WodiLib
// File Name    : FilePath.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using WodiLib.Sys;

namespace WodiLib.Cmn
{
    /// <summary>
    ///     [NotNewLine] ファイルパス
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [FilePathStringObjectValueAttribute]
    public partial class FilePath
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>デフォルトの空文字許可フラグ</summary>
        private const bool DefaultAllowEmptyStringFlag = true;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>空文字許可フラグ</summary>
        protected virtual bool IsAllowEmptyString => DefaultAllowEmptyStringFlag;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        partial void DoConstructorExpansion(string value)
        {
            // 空文字チェック
            if (value.IsEmpty())
            {
                if (!IsAllowEmptyString)
                    throw new ArgumentException(
                        ErrorMessage.Unsuitable("ファイル名", $"（パス：{value}）"));
                return;
            }

            // フルパスチェック
            //   .NET Standard 2.1 では一部の不正な文字列がパス中に含まれていても例外が発生しない
            //   そのためこのあとの処理を変更して対応
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
            var dirsAndFile = value.Split('\\');
            foreach (var path in dirsAndFile)
            {
                if (path.All(c => c.Equals('.'))) continue;

                var fileName = Path.GetFileName(path);
                if (fileName.IsEmpty()) continue;

                if (fileName.HasInvalidFileNameChars())
                {
                    throw new ArgumentException(
                        ErrorMessage.Unsuitable("ファイル名", $"（パス：{value}）"));
                }
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     ウディタ文字列のbyte配列に変換する。
        /// </summary>
        /// <returns>ウディタ文字列のbyte配列</returns>
        public IEnumerable<byte> ToWoditorStringBytes()
        {
            var woditorStr = new WoditorString(RawValue);
            return woditorStr.StringByte;
        }
    }
}
