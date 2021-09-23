// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : IMainSourceAddable.cs
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
    ///     属性情報から<see cref="GeneratorExecutionContext"/> に自動生成クラスのソースを追加できることを示すインタフェース
    /// </summary>
    internal interface IMainSourceAddable : Core.IMainSourceAddable
    {
        /// <summary>
        ///     属性プロパティのデフォルト値をセットする。
        /// </summary>
        /// <param name="attributeName">属性名</param>
        /// <param name="defaultValues">プロパティ名と値のディクショナリ</param>
        /// <param name="parentAttrName">対象属性が継承されたものである場合、継承元の属性名</param>
        void PutPropertyDefaultValues(string attributeName, Dictionary<string, PropertyValue> defaultValues,
            string? parentAttrName);
    }
}
