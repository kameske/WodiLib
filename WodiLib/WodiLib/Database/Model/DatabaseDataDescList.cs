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
        /// <param name="list">初期DB項目設定リスト</param>
        /// <exception cref="ArgumentNullException">
        ///     listがnullの場合、
        ///     またはlist中にnullが含まれる場合
        /// </exception>
        /// <exception cref="InvalidOperationException">listの要素数が不適切な場合</exception>
        public DatabaseDataDescList(IReadOnlyCollection<DatabaseDataDesc> list) : base(list)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dataNameList">[NotNull] データ名リスト</param>
        /// <param name="valuesList">[NotNull] 値リスト</param>
        /// <exception cref="ArgumentNullException">dataNameList, valuesList が null の場合</exception>
        /// <exception cref="ArgumentException">dataNameListとvaluesListの要素数が異なる場合</exception>
        internal DatabaseDataDescList(DataNameList dataNameList,
            DBItemValuesList valuesList)
            : base(new Func<IReadOnlyCollection<DatabaseDataDesc>>(() =>
            {
                if (dataNameList is null)
                    throw new ArgumentNullException(
                        ErrorMessage.NotNull(nameof(dataNameList)));
                if (valuesList is null)
                    throw new ArgumentNullException(
                        ErrorMessage.NotNull(nameof(valuesList)));

                if (dataNameList.Count != valuesList.Count)
                    throw new ArgumentException(
                        $"{nameof(dataNameList)}の要素数と{nameof(valuesList)}の要素数が一致しません。");

                var list = new List<DatabaseDataDesc>();

                for (var i = 0; i < dataNameList.Count; i++)
                {
                    list.Add(new DatabaseDataDesc(
                        dataNameList[i],
                        valuesList[i].ToLengthChangeableItemValueList()));
                }

                return list;
            })())
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
        protected override DatabaseDataDesc MakeDefaultItem(int index) => new DatabaseDataDesc();

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