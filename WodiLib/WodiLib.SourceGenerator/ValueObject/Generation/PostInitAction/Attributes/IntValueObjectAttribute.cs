// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : IntValueObjectAttribute.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using System.Linq;
using WodiLib.SourceGenerator.Core.Dtos;
using WodiLib.SourceGenerator.ValueObject.Generation.PostInitAction.Attributes.Abstract;

namespace WodiLib.SourceGenerator.ValueObject.Generation.PostInitAction.Attributes
{
    /// <summary>
    ///     単一数値オブジェクトに付与する属性プロパティ生成用クラス
    /// </summary>
    internal class IntValueObjectAttribute : IntegralNumericValueObjectAttribute
    {
        private new static readonly PropertyInfo MaxValue = new()
        {
            Name = "MaxValue",
            Type = "int",
            Summary = "許容最大値",
            DefaultValue = "int.MaxValue",
            DefaultValueAsSourceCode = true
        };

        private new static readonly PropertyInfo MinValue = new()
        {
            Name = "MinValue",
            Type = "int",
            Summary = "許容最小値",
            DefaultValue = "int.MinValue",
            DefaultValueAsSourceCode = true
        };

        private new static readonly PropertyInfo SafetyMaxValue = new()
        {
            Name = "SafetyMaxValue",
            Type = "int",
            Summary = "安全に使用できる上限値",
            DefaultValue = "int.MaxValue",
            DefaultValueAsSourceCode = true
        };

        private new static readonly PropertyInfo SafetyMinValue = new()
        {
            Name = "SafetyMinValue",
            Type = "int",
            Summary = "安全に使用できる下限値",
            DefaultValue = "int.MinValue",
            DefaultValueAsSourceCode = true
        };

        protected override string WrapType => "int";
        public override string NameSpace => GenerationConst.NameSpaces.Attributes;
        public override string AttributeName => nameof(IntValueObjectAttribute);

        public override IEnumerable<PropertyInfo> Properties()
            => base.Properties().Concat(new[]
            {
                MaxValue,
                MinValue,
                SafetyMaxValue,
                SafetyMinValue
            });

        public static IntValueObjectAttribute Instance { get; } = new();
    }
}
