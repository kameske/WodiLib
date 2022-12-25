// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : FixedLengthListInterfaceTemplateAttribute.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using WodiLib.SourceGenerator.Core.Dtos;
using WodiLib.SourceGenerator.Core.SourceAddables.PostInitialize;

namespace WodiLib.SourceGenerator.Domain.Collection.Generation.PostInitAction.Attributes
{
    /// <summary>
    ///     テンプレートを用いたリストインタフェース生成情報
    /// </summary>
    internal class FixedLengthListInterfaceTemplateAttribute : InitializeAttributeSourceAddable
    {
        /// <inheritdoc/>
        public override string AttributeName => nameof(FixedLengthListInterfaceTemplateAttribute);

        /// <inheritdoc/>
        public override string NameSpace => GenerationConst.NameSpaces.Attributes;

        /// <inheritdoc/>
        public override string Summary => "テンプレートを用いた <see cref=\"WodiLib.Sys.Collections.IFixedLengthList{T}\"/> 生成情報";

        public override bool AllowMultiple => false;

        public static readonly PropertyInfo Description = new()
        {
            Name = nameof(Description),
            Type = "string",
            Summary = "インタフェース説明",
            DefaultValue = "",
        };

        public static readonly PropertyInfo InterfaceItemType = new()
        {
            Name = nameof(InterfaceItemType),
            Type = typeof(Type).FullName,
            Summary = "リスト要素内包型",
            DefaultValue = null,
        };

        /// <inheritdoc/>
        public override AttributeTargets AttributeTargets
            => AttributeTargets.Interface;

        /// <inheritdoc/>
        public override IEnumerable<PropertyInfo> Properties()
            => new[]
            {
                Description,
                InterfaceItemType
            };

        private FixedLengthListInterfaceTemplateAttribute()
        {
        }

        public static FixedLengthListInterfaceTemplateAttribute Instance { get; } = new();
    }
}
