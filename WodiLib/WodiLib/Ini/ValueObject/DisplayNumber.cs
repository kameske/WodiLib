// ========================================
// Project Name : WodiLib
// File Name    : DisplayNumber.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Sys;

namespace WodiLib.Ini
{
    /// <summary>
    ///     [Range(0, int.MaxValue)] ディスプレイ番号
    /// </summary>
    [CommonIntValueObject(MinValue = 0)]
    public partial class DisplayNumber
    {
    }
}
