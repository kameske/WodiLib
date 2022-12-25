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
    internal class CommonIntValueObjectAttribute : IntValueObjectAttribute
    {
        /// <inheritdoc/>
        [DefaultValueAttribute(CastType.Implicit)]
        public override CastType CastType { get; init; } = default!;

        /// <inheritdoc/>
        [DefaultValueAttribute(true)]
        public override bool IsComparable { get; init; } = default!;

        // /// <inheritdoc/>
        // [DefaultValueAttribute(
        //     IntegralNumericOperation.IncreaseAndDecreasable
        //     | IntegralNumericOperation.Compare
        // )]
        // public override IntegralNumericOperation Operations { get; init; } = default!;
        //
        // /// <inheritdoc/>
        // [DefaultValueAttribute(new[]{typeof(int)})]
        // public override Type[]? AddAndSubtractTypes { get; init; } = default!;
        //
        // /// <inheritdoc/>
        // [DefaultValueAttribute(new[]{typeof(int)})]
        // public override Type[]? MultipleAndDivideOtherTypes { get; init; } = default!;
        //
        // /// <inheritdoc/>
        // [DefaultValueAttribute(new[] { typeof(int) })]
        // public override Type[]? CompareOtherTypes { get; init; } = default!;
    }
}
