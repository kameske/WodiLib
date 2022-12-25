// ========================================
// Project Name : WodiLib
// File Name    : VariableAddressAddAndSubtractableIntAttribute.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;

namespace WodiLib.Cmn
{
    /// <summary>
    ///     変数アドレス値とintの +, - 演算子オーバーロード定義属性
    /// </summary>
    internal class VariableAddressAddAndSubtractableIntAttribute : VariableAddressAddAndSubtractableAttribute
    {
        /// <inheritdoc/>
        [DefaultValue(new[] { typeof(int) })]
        public override Type[] OtherTypes { get; init; } = default!;
    }
}
