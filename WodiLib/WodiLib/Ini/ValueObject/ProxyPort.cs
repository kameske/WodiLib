// ========================================
// Project Name : WodiLib
// File Name    : ProxyPort.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Sys;

namespace WodiLib.Ini
{
    /// <summary>
    ///     [Range(-1, 65535)] プロキシポート番号
    /// </summary>
    [CommonIntValueObject(MinValue = -1, MaxValue = 65535)]
    public partial class ProxyPort
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      Constants
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>空白の場合の値</summary>
        public static readonly ProxyPort Empty = -1;
    }
}
