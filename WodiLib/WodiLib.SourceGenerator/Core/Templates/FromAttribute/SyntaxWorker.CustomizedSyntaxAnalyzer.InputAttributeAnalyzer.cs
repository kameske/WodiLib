// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : SyntaxWorker.CustomizedSyntaxAnalyzer.InputAttributeAnalyzer.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using WodiLib.SourceGenerator.Core.Dtos;
using WodiLib.SourceGenerator.Core.Extensions;

namespace WodiLib.SourceGenerator.Core.Templates.FromAttribute
{
    internal partial class SyntaxWorker
    {
        internal partial class CustomizedSyntaxAnalyzer
        {
            /// <summary>
            ///     自動生成入力用属性解析処理
            /// </summary>
            private class InputAttributeAnalyzer : IAnalyzeTransfer
            {
                /// <inheritdoc/>
                public bool CanRespond(SyntaxWorker outer, GeneratorSyntaxContext context, INamedTypeSymbol targetClass)
                {
                    return outer.ExecuteInfoResolver.Resolve(targetClass) is not null;
                }

                /// <inheritdoc/>
                public void Analyze(SyntaxWorker outer, GeneratorSyntaxContext context, INamedTypeSymbol targetClass)
                {
                    outer.Logger?.AppendLine("start InputAttributeAnalyzer.Analyze");

                    var targetClassName = targetClass.FullName();
                    outer.Logger?.AppendLine($"targetClassName: {targetClassName}");

                    var sourceAddable = outer.ExecuteInfoResolver.Resolve(targetClass);
                    if (sourceAddable is null)
                    {
                        outer.Logger?.AppendLine(
                            $"Cancel AnalyzePropertySyntax because \"{targetClassName}\" is not registered.");
                        return;
                    }

                    var props = context.Node.SyntaxTree.GetRoot().DescendantNodes().OfType<PropertyDeclarationSyntax>()
                        .Select(prop => (IPropertySymbol?)context.SemanticModel.GetDeclaredSymbol(prop));

                    var defaultValueDict = CreateDefaultValueDict(outer, props);

                    var isRootAttr = outer.ExecuteInfoResolver.IsRootAttribute(targetClass);

                    var parentAttrName = isRootAttr
                        ? null
                        : targetClass.BaseType!.FullName();
                    sourceAddable.PutPropertyDefaultValues(targetClassName, defaultValueDict, parentAttrName);

                    outer.Logger?.AppendLine("finish InputAttributeAnalyzer.Analyze");
                }

                /// <summary>
                ///     デフォルト値ディクショナリを生成する。
                /// </summary>
                /// <param name="outer"><see cref="ISyntaxWorker"/> インスタンス</param>
                /// <param name="props">プロパティ一覧</param>
                /// <returns>処理結果</returns>
                private static Dictionary<string, PropertyValue> CreateDefaultValueDict(ISyntaxWorker outer,
                    IEnumerable<IPropertySymbol?> props)
                {
                    var defaultValueDict = new Dictionary<string, PropertyValue>();

                    foreach (var targetProperty in props)
                    {
                        outer.Logger?.AppendLine($"targetProperty: {targetProperty}");
                        if (targetProperty is null)
                        {
                            continue;
                        }

                        var propertyName = targetProperty.Name;
                        outer.Logger?.AppendLine($"target propertyName: {propertyName}");

                        var defaultValue = targetProperty.GetDefaultValue();
                        outer.Logger?.AppendLine($"target defaultValue: {defaultValue?.ToValueString() ?? "<<null>>"}");

                        defaultValueDict[propertyName] = new PropertyValue(defaultValue);
                    }

                    return defaultValueDict;
                }
            }
        }
    }
}
