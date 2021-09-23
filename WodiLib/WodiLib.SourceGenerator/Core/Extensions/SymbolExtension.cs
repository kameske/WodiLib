// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : ISymbolExtension.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace WodiLib.SourceGenerator.Core.Extensions
{
    /// <summary>
    ///     <see cref="ISymbol"/> 拡張クラス
    /// </summary>
    internal static class ISymbolExtension
    {
        /// <returns>名前空間</returns>
        public static string Namespace(this ISymbol symbol)
        {
            var names = new List<string>();
            var iterator = symbol as INamespaceSymbol ?? symbol.ContainingNamespace;
            while (iterator != null)
            {
                if (!string.IsNullOrEmpty(iterator.Name))
                    names.Add(iterator.Name);
                iterator = iterator.ContainingNamespace;
            }

            names.Reverse();
            return string.Join(".", names);
        }

        /// <summary>
        ///     <paramref name="symbol"/> に含まれる指定した名前に一致する属性情報を検索する。
        /// </summary>
        /// <param name="symbol">検索対象</param>
        /// <param name="targetAttributeFullName">検索属性名（フル）</param>
        /// <returns>属性が付与されている場合、属性情報。付与されていない場合、<see langword="null"/>。</returns>
        public static AttributeData? FirstOrDefaultAttribute(this ISymbol symbol, string targetAttributeFullName)
            => symbol.GetAttributes().FirstOrDefault(attr =>
                attr.AttributeClass?.FullName().Equals(targetAttributeFullName) ?? false);
    }
}
