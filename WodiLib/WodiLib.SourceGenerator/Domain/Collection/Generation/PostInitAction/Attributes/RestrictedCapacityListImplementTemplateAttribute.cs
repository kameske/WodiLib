// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : RestrictedCapacityListImplementTemplateAttribute.cs
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
    internal class RestrictedCapacityListImplementTemplateAttribute : InitializeAttributeSourceAddable
    {
        /// <inheritdoc/>
        public override string AttributeName => nameof(RestrictedCapacityListImplementTemplateAttribute);

        /// <inheritdoc/>
        public override string NameSpace => GenerationConst.NameSpaces.Attributes;

        /// <inheritdoc/>
        public override string Summary
            => "テンプレートを用いた <see cref=\"WodiLib.Sys.Collections.RestrictedCapacityList{T,TImpl}\"/> 実装クラス生成情報";

        public override bool AllowMultiple => false;

        public static readonly PropertyInfo Description = new()
        {
            Name = nameof(Description),
            Type = "string",
            Summary = "クラス説明",
            DefaultValue = "",
        };

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
            Type = $"object",
            Summary = "最大容量",
            Remarks = "与えた値を ToString した結果をソースコードとして埋め込む。",
            DefaultValue = "int.MaxValue",
            DefaultValueAsSourceCode = true,
        };

        public static readonly PropertyInfo MinCapacity = new()
        {
            Name = nameof(MinCapacity),
            Type = $"object",
            Summary = "最小容量",
            Remarks = "与えた値を ToString した結果をソースコードとして埋め込む。",
            DefaultValue = 0,
        };

        public static readonly PropertyInfo InterfaceItemType = new()
        {
            Name = nameof(InterfaceItemType),
            Type = typeof(Type).FullName,
            Summary = "リスト要素内包型",
            DefaultValue = null,
        };

        public static readonly PropertyInfo IsAutoOverrideMakeDefaultItem = new()
        {
            Name = nameof(IsAutoOverrideMakeDefaultItem),
            Type = "bool",
            Summary = "MakeDefaultItem メソッド自動生成フラグ",
            Remarks = "自動生成する場合、内部要素型の引数なしコンストラクタを利用する。",
            DefaultValue = true,
        };

        /// <inheritdoc/>
        public override AttributeTargets AttributeTargets
            => AttributeTargets.Class;

        /// <inheritdoc/>
        public override IEnumerable<PropertyInfo> Properties()
            => new[]
            {
                Description,
                InterfaceType,
                MaxCapacity,
                MinCapacity,
                InterfaceItemType,
                IsAutoOverrideMakeDefaultItem,
            };

        private RestrictedCapacityListImplementTemplateAttribute()
        {
        }

        public static RestrictedCapacityListImplementTemplateAttribute Instance { get; } = new();
    }
}
