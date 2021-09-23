// ========================================
// Project Name : WodiLib
// File Name    : SystemDatabaseProjectFilePath.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Database;
using WodiLib.Sys;

namespace WodiLib.IO
{
    /// <summary>
    ///     システムデータベースプロジェクトファイル名
    /// </summary>
    [FilePathStringObjectValue(SafetyPattern = @"^SysDataBase\.project$")]
    public partial class SystemDatabaseProjectFilePath : DatabaseProjectFilePath
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Override Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>DB種別</summary>
        public override DBKind DBKind => DBKind.System;
    }
}
