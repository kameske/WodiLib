// ========================================
// Project Name : WodiLib
// File Name    : IReadOnlyDBItemValueList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;

namespace WodiLib.Database
{
    /// <summary>
    /// 【読み取り専用】DBデータ設定値リスト
    /// </summary>
    public interface IReadOnlyDBItemValueList : IReadOnlyList<DBItemValue>
    {
        /// <summary>
        /// 長さ固定リストに変換する。
        /// </summary>
        /// <returns>インスタンス</returns>
        IFixedLengthDBItemValueList ToFixedLengthList();
    }
}