// ========================================
// Project Name : WodiLib
// File Name    : DBType.cs
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
    /// DBタイプ（XXX.dbtype）
    /// </summary>
    [Serializable]
    public class DBType : IEquatable<DBType>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// ヘッダ
        /// </summary>
        public static readonly byte[] Header =
        {
            0x84, 0x2E, 0x77, 0x02
        };

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// [NotNull] DBタイプ名
        /// </summary>
        /// <exception cref="PropertyNullException">nullがセットされた場合</exception>
        public TypeName TypeName
        {
            get => TypeDesc.TypeName;
            set => TypeDesc.TypeName = value;
        }

        /// <summary>
        /// [NotNull] メモ
        /// </summary>
        /// <exception cref="PropertyNullException">nullがセットされた場合</exception>
        public DatabaseMemo Memo
        {
            get => TypeDesc.Memo;
            set => TypeDesc.Memo = value;
        }

        /// <summary>
        /// データの設定方法
        /// </summary>
        public DBDataSettingType DataSettingType => TypeDesc.DataSettingType;

        /// <summary>
        /// データの設定方法＝指定DBの場合の指定DB種別
        /// </summary>
        /// <exception cref="PropertyAccessException">DataSettingTypeがDesignatedTypeではない場合</exception>
        public DBKind DBKind => TypeDesc.DBKind;

        /// <summary>
        /// データの設定方法＝指定DBの場合の指定DBタイプID
        /// </summary>
        /// <exception cref="PropertyAccessException">DataSettingTypeがDesignatedTypeではない場合</exception>
        public TypeId TypeId => TypeDesc.TypeId;

        /// <summary>
        /// DB項目設定と設定値リスト
        /// </summary>
        public DatabaseItemDescList ItemDescList => TypeDesc.ItemDescList;

        /// <summary>
        /// DBデータ設定と設定値リスト
        /// </summary>
        public DatabaseDataDescList DataDescList => TypeDesc.DataDescList;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private DatabaseTypeDesc TypeDesc { get; } = DatabaseTypeDesc.Factory.CreateForDBType();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DBType()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="settingType">[NotNull] データの設定方法種別</param>
        /// <param name="dbKind">[Nullable] 種別が「データベース参照」の場合の参照DB種別</param>
        /// <param name="typeId">[Nullable] 種別が「データベース参照」の場合のタイプID</param>
        /// <exception cref="ArgumentNullException">
        ///     settingTypeがnullの場合、
        ///     またはsettingType が DesignatedType かつ dbKindまたはtypeIdがnullの場合
        /// </exception>
        public DBType(DBDataSettingType settingType,
            DBKind dbKind = null, TypeId? typeId = null)
        {
            TypeDesc.SetDataSettingType(settingType, dbKind, typeId);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dataDescList">[NotNull][LengthRange(1, 10000)] データ情報リスト</param>
        /// <param name="itemDescList">[NotNull][LengthRange(0, 100)] 項目情報リスト</param>
        /// <exception cref="ArgumentNullException">dataDescList, itemDescList が null の場合</exception>
        /// <exception cref="ArgumentException">
        ///     dataDescList, itemDescList に null 要素が含まれる場合、
        ///     または dataDescList, itemDescList の要素数が指定範囲外の場合
        /// </exception>
        public DBType(DatabaseDataDescList dataDescList,
            DatabaseItemDescList itemDescList)
        {
            if (dataDescList is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(dataDescList)));

            if (itemDescList is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(itemDescList)));

            TypeDesc = DatabaseTypeDesc.Factory.CreateForDBType(dataDescList, itemDescList);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dataDescList">[NotNull] データ情報リスト</param>
        /// <param name="itemDescList">[NotNull] 項目情報リスト</param>
        /// <param name="settingType">[NotNull] データの設定方法種別</param>
        /// <param name="dbKind">[Nullable] 種別が「データベース参照」の場合の参照DB種別</param>
        /// <param name="typeId">[Nullable] 種別が「データベース参照」の場合のタイプID</param>
        /// <exception cref="ArgumentNullException">
        ///     dataDescList, itemDescList, settingTypeがnullの場合、
        ///     またはsettingType が DesignatedType かつ dbKindまたはtypeIdがnullの場合
        /// </exception>
        /// <exception cref="ArgumentException">dataDescList, itemDescList に null 要素が含まれる場合</exception>
        public DBType(DatabaseDataDescList dataDescList,
            DatabaseItemDescList itemDescList,
            DBDataSettingType settingType, DBKind dbKind = null, TypeId? typeId = null) : this(dataDescList,
            itemDescList)
        {
            TypeDesc.SetDataSettingType(settingType, dbKind, typeId);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// データの設定方法をセットする。
        /// </summary>
        /// <param name="settingType">[NotNull] データの設定方法種別</param>
        /// <param name="dbKind">[Nullable] 種別が「データベース参照」の場合の参照DB種別</param>
        /// <param name="typeId">[Nullable] 種別が「データベース参照」の場合のタイプID</param>
        /// <exception cref="ArgumentNullException">
        ///     settingTypeがnullの場合、
        ///     またはsettingType が DesignatedType かつ dbKindまたはtypeIdがnullの場合
        /// </exception>
        public void SetDataSettingType(DBDataSettingType settingType,
            DBKind dbKind = null, TypeId? typeId = null)
        {
            TypeDesc.SetDataSettingType(settingType, dbKind, typeId);
        }

        /// <summary>
        /// データの設定方法をセットする。
        /// </summary>
        /// <param name="setting">[NotNull] DBデータ設定</param>
        /// <exception cref="ArgumentNullException">
        ///     settingがnullの場合
        /// </exception>
        public void SetDataSettingType(DBDataSetting setting)
        {
            if (setting is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNullInList(nameof(setting)));

            if (setting.DataSettingType == DBDataSettingType.DesignatedType)
            {
                TypeDesc.SetDataSettingType(setting.DataSettingType, setting.DBKind, setting.TypeId);
            }
            else
            {
                TypeDesc.SetDataSettingType(setting.DataSettingType);
            }
        }

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public bool Equals(DBType other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return TypeDesc.Equals(other.TypeDesc);
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

            // ヘッダ
            result.AddRange(Header);

            // 要素
            result.AddRange(TypeDesc.ToBinaryForDBType());

            return result.ToArray();
        }
    }
}