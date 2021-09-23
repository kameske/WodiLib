// ========================================
// Project Name : WodiLib
// File Name    : ChangeableDatabaseProjectFilePath.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Database;
using WodiLib.Sys;

namespace WodiLib.IO
{
    /// <summary>
    ///     可変データベースプロジェクトファイル名
    /// </summary>
    [FilePathStringObjectValue(
        SafetyPattern = @"^CDataBase\.project$"
    )]
    public partial class ChangeableDatabaseProjectFilePath : DatabaseProjectFilePath
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Override Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>DB種別</summary>
        public override DBKind DBKind => DBKind.Changeable;
    }
}
