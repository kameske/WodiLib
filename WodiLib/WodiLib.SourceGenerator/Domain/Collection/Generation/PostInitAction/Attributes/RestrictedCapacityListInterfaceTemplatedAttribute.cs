// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : RestrictedCapacityListInterfaceTemplatedAttribute.cs
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
    internal class RestrictedCapacityListInterfaceTemplatedAttribute : InitializeAttributeSourceAddable
    {
        /// <inheritdoc/>
        public override string AttributeName => nameof(RestrictedCapacityListInterfaceTemplatedAttribute);

        /// <inheritdoc/>
        public override string NameSpace => GenerationConst.NameSpaces.Attributes;

        /// <inheritdoc/>
        public override string Summary => "テンプレートを用いたリストインタフェース生成情報";

        public override bool AllowMultiple => false;

        public static readonly PropertyInfo Description = new()
        {
            Name = nameof(Description),
            Type = "string",
            Summary = "インタフェース説明",
            DefaultValue = "",
        };

        public static readonly PropertyInfo InterfaceInItemType = new()
        {
            Name = nameof(InterfaceInItemType),
            Type = typeof(Type).FullName,
            Summary = "リスト要素内包型",
            DefaultValue = null,
        };

        public static readonly PropertyInfo InterfaceOutItemType = new()
        {
            Name = nameof(InterfaceOutItemType),
            Type = typeof(Type).FullName,
            Summary = "リスト要素出力型",
            DefaultValue = null,
        };

        public static readonly PropertyInfo IsImplementDeepCloneableList = new()
        {
            Name = nameof(IsImplementDeepCloneableList),
            Type = "bool",
            Summary = "<see cref=\"Sys.Collections.IDeepCloneableList{T,TIn}\"/> 自動実装フラグ",
            DefaultValue = "true",
            DefaultValueAsSourceCode = true,
        };

        /// <inheritdoc/>
        public override AttributeTargets AttributeTargets
            => AttributeTargets.Interface;

        /// <inheritdoc/>
        public override IEnumerable<PropertyInfo> Properties()
            => new[]
            {
                Description,
                InterfaceInItemType,
                InterfaceOutItemType,
                IsImplementDeepCloneableList
            };

        private RestrictedCapacityListInterfaceTemplatedAttribute()
        {
        }

        public static RestrictedCapacityListInterfaceTemplatedAttribute Instance { get; } = new();
    }
}
