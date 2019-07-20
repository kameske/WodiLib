// ========================================
// Project Name : WodiLib
// File Name    : IIniTarget.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.IO
{
    /// <summary>
    /// Iniターゲットインタフェース
    /// </summary>
    internal interface IIniTarget
    {
        /// <summary>
        /// セクション名
        /// </summary>
        string SectionName { get; }
    }
}