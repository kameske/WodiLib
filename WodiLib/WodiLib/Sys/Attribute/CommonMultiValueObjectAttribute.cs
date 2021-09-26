// ========================================
// Project Name : WodiLib
// File Name    : CommonMultiValueObjectAttribute.cs
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
    ///     汎用複数値オブジェクトの設定属性
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class CommonMultiValueObjectAttribute : MultiValueObjectAttribute
    {
        /// <inheritdoc/>
        [DefaultValue(CastType.Implicit)]
        public override CastType CastType { get; init; } = default!;
    }
}
