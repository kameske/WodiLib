// ========================================
// Project Name : WodiLib
// File Name    : WoditorInt.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.SourceGenerator.ValueObject.Enums;

namespace WodiLib.Sys
{
    /// <summary>
    ///     ウディタ仕様の数値
    /// </summary>
    [CommonIntValueObject(
        MaxValue = 2000000000, MinValue = -2000000000,
        Operations = IntegralNumericOperation.All
    )]
    internal partial class WoditorInt
    {
    }
}
