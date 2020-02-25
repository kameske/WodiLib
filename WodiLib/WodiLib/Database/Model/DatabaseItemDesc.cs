// ========================================
// Project Name : WodiLib
// File Name    : DatabaseItemDesc.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using WodiLib.Sys;

namespace WodiLib.Database
{
    /// <summary>
    /// DB項目設定と設定値
    /// </summary>
    [Serializable]
    public class DatabaseItemDesc : IEquatable<DatabaseItemDesc>, ISerializable
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
                if (value is null)
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
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DatabaseItemDesc()
        {
        }

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

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public bool Equals(DatabaseItemDesc other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return itemType == other.itemType
                   && Setting.Equals(other.Setting);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Serializable
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// オブジェクトをシリアル化するために必要なデータを設定する。
        /// </summary>
        /// <param name="info">デシリアライズ情報</param>
        /// <param name="context">コンテキスト</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(itemType), itemType.Code);
            info.AddValue(nameof(Setting), Setting);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="info">デシリアライズ情報</param>
        /// <param name="context">コンテキスト</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected DatabaseItemDesc(SerializationInfo info, StreamingContext context)
        {
            itemType = DBItemType.FromCode(info.GetInt32(nameof(itemType)));
            Setting = info.GetValue<DBItemSetting>(nameof(Setting));
        }
    }
}