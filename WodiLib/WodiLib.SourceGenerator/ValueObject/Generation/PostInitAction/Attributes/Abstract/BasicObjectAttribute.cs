// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : BasicObjectAttribute.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using WodiLib.SourceGenerator.Core.Dtos;
using WodiLib.SourceGenerator.Core.SourceAddables.PostInitialize;
using WodiLib.SourceGenerator.Core.SourceBuilder;

namespace WodiLib.SourceGenerator.ValueObject.Generation.PostInitAction.Attributes.Abstract
{
    /// <summary>
    ///     オブジェクトに付与する属性プロパティ生成用テンプレートクラス
    /// </summary>
    internal abstract class BasicObjectAttribute : InitializeAttributeSourceAddable
    {
        public override bool Inherited => false;

        public static readonly PropertyInfo OverrideBasicMethods = new()
        {
            Name = nameof(OverrideBasicMethods),
            Type = "bool",
            Summary = $"{Tag.See.Cref("System.Object")} のメソッド自動派生フラグ",
            Remarks =
                Tag.Para(
                    $"{Tag.See.Cref("object.Equals(object)")}, {Tag.See.Cref("object.Equals(object)")}, {Tag.See.Cref("object.Equals(object)")} をオーバーライドする。")
                + Tag.Para($"対象が Record の場合、設定値によらず {Tag.See.Langword_False} とみなされる。"),
            DefaultValue = "true"
        };

        public static readonly PropertyInfo ImplementEquatable = new()
        {
            Name = nameof(ImplementEquatable),
            Type = "bool",
            Summary = $"{Tag.See.Cref("System.IEquatable{T}")} 自動実装フラグ",
            Remarks = Tag.Para($"対象が Record の場合、設定値によらず {Tag.See.Langword_False} とみなされる。"),
            DefaultValue = "true"
        };

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

        /// <inheritdoc/>
        public override IEnumerable<PropertyInfo> Properties()
            => new[]
            {
                OverrideBasicMethods,
                ImplementEquatable,
                CastType
            };
    }
}
