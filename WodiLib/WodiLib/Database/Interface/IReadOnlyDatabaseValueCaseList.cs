// ========================================
// Project Name : WodiLib
// File Name    : IReadOnlyDatabaseValueCaseList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Sys;
using WodiLib.Sys.Collections;

namespace WodiLib.Database
{
    /// <summary>
    /// 読み取り専用DB選択肢情報リスト
    /// </summary>
    public interface IReadOnlyDatabaseValueCaseList : IReadOnlyRestrictedCapacityList<DatabaseValueCase>
    {
    }
}
