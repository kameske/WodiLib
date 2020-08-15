// ========================================
// Project Name : WodiLib
// File Name    : DBDataSetting.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using WodiLib.Sys;

namespace WodiLib.Database
{
    /// <summary>
    /// DBデータ設定
    /// </summary>
    [Serializable]
    public class DBDataSetting : ModelBase<DBDataSetting>, ISerializable
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>ファイルヘッダ</summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
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

        private DBDataSettingType dataSettingType = DBDataSettingType.Manual;

        /// <summary>
        /// データの設定方法
        /// </summary>
        public DBDataSettingType DataSettingType
        {
            get => dataSettingType;
            private set
            {
                dataSettingType = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// データの設定方法＝指定DBの場合の指定DB種別
        /// </summary>
        /// <exception cref="PropertyAccessException">DataSettingTypeがDesignatedTypeではない場合</exception>
        [Obsolete("このプロパティは Ver 2.6 で廃止します。代わりに ReferDatabaseDesc プロパティを参照してください。")]
        public DBKind DBKind
        {
            get
            {
                if (DataSettingType != DBDataSettingType.DesignatedType)
                    throw new PropertyAccessException(PropertyErrorMessage);

                return ReferDatabaseDesc.DBKind;
            }
        }

        /// <summary>
        /// データの設定方法＝指定DBの場合の指定DBタイプID
        /// </summary>
        /// <exception cref="PropertyAccessException">DataSettingTypeがDesignatedTypeではない場合</exception>
        [Obsolete("このプロパティは Ver 2.6 で廃止します。代わりに ReferDatabaseDesc プロパティを参照してください。")]
        public TypeId TypeId
        {
            get
            {
                if (DataSettingType != DBDataSettingType.DesignatedType)
                    throw new PropertyAccessException(PropertyErrorMessage);

                return ReferDatabaseDesc.TypeId;
            }
        }

        private DataIdSpecificationDesc? referDatabaseDesc = null;

        /// <summary>
        /// データの設定方法＝指定DBの場合の指定DB情報
        /// </summary>
        /// <exception cref="PropertyAccessException">DataSettingTypeがDesignatedTypeではない場合</exception>
        public DataIdSpecificationDesc ReferDatabaseDesc
        {
            get
            {
                if (DataSettingType != DBDataSettingType.DesignatedType)
                    throw new PropertyAccessException(PropertyErrorMessage);
                return referDatabaseDesc!;
            }
        }

        private void SetReferDatabaseDesc(DataIdSpecificationDesc? value)
        {
            referDatabaseDesc = value;
            NotifyPropertyChanged(nameof(ReferDatabaseDesc));
#pragma warning disable 618 // TODO: Ver 2.6 までの暫定処置
            NotifyPropertyChanged(nameof(DBKind));
            NotifyPropertyChanged(nameof(TypeId));
#pragma warning restore 618
        }

        private DBItemValuesList settingValuesList = new DBItemValuesList();

        /// <summary>項目設定値リスト</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public DBItemValuesList SettingValuesList
        {
            get => settingValuesList;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(SettingValuesList)));

                settingValuesList = value;
                NotifyPropertyChanged();
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
        /// <param name="settingType">データの設定方法種別</param>
        /// <param name="dbKind">種別が「データベース参照」の場合の参照DB種別</param>
        /// <param name="typeId">種別が「データベース参照」の場合のタイプID</param>
        /// <exception cref="ArgumentNullException">
        ///     settingTypeがnullの場合、
        ///     またはsettingType が DesignatedType かつ dbKindがnullの場合
        /// </exception>
        [Obsolete("このコンストラクタは Ver 2.6 で廃止します。" +
                  "代わりに DBDataSetting(DBDataSettingType, DataIdSpecificationDesc) コンストラクタを使用してください。" +
                  "第2,第3引数を省略している場合はこの警告を無視して構いません。（Ver 2.6で警告が消えます）")]
        public DBDataSetting(DBDataSettingType settingType,
            DBKind? dbKind = null, TypeId? typeId = null)
        {
            SetDataSettingType(settingType, dbKind, typeId);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="settingType">データの設定方法種別</param>
        /// <param name="referDatabaseDesc">種別が「データベース参照」の場合の参照DB情報</param>
        /// <exception cref="ArgumentNullException">
        ///     settingTypeがnullの場合、
        ///     またはsettingType が DesignatedType かつ referDatabaseDescがnullの場合
        /// </exception>
        public DBDataSetting(DBDataSettingType settingType, DataIdSpecificationDesc? referDatabaseDesc)
        {
            SetDataSettingType(settingType, referDatabaseDesc);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// データの設定方法をセットする。
        /// </summary>
        /// <param name="settingType">データの設定方法種別</param>
        /// <param name="dbKind">種別が「データベース参照」の場合の参照DB種別</param>
        /// <param name="typeId">種別が「データベース参照」の場合のタイプID</param>
        /// <exception cref="ArgumentNullException">
        ///     settingTypeがnullの場合、
        ///     またはsettingType が DesignatedType かつ dbKindまたはtypeIdがnullの場合
        /// </exception>
        [Obsolete("このメソッドは Ver 2.6 で廃止します。" +
                  "代わりに SetDataSettingType(DBDataSettingType, ReferDatabaseDesc) メソッドを使用してください。" +
                  "第2,第3引数を省略している場合はこの警告を無視して構いません。（Ver 2.6で警告が消えます）")]
        public void SetDataSettingType(DBDataSettingType settingType,
            DBKind? dbKind = null, [NotNullIfNotNull("dbKind")] TypeId? typeId = null)
        {
            if (settingType is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(settingType)));

            if (settingType != DBDataSettingType.DesignatedType)
            {
                DataSettingType = settingType;
                SetReferDatabaseDesc(null);
                return;
            }

            // 設定種別＝指定DBの指定タイプ の場合、DB種別とタイプID必須なのでチェック
            if (dbKind is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(dbKind)));
            if (typeId is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(typeId)));

            DataSettingType = settingType;
            SetReferDatabaseDesc(new DataIdSpecificationDesc(dbKind, typeId.Value));
        }

