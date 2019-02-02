// ========================================
// Project Name : WodiLib
// File Name    : IWodiLibObject.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.Sys
{
    /// <summary>
    ///     WodiLib内のオブジェクト共通のインタフェース
    /// </summary>
    public interface IWodiLibObject
    {
        /// <summary>バイナリデータに変換する</summary>
        /// <returns>バイナリデータ</returns>
        byte[] ToBinary();
    }
}