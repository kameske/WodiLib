// ========================================
// Project Name : WodiLib
// File Name    : DBItemSettingList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using WodiLib.Sys;

namespace WodiLib.Database
{
    /// <summary>
    /// DB項目設定リスト
    /// </summary>
    [Serializable]
    public class DBItemSettingList : RestrictedCapacityCollection<DBItemSetting>, IReadOnlyDBItemSettingList
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>最大容量</summary>
        public static int MaxCapacity => 100;

        /// <summary>最小容量</summary>
        public static int MinCapacity => 0;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>項目名長さ</summary>
        private static int ItemSpecialTypeLength => 100;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DBItemSettingList()
        {
            Clear();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="items">初期DB項目設定リスト</param>
        /// <exception cref="ArgumentNullException">
        ///     itemsがnullの場合、
        ///     またはitems中にnullが含まれる場合
        /// </exception>
        /// <exception cref="InvalidOperationException">itemsの要素数が不適切な場合</exception>
        public DBItemSettingList(IEnumerable<DBItemSetting> items) : base(items)
        {
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Override Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        /// <summary>
        /// 容量最大値を返す。
        /// </summary>
        /// <returns>容量最大値</returns>
        public override int GetMaxCapacity() => MaxCapacity;

        /// <inheritdoc />
        /// <summary>
        /// 容量最小値を返す。
        /// </summary>
        /// <returns>容量最小値</returns>
        public override int GetMinCapacity() => MinCapacity;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Override Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        /// <summary>
        /// 格納対象のデフォルトインスタンスを生成する。
        /// </summary>
        /// <param name="index">挿入インデックス</param>
        /// <returns>デフォルトインスタンス</returns>
        protected override DBItemSetting MakeDefaultItem(int index) => new DBItemSetting();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Common
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 項目名をバイナリ変換する。
        /// </summary>
        /// <returns>バイナリデータ</returns>
        public byte[] ToBinaryInItemName()
        {
            var result = new List<byte>();

            // 項目数
            result.AddRange(Count.ToBytes(Endian.Woditor));

            // 項目名
            var nameList = Items.Select(x => x.ItemName);
            foreach (var name in nameList)
            {
                result.AddRange(name.ToWoditorStringBytes());
            }

            return result.ToArray();
        }

        /// <summary>
        /// 特殊指定情報をバイナリ変換する。
        /// </summary>
        /// <returns>バイナリデータ</returns>
        public byte[] ToBinaryInSpecialSettingDesc()
        {
            var result = new List<byte>();

            // 特殊指定数
            result.AddRange(ItemSpecialTypeLength.ToBytes(Endian.Woditor));

            // 特殊指定
            var settingTypeList = Items.Select(x => x.SpecialSettingDesc.SettingType);
            result.AddRange(settingTypeList.Select(valueType => valueType.Code));
            // 足りない分を「特殊な指定方法を使用しない」で埋める
            for (var i = Count; i < ItemSpecialTypeLength; i++)
            {
                result.Add(DBItemSpecialSettingType.Normal.Code);
            }

            // ---------- 項目メモ、特殊指定文字列パラメータ、特殊指定す内パラメータ、初期値

            var itemMemos = new List<ItemMemo>();
            var specialCaseDescriptions = new List<IReadOnlyList<DatabaseValueCaseDescription>>();
            var specialCaseNumbers = new List<IReadOnlyList<DatabaseValueCaseNumber>>();
            var initValues = new List<DBItemValue>();

            var useDataList = Items.Select(x => x.SpecialSettingDesc);
            foreach (var data in useDataList)
            {
                itemMemos.Add(data.ItemMemo);
                specialCaseDescriptions.Add(data.GetAllSpecialCaseDescription());
                specialCaseNumbers.Add((data.GetAllSpecialCaseNumber()));
                initValues.Add(data.InitValue);
            }

            // 項目メモ数
            result.AddRange(itemMemos.Count.ToBytes(Endian.Woditor));

            // 項目メモ
            itemMemos.ForEach(x =>
                result.AddRange(x.ToWoditorStringBytes()));

            // 特殊指定文字列パラメータ数
            result.AddRange(specialCaseDescriptions.Count.ToBytes(Endian.Woditor));

            // 特殊指定文字列パラメータ
            specialCaseDescriptions.ForEach(x =>
            {
                // 文字列パラメータ数
                result.AddRange(x.Count.ToBytes(Endian.Woditor));
                // 文字列パラメータ
                x.ForEach((y, _) =>
                    result.AddRange(y.ToWoditorStringBytes()));
            });

            // 特殊指定数値パラメータ数
            result.AddRange(specialCaseNumbers.Count.ToBytes(Endian.Woditor));

            // 特殊指定数値パラメータ
            specialCaseNumbers.ForEach(x =>
            {
                // 数値パラメータ数
                result.AddRange(x.Count.ToBytes(Endian.Woditor));
                // 数値パラメータ
                x.ForEach((y, _) =>
                    result.AddRange(y.ToBytes(Endian.Woditor)));
            });

            // 初期値数
            result.AddRange(initValues.Count.ToBytes(Endian.Woditor));

            // 初期値
            initValues.ForEach(x =>
                result.AddRange(x.ToBinary()));

            return result.ToArray();
        }

        /// <summary>
        /// 項目数 + 設定種別 &amp; 種別順列 をバイナリ変換する。
        /// </summary>
        /// <returns>バイナリデータ</returns>
        public byte[] ToBinaryItemTypeList()
        {
            var result = new List<byte>();

            var itemTypeList = Items.Select(x => x.ItemType).ToList();

            // 項目数
            result.AddRange(itemTypeList.Count.ToWoditorIntBytes());

            // 設定種別 + 種別順列
            var cntDict = new Dictionary<DBItemType, int>
            {
                {DBItemType.Int, 0},
                {DBItemType.String, 0}
            };

            foreach (var itemType in itemTypeList)
            {
                var addValue = itemType.TypeOrderStart + cntDict[itemType];
                result.AddRange(addValue.ToBytes(Endian.Woditor));

                cntDict[itemType] += 1;
            }

            return result.ToArray();
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Serializable
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="info">デシリアライズ情報</param>
        /// <param name="context">コンテキスト</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected DBItemSettingList(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}