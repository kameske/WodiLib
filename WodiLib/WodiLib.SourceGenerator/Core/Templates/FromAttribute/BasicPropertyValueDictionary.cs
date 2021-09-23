// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : BasicPropertyValueDictionary.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using WodiLib.SourceGenerator.Core.Dtos;
using static WodiLib.SourceGenerator.Core.Templates.FromAttribute.MainSourceAddableTemplate;

namespace WodiLib.SourceGenerator.Core.Templates.FromAttribute
{
    internal class BasicPropertyValueDictionary : IPropertyValueDictionary
    {
        /*
         * 第一キー：INamedTypeSymbol から解決したキー名
         * 第二キー：AttributeData から解決したキー名
         */
        private Dictionary<string, Dictionary<string, List<PropertyValues>>> Impl { get; } = new();

        private IPropertyValueKeyResolver KeyResolver { get; }

        private ILogger? Logger { get; }

        private List<PropertyValues> this[SyntaxWorkResult workResult]
        {
            get
            {
                var (classKey, attrKey) = KeyResolver.Resolve(workResult);
                Logger?.AppendLine($"BasicPropertyValueDictionary Get Key: {classKey}, {attrKey}");
                return Impl[classKey][attrKey];
            }
        }

        private IReadOnlyDictionary<string, List<PropertyValues>> this[INamedTypeSymbol symbol]
            => Impl[KeyResolver.Resolve(symbol)];

        public BasicPropertyValueDictionary(IPropertyValueKeyResolver keyResolver, ILogger? logger)
        {
            KeyResolver = keyResolver;
            Logger = logger;
        }

        public bool Contains(INamedTypeSymbol symbol)
            => Impl.ContainsKey(KeyResolver.Resolve(symbol));

        public bool Contains(SyntaxWorkResult workResult)
        {
            var (classKey, attrKey) = KeyResolver.Resolve(workResult);
            return Impl.ContainsKey(classKey) && Impl[classKey].ContainsKey(attrKey);
        }

        /// <inheritDoc/>
        public virtual IReadOnlyList<PropertyValues>? GetAncestorValues(SyntaxWorkResult workResult,
            AnalyzedPropertyValueDictionary propertyDefaultValueDict,
            ISyntaxWorkResultDictionary syntaxWorkResults)
        {
            var currentBaseType = workResult.TargetSymbol?.BaseType;
            return currentBaseType is null
                ? null
                : GetPropertyValues(currentBaseType, syntaxWorkResults, propertyDefaultValueDict);
        }

        public IEnumerator<PropertyValues> GetEnumerator()
            => Impl.Values
                .SelectMany(innerDict => innerDict.Values.SelectMany(list => list))
                .GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        /// <inheritDoc/>
        public PropertyValues SetupPropertyValues(SyntaxWorkResult workResult,
            AnalyzedPropertyValueDictionary propertyDefaultValueDict)
        {
            if (Contains(workResult))
            {
                var equalItem = this[workResult].FirstOrDefault(values => values.WorkResult == workResult);
                if (equalItem is not null)
                {
                    return equalItem;
                }
            }

            var result = new PropertyValues(
                workResult,
                propertyDefaultValueDict.Get(workResult.SrcAttributeName).ToReadOnlyDictionary());

            var (classKey, attrKey) = KeyResolver.Resolve(workResult);
            if (!Impl.ContainsKey(classKey))
            {
                Impl[classKey] = new Dictionary<string, List<PropertyValues>>();
            }

            if (!Impl[classKey].ContainsKey(attrKey))
            {
                Impl[classKey][attrKey] = new List<PropertyValues>();
            }

            Impl[classKey][attrKey].Add(result);

            return result;
        }

        private IReadOnlyList<PropertyValues> SetupPropertyValues(
            IReadOnlyDictionary<string, List<SyntaxWorkResult>> workResults,
            AnalyzedPropertyValueDictionary propertyDefaultValueDict)
        {
            return workResults.Values.SelectMany(results =>
                results.Select(workResult =>
                    SetupPropertyValues(workResult, propertyDefaultValueDict))
            ).ToList();
        }

        /// <summary>
        ///     プロパティ値ディクショナリを取得する。
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="syntaxWorkResults">対象解析結果</param>
        /// <param name="propertyDefaultValueDict">属性プロパティデフォルト値リスト</param>
        /// <returns><see cref="PropertyValues"/> インスタンス</returns>
        private IReadOnlyList<PropertyValues>? GetPropertyValues(INamedTypeSymbol target,
            ISyntaxWorkResultDictionary syntaxWorkResults,
            AnalyzedPropertyValueDictionary propertyDefaultValueDict)
        {
            var currentTarget = target;
            while (true)
            {
                if (Contains(target))
                {
                    return this[target].Values
                        .SelectMany(list => list)
                        .ToList();
                }

                if (!syntaxWorkResults.HasWorkResult(currentTarget))
                {
                    var baseType = currentTarget.BaseType;
                    if (baseType is null) return null;
                    currentTarget = baseType;
                    continue;
                }

                var syntaxWorkResult = syntaxWorkResults[currentTarget];
                var result = SetupPropertyValues(syntaxWorkResult, propertyDefaultValueDict);
                return result;
            }
        }
    }
}
