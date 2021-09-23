// ========================================
// Project Name : WodiLib
// File Name    : CommonEventSpecialArgCaseNumber.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Sys;

namespace WodiLib.Common
{
    /// <summary>
    /// [Range(-2000000000, 2000000000)] コモンイベント特殊指定選択肢・選択肢番号
    /// </summary>
    [CommonIntValueObject(
        MinValue = -2000000000 /* = WoditorInt.Const_MinValue */,
        MaxValue = 2000000000 /* = WoditorInt.Const_MaxValue */
        )]
    public partial class CommonEventSpecialArgCaseNumber
    {
    }
}
