// ========================================
// Project Name : WodiLib
// File Name    : DBDataSetting.cs
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
    /// DBデータ設定
    /// </summary>
    [Serializable]
    public class DBDataSetting
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>ファイルヘッダ</summary>
        public static readonly byte[] Header =
        {
            0xFE, 0xFF, 0xFF, 0xFF
        };

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>プロパティアクセス時のエラーメッセージ</summary>
        private readonly string PropertyErrorMessage =
            $"{nameof(DBDataSettingType)}が{nameof(DBDataSettingType.DesignatedType)}ではないため取得できません。";

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// データの設定方法
        /// </summary>
        public DBDataSettingType DataSettingType { get; private set; } = DBDataSettingType.Manual;

        private DBKind dbKind = DBKind.System;

        /// <summary>
        /// データの設定方法＝指定DBの場合の指定DB種別
        /// </summary>
        /// <exception cref="PropertyAccessException">DataSettingTypeがDesignatedTypeではない場合</exception>
        public DBKind DBKind
        {
            get
            {
                if (DataSettingType != DBDataSettingType.DesignatedType)
                    throw new PropertyAccessException(PropertyErrorMessage);

                return dbKind;
            }
        }

        private TypeId typeId = 0;

        /// <summary>
        /// データの設定方法＝指定DBの場合の指定DBタイプID
        /// </summary>
        /// <exception cref="PropertyAccessException">DataSettingTypeがDesignatedTypeではない場合</exception>
        public TypeId TypeId
        {
            get
            {
                if (DataSettingType != DBDataSettingType.DesignatedType)
                    throw new PropertyAccessException(PropertyErrorMessage);

                return typeId;
            }
        }

        private DBItemValuesList settingValuesList = new DBItemValuesList();

        /// <summary>[NotNull]項目設定値リスト</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public DBItemValuesList SettingValuesList
        {
            get => settingValuesList;
            set
            {
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(SettingValuesList)));

                settingValuesList = value;
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DBDataSetting()
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
        ///     またはsettingType が DesignatedType かつ dbKindがnullの場合
        /// </exception>
        public DBDataSetting(DBDataSettingType settingType, DBKind dbKind = null, TypeId? typeId = null)
        {
            SetDataSettingType(settingType, dbKind, typeId);
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
        public void SetDataSettingType(DBDataSettingType settingType, DBKind dbKind = null, TypeId? typeId = null)
        {
            if (settingType == null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(settingType)));

            if (settingType != DBDataSettingType.DesignatedType)
            {
                // 設定種別≠指定DBの指定タイプ の場合、DB種別とタイプIDは無視して設定を上書きするだけ
                DataSettingType = settingType;
                return;
            }

            // 設定種別＝指定DBの指定タイプ の場合、DB種別とタイプID必須なのでチェック
            if (dbKind == null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(dbKind)));
            if (typeId == null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(typeId)));

            DataSettingType = settingType;
            this.dbKind = dbKind;
            this.typeId = typeId.Value;
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

            // データIDの指定方法
            if (DataSettingType == DBDataSettingType.DesignatedType)
            {
                // 指定方法＝指定DBの指定タイプ の場合、DBタイプによる値 + タイプID
                var typeCode = DBKind.DBDataSettingTypeCode * 10000 + TypeId;
                result.AddRange(typeCode.ToBytes(Endian.Woditor));
            }
            else
            {
                // 指定方法≠指定DBの指定タイプ の場合、指定方法種別コードのみ
                result.AddRange(DataSettingType.Code.ToBytes(Endian.Woditor));
            }

            // 項目数 + 設定種別 & 種別順列
            result.AddRange(SettingValuesList.ToBinarySettingList());

            // データ数 + データ設定値
            result.AddRange(SettingValuesList.ToBinaryDataValues());

            return result.ToArray();
        }
    }
}