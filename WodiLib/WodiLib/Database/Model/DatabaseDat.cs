// ========================================
// Project Name : WodiLib
// File Name    : DatabaseDat.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using WodiLib.Sys;

namespace WodiLib.Database
{
    /// <summary>
    /// DBデータ（XXXDatabase.dat）
    /// </summary>
    [Serializable]
    public class DatabaseDat : IEquatable<DatabaseDat>, ISerializable
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// ファイルヘッダ
        /// </summary>
        public static readonly byte[] Header =
        {
            0x00, 0x57, 0x00, 0x00, 0x4F, 0x4C, 0x00, 0x46, 0x4D, 0x00, 0xC1
        };

        /// <summary>
        /// ファイルフッタ
        /// </summary>
        public static readonly byte[] Footer =
        {
            0xC1
        };

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>[Nullable] DB種別</summary>
        public DBKind DBKind { get; set; }

        private DBDataSettingList settingList = new DBDataSettingList();

        /// <summary>
        /// [NotNull] タイプ設定リスト
        /// </summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public DBDataSettingList SettingList
        {
            get => settingList;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(SettingList)));
                settingList = value;
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private bool IsNullDBKind => DBKind is null;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DatabaseDat()
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
        public bool Equals(DatabaseDat other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return settingList.Equals(other.settingList)
                   && DBKind == other.DBKind;
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

            // DBデータ設定
            result.AddRange(SettingList.ToBinary());

            // ファイルフッタ
            result.AddRange(Footer);

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
            info.AddValue(nameof(IsNullDBKind), IsNullDBKind);
            if (!IsNullDBKind) info.AddValue(nameof(DBKind), DBKind.Code);
            info.AddValue(nameof(settingList), settingList);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="info">デシリアライズ情報</param>
        /// <param name="context">コンテキスト</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected DatabaseDat(SerializationInfo info, StreamingContext context)
        {
            var isNullDbKind = info.GetBoolean(nameof(IsNullDBKind));
            if (!isNullDbKind) DBKind = DBKind.FromCode(info.GetByte(nameof(DBKind)));
            settingList = info.GetValue<DBDataSettingList>(nameof(settingList));
        }
    }
}