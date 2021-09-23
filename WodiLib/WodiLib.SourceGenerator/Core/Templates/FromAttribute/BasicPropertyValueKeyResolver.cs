// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : BasicPropertyValueKeyResolver.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using Microsoft.CodeAnalysis;
using WodiLib.SourceGenerator.Core.Dtos;
using WodiLib.SourceGenerator.Core.Extensions;

namespace WodiLib.SourceGenerator.Core.Templates.FromAttribute
{
    /// <summary>
    ///     基本的な属性プロパティディクショナリのキー名解決処理
    /// </summary>
    internal class BasicPropertyValueKeyResolver : IPropertyValueKeyResolver
    {
        private ILogger? Logger { get; }

        public BasicPropertyValueKeyResolver(ILogger? logger)
        {
            Logger = logger;
        }

        /// <inheritDoc/>
        public string Resolve(INamedTypeSymbol symbol)
        {
            var result = symbol.FullName();
            Logger?.AppendLine($"BasicPropertyValueKeyResolver resolve for INamedTypeSymbol: {result}");
            return result;
        }

        public string Resolve(AttributeData data)
        {
            var result = data.AttributeClass?.FullName() ?? "";
            Logger?.AppendLine($"BasicPropertyValueKeyResolver resolve for AttributeData: {result}");
            return result;
        }

        /// <inheritDoc/>
        public (string, string) Resolve(SyntaxWorkResult workResult)
        {
            var targetKeyName = workResult.FullName;
            var srcKeyName = workResult.SrcAttributeName;
            Logger?.AppendLine(
                $"BasicPropertyValueKeyResolver resolve for SyntaxWorkResult: ({targetKeyName}, {srcKeyName})");
            return (targetKeyName, srcKeyName);
        }
    }
}
