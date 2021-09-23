// ========================================
// Project Name : WodiLib
// File Name    : DBDataFilePath.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Cmn;
using WodiLib.Sys;

namespace WodiLib.IO
{
    /// <summary>
    ///     XXX.dbdata ファイルパス
    /// </summary>
    [FilePathStringObjectValue(
        SafetyPattern = @"^(.*)\.dbdata$"
    )]
    public partial class DBDataFilePath : FilePath
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Override Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>空文字許可フラグ</summary>
        protected override bool IsAllowEmptyString => false;
    }
}
