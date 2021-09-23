// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : MainSourceAddableTemplate.WorkState.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using WodiLib.SourceGenerator.Core.Dtos;
using WodiLib.SourceGenerator.Core.Extensions;

namespace WodiLib.SourceGenerator.Core.Templates.FromAttribute
{
    internal abstract partial class MainSourceAddableTemplate
    {
        /// <summary>
        ///     内部ワーク状態管理クラス
        /// </summary>
        internal class WorkState
        {
            /// <summary>コンテキスト</summary>
            public GeneratorExecutionContext Context { get; }

            /// <summary>現在注目しているクラスの属性プロパティ値</summary>
            public PropertyValues PropertyValues => Current.PropertyValues;

            /// <summary>現在注目しているクラスのターゲットシンボル</summary>
            public INamedTypeSymbol? CurrentSymbol => Current.Current.TargetSymbol;

            /// <summary>タイプ定義解決処理</summary>
            public ITypeDefinitionInfoResolver TypeDefinitionInfoResolver { get; }

            /// <summary>同じ基底属性プロパティが付与されたクラスの解析結果一覧</summary>
            public ISyntaxWorkResultDictionary SyntaxWorkResultDict { get; }

            /// <summary>プロパティデフォルト値</summary>
            public AnalyzedPropertyValueDictionary PropertyDefaultValueDict { get; }

            /// <summary>ロガー</summary>
            private ILogger? Logger { get; }

            /// <summary>同じ基底属性プロパティが付与されたクラスのプロパティ値ディクショナリ</summary>
            private IPropertyValueDictionary PropertyValuesDict { get; }

            /// <summary>
            ///     <see cref="MainSourceAddableTemplate.AddSource"/>処理Current情報
            /// </summary>
            private CurrentInfo Current { get; set; } = default!;

            /// <summary>
            ///     現在注目しているクラスの先祖クラスが同じ属性プロパティを付与されている場合の、そのプロパティ値
            /// </summary>
            private IReadOnlyList<PropertyValues>? AncestorValues { get; set; }

            /// <summary>
            ///     ソース生成したSyntaxWorkResult管理用
            /// </summary>
            private SourceGeneratedTargetList GeneratedTargetList { get; } = new();

            public WorkState(GeneratorExecutionContext context,
                ITypeDefinitionInfoResolver typeDefinitionInfoResolver,
                ISyntaxWorkResultDictionary syntaxWorkResultDict,
                AnalyzedPropertyValueDictionary propertyDefaultValueDict,
                IPropertyValueDictionary propertyValuesDict,
                ILogger? logger)
            {
                Context = context;
                TypeDefinitionInfoResolver = typeDefinitionInfoResolver;
                SyntaxWorkResultDict = syntaxWorkResultDict;
                PropertyDefaultValueDict = propertyDefaultValueDict;
                PropertyValuesDict = propertyValuesDict;
                Logger = logger;
            }

            public string Namespace => PropertyValues.Namespace;
            public string Name => PropertyValues.Name;
            public string FullName => PropertyValues.FullName;

            public bool IsExtended => AncestorValues is not null;

            public TypeDefinitionInfo CurrentTypeDefinitionInfo
                => Current.TypeDefinitionInfo;

            public TypeDefinitionInfo ResolveTypeDefinitionInfo(string typeName)
                => TypeDefinitionInfoResolver.Resolve(typeName);

            private SourceAttributeDefaultValueMap CurrentDefaultValues
                => PropertyDefaultValueDict.Get(Current.SrcAttributeName);

            /// <summary>
            ///     カレント情報のプロパティ値初期化有無を取得する。
            /// </summary>
            /// <param name="propertyName">プロパティ名</param>
            /// <returns>プロパティ値にデフォルト値以外が設定されている場合 <see langword="true"/></returns>
            public bool IsPropertyInitialized(string propertyName)
            {
                var currentValue = PropertyValues[propertyName];
                var defaultValue = GetCurrentDefaultPropertyValue(propertyName);

                return !string.Equals(currentValue, defaultValue);
            }

            /// <summary>
            ///     カレント情報のプロパティ値初期化有無を取得する。
            /// </summary>
            /// <param name="propertyNames">プロパティ名リスト</param>
            /// <returns>いずれかのプロパティ値にデフォルト値以外が設定されている場合 <see langword="true"/></returns>
            public bool IsAnyPropertyInitialized(params string[] propertyNames)
                => propertyNames.Any(IsPropertyInitialized);

            /// <summary>
            ///     カレント情報のプロパティ値上書き有無を取得する。
            /// </summary>
            /// <param name="propertyName">プロパティ名</param>
            /// <returns>プロパティ値が上書きされている場合 <see langword="true"/></returns>
            public bool IsPropertyOverwritten(string propertyName)
                => IsPropertyInitialized(propertyName)
                   && (AncestorValues?.Any(values => values.IsPropertyInitialized(propertyName)) ?? false);

            /// <summary>
            ///     カレント情報のプロパティ値上書き有無を取得する。
            /// </summary>
            /// <param name="propertyNames">プロパティ名リスト</param>
            /// <returns>プロパティ値が一つでも上書きされている場合 <see langword="true"/></returns>
            public bool IsAnyPropertyOverwritten(params string[] propertyNames)
                => propertyNames.Any(IsPropertyOverwritten);

