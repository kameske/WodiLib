// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : AccessibilityConverter.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using Microsoft.CodeAnalysis;

namespace WodiLib.SourceGenerator.Core.SourceBuilder
{
    /// <summary>
    ///     <see cref="Accessibility"/> 変換クラス
    /// </summary>
    internal static class AccessibilityConverter
    {
        /// <returns>ソースコード文字列</returns>
        public static string ConvertSourceText(Accessibility accessibility)
            => accessibility switch
            {
                Accessibility.Public => "public",
                Accessibility.Protected => "protected",
                Accessibility.Internal => "internal",
                Accessibility.ProtectedOrInternal => "internal protected",
                Accessibility.Private => "private",
                Accessibility.ProtectedAndInternal => "protected private",
                _ => throw new ArgumentOutOfRangeException(nameof(accessibility), accessibility,
                    "Not defined SwitchCase")
            };
    }
}
