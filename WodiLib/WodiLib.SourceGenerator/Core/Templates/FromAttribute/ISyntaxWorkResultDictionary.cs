// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : ISyntaxWorkResultDictionary.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using WodiLib.SourceGenerator.Core.Dtos;

namespace WodiLib.SourceGenerator.Core.Templates.FromAttribute
{
    /// <summary>
    ///     プロパティ値ディクショナリラッパー
    /// </summary>
    internal interface ISyntaxWorkResultDictionary : IEnumerable<SyntaxWorkResult>
    {
        /// <summary>解析結果</summary>
        IReadOnlyDictionary<string, List<SyntaxWorkResult>> this[INamedTypeSymbol target] { get; }

        /// <summary>解析結果件数</summary>
        int Count { get; }

        /// <summary>
        ///     解析結果を追加する。
        /// </summary>
        /// <param name="workResult">解析結果</param>
        /// <return>追加したキー名</return>
        void Add(SyntaxWorkResult workResult);

        /// <summary>
        ///     指定したシンボルの解析結果が存在するかどうかを返す。
        /// </summary>
        /// <param name="target">判定対象</param>
        /// <returns>存在する場合 <see langword="true"/></returns>
        bool HasWorkResult(INamedTypeSymbol target);
    }
}
