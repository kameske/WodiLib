// ========================================
// Project Name : WodiLib
// File Name    : CommonOneLineStringValueObjectAttribute.cs
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
    /// 汎用単一文字列値オブジェクトの設定属性
    /// </summary>
    /// <remarks>
    /// この属性はデフォルト設定で改行コードを許さない。
    /// </remarks>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class CommonOneLineStringValueObjectAttribute : StringValueObjectAttribute
    {
        /// <inheritDoc/>
        [DefaultValueAttribute(CastType.Implicit)]
        public override CastType CastType { get; init; } = default!;

        /// <inheritDoc/>
        [DefaultValueAttribute(false)]
        public override bool IsAllowNewLine { get; init; } = default!;
    }
}
