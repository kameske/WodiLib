// ========================================
// Project Name : WodiLib
// File Name    : IConvertibleDBValueInt.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.Database
{
    /// <summary>
    /// DBValueIntに変換できることを示すインタフェース
    /// </summary>
    public interface IConvertibleDBValueInt
    {
        /// <summary>
        /// DBValueIntに変換する。
        /// </summary>
        /// <returns>DBValueInt値</returns>
        DBValueInt ToDBValueInt();
    }
}