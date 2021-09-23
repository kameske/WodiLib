// ========================================
// Project Name : WodiLib
// File Name    : DBTypeFilePath.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Cmn;
using WodiLib.Sys;

namespace WodiLib.IO
{
    /// <summary>
    ///     XXX.dbtype ファイルパス
    /// </summary>
    [FilePathStringObjectValue(SafetyPattern = @"^(.*)\.dbtype$")]
    public partial class DBTypeFilePath : FilePath
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Override Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>空文字許可フラグ</summary>
        protected override bool IsAllowEmptyString => false;
    }
}
