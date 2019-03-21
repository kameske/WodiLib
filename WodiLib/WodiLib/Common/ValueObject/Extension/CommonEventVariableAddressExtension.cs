// ========================================
// Project Name : WodiLib
// File Name    : CommonEventVariableAddressExtension.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Cmn;
using WodiLib.Sys;

namespace WodiLib.Common
{
    /// <summary>
    /// 「コモンイベント変数アドレス」拡張クラス
    /// </summary>
    public static class CommonEventVariableAddressExtension
    {
        /// <summary>
        /// コモンイベントセルフ変数インデックスを取得する。
        /// </summary>
        /// <param name="src">対象</param>
        /// <returns>コモンイベントセルフ変数インデックス</returns>
        public static CommonEventSelfVariableIndex GetIndex(this CommonEventVariableAddress src)
        {
            return new CommonEventSelfVariableIndex(((int) src).SubInt(0, 2));
        }

        /// <summary>
        /// コモンイベントIDを取得する。
        /// </summary>
        /// <param name="src">対象</param>
        /// <returns>コモンイベントセルフ変数インデックス</returns>
        public static CommonEventId GetCommonEventId(this CommonEventVariableAddress src)
        {
            return new CommonEventId(((int) src).SubInt(2, 3));
        }
    }
}