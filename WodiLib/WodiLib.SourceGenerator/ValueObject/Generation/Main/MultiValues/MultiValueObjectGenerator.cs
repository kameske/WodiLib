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
using WodiLib.SourceGenerator.ValueObject.Generation.PostInitAction.Attributes;
using WodiLib.SourceGenerator.ValueObject.Generation.PostInitAction.Enums;
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
            var thisType = workState.PropertyValues.TargetSymbol?.Name ?? "";
            var defInfo = workState.CurrentTypeDefinitionInfo;

            var properties = GetInitializeProperties(workState)
                .ToList();

            return SourceTextFormatter.Format("", new SourceFormatTarget[]
                {
                    ($@"{DefinitionSource(defInfo)} {thisType}"),
                    ($@"{{")
                },
                SourceTextFormatter.Format(IndentSpace, ConstructorSource(properties, workState)),
                SourceFormatTargetBlock.Empty,
                SourceTextFormatter.Format(IndentSpace, CastSource(properties, workState)),
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
            // public なプロパティのうち init アクセッサを持つプロパティを対象とする。

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
            var initPropertiesNames = initMethods.Select(method => method.Name.Substring(4) /* "get_" を除去 */);

            foreach (var name in initPropertiesNames)
            {
                var property = properties.Find(prop => prop.Name.Equals(name));
                if (property is not null)
                {
                    yield return property;
                }
            }
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

            var typeName = workState.Name;
            var argsDocComments = propertyArray.Select(prop =>
                $"/// {Tag.Param(prop.Name.ToLowerFirstChar(), Tag.See.Cref(prop.Name))}");
            var argsCode = string.Join(",",
                propertyArray.Select(prop => $"{prop.Type.FullName()} {prop.Name.ToLowerFirstChar()}"));
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

        /// <summary>
        ///     キャスト部のソースコードブロックを生成する。
        /// </summary>
        /// <param name="properties">コンストラクタで初期化するプロパティ一覧</param>
        /// <param name="workState">ワーク状態</param>
        /// <returns>ソースコード文字列ブロック</returns>
        private static SourceFormatTargetBlock CastSource(IEnumerable<IPropertySymbol> properties, WorkState workState)
        {
            var sCastType = workState.PropertyValues.GetOrDefault(MyAttr.CastType.Name, CastType.Code_None.ToString());
            var iCastType = int.Parse(sCastType);

            if (iCastType == CastType.Code_None)
            {
                return SourceFormatTargetBlock.Empty;
            }

            var propertyArray = properties.ToArray();

            var operationKeyword = iCastType == CastType.Code_Explicit
                ? "explicit"
                : "implicit";
            var typeName = workState.Name;
            var tupleTypesList = GetCanCastTupleTypes(propertyArray).ToList();

            var tupleToTypeCodeBlocks = new List<SourceFormatTargetBlock>();
            var valueTupleToTypeCodeBlocks = new List<SourceFormatTargetBlock>();
            var typeToTupleCodeBlocks = new List<SourceFormatTargetBlock>();

            foreach (var tupleTypes in tupleTypesList)
            {
                var tupleTypeArray = tupleTypes.ToArray();

                var tupleTypeStr = string.Join(",", tupleTypeArray);
                var tupleDecomposed = string.Join(",", tupleTypeArray.Select((_, idx) => $"tuple.Item{idx + 1}"));
                var voDecomposed = string.Join(",", propertyArray.Select(prop => $"src.{prop.Name}"));

                tupleToTypeCodeBlocks.Add(new SourceFormatTarget[]
                {
                    (@$"/// <summary>"),
                    (@$"/// Tuple&lt;{tupleTypeStr}> -> {typeName} 型変換"),
                    (@$"/// </summary>"),
                    (@$"/// {Tag.Param("tuple", "変換元")}"),
                    (@$"/// {Tag.Returns("変換した値")}"),
                    (@$"public static {operationKeyword} operator {typeName}(System.Tuple<{tupleTypeStr}> tuple)"),
                    (@$"{{"),
                    (@$"{__}return new {typeName}({tupleDecomposed});"),
                    (@$"}}"),
                    (@$"")
                });

                valueTupleToTypeCodeBlocks.Add(new SourceFormatTarget[]
                {
                    (@$"/// <summary>"),
                    (@$"/// ({tupleTypeStr}) -> {typeName} 型変換"),
                    (@$"/// </summary>"),
                    (@$"/// {Tag.Param("tuple", "変換元")}"),
                    (@$"/// {Tag.Returns("変換した値")}"),
                    (@$"public static {operationKeyword} operator {typeName}(System.ValueTuple<{tupleTypeStr}> tuple)"),
                    (@$"{{"),
                    (@$"{__}return new {typeName}({tupleDecomposed});"),
                    (@$"}}"),
                    (@$"")
                });

                typeToTupleCodeBlocks.Add(new SourceFormatTarget[]
                {
                    (@$"/// <summary>"),
                    (@$"/// {typeName} -> Tuple&lt;{tupleTypeStr}> 型変換"),
                    (@$"/// </summary>"),
                    (@$"/// {Tag.Param("src", "変換元")}"),
                    (@$"/// {Tag.Returns("変換した値")}"),
                    (@$"public static {operationKeyword} operator System.Tuple<{tupleTypeStr}>({typeName} src)"),
                    (@$"{{"),
                    (@$"{__}return new System.Tuple<{tupleTypeStr}>({voDecomposed});"),
                    (@$"}}")
                });

                typeToTupleCodeBlocks.Add(new SourceFormatTarget[]
                {
                    (@$"/// <summary>"),
                    (@$"/// {typeName} -> ValueTuple&lt;{tupleTypeStr}> 型変換"),
                    (@$"/// </summary>"),
                    (@$"/// {Tag.Param("src", "変換元")}"),
                    (@$"/// {Tag.Returns("変換した値")}"),
                    (@$"public static {operationKeyword} operator System.ValueTuple<{tupleTypeStr}>({typeName} src)"),
                    (@$"{{"),
                    (@$"{__}return new System.ValueTuple<{tupleTypeStr}>({voDecomposed});"),
                    (@$"}}")
                });
            }

            var allBlocks = tupleToTypeCodeBlocks.Concat(
                valueTupleToTypeCodeBlocks
            ).Concat(
                typeToTupleCodeBlocks
            ).ToArray();

            return SourceTextFormatter.Format("", allBlocks);
        }

        /// <summary>
        ///     キャスト可能なタプル型の要素型リストを取得する。
        /// </summary>
        /// <param name="properties">プロパティ一覧</param>
        /// <returns>タプル型の要素型一覧</returns>
        private static IEnumerable<IEnumerable<string>> GetCanCastTupleTypes(IEnumerable<IPropertySymbol> properties)
        {
            var originalTypeList = new List<string>();
            var castedTypeList = new List<string?>();

            foreach (var propertySymbol in properties)
            {
                originalTypeList.Add(propertySymbol.Type.FullName());

                var propertyType = propertySymbol.Type;

                if (propertyType.IsAppliedAttribute(IntValueObjectAttribute.Instance.TypeFullName))
                {
                    castedTypeList.Add("int");
                }
                else if (propertyType.IsAppliedAttribute(ByteValueObjectAttribute.Instance.TypeFullName))
                {
                    castedTypeList.Add("byte");
                }
                else if (propertyType.IsAppliedAttribute(StringValueObjectAttribute.Instance.TypeFullName))
                {
                    castedTypeList.Add("string");
                }
                else
                {
                    castedTypeList.Add(null);
                }
            }

            if (castedTypeList.Count > 0 && castedTypeList.TrueForAll(s => s is not null))
            {
                // すべてのプロパティが SingleValueObject の場合はその値の実態でキャストできるようにする
                yield return castedTypeList.Cast<string>();
            }
            else
            {
                // 一つでも SingleValueObject ではないプロパティがあればプロパティの型でキャストできるようにする
                yield return originalTypeList;
            }
        }

        private MultiValueObjectGenerator()
        {
        }

        public static MultiValueObjectGenerator Instance { get; } = new();
    }
}
