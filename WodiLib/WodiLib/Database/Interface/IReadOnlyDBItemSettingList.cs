// ========================================
// Project Name : WodiLib
// File Name    : IReadOnlyDBItemSettingList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Sys;

namespace WodiLib.Database
{
    /// <summary>
    /// 【読み取り専用】DB項目設定リスト
    /// </summary>
    public interface IReadOnlyDBItemSettingList : IReadOnlyRestrictedCapacityCollection<DBItemSetting>
    {
    }
}