        /// <summary>
        /// データの設定方法をセットする。
        /// </summary>
        /// <param name="settingType">データの設定方法種別</param>
        /// <param name="referDatabaseDesc">種別が「データベース参照」の場合の参照DB情報</param>
        /// <exception cref="ArgumentNullException">
        ///     settingTypeがnullの場合、
        ///     またはsettingType が DesignatedType かつ referDatabaseDescがnullの場合
        /// </exception>
        public void SetDataSettingType(DBDataSettingType settingType,
            DataIdSpecificationDesc? referDatabaseDesc /* = null */) // TODO: Ver 2.6 以降 referDatabaseDesc のデフォルト値を null とする
        {
            if (settingType is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(settingType)));

            if (settingType != DBDataSettingType.DesignatedType)
            {
                // 設定種別≠指定DBの指定タイプ の場合、DB種別とタイプIDは無視して設定を上書きするだけ
                DataSettingType = settingType;
                SetReferDatabaseDesc(null);
                return;
            }

            // 設定種別＝指定DBの指定タイプ の場合、参照情報必須なのでチェック
            if (referDatabaseDesc is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(referDatabaseDesc)));

            DataSettingType = settingType;
            SetReferDatabaseDesc(referDatabaseDesc);
        }

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public override bool Equals(DBDataSetting? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            if (
                !DataSettingType.Equals(other.DataSettingType)
                || !settingValuesList.Equals(other.settingValuesList)
            ) return false;

            if (DataSettingType == DBDataSettingType.DesignatedType)
            {
                // データの設定方法 ＝指定DB の場合のみ、DB参照情報の比較
                if (
                    !EquatableCompareHelper.Equals(ReferDatabaseDesc, other.ReferDatabaseDesc)
                ) return false;
            }

            return true;
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
                var typeCode = ReferDatabaseDesc.ToTypeCode();
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
            info.AddValue(nameof(DataSettingType), DataSettingType.Code);
            info.AddValue(nameof(referDatabaseDesc), referDatabaseDesc);
            info.AddValue(nameof(settingValuesList), settingValuesList);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="info">デシリアライズ情報</param>
        /// <param name="context">コンテキスト</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected DBDataSetting(SerializationInfo info, StreamingContext context)
        {
            DataSettingType = DBDataSettingType.FromCode(info.GetInt32(nameof(DataSettingType)));
            referDatabaseDesc = info.GetValue<DataIdSpecificationDesc?>(nameof(referDatabaseDesc));
            settingValuesList = info.GetValue<DBItemValuesList>(nameof(settingValuesList));
        }
    }
}
