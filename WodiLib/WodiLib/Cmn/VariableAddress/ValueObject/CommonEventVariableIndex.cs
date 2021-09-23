// ========================================
// Project Name : WodiLib
// File Name    : CommonEventVariableIndex.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Sys;

namespace WodiLib.Cmn
{
    /// <summary>
    ///     [Range(0, 99)] コモンイベントセルフ変数インデックス
    /// </summary>
    [CommonIntValueObject(MinValue = 0, MaxValue = 99)]
    public partial class CommonEventVariableIndex
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>文字列セルフ変数インデックス最大値</summary>
        public static readonly int StringIndex_MaxValue = 9;

        /// <summary>文字列セルフ変数インデックス最小値</summary>
        public static readonly int StringIndex_MinValue = 5;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>文字列フラグ</summary>
        public bool IsStringIndex => StringIndex_MinValue <= RawValue && RawValue <= StringIndex_MaxValue;
    }
}
