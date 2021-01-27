// ========================================
// Project Name : WodiLib
// File Name    : DatabaseDataDesc.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Linq;
using WodiLib.Sys;

namespace WodiLib.Database
{
    /// <summary>
    /// DBデータ情報クラス
    /// </summary>
    [Serializable]
    public class DatabaseDataDesc : ModelBase<DatabaseDataDesc>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private DataName dataName = "";

        /// <summary>
        /// データ名
        /// </summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public DataName DataName
        {
            get => dataName;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(DataName)));

                dataName = value;
                NotifyPropertyChanged();
            }
        }

        private DBItemValueList itemValueList = new DBItemValueList();

        /// <summary>
        /// DB項目値リスト
        /// </summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public DBItemValueList ItemValueList
        {
            get => itemValueList;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(ItemValueList)));
                itemValueList = value;
                NotifyPropertyChanged();
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DatabaseDataDesc()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dataName">データ名</param>
        /// <param name="itemValueList">項目値リスト</param>
        /// <exception cref="ArgumentNullException">dataName, itemValuesがnullの場合</exception>
        public DatabaseDataDesc(DataName dataName, IEnumerable<DBItemValue> itemValueList)
        {
            if (dataName is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(dataName)));

            if (itemValueList is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(itemValueList)));

            DataName = dataName;
            ItemValueList = new DBItemValueList(itemValueList);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public override bool ItemEquals(DatabaseDataDesc? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return dataName == other.dataName
                   && itemValueList.Equals(other.itemValueList);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Common
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// DBData用にバイナリ変換する。
        /// </summary>
        /// <returns>バイナリデータ</returns>
        public byte[] ToBinaryForDBData()
        {
            var result = new List<byte>();

            // データ名
            result.AddRange(DataName.ToWoditorStringBytes());

            // 数値項目と文字列項目を分ける
            var numValues = ItemValueList.Where(x => x.Type == DBItemType.Int)
                .Select(x => (int) x.IntValue).ToList();
            var strValues = ItemValueList.Where(x => x.Type == DBItemType.String)
                .Select(x => (string) x.StringValue).ToList();

            // 数値項目数
            result.AddRange(numValues.Count.ToWoditorIntBytes());

            // 数値項目
            numValues.ForEach(x => result.AddRange(x.ToWoditorIntBytes()));

            // 文字列項目数
            result.AddRange(strValues.Count.ToWoditorIntBytes());

            // 文字列項目
            strValues.ForEach(x => result.AddRange(new WoditorString(x).StringByte));

            return result.ToArray();
        }
    }
}
