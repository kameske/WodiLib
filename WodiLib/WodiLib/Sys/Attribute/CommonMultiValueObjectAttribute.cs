// ========================================
// Project Name : WodiLib
// File Name    : CommonMultiValueObjectAttribute.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.ComponentModel;
using WodiLib.SourceGenerator.ValueObject.Attributes;

namespace WodiLib.Sys
{
    /// <summary>
    ///     汎用複数値オブジェクトの設定属性
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal class CommonMultiValueObjectAttribute : MultiValueObjectAttribute
    {
    }
}
