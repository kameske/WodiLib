// ========================================
// Project Name : WodiLib
// File Name    : DBItemSetting.cs
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
    /// DB項目設定
    /// </summary>
    [Serializable]
    public class DBItemSetting : ModelBase<DBItemSetting>, ISerializable
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
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(ItemName)));

                itemName = value;
                NotifyPropertyChanged();
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
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(SpecialSettingDesc)));

                specialSettingDesc = value;
                NotifyPropertyChanged();

                // 予め設定されていた項目種別が不適切な場合、整合性を保つために項目種別を強制変更する
                if (!SpecialSettingDesc.CanSetItemType(ItemType))
                {
                    itemType = specialSettingDesc.DefaultType;
                    NotifyPropertyChanged(nameof(ItemType));
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
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(ItemType)));

                if (!SpecialSettingDesc.CanSetItemType(value))
                    throw new PropertyException(
                        $"現在の設定では指定できない値種別がセットされました。(設定値：{value})");

                itemType = value;
                NotifyPropertyChanged();
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DBItemSetting()
        {
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public override bool Equals(DBItemSetting other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (ReferenceEquals(null, other)) return false;

            return ItemType == other.ItemType
                   && ItemName == other.ItemName
                   && SpecialSettingDesc.Equals(other.specialSettingDesc);
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
            info.AddValue(nameof(itemName), itemName);
            info.AddValue(nameof(specialSettingDesc), specialSettingDesc);
            info.AddValue(nameof(itemType), itemType.Code);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="info">デシリアライズ情報</param>
        /// <param name="context">コンテキスト</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected DBItemSetting(SerializationInfo info, StreamingContext context)
        {
            itemName = info.GetValue<ItemName>(nameof(itemName));
            specialSettingDesc = info.GetValue<DBItemSpecialSettingDesc>(nameof(specialSettingDesc));
            itemType = DBItemType.FromCode(info.GetInt32(nameof(itemType)));
        }
    }
}