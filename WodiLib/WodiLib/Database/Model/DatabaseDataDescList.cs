// ========================================
// Project Name : WodiLib
// File Name    : DatabaseDataDescList.cs
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
    /// DBデータ情報リスト
    /// </summary>
    [Serializable]
    public class DatabaseDataDescList : RestrictedCapacityCollection<DatabaseDataDesc>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>最大容量</summary>
        public static int MaxCapacity => DBItemValuesList.MaxCapacity;

        /// <summary>最小容量</summary>
        public static int MinCapacity => DBItemValuesList.MinCapacity;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DatabaseDataDescList()
        {
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
        public DatabaseDataDescList(IEnumerable<DatabaseDataDesc> items) : base(items)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dataNameList">データ名リスト</param>
        /// <param name="valuesList">値リスト</param>
        /// <exception cref="ArgumentNullException">dataNameList, valuesList が null の場合</exception>
        /// <exception cref="ArgumentException">dataNameListとvaluesListの要素数が異なる場合</exception>
        internal DatabaseDataDescList(DataNameList dataNameList,
            DBItemValuesList valuesList)
            : base(DatabaseDataDescCreator.CreateEnumerableDatabaseDataDesc(dataNameList, valuesList))
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
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 自分自身の型情報に適合するDatabaseDataDescインスタンスを生成する。
        /// </summary>
        /// <returns></returns>
        public DatabaseDataDesc CreateMatchItemInstance()
            => new DatabaseDataDesc
            {
                ItemValueList = this[0].ItemValueList.CreateDefaultValueListInstance()
            };

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Override Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        /// <summary>
        /// SetItem(int, T) 実行直前に呼び出される処理
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <param name="item">要素</param>
        protected override void PreSetItem(int index, DatabaseDataDesc item)
        {
            if (Count <= 1) return;

            if (!DBItemValueListItemTypeCompareHelper.Compare(this[0], item))
                throw new InvalidOperationException(
                    ErrorMessage.NotEqual($"{nameof(DatabaseDataDescList)}の値型情報", "セットしようとした要素の値型情報"));
        }

        /// <inheritdoc />
        /// <summary>
        /// InsertItem(int, T) 実行直前に呼び出される処理
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <param name="item">要素</param>
        protected override void PreInsertItem(int index, DatabaseDataDesc item)
        {
            if (Count <= 1) return;

            if (!DBItemValueListItemTypeCompareHelper.Compare(this[0], item))
                throw new InvalidOperationException(
                    ErrorMessage.NotEqual($"{nameof(DatabaseDataDescList)}の値型情報", "追加しようとした要素の値型情報"));
        }

        /// <inheritdoc />
        /// <summary>
        /// 格納対象のデフォルトインスタンスを生成する。
        /// </summary>
        /// <param name="index">挿入インデックス</param>
        /// <returns>デフォルトインスタンス</returns>
        protected override DatabaseDataDesc MakeDefaultItem(int index)
            => Count > 0
                ? CreateMatchItemInstance()
                : new DatabaseDataDesc();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Serializable
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="info">デシリアライズ情報</param>
        /// <param name="context">コンテキスト</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected DatabaseDataDescList(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
