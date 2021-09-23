// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : IPropertyValueDictionary.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using WodiLib.SourceGenerator.Core.Dtos;
using static WodiLib.SourceGenerator.Core.Templates.FromAttribute.MainSourceAddableTemplate;

namespace WodiLib.SourceGenerator.Core.Templates.FromAttribute
{
    /// <summary>同じ属性プロパティが付与されたクラスのプロパティ値ディクショナリ</summary>
    internal interface IPropertyValueDictionary : IEnumerable<PropertyValues>
    {
        /// <summary>
        ///     解析結果の先祖クラスのプロパティ値リストディクショナリ取得する。
        /// </summary>
        /// <param name="workResult">解析結果</param>
        /// <param name="propertyDefaultValueDict">属性のデフォルト値リスト</param>
        /// <param name="syntaxWorkResults">解析結果ディクショナリ</param>
        /// <returns>親クラスのプロパティ値リスト</returns>
        IReadOnlyList<PropertyValues>? GetAncestorValues(SyntaxWorkResult workResult,
            AnalyzedPropertyValueDictionary propertyDefaultValueDict,
            ISyntaxWorkResultDictionary syntaxWorkResults);

        /// <summary>
        ///     プロパティ値ディクショナリを使用できるよう準備して返す。
        /// </summary>
        /// <param name="workResult">解析結果</param>
        /// <param name="propertyDefaultValueDict">属性のデフォルト値リスト</param>
        /// <returns>プロパティ値ディクショナリ</returns>
        PropertyValues SetupPropertyValues(
            SyntaxWorkResult workResult,
            AnalyzedPropertyValueDictionary propertyDefaultValueDict);
    }
}
