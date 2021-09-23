// ========================================
// Project Name : WodiLib
// File Name    : VariableAddressAddAndSubtractableAttribute.cs
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
    ///     変数アドレス値の +, - 演算子オーバーロード定義属性
    /// </summary>
    public class VariableAddressAddAndSubtractableAttribute : BinaryOperateAttribute
    {
        /// <inheritDoc/>
        [DefaultValue(BinaryOperationType.AddAndSubtract)]
        public override BinaryOperationType Operation { get; init; } = default!;

        /// <inheritDoc/>
        [DefaultValue(typeof(int))]
        public override Type InnerCastType { get; init; } = default!;

        /// <inheritDoc/>
        [DefaultValue(BinaryOperateOtherPosition.Right)]
        public override BinaryOperateOtherPosition OtherPosition { get; init; } = default!;

        /// <inheritDoc/>
        [DefaultValue(typeof(int))]
        public override Type ReturnType { get; init; } = default!;

        /// <inheritDoc/>
        [DefaultValueAttribute(OperationResultReturnCodeType.ImplicitCast)]
        public override OperationResultReturnCodeType ReturnCodeType { get; init; } = default!;
    }
}
