// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : SourceConstants.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.SourceGenerator.Core.Extensions;

namespace WodiLib.SourceGenerator.Core.SourceBuilder
{
    /// <summary>
    ///     ソースコード出力用定数
    /// </summary>
    internal static class SourceConstants
    {
        /// <summary>インデント用半角スペース</summary>
        public static readonly string IndentSpace = " ".Repeat(4);

        /// <summary>インデント用半角スペース（ソースコード文字列用記号）</summary>
        public static string __ => IndentSpace;
    }
}
