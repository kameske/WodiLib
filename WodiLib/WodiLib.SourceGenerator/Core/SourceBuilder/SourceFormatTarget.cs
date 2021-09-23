// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : SourceFormatTarget.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using System.Linq;

namespace WodiLib.SourceGenerator.Core.SourceBuilder
{
    /// <summary>
    ///     ソースコード文字列整形対象
    /// </summary>
    internal class SourceFormatTarget
    {
        /// <summary>空行</summary>
        public static SourceFormatTarget Empty { get; } = new("");

        /// <summary>整形対象文字列</summary>
        public string Text { get; }

        /// <summary>結合フラグ</summary>
        /// <remarks>
        ///     この値が <see langword="false"/> の場合、この情報は無視する必要がある。
        /// </remarks>
        public bool IsAppend { get; }

        /// <summary>
        ///     空行フラグ
        /// </summary>
        /// <remarks>
        ///     <see cref="string.Trim()"/> を行った結果で判定する。
        ///     <see cref="IsAppend"/> の状態によらず判定する。
        /// </remarks>
        public bool IsEmpty => Text.Trim().Equals("");

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="text">整形対象文字列</param>
        /// <param name="isAppend">結合フラグ</param>
        public SourceFormatTarget(string text, bool isAppend = true)
        {
            Text = text;
            IsAppend = isAppend;
        }

        /// <summary>
        ///     prefix を付与する。
        /// </summary>
        /// <param name="prefix">付与するprefix</param>
        /// <returns>処理結果</returns>
        public SourceFormatTarget AppendPrefix(string prefix)
            => new($"{prefix}{Text}", IsAppend);

        /// <summary>
        ///     suffix を付与する。
        /// </summary>
        /// <param name="suffix">付与するsuffix</param>
        /// <returns>処理結果</returns>
        public SourceFormatTarget AppendSuffix(string suffix)
            => new($"{Text}{suffix}", IsAppend);

        public static implicit operator SourceFormatTarget(string sentence) => new(sentence);

        public static implicit operator SourceFormatTarget((string sentence, bool isAppend) state) =>
            new(state.sentence, state.isAppend);

        /// <summary>
        ///     文字列リストからソースコード文字列背型情報を生成する。
        /// </summary>
        /// <param name="strings">対象文字列リスト</param>
        /// <returns><see cref="SourceFormatTarget"/> 配列</returns>
        public static SourceFormatTargetBlock FromStringLines(IEnumerable<string> strings)
            => strings.Select(line => (SourceFormatTarget)line).ToArray();
    }
}
