// ========================================
// Project Name : WodiLib
// File Name    : ListConstant.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.Sys
{
    /// <summary>
    /// リスト定数クラス
    /// </summary>
    public static class ListConstant
    {
        /// <summary>
        /// 要素が変更された際のプロパティ変更通知に使用するプロパティ名
        /// </summary>
        public static string IndexerName { get; } = "Item[]";
    }
}