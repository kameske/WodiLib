// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : BasicSyntaxWorkResultDictionary.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using WodiLib.SourceGenerator.Core.Dtos;

namespace WodiLib.SourceGenerator.Core.Templates.FromAttribute
{
    /// <summary>
    ///     構文解析結果ディクショナリ
    /// </summary>
    internal class BasicSyntaxWorkResultDictionary : ISyntaxWorkResultDictionary
    {
        public IReadOnlyDictionary<string, List<SyntaxWorkResult>> this[INamedTypeSymbol target]
            => Impl[KeyResolver.Resolve(target)];

        public int Count
            => Impl.Values
                .Aggregate(0, (cnt, dict) => cnt + dict.Values.Aggregate(0,
                    (cnt2, list) => cnt2 + list.Count));

        private ILogger? Logger { get; }

        private IPropertyValueKeyResolver KeyResolver { get; }

        private Dictionary<string, Dictionary<string, List<SyntaxWorkResult>>> Impl { get; } = new();

        public BasicSyntaxWorkResultDictionary(IPropertyValueKeyResolver keyResolver,
            ILogger? logger)
        {
            KeyResolver = keyResolver;
            Logger = logger;
        }

        public void Add(SyntaxWorkResult workResult)
        {
            var (targetKeyName, srcKeyName) = KeyResolver.Resolve(workResult);
            if (!Impl.ContainsKey(targetKeyName))
            {
                Impl[targetKeyName] = new Dictionary<string, List<SyntaxWorkResult>>();
            }

            if (!Impl[targetKeyName].ContainsKey(srcKeyName))
            {
                Impl[targetKeyName][srcKeyName] = new List<SyntaxWorkResult>();
            }

            Impl[targetKeyName][srcKeyName].Add(workResult);
            var itemLength = Impl[targetKeyName][srcKeyName].Count;
            Logger?.AppendLine(
                $"BaseSyntaxWorkResultDictionary Add for key: {targetKeyName}, {srcKeyName} ({itemLength} items)");
        }

        /// <inheritDoc/>
        public IEnumerator<SyntaxWorkResult> GetEnumerator()
            => Impl.Values
                .SelectMany(innerDict => innerDict.Values)
                .SelectMany(list => list)
                .GetEnumerator();

        /// <inheritDoc/>
        public bool HasWorkResult(INamedTypeSymbol target)
            => Impl.ContainsKey(KeyResolver.Resolve(target));

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}
