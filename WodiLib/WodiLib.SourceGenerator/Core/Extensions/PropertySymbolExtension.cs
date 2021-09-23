// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : TypeSymbolExtension.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.ComponentModel;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace WodiLib.SourceGenerator.Core.Extensions
{
    /// <summary>
    ///     <see cref="IPropertySymbol"/> 拡張クラス
    /// </summary>
    internal static class PropertySymbolExtension
    {
        /// <summary>
        ///     付与された <see cref="DefaultValueAttribute"/> からデフォルト値を取得する。
        /// </summary>
        /// <returns></returns>
        public static TypedConstant? GetDefaultValue(this IPropertySymbol target)
        {
            var defaultValueAttr = target.GetAttributes().FirstOrDefault(attr =>
                attr.AttributeClass?.FullName().Equals(typeof(DefaultValueAttribute).FullName) ?? false);
            return defaultValueAttr?.ConstructorArguments[0];
        }
    }
}
