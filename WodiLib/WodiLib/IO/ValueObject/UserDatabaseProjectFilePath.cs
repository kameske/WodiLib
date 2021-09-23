// ========================================
// Project Name : WodiLib
// File Name    : UserDatabaseProjectFilePath.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Database;
using WodiLib.Sys;

namespace WodiLib.IO
{
    /// <summary>
    ///     ユーザデータベースプロジェクトファイル名
    /// </summary>
    [FilePathStringObjectValue(SafetyPattern = @"^DataBase\.project$")]
    public partial class UserDatabaseProjectFilePath : DatabaseProjectFilePath
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Override Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>DB種別</summary>
        public override DBKind DBKind => DBKind.System;
    }
}
