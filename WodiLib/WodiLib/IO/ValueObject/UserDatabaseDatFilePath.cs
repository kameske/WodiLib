// ========================================
// Project Name : WodiLib
// File Name    : UserDatabaseDatFilePath.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Database;
using WodiLib.Sys;

namespace WodiLib.IO
{
    /// <summary>
    ///     ユーザデータベースデータファイル名
    /// </summary>
    [FilePathStringObjectValue(SafetyPattern = @"^DataBase\.dat$")]
    public partial class UserDatabaseDatFilePath : DatabaseDatFilePath
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Override Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>DB種別</summary>
        public override DBKind DBKind => DBKind.System;
    }
}
