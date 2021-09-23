// ========================================
// Project Name : WodiLib
// File Name    : CommonEventMemo.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using System.ComponentModel;
using WodiLib.Sys;

namespace WodiLib.Common
{
    /// <summary>
    ///     コモンイベントメモ
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    [CommonOneLineStringValueObject]
    public partial class CommonEventMemo
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     ウディタ文字列のbyte配列に変換する。
        /// </summary>
        /// <returns>ウディタ文字列のbyte配列</returns>
        public IEnumerable<byte> ToWoditorStringBytes()
        {
            var woditorStr = new WoditorString(RawValue);
            return woditorStr.StringByte;
        }
    }
}
