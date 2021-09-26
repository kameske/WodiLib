// ========================================
// Project Name : WodiLib
// File Name    : CommonIntValueObjectAttribute.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.ComponentModel;
using WodiLib.SourceGenerator.ValueObject.Attributes;
using WodiLib.SourceGenerator.ValueObject.Enums;

namespace WodiLib.Sys
{
    /// <summary>
    ///     汎用単一int値オブジェクトの設定属性
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class CommonIntValueObjectAttribute : IntValueObjectAttribute
    {
        /// <inheritDoc/>
        [DefaultValueAttribute(CastType.Implicit)]
        public override CastType CastType { get; init; } = default!;

        /// <inheritDoc/>
        [DefaultValueAttribute(true)]
        public override bool IsComparable { get; init; } = default!;

        // /// <inheritDoc/>
        // [DefaultValueAttribute(
        //     IntegralNumericOperation.IncreaseAndDecreasable
        //     | IntegralNumericOperation.Compare
        // )]
        // public override IntegralNumericOperation Operations { get; init; } = default!;
        //
        // /// <inheritDoc/>
        // [DefaultValueAttribute(new[]{typeof(int)})]
        // public override Type[]? AddAndSubtractTypes { get; init; } = default!;
        //
        // /// <inheritDoc/>
        // [DefaultValueAttribute(new[]{typeof(int)})]
        // public override Type[]? MultipleAndDivideOtherTypes { get; init; } = default!;
        //
        // /// <inheritDoc/>
        // [DefaultValueAttribute(new[] { typeof(int) })]
        // public override Type[]? CompareOtherTypes { get; init; } = default!;
    }
}
