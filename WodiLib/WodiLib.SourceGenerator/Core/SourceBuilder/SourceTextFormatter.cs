// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : SourceTextFormatter.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Linq;

namespace WodiLib.SourceGenerator.Core.SourceBuilder
{
    /// <summary>
    ///     ソースコード文字列整形処理
    /// </summary>
    internal static class SourceTextFormatter
    {
        /// <summary>
        ///     整形対象文字列を整形する。
        /// </summary>
        /// <param name="targets">整形対象</param>
        /// <returns>ソースコード文字列</returns>
        public static string Format(params SourceFormatTargetBlock[] targets)
            => string.Join(Environment.NewLine, FilteredTextMany(targets));

        /// <summary>
        ///     整形対象文字列を整形する。
        /// </summary>
        /// <param name="prefix">整形時に各行の先頭に付与する文字列</param>
        /// <param name="targets">整形対象</param>
        /// <returns>ソースコード文字列</returns>
        public static SourceFormatTargetBlock Format(string prefix, params SourceFormatTargetBlock[] targets)
            => FilteredTextMany(targets).Select(s => new SourceFormatTarget($"{prefix}{s}")).ToArray();

        /// <summary>
        ///     整形対象文字列を結合して整形情報を整える。
        /// </summary>
        /// <param name="prefix">成形時に各業の先頭に付与する文字列</param>
        /// <param name="separator">結合文字列</param>
        /// <param name="targets">整形対象</param>
        /// <returns>整形した<see cref="SourceFormatTarget"/> 配列</returns>
        public static SourceFormatTargetBlock Join(string prefix, string separator, SourceFormatTargetBlock targets)
        {
            var filtered = FilteredText(targets);

            var targetLen = filtered.Length;
            var result = new SourceFormatTarget[targetLen];

            var i = 0;
            var lastI = targetLen - 1;
            foreach (var item in filtered)
            {
                result[i] = new SourceFormatTarget($"{prefix}{item}{(i == lastI ? "" : separator)}");
                i++;
            }

            return result;
        }

        /// <summary>
        ///     整形対象文字列を結合して整形情報を整える。
        /// </summary>
        /// <param name="prefix">成形時に各業の先頭に付与する文字列</param>
        /// <param name="separator">結合文字列</param>
        /// <param name="targets">整形対象</param>
        /// <returns>整形した<see cref="SourceFormatTarget"/> 配列</returns>
        public static SourceFormatTargetBlock Reduce(string prefix, string separator,
            SourceFormatTargetBlock targets)
        {
            var filtered = FilteredText(targets);
            var resultLen = targets.Length;
            var result = new SourceFormatTarget[resultLen];

            var lastI = resultLen - 1;
            var i = 0;
            foreach (var line in filtered)
            {
                var isLastItem = i == lastI;

                result[i] = isLastItem
                    ? new SourceFormatTarget($"{prefix}{line}")
                    : new SourceFormatTarget($"{prefix}{line}{separator}");

                i++;
            }

            return result;
        }

        /// <summary>
        ///     整形対象文字列を結合して整形情報を整える。
        /// </summary>
        /// <param name="prefix">成形時に各業の先頭に付与する文字列</param>
        /// <param name="separator">結合文字列</param>
        /// <param name="targets">整形対象</param>
        /// <returns>整形した<see cref="SourceFormatTarget"/> 配列</returns>
        public static SourceFormatTargetBlock Reduce(string prefix, string separator,
            params SourceFormatTargetBlock[] targets)
        {
            var resultLen = targets.Length;
            var result = new SourceFormatTargetBlock[resultLen];

            var lastI = resultLen - 1;
            var i = 0;
            foreach (var block in targets)
            {
                var isLastItem = i == lastI;

                result[i] = isLastItem
                    ? block.AppendPrefixAllLine(prefix)
                    : block.AppendPrefixAllLine(prefix).AppendSuffixLastLine(separator);

                i++;
            }

            return SourceFormatTargetBlock.Merge(result);
        }

        /// <summary>
        ///     整形対象文字列を結合して整形情報を整える。
        /// </summary>
        /// <param name="prefix">成形時に各業の先頭に付与する文字列</param>
        /// <param name="targets">整形対象</param>
        /// <returns>整形した<see cref="SourceFormatTarget"/> 配列</returns>
        public static SourceFormatTargetBlock ReduceMany(string prefix,
            params SourceFormatTargetBlock[] targets)
            => Reduce(prefix, "", targets.SelectMany(line => line).ToArray());

