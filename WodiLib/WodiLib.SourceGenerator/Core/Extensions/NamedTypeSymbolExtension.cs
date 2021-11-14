// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : NamedTypeSymbolExtension.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Linq;
using Microsoft.CodeAnalysis;

namespace WodiLib.SourceGenerator.Core.Extensions
{
    /// <summary>
    ///     <see cref="INamedTypeSymbol"/> 拡張クラス
    /// </summary>
    internal static class NamedTypeSymbolExtension
    {
        /// <summary>
        ///     自身が <paramref name="targetFullName"/> と同じ名前を持つクラスであるかを判定する。
        /// </summary>
        /// <param name="symbol">判定対象</param>
        /// <param name="targetFullName">継承判定クラス名（フル）</param>
        /// <returns>同じクラスの場合<see langword="true"/></returns>
        public static bool IsSame(this INamedTypeSymbol symbol, string targetFullName)
        {
            return symbol.FullName().Equals(targetFullName);
        }

        /// <summary>
        ///     自身が <paramref name="targetFullName"/> と同じ名前を持つクラスか、またはそれを継承したクラスであるかを判定する。
        /// </summary>
        /// <param name="symbol">判定対象</param>
        /// <param name="targetFullName">継承判定クラス名（フル）</param>
        /// <returns>同じクラス、または継承している場合<see langword="true"/></returns>
        public static bool IsSameOrExtended(this INamedTypeSymbol symbol, string targetFullName)
        {
            if (symbol.IsSame(targetFullName))
            {
                return true;
            }

            return IsExtended(symbol, targetFullName);
        }

        /// <summary>
        ///     自身が <paramref name="targetFullName"/> を継承したクラスであるかを判定する。
        /// </summary>
        /// <param name="symbol">判定対象</param>
        /// <param name="targetFullName">継承判定クラス名（フル）</param>
        /// <returns>継承している場合<see langword="true"/></returns>
        public static bool IsExtended(this INamedTypeSymbol symbol, string targetFullName)
        {
            var baseType = symbol.BaseType;
            if (baseType is null) return false;

            var fullName = baseType.FullName();
            if (fullName.Equals(targetFullName))
            {
                return true;
            }

            return IsExtended(baseType, targetFullName);
        }

        /// <summary>
        ///     指定した属性（またはその属性を継承した属性）のデータを取得する。
        /// </summary>
        /// <param name="symbol">処理対象</param>
        /// <param name="targetFullName">取得対象属性名（フル）</param>
        /// <returns>属性データ（属性が付与されていない場合 <see langword="null"/></returns>
        public static AttributeData? GetFirstOrDefaultAttribute(this INamedTypeSymbol symbol, string targetFullName)
        {
            var attrData = symbol.GetAttributes()
                .FirstOrDefault(data => data.AttributeClass?.IsSameOrExtended(targetFullName) ?? false);
            if (attrData is not null)
            {
                return attrData;
            }

            var baseType = symbol.BaseType;
            return baseType?.FirstOrDefaultAttribute(targetFullName);
        }

        /// <summary>
        ///     クラス名を取得する。
        /// </summary>
        /// <param name="symbol">対象</param>
        /// <returns>名称</returns>
        public static string ClassName(this INamedTypeSymbol symbol)
        {
            var generic = "";
            if (symbol.Arity > 0)
            {
                generic =
                    $"<{string.Join(", ", symbol.TypeArguments.Select(arg => arg.ToString()))}>";
            }

            return $"{symbol.Name}{generic}";
        }

        /// <summary>
        ///     名前空間名を含む全名称を取得する。
        /// </summary>
        /// <param name="symbol">対象</param>
        /// <returns>名称</returns>
        public static string FullName(this INamedTypeSymbol symbol)
        {
            var nameSpace = symbol.Namespace();

            var generic = "";
            if (symbol.Arity > 0)
            {
                generic =
                    $"<{string.Join(", ", symbol.TypeArguments.Select(arg => arg.FullName()))}>";
            }

            return $"{(nameSpace != "" ? $"{nameSpace}." : "")}{symbol.Name}{generic}";
        }
    }
}
