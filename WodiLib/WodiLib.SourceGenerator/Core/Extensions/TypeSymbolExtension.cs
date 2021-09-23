// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : TypeSymbolExtension.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using Microsoft.CodeAnalysis;

namespace WodiLib.SourceGenerator.Core.Extensions
{
    /// <summary>
    ///     <see cref="ITypeSymbol"/> 拡張クラス
    /// </summary>
    internal static class TypeSymbolExtension
    {
        /// <summary>
        ///     名前空間名を含む全名称を取得する。
        /// </summary>
        /// <param name="src">対象</param>
        /// <returns>名称</returns>
        public static string FullName(this ITypeSymbol src)
        {
            var nameSpace = src.Namespace();
            var name = src.Name;
            return $"{nameSpace}.{name}";
        }
    }
}
