// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : Tag.See.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.SourceGenerator.Core.SourceBuilder
{
    internal static partial class Tag
    {
        /// <summary>
        ///     {see} タグ文字列生成
        /// </summary>
        public static class See
        {
            /// <summary>
            ///     null
            /// </summary>
            public static string Langword_Null => Base("langword", "null");

            /// <summary>
            ///     false
            /// </summary>
            public static string Langword_True => Base("langword", "true");

            /// <summary>
            ///     false
            /// </summary>
            public static string Langword_False => Base("langword", "false");

            /// <summary>
            ///     cref要素
            /// </summary>
            /// <param name="target">cref対象</param>
            /// <returns>タグ文字列</returns>
            public static string Cref(string target) => Base("cref", target);

            /// <summary>
            ///     タグ文字列生成処理ベース
            /// </summary>
            /// <param name="attrName">属性名</param>
            /// <param name="value">属性値</param>
            /// <returns>タグ文字列</returns>
            private static string Base(string attrName, string value) =>
                $@"<see {ElementsToString((attrName, value))}/>";
        }
    }
}
