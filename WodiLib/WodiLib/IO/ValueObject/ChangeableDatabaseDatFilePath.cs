// ========================================
// Project Name : WodiLib
// File Name    : ChangeableDatabaseDatFilePath.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Database;
using WodiLib.Sys;

namespace WodiLib.IO
{
    /// <summary>
    ///     可変データベースデータファイル名
    /// </summary>
    [FilePathStringObjectValue(
        SafetyPattern = @"^CDataBase\.dat$"
    )]
    public partial class ChangeableDatabaseDatFilePath : DatabaseDatFilePath
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Override Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>DB種別</summary>
        public override DBKind DBKind => DBKind.Changeable;
    }
}
