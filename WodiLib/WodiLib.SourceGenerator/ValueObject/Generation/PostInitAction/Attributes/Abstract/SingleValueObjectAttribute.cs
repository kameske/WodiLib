// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : SingleValueObjectAttribute.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using System.Linq;
using WodiLib.SourceGenerator.Core.Dtos;
using WodiLib.SourceGenerator.Core.SourceBuilder;

namespace WodiLib.SourceGenerator.ValueObject.Generation.PostInitAction.Attributes.Abstract
{
    /// <summary>
    ///     単一値オブジェクトに付与する属性プロパティ生成用テンプレートクラス
    /// </summary>
    internal abstract class SingleValueObjectAttribute : BasicObjectAttribute
    {
        public static readonly PropertyInfo PropertyName = new()
        {
            Name = "PropertyName",
            Type = "string",
            Summary = "プロパティ名",
            DefaultValue = "RawValue"
        };

        public static readonly PropertyInfo IsComparable = new()
        {
            Name = "IsComparable",
            Type = "bool",
            Summary = "比較可否",
            Remarks =
                $"{Tag.See.Langword_True}の場合、{Tag.See.Cref($"{typeof(IComparer<>).Namespace}.IComparer{{T}}.Compare(T,T)")}を実装する。",
            DefaultValue = "false"
        };

        /// <inheritdoc/>
        public override IEnumerable<PropertyInfo> Properties()
            => base.Properties().Concat(new[]
            {
                PropertyName,
                IsComparable
            });
    }
}
