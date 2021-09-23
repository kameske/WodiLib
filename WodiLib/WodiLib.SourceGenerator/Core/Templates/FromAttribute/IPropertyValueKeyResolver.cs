// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : IPropertyValueKeyResolver.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using Microsoft.CodeAnalysis;
using WodiLib.SourceGenerator.Core.Dtos;

namespace WodiLib.SourceGenerator.Core.Templates.FromAttribute
{
    /// <summary>
    ///     属性プロパティディクショナリのキー名解決処理インタフェース
    /// </summary>
    internal interface IPropertyValueKeyResolver
    {
        /// <summary>
        ///     ソース自動生成対象シンボルからディクショナリキー名を取得する。
        /// </summary>
        /// <param name="symbol">対象シンボル</param>
        /// <returns>プロパティ値ディクショナリのキー名</returns>
        string Resolve(INamedTypeSymbol symbol);

        /// <summary>
        ///     ソース自動生成対象シンボルからディクショナリキー名を取得する。
        /// </summary>
        /// <param name="data">対象属性</param>
        /// <returns>プロパティ値ディクショナリのキー名</returns>
        string Resolve(AttributeData data);

        /// <summary>
        ///     属性プロパティ解析結果からディクショナリキー名を取得する。
        /// </summary>
        /// <param name="workResult">対象解析結果</param>
        /// <returns>プロパティ値ディクショナリのキー名</returns>
        (string, string) Resolve(SyntaxWorkResult workResult);
    }
}
