// ========================================
// Project Name : WodiLib
// File Name    : CommonEventSpecialArgCase.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Sys;

namespace WodiLib.Common
{
    /// <summary>
    ///     コモンイベント特殊指定選択肢
    /// </summary>
    [CommonMultiValueObject]
    public partial record CommonEventSpecialArgCase
    {
        /// <summary>選択肢番号</summary>
        public CommonEventSpecialArgCaseNumber CaseNumber { get; init; }

        /// <summary>選択肢内容</summary>
        public CommonEventSpecialArgCaseDescription Description { get; init; }
    }
}
