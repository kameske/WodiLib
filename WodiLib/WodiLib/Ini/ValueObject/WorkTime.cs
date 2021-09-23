// ========================================
// Project Name : WodiLib
// File Name    : WorkTime.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Sys;

namespace WodiLib.Ini
{
    /// <summary>
    ///     [Range(0, int.MaxValue)] 作業時間（単位は1/2minute）
    /// </summary>
    [CommonIntValueObject(MinValue = 0, MaxValue = int.MaxValue)]
    public partial class WorkTime
    {
    }
}
