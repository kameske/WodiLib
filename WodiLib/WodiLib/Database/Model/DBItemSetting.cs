// ========================================
// Project Name : WodiLib
// File Name    : DBItemSetting.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using WodiLib.Sys;

namespace WodiLib.Database
{
    /// <summary>
    /// DB項目設定
    /// </summary>
    [Serializable]
    public class DBItemSetting
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private ItemName itemName = "";

        /// <summary>
        /// [NotNull] 項目名
        /// </summary>
        /// <exception cref="PropertyNullException">nullを設定した場合</exception>
        public ItemName ItemName
        {
            get => itemName;
            set
            {
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(ItemName)));

                itemName = value;
            }
        }

        private DBItemSpecialSettingDesc specialSettingDesc = new DBItemSpecialSettingDesc();

        /// <summary>
        /// [NotNull DB項目特殊指定
        /// </summary>
        /// <exception cref="PropertyNullException">nullがセットされた場合</exception>
        public DBItemSpecialSettingDesc SpecialSettingDesc
        {
            get => specialSettingDesc;
            set
            {
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(SpecialSettingDesc)));

                specialSettingDesc = value;

                // 予め設定されていた項目種別が不適切な場合、整合性を保つために項目種別を強制変更する
                if (!SpecialSettingDesc.CanSetItemType(ItemType))
                {
                    itemType = specialSettingDesc.DefaultType;
                }
            }
        }

        private DBItemType itemType = DBItemType.Int;

        /// <summary>
        /// [NotNull] 項目値種別
        /// </summary>
        public DBItemType ItemType
        {
            get => itemType;
            set
            {
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(ItemType)));

                if (!SpecialSettingDesc.CanSetItemType(value))
                    throw new PropertyException(
                        $"現在の設定では指定できない値種別がセットされました。(設定値：{value})");

                itemType = value;
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public bool Equals(DBItemSetting other)
        {
            if (other == null) return false;
            if (ReferenceEquals(this, other)) return true;

            if (!ItemName.Equals(other.ItemName)) return false;
            if (!ItemType.Equals(other.ItemType)) return false;
            if (!SpecialSettingDesc.Equals(other.specialSettingDesc)) return false;

            return true;
        }
    }
}