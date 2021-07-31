// ========================================
// Project Name : WodiLib
// File Name    : IReadOnlyDataNameList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Sys;
using WodiLib.Sys.Collections;

namespace WodiLib.Database
{
    /// <summary>
    /// 【読み取り専用】DBデータ名リスト
    /// </summary>
    public interface IReadOnlyDataNameList : IReadOnlyExtendedList<DataName>
    {
    }
}
