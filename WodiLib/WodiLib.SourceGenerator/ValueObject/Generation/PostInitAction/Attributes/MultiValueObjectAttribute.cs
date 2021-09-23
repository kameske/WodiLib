// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : MultiValueObjectAttribute.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using WodiLib.SourceGenerator.Core.Dtos;
using WodiLib.SourceGenerator.Core.SourceAddables.PostInitialize;

namespace WodiLib.SourceGenerator.ValueObject.Generation.PostInitAction.Attributes
{
    /// <summary>
    ///     複数値オブジェクトに付与する属性プロパティ生成用クラス
    /// </summary>
    internal class MultiValueObjectAttribute : InitializeAttributeSourceAddable
    {
        public override string NameSpace => GenerationConst.NameSpaces.Attributes;
        public override string AttributeName => nameof(MultiValueObjectAttribute);

        public override string Summary => "複数値からなる値を表すValueObject";

        public static readonly PropertyInfo CastType = new()
        {
            Name = nameof(CastType),
            Type = Enums.CastType.Instance.TypeFullName,
            Summary = "キャスト種別",
            DefaultValue = Enums.CastType.Code_None
        };

        /// <inheritdoc/>
        public override AttributeTargets AttributeTargets
            => AttributeTargets.Class | AttributeTargets.Struct;

        public override IEnumerable<PropertyInfo> Properties()
            => new[]
            {
                CastType
            };

        public static MultiValueObjectAttribute Instance { get; } = new();
    }
}
