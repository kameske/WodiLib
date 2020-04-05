// ========================================
// Project Name : WodiLib
// File Name    : DBTypeSetting.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using WodiLib.Sys;

namespace WodiLib.Database
{
    /// <summary>
    /// DBタイプ設定
    /// </summary>
    [Serializable]
    public class DBTypeSetting : ModelBase<DBTypeSetting>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private TypeName typeName = "";

        /// <summary>[NotNull] DBタイプ名</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public TypeName TypeName
        {
            get => typeName;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(TypeName)));

                typeName = value;
                NotifyPropertyChanged();
            }
        }

        private DataNameList dataNameList = new DataNameList();

        /// <summary>
        /// [NotNull] データ名リスト
        /// </summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public DataNameList DataNameList
        {
            get => dataNameList;
            set
            {
                if (value is null)
                    throw new ArgumentNullException(
                        ErrorMessage.NotNull(nameof(DataNameList)));
                dataNameList = value;
                NotifyPropertyChanged();
            }
        }

        private DBItemSettingList itemSettingList = new DBItemSettingList();

        /// <summary>
        /// [NotNull] 項目設定リスト
        /// </summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public DBItemSettingList ItemSettingList
        {
            get => itemSettingList;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(ItemSettingList)));
                itemSettingList = value;
                NotifyPropertyChanged();
            }
        }

        private DatabaseMemo memo = "";

        /// <summary>[NotNull] メモ</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public DatabaseMemo Memo
        {
            get => memo;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(Memo)));

                memo = value;
                NotifyPropertyChanged();
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
        public override bool Equals(DBTypeSetting other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return typeName == other.typeName
                   && memo == other.memo
                   && dataNameList.Equals(other.dataNameList)
                   && itemSettingList.Equals(other.itemSettingList);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Common
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// バイナリ変換する。
        /// </summary>
        /// <returns>バイナリデータ</returns>
        public byte[] ToBinary()
        {
            var result = new List<byte>();

            // タイプ名
            result.AddRange(TypeName.ToWoditorStringBytes());

            // 項目数 & 項目名
            result.AddRange(ItemSettingList.ToBinaryInItemName());

            // データ数 & データ名
            result.AddRange(DataNameList.ToBinary());

            // メモ
            result.AddRange(Memo.ToWoditorStringBytes());

            // 特殊指定
            result.AddRange(ItemSettingList.ToBinaryInSpecialSettingDesc());

            return result.ToArray();
        }

        /// <summary>
        /// DBTypeSetで使用できるようバイナリ変換する。
        /// </summary>
        /// <returns>バイナリデータ</returns>
        public byte[] ToBinaryForDBTypeSet()
        {
            var result = new List<byte>();

            // 項目数 + 設定種別 & 種別順列
            result.AddRange(ItemSettingList.ToBinaryItemTypeList());

            // タイプ名
            result.AddRange(TypeName.ToWoditorStringBytes());

            // 項目数 & 項目名
            result.AddRange(ItemSettingList.ToBinaryInItemName());

            // メモ
            result.AddRange(Memo.ToWoditorStringBytes());

            // 特殊指定
            result.AddRange(ItemSettingList.ToBinaryInSpecialSettingDesc());

            return result.ToArray();
        }
    }
}