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
    internal class VariableAddressGapCalculatableAttribute : BinaryOperateAttribute
    {
        /// <inheritdoc/>
        [DefaultValue(BinaryOperationType.Subtract)]
        public override BinaryOperationType Operation { get; init; } = default!;

        /// <inheritdoc/>
        [DefaultValue(new[] { typeof(VariableAddress) })]
        public override Type[] OtherTypes { get; init; } = default!;

        /// <inheritdoc/>
        [DefaultValue(typeof(int))]
        public override Type InnerCastType { get; init; } = default!;

        /// <inheritdoc/>
        [DefaultValue(BinaryOperateOtherPosition.Right)]
        public override BinaryOperateOtherPosition OtherPosition { get; init; } = default!;

        /// <inheritdoc/>
        [DefaultValue(typeof(int))]
        public override Type ReturnType { get; init; } = default!;

        /// <inheritdoc/>
        [DefaultValue(OperationResultReturnCodeType.ImplicitCast)]
        public override OperationResultReturnCodeType ReturnCodeType { get; init; } = default!;
    }
}
