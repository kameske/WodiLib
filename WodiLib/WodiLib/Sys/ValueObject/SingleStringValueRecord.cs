// ========================================
// Project Name : WodiLib
// File Name    : SingleStringValueRecord.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Text;
using System.Text.RegularExpressions;
using WodiLib.Sys.Cmn;

namespace WodiLib.Sys
{
    /// <summary>
    ///     単一文字列レコード
    /// </summary>
    public abstract record SingleStringValueRecord<T> : IComparable<T>
        where T : SingleStringValueRecord<T>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     空文字許容フラグ
        /// </summary>
        protected virtual bool IsAllowEmpty => false;

        /// <summary>
        ///     改行コード許容フラグ
        /// </summary>
        protected virtual bool IsAllowNewLine => false;

        /// <summary>
        ///     必須文字列の正規表現
        /// </summary>
        protected virtual Regex? RequireRegex => null;

        /// <summary>
        ///     文字列データ（Shift-JIS）の必須サイズ最大
        /// </summary>
        protected virtual int? RequireSJisByteLengthMax => null;

        /// <summary>
        ///     推奨する文字列の正規表現
        /// </summary>
        protected virtual Regex? SafetyRegex => null;

        /// <summary>
        ///     値
        /// </summary>
        protected string Value { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private Encoding? shiftJisEncoding;

        /// <summary>
        ///     Shift-Jis エンコーディング
        /// </summary>
        private Encoding ShiftJisEncoding
        {
            get
            {
                if (shiftJisEncoding is not null) return shiftJisEncoding;

                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                shiftJisEncoding = Encoding.GetEncoding("shift-jis");

                return shiftJisEncoding;
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="value">文字列値</param>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> が <see langword="null"/> の場合</exception>
        /// <exception cref="ArgumentException">
        ///     <see cref="IsAllowEmpty"/>が <see langword="false"/> かつ <paramref name="value"/> が空文字の場合、
        ///     または <paramref name="value"/> が <see cref="RequireRegex"/>を満たさない場合、
        ///     または <paramref name="value"/> をShift-JIS に変換した際のデータサイズが
        ///     <see cref="RequireSJisByteLengthMax"/> を超える場合
        /// </exception>
        /// <exception cref="ArgumentNewLineException">
        ///     <see cref="IsAllowNewLine"/> が <see langword="false"/>
        ///     かつ <paramref name="value"/> に改行コードが含まれる場合
        /// </exception>
        protected SingleStringValueRecord(string value)
        {
            #region Validation

            ThrowHelper.ValidateArgumentNotNull(value is null, nameof(value));

            if (value.IsEmpty())
            {
                ThrowHelper.ValidateArgumentNotEmpty(!IsAllowEmpty, nameof(value));

                // 空文字許可の場合、これ以上のチェック不要
                Value = value;
                return;
            }

            ThrowHelper.ValidateArgumentNotNewLine(
                !IsAllowNewLine && value.HasNewLine(), nameof(value), value);

            ThrowHelper.ValidateArgumentNotRegex(
                !RequireRegex?.IsMatch(value) ?? false, value, RequireRegex!);

            var shiftJisLength = ShiftJisEncoding.GetBytes(value).Length;
            var byteLengthMax = RequireSJisByteLengthMax ?? int.MaxValue;
            ThrowHelper.ValidateOverDataSize(
                shiftJisLength > byteLengthMax, byteLengthMax);

            #endregion

            #region Warning

            if (!SafetyRegex?.IsMatch(value) ?? false)
            {
                var logger = WodiLibLogger.GetInstance();
                logger.Warning(
                    WarningMessage.NotMatchRegex(value, SafetyRegex!));
            }

            #endregion

            Value = value;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc/>
        public override string ToString()
            => Value;

        /// <inheritdoc/>
        public int CompareTo(T? other)
            => string.Compare(Value, other?.Value, StringComparison.Ordinal);
    }
}
