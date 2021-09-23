// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : SyntaxNodeExtension.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Linq;
using Microsoft.CodeAnalysis;

namespace WodiLib.SourceGenerator.Core.Extensions
{
    /// <summary>
    ///     <see cref="TypedConstant"/> 拡張クラス
    /// </summary>
    internal static class TypedConstantExtension
    {
        public static string? ToValueString(this TypedConstant src)
            => src.IsNull
                ? null
                : src.Kind == TypedConstantKind.Array
                    ? $"{string.Join(", ", src.Values.Select(x => x.ToValueString() ?? "null"))}"
                    : src.Value?.ToString();
    }
}
