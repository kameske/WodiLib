// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : InjectionMethodGenerator.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using WodiLib.SourceGenerator.Core;
using WodiLib.SourceGenerator.Core.Dtos;
using WodiLib.SourceGenerator.Core.Extensions;
using WodiLib.SourceGenerator.Core.SourceAddables.PostInitialize;
using WodiLib.SourceGenerator.Core.SourceBuilder;
using WodiLib.SourceGenerator.Core.Templates.FromAttribute;
using WodiLib.SourceGenerator.Domain.Container.Generation.PostInitAction.Attributes;
using static WodiLib.SourceGenerator.Core.SourceBuilder.SourceConstants;

namespace WodiLib.SourceGenerator.Domain.Container.Generation.Main
{
    /// <summary>
    /// テンプレートを用いたリスト実装クラス生成
    /// </summary>
    internal class InjectionMethodGenerator : MainSourceAddableTemplate
    {
        public override InitializeAttributeSourceAddable TargetAttribute =>
            InjectableAttribute.Instance;

        public override void AddSource(GeneratorExecutionContext context,
            ITypeDefinitionInfoResolver typeDefinitionInfoResolver)
        {
            Logger?.AppendLine($"start AddSource from attribute: {TargetAttribute.TypeFullName}");
            Logger?.AppendLine($"SyntaxWorkResultDict Count: {SyntaxWorkResultDict.Count}");
            var workState = new WorkState(
                context, typeDefinitionInfoResolver,
                SyntaxWorkResultDict, PropertyDefaultValueDict,
                PropertyValuesDict, Logger);

            var hintName = "WodiLib.Sys.Container.RegisterModels".CompressNameSpace();
            var codeBlocks = new List<SourceFormatTargetBlock>();

            foreach (var workResult in SyntaxWorkResultDict)
            {
                if (!workState.CanGenerateSource(workResult))
                {
                    Logger?.AppendLine($"Skip AddSource {workResult.FullName}.");
                    continue;
                }

                try
                {
                    workState.SetCurrent(workResult);
                }
                catch (KeyNotFoundException ex)
                {
                    Logger?.AppendLine($"[WARNING] Cannot SetCurrent for workState.");
                    Logger?.AppendLine(ex.ToString());
                    Logger?.AppendLine(ex.HelpLink);
                    continue;
                }

                try
                {
                    var codeBlock = GenerateTypeDefinitionSource(workState);
                    codeBlocks.Add(codeBlock);

                    Logger?.AppendLine($"Success GenerateCodeBlock from {workState.PropertyValues}");
                }
                catch (Exception e)
                {
                    Logger?.AppendLine($"[WARNING] Failure GenerateCodeBlock from {workState.PropertyValues}");
                    Logger?.AppendLine($"thrown exception: {e.Message}");
                    Logger?.AppendLine(e.StackTrace);
                }
            }

            var source = GenerateSource(codeBlocks.SelectMany(block => block));

            try
            {
                context.AddSource(hintName, source);
                Logger?.AppendLine($"Complete AddSource {hintName}");
            }
            catch (ArgumentException)
            {
                Logger?.AppendLine($"[WARNING] Duplicate HintName {hintName}");
                // throw new DuplicateHintNameException(ex, hintName);
            }
        }

        /// <summary>
        ///     ソース文字列を生成する。
        /// </summary>
        /// <param name="injectMethodBody">Injection処理本体ソースコード</param>
        /// <returns>ソース文字列</returns>
        private string GenerateSource(IEnumerable<SourceFormatTarget> injectMethodBody)
        {
            try
            {
                return SourceTextFormatter.Format(new SourceFormatTarget[]
                {
                    $@"namespace WodiLib.Sys",
                    $@"{{"
                }, SourceTextFormatter.Format(IndentSpace, new SourceFormatTarget[]
                    {
                        $@"internal static partial class WodiLibContainer",
                        $@"{{"
                    }, SourceTextFormatter.Format(IndentSpace, new SourceFormatTarget[]
                        {
                            $@"static partial void RegisterModels()",
                            $@"{{"
                        },
                        SourceTextFormatter.Format(IndentSpace, injectMethodBody.ToList()),
                        new SourceFormatTarget[]
                        {
                            $@"}}"
                        }
                    ), new SourceFormatTarget[]
                    {
                        $@"}}"
                    }
                ), new SourceFormatTarget[]
                {
                    $@"}}"
                });
            }
            catch (Exception ex)
            {
                return string.Join(Environment.NewLine, ex.Message, ex.StackTrace);
            }
        }

