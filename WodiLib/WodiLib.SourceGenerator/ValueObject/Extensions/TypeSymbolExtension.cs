// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : TypeSymbolExtension.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Linq;
using Microsoft.CodeAnalysis;
using WodiLib.SourceGenerator.Core.Extensions;

namespace WodiLib.SourceGenerator.ValueObject.Extensions
{
    /// <summary>
    ///     <see cref="IPropertySymbol"/> 拡張クラス
    /// </summary>
    internal static class TypeSymbolExtension
    {
        /// <summary>
        ///     指定した名前の属性、またはその属性を継承した属性が適用されているかどうかを返す。
        /// </summary>
        /// <param name="src">処理対象</param>
        /// <param name="attributeName">検索する属性名（フル名称）</param>
        /// <returns>指定した属性またはその属性を継承した属性が適用されている場合<see langrowr="true"/></returns>
        public static bool IsAppliedAttribute(this ITypeSymbol src, string attributeName)
        {
            return src.GetAttributes()
                .Any(attr => attr.AttributeClass?.IsSameOrExtended(attributeName) ?? false);
        }
    }
}