        /// <summary>
        ///     整形対象文字列を結合して整形情報を整える。
        /// </summary>
        /// <param name="prefix">成形時に各業の先頭に付与する文字列</param>
        /// <param name="separator">結合文字列</param>
        /// <param name="targets">整形対象</param>
        /// <returns>整形した<see cref="SourceFormatTarget"/> 配列</returns>
        public static SourceFormatTargetBlock ReduceMany(string prefix, string separator,
            params SourceFormatTargetBlock[] targets)
            => Reduce(prefix, separator, targets.SelectMany(line => line).ToArray());

        /// <summary>
        ///     必要に応じて空文字に変換する。
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         <paramref name="isAppend"/> が <see langword="true"/> の場合、整形を行い、その結果を返す。
        ///     </para>
        ///     <para>
        ///         <paramref name="isAppend"/> が <see langword="true"/> の場合、空文字を返却する。
        ///     </para>
        /// </remarks>
        /// <param name="isAppend">結合フラグ</param>
        /// <param name="targets">整形対象</param>
        /// <returns>整形した<see cref="SourceFormatTarget"/> 配列</returns>
        public static SourceFormatTargetBlock If(bool isAppend, params SourceFormatTargetBlock[] targets)
            => If(isAppend, "", targets);

        /// <summary>
        ///     必要に応じて空文字に変換する。
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         <paramref name="isAppend"/> が <see langword="true"/> の場合、<paramref name="prefix"/> を用いて整形を行い、その結果を返す。
        ///     </para>
        ///     <para>
        ///         <paramref name="isAppend"/> が <see langword="true"/> の場合、空文字を返却する。
        ///     </para>
        /// </remarks>
        /// <param name="isAppend">結合フラグ</param>
        /// <param name="prefix">成形時に各業の先頭に付与する文字列</param>
        /// <param name="targets">整形対象</param>
        /// <returns>整形した<see cref="SourceFormatTarget"/> 配列</returns>
        public static SourceFormatTargetBlock If(bool isAppend, string prefix, params SourceFormatTargetBlock[] targets)
            => isAppend
                ? FilteredTextMany(targets).Select(s => new SourceFormatTarget($"{prefix}{s}")).ToArray()
                : Array.Empty<SourceFormatTarget>();

        /// <summary>
        ///     必要に応じてコード生成処理を実行する。
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         <paramref name="isAppend"/> が <see langword="true"/> の場合、整形を行い、その結果を返す。
        ///     </para>
        ///     <para>
        ///         <paramref name="isAppend"/> が <see langword="true"/> の場合、空文字を返却する。
        ///     </para>
        /// </remarks>
        /// <param name="isAppend">結合フラグ</param>
        /// <param name="generateTarget">生成処理</param>
        /// <returns>整形した<see cref="SourceFormatTarget"/> 配列</returns>
        public static SourceFormatTargetBlock If(bool isAppend, params Func<SourceFormatTargetBlock>[] generateTarget)
            => If(isAppend, "", generateTarget);

        /// <summary>
        ///     必要に応じてコード生成処理を実行する。
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         <paramref name="isAppend"/> が <see langword="true"/> の場合、<paramref name="prefix"/> を用いて整形を行い、その結果を返す。
        ///     </para>
        ///     <para>
        ///         <paramref name="isAppend"/> が <see langword="true"/> の場合、空文字を返却する。
        ///     </para>
        /// </remarks>
        /// <param name="isAppend">結合フラグ</param>
        /// <param name="prefix">成形時に各業の先頭に付与する文字列</param>
        /// <param name="generateTarget">生成処理</param>
        /// <returns>整形した<see cref="SourceFormatTarget"/> 配列</returns>
        public static SourceFormatTargetBlock If(bool isAppend, string prefix,
            params Func<SourceFormatTargetBlock>[] generateTarget)
            => isAppend
                ? FilteredTextMany(generateTarget.Select(f => f()).ToArray())
                    .Select(s => new SourceFormatTarget($"{prefix}{s}")).ToArray()
                : Array.Empty<SourceFormatTarget>();

        /// <summary>
        ///     結合対象ではない <see cref="SourceFormatTarget"/> を除去し、文字列として返却する。
        /// </summary>
        /// <param name="targets">処理対象</param>
        /// <returns>処理結果</returns>
        private static string[] FilteredText(SourceFormatTargetBlock targets)
            => targets.Where(item => item.IsAppend).Select(item => item.Text).ToArray();

        /// <summary>
        ///     結合対象ではない <see cref="SourceFormatTarget"/> を除去し、文字列として返却する。
        /// </summary>
        /// <param name="targets">処理対象</param>
        /// <returns>処理結果</returns>
        private static IEnumerable<string> FilteredTextMany(SourceFormatTargetBlock[] targets)
            => targets.SelectMany(row => row).Where(line => line.IsAppend)
                .Select(line => line.Text);
    }
}
