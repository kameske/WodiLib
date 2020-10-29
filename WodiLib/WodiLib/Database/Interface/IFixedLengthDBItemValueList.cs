// ========================================
// Project Name : WodiLib
// File Name    : IFixedLengthDBItemValueList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using WodiLib.Sys;

namespace WodiLib.Database
{
    /// <summary>
    /// 【容量固定】DBデータ設定値リスト
    /// </summary>
    public interface IFixedLengthDBItemValueList : IFixedLengthList<DBItemValue>, IEquatable<IFixedLengthDBItemValueList>
    {
        /// <summary>
        /// 自身と同じ値情報を持つ、DBItemValuesList に紐付けられていないインスタンスに変換する。
        /// </summary>
        /// <returns>DBデータ設定値リスト</returns>
        DBItemValueList ToLengthChangeableItemValueList();
    }
}
