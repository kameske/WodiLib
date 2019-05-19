// ========================================
// Project Name : WodiLib
// File Name    : IConvertibleDBValueString.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.Database
{
    /// <summary>
    /// DBValueStringに変換できることを示すインタフェース
    /// </summary>
    public interface IConvertibleDBValueString
    {
        /// <summary>
        /// DBValueStringに変換する。
        /// </summary>
        /// <returns>DBValueString値</returns>
        DBValueString ToDBValueString();
    }
}