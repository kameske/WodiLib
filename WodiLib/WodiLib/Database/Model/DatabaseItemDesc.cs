// ========================================
// Project Name : WodiLib
// File Name    : DatabaseItemDesc.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Sys;

namespace WodiLib.Database
{
    /// <summary>
    /// DB項目設定と設定値
    /// </summary>
    public class DatabaseItemDesc
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// [NotNull] 項目名
        /// </summary>
        /// <exception cref="PropertyNullException">nullを設定した場合</exception>
        public ItemName ItemName
        {
            get => Setting.ItemName;
            set => Setting.ItemName = value;
        }

        /// <summary>
        /// [NotNull DB項目特殊指定
        /// </summary>
        /// <exception cref="PropertyNullException">nullがセットされた場合</exception>
        public DBItemSpecialSettingDesc SpecialSettingDesc
        {
            get => Setting.SpecialSettingDesc;
            set => Setting.SpecialSettingDesc = value;
        }

        private DBItemType itemType = DBItemType.Int;

        /// <summary>
        /// DB項目種別
        /// </summary>
        /// <exception cref="PropertyNullException"></exception>
        public DBItemType ItemType
        {
            get => itemType;
            set
            {
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(ItemType)));

                itemType = value;
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private DBItemSetting Setting { get; } = new DBItemSetting();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// DBItemSetting のインスタンスを生成する。
        /// </summary>
        /// <returns>生成したインスタンス</returns>
        public DBItemSetting ToDBItemSetting()
        {
            return new DBItemSetting
            {
                ItemName = ItemName,
                SpecialSettingDesc = SpecialSettingDesc,
                ItemType = ItemType,
            };
        }
    }
}