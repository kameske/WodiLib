// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : StringExtension.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WodiLib.SourceGenerator.Core.Extensions
{
    /// <summary>
    ///     <see cref="string"/> 拡張クラス
    /// </summary>
    internal static class StringExtension
    {
        /// <returns>指定した回数繰り返した結果文字列</returns>
        public static string Repeat(this string src, int count)
        {
            return string.Join("", Enumerable.Range(0, count).Select(_ => src));
        }

        /// <returns>改行コードを含む場合 <see langword="true"/></returns>
        private static bool HasNewLine(this string src)
            => src.Any(c => c == '\n');

        /// <returns>先頭および末尾にダブルクォートを付与した文字列</returns>
        public static string ToWrappedDoubleQuote(this string src)
            => $"\"{src}\"";

        /// <returns>改行コードと半角スベースを除去した結果空文字である場合<see langword="true"/></returns>
        public static bool IsEmpty(this string src)
            => src.HasNewLine()
                ? src.Lines(false).Any(line => line.IsEmpty())
                : src.Trim().Equals("");

        /// <returns>改行コードを除去した文字列。改行コード前後でトリムする。</returns>
        public static string TrimNewLine(this string src)
            => string.Join("", src.Lines(true).Select(line => line.Trim()));

        /// <summary>
        ///     文字列が空文字ではない場合 <paramref name="prefix"/> を付与する。
        /// </summary>
        /// <param name="src">対象文字列</param>
        /// <param name="prefix">付与するPrefix</param>
        /// <returns>処理結果</returns>
        public static string PrefixIfNotEmpty(this string src, string prefix)
            => src.IsEmpty()
                ? src
                : $"{prefix}{src}";

        /// <returns>改行コードで分割した文字列リスト</returns>
        public static IEnumerable<string> Lines(this string src, bool filterEmpty)
            => src.Replace("\r\n", "\n")
                .Split('\n')
                .Where(line => !filterEmpty || !line.IsEmpty());

        /// <returns>名前空間部分を圧縮した文字列</returns>
        public static string CompressNameSpace(this string src)
        {
            if (src.Equals("")) return "";

            var resultBuilder = new StringBuilder();

            var split = src.Split('.');
            if (split.Length == 1) return split[0];
            for (var i = 0; i < split.Length - 1; i++)
            {
                var nameSpacePart = split[i];
                if (nameSpacePart.Equals("")) continue;

                var firstStr = nameSpacePart.Substring(0, 1);

                resultBuilder.Append(firstStr).Append(".");
            }

            resultBuilder.Append(split.Last());

            return resultBuilder.ToString();
        }

        /// <summary>
        /// 山括弧 ("&lt;", "&gt;") を下線 ("_") に 置換する。
        /// </summary>
        /// <param name="src">対象文字列</param>
        /// <returns>処理結果</returns>
        public static string ReplaceAngleBracketsToUnderscore(this string src)
            => src.Replace("<", "_").Replace(">", "_");
    }
}
