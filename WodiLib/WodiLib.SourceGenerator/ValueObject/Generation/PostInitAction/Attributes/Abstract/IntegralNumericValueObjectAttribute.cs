// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : IntegralNumericValueObjectAttribute.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Linq;
using WodiLib.SourceGenerator.Core.Dtos;
using WodiLib.SourceGenerator.Core.SourceBuilder;
using WodiLib.SourceGenerator.ValueObject.Generation.PostInitAction.Enums;

namespace WodiLib.SourceGenerator.ValueObject.Generation.PostInitAction.Attributes.Abstract
{
    /// <summary>
    ///     単一数値オブジェクトに付与する属性プロパティ生成用テンプレートクラス
    /// </summary>
    internal abstract class IntegralNumericValueObjectAttribute : SingleValueObjectAttribute
    {
        public static readonly PropertyInfo Operations = new()
        {
            Name = nameof(Operations),
            Type = IntegralNumericOperation.Instance.TypeFullName,
            Summary = "オーバーロード演算子",
            DefaultValue = 0 // None
        };

        public static readonly PropertyInfo AddAndSubtractTypes = new()
        {
            Name = nameof(AddAndSubtractTypes),
            Type = "System.Type[]?",
            Summary = "加減算の右項型。",
            DefaultValue = "null"
        };

        public static readonly PropertyInfo MultipleAndDivideOtherTypes = new()
        {
            Name = nameof(MultipleAndDivideOtherTypes),
            Type = "System.Type[]?",
            Summary = "乗除算の右項型。",
            DefaultValue = "null"
        };

        public static readonly PropertyInfo CompareOtherTypes = new()
        {
            Name = nameof(CompareOtherTypes),
            Type = "System.Type[]?",
            Summary = "比較の右項型。",
            DefaultValue = "null"
        };

        public static readonly PropertyInfo IsUseBasicFormattable = new()
        {
            Name = nameof(IsUseBasicFormattable),
            Type = "bool",
            Summary = $"標準{Tag.See.Cref(typeof(IFormattable).FullName)}の実装フラグ",
            DefaultValue = "true"
        };

        /* Dummy */
        public static readonly PropertyInfo MaxValue = new()
        {
            Name = nameof(MaxValue),
            Type = "int",
            Summary = "許容最大値",
            DefaultValue = "0"
        };

        /* Dummy */
        public static readonly PropertyInfo MinValue = new()
        {
            Name = nameof(MinValue),
            Type = "int",
            Summary = "許容最小値",
            DefaultValue = "0"
        };

        /* Dummy */
        public static readonly PropertyInfo SafetyMaxValue = new()
        {
            Name = nameof(SafetyMaxValue),
            Type = "int",
            Summary = "安全に使用できる上限値",
            DefaultValue = "0"
        };

        /* Dummy */
        public static readonly PropertyInfo SafetyMinValue = new()
        {
            Name = nameof(SafetyMinValue),
            Type = "int",
            Summary = "安全に使用できる下限値",
            DefaultValue = "0"
        };

        public override string Summary => $"単一の{WrapType}を表すValueObject";

        /// <inheritdoc/>
        public override IEnumerable<PropertyInfo> Properties()
            => base.Properties().Concat(new[]
            {
                Operations,
                AddAndSubtractTypes,
                MultipleAndDivideOtherTypes,
                CompareOtherTypes,
                IsUseBasicFormattable
            });

        /// <summary>内包型</summary>
        protected abstract string WrapType { get; }
    }
}
