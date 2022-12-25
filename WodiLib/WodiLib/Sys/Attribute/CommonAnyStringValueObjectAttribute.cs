// ========================================
// Project Name : WodiLib
// File Name    : CommonAnyStringValueObjectAttribute.cs
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
    ///     汎用単一文字列値オブジェクトの設定属性
    /// </summary>
    /// <remarks>
    ///     この属性はデフォルト設定で何の制限も設けない。
    /// </remarks>
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal class CommonAnyStringValueObjectAttribute : StringValueObjectAttribute
    {
        /// <inheritdoc/>
        [DefaultValueAttribute(CastType.Implicit)]
        public override CastType CastType { get; init; } = default!;
    }
}
