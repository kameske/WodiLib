// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : Tag.SeeAlso.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.SourceGenerator.Core.SourceBuilder
{
    internal static partial class Tag
    {
        /// <summary>
        ///     {seealso} タグ文字列生成
        /// </summary>
        public static class SeeAlso
        {
            /// <summary>
            ///     cref要素
            /// </summary>
            /// <param name="target">cref対象</param>
            /// <param name="body">タグ本文</param>
            /// <returns>タグ文字列</returns>
            public static string Cref(string target, string? body = null) => Base("cref", target, body);

            /// <summary>
            ///     タグ文字列生成処理ベース
            /// </summary>
            /// <param name="attrName">属性名</param>
            /// <param name="value">属性値</param>
            /// <param name="body">タグ本文</param>
            /// <returns>タグ文字列</returns>
            private static string Base(string attrName, string value, string? body)
                => body is null
                    ? Tag.Base("seealso", (attrName, value))
                    : Tag.Base("seealso", body, (attrName, value));
        }
    }
}