        private protected override SourceFormatTargetBlock GenerateTypeDefinitionSource(WorkState workState)
        {
            var propertyValues = workState.PropertyValues;

            var outputTypeNameList = GetOutputTypeNameList(propertyValues);
            var implementTypeName = TrimPrefixNameSpaceIfWodiLib(workState.FullName);
            var paramTypeNameList = GetParamTypeNameList(propertyValues);

            var entities = outputTypeNameList.SelectMany(outputTypeName
                => paramTypeNameList.Select(paramTypeName => new OutputCodeEntity(outputTypeName, paramTypeName)));

            var codes = entities.Select(entity =>
            {
                var (output, param) = entity;

                var isUseInitParam = param is not null;
                return isUseInitParam
                    ? $"Register<{output}, {param}>(param => new {implementTypeName}(param), Lifetime.Transient);"
                    : $"Register<{output}>(() => new {implementTypeName}(), Lifetime.Transient);";
            }).ToArray();
            return new SourceFormatTargetBlock(codes);
        }

        /// <summary>
        /// 出力型名一覧を取得する。
        /// </summary>
        /// <param name="propertyValues">出力プロパティ値</param>
        /// <returns>出力型名一覧</returns>
        private static IEnumerable<string> GetOutputTypeNameList(PropertyValues propertyValues)
        {
            var result = new List<string>();
            var outputTypeName = propertyValues[InjectableAttribute.OutputType.Name];
            if (outputTypeName is not null)
            {
                result.Add(TrimPrefixNameSpaceIfWodiLib(outputTypeName));
            }

            var outputTypeNames = propertyValues.GetArrayValue(InjectableAttribute.OutputTypes.Name);
            if (outputTypeNames is null) return result;

            result.AddRange(outputTypeNames.Select(TrimPrefixNameSpaceIfWodiLib));

            return result.Distinct();
        }

        /// <summary>
        /// パラメータ型名一覧を取得する。
        /// </summary>
        /// <param name="propertyValues">出力プロパティ値</param>
        /// <returns>パラメータ型一覧</returns>
        private static IEnumerable<string?> GetParamTypeNameList(PropertyValues propertyValues)
        {
            var result = new List<string?>();
            var paramTypeName = propertyValues[InjectableAttribute.ParamType.Name];

            result.Add(paramTypeName is not null ? TrimPrefixNameSpaceIfWodiLib(paramTypeName) : null);

            var paramTypeNames = propertyValues.GetArrayValue(InjectableAttribute.ParamTypes.Name);
            if (paramTypeNames is null) return result;

            result.AddRange(
                paramTypeNames.Select(name =>
                    name is not null && !name.Equals("null") // null を明示的に指定した場合は "null" という文字列を取得する
                        ? TrimPrefixNameSpaceIfWodiLib(name)
                        : null
                )
            );

            return result.Distinct();
        }

        /// <summary>
        /// "WodiLib." または "WodiLib.Sys." で始まる文字列の場合、これを取り除く。
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        private static string TrimPrefixNameSpaceIfWodiLib(string target)
        {
            // 前から順に判定して一致する一つだけを処理する
            var trimStrings = new[]
            {
                "WodiLib.Sys.",
                "WodiLib.",
            };

            foreach (var trimString in trimStrings)
            {
                if (!target.StartsWith(trimString)) continue;

                var trimmed = target.Substring(trimString.Length);
                return trimmed;
            }

            return target;
        }

        private InjectionMethodGenerator()
        {
        }

        public static InjectionMethodGenerator Instance { get; } = new();

        private record OutputCodeEntity(string Output, string? Param);
    }
}
