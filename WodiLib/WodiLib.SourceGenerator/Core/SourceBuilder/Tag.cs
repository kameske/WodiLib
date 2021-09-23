// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : Tag.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Linq;
using WodiLib.SourceGenerator.Core.Extensions;

namespace WodiLib.SourceGenerator.Core.SourceBuilder
{
    /// <summary>
    ///     タグ文字列生成処理
    /// </summary>
    internal static partial class Tag
    {
        /// <summary>
        ///     {inheritDoc} タグ
        /// </summary>
        /// <param name="cref">cref要素</param>
        /// <returns>タグ文字列</returns>
        public static string InheritDoc(string? cref = null)
        {
            const string tag = "inheritDoc";
            return cref is null
                ? Base(tag)
                : Base(tag, ("cref", cref));
        }

        /// <summary>
        ///     {para} タグ
        /// </summary>
        /// <param name="sentence">本文</param>
        /// <returns>タグ文字列</returns>
        public static string Para(string sentence) => Base("para", ToEscapedDocumentCommentString(sentence));

        /// <summary>
        ///     {param} タグ
        /// </summary>
        /// <param name="argName">引数名</param>
        /// <param name="desc">引数説明</param>
        /// <returns>タグ文字列</returns>
        public static string Param(string argName, string desc) => Base("param", desc, ("name", argName));

        /// <summary>
        ///     {paramref} タグ
        /// </summary>
        /// <param name="name">引数名</param>
        /// <returns>タグ文字列</returns>
        public static string ParamRef(string name) => Base("paramref", ("name", name));

        /// <summary>
        ///     {remarks} タグ
        /// </summary>
        /// <param name="body">本文</param>
        /// <returns>タグ文字列</returns>
        public static string Remarks(string body) => Base("remarks", ToEscapedDocumentCommentString(body));

        /// <summary>
        ///     {returns} タグ
        /// </summary>
        /// <param name="body">本文</param>
        /// <returns>タグ文字列</returns>
        public static string Returns(string body) => Base("returns", ToEscapedDocumentCommentString(body));

        /// <summary>
        ///     {summary} タグ
        /// </summary>
        /// <param name="body">本文</param>
        /// <returns>タグ文字列</returns>
        public static string Summary(string body) => Base("summary", ToEscapedDocumentCommentString(body));

        /// <summary>
        ///     Bodyなしタグ
        /// </summary>
        /// <param name="tag">タグ名</param>
        /// <param name="elements">属性（名, 値）</param>
        /// <returns>タグ文字列</returns>
        private static string Base(string tag, params (string name, string value)[] elements)
            => $@"<{tag}{ElementsToString(elements)}/>";

        /// <summary>
        ///     Bodyありタグ
        /// </summary>
        /// <param name="tag">タグ名</param>
        /// <param name="body">Body</param>
        /// <param name="elements">属性（名, 値）</param>
        /// <returns>タグ文字列</returns>
        private static string Base(string tag, string body, params (string name, string value)[] elements)
            => $@"<{tag}{ElementsToString(elements)}>{body}</{tag}>";

        /// <summary>
        ///     属性リストをタグ用文字列に変換する。
        /// </summary>
        /// <param name="elements">属性リスト</param>
        /// <returns>タグ文字列</returns>
        private static string ElementsToString(params (string name, string value)[] elements)
            => string.Join(" ", elements.Select(elem => $"{elem.name}=\"{elem.value}\"")).PrefixIfNotEmpty(" ");

        /// <returns>ドキュメントコメント用にエスケープした文字列</returns>
        internal static string ToEscapedDocumentCommentString(string src)
            => src.Replace("&", "&amp;")
                .Replace("<", "&lt;")
                .Replace(">", "&gt;");
    }
}
