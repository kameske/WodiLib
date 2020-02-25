// ========================================
// Project Name : WodiLib
// File Name    : DBData.cs
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
    /// DBデータ（XXX.dbdata）
    /// </summary>
    [Serializable]
    public class DBData : IEquatable<DBData>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// ファイルヘッダ
        /// </summary>
        public static readonly byte[] Header =
        {
            0x40, 0x78, 0xA1, 0x02,
        };

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>データリスト</summary>
        public DatabaseDataDescList DataDescList => TypeDesc.DataDescList;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private DatabaseTypeDesc TypeDesc { get; } = DatabaseTypeDesc.Factory.CreateForDBData();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DBData()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dataDescList">[NotNull] 初期データ情報リスト</param>
        /// <exception cref="ArgumentNullException">dataDescList が null の場合</exception>
        public DBData(DatabaseDataDescList dataDescList)
        {
            if (dataDescList is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(dataDescList)));

            TypeDesc = DatabaseTypeDesc.Factory.CreateForDBData(dataDescList);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public bool Equals(DBData other)
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

            // データ数
            result.AddRange(DataDescList.Count.ToWoditorIntBytes());

            // データ
            foreach (var dataDesc in DataDescList)
            {
                result.AddRange(dataDesc.ToBinaryForDBData());
            }

            return result.ToArray();
        }
    }
}