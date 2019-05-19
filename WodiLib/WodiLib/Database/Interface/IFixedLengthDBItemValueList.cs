// ========================================
// Project Name : WodiLib
// File Name    : IFixedLengthDBItemValueList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Sys;

namespace WodiLib.Database
{
    /// <summary>
    /// 【容量固定】DBデータ設定値リスト
    /// </summary>
    public interface IFixedLengthDBItemValueList : IFixedLengthCollection<DBItemValue>
    {
        /// <summary>
        /// 容量変更可能なDBデータ設定値リストに変換する。
        /// </summary>
        /// <returns>DBデータ設定値リスト</returns>
        DBItemValueList ToLengthChangeableItemValueList();
    }
}