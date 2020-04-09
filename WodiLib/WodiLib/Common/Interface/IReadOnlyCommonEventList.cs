// ========================================
// Project Name : WodiLib
// File Name    : IReadOnlyCommonEventList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Sys;

namespace WodiLib.Common
{
    /// <summary>
    /// 読み取り専用コモンイベントリスト
    /// </summary>
    public interface IReadOnlyCommonEventList : IReadOnlyRestrictedCapacityCollection<CommonEvent>
    {
    }
}