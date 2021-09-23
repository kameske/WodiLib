// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : SyntaxWorker.CustomizedSyntaxAnalyzer.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using Microsoft.CodeAnalysis;
using WodiLib.SourceGenerator.Core.Dtos;
using WodiLib.SourceGenerator.Core.Enums;
using WodiLib.SourceGenerator.Core.Extensions;

namespace WodiLib.SourceGenerator.Core.Templates.FromAttribute
{
    internal partial class SyntaxWorker
    {
        /// <summary>
        ///     構文解析器
        /// </summary>
        internal partial class CustomizedSyntaxAnalyzer : ISyntaxAnalyzer
        {
            /// <summary>
            ///     処理転送先リスト
            /// </summary>
            /// <value></value>
            private static readonly IAnalyzeTransfer[] AnalyzeTransfers =
            {
                new InputAttributeAnalyzer(),
                new OutputTypeAnalyzer()
            };

            /// <inheritdoc/>
            public void Analyze(GeneratorSyntaxContext context, SyntaxWorkerCore outer)
                => Analyze(context, (SyntaxWorker)outer);

            /// <inheritdoc cref="Analyze"/>
            private static void Analyze(GeneratorSyntaxContext context, SyntaxWorker outer)
            {
                try
                {
                    outer.Logger?.AppendLine("start AnalyzeClassSyntax");

                    outer.Logger?.AppendLine($"Node type: {context.Node.GetType().FullName}");
                    var objectType = context.Node.GetObjectType();
                    outer.Logger?.AppendLine($"objectType: {objectType}");
                    if (!objectType.IsTypeDefinition())
                    {
                        outer.Logger?.AppendLine("Skip because this is not type definition.");
                        return;
                    }

                    var targetClass = (INamedTypeSymbol?)context.SemanticModel.GetDeclaredSymbol(context.Node);
                    outer.Logger?.AppendLine($"targetClass: {targetClass}");
                    if (targetClass is null)
                    {
                        return;
                    }

                    AnalyzeForTransfer(context, outer, targetClass);

                    var targetFullName = targetClass.ToString();

                    var DefinitionInfo = new TypeDefinitionInfo
                    {
                        TypeFullName = targetFullName,
                        ObjectType = objectType,
                        Accessibility = targetClass.DeclaredAccessibility,
                        IsAbstract = targetClass.IsAbstract,
                        IsStatic = targetClass.IsStatic
                    };
                    outer.Logger?.AppendLine($"TypeDefinition: {DefinitionInfo}");
                    outer.TypeDefinitionDict[targetFullName] = DefinitionInfo;

                    outer.Logger?.AppendLine("finish AnalyzeClassSyntax");
                }
                catch (Exception ex)
                {
                    outer.Logger?.AppendLine(ex.ToString().Split('\n'));
                }
            }

            private static void AnalyzeForTransfer(GeneratorSyntaxContext context, SyntaxWorker outer,
                INamedTypeSymbol targetClass)
            {
                var cnt = 0;
                foreach (var transfer in AnalyzeTransfers)
                {
                    if (transfer.CanRespond(outer, context, targetClass))
                    {
                        outer.Logger?.AppendLine($"transfer[{cnt}] is CanRespond.");

                        try
                        {
                            transfer.Analyze(outer, context, targetClass);
                        }
                        catch (Exception ex)
                        {
                            outer.Logger?.AppendLine(ex.ToString().Split('\n'));
                        }
                    }

                    cnt++;
                }
            }

            /// <summary>
            ///     解析処理転送先インタフェース
            /// </summary>
            private interface IAnalyzeTransfer
            {
                /// <summary>
                ///     応答可否
                /// </summary>
                /// <returns>解析を行う場合<see langword="true"/></returns>
                bool CanRespond(SyntaxWorker outer, GeneratorSyntaxContext context, INamedTypeSymbol targetClass);

                /// <summary>
                ///     解析処理を行う。
                /// </summary>
                void Analyze(SyntaxWorker outer, GeneratorSyntaxContext context, INamedTypeSymbol targetClass);
            }
        }
    }
}
