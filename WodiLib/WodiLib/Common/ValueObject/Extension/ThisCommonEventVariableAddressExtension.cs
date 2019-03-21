// ========================================
// Project Name : WodiLib
// File Name    : ThisCommonEventVariableAddressExtension.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Cmn;
using WodiLib.Sys;

namespace WodiLib.Common
{
    /// <summary>
    /// 「このコモンイベント変数」拡張クラス
    /// </summary>
    public static class ThisCommonEventVariableAddressExtension
    {
        /// <summary>
        /// コモンイベントセルフ変数インデックスを取得する。
        /// </summary>
        /// <param name="src">対象</param>
        /// <returns>コモンイベントセルフ変数インデックス</returns>
        public static CommonEventSelfVariableIndex GetIndex(this ThisCommonEventVariableAddress src)
        {
            return new CommonEventSelfVariableIndex(((int) src).SubInt(0, 2));
        }
    }
}