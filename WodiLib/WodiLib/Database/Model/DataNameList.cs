// ========================================
// Project Name : WodiLib
// File Name    : DataNameList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using WodiLib.Sys;
using WodiLib.Sys.Collections;

namespace WodiLib.Database
{
    /// <summary>
    /// DBデータ名リスト
    /// </summary>
    public class DataNameList : RestrictedCapacityList<DataName, DataNameList>, IReadOnlyDataNameList
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>リスト最大数</summary>
        public static int MaxCapacity => DBItemValuesList.MaxCapacity;

        /// <summary>リスト最小数</summary>
        public static int MinCapacity => DBItemValuesList.MinCapacity;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DataNameList()
        {
        }

        /// <summary>
        /// コンストラクタ（初期値指定）
        /// </summary>
        /// <param name="items">初期リスト</param>
        /// <exception cref="ArgumentNullException">itemsがnullの場合</exception>
        /// <exception cref="InvalidOperationException">itemsの要素数がMaxLengthを超える場合</exception>
        public DataNameList(IEnumerable<DataName> items) : base(items)
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
        protected override DataName MakeDefaultItem(int index) => new DataName("");

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

            // データ数
            result.AddRange(Count.ToBytes(Endian.Woditor));

            // データ名
            foreach (var name in this)
            {
                result.AddRange(name.ToWoditorStringBytes());
            }

            return result.ToArray();
        }

        public override bool ItemEquals(DataNameList? other)
        {
            throw new NotImplementedException();
        }

        public bool ItemEquals(IReadOnlyExtendedList<DataName>? other)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyExtendedList<DataName> DeepClone()
        {
            throw new NotImplementedException();
        }
    }
}
