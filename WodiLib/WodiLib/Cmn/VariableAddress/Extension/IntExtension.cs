// ========================================
// Project Name : WodiLib
// File Name    : IntExtension.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;

namespace WodiLib.Cmn
{
    /// <summary>
    /// int拡張クラス
    /// </summary>
    public static class IntExtension
    {
        /// <summary>
        /// この数値が変数アドレス値として適切か判定する。
        /// </summary>
        /// <param name="src">対象</param>
        /// <returns>変数アドレス値として適切な場合true</returns>
        public static bool IsVariableAddress(this int src)
        {
            try
            {
                VariableAddressFactory.Create(src);
                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                return false;
            }
        }
    }
}