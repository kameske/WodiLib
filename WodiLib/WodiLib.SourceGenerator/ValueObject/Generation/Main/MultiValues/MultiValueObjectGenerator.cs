// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : MultiValueObjectGenerator.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using WodiLib.SourceGenerator.Core.Dtos;
using WodiLib.SourceGenerator.Core.Extensions;
using WodiLib.SourceGenerator.Core.SourceAddables.PostInitialize;
using WodiLib.SourceGenerator.Core.SourceBuilder;
using WodiLib.SourceGenerator.Core.Templates.FromAttribute;
using WodiLib.SourceGenerator.ValueObject.Extensions;
using MyAttr =
    WodiLib.SourceGenerator.ValueObject.Generation.PostInitAction.Attributes.MultiValueObjectAttribute;
using static WodiLib.SourceGenerator.Core.SourceBuilder.SourceConstants;

namespace WodiLib.SourceGenerator.ValueObject.Generation.Main.MultiValues
{
    internal class MultiValueObjectGenerator : MainSourceAddableTemplate
    {
        public override InitializeAttributeSourceAddable TargetAttribute => MyAttr.Instance;

        private protected override SourceFormatTargetBlock GenerateTypeDefinitionSource(WorkState workState)
        {
            var thisType = workState.PropertyValues.TargetSymbol?.ClassName() ?? "";
            var defInfo = workState.CurrentTypeDefinitionInfo;

            var properties = GetInitializeProperties(workState)
                .ToList();

            return SourceTextFormatter.Format("", new SourceFormatTarget[]
                {
                    ($@"{DefinitionSource(defInfo)} {thisType}"),
                    ($@"{{")
                },
                SourceTextFormatter.Format(IndentSpace, ConstructorSource(properties, workState)),
                new SourceFormatTarget[]
                {
                    ($@"}}")
                });
        }

        /// <summary>
        ///     定義宣言部のソースを生成する。
        /// </summary>
        /// <param name="typeDefinitionInfo">型定義情報</param>
        /// <returns>ソースコード文字列</returns>
        private static string DefinitionSource(TypeDefinitionInfo typeDefinitionInfo)
        {
            var resultBuilder = new StringBuilder();

            var accessibility = AccessibilityConverter.ConvertSourceText(typeDefinitionInfo.Accessibility);
            resultBuilder.Append(accessibility);
            resultBuilder.Append(" partial ");
            if (typeDefinitionInfo.IsClass)
            {
                resultBuilder.Append("class");
            }
            else if (typeDefinitionInfo.IsRecord)
            {
                resultBuilder.Append("record");
            }
            else
            {
                resultBuilder.Append("struct");
            }

            return resultBuilder.ToString();
        }

        /// <summary>
        ///     初期化対象のプロパティ一覧を取得する。
        /// </summary>
        /// <param name="workState"></param>
        /// <returns></returns>
        private static IEnumerable<IPropertySymbol> GetInitializeProperties(WorkState workState)
        {
            // public なプロパティのうち init アクセサを持たないプロパティを対象とする。

            var symbol = workState.CurrentSymbol;

            var properties = symbol?.GetMembers()
                                 .Where(member => member.Kind == SymbolKind.Property
                                                  && member.DeclaredAccessibility == Accessibility.Public)
                                 .Cast<IPropertySymbol>()
                                 .ToList()
                             ?? new List<IPropertySymbol>();

            var initMethods = symbol?.GetMembers()
                                  .Where(member => member.Kind == SymbolKind.Method
                                                   && member.DeclaredAccessibility == Accessibility.Public
                                                   && ((member as IMethodSymbol)?.IsInitOnly ?? false))
                              ?? new List<ISymbol>();
            var initPropertiesNames = initMethods.Select(method => method.Name.Substring(4) /* "set_" を除去 */)
                .ToList();

            return properties.Where(prop => !initPropertiesNames.Contains(prop.Name));
        }

        /// <summary>
        ///     コンストラクタ部のソースブロックを生成する。
        /// </summary>
        /// <param name="properties">コンストラクタで初期化するプロパティ一覧</param>
        /// <param name="workState">ワーク状態</param>
        /// <returns>ソースコード文字列ブロック</returns>
        private static SourceFormatTargetBlock ConstructorSource(IEnumerable<IPropertySymbol> properties,
            WorkState workState)
        {
            var propertyArray = properties.ToArray();

            var typeName = workState.PropertyValues.TargetSymbol?.Name ?? "";
            var argsDocComments = propertyArray.Select(prop =>
                $"/// {Tag.Param(prop.Name.ToLowerFirstChar(), Tag.See.Cref(prop.Name))}");
            var argsCode = string.Join(",",
                propertyArray.Select(prop => $"{prop.Type} {prop.Name.ToLowerFirstChar()}"));
            var validateNullArgCodes = propertyArray.Where(prop => prop.Type.TypeKind == TypeKind.Class)
                .Select(prop =>
                    $"{__}if ({prop.Name.ToLowerFirstChar()} is null) throw new System.ArgumentNullException(nameof({prop.Name.ToLowerFirstChar()}));");
            var propertySetCodes = propertyArray.Select(prop => $"{__}{prop.Name} = {prop.Name.ToLowerFirstChar()};");

            return SourceTextFormatter.Format("", new SourceFormatTarget[]
                {
                    (@$"/// <summary>"),
                    (@$"/// {__}コンストラクタ"),
                    (@$"/// </summary>")
                },
                new SourceFormatTargetBlock(argsDocComments.ToArray()),
                new SourceFormatTarget[]
                {
                    (@$"public {typeName}({argsCode}) {{")
                },
                new SourceFormatTargetBlock(validateNullArgCodes.ToArray()),
                new SourceFormatTargetBlock(propertySetCodes.ToArray()),
                new SourceFormatTarget[]
                {
                    (@$"}}")
                }
            );
        }

        private MultiValueObjectGenerator()
        {
        }

        public static MultiValueObjectGenerator Instance { get; } = new();
    }
}
