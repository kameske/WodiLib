// ========================================
// Project Name : WodiLib
// File Name    : VersionConfigExtension.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Sys.Cmn;

namespace WodiLib.Sys
{
    /// <summary>
    /// VersionConfig 拡張クラス
    /// </summary>
    internal static class VersionConfigExtension
    {
        /// <summary>
        /// 設定されたバージョンが指定されたバージョン未満かどうかを返す。
        /// </summary>
        /// <param name="config">コンフィグ</param>
        /// <param name="version">[NotNull] 比較バージョン</param>
        /// <returns>コンフィグ設定バージョン &lt; version の場合true</returns>
        public static bool IsUnderVersion(this VersionConfig config, WoditorVersion version)
        {
            return config.Version < WoditorVersion.Ver2_00;
        }
    }
}