            /// <summary>
            ///     ソース生成が可能かどうかを返す。
            /// </summary>
            /// <param name="currentResult">生成対象情報</param>
            /// <returns>生成可能な場合 <see langword="true"/></returns>
            public bool CanGenerateSource(SyntaxWorkResult currentResult)
            {
                if (GeneratedTargetList.Contains(currentResult))
                {
                    Logger?.AppendLine(
                        $"SyntaxWorkResult: {{{currentResult}}} Cannot GenerateSource Because of Already Generated. ");
                    return false;
                }

                GeneratedTargetList.Add(currentResult);

                return TypeDefinitionInfoResolver.CanResolve(currentResult.FullName);
            }

            /// <returns>継承元クラスのプロパティ値（継承されていない場合<see langword="null"/></returns>
            public string? GetParentPropertyValue(string propertyName)
                => AncestorValues?.Select(values => values[propertyName])
                    .FirstOrDefault(value => value is not null);

            /// <returns>継承元クラスのプロパティ値（継承されていない場合 <paramref name="ifNullValue"/></returns>
            public string GetOrDefaultParentPropertyValue(string propertyName, string ifNullValue)
                => GetParentPropertyValue(propertyName) ?? ifNullValue;

            /// <param name="propertyName">プロパティ名</param>
            /// <returns>注目しているクラスのプロパティデフォルト値</returns>
            private string? GetCurrentDefaultPropertyValue(string propertyName)
                => CurrentDefaultValues[propertyName].ToValueString();

            /// <summary>
            ///     注目するクラスを変更する。
            /// </summary>
            /// <param name="currentResult">注目クラスの解析結果</param>
            public void SetCurrent(SyntaxWorkResult currentResult)
            {
                var currentValues =
                    PropertyValuesDict.SetupPropertyValues(currentResult, PropertyDefaultValueDict);
                var currentTypeDef = TypeDefinitionInfoResolver.Resolve(currentValues.FullName);
                Current = new CurrentInfo(currentResult, currentValues, currentTypeDef);

                AncestorValues = PropertyValuesDict.GetAncestorValues(currentResult,
                    PropertyDefaultValueDict, SyntaxWorkResultDict);
            }

            /// <summary>
            ///     注目クラス情報
            /// </summary>
            private class CurrentInfo
            {
                public SyntaxWorkResult Current { get; }
                public PropertyValues PropertyValues { get; }
                public TypeDefinitionInfo TypeDefinitionInfo { get; }

                public string SrcAttributeName => Current.SrcAttributeName;

                public CurrentInfo(SyntaxWorkResult current, PropertyValues propertyValues,
                    TypeDefinitionInfo typeDefinitionInfo)
                {
                    Current = current;
                    PropertyValues = propertyValues;
                    TypeDefinitionInfo = typeDefinitionInfo;
                }
            }

            /// <summary>
            ///     ソース生成したSyntaxWorkResult管理クラス
            /// </summary>
            private class SourceGeneratedTargetList
            {
                private List<SourceGeneratedTargetInfo> Impl { get; } = new();

                public bool Contains(SyntaxWorkResult workResult)
                {
                    var targetInfo = new SourceGeneratedTargetInfo(workResult);
                    return Impl.Contains(targetInfo, SourceGeneratedTargetInfo.SymbolAttrDataComparer);
                }

                public void Add(SyntaxWorkResult workResult)
                {
                    var targetInfo = new SourceGeneratedTargetInfo(workResult);
                    Impl.Add(targetInfo);
                }

                public class SourceGeneratedTargetInfo
                {
                    private INamedTypeSymbol? Symbol { get; }
                    private AttributeData AttrData { get; }

                    public SourceGeneratedTargetInfo(SyntaxWorkResult workResult)
                    {
                        Symbol = workResult.TargetSymbol;
                        AttrData = workResult.SrcAttributeData;
                    }

                    public override string ToString()
                        => $"TargetSymbol: ${Symbol?.FullName() ?? "<<NULL>>"}, "
                           + $"AttributeData: {AttrData.AttributeClass?.FullName()}";

                    private sealed class SymbolAttrDataEqualityComparer : IEqualityComparer<SourceGeneratedTargetInfo>
                    {
                        public bool Equals(SourceGeneratedTargetInfo x, SourceGeneratedTargetInfo y)
                        {
                            if (ReferenceEquals(x, y)) return true;
                            if (ReferenceEquals(x, null)) return false;
                            if (ReferenceEquals(y, null)) return false;
                            if (x.GetType() != y.GetType()) return false;
                            return SymbolEqualityComparer.Default.Equals(x.Symbol, y.Symbol) &&
                                   x.AttrData.Equals(y.AttrData);
                        }

                        public int GetHashCode(SourceGeneratedTargetInfo obj)
                        {
                            unchecked
                            {
#pragma warning disable RS1024
                                // RS1024: Compare symbols correctly
                                return (obj.Symbol?.GetHashCode() ?? 0 * 397) ^ obj.AttrData.GetHashCode();
#pragma warning restore RS1024
                            }
                        }
                    }

                    public static IEqualityComparer<SourceGeneratedTargetInfo> SymbolAttrDataComparer { get; } =
                        new SymbolAttrDataEqualityComparer();
                }
            }
        }
    }
}
