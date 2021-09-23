// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : SyntaxWorker.CustomizedSyntaxAnalyzer.OutputTypeAnalyzer.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Linq;
using Microsoft.CodeAnalysis;
using WodiLib.SourceGenerator.Core.Dtos;
using WodiLib.SourceGenerator.Core.Extensions;

namespace WodiLib.SourceGenerator.Core.Templates.FromAttribute
{
    internal partial class SyntaxWorker
    {
        /// <summary>
        ///     型定義構文解析器
        /// </summary>
        internal partial class CustomizedSyntaxAnalyzer
        {
            /// <summary>
            ///     自動生成出力型解析処理
            /// </summary>
            private class OutputTypeAnalyzer : IAnalyzeTransfer
            {
                /// <inheritdoc/>
                public bool CanRespond(SyntaxWorker outer, GeneratorSyntaxContext context, INamedTypeSymbol targetClass)
                {
                    return targetClass.GetAttributes()
                        .Any(attr => FilterAttribute(attr, outer));
                }

                /// <inheritdoc/>
                public void Analyze(SyntaxWorker outer, GeneratorSyntaxContext context, INamedTypeSymbol targetClass)
                {
                    outer.Logger?.AppendLine("start OutputTypeAnalyzer.Analyze");

                    foreach (var targetAttr in targetClass.GetAttributes())
                    {
                        outer.Logger?.AppendLine($"targetAttribute: {targetAttr}");
                        var targetAttrSymbol = targetAttr.AttributeClass;
                        if (targetAttrSymbol is null)
                        {
                            continue;
                        }

                        var targetAttributeFullName = targetAttrSymbol.FullName();
                        outer.Logger?.AppendLine($"targetAttributeFullName: {targetAttributeFullName}");

                        var sourceAddable = outer.ExecuteInfoResolver.Resolve(targetAttrSymbol);
                        if (sourceAddable is null)
                        {
                            outer.Logger?.AppendLine("sourceAddable not found.");
                            continue;
                        }

                        outer.Logger?.AppendLine(
                            $"constructorValues: {string.Join(", ", targetAttr.ConstructorArguments.Select((item, idx) => $"arg{idx}: {item.ToValueString()}"))}");
                        outer.Logger?.AppendLine(
                            $"targetAttr.NamedArguments: {string.Join(", ", targetAttr.NamedArguments.Select(item => $"{item.Key}: {item.Value.ToValueString()}"))}");
                        outer.Logger?.AppendLine($"target Namespace: {targetClass.Namespace()}");
                        outer.Logger?.AppendLine($"target className: {targetClass.Name}");

                        var workResult = new SyntaxWorkResult(targetClass, targetAttr);
                        outer.Logger?.AppendLine($"resultItem: {workResult}");

                        sourceAddable.PutSyntaxWorkResult(workResult);
                    }

                    outer.Logger?.AppendLine("finish OutputTypeAnalyzer.Analyze");
                }

                /// <summary>
                ///     処理対象の属性フィルタリング用
                /// </summary>
                /// <param name="data">属性データ</param>
                /// <param name="outer"><see cref="SyntaxWorker"/> インスタンス</param>
                /// <returns>処理対象の場合 <see langword="true"/></returns>
                private static bool FilterAttribute(AttributeData data, SyntaxWorker outer)
                {
                    var symbol = data.AttributeClass;
                    if (symbol is null)
                    {
                        return false;
                    }

                    var sourceAddable = outer.ExecuteInfoResolver.Resolve(symbol);
                    if (sourceAddable is null)
                    {
                        return false;
                    }

                    return true;
                }
            }
        }
    }
}
