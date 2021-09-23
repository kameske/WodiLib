// ========================================
// Project Name : WodiLib
// File Name    : MemberId.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Sys;

namespace WodiLib.Cmn
{
    /// <summary>
    ///     [Range(1, 5)] 仲間ID
    /// </summary>
    [CommonIntValueObject(MinValue = 1, MaxValue = 5)]
    public partial class MemberId
    {
    }
}
