// ========================================
// Project Name : WodiLib
// File Name    : CommonByteValueObjectAttribute.cs
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
    ///     汎用単一byte値オブジェクトの設定属性
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class CommonByteValueObjectAttribute : ByteValueObjectAttribute
    {
        /// <inheritDoc/>
        [DefaultValueAttribute(CastType.Implicit)]
        public override CastType CastType { get; init; } = default!;

        /// <inheritDoc/>
        [DefaultValueAttribute(true)]
        public override bool IsComparable { get; init; } = default!;
    }
}
