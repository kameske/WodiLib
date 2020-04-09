// ========================================
// Project Name : WodiLib
// File Name    : IReadOnlyCommonEventSpecialArgCaseList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Sys;

namespace WodiLib.Common
{
    /// <summary>
    /// 読み取り専用選択肢情報リスト
    /// </summary>
    public interface IReadOnlyCommonEventSpecialArgCaseList
        : IReadOnlyRestrictedCapacityCollection<CommonEventSpecialArgCase>
    {
    }
}