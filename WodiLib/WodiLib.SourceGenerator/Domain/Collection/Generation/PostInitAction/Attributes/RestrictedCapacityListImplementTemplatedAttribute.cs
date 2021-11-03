// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : RestrictedCapacityListImplementTemplatedAttribute.cs
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
    ///     テンプレートを用いたリスト実装クラス生成情報
    /// </summary>
    internal class RestrictedCapacityListImplementTemplatedAttribute : InitializeAttributeSourceAddable
    {
        /// <inheritdoc/>
        public override string AttributeName => nameof(RestrictedCapacityListImplementTemplatedAttribute);

        /// <inheritdoc/>
        public override string NameSpace => GenerationConst.NameSpaces.Attributes;

        /// <inheritdoc/>
        public override string Summary => "テンプレートを用いたリスト実装クラス生成情報";

        public override bool AllowMultiple => false;

        public static readonly PropertyInfo InterfaceType = new()
        {
            Name = nameof(InterfaceType),
            Type = typeof(Type).FullName,
            Summary = "生成元インタフェース型",
            DefaultValue = null,
        };

        public static readonly PropertyInfo MaxCapacity = new()
        {
            Name = nameof(MaxCapacity),
            Type = "int",
            Summary = "最大容量",
            DefaultValue = "int.MaxValue",
            DefaultValueAsSourceCode = true,
        };

        public static readonly PropertyInfo MinCapacity = new()
        {
            Name = nameof(MinCapacity),
            Type = "int",
            Summary = "最小容量",
            DefaultValue = "0",
            DefaultValueAsSourceCode = true,
        };

        public static readonly PropertyInfo InterfaceInternalItemType = new()
        {
            Name = nameof(InterfaceInternalItemType),
            Type = typeof(Type).FullName,
            Summary = "リスト要素実態型",
            DefaultValue = null,
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

        public static readonly PropertyInfo IsOverrideMakeDefaultItem = new()
        {
            Name = nameof(IsOverrideMakeDefaultItem),
            Type = "bool",
            Summary = "MakeDefaultItem メソッド自動生成フラグ",
            DefaultValue = true,
        };

        public static readonly PropertyInfo IsOverrideMakeInstance = new()
        {
            Name = nameof(IsOverrideMakeInstance),
            Type = "bool",
            Summary = "MakeInstance メソッド自動生成フラグ",
            DefaultValue = true,
        };

        public static readonly PropertyInfo IsOverrideCloneToInternal = new()
        {
            Name = nameof(IsOverrideCloneToInternal),
            Type = "bool",
            Summary = "CloneToInternal メソッド自動生成フラグ（TIn と TOut が別の型の場合のみ必要となるメソッド）",
            DefaultValue = true,
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
            => AttributeTargets.Class;

        /// <inheritdoc/>
        public override IEnumerable<PropertyInfo> Properties()
            => new[]
            {
                InterfaceType,
                MaxCapacity,
                MinCapacity,
                InterfaceInternalItemType,
                InterfaceInItemType,
                InterfaceOutItemType,
                IsOverrideMakeDefaultItem,
                IsOverrideMakeInstance,
                IsOverrideCloneToInternal,
                IsImplementDeepCloneableList
            };

        private RestrictedCapacityListImplementTemplatedAttribute()
        {
        }

        public static RestrictedCapacityListImplementTemplatedAttribute Instance { get; } = new();
    }
}
