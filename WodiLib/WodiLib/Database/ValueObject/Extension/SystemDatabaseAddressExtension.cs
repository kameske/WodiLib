// ========================================
// Project Name : WodiLib
// File Name    : SystemDatabaseAddressExtension.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Cmn;
using WodiLib.Sys;

namespace WodiLib.Database
{
    /// <summary>
    /// システムDBアドレス値拡張クラス
    /// </summary>
    public static class SystemDatabaseAddressExtension
    {
        /// <summary>
        /// タイプIDを取得する。
        /// </summary>
        /// <param name="src">対象</param>
        /// <returns>タイプID</returns>
        public static TypeId GetTypeId(this SystemDatabaseAddress src)
        {
            var value = (int) src;
            return (TypeId) value.SubInt(6, 2);
        }

        /// <summary>
        /// 項目IDを取得する。
        /// </summary>
        /// <param name="src">対象</param>
        /// <returns>項目ID</returns>
        public static DataId GetDataId(this SystemDatabaseAddress src)
        {
            var value = (int) src;
            return (DataId) value.SubInt(2, 4);
        }

        /// <summary>
        /// 項目IDを取得する。
        /// </summary>
        /// <param name="src">対象</param>
        /// <returns>項目ID</returns>
        public static ItemId GetItemId(this SystemDatabaseAddress src)
        {
            var value = (int) src;
            return (ItemId) value.SubInt(0, 2);
        }
    }
}