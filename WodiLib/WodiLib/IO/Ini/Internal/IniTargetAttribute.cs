// ========================================
// Project Name : WodiLib
// File Name    : IniTargetAttribute.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using WodiLib.Sys;

namespace WodiLib.IO
{
    /// <summary>
    /// Ini対象属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    internal class IniTargetAttribute : Attribute
    {
        /// <summary>
        /// サポート対象最小バージョン
        /// </summary>
        public WoditorVersion SupportMinVersion { get; }

        /// <summary>
        /// サポート対象最大バージョン
        /// </summary>
        public WoditorVersion SupportMaxVersion { get; }

        public IniTargetAttribute(
            string minVersionName = nameof(WoditorVersion.Ver1_20),
            string maxVersionName = nameof(WoditorVersion.Latest))
        {
            SupportMinVersion = WoditorVersion.FromName(minVersionName);
            SupportMaxVersion = WoditorVersion.FromName(maxVersionName);
        }
    }
}