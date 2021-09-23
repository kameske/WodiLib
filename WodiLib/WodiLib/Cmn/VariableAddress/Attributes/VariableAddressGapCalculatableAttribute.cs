// ========================================
// Project Name : WodiLib
// File Name    : VariableAddressGapCalculatableAttribute.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;
using WodiLib.SourceGenerator.Operation.Attributes;
using WodiLib.SourceGenerator.Operation.Enums;

namespace WodiLib.Cmn
{
    /// <summary>
    ///     変数アドレス値の差分計算演算子オーバーロード定義属性
    /// </summary>
    public class VariableAddressGapCalculatableAttribute : BinaryOperateAttribute
    {
        /// <inheritDoc/>
        [DefaultValue(BinaryOperationType.Subtract)]
        public override BinaryOperationType Operation { get; init; } = default!;

        /// <inheritDoc/>
        [DefaultValueAttribute(new[] { typeof(VariableAddress) })]
        public override Type[] OtherTypes { get; init; } = default!;

        /// <inheritDoc/>
        [DefaultValue(typeof(int))]
        public override Type InnerCastType { get; init; } = default!;

        /// <inheritDoc/>
        [DefaultValue(BinaryOperateOtherPosition.Right)]
        public override BinaryOperateOtherPosition OtherPosition { get; init; } = default!;

        /// <inheritDoc/>
        [DefaultValueAttribute(typeof(int))]
        public override Type ReturnType { get; init; } = default!;

        /// <inheritDoc/>
        [DefaultValue(OperationResultReturnCodeType.ImplicitCast)]
        public override OperationResultReturnCodeType ReturnCodeType { get; init; } = default!;
    }
}
