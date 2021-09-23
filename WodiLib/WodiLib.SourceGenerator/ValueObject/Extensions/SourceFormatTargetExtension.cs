// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : SourceFormatTargetExtension.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Linq;
using WodiLib.SourceGenerator.Core.SourceBuilder;

namespace WodiLib.SourceGenerator.ValueObject.Extensions
{
    /// <summary>
    ///     <see cref="SourceFormatTarget"/> 拡張クラス
    /// </summary>
    internal static class SourceFormatTargetExtension
    {
        /// <summary>
        ///     自身がソースコード情報を1行でも有する場合
        ///     <paramref name="prefix"/> と <paramref name="suffix"/> を付与する。
        /// </summary>
        /// <param name="src">処理対象</param>
        /// <param name="prefix">付与するprefix</param>
        /// <param name="suffix">付与するsuffix</param>
        /// <returns>処理結果</returns>
        public static SourceFormatTargetBlock AppendPrefixAndSuffixIfNotEmpty(this SourceFormatTargetBlock src,
            string prefix, string suffix)
        {
            if (src.IsEmpty) return src;
            return new SourceFormatTarget[]
                {
                    prefix
                }.Concat(src)
                .Concat(new SourceFormatTarget[]
                {
                    suffix
                })
                .ToArray();
        }
    }
}
