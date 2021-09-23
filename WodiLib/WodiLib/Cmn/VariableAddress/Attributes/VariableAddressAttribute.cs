// ========================================
// Project Name : WodiLib
// File Name    : VariableAddressAttribute.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;
using WodiLib.SourceGenerator.ValueObject.Attributes;
using WodiLib.SourceGenerator.ValueObject.Enums;

namespace WodiLib.Cmn
{
    /// <summary>
    /// 変数アドレス値クラス自動生成用属性
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class VariableAddressAttribute : IntValueObjectAttribute
    {
        /// <inheritDoc/>
        [DefaultValueAttribute(CastType.Implicit)]
        public override CastType CastType { get; init; } = default!;

        /// <inheritDoc/>
        [DefaultValueAttribute(true)]
        public override bool IsComparable { get; init; } = default!;

        /// <inheritDoc/>
        [DefaultValue(
            IntegralNumericOperation.IncreaseAndDecreasable
            | IntegralNumericOperation.Compare)]
        public override IntegralNumericOperation Operations { get; init; } = default!;

        /// <inheritDoc/>
        [DefaultValue(new[] { typeof(int) })]
        public override Type[]? AddAndSubtractTypes { get; init; }

        /// <inheritDoc/>
        [DefaultValue(new[] { typeof(VariableAddress) })]
        public override Type[]? CompareOtherTypes { get; init; }
    }
}
